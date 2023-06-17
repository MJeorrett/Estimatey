using Estimatey.Application.Common.Interfaces;
using Estimatey.Application.Common.Models;
using Estimatey.Infrastructure.Persistence;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace Estimatey.Infrastructure.Float;

public partial class FloatClient : IFloatClient
{
    private readonly ILogger _logger;
    private readonly HttpClient _httpClient;
    private readonly ApplicationDbContext _dbContext;

    private readonly string _apiKey;
    private readonly string _apiBaseUrl;

    public FloatClient(
        ILogger<FloatClient> logger,
        HttpClient httpClient,
        IOptions<FloatOptions> options,
        ApplicationDbContext dbContext)
    {
        _httpClient = httpClient;
        _logger = logger;
        _apiKey = options.Value.ApiKey;
        _apiBaseUrl = options.Value.ApiBaseUri;

        _httpClient.DefaultRequestHeaders.Authorization = new("Bearer", _apiKey);
        _dbContext = dbContext;
    }

    public async Task<List<LoggedTimeDto>> GetLoggedTime(int projectId, DateOnly? startDate = null, DateOnly? endDate = null)
    {
        var results = new List<LoggedTimeDto>();
        var currentPage = 1;
        var hasNextPage = true;

        while (hasNextPage)
        {
            if (currentPage > 2)
            {
                // To help to avoid hitting the Float API 10 requests per second rate limit.
                Thread.Sleep(1000);
            }

            var (resultsPage, _hasNextPage) = await GetLoggedTimePage(projectId, startDate, endDate, currentPage);
            currentPage++;

            results.AddRange(resultsPage);

            hasNextPage = _hasNextPage;
        }

        return results;
    }

    private async Task<(List<LoggedTimeDto>, bool)> GetLoggedTimePage(int projectId, DateOnly? startDate, DateOnly? endDate, int pageNumber)
    {
        var uri = BuildGetLoggedTimePageUri(projectId, startDate, endDate, pageNumber);

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

        var endOfCurrentPage = pageNumber * 200;
        var totalCountHeader = response.Headers.FirstOrDefault(_ => _.Key == "X-Pagination-Total-Count");

        if (totalCountHeader.Value is null || !totalCountHeader.Value.Any())
        {
            _logger.LogDebug("No X-Pagination-Total-Count header found.");
            return (loggedTime, false);
        }

        var totalCount = int.Parse(totalCountHeader.Value.First());

        return (loggedTime, totalCount > endOfCurrentPage);
    }

    private string BuildGetLoggedTimePageUri(int projectId, DateOnly? startDate, DateOnly? endDate, int pageNumber)
    {
        if (startDate > endDate)
        {
            throw new ArgumentOutOfRangeException($"{nameof(startDate)} must be <= {nameof(endDate)} but {nameof(startDate)}: {startDate} and {nameof(endDate)}: {endDate} were provided.");
        }
        var uri = $"{_apiBaseUrl}/logged-time?project_id={projectId}&page={pageNumber}&per-page=200";

        if (startDate is null && endDate is not null)
        {
            // Float limits the size of date rage you can request and doesn't seem to work if you only specify an end date.
            // As a work around we set start date to 1 year before end date if it is not provided.
            uri += $"&start_date={endDate.Value.AddYears(-1).AddDays(1):yyyy-MM-dd}";
        }
        else
        {
            uri += $"&start_date={startDate:yyyy-MM-dd}";
        }

        if (endDate is not null)
        {
            uri += $"&end_date={endDate:yyyy-MM-dd}";
        }

        return uri;
    }
}
