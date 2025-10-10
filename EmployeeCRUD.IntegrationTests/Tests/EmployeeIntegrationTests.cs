using EmployeeCRUD.Application.EmployeeModule.Commands;
using EmployeeCRUD.Application.EmployeeModule.Dtos;
using EmployeeCRUD.Application.EmployeeModule.Queries;
using EmployeeCRUD.IntegrationTests.Factory;
using FluentAssertions;
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

namespace EmployeeCRUD.IntegrationTests.Tests
{
    public class EmployeeIntegrationTests : IClassFixture<EmployeeCRUDWebApplicationFactory>
    {
        private readonly EmployeeCRUDWebApplicationFactory factory;
        private readonly HttpClient client;
        private readonly ITestOutputHelper output;
        private readonly IServiceScope scope;
        private readonly IMediator mediator;

        public EmployeeIntegrationTests(EmployeeCRUDWebApplicationFactory _factory, ITestOutputHelper _output)
        {
            factory = _factory;
            client = factory.CreateClient();
            output = _output;
            scope = factory.Services.CreateScope();
            mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        }
        //[Fact]
        //public async Task AddEmployee_ReturnsCreatedEmployee()
        //{
        //    //Arrange
        //    var employeeDto = new AddEmployeeCommand
        //    {
        //        EmpName = "John Doe",
        //        Email = "johndoe@gmail.com",
        //        Phone = "9877677890"
        //    };
        //    var command = new AddEmployeeCommand(employeeDto);

        //    //Act
        //    var response = await client.PostAsJsonAsync("/api/Employee", command);
        //    response.EnsureSuccessStatusCode();
        //    var createdEmployee = await response.Content.ReadFromJsonAsync<EmployeeResponseDto>();
        //    //Assert
        //    Assert.NotNull(createdEmployee);
        //    Assert.Equal("John Doe", createdEmployee.EmpName);
        //    Assert.Equal("johndoe@gmail.com", createdEmployee.Email);
        //    Assert.Equal("9877677890", createdEmployee.Phone);
        //}

        //[Fact]
        //public async Task AddEmployee_GetBadRequest_WhenEmployeeNameEmpty()
        //{
        //    //Arrange
        //    var employeeDto = new EmployeeDto
        //    {
        //        EmpName = "",
        //        Email = "test1@gmail.com",
        //        Phone = "9876543210"
        //    };
        //    var command = new AddEmployeeCommand(employeeDto);

        //    //Act
        //    var response = await client.PostAsJsonAsync("/api/Employee", command);

        //    //Assert
        //    Assert.False(response.IsSuccessStatusCode);
        //    Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        //}
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

        //[Fact]
        //public async Task AddEmployeeDapper_ReturnsCreatedEmployee()
        //{
        //    // Arrange
        //    var command = new AddEmployeeDapperCommand
        //    (
        //        EmpName : "Ram Ram",
        //        Email: "ram@gmail.com",
        //        Phone: "9876544321",
        //        Role:"Employee"
        //    );
           

        //    // Act
        //    var response = await client.PostAsJsonAsync("/api/Employee/add-employee-using-dapper", command);

        //    // Assert
        //    response.EnsureSuccessStatusCode();
        //    var createdEmployee = await response.Content.ReadFromJsonAsync<EmployeeResponseDto>();
        //    Assert.NotNull(createdEmployee);
        //    Assert.Equal(command.EmpName, createdEmployee.EmpName);
        //    Assert.Equal(command.Email, createdEmployee.Email);
        //    Assert.Equal(command.Phone, createdEmployee.Phone);
        //}

        //[Theory]
        //[InlineData("TestB", "testb@gmail.com", "9876543212","Employee")]
        //public async Task UpdateEmployee_ReturnUpdatedEmployee(string EmpName, string Email, string Phone, string Role)
        //{
        ////    using var scope = factory.Services.CreateScope();
        ////    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        //    var command = new AddEmployeeDapperCommand
        //    (
        //        EmpName : EmpName,
        //        Email : Email,
        //        Phone : Phone,
        //        Role: Role
        //    );
          
        //    var createdEmployee = await mediator.Send(command);

        //    output.WriteLine($"Created Employee Id: {createdEmployee.Id}");

        //    var updateCommand = new UpdateEmployeeSpCommand
        //    (
        //        Id: createdEmployee.Id,
        //        EmpName : "Updated Employee",
        //        Email : "updated@gmail.com",
        //        Phone : "9876544321"
        //    );

           
        //    var updatedEmployee = await mediator.Send(updateCommand);

        //    output.WriteLine($"Updated Employee: {updatedEmployee.EmpName}, {updatedEmployee.Email}, {updatedEmployee.Phone}");
        //    //Assert
        //    Assert.NotNull(updatedEmployee);
        //    Assert.Equal(updateCommand.EmpName, updatedEmployee.EmpName);
        //    Assert.Equal(updateCommand.Email, updatedEmployee.Email);
        //    Assert.Equal(updateCommand.Phone, updatedEmployee.Phone);
        //}

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
