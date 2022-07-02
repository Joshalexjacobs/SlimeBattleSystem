using System;

namespace SlimeBattleSystem
{

    [Serializable]
    public class Stats
    {
        public Stats()
        {
            hitPoints = 1;

            magicPoints = 1;

            strength = 1;

            agility = 1;

            attackPower = 1;

            defensePower = 1;

            dodge = 1;
        }

        public Stats(Stats stats)
        {
            hitPoints = stats.hitPoints;

            magicPoints = stats.magicPoints;

            strength = stats.strength;

            agility = stats.agility;
            
            attackPower = stats.attackPower;

            defensePower = stats.defensePower;

            dodge = stats.dodge;
        }

        public Stats(int hitPoints, int magicPoints, int strength, int agility, int attackPower, int defensePower, int dodge)
        {
            this.hitPoints = hitPoints;

            this.magicPoints = magicPoints;

            this.strength = strength;

            this.agility = agility;
            
            this.attackPower = attackPower;

            this.defensePower = defensePower;

            this.dodge = dodge;
        }

        public int hitPoints;

        public int magicPoints;

        // player's base strength
        public int strength;

        // player's base agility
        public int agility;

        // weapons/items that contribute to attack
        public int attackPower;
        
        // armor/items that contribute to defense 
        public int defensePower;

        // chance that a participant can dodge an attack
        public int dodge;
    }
    
}
