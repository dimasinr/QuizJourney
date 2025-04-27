using Microsoft.EntityFrameworkCore;
using QuizJourney.Models;

namespace QuizJourney.Data;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Choice> Choices { get; set; }
    public DbSet<StudentAnswer> StudentAnswers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var password = BCrypt.Net.BCrypt.HashPassword("12345");

        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Username = "dimas", PasswordHash = password, Role = "Teacher" },
            new User { Id = 2, Username = "ricky", PasswordHash = password, Role = "Student" }
        );
    }
}
