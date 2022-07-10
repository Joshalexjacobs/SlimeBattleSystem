using System;
using System.Collections.Generic;

namespace SlimeBattleSystem {
  [Serializable]
  public enum ParticipantActionType {
    Attack,
    Item,
    Spell,
    Flee
  }

  /// <summary>
  ///   An action a Participant makes during a battle.
  /// </summary>
  [Serializable]
  public class ParticipantAction {
    public ParticipantActionType ActionType;

    public ParticipantAction() { }

    public ParticipantAction(ParticipantActionType actionType) {
      ActionType = actionType;
    }

    /// <summary>
    ///   Determines the target the participant is attacking, casting a spell on, or using an item on.
    /// </summary>
    /// <param name="participants">A list of Participants.</param>
    /// <returns>Participant</returns>
    public virtual Participant DetermineTarget(List<Participant> participants) {
      return participants[0];
    }
  }

  [Serializable]
  public enum ParticipantType {
    Player,
    Npc,
    Enemy
  }

  /// <summary>
  ///   An enemy, NPC, or player character that participates in battle.
  /// </summary>
  [Serializable]
  public class Participant {
    public int ExperiencePoints;

    public int GoldPoints;

    public string Name;

    public ParticipantType ParticipantType = ParticipantType.Enemy;

    public Stats Stats;

    public int TurnOrder;

    public Participant() {
      Stats = new Stats();
    }

    public Participant(string name) {
      Name = name;

      Stats = new Stats();
    }

    public Participant(string name, Stats stats) {
      Name = name;

      Stats = stats;
    }

    /// <summary>
    ///   Recalculates participants attack power. Should occur after a new weapon is equipped.
    /// </summary>
    /// <param name="weaponAttackPower">The attack power stat of the newly equipped weapon.</param>
    /// <returns>void</returns>
    public void CalculateAttackPower(int weaponAttackPower) {
      Stats.AttackPower = Stats.Strength + weaponAttackPower;
    }

    /// <summary>
    ///   Recalculates participants defense power. Should occur after a new piece of armor is equipped.
    /// </summary>
    /// <param name="armorDefensePower">The defense power stat of the newly equipped piece of armor.</param>
    /// <returns>void</returns>
    public void CalculateDefensePower(int armorDefensePower) {
      Stats.DefensePower = Stats.Agility + armorDefensePower;
    }

    /// <summary>
    ///   Determines the participant's action during their turn.
    /// </summary>
    /// <param name="random">Random class to use in generating turn order.</param>
    /// <returns>ParticipantAction</returns>
    public virtual ParticipantAction DetermineParticipantAction(Random random) {
      // used to determine enemy patterns
      // eg. 50% to attack or cast a spell

      return new ParticipantAction(ParticipantActionType.Attack);
    }

    /// <summary>
    ///   Inflicts allotted damage to the participant. Will floor hit points to 0.
    /// </summary>
    /// <param name="damage">Amount of damage inflicted.</param>
    /// <returns>void</returns>
    public virtual void InflictDamage(int damage) {
      Stats.HitPoints -= damage;

      if (Stats.HitPoints < 0) Stats.HitPoints = 0;
    }
  }
}