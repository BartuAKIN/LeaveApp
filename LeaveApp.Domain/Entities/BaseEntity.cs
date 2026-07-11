using System;
using System.Collections.Generic;
using System.Text;

namespace LeaveApp.Domain.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        // ileride: public DateTime? UpdatedDate { get; set; }
        // ileride: public bool IsDeleted { get; set; }
    }
}
