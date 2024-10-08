﻿using ASP_NET_WEEK2_Homework_Roguelike.Model;
using ASP_NET_WEEK2_Homework_Roguelike.Model.Items;
using System;
using System.Collections.Generic;
using static System.Console;

namespace ASP_NET_WEEK2_Homework_Roguelike.View
{
    public class PlayerCharacterView
    {
        public void ShowMap(Map map, PlayerCharacter player)
        {
            var minX = map.DiscoveredRooms.Keys.Min(k => k.Item1);
            var maxX = map.DiscoveredRooms.Keys.Max(k => k.Item1);
            var minY = map.DiscoveredRooms.Keys.Min(k => k.Item2);
            var maxY = map.DiscoveredRooms.Keys.Max(k => k.Item2);

            WriteLine();
            for (int y = maxY; y >= minY; y--)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    if (map.DiscoveredRooms.TryGetValue((x, y), out Room room))
                    {
                        if (player.CurrentX == x && player.CurrentY == y)
                        {
                            Write("P"); // Player's current position
                        }
                        else
                        {
                            Write("+"); // Discovered room
                        }
                    }
                    else
                    {
                        Write(" "); // Undiscovered room
                    }

                    // Draw horizontal connections
                    if (x < maxX && map.DiscoveredRooms.TryGetValue((x, y), out Room currentRoom) && currentRoom.Exits.ContainsKey("east"))
                    {
                        Write("-");
                    }
                    else
                    {
                        Write(" ");
                    }
                }
                WriteLine();

                // Draw vertical connections
                if (y > minY)
                {
                    for (int x = minX; x <= maxX; x++)
                    {
                        if (map.DiscoveredRooms.TryGetValue((x, y), out Room room) && room.Exits.ContainsKey("south"))
                        {
                            Write("|");
                        }
                        else
                        {
                            Write(" ");
                        }

                        // Space between columns
                        if (x < maxX)
                        {
                            Write(" ");
                        }
                    }
                    WriteLine();
                }
            }
        }

        public void ShowCharacterStats(PlayerCharacter player)
        {
            WriteLine($@"
        Character name: {player.Name}
        Attack: {player.Attack}
        Defense: {player.Defense}
        Speed: {player.Speed}
        Weight: {player.Weight}
        Money: {player.Money}
        Health: {player.Health}
        Level: {player.Level}
        Experience: {player.Experience}
        ");

            var equippedItems = new Dictionary<string, Item>
            {
                { "Amulet", player.EquippedAmulet },
                { "Armor", player.EquippedArmor },
                { "Boots", player.EquippedBoots },
                { "Gloves", player.EquippedGloves },
                { "Helmet", player.EquippedHelmet },
                { "Shield", player.EquippedShield },
                { "SwordOneHanded", player.EquippedSwordOneHanded },
                { "SwordTwoHanded", player.EquippedSwordTwoHanded },
                { "Trousers", player.EquippedTrousers }
            };

            foreach (var equippedItem in equippedItems)
            {
                if (equippedItem.Value != null)
                {
                    WriteLine($@"
                Equipped {equippedItem.Key}: {equippedItem.Value.Name ?? "None"} 
                  ID: {equippedItem.Value.ID}, Defense: {equippedItem.Value.Defense}, Attack: {equippedItem.Value.Attack}, Weight: {equippedItem.Value.Weight}, Money worth: {equippedItem.Value.MoneyWorth}, Description: {equippedItem.Value.Description}");
                }
                else
                {
                    WriteLine($@"
                Equipped {equippedItem.Key}: None");
                }
            }
        }

        public void DisplayInventory(PlayerCharacter player)
        {
            WriteLine(" \nHere is your inventory: ");
            if (!player.Inventory.Any())
            {
                WriteLine("Your inventory is empty.");
                return;
            }

            foreach (Item item in player.Inventory)
            {
                WriteLine($"Item: {item.Name} | ID: {item.ID} | Defense: {item.Defense} | Attack: {item.Attack} | Weight: {item.Weight} | Value: {item.MoneyWorth} coins");
            }
        }

        public void ShowEquipItemSuccess(string itemName)
        {
            WriteLine($"You have equipped {itemName}.");
        }

        public void ShowDiscardItemSuccess(string itemName)
        {
            WriteLine($"Item '{itemName}' has been discarded.");
        }

        public void ShowPlayerMovement(string direction, int currentX, int currentY)
        {
            WriteLine($"Moved {direction}. Current position: ({currentX}, {currentY})");
        }

        public void ShowEventEncounter(string eventType)
        {
            WriteLine($"You encounter a {eventType}!");
        }

        public void ShowEventOutcome(string outcome)
        {
            WriteLine(outcome);
        }

        public void ShowError(string message)
        {
            WriteLine($"Error: {message}");
        }

        public void ShowBuyHealthPotionSuccess()
        {
            WriteLine("You bought a health potion for 40 coins.");
        }

        public void ShowSellItemSuccess(string itemName, int price)
        {
            WriteLine($"You sold {itemName} for {price} coins.");
        }

        public void ShowErrorCannotMove()
        {
            WriteLine("\nYou cannot move in that direction. There is no room.");
        }

        public string ShowMerchantOptions()
        {
            WriteLine("\nWrite: \nb. Buy health potion for 40 \ns. Sell an item \nl. Leave");
            return ReadLine().ToLower();
        }

        public int? PromptForItemIdToSell()
        {
            WriteLine("Enter the ID of the item you want to sell:");
            if (int.TryParse(ReadLine(), out int itemId))
            {
                return itemId;
            }
            return null;
        }

        public void ShowErrorInvalidItemId()
        {
            WriteLine("Invalid item ID.");
        }

        public void ShowErrorInvalidChoice()
        {
            WriteLine("Invalid choice. Please choose 'b', 's', or 'l'.");
        }

        public void ShowMerchantLeaveMessage()
        {
            WriteLine("The merchant nods and moves on.");
        }

        public string PromptForItemPickup()
        {
            WriteLine("Would you like to take it? (y/n)");
            return ReadLine();
        }

        public string ShowMonsterOptions()
        {
            WriteLine("\nf. Fight \nh. Heal \nl. Leave/Flee");
            return ReadLine().ToLower();
        }

        public void ShowFleeMessage()
        {
            WriteLine("You flee from the monster.");
        }
    }
}