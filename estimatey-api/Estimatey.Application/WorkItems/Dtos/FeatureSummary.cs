using Estimatey.Core.Entities;

namespace Estimatey.Application.WorkItems.Dtos;

public record FeatureSummary
{
    public int Id { get; init; }

    public string Title { get; init; } = "";

    public string State { get; init; } = "";

    public List<UserStorySummary> UserStories { get; init; } = new();

    public static FeatureSummary MapFromEntity(FeatureEntity entity)
    {
        return new()
        {
            Id = entity.Id,
            Title = entity.Title,
            State = entity.State,
            UserStories = entity.UserStories
                .Select(UserStorySummary.MapFromEntity)
                .ToList(),
        };
    }
}
