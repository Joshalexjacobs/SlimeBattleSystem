using System;
using System.Collections.Generic;
using System.Linq;

namespace SlimeBattleSystem {
  // reading material: 
  // https://dragonquestcosmos.fandom.com/wiki/Formulas#Physical_Attack
  // https://www.gamedeveloper.com/design/number-punchers-how-i-final-fantasy-i-and-i-dragon-quest-i-handle-combat-math

  /// <summary>
  ///   An easy to use RPG combat system for Unity that utilizes formulas from the original Dragon Quest.
  /// </summary>
  public static class BattleSystem {
    public static Random Random;

    public static int Seed;

    static BattleSystem() {
      Random = new Random();
    }

    /// <summary>
    ///   Sets the randomization seed used by most formulas.
    /// </summary>
    /// <param name="seed">String used to seed randomization.</param>
    /// <returns>void</returns>
    public static void SetRandomizationSeed(string seed) {
      Seed = seed.GetHashCode();

      Random = new Random(Seed);
    }

    public static int CalculateTurnOrder(int agility) {
      return CalculateTurnOrder(agility, Random);
    }

    /// <summary>
    ///   Calculates the turn order given a Participant's Agility stat.
    /// </summary>
    /// <param name="agility">Agility stat.</param>
    /// <param name="random">Random class to use in generating turn order.</param>
    /// <returns>int</returns>
    public static int CalculateTurnOrder(int agility, Random random) {
      // agility - ([0 - 255] * (agility - agility / 4)) / 256

      return agility - random.Next(0, 255) * (agility - agility / 4) / 256;
    }

    public static List<Participant> DetermineTurnOrder(List<Participant> participants) {
      return DetermineTurnOrder(participants, Random);
    }

    /// <summary>
    ///   Determines the order a group of Participants will attack in.
    /// </summary>
    /// <param name="participants">A list of battle participants.</param>
    /// <param name="random">Random class to use in generating turn order.</param>
    /// <returns>List<Participant></returns>
    public static List<Participant> DetermineTurnOrder(List<Participant> participants, Random random) {
      foreach (var participant in participants)
        participant.TurnOrder = CalculateTurnOrder(participant.Stats.Agility, random);

      participants.Sort((participantA, participantB) => participantB.TurnOrder.CompareTo(participantA.TurnOrder));

      return participants;
    }

    public static Participant DetermineEnemyTarget(List<Participant> playerParticipants) {
      return DetermineEnemyTarget(playerParticipants, Random);
    }

    /// <summary>
    ///   Determines the player participant and enemy will target.
    /// </summary>
    /// <param name="playerParticipants">A list of player participants.</param>
    /// <param name="random">Random class to use in generating turn order.</param>
    /// <returns>Participant</returns>
    public static Participant DetermineEnemyTarget(List<Participant> playerParticipants, Random random) {
      // currently each player character has an equal chance to be hit by the enemy
      return playerParticipants[random.Next(0, playerParticipants.Count)];
    }

    public static AttackResults DetermineAttackDamage(Participant attacker, Participant defender) {
      return DetermineAttackDamage(attacker, defender, Random);
    }

    /// <summary>
    ///   Determines whether the attacker hits the target and how much damage is dealt.
    /// </summary>
    /// <param name="attacker">The attacking participant.</param>
    /// <param name="defender">The defending participant.</param>
    /// <param name="random">Random class to use in generating turn order.</param>
    /// <returns>AttackResults</returns>
    public static AttackResults DetermineAttackDamage(Participant attacker, Participant defender, Random random) {
      if (random.Next(defender.Stats.Dodge, 64) <= defender.Stats.Dodge)
        // attack was dodged

        return new AttackResults(AttackResults.AttackType.Missed, 0);

      if (random.Next(1, 32) == 1) {
        // critical hit

        var criticalHitAttackStrength = attacker.Stats.AttackPower;

        var criticalHitDamage = criticalHitAttackStrength / random.Next(1, 2);

        return new AttackResults(AttackResults.AttackType.CriticalHit, criticalHitDamage);
      }

      // perform regular attack

      var attackStrength = attacker.Stats.AttackPower;

      var targetDefense = defender.Stats.DefensePower;

      var damage = (attackStrength - targetDefense / 2) / random.Next(2, 4);

      return new AttackResults(AttackResults.AttackType.Hit, damage);
    }

    public static bool DetermineParticipantFleeing(Participant participant, Participant runningFrom) {
      return DetermineParticipantFleeing(participant, runningFrom, Random);
    }

    public static bool DetermineParticipantFleeing(Participant participant, List<Participant> runningFrom) {
      return DetermineParticipantFleeing(participant, GetParticipantWithHighestAgility(runningFrom), Random);
    }

    public static bool DetermineParticipantFleeing(Participant participant, List<Participant> runningFrom,
      Random random) {
      return DetermineParticipantFleeing(participant, GetParticipantWithHighestAgility(runningFrom), random);
    }

    /// <summary>
    ///   Determines whether the participant is able to flee or not.
    /// </summary>
    /// <param name="participant">The fleeing participant.</param>
    /// <param name="runningFrom">The participant fleeing from.</param>
    /// <param name="random">Random class to use in generating turn order.</param>
    /// <returns>bool</returns>
    public static bool DetermineParticipantFleeing(Participant participant, Participant runningFrom, Random random) {
      // fleeing formula
      // this currently is missing the RunFactor which monsters have
      // the stronger the monster the higher the RunFactor:
      // https://dragonquestcosmos.fandom.com/wiki/Formulas#Fleeing_Battle 
      // heroAgility * rand(0, 255) >= toughestMonsterAgility * rand(0, 255) * monsterRunFactor

      return participant.Stats.Agility * random.Next(0, 255) >=
             runningFrom.Stats.Agility * random.Next(0, 255);
    }

