using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorizonControlCenterModels
{
    public class AuditLogFormDetails
    {
        public string? currentappname { get; set; }

        public int currentappid { get; set; }

        public string? currentformname { get; set; }

        public int currentformid { get; set; }

        public string? requestappname { get; set; }

        public int requestappid { get; set; }
    }
}
