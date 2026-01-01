using System;
using System.Collections.Generic;

namespace Helpdesk.Infrastructure.Legacy.Entities;

public partial class TblVendorSupportUser
{
    public int VendorSupportId { get; set; }

    public string? VendorName { get; set; }

    public string? VendorAddress { get; set; }

    public string? VendorContactNo { get; set; }

    public string? Spfname { get; set; }

    public string? Splname { get; set; }

    public string? Spemail { get; set; }

    public string? Spdesignation { get; set; }

    public string? UserPassword { get; set; }

    public string? Active { get; set; }

    public string? UserName { get; set; }

    public int? EmpId { get; set; }

    public int? RoleId { get; set; }

    public int? VendorCommonId { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }
}
