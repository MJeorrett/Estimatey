namespace Estimatey.Core.Entities;

public class FeatureEntity : WorkItemEntity
{
    public List<UserStoryEntity> UserStories { get; set; } = null!;
}
