using Estimatey.Core.Entities;

namespace Estimatey.Application.WorkItems.Dtos;

public record FeatureSummary
{
    public int Id { get; init; }

    public string Title { get; init; } = "";

    public string State { get; init; } = "";

    public double SortOrder { get; init; }

    public List<UserStorySummary> UserStories { get; init; } = new();

    public static FeatureSummary MapFromEntity(FeatureEntity entity)
    {
        return new()
        {
            Id = entity.Id,
            Title = entity.Title,
            State = entity.State,
            SortOrder = CalculateSortOrder(entity),
            UserStories = entity.UserStories
                .Select(UserStorySummary.MapFromEntity)
                .ToList(),
        };  
    }

    private static double CalculateSortOrder(FeatureEntity entity)
    {
        var allTickets = entity.UserStories.SelectMany(_ => _.Tickets);

        if (allTickets.Count() == 0) return 300;

        if (allTickets.All(_ => _.State == TicketEntity.ClosedState)) return 0;

        if (allTickets.All(_ => _.State == TicketEntity.NewState)) return 200;

        var completedTicketCount = allTickets.Count(_ => _.State == TicketEntity.ClosedState);

        return (double) completedTicketCount / allTickets.Count();
    }
}
