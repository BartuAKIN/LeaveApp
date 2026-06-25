using System;
using System.Collections.Generic;
using System.Text;

namespace LeaveApp.Domain.Entities
{
    public class Department
    { 
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
