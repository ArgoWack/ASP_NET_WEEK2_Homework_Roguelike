using System.Text.Json;

namespace ASP_NET_WEEK3_Homework_Roguelike.Converters
{
    public interface IConverter<T>
    {
        T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options);
        void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options);
    }
}