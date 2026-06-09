using System;
using System.Collections.Generic;

namespace HorizonControlCenterDAL.Entities;

public partial class GroupRole
{
    public Guid GuidD { get; set; }

    public Guid? SuiteApplicationId { get; set; }

    public string? GroupName { get; set; }

    public int? CreateByUserId { get; set; }

    public DateTime? CreationDate { get; set; }

    public int? LastUpdatedByUserId { get; set; }

    public DateTime? LastUpdatedDate { get; set; }

    public bool? IsActive { get; set; }

    public string? Remarks { get; set; }

    public Guid? ApplicationRoleId { get; set; }
}
