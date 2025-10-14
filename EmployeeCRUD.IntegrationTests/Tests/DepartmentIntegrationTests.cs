using EmployeeCRUD.Application.Department.Command;
using EmployeeCRUD.Application.Department.Dtos;
using EmployeeCRUD.Application.Department.Queries;
using EmployeeCRUD.Application.DepartmentModule.Command;
using EmployeeCRUD.IntegrationTests.Factory;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace EmployeeCRUD.IntegrationTests.Tests
{
    public class DepartmentIntegrationTests:IClassFixture<EmployeeCRUDWebApplicationFactory>
    {
        private readonly EmployeeCRUDWebApplicationFactory factory;
        private readonly HttpClient client;
        private readonly ITestOutputHelper output;
        private readonly IServiceScope scope;
        private readonly IMediator mediator;

        public DepartmentIntegrationTests(EmployeeCRUDWebApplicationFactory _factory, ITestOutputHelper _output)
        {
            factory = _factory;
            client = factory.CreateClient();
            output = _output;
            scope = factory.Services.CreateScope();
            mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        }

        [Theory]
        [InlineData("HR")]
        [InlineData("IT")]
        public async Task AddDepartment_ReturnsCreatedDepartment(string deptName)
        {
            //Assign
            var department= new AddDepartmentDapperCommand
            (
                DeptName : deptName
            );
          
            //Act
            var addedDepartment = await mediator.Send(department);

            var result= await mediator.Send(new GetDepartmentQuery());

            //Assert
            result.Should().NotBeNull();
            result.Should().ContainSingle(d => d.Name == deptName);

        }

        [Theory]
        [InlineData("HR")]
        [InlineData("IT")]
        public async Task DeleteDepartment_ReturnSucess(string deptName)
        {
            //Assign
            var department = new AddDepartmentCommand
            (
                DeptName :deptName
            );
            var addedDepartment = await mediator.Send(department);
            var Id= addedDepartment;

            //Act

            var delteCommand = new DeleteDepartmentCommand(addedDepartment.Id);
            var result = await mediator.Send(delteCommand);

            //Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Message.Should().Be("Department removed successfully.");

        }

    }
}
