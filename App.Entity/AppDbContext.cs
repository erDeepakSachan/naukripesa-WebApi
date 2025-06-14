using App.Core;
using App.Util;
using Microsoft.EntityFrameworkCore;

namespace App.Entity
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AccessActivity> AccessActivities { get; set; } = null!;
        public virtual DbSet<AppIcon> AppIcons { get; set; } = null!;
        public virtual DbSet<Company> Companies { get; set; } = null!;
        public virtual DbSet<Currency> Currencies { get; set; } = null!;
        public virtual DbSet<DemoTab> DemoTabs { get; set; } = null!;
        public virtual DbSet<ErrorActivity> ErrorActivities { get; set; } = null!;
        public virtual DbSet<MenuCategory> MenuCategories { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<Setting> Settings { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserGroup> UserGroups { get; set; } = null!;
        public virtual DbSet<UserGroupPermission> UserGroupPermissions { get; set; } = null!;
        public virtual DbSet<UserSession> UserSessions { get; set; } = null!;
        public virtual DbSet<UserType> UserTypes { get; set; } = null!;
        public virtual DbSet<Webpage> Webpages { get; set; } = null!;
        public virtual DbSet<Jobdetail> Jobdetails { get; set; }
        public virtual DbSet<Joblocation> Joblocations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Console.WriteLine(Database.ProviderName);
            var provider = Database.ProviderName.ToEfProvider();
            
            modelBuilder.Entity<AccessActivity>(entity =>
            {
                entity.ToTable("AccessActivity");

                entity.Property(e => e.AccessActivityId).HasColumnName("AccessActivityID");

                entity.Property(e => e.ActivityType)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                if (provider == EfProviders.SqlServer)
                {
                    entity.Property(e => e.CreatedOn)
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");
                }
                
                if (provider == EfProviders.MySql)
                {
                    entity.Property(e => e.CreatedOn)
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("('1900-01-01')");
                }
                
                if (provider == EfProviders.PostgreSql)
                {
                    entity.Property(e => e.CreatedOn)
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("('1900-01-01')");
                }

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.UserSessionId).HasColumnName("UserSessionID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AccessActivities)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_AccessActivity_User");
            });

            modelBuilder.Entity<AppIcon>(entity =>
            {
                entity.ToTable("AppIcon");

                entity.Property(e => e.AppIconId).HasColumnName("AppIconID");

                entity.Property(e => e.CssClass)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.IconColor)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Company>(entity =>
            {
                entity.ToTable("Company");

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                if (provider == EfProviders.PostgreSql)
                {
                    entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");
                }
                else
                {
                    entity.Property(e => e.CreatedOn).HasColumnType("datetime");
                }

                entity.Property(e => e.CurrencyId).HasColumnName("CurrencyID");

                entity.Property(e => e.Address)
                    .HasMaxLength(256)
                    .IsUnicode(false);
                
                if (provider == EfProviders.PostgreSql)
                {
                    entity.Property(e => e.ModifiedOn).HasColumnType("timestamp with time zone");
                }
                else
                {
                    entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
                }

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.Companies)
                    .HasForeignKey(d => d.CurrencyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Company_Currency");
            });

            modelBuilder.Entity<Currency>(entity =>
            {
                entity.ToTable("Currency");

                entity.Property(e => e.CurrencyId).HasColumnName("CurrencyID");

                if (provider == EfProviders.PostgreSql)
                {
                    entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");
                }
                else
                {
                    entity.Property(e => e.CreatedOn).HasColumnType("datetime");
                }

                entity.Property(e => e.Description)
                    .HasMaxLength(256)
                    .IsUnicode(false);
                
                if (provider == EfProviders.PostgreSql)
                {
                    entity.Property(e => e.ModifiedOn).HasColumnType("timestamp with time zone");
                }
                else
                {
                    entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
                }

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DemoTab>(entity =>
            {
                entity.HasKey(e => e.DemoId);

                entity.ToTable("DemoTab");

                entity.Property(e => e.DemoId).HasColumnName("DemoID");

                entity.Property(e => e.AppIconId).HasColumnName("AppIconID");

                entity.Property(e => e.Description).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Other).HasMaxLength(50);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.AppIcon)
                    .WithMany(p => p.DemoTabs)
                    .HasForeignKey(d => d.AppIconId)
                    .HasConstraintName("FK_DemoTab_AppIcon");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.DemoTabs)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_DemoTab_User");
            });

            modelBuilder.Entity<ErrorActivity>(entity =>
            {
                entity.ToTable("ErrorActivity");

                entity.Property(e => e.ErrorActivityId).HasColumnName("ErrorActivityID");

                if (provider == EfProviders.PostgreSql)
                {
                    entity.Property(e => e.ErrorDateTime).HasColumnType("timestamp with time zone");
                }
                else
                {
                    entity.Property(e => e.ErrorDateTime).HasColumnType("datetime");
                }

                entity.Property(e => e.ErrorMessage).IsUnicode(false);

                entity.Property(e => e.StackTraceMessage).IsUnicode(false);

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<MenuCategory>(entity =>
            {
                entity.ToTable("MenuCategory");

                entity.Property(e => e.MenuCategoryId).HasColumnName("MenuCategoryID");

                entity.Property(e => e.AppIconId).HasColumnName("AppIconID");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.HasOne(d => d.AppIcon)
                    .WithMany(p => p.MenuCategories)
                    .HasForeignKey(d => d.AppIconId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MenuCategory_AppIcon");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.Description).HasMaxLength(50);

                entity.Property(e => e.Pname)
                    .HasMaxLength(50)
                    .HasColumnName("PName");

                entity.Property(e => e.UserTypeId).HasColumnName("UserTypeID");

                entity.Property(e => e.WebPageId).HasColumnName("WebPageID");

                entity.HasOne(d => d.UserType)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.UserTypeId)
                    .HasConstraintName("FK_Product_UserType");

                entity.HasOne(d => d.WebPage)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.WebPageId)
                    .HasConstraintName("FK_Product_Webpage");
            });

            modelBuilder.Entity<Setting>(entity =>
            {
                entity.ToTable("Setting");

                entity.Property(e => e.SettingId).HasColumnName("SettingID");

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                if (provider == EfProviders.PostgreSql)
                {
                    entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");
                }
                else
                {
                    entity.Property(e => e.CreatedOn).HasColumnType("datetime");
                }

                if (provider == EfProviders.PostgreSql)
                {
                    entity.Property(e => e.ModifiedOn).HasColumnType("timestamp with time zone");
                }
                else
                {
                    entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
                }

                entity.Property(e => e.Name)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Value)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.Settings)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Setting_Company");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.Code)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('ADMIN')");

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                if (provider == EfProviders.SqlServer)
                {
                    entity.Property(e => e.CreatedOn)
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");
                }
                
                if (provider == EfProviders.MySql)
                {
                    entity.Property(e => e.CreatedOn)
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("('1900-01-01')");
                }
                
                if (provider == EfProviders.PostgreSql)
                {
                    entity.Property(e => e.CreatedOn)
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("('1900-01-01')");
                }

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                if (provider == EfProviders.PostgreSql)
                {
                    entity.Property(e => e.IsArchived).HasDefaultValueSql("(('TRUE'))");
                }
                else
                {
                    entity.Property(e => e.IsArchived).HasDefaultValueSql("((0))");
                }

                entity.Property(e => e.MobileNo).HasMaxLength(14);
                
                if (provider == EfProviders.SqlServer)
                {
                    entity.Property(e => e.ModifiedOn)
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");
                }
                
                if (provider == EfProviders.MySql)
                {
                    entity.Property(e => e.ModifiedOn)
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("('1900-01-01')");
                }
                
                if (provider == EfProviders.PostgreSql)
                {
                    entity.Property(e => e.ModifiedOn)
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("('1900-01-01')");
                }

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Otp)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("OTP");

                entity.Property(e => e.Password)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                if (provider == EfProviders.PostgreSql)
                {
                    entity.Property(e => e.RecentLogin).HasColumnType("timestamp with time zone");
                }
                else
                {
                    entity.Property(e => e.RecentLogin).HasColumnType("datetime");
                }
                
                entity.Property(e => e.UserGroupId).HasColumnName("UserGroupID");

                entity.Property(e => e.UserTypeId).HasColumnName("UserTypeID");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_Company");

                entity.HasOne(d => d.UserGroup)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.UserGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_UserGroup");

                entity.HasOne(d => d.UserType)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.UserTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_UserType");
            });

            modelBuilder.Entity<UserGroup>(entity =>
            {
                entity.ToTable("UserGroup");

                entity.Property(e => e.UserGroupId).HasColumnName("UserGroupID");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UserGroupPermission>(entity =>
            {
                entity.ToTable("UserGroupPermission");

                entity.Property(e => e.UserGroupPermissionId).HasColumnName("UserGroupPermissionID");

                entity.Property(e => e.MenuCategoryId).HasColumnName("MenuCategoryID");

                entity.Property(e => e.UserGroupId).HasColumnName("UserGroupID");

                entity.Property(e => e.WebpageId).HasColumnName("WebpageID");

                entity.HasOne(d => d.MenuCategory)
                    .WithMany(p => p.UserGroupPermissions)
                    .HasForeignKey(d => d.MenuCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserGroupPermission_MenuCategory");

                entity.HasOne(d => d.UserGroup)
                    .WithMany(p => p.UserGroupPermissions)
                    .HasForeignKey(d => d.UserGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserGroupPermission_UserGroup");

                entity.HasOne(d => d.Webpage)
                    .WithMany(p => p.UserGroupPermissions)
                    .HasForeignKey(d => d.WebpageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserGroupPermission_WebPage");
            });

            modelBuilder.Entity<UserSession>(entity =>
            {
                entity.ToTable("UserSession");

                entity.Property(e => e.UserSessionId).HasColumnName("UserSessionID");

                if (provider == EfProviders.PostgreSql)
                {
                    entity.Property(e => e.EndTime).HasColumnType("timestamp with time zone");
                }
                else
                {
                    entity.Property(e => e.EndTime).HasColumnType("datetime");
                }

                entity.Property(e => e.ExpirationTimeFrame).HasDefaultValueSql("((45))");

                if (provider == EfProviders.PostgreSql)
                {
                    entity.Property(e => e.IsActive)
                        .IsRequired()
                        .HasDefaultValueSql("(('TRUE'))"); 
                }
                else
                {
                    entity.Property(e => e.IsActive)
                        .IsRequired()
                        .HasDefaultValueSql("((1))");
                }

                if (provider == EfProviders.SqlServer)
                {
                    entity.Property(e => e.SessionGuid).HasDefaultValueSql("(newid())");
                }
                
                if (provider == EfProviders.MySql)
                {
                    entity.Property(e => e.SessionGuid).HasDefaultValueSql("(uuid())");
                }
                
                if (provider == EfProviders.PostgreSql)
                {
                    entity.Property(e => e.SessionGuid).HasDefaultValueSql("(uuid_generate_v4())");
                }

                if (provider == EfProviders.PostgreSql)
                {
                    entity.Property(e => e.StartTime).HasColumnType("timestamp with time zone");
                }
                else
                {
                    entity.Property(e => e.StartTime).HasColumnType("datetime");
                }

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserSessions)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_UserSession_User");
            });

            modelBuilder.Entity<UserType>(entity =>
            {
                entity.ToTable("UserType");

                entity.Property(e => e.UserTypeId).HasColumnName("UserTypeID");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);
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
                entity.Property(e => e.CreatedBy).HasColumnName("CreatedBy");
                entity.Property(e => e.ModifiedBy).HasColumnName("ModifiedBy");
                if (provider == EfProviders.SqlServer)
                {
                    entity.Property(e => e.CreatedOn)
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");
                }

                if (provider == EfProviders.MySql)
                {
                    entity.Property(e => e.CreatedOn)
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("('1900-01-01')");
                }

                if (provider == EfProviders.PostgreSql)
                {
                    entity.Property(e => e.CreatedOn)
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("('1900-01-01')");
                }

                if (provider == EfProviders.PostgreSql)
                {
                    entity.Property(e => e.ModifiedOn).HasColumnType("timestamp with time zone");
                }
                else
                {
                    entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
                }
                entity.HasOne(d => d.Company).WithMany(p => p.Jobdetails)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_JobDetails_Company");
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

            modelBuilder.Entity<Webpage>(entity =>
            {
                entity.ToTable("Webpage");

                entity.Property(e => e.WebpageId).HasColumnName("WebpageID");

                entity.Property(e => e.AppIconId).HasColumnName("AppIconID");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ParentWebpageId)
                    .HasColumnName("ParentWebpageID")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Url)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("URL");
                entity.Property(e => e.UiUrl)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("UiURL");

                entity.HasOne(d => d.AppIcon)
                    .WithMany(p => p.Webpages)
                    .HasForeignKey(d => d.AppIconId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Webpage_AppIcon");

                entity.HasOne(d => d.ParentWebpage)
                    .WithMany(p => p.InverseParentWebpage)
                    .HasForeignKey(d => d.ParentWebpageId)
                    .HasConstraintName("FK_Webpage_ParentWebpage");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
