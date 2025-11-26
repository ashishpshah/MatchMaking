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

        public virtual DbSet<Profile> Profiles { get; set; } = null!;

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
            modelBuilder.Entity<Profile>(entity =>
            {
                entity.Property(e => e.BodyType).HasMaxLength(50);

                entity.Property(e => e.City).HasMaxLength(100);

                entity.Property(e => e.Country).HasMaxLength(100);

                entity.Property(e => e.CoverPhotoPath).HasMaxLength(300);

                entity.Property(e => e.CreatedBy).HasDefaultValueSql("((0))");

                entity.Property(e => e.Education).HasMaxLength(200);

                entity.Property(e => e.Ethnicity).HasMaxLength(100);

                entity.Property(e => e.EyeColor).HasMaxLength(50);

                entity.Property(e => e.FatherSurname).HasMaxLength(100);

                entity.Property(e => e.Firstname).HasMaxLength(100);

                entity.Property(e => e.Gender).HasMaxLength(10);

                entity.Property(e => e.HairColor).HasMaxLength(50);

                entity.Property(e => e.Height).HasMaxLength(50);

                entity.Property(e => e.LastModifiedBy).HasDefaultValueSql("((0))");

                entity.Property(e => e.Lastname).HasMaxLength(100);

                entity.Property(e => e.MaternalSurname).HasMaxLength(100);

                entity.Property(e => e.Middlename).HasMaxLength(100);

                entity.Property(e => e.Mosal).HasMaxLength(100);

                entity.Property(e => e.MotherSurname).HasMaxLength(100);

                entity.Property(e => e.Occupation).HasMaxLength(200);

                entity.Property(e => e.PaternalSurname).HasMaxLength(100);

                entity.Property(e => e.ProfilePhotoPath).HasMaxLength(300);

                entity.Property(e => e.Smoking).HasMaxLength(100);

                entity.Property(e => e.State).HasMaxLength(100);

                entity.Property(e => e.Summary).HasMaxLength(500);

                entity.Property(e => e.Weight).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
