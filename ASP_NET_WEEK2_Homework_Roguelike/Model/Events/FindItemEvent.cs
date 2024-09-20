using ASP_NET_WEEK2_Homework_Roguelike.Services;
using ASP_NET_WEEK2_Homework_Roguelike.Model;
using ASP_NET_WEEK2_Homework_Roguelike.Controller;
using ASP_NET_WEEK2_Homework_Roguelike.Model.Items;

namespace ASP_NET_WEEK2_Homework_Roguelike.Model.Events
{
    public class FindItemEvent : RandomEvent
    {
        private readonly CharacterInteractionService _interactionService;
        private readonly EventService _eventService;

        public FindItemEvent(EventService eventService, CharacterInteractionService interactionService)
        {
            _eventService = eventService ?? throw new ArgumentNullException(nameof(eventService));
            _interactionService = interactionService ?? throw new ArgumentNullException(nameof(interactionService));
        }

        public override void Execute(PlayerCharacter player, Room room, PlayerCharacterController controller)
        {
            if (player == null || room == null)
                throw new ArgumentNullException("Player or Room cannot be null.");

            var item = ItemFactoryService.GenerateItem<SwordOneHanded>();
            _eventService.HandleEventOutcome($"You have found an item: {item.Name}.");

            string choice = _eventService.PromptForItemPickup();

            if (choice.ToLower() == "y")
            {
                player.Inventory.Add(item);
                _eventService.HandleEventOutcome($"You have taken the item: {item.Name}.");
            }
            else
            {
                _eventService.HandleEventOutcome($"You have left the item: {item.Name}.");
            }

            room.EventStatus = "none";
        }
    }
}