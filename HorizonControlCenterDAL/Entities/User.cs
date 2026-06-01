using System;
using System.Collections.Generic;

namespace HorizonControlCenterDAL.Entities;

public partial class User
{
    public Guid GuidId { get; set; }

    public string? WindowsUserName { get; set; }

    public string? UserFullName { get; set; }

    public int? EmployeeId { get; set; }

    public string? Email { get; set; }

    public string? Authentication { get; set; }

    public int? CreatedByUserId { get; set; }

    public DateTime? CreationDate { get; set; }

    public int? LastUpdatedByUserId { get; set; }

    public DateTime? LastUpdatedDate { get; set; }

    public string? UserAccountType { get; set; }

    public string? Password { get; set; }

    public string? Remarks { get; set; }

    public string? UserType { get; set; }
}
