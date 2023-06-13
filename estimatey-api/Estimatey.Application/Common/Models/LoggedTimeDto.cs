using System.Text.Json.Serialization;

namespace Estimatey.Application.Common.Models;

public record LoggedTimeDto
{
    [JsonPropertyName("people_id")]
    public int PersonId { get; init; }

    public string Date { get; init; } = "";

    public double Hours { get; init; }

    public bool Locked { get; init; }

    [JsonPropertyName("locked_date")]
    public string LockedDate { get; init; } = "";
}
