namespace Estimatey.Core.Entities;

public class TagEntity
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public List<FeatureEntity> Features { get; set; } = null!;

    public List<UserStoryEntity> UserStories { get; set; } = null!;

    public List<TicketEntity> Tickets { get; set; } = null!;
}
