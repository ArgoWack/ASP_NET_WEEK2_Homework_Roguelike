using ASP_NET_WEEK2_Homework_Roguelike.Events;
using ASP_NET_WEEK2_Homework_Roguelike.View;

namespace ASP_NET_WEEK2_Homework_Roguelike.Controller
{
    public class EventController
    {
        private readonly PlayerCharacterController _playerController;
        private readonly PlayerCharacterView _view;

        public EventController(PlayerCharacterController playerController)
        {
            _playerController = playerController;
            _view = new PlayerCharacterView();
        }

        public void ExecuteEvent(Room room)
        {
            if (room.EventStatus == "none") return;

            // Generate and execute the event based on the room's EventStatus
            var randomEvent = EventGenerator.GenerateEvent(room.EventStatus);
            randomEvent?.Execute(_playerController.PlayerCharacter, room, _playerController);

            // Display message after event execution
            _view.ShowEventOutcome($"Event {room.EventStatus} has been handled.");
            room.EventStatus = "none"; // Clear event status after handling
        }
    }
}