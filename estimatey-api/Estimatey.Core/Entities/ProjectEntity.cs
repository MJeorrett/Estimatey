namespace Estimatey.Core.Entities;

public class ProjectEntity
{
    public int Id { get; set; }

    public string DevOpsProjectName { get; set; } = "";

    public string DevOpsContinuationToken { get; set; } = "";
}
