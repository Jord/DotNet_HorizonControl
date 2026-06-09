using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorizonControlCenterModels
{
    public class UserModel
    {
        public Guid GuidId { get; set; }

        public string? WindowsUserName { get; set; }

        public string? UserFullName { get; set; }

        public int? EmployeeId { get; set; }

        public string? Email { get; set; }

        public int? CreatedByUserId { get; set; }

        public DateTime? CreationDate { get; set; }

        public int? LastUpdatedByUserId { get; set; }

        public DateTime? LastUpdatedDate { get; set; }

        public string? Remarks { get; set; }
    }
}
