namespace Estimatey.Core.Entities;

public abstract class WorkItemEntity
{
    public int Id { get; set; }

    public int DevOpsId { get; set; }

    public string Title { get; set; } = "";

    public string State { get; set; } = "";

    public List<TagEntity> Tags { get; set; } = null!;
}
