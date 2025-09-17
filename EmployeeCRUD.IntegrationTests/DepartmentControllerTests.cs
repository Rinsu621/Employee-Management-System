using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace EmployeeCRUD.IntegrationTests
{
    public class DepartmentControllerTests:IClassFixture<EmployeeCRUDWebApplicationFactory>
    {
        private readonly EmployeeCRUDWebApplicationFactory factory;
        private readonly HttpClient client;
        private readonly ITestOutputHelper output;
        private readonly IServiceScope scope;
        private readonly IMediator mediator;

        public DepartmentControllerTests(EmployeeCRUDWebApplicationFactory _factory, ITestOutputHelper _output)
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
            
           
        }

    }
}
