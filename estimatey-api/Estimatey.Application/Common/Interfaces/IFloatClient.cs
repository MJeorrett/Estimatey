using Estimatey.Application.Common.Models;

namespace Estimatey.Application.Common.Interfaces;

public interface IFloatClient
{
    Task<List<LoggedTimeDto>> GetLoggedTime(int projectId, DateOnly? startDate = null, DateOnly? endDate = null);

    Task<List<FloatPersonDto>> GetPeople();

    Task SyncPeople();
}