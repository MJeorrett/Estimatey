using Estimatey.E2eTests.Shared.Dtos.Projects;
using Estimatey.E2eTests.Shared.Endpoints;
using Estimatey.E2eTests.Shared.Extensions;
using Estimatey.E2eTests.Shared.WebApplicationFactory;
using FluentAssertions;
using Xunit.Abstractions;

namespace Estimatey.E2eTests.Projects;

[Collection(CustomWebApplicationCollection.Name)]
public class ProjectE2eTests : TestBase
{
    public ProjectE2eTests(
        CustomWebApplicationFixture webApplicationFixture,
        ITestOutputHelper testOutputHelper) :
        base(webApplicationFixture, testOutputHelper)
    {
    }

    [Fact]
    public async Task ShouldReturnCreatedProjectById()
    {
        var createResponse = await HttpClient.CreateProject().Call(new() { Title = "Expected title" });

        await createResponse.Should().HaveStatusCode(201);

        var createdProjectId = await createResponse.ReadResponseContentAs<int>();

        var getProjectByIdResponse = await HttpClient.GetProjectById().Call(createdProjectId);

        await getProjectByIdResponse.Should().HaveStatusCode(200);

        var returnedProject = await getProjectByIdResponse.ReadResponseContentAs<ProjectDto>();

        returnedProject.Title.Should().Be("Expected title");
    }

    [Fact]
    public async Task ShouldListCreatedProjects()
    {
        var project1Id = await HttpClient.CreateProject().CallAndParseResponse(new() { Title = "Title 1" });
        var project2Id = await HttpClient.CreateProject().CallAndParseResponse(new() { Title = "Title 2" });

        var listProjectsResponse = await HttpClient.ListProjects().Call();

        await listProjectsResponse.Should().HaveStatusCode(200);

        var listProjectsResults = await listProjectsResponse.ReadResponseContentAs<List<ProjectDto>>();

        listProjectsResults.Should().HaveCount(2);
        listProjectsResults[0].Should().BeEquivalentTo(new ProjectDto() { Id = project1Id, Title = "Title 1" });
        listProjectsResults[1].Id.Should().Be(project2Id);
    }

    [Fact]
    public async Task ShouldUpdateProject()
    {
        var projectId = await HttpClient.CreateProject().CallAndParseResponse(new() { Title = "Title 1" });

        var updateResponse = await HttpClient.UpdateProject().Call(new() { Id = projectId, Title = "Updated title" });

        await updateResponse.Should().HaveStatusCode(200);

        var updatedProject = await HttpClient.GetProjectById().CallAndParseResponse(projectId);

        updatedProject.Should().BeEquivalentTo(new ProjectDto() { Id = projectId, Title = "Updated title" });
    }

    [Fact]
    public async Task ShouldDeleteProject()
    {
        var projectId1 = await HttpClient.CreateProject().CallAndParseResponse(new() { Title = "Title 1" });
        var projectId2 = await HttpClient.CreateProject().CallAndParseResponse(new() { Title = "Title 2" });

        var deleteProjectResponse = await HttpClient.DeleteProject().Call(projectId1);

        await deleteProjectResponse.Should().HaveStatusCode(200);

        var listProjectsResponse = await HttpClient.ListProjects().CallAndParseResponse();

        listProjectsResponse.Should().HaveCount(1);
        listProjectsResponse[0].Id.Should().Be(projectId2);
    }
}
