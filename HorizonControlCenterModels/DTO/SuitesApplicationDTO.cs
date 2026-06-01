using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorizonControlCenterModels.DTO
{
    public class SuitesApplicationDTO
    {
        public string? ApplicationName { get; set; }
        public string? ApplicationDescription { get; set; }
        public bool? IsActive { get; set; }
        public Guid? SuiteId { get; set; }
        public int? ParentApplicationId { get; set; }
        public string? ApplicationGroup { get; set; }
        public bool? HasChildApplications { get; set; }
        public bool? IsReadyToReplicate { get; set; }
        public int? PermissionCodeSeries { get; set; }
        public string? BasepathUrl { get; set; }
        public string? ApiBasepathUrl { get; set; }
        public string? BusinessOwner { get; set; }
        public string? BusinessUserGroup { get; set; }
        public string? Developers { get; set; }
        public int? ApiPortNumber { get; set; }
        public string? ApplicationType { get; set; }
    }
}
