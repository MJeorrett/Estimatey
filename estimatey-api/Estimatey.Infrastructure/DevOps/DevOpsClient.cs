using Estimatey.Infrastructure.DevOps.Models;
using Estimatey.Infrastructure.DevOps;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Net;
using Azure.Core;
using Azure.Identity;
using Microsoft.VisualStudio.Services.Client;

internal class DevOpsClient
{
    private readonly ILogger _logger;
    private readonly HttpClient _httpClient;

    private readonly string _azureAadtenantId;
    private readonly string _clientId;
    private readonly string _clientSecret;
    private readonly string _organizationName;

    private string? _accessToken;

    public DevOpsClient(
        ILogger<DevOpsWorkItemSynchronizer> logger,
        HttpClient httpClient,
        IOptions<DevOpsOptions> options)
    {
        _logger = logger;
        _httpClient = httpClient;

        _organizationName = options.Value.OrganizationName;
        _azureAadtenantId = options.Value.AzureAadTenantId;
        _clientId = options.Value.ClientId;
        _clientSecret = options.Value.ClientSecret;
    }

    public async Task<WorkItemRevisionsDto> GetWorkItemRevisions(string projectName, string continuationToken)
    {
        _logger.LogInformation("Fetching work item revisions for project {projectName} with continuation token {continuationToken}.", projectName, continuationToken);

        string uri = BuildGetWorkItemRevisionsUri(projectName, continuationToken);
        await EnsureAccessTokenSet();

        var workItemRevisionsResponse = await _httpClient.GetAsync(uri);

        _logger.LogDebug("Received work item revisions response from DevOps for project {projectName}:\n{response}", await workItemRevisionsResponse.Content.ReadAsStringAsync(), projectName);

        if (workItemRevisionsResponse.StatusCode != HttpStatusCode.OK)
        {
            _logger.LogWarning("Fetching work item revisions from DevOps returned unexpected status code {statusCode} and body {responseBody} when syncing project {projectName}.", workItemRevisionsResponse.StatusCode, await workItemRevisionsResponse.Content.ReadAsStringAsync(), projectName);

            throw new Exception($"Failed to fetch work item revisions from uri {uri}.");
        }

        var workItemRevisionsDto = await workItemRevisionsResponse.Content.ReadFromJsonAsync<WorkItemRevisionsDto>() ??
            throw new Exception("Failed to parse response from get work item revisions response.");

        _logger.LogInformation("Received {numberOfWorkItemRevisions} work item revisions from DevOps for project {projectName}.", workItemRevisionsDto.Values.Count, projectName);

        return workItemRevisionsDto;
    }

    private async Task EnsureAccessTokenSet()
    {
        if (_accessToken is not null) return;

        var credential = new ClientSecretCredential(_azureAadtenantId, _clientId, _clientSecret);
        var tokenRequestContext = new TokenRequestContext(VssAadSettings.DefaultScopes);
        var accessToken = await credential.GetTokenAsync(tokenRequestContext, CancellationToken.None);

        _httpClient.DefaultRequestHeaders.Authorization = new("Bearer", accessToken.Token);
    }

    private string BuildGetWorkItemRevisionsUri(string projectName, string continuationToken)
    {
        var uri = $"https://dev.azure.com/{_organizationName}/{projectName}/_apis/wit/reporting/workitemrevisions?api-version=4.1";

        if (!string.IsNullOrWhiteSpace(continuationToken))
        {
            uri += $"&continuationToken={continuationToken}";
        }

        return uri;
    }
}