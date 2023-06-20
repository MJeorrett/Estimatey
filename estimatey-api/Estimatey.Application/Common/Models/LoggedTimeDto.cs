using Estimatey.Core.Entities;
using System.Text.Json.Serialization;

namespace Estimatey.Application.Common.Models;

public record LoggedTimeDto
{
    [JsonPropertyName("logged_time_id")]
    public string Id { get; set; } = "";

    [JsonPropertyName("people_id")]
    public int PersonId { get; init; }

    public string Date { get; init; } = "";

    public double Hours { get; init; }

    public bool Locked { get; init; }

    [JsonPropertyName("locked_date")]
    public string? LockedDate { get; init; }
}
