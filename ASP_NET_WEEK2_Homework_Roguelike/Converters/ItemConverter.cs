using System.Text.Json.Serialization;
using System.Text.Json;
using ASP_NET_WEEK3_Homework_Roguelike.Model.Items;

namespace ASP_NET_WEEK3_Homework_Roguelike.Converters
{
    public class ItemConverter : JsonConverter<Item>
    {
        public override Item Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
            {
                var jsonObject = doc.RootElement;
                var itemType = jsonObject.GetProperty("ItemType").GetString();
                Item item = itemType switch
                {
                    "Amulet" => JsonSerializer.Deserialize<Amulet>(jsonObject.GetRawText(), options),
                    "Armor" => JsonSerializer.Deserialize<Armor>(jsonObject.GetRawText(), options),
                    "Boots" => JsonSerializer.Deserialize<Boots>(jsonObject.GetRawText(), options),
                    "Gloves" => JsonSerializer.Deserialize<Gloves>(jsonObject.GetRawText(), options),
                    "Helmet" => JsonSerializer.Deserialize<Helmet>(jsonObject.GetRawText(), options),
                    "Shield" => JsonSerializer.Deserialize<Shield>(jsonObject.GetRawText(), options),
                    "SwordOneHanded" => JsonSerializer.Deserialize<SwordOneHanded>(jsonObject.GetRawText(), options),
                    "SwordTwoHanded" => JsonSerializer.Deserialize<SwordTwoHanded>(jsonObject.GetRawText(), options),
                    "Trousers" => JsonSerializer.Deserialize<Trousers>(jsonObject.GetRawText(), options),
                    "HealthPotion" => DeserializeHealthPotion(jsonObject, options),
                    _ => throw new NotSupportedException($"Item type '{itemType}' is not supported.")
                };
                return item;
            }
        }
        public override void Write(Utf8JsonWriter writer, Item value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
        private HealthPotion DeserializeHealthPotion(JsonElement jsonObject, JsonSerializerOptions options)
        {
            return new HealthPotion
            {
                ID = jsonObject.GetProperty("ID").GetInt32(),
                Name = jsonObject.GetProperty("Name").GetString(),
                Weight = jsonObject.GetProperty("Weight").GetInt32(),
                MoneyWorth = jsonObject.GetProperty("MoneyWorth").GetInt32(),
                Quantity = jsonObject.GetProperty("Quantity").GetInt32(),
                MaxStackSize = jsonObject.GetProperty("MaxStackSize").GetInt32(),
                HealingAmount = jsonObject.GetProperty("HealingAmount").GetInt32(),
                Description = jsonObject.GetProperty("Description").GetString()
            };
        }
    }
}