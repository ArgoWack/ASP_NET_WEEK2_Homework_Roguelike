using System.Text.Json;
using System.Text.Json.Serialization;
using ASP_NET_WEEK3_Homework_Roguelike.Model;

public class MapConverter : JsonConverter<Map>
{
    public override Map Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var map = new Map();
        var discoveredRooms = new Dictionary<(int, int), Room>();

        using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
        {
            var jsonObject = doc.RootElement;

            // Deserialize DiscoveredRooms
            if (jsonObject.TryGetProperty("DiscoveredRooms", out var roomsElement))
            {
                foreach (var roomElement in roomsElement.EnumerateObject())
                {
                    if (roomElement.Name.StartsWith("$"))
                        continue; // Skip metadata

                    var coordinates = roomElement.Name.Split(',');
                    var key = (int.Parse(coordinates[0]), int.Parse(coordinates[1]));

                    var room = JsonSerializer.Deserialize<Room>(roomElement.Value.GetRawText(), options);
                    if (room != null)
                    {
                        discoveredRooms[key] = room;
                    }
                }
            }
            map.DiscoveredRooms = discoveredRooms;

            // Deserialize RoomsToDiscover
            if (jsonObject.TryGetProperty("RoomsToDiscover", out var rtdElement))
            {
                map.RoomsToDiscover = JsonSerializer.Deserialize<List<RoomToDiscover>>(rtdElement.GetRawText(), options)
                                      ?? new List<RoomToDiscover>();
            }
        }

        return map;
    }

    public override void Write(Utf8JsonWriter writer, Map value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        // Serialize DiscoveredRooms
        writer.WritePropertyName("DiscoveredRooms");
        writer.WriteStartObject();
        foreach (var kvp in value.DiscoveredRooms)
        {
            var key = $"{kvp.Key.Item1},{kvp.Key.Item2}";
            writer.WritePropertyName(key);
            JsonSerializer.Serialize(writer, kvp.Value, options);
        }
        writer.WriteEndObject();

        // Serialize RoomsToDiscover
        writer.WritePropertyName("RoomsToDiscover");
        JsonSerializer.Serialize(writer, value.RoomsToDiscover ?? new List<RoomToDiscover>(), options);

        writer.WriteEndObject();
    }
}