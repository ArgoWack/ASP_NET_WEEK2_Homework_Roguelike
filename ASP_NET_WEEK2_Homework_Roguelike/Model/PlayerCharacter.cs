using ASP_NET_WEEK2_Homework_Roguelike.Items;
using ASP_NET_WEEK2_Homework_Roguelike.ItemKinds;
using ASP_NET_WEEK2_Homework_Roguelike.Model.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.IO;
using static System.Console;
using System.Text.Json.Serialization;
using ASP_NET_WEEK2_Homework_Roguelike.Services;

namespace ASP_NET_WEEK2_Homework_Roguelike.Model
{
    public class PlayerCharacter
    {
        private readonly CharacterStatsService _statsService;
        private readonly InventoryService _inventoryService;

        private int currentX;
        private int currentY;

        // Base stats
        private float baseSpeed;
        private float baseAttack;
        private float baseDefense;

        // Modifiers (buffs/debuffs)
        private float speedModifier;
        private float attackModifier;
        private float defenseModifier;

        // Other properties...
        private List<Item> inventory;

        public int CurrentX
        {
            get => currentX;
            set => currentX = value;
        }

        public int CurrentY
        {
            get => currentY;
            set => currentY = value;
        }

        public Map CurrentMap { get; set; }

        public List<Item> Inventory
        {
            get => inventory;
            set
            {
                inventory = value;
                UpdateWeight();
                UpdateAttack();
                UpdateDefense();
            }
        }

        public string Name { get; set; }
        public int Health { get; set; }
        public int Weight { get; private set; }
        public int Money { get; set; }
        public int Level { get; set; }
        public int Experience { get; set; }

        public Helmet EquippedHelmet { get; set; }
        public Armor EquippedArmor { get; set; }
        public Shield EquippedShield { get; set; }
        public Gloves EquippedGloves { get; set; }
        public Trousers EquippedTrousers { get; set; }
        public Boots EquippedBoots { get; set; }
        public Amulet EquippedAmulet { get; set; }
        public SwordOneHanded EquippedSwordOneHanded { get; set; }
        public SwordTwoHanded EquippedSwordTwoHanded { get; set; }

        public int HealthLimit => 100 + Level * 10;

        // Updated Speed, Attack, and Defense properties with base values and modifiers
        public float Speed => (baseSpeed + speedModifier) / (Weight / 100 + 1);
        public float Attack => (baseAttack + attackModifier) * Speed;
        public float Defense => (baseDefense + defenseModifier) * Speed;

        public PlayerCharacter()
        {
            _statsService = new CharacterStatsService();
            _inventoryService = new InventoryService();

            Inventory = new List<Item>();
            Level = 1;
            Health = HealthLimit;
            Weight = 0;
            Money = 0;
            currentX = 0;
            currentY = 0;
            CurrentMap = new Map();

            baseSpeed = Level * 10;
            baseAttack = 0;
            baseDefense = 0;

            speedModifier = 0;
            attackModifier = 0;
            defenseModifier = 0;

            UpdateAttack();
            UpdateDefense();
            UpdateWeight();
        }

        public void MovePlayer(string direction, Map map)
        {
            Room newRoom = map.MovePlayer(ref currentX, ref currentY, direction);
        }

        public void UpdateWeight()
        {
            Weight = _statsService.CalculateWeight(this);
        }

        public int CheckWeight()
        {
            int totalWeight = 0;
            foreach (Item item in Inventory)
            {
                totalWeight += item.Weight;
            }
            return totalWeight;
        }

        public void UpdateAttack()
        {
            baseAttack = _statsService.CalculateAttack(this);
        }

        public float CheckAttack()
        {
            return (EquippedHelmet?.Attack ?? 0) +
                    (EquippedArmor?.Attack ?? 0) +
                    (EquippedShield?.Attack ?? 0) +
                    (EquippedGloves?.Attack ?? 0) +
                    (EquippedTrousers?.Attack ?? 0) +
                    (EquippedBoots?.Attack ?? 0) +
                    (EquippedAmulet?.Attack ?? 0) +
                    (EquippedSwordOneHanded?.Attack ?? 0) +
                    (EquippedSwordTwoHanded?.Attack ?? 0);
        }

        public void UpdateDefense()
        {
            baseDefense = _statsService.CalculateDefense(this);
        }

