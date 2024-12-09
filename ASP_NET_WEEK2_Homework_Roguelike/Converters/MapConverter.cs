using System.Text.Json;
using System.Text.Json.Serialization;
using ASP_NET_WEEK3_Homework_Roguelike.Converters;
using ASP_NET_WEEK3_Homework_Roguelike.Model;
using ASP_NET_WEEK3_Homework_Roguelike.Model.Items;
public class MapConverter : JsonConverter<Map>, IConverter<Map>
{
    public override Map Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        try
        {
            var jsonObject = JsonDocument.ParseValue(ref reader).RootElement;
            var map = new Map();

            // Deserialize DiscoveredRooms
            if (jsonObject.TryGetProperty("DiscoveredRooms", out var discoveredRoomsProperty))
            {
                map.DiscoveredRooms = new Dictionary<(int, int), Room>();
                foreach (var roomEntry in discoveredRoomsProperty.EnumerateObject())
                {
                    var coordinates = roomEntry.Name.Split(',');
                    var x = int.Parse(coordinates[0]);
                    var y = int.Parse(coordinates[1]);

                    var roomElement = roomEntry.Value;
                    var room = new Room
                    {
                        X = roomElement.GetProperty("X").GetInt32(),
                        Y = roomElement.GetProperty("Y").GetInt32(),
                        EventStatus = roomElement.GetProperty("EventStatus").GetString(),
                        IsExplored = roomElement.GetProperty("IsExplored").GetBoolean()
                    };

                    // Deserialize Exits as a List<string> and convert to Dictionary<string, Room?>
                    if (roomElement.TryGetProperty("Exits", out var exitsProperty))
                    {
                        var exitsList = JsonSerializer.Deserialize<List<string>>(exitsProperty.GetRawText(), options);
                        room.Exits = exitsList?.ToDictionary(exit => exit, _ => (Room?)null) ?? new Dictionary<string, Room?>();
                    }
                    map.DiscoveredRooms[(x, y)] = room;
                }
            }
            // Deserialize RoomsToDiscover
            if (jsonObject.TryGetProperty("RoomsToDiscover", out var roomsToDiscoverProperty))
            {
                map.RoomsToDiscover = JsonSerializer.Deserialize<List<RoomToDiscover>>(roomsToDiscoverProperty.GetRawText(), options) ?? new List<RoomToDiscover>();
            }
            return map;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to deserialize Map from JSON.", ex);
        }
    }
    public override void Write(Utf8JsonWriter writer, Map value, JsonSerializerOptions options)
    {
        try
        {
            writer.WriteStartObject();

            // Serialize DiscoveredRooms
            writer.WritePropertyName("DiscoveredRooms");
            writer.WriteStartObject();
            foreach (var kvp in value.DiscoveredRooms)
            {
                var key = $"{kvp.Key.Item1},{kvp.Key.Item2}";
                writer.WritePropertyName(key);

                writer.WriteStartObject();
                var room = kvp.Value;

                writer.WriteNumber("X", room.X);
                writer.WriteNumber("Y", room.Y);
                writer.WriteString("EventStatus", room.EventStatus);
                writer.WriteBoolean("IsExplored", room.IsExplored);

                // Serialize Exits as a List<string>
                writer.WritePropertyName("Exits");
                JsonSerializer.Serialize(writer, room.Exits.Keys.ToList(), options);

                writer.WriteEndObject();
            }
            writer.WriteEndObject();

            // Serialize RoomsToDiscover
            writer.WritePropertyName("RoomsToDiscover");
            JsonSerializer.Serialize(writer, value.RoomsToDiscover, options);
            writer.WriteEndObject();
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to serialize Map to JSON.", ex);
        }
    }
}