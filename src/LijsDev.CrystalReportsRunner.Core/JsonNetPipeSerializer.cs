namespace LijsDev.CrystalReportsRunner.Core;

using System.Text;
using Newtonsoft.Json;
using PipeMethodCalls;

/// <summary>
/// Serializer used for named pipes communication
/// </summary>
public class JsonNetPipeSerializer : IPipeSerializer
{
    private readonly JsonSerializerSettings _jsonSettings;

    /// <inheritdoc/>
    public JsonNetPipeSerializer()
    {
        _jsonSettings = new();
        _jsonSettings.Converters.Add(new DataSetJsonConverter());
    }

    /// <inheritdoc/>
    public object? Deserialize(byte[] data, Type type)
    {
        using var stream = new MemoryStream(data);
        using var reader = new StreamReader(stream, Encoding.UTF8);
        using var jsonReader = new JsonTextReader(reader);

        var serializer = JsonSerializer.CreateDefault(_jsonSettings);

        return serializer.Deserialize(jsonReader, type);
    }

    /// <inheritdoc/>
    public byte[] Serialize(object o)
    {
        using var memoryStream = new MemoryStream();
        using var writer = new StreamWriter(memoryStream, Encoding.UTF8);
        using var jsonWriter = new JsonTextWriter(writer);

        var serializer = JsonSerializer.CreateDefault(_jsonSettings);

        serializer.Serialize(jsonWriter, o);
        jsonWriter.Flush();
        memoryStream.Position = 0;

        return memoryStream.ToArray();
    }
}
