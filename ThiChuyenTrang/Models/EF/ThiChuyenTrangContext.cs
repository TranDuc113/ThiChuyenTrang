using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ThiChuyenTrang.Models.EF
{
    public partial class ThiChuyenTrangContext : DbContext
    {
        public ThiChuyenTrangContext()
        {
        }

        public ThiChuyenTrangContext(DbContextOptions<ThiChuyenTrangContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<File> File { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasIndex(e => e.CreatedBy);

                entity.HasIndex(e => e.UpdatedBy);

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.Slug).HasMaxLength(50);

                entity.Property(e => e.Title)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.UpdatedDate).HasColumnType("date");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.Category)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_Category_User");
            });

            modelBuilder.Entity<File>(entity =>
            {
                entity.HasIndex(e => e.NewsId);

                entity.Property(e => e.Url).HasMaxLength(200);

                entity.HasOne(d => d.News)
                    .WithMany(p => p.File)
                    .HasForeignKey(d => d.NewsId)
                    .HasConstraintName("FK_File_News");
            });

            modelBuilder.Entity<News>(entity =>
            {
                entity.HasIndex(e => e.CreatedBy);

                entity.HasIndex(e => e.UpdatedBy);

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.Image).HasMaxLength(200);

                entity.Property(e => e.Slug).HasMaxLength(50);

                entity.Property(e => e.Title).HasMaxLength(50);

                entity.Property(e => e.UpdatedDate).HasColumnType("date");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.News)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_News_Category");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.NewsCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_News_User");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.NewsUpdatedByNavigation)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_News_User1");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Avatar).HasMaxLength(200);

                entity.Property(e => e.DateOfBirth).HasColumnType("date");

                entity.Property(e => e.FullName).HasMaxLength(50);

                entity.Property(e => e.LastLogin).HasColumnType("date");

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.Property(e => e.Username).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
