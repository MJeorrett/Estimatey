using System.ComponentModel.DataAnnotations;

namespace Estimatey.Infrastructure.Float;

public record FloatOptions
{
    [Required]
    public string ApiKey { get; init; } = null!;

    [Required]
    public string ApiBaseUri { get; init; } = null!;
}
