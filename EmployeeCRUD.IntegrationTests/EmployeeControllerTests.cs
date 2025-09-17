using EmployeeCRUD.Application.EmployeeModule.Commands;
using EmployeeCRUD.Application.EmployeeModule.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.IntegrationTests
{
    public class EmployeeControllerTests : IClassFixture<EmployeeCRUDWebApplicationFactory>
    {
        private readonly EmployeeCRUDWebApplicationFactory factory;
        private readonly HttpClient client;
        public EmployeeControllerTests(EmployeeCRUDWebApplicationFactory _factory)
        {
            factory = _factory;
            client = factory.CreateClient();
        }
        [Fact]
        public async Task AddEmployee_ReturnsCreatedEmployee()
        {
            //Arrange
            {
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
        }

    }
}
