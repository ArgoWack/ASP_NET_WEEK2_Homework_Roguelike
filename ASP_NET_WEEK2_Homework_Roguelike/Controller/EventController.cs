using ASP_NET_WEEK2_Homework_Roguelike.Model;
using ASP_NET_WEEK2_Homework_Roguelike.Model.Events;
using ASP_NET_WEEK2_Homework_Roguelike.Services;

namespace ASP_NET_WEEK2_Homework_Roguelike.Controller
{
    public class EventController
    {
        private readonly PlayerCharacterController _playerController;
        private readonly EventService _eventService;

        public EventController(PlayerCharacterController playerController, EventService eventService)
        {
            _playerController = playerController;
            _eventService = eventService;
        }

        public void ExecuteEvent(Room room)
        {
            if (room == null || room.EventStatus == "none") return;

            // Generate the event based on the room's EventStatus and execute it
            var randomEvent = EventGenerator.GenerateEvent(room.EventStatus);

            if (randomEvent != null)
            {
                try
                {
                    randomEvent.Execute(_playerController.PlayerCharacter, room, _playerController);
                    _eventService.HandleEventOutcome($"Event {room.EventStatus} has been handled.");
                }
                catch (Exception ex)
                {
                    _eventService.HandleEventOutcome($"An error occurred during event execution: {ex.Message}");
                }
            }
            else
            {
                _eventService.HandleEventOutcome("No valid event found for this room.");
            }

            room.EventStatus = "none"; // Clear event status after handling
        }
    }
}