using Bogus;
using Microsoft.EntityFrameworkCore;
using MovieAPI.Core.Models.ShowTime;
using MovieAPI.Infrastructure.Contexts;

namespace MovieAPI.Application.Extensions;

public static class MigrationsExtensions
{
    private const int NUM_MOVIES = 300;
    private const int NUM_ROOMS = 50;
    private const int NUM_SHOWTIMES = 400;
    private const int NUM_RESERVATIONS = 2000;

    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();
        using ShowTimeDbContext dbContext =
            scope.ServiceProvider.GetService<ShowTimeDbContext>();
        Console.WriteLine("Applying migrations...");
        try
        {
            dbContext.Database.Migrate();
            Console.WriteLine("Migrations applied successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Migration failed: {ex.Message}");
        }

        SeedData(dbContext);
    }

    private static void SeedData(ShowTimeDbContext dbContext)
    {
        AddMovies(dbContext);
        AddRooms(dbContext);
        AddShowTimes(dbContext);
        AddReservations(dbContext);
    }

    private static void AddMovies(ShowTimeDbContext dbContext)
    {
        if (!dbContext.MovieShowTimes.Any())
        {
            var fakerMovies = new Faker<MovieShowTime>()
                .RuleFor(m => m.Id, f => f.IndexFaker + 1)
                .RuleFor(m => m.Title, f => f.Lorem.Sentence(3))
                .RuleFor(m => m.Genres, f => f.PickRandom(["Action", "Drama", "Comedy", "Horror", "Sci-Fi", "Romance"], f.Random.Int(1,3)).ToList())
                .RuleFor(m => m.Languages, f => f.PickRandom(["English", "Spanish", "French", "German", "Italian"], f.Random.Int(1,2)).ToList() )
                .RuleFor(m => m.ReleaseYear, f => f.Random.Number(1930, 2025));

            var movies = fakerMovies.Generate(NUM_MOVIES);
            dbContext.MovieShowTimes.AddRange(movies);
            dbContext.SaveChanges();
        }
    }

    private static void AddRooms(ShowTimeDbContext dbContext)
    {
        if (!dbContext.Rooms.Any())
        {
            var fakerRooms = new Faker<Room>()
                .RuleFor(r => r.Id, f => f.IndexFaker + 1)
                .RuleFor(r => r.Name, (f, r) => $"Room {r.Id}")
                .RuleFor(r => r.Capacity, f => f.Random.Number(30, 150));
            var rooms = fakerRooms.Generate(NUM_ROOMS);
            dbContext.Rooms.AddRange(rooms);
            dbContext.SaveChanges();
        }
    }

    private static void AddShowTimes(ShowTimeDbContext dbContext)
    {
        if (!dbContext.ShowTimes.Any())
        {
            var movies = dbContext.MovieShowTimes.ToList();
            var rooms = dbContext.Rooms.ToList();
            var showTimesPerDay = 2;
            var random = new Random();

            var startDate = DateTime.UtcNow;
            for (int i = 0; i < NUM_SHOWTIMES; i++)
            {
                var movie = movies[random.Next(movies.Count - 1)];
                var room = rooms[random.Next(rooms.Count - 1)];
                var showTime = new ShowTime
                {
                    Id = i + 1,
                    MovieId = movie.Id,
                    RoomId = room.Id,
                    Schedule = startDate.AddDays(i / showTimesPerDay).AddHours(random.Next(10, 24)),
                    AvailableSeats = room.Capacity
                };
                dbContext.ShowTimes.Add(showTime);
            }
            dbContext.SaveChanges();

        }
    }

    private static void AddReservations(ShowTimeDbContext dbContext)
    {
        if (!dbContext.Reservations.Any())
        {
            var showTimes = dbContext.ShowTimes.ToList();
            var random = new Random();
            var faker = new Faker();
            var reservationStatusesAndWeights = new[]
            {
                ( ReservationStatus.Confirmed, .7f ),
                ( ReservationStatus.Pending, .2f ),
                ( ReservationStatus.Cancelled, .1f )
            };
            for (int i = 0; i < NUM_RESERVATIONS; i++)
            {
                var seatsReservedByTheUser = random.Next(1, 10);
                var shuffleShowTime = showTimes.OrderBy(x => random.Next()).ToList();
                ShowTime showTime = shuffleShowTime.FirstOrDefault(st => st.AvailableSeats >= seatsReservedByTheUser);
                if (showTime != null)
                {
                    var reservation = new Reservation
                    {
                        Id = i + 1,
                        ShowTimeId = showTime.Id,
                        UserId = faker.Internet.UserName(),
                        SeatsReserved = seatsReservedByTheUser,
                        Status = faker.Random.WeightedRandom(
                            [.. reservationStatusesAndWeights.Select(x => x.Item1)], 
                            [.. reservationStatusesAndWeights.Select(x => x.Item2)])
                    };
                    dbContext.Reservations.Add(reservation);
                    if (reservation.Status == ReservationStatus.Confirmed)
                        showTime.AvailableSeats -= seatsReservedByTheUser;
                }
            }
            dbContext.SaveChanges();
        }
    }


}
