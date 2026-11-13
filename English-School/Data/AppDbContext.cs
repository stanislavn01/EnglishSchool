using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using EnglishSchool.Entities;

namespace EnglishSchool.Data;

public class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration
                .GetConnectionString("DefaultConnection");

            optionsBuilder.UseNpgsql(connectionString);
        }
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Таблица ролей
        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("roles");

            entity.HasKey(r => r.Id);

            entity.Property(r => r.Name)
                .HasMaxLength(20)
                .IsRequired();

            entity.HasIndex(r => r.Name)
                .IsUnique();
        });

        // Таблица пользователей
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");

            entity.HasKey(u => u.Id);

            entity.Property(u => u.Email)
                .HasMaxLength(255)
                .IsRequired();

            entity.HasIndex(u => u.Email)
                .IsUnique();

            entity.Property(u => u.FullName)
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(u => u.Phone)
                .HasMaxLength(20);

            entity.Property(u => u.PasswordHash)
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(u => u.IsBlocked)
                .HasDefaultValue(false);

            entity.Property(u => u.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .IsRequired();
        });

        // Таблица категорий
        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("categories");

            entity.HasKey(c => c.Id);

            entity.Property(c => c.Name)
                .HasMaxLength(100)
                .IsRequired();

            entity.HasIndex(c => c.Name)
                .IsUnique();
        });

        // Таблица курсов
        modelBuilder.Entity<Course>(entity =>
        {
            entity.ToTable("courses");

            entity.HasKey(c => c.Id);

            entity.Property(c => c.Title)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(c => c.Price)
                .HasPrecision(10, 2);

            entity.Property(c => c.DurationWeeks)
                .IsRequired();

            entity.Property(c => c.IsHidden)
                .HasDefaultValue(false);

            entity.HasOne(c => c.Category)
                .WithMany(cat => cat.Courses)
                .HasForeignKey(c => c.CategoryId);
        });

        // Таблица записей на курсы
        modelBuilder.Entity<Enrollment>(entity =>
        {
            entity.ToTable("enrollments");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.EnrolledAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.Property(e => e.IsPaid)
                .HasDefaultValue(false);

            entity.HasIndex(e => new { e.UserId, e.CourseId })
                .IsUnique();

            entity.HasOne(e => e.User)
                .WithMany(u => u.Enrollments)
                .HasForeignKey(e => e.UserId);

            entity.HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseId);
        });
    }
}