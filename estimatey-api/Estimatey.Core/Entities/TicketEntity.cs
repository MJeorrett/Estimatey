namespace Estimatey.Core.Entities;

public class TicketEntity : WorkItemEntity
{
    public int? UserStoryId { get; set; }

    public UserStoryEntity? UserStory { get; set; } = null!;
}
