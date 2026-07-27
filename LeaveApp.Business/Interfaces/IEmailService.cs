using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LeaveApp.Business.Interfaces
{
    public interface IEmailService
    {
        Task SendNewLeaveRequestNotificationAsync(
            List<string> managerEmails,
            string employeeFullName,
            string leaveTypeName,
            DateTime startDate,
            DateTime endDate,
            string description);

        Task SendLeaveStatusUpdateNotificationAsync(
            string employeeEmail,
            string employeeFullName,
            string leaveTypeName,
            DateTime startDate,
            DateTime endDate,
            bool isApproved);
    }
}
