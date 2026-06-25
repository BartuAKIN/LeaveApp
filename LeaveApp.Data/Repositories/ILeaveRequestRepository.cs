using LeaveApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeaveApp.Data.Repositories
{
    public interface ILeaveRequestRepository
    {
        List<LeaveRequest> GetAll();

        LeaveRequest GetById(int id);

        List<LeaveRequest> GetByUserId(int userId);

        List<LeaveRequest> GetByDepartmentId(int departmentId);

        void Add(LeaveRequest leaveRequest);

        void Update(LeaveRequest leaveRequest);
    }
}
