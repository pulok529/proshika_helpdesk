using System;
using System.Collections.Generic;

namespace Helpdesk.Infrastructure.Legacy.Entities;

public partial class TblVendor
{
    public int VendorId { get; set; }

    public string? VendorName { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }

    public string? VendorAddress { get; set; }

    public string? VendorEmail { get; set; }

    public string? VendorEmailOptional { get; set; }

    public string? VendorContactNo { get; set; }

    public string? VendorContactNoOptional { get; set; }

    public string? VendorAddressOptional { get; set; }

    public string? IsActive { get; set; }

    public byte[]? Logo { get; set; }

    public string? LogoName { get; set; }

    public string? LogoType { get; set; }

    public int? VendorCommonId { get; set; }

    public string? Asas { get; set; }
}
