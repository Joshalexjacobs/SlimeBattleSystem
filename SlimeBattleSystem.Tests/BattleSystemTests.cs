using System.Collections.Generic;
using NUnit.Framework;

namespace SlimeBattleSystem.Tests;

public class BattleSystemTests {
  [Test]
  public void SetRandomizationSeed() {
    var seed = "New Seed";

    BattleSystem.SetRandomizationSeed(seed);

    Assert.AreEqual(seed.GetHashCode(), BattleSystem.Seed);
  }

  [Test]
  public void CalculateTurnOrder() {
    var calculatedTurnOrder = BattleSystem.CalculateTurnOrder(9, new RandomMock(new[] { 150 }));

    Assert.AreEqual(5, calculatedTurnOrder);
  }

  [Test]
  public void DetermineTurnOrder() {
    var participantA = new Participant("Participant A");

    participantA.Stats.Agility = 7;

    var participantB = new Participant("Participant B");

    participantB.Stats.Agility = 11;

    var participantC = new Participant("Participant C");

    participantC.Stats.Agility = 4;

    var participants = new List<Participant> { participantA, participantB, participantC };

    var orderedParticipants = BattleSystem.DetermineTurnOrder(participants, new RandomMock(new[] { 25, 150, 90 }));

    Assert.AreEqual(3, orderedParticipants.Count);

    Assert.AreEqual(participantB.Name, orderedParticipants[0].Name);

    Assert.AreEqual(participantA.Name, orderedParticipants[1].Name);

    Assert.AreEqual(participantC.Name, orderedParticipants[2].Name);
  }

  [Test]
  public void DetermineEnemyTarget() {
    var participantA = new Participant("Participant A");

    participantA.Stats.Agility = 7;

    var participantB = new Participant("Participant B");

    participantB.Stats.Agility = 11;

    var participants = new List<Participant> { participantA, participantB };

    var target = BattleSystem.DetermineEnemyTarget(participants, new RandomMock(new[] { 1 }));

    Assert.AreEqual(participantB, target);
  }

  [Test]
  public void DetermineParticipantFleeing() {
    var participantA = new Participant("Participant A",
      new Stats(22, 22, 12, 12, 8, 6, 4, 8, 2));

    var participantB = new Participant("Participant B",
      new Stats(25, 25, 7, 7, 7, 5, 6, 5, 5));

    Assert.IsTrue(
      BattleSystem.DetermineParticipantFleeing(participantA, participantB, new RandomMock(new[] { 0, 3 })));

    Assert.IsFalse(
      BattleSystem.DetermineParticipantFleeing(participantA, participantB, new RandomMock(new[] { 255, 1 })));
  }

  [Test]
  public void GetPlayerParticipants() {
    var participantA = new Participant("Participant A",
      new Stats(22, 22, 12, 12, 8, 6, 4, 8, 2));

    var participantB = new Participant("Participant B",
      new Stats(25, 25, 7, 7, 7, 5, 6, 5, 5));

    participantB.ParticipantType = ParticipantType.Player;

    var participantC = new Participant("Participant C",
      new Stats(25, 25, 7, 7, 7, 5, 6, 5, 5));

    Assert.AreEqual(participantB,
      BattleSystem.GetPlayerParticipants(new List<Participant> { participantA, participantB, participantC })[0]);
  }

  [Test]
  public void GetEnemyParticipants() {
    var participantA = new Participant("Participant A",
      new Stats(22, 22, 12, 12, 8, 6, 4, 8, 2));

    var participantB = new Participant("Participant B",
      new Stats(25, 25, 7, 7, 7, 5, 6, 5, 5));

    participantB.ParticipantType = ParticipantType.Player;

    var participantC = new Participant("Participant C",
      new Stats(25, 25, 7, 7, 7, 5, 6, 5, 5));

    var participants = new List<Participant> { participantA, participantB, participantC };

    var enemyParticipants = BattleSystem.GetEnemyParticipants(participants);

    Assert.AreEqual(participantA, enemyParticipants[0]);

    Assert.AreEqual(participantC, enemyParticipants[1]);
  }

