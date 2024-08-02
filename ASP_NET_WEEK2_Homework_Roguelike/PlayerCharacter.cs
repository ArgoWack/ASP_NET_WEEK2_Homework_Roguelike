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
            Weight = 0;
            Attack = 0;
            Defense = 0;
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
            return (EquippedHelmet.Attack+ EquippedArmor.Attack+ EquippedShield.Attack+ EquippedGloves.Attack+ EquippedTrousers.Attack+ EquippedBoots.Attack+ EquippedAmulet.Attack+ EquippedSwordOneHanded.Attack+ EquippedSwordTwoHanded.Attack)*Speed;
        }
        private void UpdateDefense()
        {
            Defense = CheckDefense();
        }
        public float CheckDefense()
        {
            return (EquippedHelmet.Defense + EquippedArmor.Defense + EquippedShield.Defense + EquippedGloves.Defense + EquippedTrousers.Defense + EquippedBoots.Defense + EquippedAmulet.Defense + EquippedSwordOneHanded.Defense + EquippedSwordTwoHanded.Defense)*Speed;
        }

        public void EquipItem(Item item)
        {
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
            foreach (Item item in Inventory)
            {
                WriteLine("This is: "+item.Name+"Defense: "+ item.Defense + "Attack: " + item.Attack + "Weight: " + item.Weight + "Money worth: "+item.MoneyWorth+"Description: "+item.Description);
            }
        }

        public void SaveGame(string filePath)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true 
            };
            string jsonString = JsonSerializer.Serialize(this, options);
            File.WriteAllText(filePath, jsonString);
        }

        public static PlayerCharacter LoadGame(string filePath)
        {
            if (File.Exists(filePath))
            {
                string jsonString = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<PlayerCharacter>(jsonString);
            }
            else
            {
                throw new FileNotFoundException("Save file not found.");
            }
        }
    }
}
