using LeaveApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeaveApp.Data.Repositories
{
    public interface IDepartmentRepository
    {
        List<Department> GetAll();

        Department GetById(int id);

        void Add(Department department);

        void Update(Department department);

        void Delete(int id);
    }
}
