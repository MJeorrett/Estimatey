namespace Estimatey.Core.Entities;

public class ProjectEntity
{
    public int Id { get; set; }

    public string DevOpsProjectName { get; set; } = "";

    public int FloatId { get; set; }

    public string? WorkItemsContinuationToken { get; set; }

    public string? LinksContinuationToken { get; set; }

    public DateOnly? LoggedTimeHasBeenSyncedUntil { get; set; }

    public List<FeatureEntity> Features { get; set; } = null!;

    public List<UserStoryEntity> UserStories { get; set; } = null!;

    public List<TicketEntity> Tickets { get; set; } = null!;

    public List<LoggedTimeEntity> LoggedTime { get; set; } = null!;
}
