using MatchMaking.Models;
using Microsoft.EntityFrameworkCore;

namespace MatchMaking.Infra
{
	public partial class DataContext : DbContext
	{
		public DataContext() { }

		public DataContext(DbContextOptions<DataContext> options) : base(options) { }

		public virtual DbSet<Menu> Menus { get; set; } = null!;
		public virtual DbSet<Role> Roles { get; set; } = null!;
		public virtual DbSet<RoleMenuAccess> RoleMenuAccesses { get; set; } = null!;
		public virtual DbSet<User> Users { get; set; } = null!;
		public virtual DbSet<UserRoleMapping> UserRoleMappings { get; set; } = null!;

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

				entity.Property(e => e.ProjectDetailTypeAccess).HasMaxLength(500);
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
