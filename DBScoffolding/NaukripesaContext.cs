using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace DBScoffolding;

public partial class NaukripesaContext : DbContext
{
    public NaukripesaContext()
    {
    }

    public NaukripesaContext(DbContextOptions<NaukripesaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Accessactivity> Accessactivities { get; set; }

    public virtual DbSet<Appicon> Appicons { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<Currency> Currencies { get; set; }

    public virtual DbSet<Demotab> Demotabs { get; set; }

    public virtual DbSet<Efmigrationshistory> Efmigrationshistories { get; set; }

    public virtual DbSet<Erroractivity> Erroractivities { get; set; }

    public virtual DbSet<Jobdetail> Jobdetails { get; set; }

    public virtual DbSet<Joblocation> Joblocations { get; set; }

    public virtual DbSet<Menucategory> Menucategories { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Setting> Settings { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Usergroup> Usergroups { get; set; }

    public virtual DbSet<Usergrouppermission> Usergrouppermissions { get; set; }

    public virtual DbSet<Usersession> Usersessions { get; set; }

    public virtual DbSet<Usertype> Usertypes { get; set; }

    public virtual DbSet<Webpage> Webpages { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;database=naukripesa;user id=root;password=abc@123;port=3306", Microsoft.EntityFrameworkCore.ServerVersion.Parse("9.2.0-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Accessactivity>(entity =>
        {
            entity.HasKey(e => e.AccessActivityId).HasName("PRIMARY");

            entity
                .ToTable("accessactivity")
                .UseCollation("utf8mb4_general_ci");

            entity.HasIndex(e => e.UserId, "IX_AccessActivity_UserID");

            entity.Property(e => e.AccessActivityId).HasColumnName("AccessActivityID");
            entity.Property(e => e.ActivityType).HasMaxLength(200);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("'1900-01-01 00:00:00'")
                .HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.UserSessionId).HasColumnName("UserSessionID");

            entity.HasOne(d => d.User).WithMany(p => p.Accessactivities)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_AccessActivity_User");
        });

        modelBuilder.Entity<Appicon>(entity =>
        {
            entity.HasKey(e => e.AppIconId).HasName("PRIMARY");

            entity
                .ToTable("appicon")
                .UseCollation("utf8mb4_general_ci");

            entity.Property(e => e.AppIconId).HasColumnName("AppIconID");
            entity.Property(e => e.CssClass).HasMaxLength(100);
            entity.Property(e => e.IconColor).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.CompanyId).HasName("PRIMARY");

            entity
                .ToTable("company")
                .UseCollation("utf8mb4_general_ci");

            entity.HasIndex(e => e.CurrencyId, "IX_Company_CurrencyID");

            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.CurrencyId).HasColumnName("CurrencyID");
            entity.Property(e => e.Description).HasMaxLength(256);
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.Currency).WithMany(p => p.Companies)
                .HasForeignKey(d => d.CurrencyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Company_Currency");
        });

        modelBuilder.Entity<Currency>(entity =>
        {
            entity.HasKey(e => e.CurrencyId).HasName("PRIMARY");

            entity
                .ToTable("currency")
                .UseCollation("utf8mb4_general_ci");

            entity.Property(e => e.CurrencyId).HasColumnName("CurrencyID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(256);
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Demotab>(entity =>
        {
            entity.HasKey(e => e.DemoId).HasName("PRIMARY");

            entity
                .ToTable("demotab")
                .UseCollation("utf8mb4_general_ci");

            entity.HasIndex(e => e.AppIconId, "IX_DemoTab_AppIconID");

            entity.HasIndex(e => e.UserId, "IX_DemoTab_UserID");

            entity.Property(e => e.DemoId).HasColumnName("DemoID");
            entity.Property(e => e.AppIconId).HasColumnName("AppIconID");
            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Other).HasMaxLength(50);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.AppIcon).WithMany(p => p.Demotabs)
                .HasForeignKey(d => d.AppIconId)
                .HasConstraintName("FK_DemoTab_AppIcon");

            entity.HasOne(d => d.User).WithMany(p => p.Demotabs)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_DemoTab_User");
        });

        modelBuilder.Entity<Efmigrationshistory>(entity =>
        {
            entity.HasKey(e => e.MigrationId).HasName("PRIMARY");

            entity
                .ToTable("__efmigrationshistory")
                .UseCollation("utf8mb4_general_ci");

            entity.Property(e => e.MigrationId).HasMaxLength(150);
            entity.Property(e => e.ProductVersion).HasMaxLength(32);
        });

        modelBuilder.Entity<Erroractivity>(entity =>
        {
            entity.HasKey(e => e.ErrorActivityId).HasName("PRIMARY");

            entity
                .ToTable("erroractivity")
                .UseCollation("utf8mb4_general_ci");

            entity.Property(e => e.ErrorActivityId).HasColumnName("ErrorActivityID");
            entity.Property(e => e.ErrorDateTime).HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(100);
            entity.Property(e => e.UserId).HasColumnName("UserID");
        });

        modelBuilder.Entity<Jobdetail>(entity =>
        {
            entity.HasKey(e => e.JobDetailId).HasName("PRIMARY");

            entity.ToTable("jobdetails");

            entity.HasIndex(e => e.CompanyId, "FK_JobDetails_Company_idx");

            entity.HasIndex(e => e.JobLocationId, "FK_JobDetails_JobLocation_idx");

            entity.Property(e => e.JobDetailId).HasColumnName("JobDetailID");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.ContactNumber).HasMaxLength(12);
            entity.Property(e => e.Department).HasMaxLength(128);
            entity.Property(e => e.InterviewDate).HasColumnType("datetime");
            entity.Property(e => e.InterviewLocation).HasMaxLength(512);
            entity.Property(e => e.InterviewTime).HasColumnType("text");
            entity.Property(e => e.JobLocationId).HasColumnName("JobLocationID");
            entity.Property(e => e.OtherDetail).HasColumnType("text");
            entity.Property(e => e.Qualification).HasMaxLength(512);

            entity.HasOne(d => d.Company).WithMany(p => p.Jobdetails)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_JobDetails_Company");

            entity.HasOne(d => d.JobLocation).WithMany(p => p.Jobdetails)
                .HasForeignKey(d => d.JobLocationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_JobDetails_JobLocation");
        });

        modelBuilder.Entity<Joblocation>(entity =>
        {
            entity.HasKey(e => e.JobLocationId).HasName("PRIMARY");

            entity.ToTable("joblocation");

            entity.Property(e => e.JobLocationId).HasColumnName("JobLocationID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Location).HasMaxLength(100);
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<Menucategory>(entity =>
        {
            entity.HasKey(e => e.MenuCategoryId).HasName("PRIMARY");

            entity
                .ToTable("menucategory")
                .UseCollation("utf8mb4_general_ci");

            entity.HasIndex(e => e.AppIconId, "IX_MenuCategory_AppIconID");

            entity.Property(e => e.MenuCategoryId).HasColumnName("MenuCategoryID");
            entity.Property(e => e.AppIconId).HasColumnName("AppIconID");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.AppIcon).WithMany(p => p.Menucategories)
                .HasForeignKey(d => d.AppIconId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MenuCategory_AppIcon");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PRIMARY");

            entity
                .ToTable("product")
                .UseCollation("utf8mb4_general_ci");

            entity.HasIndex(e => e.UserTypeId, "IX_Product_UserTypeID");

            entity.HasIndex(e => e.WebPageId, "IX_Product_WebPageID");

            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.Pname)
                .HasMaxLength(50)
                .HasColumnName("PName");
            entity.Property(e => e.UserTypeId).HasColumnName("UserTypeID");
            entity.Property(e => e.WebPageId).HasColumnName("WebPageID");

            entity.HasOne(d => d.UserType).WithMany(p => p.Products)
                .HasForeignKey(d => d.UserTypeId)
                .HasConstraintName("FK_Product_UserType");

            entity.HasOne(d => d.WebPage).WithMany(p => p.Products)
                .HasForeignKey(d => d.WebPageId)
                .HasConstraintName("FK_Product_Webpage");
        });

        modelBuilder.Entity<Setting>(entity =>
        {
            entity.HasKey(e => e.SettingId).HasName("PRIMARY");

            entity
                .ToTable("setting")
                .UseCollation("utf8mb4_general_ci");

            entity.HasIndex(e => e.CompanyId, "IX_Setting_CompanyID");

            entity.Property(e => e.SettingId).HasColumnName("SettingID");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Value).HasMaxLength(500);

            entity.HasOne(d => d.Company).WithMany(p => p.Settings)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Setting_Company");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity
                .ToTable("user")
                .UseCollation("utf8mb4_general_ci");

            entity.HasIndex(e => e.CompanyId, "IX_User_CompanyID");

            entity.HasIndex(e => e.UserGroupId, "IX_User_UserGroupID");

            entity.HasIndex(e => e.UserTypeId, "IX_User_UserTypeID");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Code)
                .HasMaxLength(5)
                .HasDefaultValueSql("'ADMIN'");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("'1900-01-01 00:00:00'")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.IsArchived).HasDefaultValueSql("'0'");
            entity.Property(e => e.MobileNo).HasMaxLength(14);
            entity.Property(e => e.ModifiedOn)
                .HasDefaultValueSql("'1900-01-01 00:00:00'")
                .HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Otp)
                .HasMaxLength(10)
                .HasColumnName("OTP");
            entity.Property(e => e.Password).HasMaxLength(500);
            entity.Property(e => e.RecentLogin).HasColumnType("datetime");
            entity.Property(e => e.UserGroupId).HasColumnName("UserGroupID");
            entity.Property(e => e.UserTypeId).HasColumnName("UserTypeID");

            entity.HasOne(d => d.Company).WithMany(p => p.Users)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Company");

            entity.HasOne(d => d.UserGroup).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserGroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_UserGroup");

            entity.HasOne(d => d.UserType).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_UserType");
        });

