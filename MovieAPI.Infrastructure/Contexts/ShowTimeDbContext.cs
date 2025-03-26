using Microsoft.EntityFrameworkCore;
using MovieAPI.Core.Models.ShowTime;

namespace MovieAPI.Infrastructure.Contexts;

public class ShowTimeDbContext : DbContext
{
    public DbSet<ShowTime> ShowTimes { get; set; } = null!;
    public DbSet<Room> Rooms { get; set; } = null!;
    public DbSet<Reservation> Reservations { get; set; } = null!;
    public DbSet<MovieShowTime> MovieShowTimes { get; set; } = null!;

    public ShowTimeDbContext(DbContextOptions<ShowTimeDbContext> options) : base(options)
    {
    }
}