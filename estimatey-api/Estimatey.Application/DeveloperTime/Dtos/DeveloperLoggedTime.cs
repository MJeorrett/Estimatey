using Estimatey.Application.Common.Models;
using Estimatey.Core.Entities;

namespace Estimatey.Application.DeveloperTime.Dtos;

public record DeveloperLoggedTime
{
    public string DeveloperName { get; set; } = "";

    public double TotalLoggedHours { get; set; }

    public static List<DeveloperLoggedTime> MapFromLoggedTime(List<LoggedTimeDto> loggedTime, List<FloatPersonEntity> people)
    {
        return loggedTime
            .GroupBy(_ => _.PersonId)
            .Select(personTimeGroup => new DeveloperLoggedTime()
            {
                DeveloperName = people.FirstOrDefault(_ => _.FloatId == personTimeGroup.Key)?.Name ?? $"Person {personTimeGroup.Key}",
                TotalLoggedHours = personTimeGroup.Sum(_ => _.Hours)
            })
            .ToList();
    }
}
