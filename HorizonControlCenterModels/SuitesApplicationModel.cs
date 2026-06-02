using HorizonControlCenterModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HorizonControlCenterModels
{
    public class SuitesApplicationModel
    {
        public Guid GuidId { get; set; }
        public string? ApplicationName { get; set; }
        public string? ApplicationDescription { get; set; }
        public int? CreateByUserId { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? LastUpdatedByUserId { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
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

        [JsonIgnore]
        public int? ApplicationType { get; set; }

        public string? ApplicationTypeName
        {
            get
            {
                if (ApplicationType.HasValue && Enum.IsDefined(typeof(ApplicationType), ApplicationType.Value))
                {
                    return ((ApplicationType)ApplicationType.Value).ToString();
                }
                return null;
            }
        }
    }
}
