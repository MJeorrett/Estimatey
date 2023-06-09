namespace Estimatey.Infrastructure.DevOps.Models;

internal record WorkItemLinksDto
{
    public List<WorkItemLink> Values { get; init; } = new();

    public string NextLink { get; init; } = "";

    public string ContinuationToken { get; init; } = "";

    public bool IsLastBatch { get; init; }
}

internal record WorkItemLink
{
    public string LinkType { get; init; } = "";

    public int SourceId { get; init; }

    public int TargetId { get; init; }

    public bool IsActive { get; init; }
}