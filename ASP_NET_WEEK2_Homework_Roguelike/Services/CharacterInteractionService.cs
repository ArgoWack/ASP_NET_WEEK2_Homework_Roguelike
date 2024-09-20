using ASP_NET_WEEK2_Homework_Roguelike.Model;
using System;

namespace ASP_NET_WEEK2_Homework_Roguelike.Services
{
    public class CharacterInteractionService
    {
        private readonly EventService _eventService;

        public CharacterInteractionService(EventService eventService)
        {
            _eventService = eventService;
        }

        public void ModifyPlayerStats(PlayerCharacter player, string statType, float amount)
        {
            switch (statType.ToLower())
            {
                case "speed":
                    player.ModifySpeed(amount);
                    _eventService.HandleEventOutcome($"Your speed has been modified by {amount}.");
                    break;
                case "attack":
                    player.ModifyAttack(amount);
                    _eventService.HandleEventOutcome($"Your attack has been modified by {amount}.");
                    break;
                case "defense":
                    player.ModifyDefense(amount);
                    _eventService.HandleEventOutcome($"Your defense has been modified by {amount}.");
                    break;
                default:
                    throw new InvalidOperationException("Invalid stat type.");
            }
        }

        public void HealPlayer(PlayerCharacter player, int amount)
        {
            player.Heal(amount);
            _eventService.HandleEventOutcome($"You have been healed by {amount} health points.");
        }

        public void HandleInventoryInteraction(PlayerCharacter player, int itemId, string action)
        {
            switch (action.ToLower())
            {
                case "equip":
                    player.EquipItem(itemId);
                    _eventService.HandleEventOutcome($"Item {itemId} has been equipped.");
                    break;
                case "discard":
                    player.DiscardItem(itemId);
                    _eventService.HandleEventOutcome($"Item {itemId} has been discarded.");
                    break;
                default:
                    throw new InvalidOperationException("Invalid inventory action.");
            }
        }
    }
}