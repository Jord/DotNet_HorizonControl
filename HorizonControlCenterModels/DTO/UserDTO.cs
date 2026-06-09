using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorizonControlCenterModels.DTO
{
    public class UserDTO
    {
        public string? WindowsUserName { get; set; }

        public string? UserFullName { get; set; }

        public int? EmployeeId { get; set; }

        public string? Email { get; set; }

        public string? Remarks { get; set; }
        public bool? IsActive { get; set; }
    }
}
