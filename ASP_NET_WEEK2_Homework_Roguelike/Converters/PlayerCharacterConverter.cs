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

            // Deserialize stat modifiers
            if (jsonObject.TryGetProperty("SpeedModifier", out var speedModifierElement))
                playerCharacter.ModifySpeed(speedModifierElement.GetSingle());

            if (jsonObject.TryGetProperty("AttackModifier", out var attackModifierElement))
                playerCharacter.ModifyAttack(attackModifierElement.GetSingle());

            if (jsonObject.TryGetProperty("DefenseModifier", out var defenseModifierElement))
                playerCharacter.ModifyDefense(defenseModifierElement.GetSingle());

            // Deserialize inventory
            if (jsonObject.TryGetProperty("Inventory", out var inventoryElement))
            {
                playerCharacter.Inventory = JsonSerializer.Deserialize<List<Item>>(inventoryElement.GetRawText(), options) ?? new List<Item>();
            }

            // Deserialize equipped items
            playerCharacter.EquippedHelmet = DeserializeEquippedItem<Helmet>(jsonObject, "EquippedHelmet", options);
            playerCharacter.EquippedArmor = DeserializeEquippedItem<Armor>(jsonObject, "EquippedArmor", options);
            playerCharacter.EquippedShield = DeserializeEquippedItem<Shield>(jsonObject, "EquippedShield", options);
            playerCharacter.EquippedGloves = DeserializeEquippedItem<Gloves>(jsonObject, "EquippedGloves", options);
            playerCharacter.EquippedTrousers = DeserializeEquippedItem<Trousers>(jsonObject, "EquippedTrousers", options);
            playerCharacter.EquippedBoots = DeserializeEquippedItem<Boots>(jsonObject, "EquippedBoots", options);
            playerCharacter.EquippedAmulet = DeserializeEquippedItem<Amulet>(jsonObject, "EquippedAmulet", options);
            playerCharacter.EquippedSwordOneHanded = DeserializeEquippedItem<SwordOneHanded>(jsonObject, "EquippedSwordOneHanded", options);
            playerCharacter.EquippedSwordTwoHanded = DeserializeEquippedItem<SwordTwoHanded>(jsonObject, "EquippedSwordTwoHanded", options);

            // Deserialize map
            if (jsonObject.TryGetProperty("CurrentMap", out var mapElement))
            {
                try
                {
                    playerCharacter.CurrentMap = JsonSerializer.Deserialize<Map>(mapElement.GetRawText(), options);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to deserialize map: {ex.Message}");
                    playerCharacter.CurrentMap = new Map(); // Fallback to an empty map
                }
            }
            return playerCharacter;
        }

        private T? DeserializeEquippedItem<T>(JsonElement jsonObject, string propertyName, JsonSerializerOptions options) where T : Item
        {
            if (jsonObject.TryGetProperty(propertyName, out var propertyElement) && propertyElement.ValueKind != JsonValueKind.Null)
            {
                try
                {
                    var item = JsonSerializer.Deserialize<T>(propertyElement.GetRawText(), options);
                    return item;
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
            writer.WriteStartObject();

            // Serialize basic properties
            writer.WriteString("Name", value.Name);
            writer.WriteNumber("Health", value.Health);
            writer.WriteNumber("Level", value.Level);
            writer.WriteNumber("CurrentX", value.CurrentX);
            writer.WriteNumber("CurrentY", value.CurrentY);
            writer.WriteNumber("Money", value.Money);
            writer.WriteNumber("Experience", value.Experience);

            // Serialize stat modifiers
            writer.WriteNumber("SpeedModifier", value.SpeedModifier);
            writer.WriteNumber("AttackModifier", value.AttackModifier);
            writer.WriteNumber("DefenseModifier", value.DefenseModifier);

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
        }
    }
}