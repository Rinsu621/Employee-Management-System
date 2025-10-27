using EmployeeManagementSystem.Application.DepartmentModule.Command;
using EmployeeManagementSystem.Application.EmployeeModule.Commands;
using EmployeeManagementSystem.Application.EmployeeModule.Dtos;
using EmployeeManagementSystem.Application.Exceptions;
using EmployeeManagementSystem.IntegrationTests.Factory;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit.Abstractions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EmployeeManagementSystem.IntegrationTests.Tests
{
    public class EmployeeIntegrationTests : IClassFixture<EmployeeManagementSystemWebApplicationFactory>
    {
        private readonly EmployeeManagementSystemWebApplicationFactory factory;
        private readonly HttpClient client;
        private readonly ITestOutputHelper output;
        private readonly IServiceScope scope;
        private readonly IMediator mediator;

        public EmployeeIntegrationTests(EmployeeManagementSystemWebApplicationFactory _factory, ITestOutputHelper _output)
        {
            factory = _factory;
            client = factory.CreateClient();
            output = _output;
            scope = factory.Services.CreateScope();
            mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        }
        [Fact]
        public async Task AddEmployee_ReturnsCreatedEmployee()
        {
            //Arrange
            var employee = new AddEmployeeCommand
            (
                EmpName: "John Doe",
                Email: "johndoe@gmail.com",
                Phone: "9877677890",
                DepartmentId: null,
                Role: "Employee"
            );

            //Act
            var createdEmployee = await mediator.Send(employee);

            //Assert
            Assert.NotNull(createdEmployee);
            Assert.Equal("John Doe", createdEmployee.EmpName);
            Assert.Equal("johndoe@gmail.com", createdEmployee.Email);
            Assert.Equal("9877677890", createdEmployee.Phone);
        }

        [Fact]
        public async Task AddEmployee_GetBadRequest_WhenEmployeeNameEmpty()
        {
            //Arrange
            var employee = new AddEmployeeCommand   
            (
                EmpName : "",
                Email : "test1@gmail.com",
                Phone : "9876543210",
                DepartmentId: new Guid(),
                Role: "Employee"
            );
            //Act
            Func<Task> act = async () => await mediator.Send(employee);

            //Assert
            await act.Should().ThrowAsync<CustomValidationException>().WithMessage("Validation Failed");

        }
        [Fact]
        public async Task AddEmployee_GetBadRequest_WhenInvalidNumber()
        {
            //Arrange
            var command = new AddEmployeeSPCommand
            (
                EmpName: "TestA",
                Email: "testa@gmail.com",
                Phone: "1234567890"
            );
            

            //Act
            var response = await client.PostAsJsonAsync("/api/Employee/using-sp", command);

            //Assert

            Assert.False(response.IsSuccessStatusCode);
            var error = await response.Content.ReadAsStringAsync();
            Assert.Contains("Phone number must be 10 digits and must start with 98.", error);
        }

        [Fact]
        public async Task AddEmployeeDapper_ReturnsCreatedEmployee()
        {
            var department = new AddDepartmentDapperCommand
            (
                DeptName : "ABC"
            );
            var deptResponse = await mediator.Send(department);
            // Arrange
            var command = new AddEmployeeDapperCommand
            (
                EmpName: "Ram Ram",
                Email: "ram@gmail.com",
                Phone: "9876544321",
                DepartmentId: deptResponse.Id,
                Role: "Employee"
            );


            // Act
           var employeeCreated= await mediator.Send(command);

            // Assert
          
            Assert.NotNull(employeeCreated);
            Assert.Equal(command.EmpName, employeeCreated.EmpName);
            Assert.Equal(command.Email, employeeCreated.Email);
            Assert.Equal(command.Phone, employeeCreated.Phone);
        }

        [Theory]
        [InlineData("TestB", "testb@gmail.com", "9876543212", "Employee")]
        public async Task UpdateEmployee_ReturnUpdatedEmployee(string EmpName, string Email, string Phone, string Role)
        {
            //    using var scope = factory.Services.CreateScope();
            //    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var department = new AddDepartmentDapperCommand
           (
               DeptName: "ABC"
           );
            var deptResponse = await mediator.Send(department);

            var command = new AddEmployeeDapperCommand
            (
                EmpName: EmpName,
                Email: Email,
                Phone: Phone,
                DepartmentId: deptResponse.Id,
                Role: Role
            );

            var createdEmployee = await mediator.Send(command);

            output.WriteLine($"Created Employee Id: {createdEmployee.Id}");

            var updateCommand = new UpdateEmployeeWithDapperCommand
            (
                Id: createdEmployee.Id,
                EmpName: "Updated Employee",
                Email: "updated@gmail.com",
                DepartmentId: createdEmployee.Id,
                Role: "Manager",
                Phone: "9876544321"
            );


        var updatedEmployee = await mediator.Send(updateCommand);

        output.WriteLine($"Updated Employee: {updatedEmployee.Name}, {updatedEmployee.Email}, {updatedEmployee.Phone}");
                //Assert
                Assert.NotNull(updatedEmployee);
                Assert.Equal(updateCommand.EmpName, updatedEmployee.Name);
                Assert.Equal(updateCommand.Email, updatedEmployee.Email);
                Assert.Equal(updateCommand.Phone, updatedEmployee.Phone);
            }

    //[Theory]
    //[InlineData("TestA","testa@gmail.com","9812322123")]
    //[InlineData("TestB", "testb@gmail.com", "9812322122")]
    //public async Task GetAllEmployee_ReturnAllEmployee(string EmpName, string Email, string Phone)
    //{
    //    var employeeDto = new AddEmployeeDapperCommand
    //    (
    //        EmpName : EmpName,
    //        Email: Email,
    //        Phone: Phone
    //    );

    //    var createdEmployee = await mediator.Send(employeeDto);

    //    var response= await mediator.Send(new GetAllEmployeesQuery());
    //    response.Should().NotBeEmpty();

    //    foreach (var emp in response)
    //    {
    //        output.WriteLine($"Id: {emp.Id}, Name: {emp.EmpName}, Email: {emp.Email}, Phone: {emp.Phone}");
    //    }

    //}

}
    }