        modelBuilder.Entity<Usergroup>(entity =>
        {
            entity.HasKey(e => e.UserGroupId).HasName("PRIMARY");

            entity
                .ToTable("usergroup")
                .UseCollation("utf8mb4_general_ci");

            entity.Property(e => e.UserGroupId).HasColumnName("UserGroupID");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Usergrouppermission>(entity =>
        {
            entity.HasKey(e => e.UserGroupPermissionId).HasName("PRIMARY");

            entity
                .ToTable("usergrouppermission")
                .UseCollation("utf8mb4_general_ci");

            entity.HasIndex(e => e.MenuCategoryId, "IX_UserGroupPermission_MenuCategoryID");

            entity.HasIndex(e => e.UserGroupId, "IX_UserGroupPermission_UserGroupID");

            entity.HasIndex(e => e.WebpageId, "IX_UserGroupPermission_WebpageID");

            entity.Property(e => e.UserGroupPermissionId).HasColumnName("UserGroupPermissionID");
            entity.Property(e => e.MenuCategoryId).HasColumnName("MenuCategoryID");
            entity.Property(e => e.UserGroupId).HasColumnName("UserGroupID");
            entity.Property(e => e.WebpageId).HasColumnName("WebpageID");

            entity.HasOne(d => d.MenuCategory).WithMany(p => p.Usergrouppermissions)
                .HasForeignKey(d => d.MenuCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserGroupPermission_MenuCategory");

            entity.HasOne(d => d.UserGroup).WithMany(p => p.Usergrouppermissions)
                .HasForeignKey(d => d.UserGroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserGroupPermission_UserGroup");

            entity.HasOne(d => d.Webpage).WithMany(p => p.Usergrouppermissions)
                .HasForeignKey(d => d.WebpageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserGroupPermission_WebPage");
        });

        modelBuilder.Entity<Usersession>(entity =>
        {
            entity.HasKey(e => e.UserSessionId).HasName("PRIMARY");

            entity
                .ToTable("usersession")
                .UseCollation("utf8mb4_general_ci");

            entity.HasIndex(e => e.UserId, "IX_UserSession_UserID");

            entity.Property(e => e.UserSessionId).HasColumnName("UserSessionID");
            entity.Property(e => e.EndTime).HasColumnType("datetime");
            entity.Property(e => e.ExpirationTimeFrame).HasDefaultValueSql("'45'");
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("'1'");
            entity.Property(e => e.SessionGuid)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.StartTime).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Usersessions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_UserSession_User");
        });

        modelBuilder.Entity<Usertype>(entity =>
        {
            entity.HasKey(e => e.UserTypeId).HasName("PRIMARY");

            entity
                .ToTable("usertype")
                .UseCollation("utf8mb4_general_ci");

            entity.Property(e => e.UserTypeId).HasColumnName("UserTypeID");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Webpage>(entity =>
        {
            entity.HasKey(e => e.WebpageId).HasName("PRIMARY");

            entity
                .ToTable("webpage")
                .UseCollation("utf8mb4_general_ci");

            entity.HasIndex(e => e.AppIconId, "IX_Webpage_AppIconID");

            entity.HasIndex(e => e.ParentWebpageId, "IX_Webpage_ParentWebpageID");

            entity.Property(e => e.WebpageId).HasColumnName("WebpageID");
            entity.Property(e => e.AppIconId).HasColumnName("AppIconID");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.ParentWebpageId)
                .HasDefaultValueSql("'0'")
                .HasColumnName("ParentWebpageID");
            entity.Property(e => e.UiUrl)
                .HasMaxLength(255)
                .HasColumnName("UiURL");
            entity.Property(e => e.Url)
                .HasMaxLength(255)
                .HasColumnName("URL");

            entity.HasOne(d => d.AppIcon).WithMany(p => p.Webpages)
                .HasForeignKey(d => d.AppIconId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Webpage_AppIcon");

            entity.HasOne(d => d.ParentWebpage).WithMany(p => p.InverseParentWebpage)
                .HasForeignKey(d => d.ParentWebpageId)
                .HasConstraintName("FK_Webpage_ParentWebpage");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
