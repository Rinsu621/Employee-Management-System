using EmployeeCRUD.Application.EmployeeModule.Commands;
using EmployeeCRUD.Application.EmployeeModule.Dtos;
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

namespace EmployeeCRUD.IntegrationTests
{
    public class EmployeeControllerTests : IClassFixture<EmployeeCRUDWebApplicationFactory>
    {
        private readonly EmployeeCRUDWebApplicationFactory factory;
        private readonly HttpClient client;
        private readonly ITestOutputHelper output;

        public EmployeeControllerTests(EmployeeCRUDWebApplicationFactory _factory, ITestOutputHelper _output)
        {
            factory = _factory;
            client = factory.CreateClient();
            output = _output;
        }
        [Fact]
        public async Task AddEmployee_ReturnsCreatedEmployee()
        {
            //Arrange
            var employeeDto = new EmployeeDto
            {
                EmpName = "John Doe",
                Email = "johndoe@gmail.com",
                Phone = "9877677890"
            };
            var command = new AddEmployeeCommand(employeeDto);

            //Act
            var response = await client.PostAsJsonAsync("/api/Employee", command);
            response.EnsureSuccessStatusCode();
            var createdEmployee = await response.Content.ReadFromJsonAsync<EmployeeResponseDto>();
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
            var employeeDto = new EmployeeDto
            {
                EmpName = "",
                Email = "test1@gmail.com",
                Phone = "9876543210"
            };
            var command = new AddEmployeeCommand(employeeDto);

            //Act
            var response = await client.PostAsJsonAsync("/api/Employee", command);

            //Assert
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Fact]
        public async Task AddEmployee_GetBadRequest_WhenInvalidNumber()
        {
            //Arrange
            var employeeDto = new EmployeeDto
            {
                EmpName = "TestA",
                Email = "testa@gmail.com",
                Phone = "1234567890"
            };
            var command = new AddEmployeeSPCommand(employeeDto);

            //Act
            var response = await client.PostAsJsonAsync("/api/Employee/using-sp", command);

            //Assert

            Assert.False(response.IsSuccessStatusCode);
            var error = await response.Content.ReadAsStringAsync();
            Assert.Contains("Phone number must be 10 digits and must start with 98.", error);
        }

        [Fact]
        public async Task AddEmployeeDapper_ReturnsCreateddEmployee()
        {
            // Arrange
            var employeeDto = new EmployeeDto
            {
                EmpName = "Testz",
                Email = "testz@gmail.com",
                Phone = "9876544321"
            };
            var command = new AddEmployeeDapperCommand(employeeDto);

            // Act
            var response = await client.PostAsJsonAsync("/api/Employee/add-employee-using-dapper", command);

            // Log the response for debugging
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error Response: {errorContent}");
            }

            // Assert
            response.EnsureSuccessStatusCode();
            var createdEmployee = await response.Content.ReadFromJsonAsync<EmployeeResponseDto>(
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.NotNull(createdEmployee);
            Assert.Equal(employeeDto.EmpName, createdEmployee.EmpName);
            Assert.Equal(employeeDto.Email, createdEmployee.Email);
            Assert.Equal(employeeDto.Phone, createdEmployee.Phone);
        }

    }
}
