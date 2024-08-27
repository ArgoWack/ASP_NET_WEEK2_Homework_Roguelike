using ASP_NET_WEEK2_Homework_Roguelike.ItemKinds;
using ASP_NET_WEEK2_Homework_Roguelike.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using System.Text.Json;
using System.IO;
using System.Text.Json.Serialization;
using System.Collections;
using System.Reflection.Emit;
using System.Xml.Linq;

namespace ASP_NET_WEEK2_Homework_Roguelike
{
    public class PlayerCharacter
    {
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
            Inventory = new List<Item>();
            Level = 1;
            Health = HealthLimit;
            Weight = 0;
            Money = 0;
            currentX = 0; // Starting X position
            currentY = 0; // Starting Y position
            CurrentMap = new Map(); // Initialize the map

            // Initialize base stats
            baseSpeed = 100 + Level * 10;
            baseAttack = 0;
            baseDefense = 0;

            // Initialize modifiers
            speedModifier = 0;
            attackModifier = 0;
            defenseModifier = 0;
        }

        public void DisplayCharacterStats()
        {
            var equippedItems = new Dictionary<string, Item>
        {
            { "Amulet", EquippedAmulet },
            { "Armor", EquippedArmor },
            { "Boots", EquippedBoots },
            { "Gloves", EquippedGloves },
            { "Helmet", EquippedHelmet },
            { "Shield", EquippedShield },
            { "SwordOneHanded", EquippedSwordOneHanded },
            { "SwordTwoHanded", EquippedSwordTwoHanded },
            { "Trousers", EquippedTrousers }
        };

            WriteLine($@"
            Character name: {Name}
            Attack: {Attack}
            Defense: {Defense}
            Speed: {Speed}
            Weight: {Weight}
            Money: {Money}
            Health: {Health}
            Level: {Level}
            Experience: {Experience}");

            foreach (var equippedItem in equippedItems)
            {
                if (equippedItem.Value != null)
                {
                    WriteLine($@"
                Equipped {equippedItem.Key}: {equippedItem.Value.Name ?? "None"} 
                  ID: {equippedItem.Value.ID}, Defense: {equippedItem.Value.Defense}, Attack: {equippedItem.Value.Attack}, Weight: {equippedItem.Value.Weight}, Money worth: {equippedItem.Value.MoneyWorth}, Description: {equippedItem.Value.Description}");
                }
                else
                {
                    WriteLine($@"
                Equipped {equippedItem.Key}: None");
                }
            }
        }

        public void MovePlayer(string direction, Map map)
        {
            Room newRoom = map.MovePlayer(ref currentX, ref currentY, direction);
            WriteLine($"Moved {direction}. Current position: ({CurrentX}, {CurrentY})");
        }

        private void UpdateWeight()
        {
            Weight = CheckWeight();
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

        private void UpdateAttack()
        {
            baseAttack = CheckAttack();
        }

        public float CheckAttack()
        {
            return ((EquippedHelmet?.Attack ?? 0) +
                    (EquippedArmor?.Attack ?? 0) +
                    (EquippedShield?.Attack ?? 0) +
                    (EquippedGloves?.Attack ?? 0) +
                    (EquippedTrousers?.Attack ?? 0) +
                    (EquippedBoots?.Attack ?? 0) +
                    (EquippedAmulet?.Attack ?? 0) +
                    (EquippedSwordOneHanded?.Attack ?? 0) +
                    (EquippedSwordTwoHanded?.Attack ?? 0));
        }

        private void UpdateDefense()
        {
            baseDefense = CheckDefense();
        }

        public float CheckDefense()
        {
            return ((EquippedHelmet?.Defense ?? 0) +
                    (EquippedArmor?.Defense ?? 0) +
                    (EquippedShield?.Defense ?? 0) +
                    (EquippedGloves?.Defense ?? 0) +
                    (EquippedTrousers?.Defense ?? 0) +
                    (EquippedBoots?.Defense ?? 0) +
                    (EquippedAmulet?.Defense ?? 0) +
                    (EquippedSwordOneHanded?.Defense ?? 0) +
                    (EquippedSwordTwoHanded?.Defense ?? 0));
        }

        public void EquipItem(int itemId)
        {
            var item = Inventory.FirstOrDefault(i => i.ID == itemId);
            if (item == null)
            {
                throw new InvalidOperationException("Item not found in inventory.");
            }

            var itemType = item.GetType();
            var itemTypeAttribute = itemType.GetCustomAttribute<ItemTypeAttribute>();

            if (itemTypeAttribute != null)
            {
                if (item is SwordTwoHanded)
                {
                    UnequipItem(typeof(SwordOneHanded));
                    UnequipItem(typeof(Shield));
                }
                else if (item is SwordOneHanded || item is Shield)
                {
                    UnequipItem(typeof(SwordTwoHanded));
                }

                UnequipItem(itemType);

                var property = GetType().GetProperty($"Equipped{itemTypeAttribute.Kind}");
                if (property != null)
                {
                    property.SetValue(this, item);
                    UpdateWeight();
                    UpdateAttack();
                    UpdateDefense();
                }
                else
                {
                    throw new InvalidOperationException($"No equipped property found for {itemTypeAttribute.Kind}");
                }
            }
            else
            {
                throw new InvalidOperationException("Item type not supported");
            }
        }

        public void UnequipItem(Type itemType)
        {
            var itemTypeAttribute = itemType.GetCustomAttribute<ItemTypeAttribute>();
            if (itemTypeAttribute != null)
            {
                var property = GetType().GetProperty($"Equipped{itemTypeAttribute.Kind}");
                if (property != null)
                {
                    property.SetValue(this, null);
                    UpdateWeight();
                    UpdateDefense();
                    UpdateAttack();
                }
                else
                {
                    throw new InvalidOperationException($"No equipped property found for {itemTypeAttribute.Kind}");
                }
            }
            else
            {
                throw new InvalidOperationException("Item type not supported");
            }
        }

        public void DiscardItem(int itemId)
        {
            var item = Inventory.FirstOrDefault(i => i.ID == itemId);

            if (item == null)
            {
                WriteLine("Item not found in inventory.");
                return;
            }

            Inventory.Remove(item);

            if (EquippedAmulet?.ID == itemId) EquippedAmulet = null;
            if (EquippedArmor?.ID == itemId) EquippedArmor = null;
            if (EquippedBoots?.ID == itemId) EquippedBoots = null;
            if (EquippedGloves?.ID == itemId) EquippedGloves = null;
            if (EquippedHelmet?.ID == itemId) EquippedHelmet = null;
            if (EquippedShield?.ID == itemId) EquippedShield = null;
            if (EquippedSwordOneHanded?.ID == itemId) EquippedSwordOneHanded = null;
            if (EquippedSwordTwoHanded?.ID == itemId) EquippedSwordTwoHanded = null;
            if (EquippedTrousers?.ID == itemId) EquippedTrousers = null;

            UpdateWeight();
            UpdateAttack();
            UpdateDefense();

            WriteLine($"Item '{item.Name}' has been discarded.");
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

        public void CheckInventory()
        {
            WriteLine(" \n Here is your inventory: ");
            foreach (Item item in Inventory)
            {
                WriteLine($"This is: {item.Name} ID: {item.ID} Defense: {item.Defense} Attack: {item.Attack} Weight: {item.Weight} Money worth: {item.MoneyWorth} Description: {item.Description}");
            }
        }

        public void SaveGame(string filePath)
        {
            var gameState = new GameState
            {
                PlayerCharacter = this,
                Map = CurrentMap
            };

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                ReferenceHandler = ReferenceHandler.Preserve,
                Converters = { new ItemConverter(), new MapConverter() }
            };

            string jsonString = JsonSerializer.Serialize(gameState, options);
            File.WriteAllText(filePath, jsonString);
        }

        public static GameState LoadGame(string filePath)
        {
            if (File.Exists(filePath))
            {
                string jsonString = File.ReadAllText(filePath);

                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    Converters = { new ItemConverter(), new MapConverter() }
                };

                var gameState = JsonSerializer.Deserialize<GameState>(jsonString, options);

                if (gameState != null)
                {
                    gameState.PlayerCharacter.CurrentMap = gameState.Map;
                    ItemFactory.LastGeneratedItemId = gameState.PlayerCharacter.Inventory.Max(i => i.ID);
                }

                return gameState;
            }
            else
            {
                throw new FileNotFoundException("Save file not found.");
            }
        }

        // Methods to apply buffs/debuffs
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
                WriteLine("Item not found in inventory.");
                return;
            }

            Money += item.MoneyWorth;
            Inventory.Remove(item);

            UpdateWeight();
            UpdateAttack();
            UpdateDefense();

            WriteLine($"You sold {item.Name} for {item.MoneyWorth} coins.");
        }

        public void BuyHealthPotion()
        {

            if (Money < 40)
            {
                WriteLine("You don't have enough money to buy a health potion.");
                return;
            }

            Money -= 40;
            var healthPotion = ItemFactory.GenerateItem<HealthPotion>();
            Inventory.Add(healthPotion);

            UpdateWeight();
            WriteLine($"You bought a health potion for {40} coins.");
        }
    }
}