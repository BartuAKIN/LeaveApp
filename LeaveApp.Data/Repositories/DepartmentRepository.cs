using LeaveApp.Data.Context;
using LeaveApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeaveApp.Data.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly AppDbContext _context;

        public DepartmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(Department department)
        {
            _context.Departments.Add(department);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var department = GetById(id);
            _context.Departments.Remove(department);
            _context.SaveChanges();
        }

        public List<Department> GetAll()
        {
            return _context.Departments.ToList();
        }

        public Department GetById(int id)
        {
            return _context.Departments.FirstOrDefault(x => x.Id == id);
        }

        public void Update(Department department)
        {
            _context.Departments.Update(department);
            _context.SaveChanges();
        }
    }
}
