using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.Infrastructure.Legacy.Entities;

public partial class LegacyDbContext : DbContext
{
    public LegacyDbContext(DbContextOptions<LegacyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblActivity> TblActivities { get; set; }

    public virtual DbSet<TblCategory> TblCategories { get; set; }

    public virtual DbSet<TblComment> TblComments { get; set; }

    public virtual DbSet<TblModalChecker> TblModalCheckers { get; set; }

    public virtual DbSet<TblPermittedBanch> TblPermittedBanches { get; set; }

    public virtual DbSet<TblPermittedUser> TblPermittedUsers { get; set; }

    public virtual DbSet<TblRole> TblRoles { get; set; }

    public virtual DbSet<TblSoftwarePermission> TblSoftwarePermissions { get; set; }

    public virtual DbSet<TblSoftwareVersion> TblSoftwareVersions { get; set; }

    public virtual DbSet<TblSubCategory> TblSubCategories { get; set; }

    public virtual DbSet<TblTokenFile> TblTokenFiles { get; set; }

    public virtual DbSet<TblTokenMaster> TblTokenMasters { get; set; }

    public virtual DbSet<TblTokenReturn> TblTokenReturns { get; set; }

    public virtual DbSet<TblTrdCategory> TblTrdCategories { get; set; }

    public virtual DbSet<TblVendor> TblVendors { get; set; }

    public virtual DbSet<TblVendorSupportUser> TblVendorSupportUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblActivity>(entity =>
        {
            entity.HasKey(e => e.ActivityId);

            entity.ToTable("TblActivity");

            entity.Property(e => e.ActivityDateTime).HasColumnType("datetime");
            entity.Property(e => e.ActivitySection).HasMaxLength(500);
        });

        modelBuilder.Entity<TblCategory>(entity =>
        {
            entity.HasKey(e => e.CatId);

            entity.ToTable("TblCategory");

            entity.Property(e => e.CatName).HasMaxLength(50);
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblComment>(entity =>
        {
            entity.HasKey(e => e.CommentId);

            entity.ToTable("TblComment");

            entity.Property(e => e.CommentBy).HasMaxLength(50);
            entity.Property(e => e.CommentDateTime).HasColumnType("datetime");
            entity.Property(e => e.CommentStatus).HasMaxLength(50);
        });

        modelBuilder.Entity<TblModalChecker>(entity =>
        {
            entity.HasKey(e => e.CheckId);

            entity.ToTable("TblModalChecker");

            entity.Property(e => e.CheckId).HasColumnName("Check_Id");
            entity.Property(e => e.ModalView)
                .HasMaxLength(50)
                .HasColumnName("Modal_View");
        });

        modelBuilder.Entity<TblPermittedBanch>(entity =>
        {
            entity.HasKey(e => e.PermittedBranch);

            entity.ToTable("TblPermittedBanch");

            entity.Property(e => e.PermittedBranch).HasColumnName("Permitted_Branch");
            entity.Property(e => e.BranchCode)
                .HasMaxLength(50)
                .HasColumnName("Branch_Code");
            entity.Property(e => e.EntryBy).HasColumnName("Entry_By");
            entity.Property(e => e.EntryDate)
                .HasColumnType("datetime")
                .HasColumnName("Entry_Date");
        });

        modelBuilder.Entity<TblPermittedUser>(entity =>
        {
            entity.HasKey(e => e.PermittedId);

            entity.ToTable("TblPermittedUser");

            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.WorkingAs).HasMaxLength(50);
        });

        modelBuilder.Entity<TblRole>(entity =>
        {
            entity.HasKey(e => e.RoleId);

            entity.ToTable("TblRole");

            entity.Property(e => e.RoleActivity).HasMaxLength(50);
            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<TblSoftwarePermission>(entity =>
        {
            entity.HasKey(e => e.SupportSoftwareId);

            entity.ToTable("TBL_Software_Permission");

            entity.Property(e => e.SupportSoftwareId).HasColumnName("SupportSoftware_ID");
            entity.Property(e => e.SoftCatId).HasColumnName("SoftCatID");
        });

        modelBuilder.Entity<TblSoftwareVersion>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("TblSoftwareVersion");

            entity.Property(e => e.SoftwareVersion).HasMaxLength(50);
        });

        modelBuilder.Entity<TblSubCategory>(entity =>
        {
            entity.HasKey(e => e.SubCatId);

            entity.ToTable("TblSubCategory");

            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.SubCatName).HasMaxLength(50);
        });

        modelBuilder.Entity<TblTokenFile>(entity =>
        {
            entity.HasKey(e => e.TokenFileId).HasName("PK_TblTokenImg");

            entity.ToTable("TblTokenFile");

            entity.Property(e => e.IssueDate).HasColumnType("datetime");
            entity.Property(e => e.TokenFileName).HasMaxLength(500);
            entity.Property(e => e.TokenFileType).HasMaxLength(500);
        });

        modelBuilder.Entity<TblTokenMaster>(entity =>
        {
            entity.HasKey(e => e.TokenId);

            entity.ToTable("TblTokenMaster");

            entity.Property(e => e.AssignedDateTime).HasColumnType("datetime");
            entity.Property(e => e.IssueDate).HasColumnType("datetime");
            entity.Property(e => e.Priority).HasMaxLength(500);
            entity.Property(e => e.ReturnToken).HasMaxLength(50);
            entity.Property(e => e.SolveDateTime).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.TokenReceiveDateTime).HasColumnType("datetime");
            entity.Property(e => e.TokenSerialId).HasMaxLength(50);
        });

