using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace ASP_NET_WEEK2_Homework_Roguelike
{
    public class MapConverter : JsonConverter<Map>
    {
        public override Map Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var map = new Map();

            using (var document = JsonDocument.ParseValue(ref reader))
            {
                var root = document.RootElement;

                // Deserialize discoveredRooms
                if (root.TryGetProperty("discoveredRooms", out JsonElement roomsElement))
                {
                    foreach (var roomElement in roomsElement.EnumerateObject())
                    {
                        var coordinates = roomElement.Name.Split(',');
                        var room = JsonSerializer.Deserialize<Room>(roomElement.Value.GetRawText(), options);
                        map.discoveredRooms[(int.Parse(coordinates[0]), int.Parse(coordinates[1]))] = room;

                        // Ensure Exits is not null
                        room.Exits ??= new Dictionary<string, Room>();
                    }
                }

                // Manually deserialize roomsToDiscover
                if (root.TryGetProperty("roomsToDiscover", out JsonElement rtdElement))
                {
                    var roomsToDiscoverList = new List<RoomToDiscover>();

                    if (rtdElement.TryGetProperty("$values", out JsonElement valuesElement) && valuesElement.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var item in valuesElement.EnumerateArray())
                        {
                            var roomToDiscover = new RoomToDiscover
                            {
                                X = item.GetProperty("X").GetInt32(),
                                Y = item.GetProperty("Y").GetInt32(),
                                EnteringDirection = item.GetProperty("EnteringDirection").GetString(),
                                BlockedDirections = new HashSet<string>(item.GetProperty("BlockedDirections").GetProperty("$values").EnumerateArray().Select(x => x.GetString()))
                            };

                            roomsToDiscoverList.Add(roomToDiscover);
                        }
                    }

                    map.roomsToDiscover = roomsToDiscoverList;
                }
            }

            // Second pass: resolve the exits
            foreach (var kvp in map.discoveredRooms)
            {
                var room = kvp.Value;

                if (room.Exits != null)
                {
                    foreach (var exit in room.Exits.ToList())
                    {
                        var exitCoordinates = exit.Value;
                        if (exitCoordinates != null)
                        {
                            room.Exits[exit.Key] = map.discoveredRooms[(exitCoordinates.X, exitCoordinates.Y)];
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
            foreach (var kvp in value.discoveredRooms)
            {
                writer.WritePropertyName($"{kvp.Key.Item1},{kvp.Key.Item2}");

                var room = kvp.Value;
                var exitsToSerialize = room.Exits.ToDictionary(kvp => kvp.Key, kvp => kvp.Value != null ? new { X = kvp.Value.X, Y = kvp.Value.Y } : null);
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
            JsonSerializer.Serialize(writer, value.roomsToDiscover, options);
            writer.WriteEndObject();

            writer.WriteEndObject();
        }
    }
}