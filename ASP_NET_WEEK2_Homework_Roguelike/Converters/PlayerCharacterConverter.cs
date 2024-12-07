using ASP_NET_WEEK3_Homework_Roguelike.Model.Items;
using ASP_NET_WEEK3_Homework_Roguelike.Model;
using System.Text.Json.Serialization;
using System.Text.Json;
public class PlayerCharacterConverter : JsonConverter<PlayerCharacter>
{
    public override PlayerCharacter Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        try
        {
            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException("Expected StartObject token");

            var playerCharacter = new PlayerCharacter();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                    return playerCharacter;

                if (reader.TokenType != JsonTokenType.PropertyName)
                    throw new JsonException("Expected PropertyName token");

                string propertyName = reader.GetString();
                reader.Read();

                switch (propertyName)
                {
                    case "Name":
                        playerCharacter.Name = reader.GetString();
                        break;

                    case "Health":
                        playerCharacter.Health = reader.GetInt32();
                        break;

                    case "Level":
                        playerCharacter.Level = reader.GetInt32();
                        break;

                    case "CurrentX":
                        playerCharacter.CurrentX = reader.GetInt32();
                        break;

                    case "CurrentY":
                        playerCharacter.CurrentY = reader.GetInt32();
                        break;

                    case "Money":
                        playerCharacter.Money = reader.GetInt32();
                        break;

                    case "Experience":
                        playerCharacter.Experience = reader.GetInt32();
                        break;

                    case "Inventory":
                        playerCharacter.Inventory = JsonSerializer.Deserialize<List<Item>>(ref reader, options);
                        break;

                    case "EquippedItems":
                        var equippedItems = JsonSerializer.Deserialize<Dictionary<string, Item>>(ref reader, options);
                        if (equippedItems != null)
                        {
                            foreach (var kvp in equippedItems)
                            {
                                if (Enum.TryParse(typeof(ItemType), kvp.Key, out var parsedType))
                                {
                                    playerCharacter.EquippedItems[(ItemType)parsedType] = kvp.Value;
                                }
                                else
                                {
                                    throw new JsonException($"Invalid ItemType: {kvp.Key}");
                                }
                            }
                        }
                        break;

                    default:
                        reader.Skip();
                        break;
                }
            }
            throw new JsonException("Unexpected end of JSON");
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to deserialize PlayerCharacter from JSON.", ex);
        }
    }
    public override void Write(Utf8JsonWriter writer, PlayerCharacter value, JsonSerializerOptions options)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value), "PlayerCharacter cannot be null.");
        }
        try
        {
            writer.WriteStartObject();

        // Write basic properties
        writer.WriteString("Name", value.Name);
        writer.WriteNumber("Health", value.Health);
        writer.WriteNumber("Level", value.Level);
        writer.WriteNumber("CurrentX", value.CurrentX);
        writer.WriteNumber("CurrentY", value.CurrentY);
        writer.WriteNumber("Money", value.Money);
        writer.WriteNumber("Experience", value.Experience);

        // Write Inventory
        writer.WritePropertyName("Inventory");
        writer.WriteStartArray();
        if (value.Inventory != null)
        {
            foreach (var item in value.Inventory)
            {
                writer.WriteStartObject();
                writer.WriteString("ItemType", item.Type.ToString());
                writer.WriteNumber("ID", item.ID);
                writer.WriteString("Name", item.Name);
                writer.WriteNumber("Weight", item.Weight);
                writer.WriteNumber("Defense", item.Defense);
                writer.WriteNumber("Attack", item.Attack);
                writer.WriteNumber("MoneyWorth", item.MoneyWorth);
                writer.WriteNumber("Quantity", item.Quantity);

                // Specific to HealthPotion
                if (item is HealthPotion potion)
                {
                    writer.WriteNumber("HealingAmount", potion.HealingAmount);
                    writer.WriteNumber("MaxStackSize", potion.MaxStackSize);
                }
                writer.WriteEndObject();
            }
        }
        writer.WriteEndArray();

        // Write EquippedItems
        writer.WritePropertyName("EquippedItems");
        writer.WriteStartObject();
        if (value.EquippedItems != null)
        {
            foreach (var kvp in value.EquippedItems)
            {
                writer.WritePropertyName(kvp.Key.ToString());
                if (kvp.Value != null)
                {
                    writer.WriteStartObject();
                    writer.WriteString("ItemType", kvp.Value.Type.ToString());
                    writer.WriteNumber("ID", kvp.Value.ID);
                    writer.WriteString("Name", kvp.Value.Name);
                    writer.WriteNumber("Weight", kvp.Value.Weight);
                    writer.WriteNumber("Defense", kvp.Value.Defense);
                    writer.WriteNumber("Attack", kvp.Value.Attack);
                    writer.WriteNumber("MoneyWorth", kvp.Value.MoneyWorth);
                    writer.WriteNumber("Quantity", kvp.Value.Quantity);
                    writer.WriteEndObject();
                }
                else
                {
                    writer.WriteNullValue();
                }
            }
        }
        // End object cycles (there are 2 nested)
        writer.WriteEndObject();
        writer.WriteEndObject();
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to serialize PlayerCharacter to JSON.", ex);
        }
    }
}