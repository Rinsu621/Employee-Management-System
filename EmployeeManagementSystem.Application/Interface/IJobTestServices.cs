

namespace EmployeeManagementSystem.Application.Interface
{
    public interface IJobTestServices
    {
        void FireAndForgetJob();
        void RecurringJob();
        void DelayedJob();
        void ContinuationJob();
    }
}
