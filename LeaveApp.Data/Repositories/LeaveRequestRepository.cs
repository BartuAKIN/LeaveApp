using LeaveApp.Data.Context;
using LeaveApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using LeaveApp.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LeaveApp.Data.Repositories
{
    public class LeaveRequestRepository : GenericRepository<LeaveRequest>, ILeaveRequestRepository
    {

        public LeaveRequestRepository(AppDbContext context) : base(context)
        {
        }

        public List<LeaveRequest> GetByDepartmentId(int departmentId)
        {
            return _context.LeaveRequests
                .Include(lr => lr.User)
                .Where(lr => _context.Users.Any(u=> u.Id == lr.UserId && u.DepartmentId == departmentId))
                .ToList();
        }

        public List<LeaveRequest> GetByUserId(int userId)
        {
            return _context.LeaveRequests.Where(x => x.UserId == userId).ToList();
        }

    }
}
