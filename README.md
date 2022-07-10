# SlimeBattleSystem
An easy to use RPG combat system for Unity that utilizes formulas from the original Dragon Quest.

## Check out the sample!
[url here]

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

### Spells and Items
I originally planned on baking spells and items into the package, but after some testing decided that this will be much more flexible if handled by the user.

### Status ailments
Currently there is no way to impair a player character or enemy's status (eg. Sleep, Poison, Paralysis, etc). These kinds of impairments are crucial to Dragon Quest and most other RPGs so I will be adding them in the near future, but for now they have not been included.

### Leveling and name based stat growth
In the original Dragon Quest, your character's name determines how your stats will grow upon leveling. After reviewing the logic I figured this was a bit too complex at this time. For now I'm leaving all leveling and stat growth to the user, but may add this as an optional function in the future.

### 75% - 100% Enemy Health Formula
In the base game, enemy health is randomly determined between 75% - 100% of their max health (eg. a Magician has a max health pool of 13 hit points. When encountered, a Magician would have 10 - 13 hit points). For simplicity sake, I've decided to omit this logic for now as it could be easily added before beginning the encounter if desired.

### Experience/Gold Multiplier
I did discover a formula for increasing experience points and gold points based on the number of characters in the player's party. For now I've omitted any multiplier due to the fact that this system is entirely based on the original Dragon Quest (in which there is only 1 member in the party). However, I would love to add some sort of configurable profiles in the future allowing for minor changes like this one.
