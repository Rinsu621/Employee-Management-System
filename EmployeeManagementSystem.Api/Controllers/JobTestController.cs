using EmployeeManagementSystem.Application.Interface;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobTestController : ControllerBase
    {
        private readonly IJobTestServices jobTestServices;
        private readonly IBackgroundJobClient backgroundJobClient;
        private readonly IRecurringJobManager recurringJobManager;
        public JobTestController(IJobTestServices _jobTestServices , IBackgroundJobClient _backgroundJobClient,IRecurringJobManager _recurringJobManager)
        {
            jobTestServices = _jobTestServices;
            backgroundJobClient = _backgroundJobClient;
            recurringJobManager = _recurringJobManager;
        }

        [HttpGet("fire-forget")]
        public IActionResult CreateFireAndForgetJob()
        {
            backgroundJobClient.Enqueue(() => jobTestServices.FireAndForgetJob());
            return Ok();
        }

        [HttpGet("delayed-job")]
        public IActionResult CreateDelayedJob()
        {
            backgroundJobClient.Schedule(() => jobTestServices.DelayedJob(), TimeSpan.FromSeconds(60));
            return Ok();
        }
        [HttpGet("recurring-job")]
        public IActionResult CreateRecurringJob()
        {
            recurringJobManager.AddOrUpdate("jobId",() => jobTestServices.RecurringJob(), Cron.Minutely);
            return Ok();
        }

        [HttpGet("continuation-job")]
        public IActionResult CreateContinuationJob()
        {
            var parentJobId = backgroundJobClient.Enqueue(() => jobTestServices.FireAndForgetJob());
            backgroundJobClient.ContinueJobWith(parentJobId, () => jobTestServices.ContinuationJob());
            return Ok();
        }

    }
}
