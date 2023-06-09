namespace Estimatey.Application.Common.Models;

public record DevOpsFeature
{
    public int Id { get; init; }

    public required string Title { get; init; }
}
