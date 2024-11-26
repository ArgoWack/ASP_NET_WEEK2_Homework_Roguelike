using ASP_NET_WEEK2_Homework_Roguelike.Services;
using ASP_NET_WEEK2_Homework_Roguelike.Model.Items;
using ASP_NET_WEEK2_Homework_Roguelike.View;
using System.Text.Json.Serialization;

namespace ASP_NET_WEEK2_Homework_Roguelike.Model
{
    public class PlayerCharacter
    {
        public PlayerCharacter()
        {
            Inventory = new List<Item>();
        }

        private CharacterStatsService _statsService;
        private InventoryService _inventoryService;
        private EventService _eventService;
        private PlayerCharacterView _view;

        private int _currentX;
        private int _currentY;

        private float _baseSpeed;
        private float _baseAttack;
        private float _baseDefense;
        private float _speedModifier;
        private float _attackModifier;
        private float _defenseModifier;

        //for serialization
        public float SpeedModifier => _speedModifier;
        public float AttackModifier => _attackModifier;
        public float DefenseModifier => _defenseModifier;

        private List<Item> _inventory;

        // Properties
        public string Name { get; set; }
        public int Health { get; set; }
        public int Weight { get; private set; }
        public int Money { get; set; }
        public int Level { get; set; }
        public int Experience { get; set; }
        public Map CurrentMap { get; set; }
        public int CurrentX
        {
            get => _currentX;
            set => _currentX = value;
        }
        public int CurrentY
        {
            get => _currentY;
            set => _currentY = value;
        }
        public List<Item> Inventory
        {
            get => _inventory;
            set
            {
                _inventory = value;
                UpdateStats();
            }
        }
        // Equipped Items
        public Helmet EquippedHelmet { get; set; }
        public Armor EquippedArmor { get; set; }
        public Shield EquippedShield { get; set; }
        public Gloves EquippedGloves { get; set; }
        public Trousers EquippedTrousers { get; set; }
        public Boots EquippedBoots { get; set; }
        public Amulet EquippedAmulet { get; set; }
        public SwordOneHanded EquippedSwordOneHanded { get; set; }
        public SwordTwoHanded EquippedSwordTwoHanded { get; set; }

        // Derived Stats
        public int HealthLimit => 100 + Level * 10;
        public float Speed
        {
            get
            {
                float weightPenalty = Math.Max(0.5f, 1.0f - (Weight / 100.0f)); // Minimum speed penalty of 50%
                return _baseSpeed * weightPenalty;
            }
        }

        public float Attack
        {
            get
            {
                return _baseAttack * (Speed / 10.0f); // Scale attack by speed
            }
        }

        public float Defense
        {
            get
            {
                return _baseDefense * (Speed / 10.0f); // Scale defense by speed
            }
        }

        public PlayerCharacter(CharacterStatsService statsService, InventoryService inventoryService, EventService eventService, PlayerCharacterView view)
        {
            _statsService = statsService ?? throw new ArgumentNullException(nameof(statsService));
            _inventoryService = inventoryService ?? throw new ArgumentNullException(nameof(inventoryService));
            _eventService = eventService ?? throw new ArgumentNullException(nameof(eventService));
            _view = view ?? throw new ArgumentNullException(nameof(view));

            _inventory = new List<Item>();
            Level = 1;
            Health = HealthLimit;
            Money = 0;
            _currentX = 0;
            _currentY = 0;
            CurrentMap = new Map();

            _baseSpeed = Level * 10;
            UpdateStats();
        }
        //For deserialization
        public void InitializeServices(
            CharacterStatsService statsService,
            InventoryService inventoryService,
            EventService eventService,
            PlayerCharacterView view)
        {
            _statsService = statsService ?? throw new ArgumentNullException(nameof(statsService));
            _inventoryService = inventoryService ?? throw new ArgumentNullException(nameof(inventoryService));
            _eventService = eventService ?? throw new ArgumentNullException(nameof(eventService));
            _view = view ?? throw new ArgumentNullException(nameof(view));

            // Initialize default properties if needed
            Inventory ??= new List<Item>();
            EquippedHelmet ??= null;
            EquippedArmor ??= null;
            EquippedShield ??= null;

            UpdateStats(); // Recalculate stats
        }
        // Movement
        public void MovePlayer(string direction, Map map, MapService mapService)
        {
            int playerX = _currentX;
            int playerY = _currentY;
            mapService.MovePlayer(map, ref playerX, ref playerY, direction);
            _currentX = playerX;
            _currentY = playerY;
        }

