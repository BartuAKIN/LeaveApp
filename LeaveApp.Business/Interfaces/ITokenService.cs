using System;
using System.Collections.Generic;
using System.Text;
using LeaveApp.Domain.Entities;

namespace LeaveApp.Business.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(ApplicationUser user);
    }
}