        public float CheckDefense()
        {
            return (EquippedHelmet?.Defense ?? 0) +
                    (EquippedArmor?.Defense ?? 0) +
                    (EquippedShield?.Defense ?? 0) +
                    (EquippedGloves?.Defense ?? 0) +
                    (EquippedTrousers?.Defense ?? 0) +
                    (EquippedBoots?.Defense ?? 0) +
                    (EquippedAmulet?.Defense ?? 0) +
                    (EquippedSwordOneHanded?.Defense ?? 0) +
                    (EquippedSwordTwoHanded?.Defense ?? 0);
        }

        public void EquipItem(int itemId)
        {
            _inventoryService.EquipItem(this, itemId);
        }
        public void UnequipItem(Type itemType)
        {
            _inventoryService.UnequipItem(this, itemType);
        }

        public void DiscardItem(int itemId)
        {
            _inventoryService.DiscardItem(this, itemId);
        }

        public void HealByPotion(HealthPotion healthPotion)
        {
            Heal(healthPotion.MoneyWorth);
            Inventory.Remove(healthPotion);
        }

        public void Heal(int amount)
        {
            Health = Math.Min(Health + amount, HealthLimit);
        }

        public void GetExperience(int amount)
        {
            Experience += amount;
            while (Experience >= Level * 100)
            {
                Experience -= Level * 100;
                Level++;
                Health = HealthLimit;
                UpdateAttack();
                UpdateDefense();
            }
        }

        public void SaveGame()
        {
            string sanitizedFileName = $"{Name}_savefile.json".Replace(" ", "_").Replace(":", "_").Replace("/", "_");
            var gameState = new GameState
            {
                PlayerCharacter = this,
                Map = CurrentMap
            };

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                ReferenceHandler = ReferenceHandler.Preserve,
                Converters = { new Converters.ItemConverter(), new Converters.MapConverter() }
            };

            string jsonString = JsonSerializer.Serialize(gameState, options);
            File.WriteAllText(sanitizedFileName, jsonString);
            WriteLine($"\n Game saved as {sanitizedFileName} \n");
        }

        public static GameState LoadGame(string characterName)
        {
            string sanitizedFileName = $"{characterName}_savefile.json".Replace(" ", "_").Replace(":", "_").Replace("/", "_");

            if (File.Exists(sanitizedFileName))
            {
                string jsonString = File.ReadAllText(sanitizedFileName);

                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    Converters = { new Converters.ItemConverter(), new Converters.MapConverter() }
                };

                var gameState = JsonSerializer.Deserialize<GameState>(jsonString, options);

                if (gameState != null)
                {
                    gameState.PlayerCharacter.CurrentMap = gameState.Map;

                    if (gameState.PlayerCharacter.Inventory.Any())
                    {
                        ItemFactory.LastGeneratedItemId = gameState.PlayerCharacter.Inventory.Max(i => i.ID);
                    }
                    else
                    {
                        ItemFactory.LastGeneratedItemId = 0;
                    }

                    WriteLine($"Game loaded from {sanitizedFileName} \n");
                }

                return gameState;
            }
            else
            {
                throw new FileNotFoundException($"Save file for character '{characterName}' not found.");
            }
        }

        public void ModifySpeed(float amount)
        {
            speedModifier += amount;
        }

        public void ModifyAttack(float amount)
        {
            attackModifier += amount;
        }

        public void ModifyDefense(float amount)
        {
            defenseModifier += amount;
        }

        public void SellItem(int itemId)
        {
            var item = Inventory.FirstOrDefault(i => i.ID == itemId);
            if (item == null)
            {
                throw new InvalidOperationException("Item not found in inventory.");
            }

            Money += item.MoneyWorth;
            Inventory.Remove(item);

            UpdateWeight();
            UpdateAttack();
            UpdateDefense();
        }

        public void BuyHealthPotion()
        {
            if (Money < 40)
            {
                throw new InvalidOperationException("You don't have enough money to buy a health potion.");
            }

            Money -= 40;
            var healthPotion = ItemFactory.GenerateItem<HealthPotion>();
            Inventory.Add(healthPotion);

            UpdateWeight();
        }
        public void SetBaseAttack(float value)
        {
            baseAttack = value;
        }

        public void SetBaseDefense(float value)
        {
            baseDefense = value;
        }

        public void SetWeight(int value)
        {
            Weight = value;
        }
    }
}