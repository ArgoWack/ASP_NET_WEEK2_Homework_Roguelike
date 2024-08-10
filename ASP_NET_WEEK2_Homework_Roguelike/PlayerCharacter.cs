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

namespace ASP_NET_WEEK2_Homework_Roguelike
{
    public class PlayerCharacter
    {
        private int currentX;
        private int currentY;

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

        private List<Item> inventory;
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
        public int HealthLimit => 100 + Level * 10;
        public float Speed => Weight / 100 + Level * 10;
        public float Attack { get; private set; }
        public float Defense { get; private set; }
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


        public PlayerCharacter()
        {
            Inventory = new List<Item>();
            Level = 1;
            Weight = 0;
            Attack = 0;
            Defense = 0;
            CurrentX = 0; // Starting X position
            CurrentY = 0; // Starting Y position
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
            Attack = CheckAttack();
        }

        public float CheckAttack()
        {
            // since the slot may be empty in case of null returns 0 for given slot
            return ((EquippedHelmet?.Attack ?? 0) +
                    (EquippedArmor?.Attack ?? 0) +
                    (EquippedShield?.Attack ?? 0) +
                    (EquippedGloves?.Attack ?? 0) +
                    (EquippedTrousers?.Attack ?? 0) +
                    (EquippedBoots?.Attack ?? 0) +
                    (EquippedAmulet?.Attack ?? 0) +
                    (EquippedSwordOneHanded?.Attack ?? 0) +
                    (EquippedSwordTwoHanded?.Attack ?? 0)) * Speed;
        }
        private void UpdateDefense()
        {
            Defense = CheckDefense();
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
                    (EquippedSwordTwoHanded?.Defense ?? 0)) * Speed;
        }

        public void EquipItem(int itemId)
        {
            // Find the item in the inventory by ID
            var item = Inventory.FirstOrDefault(i => i.ID == itemId);
            if (item == null)
            {
                throw new InvalidOperationException("Item not found in inventory.");
            }

            var itemType = item.GetType();
            var itemTypeAttribute = itemType.GetCustomAttribute<ItemTypeAttribute>();

            if (itemTypeAttribute != null)
            {
                // Special handling for swords and shields
                if (item is SwordTwoHanded)
                {
                    // Unequip one-handed sword and shield
                    UnequipItem(typeof(SwordOneHanded));
                    UnequipItem(typeof(Shield));
                }
                else if (item is SwordOneHanded || item is Shield)
                {
                    // Unequip two-handed sword
                    UnequipItem(typeof(SwordTwoHanded));
                }

                // Unequip the currently equipped item of the same type
                UnequipItem(itemType);

                // Equip the new item
                var property = GetType().GetProperty($"Equipped{itemTypeAttribute.Kind}");
                if (property != null)
                {
                    property.SetValue(this, item);
                    UpdateWeight();
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
        public void HealByPotion(HealthPotion HealthPotion)
        {
            Heal(HealthPotion.MoneyWorth);
            Inventory.Remove(HealthPotion);
        }
        public void Heal(int amount)
        {
            if (Health + amount <= HealthLimit)
                Health += amount;
            else
                Health = HealthLimit;
        }

        public void GetExperience(int amount)
        {
            if (Experience + amount > Level * 100)
                Experience += amount;
            else
            {
                Experience += amount - Level * 100;
                Level++;
                UpdateAttack();
                UpdateDefense();
            }
        }

        public void CheckInventory()
        {
            WriteLine(" \n Here is your inventory: ");
            foreach (Item item in Inventory)
            {
                WriteLine("This is: "+item.Name+" ID: "+item.ID+" Defense: "+ item.Defense + " Attack: " + item.Attack + " Weight: " + item.Weight + " Money worth: "+item.MoneyWorth+" Description: "+item.Description);
            }
        }

        public void SaveGame(string filePath, Map map)
        {
            var gameState = new GameState
            {
                PlayerCharacter = this,
                Map = map
            };

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            string jsonString = JsonSerializer.Serialize(gameState, options);
            File.WriteAllText(filePath, jsonString);
        }

        public static GameState LoadGame(string filePath)
        {
            if (File.Exists(filePath))
            {
                string jsonString = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<GameState>(jsonString);
            }
            else
            {
                throw new FileNotFoundException("Save file not found.");
            }
        }
    }
}
