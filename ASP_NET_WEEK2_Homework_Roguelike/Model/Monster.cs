namespace ASP_NET_WEEK3_Homework_Roguelike.Model
{
    public class Monster
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int Level { get; set; }
        public Monster(string name, int baseHealth, int baseAttack, int baseDefense, int level)
        {
            Name = name;
            Level = level;
            Health = GenerateStatInRange(baseHealth);
            Attack = GenerateStatInRange(baseAttack);
            Defense = GenerateStatInRange(baseDefense);
        }
        private int GenerateStatInRange(int baseStat)
        {
            var random = new Random();
            double modifier = random.NextDouble() * 0.4 + 0.8; // generates a number between 0.8 and 1.2
            return (int)(baseStat * modifier);
        }
    }
}