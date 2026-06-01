using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorizonControlCenterModels.DTO
{
    public class GroupSummaryDTO
    {
        public Guid? GuidId { get; set; }
        public string? GroupType { get; set; }
        public string? GroupName { get; set; }
    }
}
