using System;
using System.Collections.Generic;

namespace HorizonControlCenterDAL.Entities;

public partial class Suite
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
