using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorizonControlCenterModels.DTO
{
    public class SuitesDTO
    {
        public string? SuiteName { get; set; }
        public string? SuiteCode { get; set; }
        public bool? IsActive { get; set; }
        public string? SuiteDescription { get; set; }
        public string? DeActiveRemark { get; set; }
        //public int? LastUpdatedByUserId { get; set; }
    }
}
