using ASP_NET_WEEK2_Homework_Roguelike.Services;
using ASP_NET_WEEK2_Homework_Roguelike.View;

namespace ASP_NET_WEEK2_Homework_Roguelike.Model.Events
{
    public static class EventGenerator
    {
        private static EventService _eventService;
        private static CharacterInteractionService _interactionService;
        private static GameView _gameView;
        private static PlayerCharacterView _playerCharacterView;
        private static readonly Random random = new Random();
        public static void Initialize(EventService eventService, CharacterInteractionService interactionService, GameView gameView, PlayerCharacterView playerCharacterView)
        {
            _eventService = eventService ?? throw new ArgumentNullException(nameof(eventService));
            _interactionService = interactionService ?? throw new ArgumentNullException(nameof(interactionService));
            _gameView = gameView ?? throw new ArgumentNullException(nameof(gameView));
            _playerCharacterView = playerCharacterView ?? throw new ArgumentNullException(nameof(playerCharacterView));
        }
        public static RandomEvent GenerateEvent()
        {
            int roll = random.Next(100);
            if (roll < 25)
                return new FindItemEvent(_eventService, _interactionService, _playerCharacterView);
            else if (roll < 75)
                return new MonsterEvent(_eventService, _interactionService, _playerCharacterView);
            else if (roll < 85)
                return new DialogEvent(_eventService, _interactionService, _gameView);
            else
                return null; // no event occurs
        }
        public static RandomEvent GenerateEvent(string eventStatus)
        {
            return eventStatus switch
            {
                "FindItemEvent" => new FindItemEvent(_eventService, _interactionService, _playerCharacterView),
                "MonsterEvent" => new MonsterEvent(_eventService, _interactionService, _playerCharacterView),
                "DialogEvent" => new DialogEvent(_eventService, _interactionService, _gameView),
                _ => null, // no valid event
            };
        }
    }
}