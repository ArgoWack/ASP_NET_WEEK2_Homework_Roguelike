
namespace ASP_NET_WEEK3_Homework_Roguelike.Model
{
    public class GameState
    {
        public PlayerCharacter PlayerCharacter { get; set; }
        public Map Map { get; set; }
        public int LastGeneratedItemId { get; set; }
    }
}