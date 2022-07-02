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
            // NPCs that are on the player's team have a 25% chance to be hit
            // this is where you would also handle any formation based logic
            // else, each player character has an equal chance to be hit by the enemy
            return playerParticipants[random.Next(0, playerParticipants.Count )];
        }


        public static int DetermineAttackDamage(Participant attacker, Participant defender)
        {
            return DetermineAttackDamage(attacker, defender, Random);
        }

        public static int DetermineAttackDamage(Participant attacker, Participant defender, Random random)
        {

            if (random.Next(defender.stats.dodge, 64) <= defender.stats.dodge)
            {
                // attack was dodged
                
                return 0;
            }

            if (random.Next(1, 32) == 1)
            {
                // critical hit

                var criticalHitAttackStrength = attacker.stats.strength + attacker.stats.attackPower;

                var criticalHitDamage = criticalHitAttackStrength / random.Next(1, 2);

                return criticalHitDamage;
            }

            // perform regular attack

            var attackStrength = attacker.stats.strength + attacker.stats.attackPower;

            var targetDefense = defender.stats.agility + defender.stats.defensePower;

            var damage = ((attackStrength - (targetDefense / 2)) / random.Next(2, 4));

            return damage;
        }

        public static bool DetermineParticipantFleeing(Participant participant, List<Participant> runningFrom)
        {
            return DetermineParticipantFleeing(participant, GetParticipantWithHighestAgility(runningFrom), Random);
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
        
        public static int DetermineRemainingParticipants(List<Participant> enemyParticipants) {
            return enemyParticipants.Count(participant => participant.stats.hitPoints > 0);
        }

        public static int DetermineExperiencePoints(List<Participant> defeatedParticipants) 
        {
            int xpSum = 0;
            
            foreach (var defeatedParticipant in defeatedParticipants) {
                xpSum += defeatedParticipant.experiencePoints;
            }
            
            return xpSum;
        }
        
        public static int DetermineGoldPoints(List<Participant> defeatedParticipants)
        {
            return DetermineGoldPoints(defeatedParticipants, Random);
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

        public static List<Item> DetermineItemsDropped(List<Participant> defeatedParticipants)
        {
            return DetermineItemsDropped(defeatedParticipants, Random);
        }

        public static List<Item> DetermineItemsDropped(List<Participant> defeatedParticipants, Random random) 
        {
            List<Item> itemsDropped = new List<Item>();
            
            foreach (var defeatedParticipant in defeatedParticipants) {
                itemsDropped.AddRange(defeatedParticipant.droppableItems
                    .Where(item => random.Next(item.chanceToDrop, 100) <= item.chanceToDrop)
                    .Select(item => item.itemToDrop).ToList());
            }

            return itemsDropped;
        }

    }
    
}

// TODO: remove this class and move this test logic into a sample battle scene
// public class TestBattleSystem
// {
//
//     private List<Participant> _participants;
//
//     private List<Participant> _enemyParticipants;
//
//     private List<Participant> _playerParticipants;
//     
//     public TestBattleSystem() 
//     {
//         _enemyParticipants = BattleSystem.GetEnemyParticipants(_participants);
//         
//         _playerParticipants = BattleSystem.GetPlayerParticipants(_participants);
//     }
//
//     public void Tick() {
//         while (BattleSystem.DetermineRemainingParticipants(_enemyParticipants) > 0 
//                && BattleSystem.DetermineRemainingParticipants(_playerParticipants) > 0) {
//             
//             var orderedParticipants = BattleSystem.DetermineTurnOrder(_participants);
//
//             foreach (var orderedParticipant in orderedParticipants)
//             {
//                 if (orderedParticipant.stats.hitPoints > 0) {
//                     if (ParticipantType.Player.Equals(orderedParticipant.ParticipantType))
//                     {
//                         // prompt player for action
//                     }
//                     else // currently enemies only
//                     {
//                         var action = orderedParticipant.DetermineParticipantAction(BattleSystem.Random);
//
//                         switch (action.ActionType)
//                         {
//                             case ParticipantActionType.Attack:
//                                 Participant attackTarget = BattleSystem.DetermineEnemyTarget(BattleSystem.GetPlayerParticipants(_participants));
//
//                                 var damage = BattleSystem.DetermineAttackDamage(orderedParticipant, attackTarget);
//                                 
//                                 // inflict damage to target
//                                 attackTarget.InflictDamage(damage);
//                                 
//                                 return;
//                              case ParticipantActionType.Item:
//                                 Participant itemTarget = action.DetermineTarget(_participants);
//                                 
//                                 action.item.UseItem(itemTarget);
//                                 
//                                 return;
//                             case ParticipantActionType.Spell:
//                                 Participant spellTarget = action.DetermineTarget(_participants);
//
//                                 action.spell.CastSpell(spellTarget);
//                                 
//                                 return;
//                             case ParticipantActionType.Flee:
//                                 if (BattleSystem.DetermineParticipantFleeing(
//                                     orderedParticipant,
//                                     BattleSystem.GetPlayerParticipants(_participants)
//                                 ))
//                                 {
//                                     // enemy escaped!
//                                 }
//                                 else
//                                 {
//                                     // enemy failed to escape!
//                                 }
//                                 
//                                 return;
//                         }
//                     }
//                 }
//             }
//         }
//
//         if (BattleSystem.DetermineRemainingParticipants(_playerParticipants) > 0) {
//             var experiencePoints = BattleSystem.DetermineExperiencePoints(_enemyParticipants);
//
//             // apply XP points and level up any characters
//             
//             var goldPoints = BattleSystem.DetermineGoldPoints(_enemyParticipants);
//             
//             // reward gold points
//
//             var itemsDropped = BattleSystem.DetermineItemsDropped(_enemyParticipants);
//
//             // reward items to player
//         }
//         
//     }
//     
// }
