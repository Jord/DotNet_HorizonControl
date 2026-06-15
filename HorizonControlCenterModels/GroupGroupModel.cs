using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorizonControlCenterModels
{
    public class GroupGroupModel
    {
        public Guid GuidId { get; set; }

        public Guid GroupId { get; set; }

        public Guid MapToGroupId { get; set; }

        public int? CreatedByUserId { get; set; }

        public DateTime? CreationDate { get; set; }

        public int? LastUpdatedByUserId { get; set; }

        public DateTime? LastUpdatedDate { get; set; }

        public string? GroupName { get; set; }

        public string? MapToGroupName { get; set; }
    }
}
