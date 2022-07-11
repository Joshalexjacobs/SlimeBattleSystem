# Slime Battle System 🐉⚔️🛡️
An easy to use RPG combat system for Unity that emulates the battle formulas from the original Dragon Quest.

## Installation

1. Download the latest SlimeBattleSystem.dll release found [here](https://github.com/Joshalexjacobs/SlimeBattleSystem/releases) 
2. Drop it into your assets folder
3. You should now be able to reference the SlimeBattleSystem namespace within your own scripts

# API

## Battle System
A static class that handles all combat logic.

### SetRandomizationSeed
Sets the randomization seed used by most formulas.
```csharp
var seed = "New Seed";
    
BattleSystem.SetRandomizationSeed(seed);
```

### CalculateTurnOrder
Calculates a participant's turn order with a random range using their Agility stat. Returns an integer.
```csharp
foreach (var participant in participants) {
    participant.TurnOrder = BattleSystem.CalculateTurnOrder(participant.Stats.Agility, random);
}
```

### DetermineTurnOrder
Determines the order a group of Participants will attack in.
```csharp
a
```

### DetermineEnemyTarget
Determines the order a group of Participants will attack in.
```csharp
a
```

### DetermineAttackDamage
Determines the order a group of Participants will attack in.
```csharp
a
```

### DetermineParticipantFleeing
Determines the order a group of Participants will attack in.
```csharp
a
```

### GetPlayerParticipants
Determines the order a group of Participants will attack in.
```csharp
a
```

### GetEnemyParticipants
Determines the order a group of Participants will attack in.
```csharp
a
```

### GetParticipantWithHighestAgility
Determines the order a group of Participants will attack in.
```csharp
a
```

### GetNumberOfRemainingParticipants
Determines the order a group of Participants will attack in.
```csharp
a
```

### IsBattleOver
Determines the order a group of Participants will attack in.
```csharp
a
```

### DetermineExperiencePoints
Determines the order a group of Participants will attack in.
```csharp
a
```

### DetermineGoldPoints
Determines the order a group of Participants will attack in.
```csharp
a
```

### DetermineItemsDropped
Determines the order a group of Participants will attack in.
```csharp
a
```

# Confused? Check out the sample project!

https://github.com/Joshalexjacobs/SlimeBattleSystemSample

![Unity Sample Gif](https://i.imgur.com/S3mjjGf.gif)


*Note: This package was created with the first Dragon Quest game in mind. Most of the logic being handled here is intended for 1v1 battles, but I've done my best to try and support battles bigger than 2 combatants.*

----

## Formulas used

- Battle Turn Order
- Attacking
- Dodging
- Critical Hits
- Fleeing
- Awarding Experience Points
- Awarding Gold Points
- Determining items dropped

## What's missing?

### Spells, Items, and Status Ailments
I originally planned on baking these into the package, but after some testing decided that this will be much more flexible if handled by the user. If you're looking for an easy way to do this, check out the [sample repo.](https://github.com/Joshalexjacobs/SlimeBattleSystemSample) 

### Leveling and name based stat growth
In the original Dragon Quest, your character's name determines how your stats will grow upon leveling. After reviewing the logic I figured this was a bit too complex at this time. For now I'm leaving all leveling and stat growth to the user, but may add this as an optional function in the future.

*For those interested: akiraslime posted a great Dragon Warrior stat growth FAQ [here.](https://gamefaqs.gamespot.com/nes/563408-dragon-warrior/faqs/18342)*

### 75% - 100% Enemy Health Formula
In the base game, enemy health is randomly determined between 75% - 100% of their max health (eg. a Magician has a max health pool of 13 hit points. When encountered, a Magician would have 10 - 13 hit points). For simplicity sake, I've decided to omit this logic for now as it could be easily added before beginning the encounter if desired.

### Experience/Gold Multiplier
I did discover a formula for increasing experience points and gold points based on the number of characters in the player's party. For now I've omitted any multiplier due to the fact that this system is entirely based on the original Dragon Quest (in which there is only 1 member in the party). However, I would love to add some sort of configurable profiles in the future allowing for minor changes like this one.
