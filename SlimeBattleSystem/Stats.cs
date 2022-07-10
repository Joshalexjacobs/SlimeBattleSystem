using System;

namespace SlimeBattleSystem {
  /// <summary>
  ///   Stats used to track HP, MP, Strength, Agility, Attack Power, Defense Power, and chance to Dodge (X/64).
  /// </summary>
  [Serializable]
  public class Stats {
    // player's unmodified agility
    public int Agility;

    // weapons/items that contribute to attack
    public int AttackPower;

    // armor/items that contribute to defense 
    public int DefensePower;

    // chance that a participant can dodge an attack out of 64
    public int Dodge;

    // current HP
    public int HitPoints;

    // current level
    public int Level;

    // current MP
    public int MagicPoints;

    // max HP
    public int MaxHitPoints;

    // max MP
    public int MaxMagicPoints;

    // player's unmodified strength
    public int Strength;

    public Stats() {
      HitPoints = 1;

      MaxHitPoints = 1;

      MagicPoints = 1;

      MaxMagicPoints = 1;

      Strength = 1;

      Agility = 1;

      AttackPower = 1;

      DefensePower = 1;

      Dodge = 1;

      Level = 1;
    }

    public Stats(Stats stats) {
      HitPoints = stats.HitPoints;

      MaxHitPoints = stats.MaxHitPoints;

      MagicPoints = stats.MagicPoints;

      MaxMagicPoints = stats.MaxMagicPoints;

      Strength = stats.Strength;

      Agility = stats.Agility;

      AttackPower = stats.AttackPower;

      DefensePower = stats.DefensePower;

      Dodge = stats.Dodge;

      Level = stats.Level;
    }

    public Stats(int hitPoints, int maxHitPoints, int magicPoints, int maxMagicPoints, int strength, int agility,
      int attackPower, int defensePower, int dodge, int level = 1) {
      HitPoints = hitPoints;

      MaxHitPoints = maxHitPoints;

      MagicPoints = magicPoints;

      MaxMagicPoints = maxMagicPoints;

      Strength = strength;

      Agility = agility;

      AttackPower = attackPower;

      DefensePower = defensePower;

      Dodge = dodge;

      Level = level;
    }
  }
}