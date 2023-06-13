namespace Estimatey.Core.Entities;

public class TicketEntity : WorkItemEntity
{
    public const string NewState = "New";
    public const string ClosedState = "Closed";

    public int? UserStoryId { get; set; }

    public UserStoryEntity? UserStory { get; set; } = null!;
}
