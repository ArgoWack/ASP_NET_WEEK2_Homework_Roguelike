using ASP_NET_WEEK3_Homework_Roguelike.Model.Items;
using System.Text.Json;
using System.Text.Json.Serialization;
public class ItemConverter : JsonConverter<Item>
{
    public override Item Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using JsonDocument document = JsonDocument.ParseValue(ref reader);
        JsonElement root = document.RootElement;

        // validates that ItemType exists
        if (!root.TryGetProperty("ItemType", out JsonElement itemTypeElement))
            throw new JsonException("Missing 'ItemType' property in JSON.");

        string itemTypeName = itemTypeElement.GetString();
        if (!Enum.TryParse<ItemType>(itemTypeName, out ItemType itemType))
            throw new JsonException($"Unknown or unsupported ItemType: {itemTypeName}");

        // handles HealthPotion as a special case
        if (itemType == ItemType.HealthPotion)
        {
            return new HealthPotion
            {
                Type = itemType,
                ID = root.GetProperty("ID").GetInt32(),
                Name = root.GetProperty("Name").GetString(),
                Weight = root.GetProperty("Weight").GetInt32(),
                MoneyWorth = root.GetProperty("MoneyWorth").GetInt32(),
                Quantity = root.GetProperty("Quantity").GetInt32(),
                MaxStackSize = root.GetProperty("MaxStackSize").GetInt32(),
                HealingAmount = root.GetProperty("HealingAmount").GetInt32(),
                Description = root.TryGetProperty("Description", out var desc) ? desc.GetString() : null
            };
        }

        // generic case for other items
        var item = new Item
        {
            Type = itemType
        };
        foreach (var property in root.EnumerateObject())
        {
            if (property.NameEquals("ItemType"))
                continue;

            var propertyInfo = typeof(Item).GetProperty(property.Name);
            if (propertyInfo != null)
            {
                object value = JsonSerializer.Deserialize(property.Value.GetRawText(), propertyInfo.PropertyType, options);
                propertyInfo.SetValue(item, value);
            }
        }
        return item;
    }
    public override void Write(Utf8JsonWriter writer, Item value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("ItemType", value.Type.ToString());

        // serializes the rest of the properties dynamically
        foreach (var property in typeof(Item).GetProperties())
        {
            object propertyValue = property.GetValue(value);
            if (propertyValue != null)
            {
                writer.WritePropertyName(property.Name);
                JsonSerializer.Serialize(writer, propertyValue, property.PropertyType, options);
            }
        }
        writer.WriteEndObject();
    }
}