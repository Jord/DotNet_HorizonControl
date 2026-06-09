using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorizonControlCenterModels.DTO
{
    public class GroupRoleDTO
    {
        public Guid? SuiteApplicationId { get; set; }

        public string? GroupName { get; set; }

        public bool? IsActive { get; set; }

        public string? Remarks { get; set; }

        public Guid? ApplicationRoleId { get; set; }
    }
}
