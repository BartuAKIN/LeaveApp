using System;
using System.Collections.Generic;
using System.Text;
using LeaveApp.Domain.Enums;

namespace LeaveApp.Business.DTOs
{
    public class UpdateLeaveRequestStatusDto
    {
        public LeaveStatus NewStatus {  get; set; }
    }
}
