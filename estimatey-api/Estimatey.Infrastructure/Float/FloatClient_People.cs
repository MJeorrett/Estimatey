using Estimatey.Application.Common.Interfaces;
using Estimatey.Application.Common.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Threading;

namespace Estimatey.Infrastructure.Float;

public partial class FloatClient : IFloatClient
{
    private static SemaphoreSlim _syncPeopleSemaphore = new SemaphoreSlim(1, 1);

    public async Task<List<FloatPersonDto>> GetPeople()
    {
        var uri = $"{_apiBaseUrl}/people?per-page=200";

        var response = await _httpClient.GetAsync(uri);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogInformation("Request to get people from Float failed with status code {statusCode}.", response.StatusCode);
            throw new Exception($"Request to get people from Float failed with status code {response.StatusCode}.");
        }

        var people = await response.Content.ReadFromJsonAsync<List<FloatPersonDto>>();

        if (people is null)
        {
            _logger.LogWarning("Failed to parse people from response from Float.");
            throw new Exception("Failed to parse people from response from Float.");
        }

        if (people.Count >= 200)
        {
            // Unlikely to hit this limit for people but should ideally handle multiple pages.
            _logger.LogWarning("Probably missing people data as the returned page is full.");
        }

        _logger.LogDebug("Float returned {peopleCount} people.", people.Count);

        return people;
    }

    public async Task SyncPeople()
    {
        await _syncPeopleSemaphore.WaitAsync();

        try
        {
            var existingPeopleFloatIds = await _dbContext.FloatPeople
            .Select(_ => _.FloatId)
                .ToListAsync();

            var peopleFromFloat = await GetPeople();

            foreach (var floatPerson in peopleFromFloat)
            {
                if (!existingPeopleFloatIds.Contains(floatPerson.Id))
                {
                    _logger.LogInformation("{personName} doesn't exist so adding them.", floatPerson.Name);

                    _dbContext.FloatPeople.Add(new()
                    {
                        Name = floatPerson.Name,
                        FloatId = floatPerson.Id,
                    });
                }
            }

            await _dbContext.SaveChangesAsync();
        }
        finally
        {
            _syncPeopleSemaphore.Release();
        }
    }
}
