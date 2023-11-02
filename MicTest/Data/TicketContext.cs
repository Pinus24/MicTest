using Microsoft.EntityFrameworkCore;
using MicTest.Models;

namespace MicTest.Data
{
    public class TicketContext : DbContext
    {
        public TicketContext(DbContextOptions<TicketContext> options) : base(options) { }

        public DbSet<Passenger> Passenger { get; set; }
        public DbSet<AirTicket> AirTicket { get; set; }
        public DbSet<Document> Document { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Passenger>().ToTable("Passenger")
            .HasOne(a => a.Document)
            .WithOne(a => a.Passenger)
            .HasForeignKey<Document>(c => c.PassengerId)
            .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Document>().ToTable("Document")
            .HasOne(a => a.Passenger)
            .WithOne(a => a.Document)
            .HasForeignKey<Passenger>(c => c.DocumentId)
            .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<AirTicket>().ToTable("AirTicket")
                .HasOne(a => a.Document)
                .WithMany(a => a.AirTickets)
                .OnDelete(DeleteBehavior.Cascade);


        }
    }
}
