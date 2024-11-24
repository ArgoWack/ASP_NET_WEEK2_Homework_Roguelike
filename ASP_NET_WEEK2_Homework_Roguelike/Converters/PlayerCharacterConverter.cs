using ASP_NET_WEEK2_Homework_Roguelike.Model.Items;
using ASP_NET_WEEK2_Homework_Roguelike.Model;
using System.Text.Json.Serialization;
using System.Text.Json;
using ASP_NET_WEEK2_Homework_Roguelike.Services;
using ASP_NET_WEEK2_Homework_Roguelike.View;


namespace ASP_NET_WEEK2_Homework_Roguelike.Converters
{
    public class PlayerCharacterConverter : JsonConverter<PlayerCharacter>
    {
        public override PlayerCharacter? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Console.WriteLine("PlayerCharacterConverter: Starting deserialization.");
            var jsonDocument = JsonDocument.ParseValue(ref reader);
            var jsonObject = jsonDocument.RootElement;

            var playerCharacter = new PlayerCharacter
            {
                Name = jsonObject.GetProperty("Name").GetString(),
                Health = jsonObject.GetProperty("Health").GetInt32(),
                Level = jsonObject.GetProperty("Level").GetInt32(),
                CurrentX = jsonObject.GetProperty("CurrentX").GetInt32(),
                CurrentY = jsonObject.GetProperty("CurrentY").GetInt32(),
                Money = jsonObject.GetProperty("Money").GetInt32(),
                Experience = jsonObject.GetProperty("Experience").GetInt32(),
            };

            // Deserialize Inventory
            if (jsonObject.TryGetProperty("Inventory", out var inventoryElement))
            {
                playerCharacter.Inventory = JsonSerializer.Deserialize<List<Item>>(inventoryElement.GetRawText(), options) ?? new List<Item>();
            }

            // Deserialize Equipped Items
            playerCharacter.EquippedHelmet = DeserializeEquippedItem<Helmet>(jsonObject, "EquippedHelmet", options);
            playerCharacter.EquippedArmor = DeserializeEquippedItem<Armor>(jsonObject, "EquippedArmor", options);
            playerCharacter.EquippedShield = DeserializeEquippedItem<Shield>(jsonObject, "EquippedShield", options);

            // Deserialize Map
            if (jsonObject.TryGetProperty("CurrentMap", out var mapElement))
            {
                try
                {
                    playerCharacter.CurrentMap = JsonSerializer.Deserialize<Map>(mapElement.GetRawText(), options);
                    Console.WriteLine($"Map deserialized. DiscoveredRooms count: {playerCharacter.CurrentMap?.DiscoveredRooms.Count ?? 0}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to deserialize map: {ex.Message}");
                    playerCharacter.CurrentMap = new Map(); // Fallback to an empty map
                }
            }

            Console.WriteLine("PlayerCharacterConverter: Deserialization complete.");
            return playerCharacter;
        }

        private T? DeserializeEquippedItem<T>(JsonElement jsonObject, string propertyName, JsonSerializerOptions options) where T : Item
        {
            if (jsonObject.TryGetProperty(propertyName, out var propertyElement) && propertyElement.ValueKind != JsonValueKind.Null)
            {
                try
                {
                    return JsonSerializer.Deserialize<T>(propertyElement.GetRawText(), options);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to deserialize {propertyName}: {ex.Message}");
                }
            }
            return null;
        }

        public override void Write(Utf8JsonWriter writer, PlayerCharacter value, JsonSerializerOptions options)
        {
            Console.WriteLine("PlayerCharacterConverter: Starting serialization.");
            writer.WriteStartObject();

            // Serialize basic properties
            writer.WriteString("Name", value.Name);
            writer.WriteNumber("Health", value.Health);
            writer.WriteNumber("Level", value.Level);
            writer.WriteNumber("CurrentX", value.CurrentX);
            writer.WriteNumber("CurrentY", value.CurrentY);
            writer.WriteNumber("Money", value.Money);
            writer.WriteNumber("Experience", value.Experience);

            // Serialize inventory
            writer.WritePropertyName("Inventory");
            JsonSerializer.Serialize(writer, value.Inventory, new JsonSerializerOptions
            {
                Converters = { new ItemConverter() }
            });

            // Serialize equipped items
            writer.WritePropertyName("EquippedHelmet");
            JsonSerializer.Serialize(writer, value.EquippedHelmet, options);

            writer.WritePropertyName("EquippedArmor");
            JsonSerializer.Serialize(writer, value.EquippedArmor, options);

            writer.WritePropertyName("EquippedShield");
            JsonSerializer.Serialize(writer, value.EquippedShield, options);

            writer.WritePropertyName("EquippedAmulet");
            JsonSerializer.Serialize(writer, value.EquippedAmulet, options);

            writer.WritePropertyName("EquippedSwordOneHanded");
            JsonSerializer.Serialize(writer, value.EquippedSwordOneHanded, options);

            writer.WritePropertyName("EquippedSwordTwoHanded");
            JsonSerializer.Serialize(writer, value.EquippedSwordTwoHanded, options);

            writer.WriteEndObject();
            Console.WriteLine("PlayerCharacterConverter: Serialization complete.");
        }
    }
}