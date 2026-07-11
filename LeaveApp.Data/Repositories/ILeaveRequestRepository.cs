using LeaveApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeaveApp.Data.Repositories
{
    public interface ILeaveRequestRepository : IGenericRepository<LeaveRequest>
    {
        List<LeaveRequest> GetByUserId(int userId);

        List<LeaveRequest> GetByDepartmentId(int departmentId);

    }
}
