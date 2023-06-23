namespace Estimatey.Core.Entities;

public class BugEntity : WorkItemEntity
{
    public const string NewState = "New";
    public const string ClosedState = "Closed";

    public int? UserStoryId { get; set; }

    public UserStoryEntity? UserStory { get; set; } = null!;
}
