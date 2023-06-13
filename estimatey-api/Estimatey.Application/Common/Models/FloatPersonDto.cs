using System.Text.Json.Serialization;

namespace Estimatey.Application.Common.Models;

public record FloatPersonDto
{
    [JsonPropertyName("people_id")]
    public int Id { get; init; }

    public string Name { get; init; } = "";
}
