using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AfterSchool.Models.DataAccess
{
    public partial class AfterSchoolContext : DbContext
    {
        public AfterSchoolContext()
        {
        }

        public AfterSchoolContext(DbContextOptions<AfterSchoolContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Course> Courses { get; set; } = null!;
        public virtual DbSet<CourseOffer> CourseOffers { get; set; } = null!;
        public virtual DbSet<Instructor> Instructors { get; set; } = null!;
        public virtual DbSet<Location> Locations { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                /*#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=AfterSchool;Trusted_Connection=True;");*/
                
                // To access the DbContext outside controllers, you have to provide the connectionString to the AfterSchoolContext's optionsBuilder
                var builder = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.json");
                IConfiguration configuration = builder.Build();
                string connectionString = configuration.GetConnectionString("AfterSchool");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.ToTable("Course");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Courses)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Category");
            });

            modelBuilder.Entity<CourseOffer>(entity =>
            {
                entity.HasKey(e => new { e.CourseId, e.LocationId, e.StartDate })
                    .HasName("PK__tmp_ms_x__511C8212609E1FF0");

                entity.ToTable("CourseOffer");

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.ImageName).HasColumnName("ImageName");

                entity.Property(e => e.Price).HasColumnType("decimal(6, 2)");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.CourseOffers)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CourseOffer_Course");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.CourseOffers)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CourseOffer_Location");

                entity.HasMany(d => d.Instructors)
                    .WithMany(p => p.CourseOffers)
                    .UsingEntity<Dictionary<string, object>>(
                        "TeachingRecord",
                        l => l.HasOne<Instructor>().WithMany().HasForeignKey("InstructorId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_TeachingRecord_Instructor"),
                        r => r.HasOne<CourseOffer>().WithMany().HasForeignKey("CourseId", "LocationId", "StartDate").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_TeachingRecord_Course"),
                        j =>
                        {
                            j.HasKey("CourseId", "LocationId", "StartDate", "InstructorId").HasName("PK__Teaching__F8B552027E2FD84B");

                            j.ToTable("TeachingRecord");

                            j.IndexerProperty<DateTime>("StartDate").HasColumnType("date");
                        });
            });

            modelBuilder.Entity<Instructor>(entity =>
            {
                entity.ToTable("Instructor");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.Property(e => e.ImageName).HasColumnName("ImageName");

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.ToTable("Location");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsFixedLength();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
