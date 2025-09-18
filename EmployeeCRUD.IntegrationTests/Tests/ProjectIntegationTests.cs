using EmployeeCRUD.Application.EmployeeModule.Commands;
using EmployeeCRUD.Application.EmployeeModule.Dtos;
using EmployeeCRUD.Application.Exceptions;
using EmployeeCRUD.Application.ProjectModule.Commands;
using EmployeeCRUD.Application.ProjectModule.Dtos;
using EmployeeCRUD.IntegrationTests.Factory;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace EmployeeCRUD.IntegrationTests.Tests
{
    public class ProjectIntegationTests:IClassFixture<EmployeeCRUDWebApplicationFactory>
    {
        private readonly EmployeeCRUDWebApplicationFactory factory;
        private readonly HttpClient client;
        private readonly ITestOutputHelper output;
        private readonly IServiceScope scope;
        private readonly IMediator mediator;

        public ProjectIntegationTests(EmployeeCRUDWebApplicationFactory _factory, ITestOutputHelper _output)
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

        [Fact]
        public async Task AddProjectDapper_WithValidInput_ShouldCreateProject()
        {
            var project = new AddProjectDapperCommand
            (
                ProjectName :"Dapper Project",
                Description : "Dapper Description",
                StartDate : DateTime.Now,
                EndDate : DateTime.Now.AddDays(15),
                Budget :3000,
                Status : "Active",
                ClientName : "Dapper Client"
            );

            var addedProject = await mediator.Send(project);
            addedProject.Should().NotBeNull();
            addedProject.ProjectName.Should().Be("Dapper Project");
            output.WriteLine($"Dapper Project ID: {addedProject.Id}, Name: {addedProject.ProjectName}");

        }

        [Fact]
        public async Task PatchProjectDapper_ValidData_GetSuccess()
        {
            var employee1 = new EmployeeDto { EmpName = "User A", Email = "usera@gmail.com", Phone = "9876544567" };
            var employee2 = new EmployeeDto { EmpName = "User B", Email = "userb@gmail.com", Phone = "9876544560" };

            var emp1Response = await mediator.Send(new AddEmployeeCommand(employee1));
            var emp2Response = await mediator.Send(new AddEmployeeCommand(employee2));

            emp1Response.Should().NotBeNull();
            emp2Response.Should().NotBeNull();


            var projectCommand = new AddProjectDapperCommand
                (
                ProjectName: "Dapper Project",
                Description: "Dapper Description",
                StartDate: DateTime.Now,
                EndDate: DateTime.Now.AddDays(15),
                Budget: 3000,
                Status: "Active",
                ClientName: "Dapper Client"
                );

        var addedProject = await mediator.Send(projectCommand);

        addedProject.Should().NotBeNull();

            var patchCommand = new PatchProjectDapperCommand(
             Id: addedProject.Id,
             ProjectName: "Dapper Project Updated",
             Description: null,
             EndDate: null,
             Budget: null,
             Status: null,
             ClientName: null,
             ProjectManagerId: null,
             TeamMembersIds: new List<Guid> { emp1Response.Id, emp2Response.Id }
         );

            var patchedProject = await mediator.Send(patchCommand);
            patchedProject.Should().NotBeNull();
            patchedProject.Description.Should().Be(addedProject.Description);
            patchedProject.Budget.Should().Be(addedProject.Budget);
            output.WriteLine($"Id: {patchedProject.Id}");
            output.WriteLine($"ProjectName: {patchedProject.ProjectName}");
            output.WriteLine($"Description: {patchedProject.Description}");
            output.WriteLine($"StartDate: {patchedProject.StartDate}");
            output.WriteLine($"EndDate: {patchedProject.EndDate}");
            output.WriteLine($"Budget: {patchedProject.Budget}");
            output.WriteLine($"Status: {patchedProject.Status}");
            output.WriteLine($"ClientName: {patchedProject.ClientName}");
            output.WriteLine($"ProjectManagerName: {patchedProject.ProjectManagerName}");
            output.WriteLine($"TeamMembers: {string.Join(", ", patchedProject.TeamMember)}");

        }

        [Fact]
        public async Task ConcurrentProjectCreation()
        {
            int concurrentRequest = 5;
            var tasks = new List<Task<ProjectDto>>();

            for (int i = 0; i < concurrentRequest; i++)
            {
                tasks.Add(Task.Run(async () =>
                {
                    using var scope = factory.Services.CreateScope();
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                    var projectName = $"Concurrent Project {i}-{Guid.NewGuid()}";

                    var projectCommand = new AddProjectDapperCommand(
                        ProjectName: projectName,
                        Description: "Test concurrent",
                        StartDate: DateTime.Now,
                        EndDate: DateTime.Now.AddDays(10),
                        Budget: 1000,
                        Status: "Active",
                        ClientName: "Test Client"
                    );

                    return await mediator.Send(projectCommand);
                }));
            }

            var results = await Task.WhenAll(tasks);

            results.Should().HaveCount(concurrentRequest);
            results.Select(p => p.ProjectName).Should().OnlyHaveUniqueItems();
            foreach (var project in results)
            {
                output.WriteLine($"Id: {project.Id}, ProjectName: {project.ProjectName}");
            }
        }
    }
}
