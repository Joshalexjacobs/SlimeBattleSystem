using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace SlimeBattleSystem.Tests
{
    public class BattleSystemTests
    {
        
        [Test]
        public void SetRandomizationSeed()
        {
            string seed = "New Seed";
            
            BattleSystem.SetRandomizationSeed(seed);

            Assert.AreEqual(seed.GetHashCode(), BattleSystem.Seed);
        }

        [Test]
        public void CalculateTurnOrder()
        {
            var calculatedTurnOrder = BattleSystem.CalculateTurnOrder(9, new RandomMock(new [] { 150 }));

            Assert.AreEqual(5, calculatedTurnOrder);
        }

        [Test]
        public void DetermineTurnOrder()
        {
            Participant participantA = new Participant("Participant A");

            participantA.stats.agility = 7;
            
            Participant participantB = new Participant("Participant B");

            participantB.stats.agility = 11;
            
            Participant participantC = new Participant("Participant C");

            participantC.stats.agility = 4;

            var participants = new List<Participant>() {participantA, participantB, participantC}; 
            
            List<Participant> orderedParticipants = BattleSystem.DetermineTurnOrder(participants, new RandomMock(new [] { 25, 150, 90 }));

            Assert.AreEqual(3, orderedParticipants.Count);
            
            Assert.AreEqual(participantB.name, orderedParticipants[0].name);
            
            Assert.AreEqual(participantA.name, orderedParticipants[1].name);
            
            Assert.AreEqual(participantC.name, orderedParticipants[2].name);
        }

        [Test]
        public void DetermineEnemyTarget()
        {
            Participant participantA = new Participant("Participant A");

            participantA.stats.agility = 7;
            
            Participant participantB = new Participant("Participant B");

            participantB.stats.agility = 11;
            
            var participants = new List<Participant>() {participantA, participantB};

            Participant target = BattleSystem.DetermineEnemyTarget(participants, new RandomMock( new []{ 1 }));
            
            Assert.AreEqual(participantB, target);
        }

        public class DetermineAttackDamage
        {
         
            [Test]
            public void CriticalHit()
            {
                Participant participantA = new Participant("Participant A", 
                    new Stats(22, 12, 8, 6, 4, 8, 2));

                Participant participantB = new Participant("Participant B", 
                    new Stats(25, 7, 7, 5, 6, 5, 5));

                // Critical hit
                Assert.AreEqual(12, BattleSystem.DetermineAttackDamage(
                    participantA, 
                    participantB, 
                    new RandomMock(new [] {1, 1, 64})));
            }
         
            [Test]
            public void Dodge()
            {
                Participant participantA = new Participant("Participant A", 
                    new Stats(22, 12, 8, 6, 4, 8, 2));

                Participant participantB = new Participant("Participant B", 
                    new Stats(25, 7, 7, 5, 6, 5, 5));
                
                // Dodge
                Assert.AreEqual(0, BattleSystem.DetermineAttackDamage(
                        participantA, 
                        participantB, 
                        new RandomMock(new [] {participantB.stats.dodge})));
            }
         
            [Test]
            public void RegularHit()
            {
                Participant participantA = new Participant("Participant A", 
                    new Stats(22, 12, 8, 6, 4, 8, 2));

                Participant participantB = new Participant("Participant B", 
                    new Stats(25, 7, 7, 5, 6, 5, 5));
                
                // Regular hit
                Assert.AreEqual(3, BattleSystem.DetermineAttackDamage(
                    participantA, 
                    participantB,
                    new RandomMock(new [] {2, 2, 64})));
            }
            
        }

        [Test]
        public void DetermineParticipantFleeing()
        {
            Participant participantA = new Participant("Participant A", 
                new Stats(22, 12, 8, 6, 4, 8, 2));
            
            Participant participantB = new Participant("Participant B", 
                new Stats(25, 7, 7, 5, 6, 5, 5));

            Assert.IsTrue(
                BattleSystem.DetermineParticipantFleeing(participantA, participantB, new RandomMock(new [] { 0, 3 })));

            Assert.IsFalse(
                BattleSystem.DetermineParticipantFleeing(participantA, participantB, new RandomMock(new [] { 255, 1 })));
        }

        [Test]
        public void GetPlayerParticipants()
        {
            Participant participantA = new Participant("Participant A", 
                new Stats(22, 12, 8, 6, 4, 8, 2));
            
            Participant participantB = new Participant("Participant B", 
                new Stats(25, 7, 7, 5, 6, 5, 5));

            participantB.ParticipantType = ParticipantType.Player;
            
            Participant participantC = new Participant("Participant C", 
                new Stats(25, 7, 7, 5, 6, 5, 5));

            Assert.AreEqual(participantB, 
                BattleSystem.GetPlayerParticipants(new List<Participant>() {participantA, participantB, participantC})[0]);
        }

        [Test]
        public void GetEnemyParticipants()
        {
            Participant participantA = new Participant("Participant A", 
                new Stats(22, 12, 8, 6, 4, 8, 2));
            
            Participant participantB = new Participant("Participant B", 
                new Stats(25, 7, 7, 5, 6, 5, 5));

            participantB.ParticipantType = ParticipantType.Player;
            
            Participant participantC = new Participant("Participant C", 
                new Stats(25, 7, 7, 5, 6, 5, 5));

            var participants = new List<Participant>() {participantA, participantB, participantC};

            var enemyParticipants = BattleSystem.GetEnemyParticipants(participants); 
            
            Assert.AreEqual(participantA, enemyParticipants[0]);
            
            Assert.AreEqual(participantC, enemyParticipants[1]);
        }

        [Test]
        public void GetParticipantWithHighestAgility()
        {
            Participant participantA = new Participant("Participant A", 
                new Stats(22, 12, 8, 6, 4, 8, 2));
            
            Participant participantB = new Participant("Participant B", 
                new Stats(25, 7, 7, 5, 6, 5, 5));

            participantB.ParticipantType = ParticipantType.Player;
            
            Participant participantC = new Participant("Participant C", 
                new Stats(25, 7, 7, 7, 6, 5, 5));

            Assert.AreEqual(participantC, 
                BattleSystem.GetParticipantWithHighestAgility(new List<Participant>() {participantA, participantB, participantC}));
        }

        [Test]
        public void DetermineRemainingParticipants()
        {
            Participant participantA = new Participant("Participant A", 
                new Stats(22, 12, 8, 6, 4, 8, 2));
            
            Participant participantB = new Participant("Participant B", 
                new Stats(0, 7, 7, 5, 6, 5, 5));

            Participant participantC = new Participant("Participant C", 
                new Stats(25, 7, 7, 7, 6, 5, 5));

            Assert.AreEqual(2, 
                BattleSystem.GetNumberOfRemainingParticipants(new List<Participant>()
                    {participantA, participantB, participantC}));
        }

        public class DetermineEndOfBattle
        {

            [Test]
            public void ExperiencePoints()
            {
                Participant participantA = new Participant("Participant A",
                    new Stats(22, 12, 8, 6, 4, 8, 2));

                participantA.experiencePoints = 12;

                Assert.AreEqual(participantA.experiencePoints,
                    BattleSystem.DetermineExperiencePoints(new List<Participant>() {participantA}));
            }

            [Test]
            public void GoldPoints()
            {
                Participant participantA = new Participant("Participant A",
                    new Stats(22, 12, 8, 6, 4, 8, 2));

                participantA.goldPoints = 12;

                Assert.AreEqual(10,
                    BattleSystem.DetermineGoldPoints(
                        new List<Participant>() {participantA},
                        new RandomMock(new[] {32})
                    )
                );
            }

            [Test]
            public void ItemsDropped()
            {
                Participant participantA = new Participant("Participant A",
                    new Stats(22, 12, 8, 6, 4, 8, 2));

                Item potion = new Item("Potion");

                DroppableItem droppableItem = new DroppableItem(potion, 50);

                participantA.droppableItems = new List<DroppableItem>() {droppableItem};

                Assert.AreEqual(1,
                    BattleSystem.DetermineItemsDropped(
                        new List<Participant>() {participantA},
                        new RandomMock(new[] {50})).Count
                );

                Assert.AreEqual(potion,
                    BattleSystem.DetermineItemsDropped(
                        new List<Participant>() {participantA},
                        new RandomMock(new[] {50}))[0]
                );

            }
        }

    }
    
}


