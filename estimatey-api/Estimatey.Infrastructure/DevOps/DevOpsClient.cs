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
        ILogger<WorkItemSynchronizationService> logger,
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

    public async Task<WorkItemRevisionsBatch> GetWorkItemRevisionsBatch(string projectName, string? continuationToken)
    {
        _logger.LogInformation("Fetching work item revisions for project {projectName} with continuation token {continuationToken}.", projectName, continuationToken);

        string uri = BuildGetWorkItemRevisionsUri(projectName, continuationToken);
        await EnsureAccessTokenSet();

        var response = await _httpClient.GetAsync(uri);

        _logger.LogDebug("Received work item revisions response from DevOps for project {projectName}:\n{response}", await response.Content.ReadAsStringAsync(), projectName);

        if (response.StatusCode != HttpStatusCode.OK)
        {
            _logger.LogWarning("Fetching work item revisions from DevOps returned unexpected status code {statusCode} and body {responseBody} when syncing project {projectName}.", response.StatusCode, await response.Content.ReadAsStringAsync(), projectName);

            throw new Exception($"Failed to fetch work item revisions from uri {uri}.");
        }

        var workItemRevisionsBatch = await response.Content.ReadFromJsonAsync<WorkItemRevisionsBatch>() ??
            throw new Exception("Failed to parse response from get work item revisions response.");

        _logger.LogInformation("Received {numberOfWorkItemRevisions} work item revisions from DevOps for project {projectName}.", workItemRevisionsBatch.Values.Count, projectName);

        return workItemRevisionsBatch;
    }

    public async Task<WorkItemLinksBatch> GetWorkItemLinksBatch(string projectName, string? continuationToken)
    {
        _logger.LogInformation("Fetching work item links for project {projectName} with continuation token {continuationToken}.", projectName, continuationToken);

        string uri = BuildGetWorkItemLinksUri(projectName, continuationToken);
        await EnsureAccessTokenSet();

        var response = await _httpClient.GetAsync(uri);

        _logger.LogDebug("Received work item links response from DevOps for project {projectName}:\n{response}", await response.Content.ReadAsStringAsync(), projectName);

        if (response.StatusCode != HttpStatusCode.OK)
        {
            _logger.LogWarning("Fetching work item links from DevOps returned unexpected status code {statusCode} and body {responseBody} when syncing project {projectName}.", response.StatusCode, await response.Content.ReadAsStringAsync(), projectName);

            throw new Exception($"Failed to fetch work item links from uri {uri}.");
        }

        var workItemLinksBatch = await response.Content.ReadFromJsonAsync<WorkItemLinksBatch>() ??
            throw new Exception("Failed to parse response from get work item links response.");

        _logger.LogInformation("Received {numberOfWorkItemLinks} work item links from DevOps for project {projectName}.", workItemLinksBatch.Values.Count, projectName);

        return workItemLinksBatch;
    }

    public async Task<List<WorkItemShallowReference>> GetRecycleBinWorkItems(string projectName)
    {
        _logger.LogInformation("Fetching work item links for project {projectName}.", projectName);

        string uri = BuildGetRecycleBinWorkItemReferencesUri(projectName);
        await EnsureAccessTokenSet();

        var response = await _httpClient.GetAsync(uri);

        _logger.LogDebug("Received recycle bin work items response from DevOps for project {projectName}:\n{response}", await response.Content.ReadAsStringAsync(), projectName);

        if (response.StatusCode != HttpStatusCode.OK)
        {
            _logger.LogWarning("Fetching recycle bin work items from DevOps returned unexpected status code {statusCode} and body {responseBody} when syncing project {projectName}.", response.StatusCode, await response.Content.ReadAsStringAsync(), projectName);

            throw new Exception($"Failed to fetch recycle bin work items from uri {uri}.");
        }

        var recycleBinWorkItems = await response.Content.ReadFromJsonAsync<RecycleBinWorkItems>() ??
            throw new Exception("Failed to parse response from get recycle bin work items response.");

        _logger.LogInformation("Received {numberOfWorkItemInRecycleBin} recycle bin work items from DevOps for project {projectName}.", recycleBinWorkItems.Count, projectName);

        return recycleBinWorkItems.Value;
    }

    private async Task EnsureAccessTokenSet()
    {
        if (_accessToken is not null) return;

        var credential = new ClientSecretCredential(_azureAadtenantId, _clientId, _clientSecret);
        var tokenRequestContext = new TokenRequestContext(VssAadSettings.DefaultScopes);
        var accessToken = await credential.GetTokenAsync(tokenRequestContext, CancellationToken.None);
        _accessToken = accessToken.Token;

        _httpClient.DefaultRequestHeaders.Authorization = new("Bearer", accessToken.Token);
    }

    private string BuildGetWorkItemRevisionsUri(string projectName, string? continuationToken)
    {
        var uri = $"https://dev.azure.com/{_organizationName}/{projectName}/_apis/wit/reporting/workitemrevisions?api-version=4.1";

        if (!string.IsNullOrWhiteSpace(continuationToken))
        {
            uri += $"&continuationToken={continuationToken}";
        }

        return uri;
    }

    private string BuildGetWorkItemLinksUri(string projectName, string? continuationToken)
    {
        var uri = $"https://dev.azure.com/{_organizationName}/{projectName}/_apis/wit/reporting/workitemlinks?api-version=4.1";

        if (!string.IsNullOrWhiteSpace(continuationToken))
        {
            uri += $"&continuationToken={continuationToken}";
        }

        return uri;
    }

    private string BuildGetRecycleBinWorkItemReferencesUri(string projectName)
    {
        return $"https://dev.azure.com/{_organizationName}/{projectName}/_apis/wit/recyclebin?api-version=7.0";
    }
}