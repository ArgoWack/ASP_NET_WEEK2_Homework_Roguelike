using ASP_NET_WEEK3_Homework_Roguelike.Model;
using ASP_NET_WEEK3_Homework_Roguelike.Model.Events;
using ASP_NET_WEEK3_Homework_Roguelike.Services;

namespace ASP_NET_WEEK3_Homework_Roguelike.Controller
{
    public class EventController : IEventController
    {
        private readonly IPlayerCharacterController _playerController;
        private readonly IEventService _eventService;

        public EventController(IPlayerCharacterController playerController, IEventService eventService)
        {
            _playerController = playerController ?? throw new ArgumentNullException(nameof(playerController));
            _eventService = eventService ?? throw new ArgumentNullException(nameof(eventService));
        }

        public void ExecuteEvent(Room room)
        {
            if (room == null || string.IsNullOrEmpty(room.EventStatus) || room.EventStatus == "none")
                return;

            var randomEvent = EventGenerator.GenerateEvent(room.EventStatus);
            if (randomEvent == null)
            {
                _eventService.HandleEventOutcome($"No valid event found for status: {room.EventStatus}");
                return;
            }

            try
            {
                randomEvent.Execute(_playerController.PlayerCharacter, room, (PlayerCharacterController)_playerController);
                _eventService.HandleEventOutcome($"Event '{room.EventStatus}' executed successfully.");
            }
            catch (Exception ex)
            {
                _eventService.HandleEventOutcome($"Error during event execution: {ex.Message}");
            }

            room.EventStatus = "none";
        }
    }
}