using Estimatey.Application.Common.Models;

namespace Estimatey.Application.Common.Interfaces;

public interface IFloatClient
{
    Task<List<LoggedTimeDto>> GetLoggedTime(int projectId);

    Task<List<FloatPersonDto>> GetPeople();
}