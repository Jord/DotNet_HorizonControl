using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorizonControlCenterModels
{
    public class SuitesApplicationModel
    {
        public Guid GuidId { get; set; }

        public string SuiteApplicationName { get; set; } = null!;

        public string? SuiteApplicationDescription { get; set; }

        public int CreateByUserId { get; set; }

        public DateTime CreationDate { get; set; }

        public int LastUpdatedByUserId { get; set; }

        public DateTime LastUpdatedDate { get; set; }

        public bool IsActive { get; set; }

        public Guid SuiteId { get; set; }

        public string? ApplicationType { get; set; }
    }
}
