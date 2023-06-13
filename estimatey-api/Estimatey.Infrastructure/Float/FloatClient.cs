using Estimatey.Application.Common.Interfaces;
using Estimatey.Application.Common.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace Estimatey.Infrastructure.Float;

public class FloatClient : IFloatClient
{
    private readonly ILogger _logger;
    private readonly HttpClient _httpClient;

    private readonly string _apiKey;
    private readonly string _apiBaseUrl;

    public FloatClient(
        ILogger<FloatClient> logger,
        HttpClient httpClient,
        IOptions<FloatOptions> options)
    {
        _httpClient = httpClient;
        _logger = logger;
        _apiKey = options.Value.ApiKey;
        _apiBaseUrl = options.Value.ApiBaseUri;

        _httpClient.DefaultRequestHeaders.Authorization = new("Bearer", _apiKey);
    }

    public async Task<List<LoggedTimeDto>> GetLoggedTime(int projectId)
    {
        var uri = $"{_apiBaseUrl}/logged-time?project_id={projectId}&per-page=200";

        var response = await _httpClient.GetAsync(uri);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogInformation("Request to get logged time from Float for project {projectId} failed with status code {statusCode}.", projectId, response.StatusCode);
            throw new Exception($"Request to get logged time from Float for project {projectId} failed with status code {response.StatusCode}.");
        }

        var loggedTime = await response.Content.ReadFromJsonAsync<List<LoggedTimeDto>>();

        if (loggedTime is null)
        {
            _logger.LogWarning("Failed to parse logged time from response from Float.");
            throw new Exception("Failed to parse logged time from response from Float.");
        }

        if (loggedTime.Count >= 200)
        {
            // Will have to handle multiple pages in future / cache the historic logged time but lets just get something simple up for now.
            _logger.LogWarning("Probably missing logged time data as the returned page is full.");
        }

        return loggedTime;
    }

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
}
