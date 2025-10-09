using EmployeeCRUD.Application.Interface;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.Services
{
  
    public class JobTestService:IJobTestServices
    {
        private readonly ILogger<JobTestService> logger;
        public JobTestService(ILogger<JobTestService> _logger)
        {
            logger = _logger;
        }
        public void FireAndForgetJob()
        {
            logger.LogInformation("Hello from fire and forget job");
        }
        public void RecurringJob()
        {
            logger.LogInformation("Hello from recurring job");
        
        }
        public void DelayedJob()
        {
            logger.LogInformation("Hello from delayed job");
        
        }
        public void ContinuationJob()
        {
            logger.LogInformation("Hello from Continuation Job");
        
        }
    }
}
