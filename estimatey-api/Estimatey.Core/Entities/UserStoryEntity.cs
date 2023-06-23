namespace Estimatey.Core.Entities;

public class UserStoryEntity : WorkItemEntity
{
    public int? FeatureId { get; set; }
    public FeatureEntity? Feature { get; set; } = null!;

    public List<TicketEntity> Tickets { get; set; } = null!;
    public List<BugEntity> Bugs { get; set; } = null!;
}
