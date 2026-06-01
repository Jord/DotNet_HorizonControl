using System;
using System.Collections.Generic;

namespace HorizonControlCenterDAL.Entities;

public partial class GroupWithCategoryView
{
    public Guid? GuidId { get; set; }

    public string? Name { get; set; }

    public bool? IsActive { get; set; }

    public string? Remarks { get; set; }

    public int? CreatedByUserId { get; set; }

    public DateTime? CreationDate { get; set; }

    public int? LastUpdatedByUserId { get; set; }

    public DateTime? LastUpdatedDate { get; set; }

    public Guid? GroupCategoryId { get; set; }

    public string? Category { get; set; }

    public string? GroupType { get; set; }
}
