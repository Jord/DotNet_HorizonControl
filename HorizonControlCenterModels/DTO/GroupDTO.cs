using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorizonControlCenterModels.DTO
{
    public class GroupDTO
    {
        public string Name { get; set; } = null!;
        public string GroupType { get; set; } = null!;
        public bool IsActive { get; set; }

        public string? Remarks { get; set; }

    }
}
