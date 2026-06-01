using System;
using System.Collections.Generic;

namespace HorizonControlCenterDAL.Entities;

public partial class GroupCategory
{
    public Guid GuidId { get; set; }

    public string? Name { get; set; }

    public int? CreatedByUserId { get; set; }

    public DateTime? CreationDate { get; set; }

    public int? LastUpdatedByUserId { get; set; }

    public DateTime? LastUpdatedDate { get; set; }
}
