using Estimatey.Core.Entities;

namespace Estimatey.Application.WorkItems.Dtos;

public record UserStorySummary
{
    public int Id { get; init; }

    public string Title { get; init; } = "";

    public string State { get; init; } = "";

    public List<TicketSummary> Tickets { get; init; } = new();

    public static UserStorySummary MapFromEntity(UserStoryEntity entity)
    {
        return new()
        {
            Id = entity.Id,
            Title = entity.Title,
            State = entity.State,
            Tickets = entity.Tickets
                .Select(TicketSummary.MapFromEntity)
                .ToList(),
        };
    }
}
