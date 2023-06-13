using Estimatey.Core.Entities;

namespace Estimatey.Application.WorkItems.Dtos;

public record UserStorySummaryDto
{
    public int Id { get; init; }

    public string Title { get; init; } = "";

    public string State { get; init; } = "";

    public List<TicketSummaryDto> Tickets { get; init; } = new();

    public static UserStorySummaryDto MapFromEntity(UserStoryEntity entity)
    {
        return new()
        {
            Id = entity.Id,
            Title = entity.Title,
            State = entity.State,
            Tickets = entity.Tickets
                .Select(TicketSummaryDto.MapFromEntity)
                .ToList(),
        };
    }
}
