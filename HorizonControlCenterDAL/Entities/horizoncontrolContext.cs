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

    public virtual DbSet<GroupCategory> GroupCategories { get; set; }

    public virtual DbSet<GroupRoleMapping> GroupRoleMappings { get; set; }

    public virtual DbSet<GroupWithCategoryView> GroupWithCategoryViews { get; set; }

    public virtual DbSet<Suite> Suites { get; set; }

    public virtual DbSet<SuitesApplication> SuitesApplications { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserGroup> UserGroups { get; set; }

    public virtual DbSet<UserGroupView> UserGroupViews { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasKey(e => e.GuidId).HasName("group_pkey");

            entity.ToTable("group", "hcc");

            entity.Property(e => e.GuidId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("guid_id");
            entity.Property(e => e.CreatedByUserId).HasColumnName("created_by_user_id");
            entity.Property(e => e.CreationDate).HasColumnName("creation_date");
            entity.Property(e => e.GroupCategoryId).HasColumnName("group_category_id");
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

        modelBuilder.Entity<GroupCategory>(entity =>
        {
            entity.HasKey(e => e.GuidId).HasName("group_category_pkey");

            entity.ToTable("group_category", "hcc");

            entity.Property(e => e.GuidId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("guid_id");
            entity.Property(e => e.CreatedByUserId).HasColumnName("created_by_user_id");
            entity.Property(e => e.CreationDate).HasColumnName("creation_date");
            entity.Property(e => e.LastUpdatedByUserId).HasColumnName("last_updated_by_user_id");
            entity.Property(e => e.LastUpdatedDate).HasColumnName("last_updated_date");
            entity.Property(e => e.Name)
                .HasMaxLength(500)
                .HasColumnName("name");
        });

        modelBuilder.Entity<GroupRoleMapping>(entity =>
        {
            entity.HasKey(e => e.GuidD).HasName("group_role_mapping_pkey");

            entity.ToTable("group_role_mapping", "hcc");

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
            entity.Property(e => e.SuiteId).HasColumnName("suite_id");
            entity.Property(e => e.SuitesapplicationId).HasColumnName("suitesapplication_id");
        });

        modelBuilder.Entity<GroupWithCategoryView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("group_with_category_view", "hcc");

            entity.Property(e => e.Category)
                .HasMaxLength(500)
                .HasColumnName("category");
            entity.Property(e => e.CreatedByUserId).HasColumnName("created_by_user_id");
            entity.Property(e => e.CreationDate).HasColumnName("creation_date");
            entity.Property(e => e.GroupCategoryId).HasColumnName("group_category_id");
            entity.Property(e => e.GroupType)
                .HasColumnType("character varying")
                .HasColumnName("group_type");
            entity.Property(e => e.GuidId).HasColumnName("guid_id");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.LastUpdatedByUserId).HasColumnName("last_updated_by_user_id");
            entity.Property(e => e.LastUpdatedDate).HasColumnName("last_updated_date");
            entity.Property(e => e.Name)
                .HasMaxLength(500)
                .HasColumnName("name");
            entity.Property(e => e.Remarks).HasColumnName("remarks");
        });

        modelBuilder.Entity<Suite>(entity =>
        {
            entity.HasKey(e => e.GuidId).HasName("suite_pkey");

            entity.ToTable("suites", "hcc");

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

        modelBuilder.Entity<SuitesApplication>(entity =>
        {
            entity.HasKey(e => e.GuidId).HasName("suites_application_pkey");

            entity.ToTable("suites_application", "hcc");

            entity.Property(e => e.GuidId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("guid_id");
            entity.Property(e => e.ApplicationType).HasColumnName("application_type");
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

            entity.ToTable("user", "hcc");

            entity.Property(e => e.GuidId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("guid_id");
            entity.Property(e => e.Authentication)
                .HasMaxLength(100)
                .HasColumnName("authentication");
            entity.Property(e => e.CreatedByUserId).HasColumnName("created_by_user_id");
            entity.Property(e => e.CreationDate).HasColumnName("creation_date");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.LastUpdatedByUserId).HasColumnName("last_updated_by_user_id");
            entity.Property(e => e.LastUpdatedDate).HasColumnName("last_updated_date");
            entity.Property(e => e.Password).HasColumnName("password");
            entity.Property(e => e.Remarks)
                .HasColumnType("character varying")
                .HasColumnName("remarks");
            entity.Property(e => e.UserAccountType)
                .HasColumnType("character varying")
                .HasColumnName("user_account_type");
            entity.Property(e => e.UserFullName)
                .HasMaxLength(500)
                .HasColumnName("user_full_name");
            entity.Property(e => e.UserType)
                .HasColumnType("character varying")
                .HasColumnName("user_type");
            entity.Property(e => e.WindowsUserName)
                .HasMaxLength(500)
                .HasColumnName("windows_user_name");
        });

        modelBuilder.Entity<UserGroup>(entity =>
        {
            entity.HasKey(e => e.GuidId).HasName("user_group_pkey");

            entity.ToTable("user_group", "hcc");

            entity.Property(e => e.GuidId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("guid_id");
            entity.Property(e => e.CreatedByUserId).HasColumnName("created_by_user_id");
            entity.Property(e => e.CreationDate).HasColumnName("creation_date");
            entity.Property(e => e.GroupId).HasColumnName("group_id");
            entity.Property(e => e.LastUpdatedByUserId).HasColumnName("last_updated_by_user_id");
            entity.Property(e => e.LastUpdatedDate).HasColumnName("last_updated_date");
            entity.Property(e => e.MapToGroupId).HasColumnName("map_to_group_id");
            entity.Property(e => e.MappingType)
                .HasMaxLength(100)
                .HasColumnName("mapping_type");
            entity.Property(e => e.UserId).HasColumnName("user_id");
        });

        modelBuilder.Entity<UserGroupView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("user_group_view", "hcc");

            entity.Property(e => e.CreatedByUserId).HasColumnName("created_by_user_id");
            entity.Property(e => e.CreationDate).HasColumnName("creation_date");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.GroupId).HasColumnName("group_id");
            entity.Property(e => e.GroupType)
                .HasColumnType("character varying")
                .HasColumnName("group_type");
            entity.Property(e => e.GuidId).HasColumnName("guid_id");
            entity.Property(e => e.LastUpdatedByUserId).HasColumnName("last_updated_by_user_id");
            entity.Property(e => e.LastUpdatedDate).HasColumnName("last_updated_date");
            entity.Property(e => e.MapGroupName)
                .HasMaxLength(500)
                .HasColumnName("map_group_name");
            entity.Property(e => e.MapToGroupId).HasColumnName("map_to_group_id");
            entity.Property(e => e.MappingType)
                .HasMaxLength(100)
                .HasColumnName("mapping_type");
            entity.Property(e => e.Name)
                .HasMaxLength(500)
                .HasColumnName("name");
            entity.Property(e => e.UserId).HasColumnName("user_id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