    /// <summary>
    ///   Gets a list of player participants.
    /// </summary>
    /// <param name="participants">A list of participants.</param>
    /// <returns>List<Participant></returns>
    public static List<Participant> GetPlayerParticipants(List<Participant> participants) {
      return participants.Where(participant =>
        participant.ParticipantType == ParticipantType.Player ||
        participant.ParticipantType == ParticipantType.Npc).ToList();
    }

    /// <summary>
    ///   Gets a list of enemy participants.
    /// </summary>
    /// <param name="participants">A list of participants.</param>
    /// <returns>List<Participant></returns>
    public static List<Participant> GetEnemyParticipants(List<Participant> participants) {
      return participants.Where(participant =>
        participant.ParticipantType == ParticipantType.Enemy).ToList();
    }

    /// <summary>
    ///   Returns the participant with the highest agility.
    /// </summary>
    /// <param name="participants">A list of participants.</param>
    /// <returns>Participant</returns>
    public static Participant GetParticipantWithHighestAgility(List<Participant> participants) {
      return participants.OrderByDescending(participant => participant.Stats.Agility).ToList()[0];
    }

    /// <summary>
    ///   Returns the number of remaining participants with hit points.
    /// </summary>
    /// <param name="participants">A list of participants.</param>
    /// <returns>int</returns>
    public static int GetNumberOfRemainingParticipants(List<Participant> participants) {
      return participants.Count(participant => participant.Stats.HitPoints > 0);
    }

    /// <summary>
    ///   Returns whether the battle is over.
    /// </summary>
    /// <param name="participants">A list of participants.</param>
    /// <returns>bool</returns>
    public static bool IsBattleOver(List<Participant> participants) {
      var enemyParticipants = GetEnemyParticipants(participants);

      var playerParticipants = GetPlayerParticipants(participants);

      return GetNumberOfRemainingParticipants(enemyParticipants) == 0
             || GetNumberOfRemainingParticipants(playerParticipants) == 0;
    }

    /// <summary>
    ///   Returns experience points gained from defeated participants.
    /// </summary>
    /// <param name="defeatedParticipant">A participant.</param>
    /// <returns>int</returns>
    public static int DetermineExperiencePoints(Participant defeatedParticipant) {
      return DetermineExperiencePoints(new List<Participant> { defeatedParticipant });
    }

    /// <summary>
    ///   Returns experience points gained from defeated participants.
    /// </summary>
    /// <param name="defeatedParticipants">A list of participants.</param>
    /// <returns>int</returns>
    public static int DetermineExperiencePoints(List<Participant> defeatedParticipants) {
      var xpSum = 0;

      foreach (var defeatedParticipant in defeatedParticipants) xpSum += defeatedParticipant.ExperiencePoints;

      return xpSum;
    }

    public static int DetermineGoldPoints(Participant defeatedParticipant) {
      return DetermineGoldPoints(new List<Participant> { defeatedParticipant }, Random);
    }

    public static int DetermineGoldPoints(List<Participant> defeatedParticipants) {
      return DetermineGoldPoints(defeatedParticipants, Random);
    }

    public static int DetermineGoldPoints(Participant defeatedParticipant, Random random) {
      return DetermineGoldPoints(new List<Participant> { defeatedParticipant }, random);
    }

    /// <summary>
    ///   Returns gold points gained from defeated participants.
    /// </summary>
    /// <param name="defeatedParticipants">A list of participants.</param>
    /// <param name="random">Random class to use in generating turn order.</param>
    /// <returns>int</returns>
    public static int DetermineGoldPoints(List<Participant> defeatedParticipants, Random random) {
      // (GP * RAND(0, 63) + 192) / 256

      var gpSum = 0;

      foreach (var defeatedParticipant in defeatedParticipants)
        gpSum += defeatedParticipant.GoldPoints * (random.Next(0, 63) + 192) / 256;

      return gpSum;
    }

    public static List<T> DetermineItemsDropped<T>(Dictionary<T, int> droppableItems) {
      return DetermineItemsDropped(droppableItems, Random);
    }

    /// <summary>
    ///   Returns gold points gained from defeated participants.
    /// </summary>
    /// <param name="droppableItems">A Dictionary<T, int> of items and their chance to drop out of 100.</param>
    /// <param name="random">Random class to use in generating turn order.</param>
    /// <returns>List<T></returns>
    public static List<T> DetermineItemsDropped<T>(Dictionary<T, int> droppableItems, Random random) {
      var itemsDropped = new List<T>();

      foreach (var droppableItem in droppableItems)
        if (random.Next(droppableItem.Value, 100) <= droppableItem.Value)
          itemsDropped.Add(droppableItem.Key);

      return itemsDropped;
    }
  }

  /// <summary>
  ///   The results of an attack containing the type and damage amount.
  /// </summary>
  [Serializable]
  public class AttackResults {
    public enum AttackType {
      Hit,
      CriticalHit,
      Missed
    }

    public AttackType attackType;

    public int damage;

    public AttackResults(AttackType attackType, int damage) {
      this.attackType = attackType;
      this.damage = damage;
    }
  }
}