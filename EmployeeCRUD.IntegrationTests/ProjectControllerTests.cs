using EmployeeCRUD.Application.Exceptions;
using EmployeeCRUD.Application.ProjectModule.Commands;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace EmployeeCRUD.IntegrationTests
{
    public class ProjectControllerTests:IClassFixture<EmployeeCRUDWebApplicationFactory>
    {
        private readonly EmployeeCRUDWebApplicationFactory factory;
        private readonly HttpClient client;
        private readonly ITestOutputHelper output;
        private readonly IServiceScope scope;
        private readonly IMediator mediator;

        public ProjectControllerTests(EmployeeCRUDWebApplicationFactory _factory, ITestOutputHelper _output)
        {
            factory = _factory;
            client = factory.CreateClient();
            output = _output;
            scope = factory.Services.CreateScope();
            mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        }

        [Fact]
        public async Task AddProject_WithInvalidData_ShouldThrowValidationException()
        {
            // Arrange
            var command = new AddProjectCommand(
            ProjectName: "", 
            Description: "", 
            StartDate: DateTime.Now,
            EndDate: DateTime.Now.AddDays(-1), 
            Budget: 100,
            Status: "Active",
            ClientName: "Test Client"
             );
            var exception = await Assert.ThrowsAsync<CustomValidationException>(() => mediator.Send(command));

            foreach (var kvp in exception.Errors)
            {
                output.WriteLine($"{kvp.Key}: {string.Join(", ", kvp.Value)}");
            }

        }

        [Theory]
        [InlineData("Project A", "Description A", 1000, "Active", "Client A")]
        [InlineData("Project B", "Description B", 5000, "Completed", "Client B")]
        public async Task AddProject_WithValidData_ShouldCreateSuccessfully(
        string projectName,
        string description,
        decimal budget,
        string status,
        string clientName)
        {
            var command = new AddProjectCommand(
           ProjectName: projectName,
           Description: description,
           StartDate: DateTime.Now,
           EndDate: DateTime.Now.AddDays(10),
           Budget: budget,
           Status: status,
           ClientName: clientName
       );

            var result = await mediator.Send(command);
            Assert.NotNull(result);
            Assert.Equal(projectName, result.ProjectName);
            Assert.Equal(description, result.Description);
            Assert.Equal(budget, result.Budget);
            Assert.Equal(status, result.Status);
            Assert.Equal(clientName, result.ClientName);
            output.WriteLine($"Project ID: {result.Id}, Name: {result.ProjectName}");
        }
    }
}
