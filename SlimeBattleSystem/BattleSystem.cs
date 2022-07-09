using System;
using System.Collections.Generic;
using System.Linq;
using SlimeBattleSystem;
using Random = System.Random;

namespace SlimeBattleSystem
{
    
    // reading material: 
    // https://dragonquestcosmos.fandom.com/wiki/Formulas#Physical_Attack
    // https://www.gamedeveloper.com/design/number-punchers-how-i-final-fantasy-i-and-i-dragon-quest-i-handle-combat-math
    
    public static class BattleSystem
    {
        public static Random Random;

        public static int Seed = 0;
        
        static BattleSystem()
        {
            Random = new Random();
        }

        public static void SetRandomizationSeed(string seed)
        {
            Seed = seed.GetHashCode();
            
            Random = new Random(Seed);
        }

        public static int CalculateTurnOrder(int agility)
        {
            return CalculateTurnOrder(agility, Random);
        }

        public static int CalculateTurnOrder(int agility, Random random)
        {
            // agility - ([0 - 255] * (agility - agility / 4)) / 256

            return agility - (random.Next(0, 255) * (agility - agility / 4)) / 256;
        }

        public static List<Participant> DetermineTurnOrder(List<Participant> participants)
        {
            return DetermineTurnOrder(participants, Random);
        }
        
        public static List<Participant> DetermineTurnOrder(List<Participant> participants, Random random)
        {
            foreach (var participant in participants)
            {
                participant.turnOrder = CalculateTurnOrder(participant.stats.agility, random);
            }
            
            participants.Sort((participantA, participantB) => participantB.turnOrder.CompareTo(participantA.turnOrder));

            return participants;
        }

        public static Participant DetermineEnemyTarget(List<Participant> playerParticipants)
        {
            return DetermineEnemyTarget(playerParticipants, Random);
        }
        
        public static Participant DetermineEnemyTarget(List<Participant> playerParticipants, Random random)
        {
            // currently each player character has an equal chance to be hit by the enemy
            return playerParticipants[random.Next(0, playerParticipants.Count )];
        }

        public static AttackResults DetermineAttackDamage(Participant attacker, Participant defender)
        {
            return DetermineAttackDamage(attacker, defender, Random);
        }

        public static AttackResults DetermineAttackDamage(Participant attacker, Participant defender, Random random)
        {

            if (random.Next(defender.stats.dodge, 64) <= defender.stats.dodge)
            {
                // attack was dodged

                return new AttackResults(AttackResults.AttackType.Missed, 0);
            }

            if (random.Next(1, 32) == 1)
            {
                // critical hit

                var criticalHitAttackStrength = attacker.stats.attackPower;

                var criticalHitDamage = criticalHitAttackStrength / random.Next(1, 2);

                return new AttackResults(AttackResults.AttackType.CriticalHit, criticalHitDamage);
            }

            // perform regular attack

            var attackStrength = attacker.stats.attackPower;

            var targetDefense = defender.stats.defensePower;

            var damage = ((attackStrength - (targetDefense / 2)) / random.Next(2, 4));

            return new AttackResults(AttackResults.AttackType.Hit, damage);
        }

        public static bool DetermineParticipantFleeing(Participant participant, Participant runningFrom)
        {
            return DetermineParticipantFleeing(participant,runningFrom, Random);
        }

        public static bool DetermineParticipantFleeing(Participant participant, List<Participant> runningFrom)
        {
            return DetermineParticipantFleeing(participant, GetParticipantWithHighestAgility(runningFrom), Random);
        }

        public static bool DetermineParticipantFleeing(Participant participant, List<Participant> runningFrom, Random random)
        {
            return DetermineParticipantFleeing(participant, GetParticipantWithHighestAgility(runningFrom), random);
        }

