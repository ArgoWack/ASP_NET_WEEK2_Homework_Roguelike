# Roguelike .NET Console Game: Detailed Player Manual

## Table of Contents
1. [Overview](#overview)
2. [Getting Started](#getting-started)
3. [Controls](#controls)
4. [Character Stats](#character-stats)
5. [Equipment and Inventory](#equipment-and-inventory)
6. [Combat System](#combat-system)
7. [Leveling Up](#leveling-up)
8. [Random Events](#random-events)
9. [Saving and Loading](#saving-and-loading)
10. [Tips and Strategies](#tips-and-strategies)

---

## Overview

The Roguelike .NET Console Game is a procedurally generated adventure where players navigate through random rooms, face enemies, and collect gear. The game uses permadeath mechanics, but you can save at any time to reload your progress. Each session offers a unique experience due to the random nature of the game world.

---

## Getting Started

At the start of the game, you will be prompted to enter your character's name. This name is tied to your save file, allowing you to return to the same game in future sessions.

---

## Controls

- **Movement**: Use the classic **W/A/S/D** keys to move up, left, down, and right.
- **Map**: Press **M** to open map.
- **Interact with Objects/Events**: When you encounter an object or event, the game will present options with corresponding letters or numbers. Type the matching input to interact.
- **Inventory**: You can view your inventory by typing `I` at any time during gameplay.
- **Equip/Use Items**: Select an item from the inventory using its associated ID or letter to equip or use it.

---

## Character Stats

Your character's strength and survival depend on several key stats. These stats can be improved by leveling up, equipping better gear, or participating in special events.

- **Attack**: Determines how much damage your character deals to enemies.
- **Defense**: Reduces damage taken from enemy attacks.
- **Speed**: Affects movement speed which influences attack and defense
- **Weight**: Affects how much equipment your character can carry. Heavier characters may be slower reducing speed, but they can wear more protective gear.
- **Money**: Earned through battles and events. Can be used to purchase items in certain events or upgrade equipment.
- **Health**: Represents your overall vitality. When health drops to 0, your character dies.

---

## Equipment and Inventory

### Equipment Types
You can equip your character with various gear to improve their stats:

- **Armors, Amulets, Boots, Gloves, Helmets, and Shields**: Increases defense, but may add weight.
- **Weapons**: Increases attack. Choose between one-handed and two-handed weapons, depending on your combat strategy.
- **Consumables**: Items such as health potions that can be used once to restore health or provide temporary boosts.

### Example Gear:
- **Legendary Armor**  
  *Defense: 47, Weight: 51, Money worth: 14*  
  This armor provides significant protection at the cost of speed due to its weight.
  
- **Sword (Two-Handed)**  
  *Attack: 60, Defense: 0, Weight: 40*  
  A powerful weapon that boosts attack but leaves no room for a shield.

### Inventory Management
- You can carry items in your inventory for later use. To equip or use an item, open the inventory (`I`) and select the item.
- Keep an eye on your **weight** to avoid slowing down your character's speed.

---

## Combat System

Combat is turn-based and initiated when you encounter an enemy in a room. Each turn allows you to attack, use items, or attempt to flee.

- **Attack**: Deals damage based on your attack stat and enemy defense. Different weapons have unique properties (e.g., a two-handed sword may deal more damage but is havier and excluded shield usage).
- **Defense**: Your defense stat reduces incoming damage. Equip shields and armor to improve your defense.
- **Speed**: Speed depends on weight, the lighter you are the better is your attack and defense.

---

## Leveling Up

Experience points (XP) are earned primarily by defeating monsters. Once you collect enough XP, your character will level up, improving their core stats:

- **Leveling Up**: Each level increases your base stats (Attack, Defense, Health) and grants points to toward specific attributes (e.g., more Attack or Speed).

---

## Random Events

Throughout your adventure, you will encounter random events, including:
- **Conversations with NPCs**: Certain characters may offer shop, items, or even stat boosts based on dialogue choices.
- **Finding items**: Just random items which can be taken.
- **Empty rooms**: Some rooms are just empty.
- **Monster rooms**: There are rooms in which there is monster guarding them which can be killed.
  
The outcome of events may depend on your stats or gear.

---

## Saving and Loading

- You can save and quit your game at any time by typing `Q`.
- Saved games are linked to your character’s name and are stored in the `saves/` folder in the project directory.
- If you die, the game shuts down. However, you can reload your most recent save by typing `2` on the main menu.

---

## Tips and Strategies

- **Save Often**: Since permadeath is a core mechanic, save frequently, especially before entering new rooms or engaging in battles.
- **Balance Attack and Defense**: Don’t neglect defense in favor of pure attack power. Strong defense will keep you alive longer.
- **Mind Your Weight**: Heavier gear can slow you down decreesing your stats.
- **Explore Thoroughly**: The randomly generated map means every room could hold valuable gear or a deadly trap.
