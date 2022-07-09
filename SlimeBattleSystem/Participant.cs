using System;
using System.Collections.Generic;

namespace SlimeBattleSystem
{
    [Serializable]
    public enum ParticipantActionType
    {
        Attack,
        Item,
        Spell,
        Flee
    }

    [Serializable]
    public class ParticipantAction
    {
        public ParticipantAction()
        {
        }

        public ParticipantAction(ParticipantActionType actionType)
        {
            ActionType = actionType;
        }

        public virtual Participant DetermineTarget(List<Participant> participants)
        {
            return participants[0];
        }
        
        public ParticipantActionType ActionType;
    }

    [Serializable]
    public enum ParticipantType
    {
        Player,
        NPC,
        Enemy
    }

    [Serializable]
    public class Participant
    {
        public Participant()
        {
            stats = new Stats();
        }

        public Participant(string name)
        {
            this.name = name;

            stats = new Stats();
        }

        public Participant(string name, Stats stats)
        {
            this.name = name;

            this.stats = stats;
        }

        // attack power and defense power should be recalculated when
        // the player equips a new weapon or piece of armor
        public void CalculateAttackPower(int weaponAttackPower)
        {
            stats.attackPower = stats.strength + weaponAttackPower;
        }

        public void CalculateDefensePower(int armorDefensePower)
        {
            stats.defensePower = stats.agility + armorDefensePower;
        }

        public virtual ParticipantAction DetermineParticipantAction(Random random)
        {
            // used to determine enemy patterns
            // eg. 50% to attack or cast a spell
            if (random.Next(1, 2) == 1)
            {
                return new ParticipantAction(ParticipantActionType.Attack);    
            }

            return new ParticipantAction();
        }

        public virtual void InflictDamage(int damage)
        {
            stats.hitPoints -= damage;
            
            // did the participant faint?
        }

        public string name;

        public ParticipantType ParticipantType = ParticipantType.Enemy;
        
        public int turnOrder;

        public Stats stats;

        public int experiencePoints;

        public int goldPoints;
    }
    
}
