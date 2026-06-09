using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorizonControlCenterModels
{
    public class GroupModel
    {
        public Guid GuidId { get; set; }

        public string Name { get; set; } = null!;

        public bool IsActive { get; set; }

        public string? Remarks { get; set; }

        public int CreatedByUserId { get; set; }

        public DateTime CreationDate { get; set; }

        public int LastUpdatedByUserId { get; set; }

        public DateTime LastUpdatedDate { get; set; }

        public string GroupType { get; set; } = null!;
    }
}
