using EmployeeCRUD.Application.AuthModel.Commands;
using EmployeeCRUD.Domain.Entities;
using EmployeeCRUD.IntegrationTests.Factory;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace EmployeeCRUD.IntegrationTests.Tests
{
    public class AuthIntegrationTests:IClassFixture<EmployeeCRUDWebApplicationFactory>
    {
        private readonly EmployeeCRUDWebApplicationFactory _factory;
        private readonly HttpClient _httpClient;
        private readonly ITestOutputHelper _output;
        private readonly IServiceScope _scope;
        private readonly IMediator _mediator;

        public AuthIntegrationTests(EmployeeCRUDWebApplicationFactory factory, ITestOutputHelper output)
        {
            _factory = factory;
            _httpClient = _factory.CreateClient(); 
            _output = output;

           
            var scope = _factory.Services.CreateScope();
            _mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        }

        [Fact]
        public async Task UserLogin_ReturnedTokenandRefreshToken()
        {


            var loginCredential = new LoginCommand
                (
                Email: "admin@gmail.com",
                Password: "Admin@123"
                );
            var response= await _mediator.Send(loginCredential);
            Assert.NotNull(response);
            Assert.False(string.IsNullOrEmpty(response.Token));
            Assert.False(string.IsNullOrEmpty(response.RefreshToken));


        }

        [Fact]
        public async Task ChangePassword_ReturnSuccess()
        {
            var scope = _factory.Services.CreateScope();
            var services = scope.ServiceProvider;

            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var mediator = services.GetRequiredService<IMediator>();

            var adminUser = await userManager.FindByEmailAsync("admin@gmail.com");

            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, adminUser.Id) };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            var httpContext = new DefaultHttpContext { User = claimsPrincipal };
            var httpContextAccessor = new HttpContextAccessor { HttpContext = httpContext };

            var handler = new ChangePasswordCommandHandler(userManager, httpContextAccessor);

            var passwordChange = new ChangePasswordCommand(
                currentPassword: "Admin@123",
                newPassword: "Default@123"
            );

            var result = await handler.Handle(passwordChange, default);
            Assert.True(result);
        }



    }
}