        public static bool DetermineParticipantFleeing(Participant participant, Participant runningFrom, Random random)
        {
            // fleeing formula
            // this currently is missing the RunFactor which monsters have
            // the stronger the monster the higher the RunFactor:
            // https://dragonquestcosmos.fandom.com/wiki/Formulas#Fleeing_Battle 
            // heroAgility * rand(0, 255) >= toughestMonsterAgility * rand(0, 255) * monsterRunFactor

            return participant.stats.agility * random.Next(0, 255) >=
                   runningFrom.stats.agility * random.Next(0, 255);
        }
        
        public static List<Participant> GetPlayerParticipants(List<Participant> participants)
        {
            return participants.Where((participant =>
                participant.ParticipantType == ParticipantType.Player ||
                participant.ParticipantType == ParticipantType.NPC)).ToList();
        }
        
        public static List<Participant> GetEnemyParticipants(List<Participant> participants) 
        {
            return participants.Where(participant =>
                participant.ParticipantType == ParticipantType.Enemy).ToList();
        }
        
        public static Participant GetParticipantWithHighestAgility(List<Participant> participants)
        {
            return participants.OrderByDescending(participant => participant.stats.agility).ToList()[0];
        }
        
        public static int GetNumberOfRemainingParticipants(List<Participant> participants) {
            return participants.Count(participant => participant.stats.hitPoints > 0);
        }

        public static bool IsBattleOver(List<Participant> participants)
        {
            var enemyParticipants = GetEnemyParticipants(participants);
            
            var playerParticipants = GetPlayerParticipants(participants);
            
            return GetNumberOfRemainingParticipants(enemyParticipants) == 0 
                   || GetNumberOfRemainingParticipants(playerParticipants) == 0;
        }

        public static int DetermineExperiencePoints(Participant defeatedParticipant) 
        {
            return DetermineExperiencePoints(new List<Participant>() { defeatedParticipant });
        }

        public static int DetermineExperiencePoints(List<Participant> defeatedParticipants) 
        {
            int xpSum = 0;
            
            foreach (var defeatedParticipant in defeatedParticipants) {
                xpSum += defeatedParticipant.experiencePoints;
            }
            
            return xpSum;
        }
        
        public static int DetermineGoldPoints(Participant defeatedParticipant)
        {
            return DetermineGoldPoints(new List<Participant>() { defeatedParticipant }, Random);
        }

        public static int DetermineGoldPoints(List<Participant> defeatedParticipants)
        {
            return DetermineGoldPoints(defeatedParticipants, Random);
        }
        
        public static int DetermineGoldPoints(Participant defeatedParticipant, Random random)
        {
            return DetermineGoldPoints(new List<Participant>() { defeatedParticipant }, random);
        }
        
        public static int DetermineGoldPoints(List<Participant> defeatedParticipants, Random random) 
        {
            // (GP * RAND(0, 63) + 192) / 256
            
            int gpSum = 0;
            
            foreach (var defeatedParticipant in defeatedParticipants) {
                gpSum += (defeatedParticipant.goldPoints * (random.Next(0, 63) + 192)) / 256;
            }

            return gpSum;
        }

        public static List<T> DetermineItemsDropped<T>(Dictionary<T, int> droppableItems) {
            return DetermineItemsDropped(droppableItems, Random);
        }
        
        public static List<T> DetermineItemsDropped<T>(Dictionary<T, int> droppableItems, Random random) {
            List<T> itemsDropped = new List<T>();
            
            foreach (var droppableItem in droppableItems) {
                if (random.Next(droppableItem.Value, 100) <= droppableItem.Value) {
                    itemsDropped.Add(droppableItem.Key);
                }
            }
            
            return itemsDropped;
        }

    }

    [Serializable]
    public class AttackResults
    {

        public enum AttackType
        {
            Hit,
            CriticalHit,
            Missed
        }

        public AttackType attackType;
        
        public int damage;

        public AttackResults(AttackType attackType, int damage)
        {
            this.attackType = attackType;
            this.damage = damage;
        }
    }
    
}