        // Stats Updates
        public void UpdateStats()
        {
            if (_statsService == null)
            {
                return;
            }
            
            /*
            _baseAttack = _statsService.CalculateAttack(this);
            _baseDefense = _statsService.CalculateDefense(this);
            Weight = _statsService.CalculateWeight(this);
            */
            // Calculate speed with a weight penalty
            float weightPenalty = Math.Max(0.5f, 1.0f - (Weight / 100.0f)); // Minimum 50% speed
            _baseSpeed = 10.0f + Level * 2; // Base speed depends on level
            float adjustedSpeed = _baseSpeed * weightPenalty;

            // Calculate derived stats
            _baseAttack = _statsService.CalculateAttack(this) * (adjustedSpeed / 10.0f);
            _baseDefense = _statsService.CalculateDefense(this) * (adjustedSpeed / 10.0f);
            //Speed = adjustedSpeed;
            Weight = _statsService.CalculateWeight(this);

        }

        // Inventory Management
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
            var potion = _inventory.OfType<HealthPotion>().FirstOrDefault(p => p.Quantity > 0);
            if (potion == null)
                throw new InvalidOperationException("You don't have any health potions to sell.");

            Money += potion.MoneyWorth;
            potion.Quantity--;

            if (potion.Quantity == 0)
                _inventory.Remove(potion);

            Weight -= potion.Weight;
            UpdateStats();
        }
        public void ReceiveHealthPotion(int quantity = 1)
        {
            var existingPotion = _inventory.OfType<HealthPotion>().FirstOrDefault(p => p.Quantity < p.MaxStackSize);
            if (existingPotion != null)
            {
                int addable = Math.Min(existingPotion.MaxStackSize - existingPotion.Quantity, quantity);
                existingPotion.Quantity += addable;
                quantity -= addable;
            }
            while (quantity > 0)
            {
                var newPotion = ItemFactoryService.GenerateItem(typeof(HealthPotion), _view) as HealthPotion;
                if (newPotion == null) throw new InvalidOperationException("Failed to generate HealthPotion.");

                newPotion.Quantity = Math.Min(newPotion.MaxStackSize, quantity);
                _inventory.Add(newPotion);
                quantity -= newPotion.Quantity;
            }
            Weight = _statsService.CalculateWeight(this);
        }
        public void HealByPotion()
        {
            var potion = _inventory.OfType<HealthPotion>().FirstOrDefault(p => p.Quantity > 0);
            if (potion == null)
            {
                _eventService.HandleEventOutcome("You don't have any potions to use.");
                return;
            }
            Heal(potion.HealingAmount);
            potion.Quantity--;
            if (potion.Quantity == 0)
                _inventory.Remove(potion);

            Weight -= potion.Weight;
            UpdateStats();
        }

        // Healing
        public void Heal(int amount)
        {
            int previousHealth = Health;
            Health = Math.Min(Health + amount, HealthLimit);
            int healedAmount = Health - previousHealth;
            _eventService.HandleEventOutcome($"You healed for: {healedAmount} health points.");
        }

        // Experience and Leveling
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

        // Stat Modifiers
        public void ModifySpeed(float amount)
        {
            _speedModifier = Math.Max(0, _speedModifier + amount);
            UpdateStats();
        }

        public void ModifyAttack(float amount)
        {
            _attackModifier = Math.Max(0, _attackModifier + amount);
            UpdateStats();
        }

        public void ModifyDefense(float amount)
        {
            _defenseModifier = Math.Max(0, _defenseModifier + amount);
            UpdateStats();
        }

        // Item Transactions
        public void SellItem(int itemId)
        {
            var item = _inventory.FirstOrDefault(i => i.ID == itemId);
            if (item == null)
                throw new InvalidOperationException("Item not found in inventory.");

            if (item is HealthPotion potion)
            {
                Money += potion.MoneyWorth;
                potion.Quantity--;

                if (potion.Quantity == 0)
                    _inventory.Remove(potion);
            }
            else
            {
                Money += item.MoneyWorth;
                _inventory.Remove(item);
            }
            UpdateStats();
        }
        public void BuyHealthPotion()
        {
            if (Money < 40)
                throw new InvalidOperationException("You don't have enough money to buy a health potion.");
            Money -= 40;
            ReceiveHealthPotion();
            UpdateStats();
            _eventService.HandleEventOutcome("You bought a health potion for 40 coins.");
        }
    }
}