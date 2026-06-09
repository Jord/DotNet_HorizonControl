using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorizonControlCenterModels
{
    public class UserGroupModel
    {
        public Guid GuidId { get; set; }

        public Guid? UserId { get; set; }

        public Guid? GroupId { get; set; }

        public int? CreatedByUserId { get; set; }

        public DateTime? CreationDate { get; set; }

        public int? LastUpdatedByUserId { get; set; }

        public DateTime? LastUpdatedDate { get; set; }
    }
}
