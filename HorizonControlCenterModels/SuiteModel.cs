using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorizonControlCenterModels
{
    public class SuiteModel
    {
        public string? SuiteName { get; set; }

        public string? SuiteCode { get; set; }

        public int? CreatedByUserId { get; set; }

        public DateTime? CreationDate { get; set; }

        public int? LastUpdatedByUserId { get; set; }

        public DateTime? LastUpdatedDate { get; set; }

        public bool? IsActive { get; set; }

        public string? SuiteDescription { get; set; }

        public string? DeActiveRemark { get; set; }

        public Guid GuidId { get; set; }
    }
}
