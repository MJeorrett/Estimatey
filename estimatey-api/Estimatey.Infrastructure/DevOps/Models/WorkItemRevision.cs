using System.Text.Json.Serialization;

namespace Estimatey.Infrastructure.DevOps.Models;

internal record WorkItemRevisionsBatch
{
    public List<WorkItemRevision> Values { get; init; } = new();

    public string NextLink { get; init; } = "";

    public string ContinuationToken { get; init; } = "";

    public bool IsLastBatch { get; init; }
}

internal record WorkItemRevision
{
    public int Id { get; init; }

    public int Rev { get; init; }

    public WorkItemRevisionField Fields { get; init; } = new();

    public List<string> Tags => Fields.RawTags.Length == 0 ?
        new List<string>() :
        Fields.RawTags
            .Split(";")
            .Select(_ => _.Trim())
            .ToList();

}

internal record WorkItemRevisionField
{
    [JsonPropertyName("System.TeamProject")]
    public string TeamProject { get; init; } = "";

    [JsonPropertyName("System.WorkItemType")]
    public string WorkItemType { get; init; } = "";

    [JsonPropertyName("System.State")]
    public string State { get; init; } = "";

    [JsonPropertyName("System.Title")]
    public string Title { get; init; } = "";

    [JsonPropertyName("System.Tags")]
    public string RawTags { get; init; } = "";

    [JsonPropertyName("System.CreatedDate")]
    public DateTime CreatedDate { get; init; }

    [JsonPropertyName("System.ChangedDate")]
    public DateTime ChangedDate { get; init; }

    [JsonPropertyName("System.IterationLevel2")]
    public string? IterationLevel2 { get; init; }
}
