using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MatchMaking.Models_Temp
{
    public partial class MatchMakingDBContext : DbContext
    {
        public MatchMakingDBContext()
        {
        }

        public MatchMakingDBContext(DbContextOptions<MatchMakingDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Menu> Menus { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<RoleMenuAccess> RoleMenuAccesses { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserRoleMapping> UserRoleMappings { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=KL-ITS-DEVELOP2\\SQLEXPRESS;Initial Catalog=MatchMakingDB;Integrated Security=True;Trust Server Certificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Menu>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.ParentId });

                entity.ToTable("Menu");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<RoleMenuAccess>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("RoleMenuAccess");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.CreatedBy).HasDefaultValueSql("((0))");

                entity.Property(e => e.LastModifiedBy).HasDefaultValueSql("((0))");

                entity.Property(e => e.NextChangePasswordDate).HasColumnName("Next_Change_Password_Date");

                entity.Property(e => e.NoOfWrongPasswordAttempts).HasColumnName("No_Of_Wrong_Password_Attempts");
            });

            modelBuilder.Entity<UserRoleMapping>(entity =>
            {
                entity.ToTable("UserRoleMapping");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
