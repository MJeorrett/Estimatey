namespace Estimatey.Core.Entities;

public class ProjectEntity
{
    public int Id { get; set; }

    public string DevOpsProjectName { get; set; } = "";

    public string? WorkItemsContinuationToken { get; set; }

    public string? LinksContinuationToken { get; set; }
}
