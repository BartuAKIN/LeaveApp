using LeaveApp.Data.Context;
using LeaveApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using LeaveApp.Data.Repositories;

namespace LeaveApp.Data.Repositories
{
    public class LeaveRequestRepository : ILeaveRequestRepository
    {
        private readonly AppDbContext _context;

        public LeaveRequestRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(LeaveRequest leaveRequest)
        {
            _context.LeaveRequests.Add(leaveRequest);
            _context.SaveChanges();
        }

        public List<LeaveRequest> GetAll()
        {
           return _context.LeaveRequests.ToList();
        }

        public List<LeaveRequest> GetByDepartmentId(int departmentId)
        {
            return _context.LeaveRequests.Where(x => x.UserId == departmentId).ToList();
        }

        public LeaveRequest GetById(int id)
        {
            return _context.LeaveRequests.FirstOrDefault(x => x.Id == id);
        }

        public List<LeaveRequest> GetByUserId(int userId)
        {
            return _context.LeaveRequests.Where(x => x.UserId == userId).ToList();
        }

        public void Update(LeaveRequest leaveRequest)
        {
            _context.LeaveRequests.Update(leaveRequest);
            _context.SaveChanges();
        }
    }
}
