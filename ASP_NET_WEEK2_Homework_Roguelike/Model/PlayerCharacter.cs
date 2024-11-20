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
using ASP_NET_WEEK2_Homework_Roguelike.Model.Items;

namespace ASP_NET_WEEK2_Homework_Roguelike.Model
{
    public class PlayerCharacter
    {
        private readonly CharacterStatsService _statsService;
        private readonly InventoryService _inventoryService;
        private readonly EventService _eventService;

        private int currentX;
        private int currentY;

        // Base stats and modifiers
        private float baseSpeed;
        private float baseAttack;
        private float baseDefense;
        private float speedModifier;
        private float attackModifier;
        private float defenseModifier;

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
                UpdateStats();
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
            Money = 0;
            currentX = 0;
            currentY = 0;
            CurrentMap = new Map();

            baseSpeed = Level * 10;

            UpdateStats();
        }

        public void MovePlayer(string direction, Map map, MapService mapService)
        {
            int playerX = CurrentX;
            int playerY = CurrentY;

            mapService.MovePlayer(map, ref playerX, ref playerY, direction);

            CurrentX = playerX;
            CurrentY = playerY;
        }

        public void UpdateStats()
        {
            Weight = _statsService.CalculateWeight(this);
            baseAttack = _statsService.CalculateAttack(this);
            baseDefense = _statsService.CalculateDefense(this);
        }

        public void EquipItem(int itemId)
        {
            _inventoryService.EquipItem(this, itemId);
            UpdateStats();
        }

        public void UnequipItem(Type itemType)
        {
            _inventoryService.UnequipItem(this, itemType);
            UpdateStats();
        }

        public void DiscardItem(int itemId)
        {
            _inventoryService.DiscardItem(this, itemId);
            UpdateStats();
        }
        public void SellHealthPotion()
        {
            var potion = Inventory.OfType<HealthPotion>().FirstOrDefault(p => p.Quantity > 0);

            if (potion == null)
            {
                throw new InvalidOperationException("You don't have any health potions to sell.");
            }

            Money += potion.MoneyWorth;
            potion.Quantity--;

            if (potion.Quantity == 0)
            {
                Inventory.Remove(potion);
            }

            Weight -= potion.Weight; // Adjust weight dynamically
            UpdateStats();
        }
        public void ReceiveHealthPotion()
        {
            var existingPotion = Inventory.OfType<HealthPotion>().FirstOrDefault(p => p.Quantity < p.StackSize);

            if (existingPotion != null)
            {
                existingPotion.Quantity++;
            }
            else
            {
                var newPotion = ItemFactoryService.GenerateItem(typeof(HealthPotion)) as HealthPotion;
                if (newPotion != null)
                {
                    newPotion.Quantity = 1;
                    Inventory.Add(newPotion);
                }
            }

            Weight += Inventory.OfType<HealthPotion>().Sum(p => p.Weight); // Recalculate total weight
            UpdateStats();
        }
        public void HealByPotion()
        {
            var potion = Inventory.OfType<HealthPotion>().FirstOrDefault(p => p.Quantity > 0);

            if (potion == null)
            {
                _eventService.HandleEventOutcome("You don't have any potions to use.");
                return;
            }

            Heal(potion.HealingAmount);
            potion.Quantity--;

            if (potion.Quantity == 0)
            {
                Inventory.Remove(potion);
            }

            Weight -= potion.Weight; // Adjust weight dynamically
            UpdateStats();
        }

        public void Heal(int amount)
        {
            int healed = Math.Min(Health + amount, HealthLimit);
            Health = healed;
            _eventService.HandleEventOutcome("You healed for: " + (healed- Health).ToString());
        }

        public void GetExperience(int amount)
        {
            Experience += amount;
            while (Experience >= Level * 100)
            {
                Experience -= Level * 100;
                Level++;
                Health = HealthLimit;
                UpdateStats();
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
                        ItemFactoryService.LastGeneratedItemId = gameState.PlayerCharacter.Inventory.Max(i => i.ID);
                    }
                    else
                    {
                        ItemFactoryService.LastGeneratedItemId = 0;
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
            UpdateStats();
        }

        public void ModifyAttack(float amount)
        {
            attackModifier += amount;
            UpdateStats();
        }

        public void ModifyDefense(float amount)
        {
            defenseModifier += amount;
            UpdateStats();
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

            UpdateStats();
        }

        public void BuyHealthPotion()
        {
            if (Money < 40)
            {
                throw new InvalidOperationException("You don't have enough money to buy a health potion.");
            }

            Money -= 40;
            var healthPotion = ItemFactoryService.GenerateItem(typeof(HealthPotion)) as HealthPotion;

            if (healthPotion != null)
            {
                ReceiveHealthPotion();
                UpdateStats();
                _eventService.HandleEventOutcome("You bought a health potion.");
            }
            else
            {
                throw new InvalidOperationException("Failed to generate health potion.");
            }
        }
    }
}