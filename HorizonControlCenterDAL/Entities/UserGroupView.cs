using System;
using System.Collections.Generic;

namespace HorizonControlCenterDAL.Entities;

public partial class UserGroupView
{
    public Guid? GuidId { get; set; }

    public Guid? UserId { get; set; }

    public Guid? GroupId { get; set; }

    public Guid? MapToGroupId { get; set; }

    public string? MappingType { get; set; }

    public int? CreatedByUserId { get; set; }

    public DateTime? CreationDate { get; set; }

    public int? LastUpdatedByUserId { get; set; }

    public DateTime? LastUpdatedDate { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? MapGroupName { get; set; }

    public string? GroupType { get; set; }
}
