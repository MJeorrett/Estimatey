namespace Estimatey.Core.Entities;

public abstract class WorkItemEntity
{
    public int Id { get; set; }

    public int DevOpsId { get; set; }

    public int ProjectId { get; set; }
    public ProjectEntity Project { get; set; } = null!;

    public string Title { get; set; } = "";

    public string State { get; set; } = "";

    public List<TagEntity> Tags { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime ChangedDate { get; set; }
}
