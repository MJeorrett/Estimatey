namespace Estimatey.Infrastructure.DevOps.Models;

internal record RecycleBinWorkItems
{
    public int Count { get; init; }

    public List<WorkItemShallowReference> Value { get; init; } = new();
}
