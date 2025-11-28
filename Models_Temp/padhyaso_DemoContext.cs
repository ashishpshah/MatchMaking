using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MatchMaking.Models_Temp
{
    public partial class padhyaso_DemoContext : DbContext
    {
        public padhyaso_DemoContext()
        {
        }

        public padhyaso_DemoContext(DbContextOptions<padhyaso_DemoContext> options)
            : base(options)
        {
        }

        public virtual DbSet<JainGroup> JainGroups { get; set; } = null!;
        public virtual DbSet<Lov> Lovs { get; set; } = null!;
        public virtual DbSet<Menu> Menus { get; set; } = null!;
        public virtual DbSet<Profile> Profiles { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<RoleMenuAccess> RoleMenuAccesses { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserMenuAccess> UserMenuAccesses { get; set; } = null!;
        public virtual DbSet<UserRoleMapping> UserRoleMappings { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=103.120.179.247;Initial Catalog=padhyaso_Demo;User ID=Padhyaso_admin;Password=satest@123;Trust Server Certificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("padhyaso_admin");

            modelBuilder.Entity<JainGroup>(entity =>
            {
                entity.ToTable("JainGroup", "dbo");

                entity.Property(e => e.Name)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Lov>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("LOV", "dbo");

                entity.Property(e => e.LovCode).HasColumnName("LOV_Code");

                entity.Property(e => e.LovColumn).HasColumnName("LOV_Column");

                entity.Property(e => e.LovDesc).HasColumnName("LOV_Desc");
            });

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.ParentId });

                entity.ToTable("Menu", "dbo");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Profile>(entity =>
            {
                entity.ToTable("Profiles", "dbo");

                entity.Property(e => e.Address).HasMaxLength(100);

                entity.Property(e => e.BodyType).HasMaxLength(50);

                entity.Property(e => e.City).HasMaxLength(100);

                entity.Property(e => e.Country).HasMaxLength(100);

                entity.Property(e => e.Education).HasMaxLength(200);

                entity.Property(e => e.Ethnicity).HasMaxLength(100);

                entity.Property(e => e.EyeColor).HasMaxLength(50);

                entity.Property(e => e.Fathername).HasMaxLength(100);

                entity.Property(e => e.Firstname).HasMaxLength(100);

                entity.Property(e => e.Gender).HasMaxLength(10);

                entity.Property(e => e.HairColor).HasMaxLength(50);

                entity.Property(e => e.Height).HasMaxLength(50);

                entity.Property(e => e.Interests).HasMaxLength(100);

                entity.Property(e => e.Language).HasMaxLength(100);

                entity.Property(e => e.LookingForGender).HasMaxLength(10);

                entity.Property(e => e.MaritalStatus).HasMaxLength(10);

                entity.Property(e => e.MaternalSurname).HasMaxLength(100);

                entity.Property(e => e.Mosal).HasMaxLength(100);

                entity.Property(e => e.Mothername).HasMaxLength(100);

                entity.Property(e => e.Occupation).HasMaxLength(200);

                entity.Property(e => e.PaternalSurname).HasMaxLength(100);

                entity.Property(e => e.Smoking).HasMaxLength(100);

                entity.Property(e => e.State).HasMaxLength(100);

                entity.Property(e => e.Summary).HasMaxLength(500);

                entity.Property(e => e.Weight).HasMaxLength(50);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Roles", "dbo");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<RoleMenuAccess>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("RoleMenuAccess", "dbo");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users", "dbo");

                entity.Property(e => e.NextChangePasswordDate).HasColumnName("Next_Change_Password_Date");

                entity.Property(e => e.NoOfWrongPasswordAttempts).HasColumnName("No_Of_Wrong_Password_Attempts");
            });

            modelBuilder.Entity<UserMenuAccess>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("UserMenuAccess", "dbo");
            });

            modelBuilder.Entity<UserRoleMapping>(entity =>
            {
                entity.ToTable("UserRoleMapping", "dbo");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
