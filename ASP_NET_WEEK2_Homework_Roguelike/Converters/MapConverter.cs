using System.Text.Json;
using System.Text.Json.Serialization;
using ASP_NET_WEEK3_Homework_Roguelike.Model;

namespace ASP_NET_WEEK3_Homework_Roguelike.Converters
{
    public class MapConverter : JsonConverter<Map>
    {
        public override Map Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var map = new Map();
            using (var document = JsonDocument.ParseValue(ref reader))
            {
                var root = document.RootElement;
                // deserializes DiscoveredRooms
                if (root.TryGetProperty("discoveredRooms", out JsonElement roomsElement))
                {
                    map.DiscoveredRooms = new Dictionary<(int, int), Room>();
                    foreach (var roomElement in roomsElement.EnumerateObject())
                    {
                        var coordinates = roomElement.Name.Split(',');
                        var room = JsonSerializer.Deserialize<Room>(roomElement.Value.GetRawText(), options);
                        map.DiscoveredRooms[(int.Parse(coordinates[0]), int.Parse(coordinates[1]))] = room;
                        // ensures Exits are not null
                        room.Exits ??= new Dictionary<string, Room>();
                    }
                }
                // deserializes RoomsToDiscover
                if (root.TryGetProperty("roomsToDiscover", out JsonElement rtdElement))
                {
                    if (rtdElement.TryGetProperty("$values", out JsonElement valuesElement) && valuesElement.ValueKind == JsonValueKind.Array)
                    {
                        var roomsToDiscoverList = new List<RoomToDiscover>();

                        foreach (var item in valuesElement.EnumerateArray())
                        {
                            var roomToDiscover = new RoomToDiscover
                            {
                                X = item.GetProperty("X").GetInt32(),
                                Y = item.GetProperty("Y").GetInt32(),
                                EnteringDirection = item.GetProperty("EnteringDirection").GetString(),
                                BlockedDirections = new HashSet<string>(item.GetProperty("BlockedDirections")
                                                                         .GetProperty("$values").EnumerateArray()
                                                                         .Select(x => x.GetString()))
                            };
                            roomsToDiscoverList.Add(roomToDiscover);
                        }
                        map.RoomsToDiscover = roomsToDiscoverList;
                    }
                    else
                    {
                        // handles case where $values is empty or missing
                        map.RoomsToDiscover = new List<RoomToDiscover>();
                    }
                }
                else
                {
                    // if the property doesn't exist at all, initializes an empty list
                    map.RoomsToDiscover = new List<RoomToDiscover>();
                }
            }
            // second pass: resolve the exits
            foreach (var kvp in map.DiscoveredRooms)
            {
                var room = kvp.Value;

                if (room.Exits != null)
                {
                    foreach (var exit in room.Exits.ToList())
                    {
                        var exitCoordinates = exit.Value;
                        if (exitCoordinates != null && map.DiscoveredRooms.ContainsKey((exitCoordinates.X, exitCoordinates.Y)))
                        {
                            room.Exits[exit.Key] = map.DiscoveredRooms[(exitCoordinates.X, exitCoordinates.Y)];
                        }
                    }
                }
            }
            return map;
        }
        public override void Write(Utf8JsonWriter writer, Map value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("discoveredRooms");
            writer.WriteStartObject();
            foreach (var kvp in value.DiscoveredRooms)
            {
                writer.WritePropertyName($"{kvp.Key.Item1},{kvp.Key.Item2}");

                var room = kvp.Value;
                var exitsToSerialize = room.Exits.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value != null ? new { kvp.Value.X, kvp.Value.Y } : null);
                var roomData = new
                {
                    room.X,
                    room.Y,
                    room.EventStatus,
                    room.IsExplored,
                    Exits = exitsToSerialize
                };
                JsonSerializer.Serialize(writer, roomData, options);
            }
            writer.WriteEndObject();
            writer.WritePropertyName("roomsToDiscover");
            writer.WriteStartObject();
            writer.WritePropertyName("$values");
            JsonSerializer.Serialize(writer, value.RoomsToDiscover, options);
            writer.WriteEndObject();
            writer.WriteEndObject();
        }
    }
}