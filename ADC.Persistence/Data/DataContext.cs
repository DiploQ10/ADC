using ADC.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Expressions.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADC.Persistence.Data;

internal class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<CourseEntity> Courses { get; set; }
    public DbSet<SectionEntity> Sections { get; set; }
    public DbSet<LessonEntity> Lessons { get; set; }
    public DbSet<UserRoleEntity> UserRoles { get; set; }
    public DbSet<StudentCourseEntity> StudentCourses { get; set; }
    public DbSet<CourseFeedbackEntity> CourseFeedbacks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configurar índices únicos
        modelBuilder.Entity<UserEntity>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<UserEntity>()
            .HasIndex(u => u.IdentityDocument)
            .IsUnique();

        // Configurar eliminación en cascada
        modelBuilder.Entity<CourseEntity>()
            .HasMany(c => c.Students)
            .WithOne(sc => sc.Course)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SectionEntity>()
            .HasMany(s => s.Lessons)
            .WithOne(l => l.Section)
            .OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(modelBuilder);
    }
}