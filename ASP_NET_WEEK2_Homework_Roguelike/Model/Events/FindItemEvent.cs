using ASP_NET_WEEK3_Homework_Roguelike.Services;
using ASP_NET_WEEK3_Homework_Roguelike.Controller;
using ASP_NET_WEEK3_Homework_Roguelike.View;

namespace ASP_NET_WEEK3_Homework_Roguelike.Model.Events
{
    public class FindItemEvent : RandomEvent
    {
        private readonly CharacterInteractionService _interactionService;
        private readonly EventService _eventService;
        private readonly PlayerCharacterView _view;
        public FindItemEvent(EventService eventService, CharacterInteractionService interactionService, PlayerCharacterView view)
        {
            _eventService = eventService ?? throw new ArgumentNullException(nameof(eventService));
            _interactionService = interactionService ?? throw new ArgumentNullException(nameof(interactionService));
            _view = view ?? throw new ArgumentNullException(nameof(view));
        }
        public override void Execute(PlayerCharacter player, Room room, PlayerCharacterController controller)
        {
            if (player == null || room == null)
                throw new ArgumentNullException("Player or Room cannot be null.");

            try
            {
                if (new Random().Next(100) < 34) // 34% chance to find health potions
                {
                    int potionCount = new Random().Next(1, 5);
                    player.ReceiveHealthPotion(potionCount);
                    _eventService.HandleEventOutcome($"You have found {potionCount} Health Potions.");
                }
                else
                {
                    var item = ItemFactoryService.GenerateRandomItem(_view);
                    if (item != null)
                    {
                        _eventService.HandleEventOutcome($"You have found an item: {item.Name}.");
                        string choice = _eventService.PromptForItemPickup();
                        if (choice.ToLower() == "y")
                        {
                            player.Inventory.Add(item);
                            _eventService.HandleEventOutcome($"You have taken the item: {item.Name}.");
                        }
                        else
                        {
                            _eventService.HandleEventOutcome($"You left the item: {item.Name}.");
                        }
                    }
                    else
                    {
                        _eventService.HandleEventOutcome("No valid item was generated.");
                    }
                }
            }
            catch (Exception ex)
            {
                _eventService.HandleEventOutcome($"Error during FindItemEvent: {ex.Message}");
            }
            finally
            {
                room.EventStatus = "none";
            }
        }
    }
}