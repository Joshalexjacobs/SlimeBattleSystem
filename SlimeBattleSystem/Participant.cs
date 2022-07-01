using System;
using System.Collections.Generic;

namespace SlimeBattleSystem
{
    public enum ParticipantActionType
    {
        Attack,
        Item,
        Spell,
        Flee
    }

    public class ParticipantAction
    {
        public ParticipantAction()
        {
        }

        public ParticipantAction(ParticipantActionType actionType)
        {
            ActionType = actionType;
        }

        public ParticipantAction(ParticipantActionType actionType, Item item)
        {
            ActionType = actionType;
            
            this.item = item;
        }

        public ParticipantAction(ParticipantActionType actionType, Spell spell)
        {
            ActionType = actionType;
            
            this.spell = spell;
        }

        public virtual Participant DetermineTarget(List<Participant> participants)
        {
            return participants[0];
        }
        
        public ParticipantActionType ActionType;

        public Item item;

        public Spell spell;
    }

    public enum ParticipantType
    {
        Player,
        NPC,
        Enemy
    }

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
        public void CalculateAttackPower()
        {
            stats.attackPower = weapon.attackPower;
        }

        public void CalculateDefensePower()
        {
            stats.defensePower = armor.defensePower + shield.defensePower;
        }

        public virtual ParticipantAction DetermineParticipantAction(Random random)
        {
            // used to determine enemy patterns
            // eg. 50% to attack or cast a spell
            if (random.Next(1, 2) == 1)
            {
                return new ParticipantAction(ParticipantActionType.Attack);    
            }

            return new ParticipantAction(ParticipantActionType.Spell, new Spell());
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

        public Weapon weapon;

        public Armor armor;

        public Armor shield;

        public List<Spell> spells;

        public int experiencePoints;

        public int goldPoints;

        public List<DroppableItem> droppableItems;
    }
    
}
