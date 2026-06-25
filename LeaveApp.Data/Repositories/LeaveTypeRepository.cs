using LeaveApp.Data.Context;
using LeaveApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeaveApp.Data.Repositories
{
    public class LeaveTypeRepository : ILeaveTypeRepository
    {
        private readonly AppDbContext _context;

        public LeaveTypeRepository(AppDbContext context)
        {
            _context = context;
        }
        public void Add(LeaveType leaveType)
        {
            _context.LeaveTypes.Add(leaveType);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var leaveType = GetById(id);
            _context.LeaveTypes.Remove(leaveType);
            _context.SaveChanges();
        }

        public List<LeaveType> GetAll()
        {
            return _context.LeaveTypes.ToList();
        }

        public LeaveType GetById(int id)
        {
            return _context.LeaveTypes.FirstOrDefault(x => x.Id == id);
        }

        public void Update(LeaveType leaveType)
        {
            _context.LeaveTypes.Update(leaveType);
            _context.SaveChanges();
        }
    }
}
