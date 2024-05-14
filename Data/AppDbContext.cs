using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ReservationSystem.Models;
namespace ReservationSystem.Data{
    public class AppDbContext : IdentityDbContext
    {
    public AppDbContext(DbContextOptions<AppDbContext> options) :
    base(options)
    {
    }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    }
}