  [Test]
  public void GetParticipantWithHighestAgility() {
    var participantA = new Participant("Participant A",
      new Stats(22, 22, 12, 12, 8, 6, 4, 8, 2));

    var participantB = new Participant("Participant B",
      new Stats(25, 25, 7, 7, 7, 5, 6, 5, 5));

    participantB.ParticipantType = ParticipantType.Player;

    var participantC = new Participant("Participant C",
      new Stats(25, 25, 7, 7, 7, 7, 6, 5, 5));

    Assert.AreEqual(participantC,
      BattleSystem.GetParticipantWithHighestAgility(new List<Participant>
        { participantA, participantB, participantC }));
  }

  [Test]
  public void DetermineRemainingParticipants() {
    var participantA = new Participant("Participant A",
      new Stats(22, 22, 12, 12, 8, 6, 4, 8, 2));

    var participantB = new Participant("Participant B",
      new Stats(0, 0, 7, 7, 7, 5, 6, 5, 5));

    var participantC = new Participant("Participant C",
      new Stats(25, 25, 7, 7, 7, 7, 6, 5, 5));

    Assert.AreEqual(2,
      BattleSystem.GetNumberOfRemainingParticipants(new List<Participant>
        { participantA, participantB, participantC }));
  }

  public class DetermineAttackDamage {
    [Test]
    public void CriticalHit() {
      var participantA = new Participant("Participant A",
        new Stats(22, 22, 12, 12, 8, 6, 8, 8, 2));

      var participantB = new Participant("Participant B",
        new Stats(25, 25, 7, 7, 7, 5, 6, 5, 5));

      var results = BattleSystem.DetermineAttackDamage(
        participantA,
        participantB,
        new RandomMock(new[] { 1, 1, 64 }));

      // Critical hit
      Assert.AreEqual(8, results.Damage);
    }

    [Test]
    public void Dodge() {
      var participantA = new Participant("Participant A",
        new Stats(22, 22, 12, 12, 8, 6, 4, 8, 2));

      var participantB = new Participant("Participant B",
        new Stats(25, 25, 7, 7, 7, 5, 6, 5, 5));

      var results = BattleSystem.DetermineAttackDamage(
        participantA,
        participantB,
        new RandomMock(new[] { participantB.Stats.Dodge }));

      // Dodge
      Assert.AreEqual(0, results.Damage);
    }

    [Test]
    public void RegularHit() {
      var participantA = new Participant("Participant A",
        new Stats(22, 22, 12, 12, 4, 6, 4, 8, 2));

      var participantB = new Participant("Participant B",
        new Stats(25, 25, 7, 7, 7, 5, 6, 5, 5));

      var results = BattleSystem.DetermineAttackDamage(
        participantA,
        participantB,
        new RandomMock(new[] { 2, 2, 64 }));

      // Regular hit
      Assert.AreEqual(1, results.Damage);
    }
  }

  public class DetermineEndOfBattle {
    [Test]
    public void ExperiencePoints() {
      var participantA = new Participant("Participant A",
        new Stats(22, 22, 12, 12, 8, 6, 4, 8, 2));

      participantA.ExperiencePoints = 12;

      Assert.AreEqual(participantA.ExperiencePoints,
        BattleSystem.DetermineExperiencePoints(new List<Participant> { participantA }));
    }

    [Test]
    public void GoldPoints() {
      var participantA = new Participant("Participant A",
        new Stats(22, 22, 12, 12, 8, 6, 4, 8, 2));

      participantA.GoldPoints = 12;

      Assert.AreEqual(10,
        BattleSystem.DetermineGoldPoints(
          new List<Participant> { participantA },
          new RandomMock(new[] { 32 })
        )
      );
    }

    [Test]
    public void ItemsDropped() {
      var participantA = new Participant("Participant A",
        new Stats(22, 22, 12, 12, 8, 6, 4, 8, 2));

      var potion = new object();

      var droppableItems = new Dictionary<object, int>();

      droppableItems.Add(potion, 50);

      Assert.AreEqual(1,
        BattleSystem.DetermineItemsDropped(
          droppableItems,
          new RandomMock(new[] { 50 })).Count
      );

      Assert.AreEqual(potion,
        BattleSystem.DetermineItemsDropped(
          droppableItems,
          new RandomMock(new[] { 50 }))[0]
      );
    }
  }
}