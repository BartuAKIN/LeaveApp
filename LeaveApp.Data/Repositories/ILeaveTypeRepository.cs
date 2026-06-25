using LeaveApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeaveApp.Data.Repositories
{
    public interface ILeaveTypeRepository
    {
        List<LeaveType> GetAll();

        LeaveType GetById(int id);

        void Add(LeaveType leaveType);

        void Update(LeaveType leaveType);

        void Delete(int id);
    }
}
