using Microsoft.EntityFrameworkCore;
using backend.Models;

namespace backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Pet> Pets { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<PetImage> PetImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pet>()
                .HasOne(p => p.user)          
                .WithMany(u => u.Pets)         
                .HasForeignKey(p => p.OwnerId)
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<PetImage>()
                .HasOne(pi => pi.Pet)
                .WithMany(p => p.Images)
                .HasForeignKey(pi => pi.PetId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Pet>()
                .Property(p => p.Status)
                .HasConversion<string>();

            modelBuilder.Entity<User>()   // 3shan role teb2a string fe db msh 0,1,2
                .Property(u => u.Role)
                .HasConversion<string>();
        }
    }
}