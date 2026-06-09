using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace HorizonControlCenterDAL.Entities;

public partial class horizoncontrolContext : DbContext
{
    public horizoncontrolContext(DbContextOptions<horizoncontrolContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<GroupGroup> GroupGroups { get; set; }

    public virtual DbSet<GroupRole> GroupRoles { get; set; }

    public virtual DbSet<Suite> Suites { get; set; }

    public virtual DbSet<SuiteApplication> SuiteApplications { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserGroup> UserGroups { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasKey(e => e.GuidId).HasName("group_pkey");

            entity.ToTable("group");

            entity.HasIndex(e => e.Name, "name_uk").IsUnique();

            entity.Property(e => e.GuidId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("guid_id");
            entity.Property(e => e.CreatedByUserId).HasColumnName("created_by_user_id");
            entity.Property(e => e.CreationDate).HasColumnName("creation_date");
            entity.Property(e => e.GroupType)
                .HasColumnType("character varying")
                .HasColumnName("group_type");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.LastUpdatedByUserId).HasColumnName("last_updated_by_user_id");
            entity.Property(e => e.LastUpdatedDate).HasColumnName("last_updated_date");
            entity.Property(e => e.Name)
                .HasMaxLength(500)
                .HasColumnName("name");
            entity.Property(e => e.Remarks).HasColumnName("remarks");
        });

        modelBuilder.Entity<GroupGroup>(entity =>
        {
            entity.HasKey(e => e.GuidId).HasName("group_group_pkey");

            entity.ToTable("group_group");

            entity.HasIndex(e => new { e.GroupId, e.MapToGroupId }, "group_to_group_ids_uk").IsUnique();

            entity.Property(e => e.GuidId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("guid_id");
            entity.Property(e => e.CreatedByUserId).HasColumnName("created_by_user_id");
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("now()")
                .HasColumnName("creation_date");
            entity.Property(e => e.GroupId).HasColumnName("group_id");
            entity.Property(e => e.LastUpdatedByUserId).HasColumnName("last_updated_by_user_id");
            entity.Property(e => e.LastUpdatedDate).HasColumnName("last_updated_date");
            entity.Property(e => e.MapToGroupId).HasColumnName("map_to_group_id");
        });

        modelBuilder.Entity<GroupRole>(entity =>
        {
            entity.HasKey(e => e.GuidD).HasName("group_role_mapping_pkey");

            entity.ToTable("group_role");

            entity.HasIndex(e => e.GroupName, "group_name_uk").IsUnique();

            entity.Property(e => e.GuidD)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("guid_d");
            entity.Property(e => e.ApplicationRoleId).HasColumnName("application_role_id");
            entity.Property(e => e.CreateByUserId).HasColumnName("create_by_user_id");
            entity.Property(e => e.CreationDate).HasColumnName("creation_date");
            entity.Property(e => e.GroupName)
                .HasColumnType("character varying")
                .HasColumnName("group_name");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.LastUpdatedByUserId).HasColumnName("last_updated_by_user_id");
            entity.Property(e => e.LastUpdatedDate).HasColumnName("last_updated_date");
            entity.Property(e => e.Remarks)
                .HasColumnType("character varying")
                .HasColumnName("remarks");
            entity.Property(e => e.SuiteApplicationId).HasColumnName("suite_application_id");
        });

        modelBuilder.Entity<Suite>(entity =>
        {
            entity.HasKey(e => e.GuidId).HasName("suite_pkey");

            entity.ToTable("suite");

            entity.HasIndex(e => e.SuiteName, "Suite_uk").IsUnique();

            entity.Property(e => e.GuidId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("guid_id");
            entity.Property(e => e.CreatedByUserId).HasColumnName("created_by_user_id");
            entity.Property(e => e.CreationDate).HasColumnName("creation_date");
            entity.Property(e => e.DeActiveRemark)
                .HasMaxLength(1000)
                .HasColumnName("de_active_remark");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.LastUpdatedByUserId).HasColumnName("last_updated_by_user_id");
            entity.Property(e => e.LastUpdatedDate).HasColumnName("last_updated_date");
            entity.Property(e => e.SuiteCode)
                .HasMaxLength(25)
                .HasColumnName("suite_code");
            entity.Property(e => e.SuiteDescription)
                .HasMaxLength(1000)
                .HasColumnName("suite_description");
            entity.Property(e => e.SuiteName)
                .HasMaxLength(500)
                .HasColumnName("suite_name");
        });

        modelBuilder.Entity<SuiteApplication>(entity =>
        {
            entity.HasKey(e => e.GuidId).HasName("suites_application_pkey");

            entity.ToTable("suite_application");

            entity.HasIndex(e => e.SuiteApplicationName, "suite_app_name_uk").IsUnique();

            entity.Property(e => e.GuidId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("guid_id");
            entity.Property(e => e.ApplicationType)
                .HasColumnType("character varying")
                .HasColumnName("application_type");
            entity.Property(e => e.CreateByUserId).HasColumnName("create_by_user_id");
            entity.Property(e => e.CreationDate).HasColumnName("creation_date");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.LastUpdatedByUserId).HasColumnName("last_updated_by_user_id");
            entity.Property(e => e.LastUpdatedDate).HasColumnName("last_updated_date");
            entity.Property(e => e.SuiteApplicationDescription).HasColumnName("suite_application_description");
            entity.Property(e => e.SuiteApplicationName)
                .HasMaxLength(150)
                .HasColumnName("suite_application_name");
            entity.Property(e => e.SuiteId).HasColumnName("suite_id");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.GuidId).HasName("user_pkey");

            entity.ToTable("user");

            entity.HasIndex(e => e.WindowsUserName, "windows_user_name_uk").IsUnique();

            entity.Property(e => e.GuidId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("guid_id");
            entity.Property(e => e.CreatedByUserId).HasColumnName("created_by_user_id");
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("now()")
                .HasColumnName("creation_date");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.LastUpdatedByUserId).HasColumnName("last_updated_by_user_id");
            entity.Property(e => e.LastUpdatedDate).HasColumnName("last_updated_date");
            entity.Property(e => e.Remarks)
                .HasColumnType("character varying")
                .HasColumnName("remarks");
            entity.Property(e => e.UserFullName)
                .HasMaxLength(500)
                .HasColumnName("user_full_name");
            entity.Property(e => e.WindowsUserName)
                .HasMaxLength(500)
                .HasColumnName("windows_user_name");
        });

        modelBuilder.Entity<UserGroup>(entity =>
        {
            entity.HasKey(e => e.GuidId).HasName("user_group_pkey");

            entity.ToTable("user_group");

            entity.HasIndex(e => new { e.UserId, e.GroupId }, "user_group_ids_uk").IsUnique();

            entity.Property(e => e.GuidId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("guid_id");
            entity.Property(e => e.CreatedByUserId).HasColumnName("created_by_user_id");
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("now()")
                .HasColumnName("creation_date");
            entity.Property(e => e.GroupId).HasColumnName("group_id");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.LastUpdatedByUserId).HasColumnName("last_updated_by_user_id");
            entity.Property(e => e.LastUpdatedDate).HasColumnName("last_updated_date");
            entity.Property(e => e.UserId).HasColumnName("user_id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