        modelBuilder.Entity<TblTokenReturn>(entity =>
        {
            entity.HasKey(e => e.TokenReturnId);

            entity.ToTable("TblTokenReturn");

            entity.Property(e => e.TokenReturnId).ValueGeneratedNever();
        });

        modelBuilder.Entity<TblTrdCategory>(entity =>
        {
            entity.HasKey(e => e.TrdCatId);

            entity.ToTable("TBL_TrdCategory");

            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.TrdCatName).HasMaxLength(50);
        });

        modelBuilder.Entity<TblVendor>(entity =>
        {
            entity.HasKey(e => e.VendorId);

            entity.ToTable("TblVendor");

            entity.Property(e => e.Asas)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("asas");
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasMaxLength(50);
            entity.Property(e => e.LogoName).HasMaxLength(250);
            entity.Property(e => e.LogoType).HasMaxLength(250);
            entity.Property(e => e.VendorAddress).HasMaxLength(250);
            entity.Property(e => e.VendorAddressOptional).HasMaxLength(250);
            entity.Property(e => e.VendorCommonId).HasColumnName("Vendor_CommonId");
            entity.Property(e => e.VendorContactNo).HasMaxLength(20);
            entity.Property(e => e.VendorContactNoOptional).HasMaxLength(20);
            entity.Property(e => e.VendorEmail).HasMaxLength(250);
            entity.Property(e => e.VendorEmailOptional).HasMaxLength(250);
            entity.Property(e => e.VendorName).HasMaxLength(500);
        });

        modelBuilder.Entity<TblVendorSupportUser>(entity =>
        {
            entity.HasKey(e => e.VendorSupportId);

            entity.ToTable("TblVendorSupportUser");

            entity.Property(e => e.Active).HasMaxLength(50);
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.Spdesignation)
                .HasMaxLength(50)
                .HasColumnName("SPDesignation");
            entity.Property(e => e.Spemail)
                .HasMaxLength(50)
                .HasColumnName("SPEmail");
            entity.Property(e => e.Spfname)
                .HasMaxLength(50)
                .HasColumnName("SPFName");
            entity.Property(e => e.Splname)
                .HasMaxLength(50)
                .HasColumnName("SPLName");
            entity.Property(e => e.UserName).HasMaxLength(50);
            entity.Property(e => e.UserPassword).HasMaxLength(50);
            entity.Property(e => e.VendorAddress).HasMaxLength(50);
            entity.Property(e => e.VendorCommonId).HasColumnName("Vendor_CommonId");
            entity.Property(e => e.VendorContactNo).HasMaxLength(50);
            entity.Property(e => e.VendorName).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
