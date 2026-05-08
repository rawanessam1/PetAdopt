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
        public DbSet<adoptionRequest> adoptionRequests { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Pet>()
                .HasOne(p => p.user)          
                .WithMany(u => u.Pets)         
                .HasForeignKey(p => p.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);

            /*   modelBuilder.Entity<PetImage>()
                   .HasOne(pi => pi.Pet)
                   .WithMany(p => p.Images)
                   .HasForeignKey(pi => pi.PetId)
                   .OnDelete(DeleteBehavior.Cascade);  */

            // added this
            modelBuilder.Entity<Pet>()
                 .HasMany(p => p.Images)
                 .WithOne()
                 .HasForeignKey(pi => pi.PetId)
                 .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Pet>()
                .Property(p => p.Status)
                .HasConversion<string>();

            modelBuilder.Entity<User>()   // 3shan role teb2a string fe db msh 0,1,2
                .Property(u => u.Role)
                .HasConversion<string>();


            // added these for review
            modelBuilder.Entity<Review>()
                 .HasOne(r => r.Adopter)
                 .WithMany(u => u.ReviewsGiven)
                 .HasForeignKey(r => r.AdopterId)
                 .OnDelete(DeleteBehavior.Restrict);
  
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Owner)
                .WithMany(u => u.ReviewsReceived)
                .HasForeignKey(r => r.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);


            // favourites
            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.Pet)
                .WithMany(p => p.Favorites)
                .HasForeignKey(f => f.PetId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.User)
                .WithMany(u => u.Favorites)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Restrict);


            // updates 
            modelBuilder.Entity<adoptionRequest>()
                .HasOne(a => a.Pet)
                .WithMany(p => p.adoptionRequests)
                .HasForeignKey(a => a.PetId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<adoptionRequest>()
                .HasOne(a => a.Adopter)
                .WithMany(u => u.adoptionRequests)
                .HasForeignKey(a => a.AdopterId)
                .OnDelete(DeleteBehavior.Restrict);

            // Enum Conversions to Strings
            modelBuilder.Entity<Pet>().Property(p => p.Status).HasConversion<string>();
            modelBuilder.Entity<User>().Property(u => u.Role).HasConversion<string>();
            modelBuilder.Entity<adoptionRequest>().Property(a => a.Status).HasConversion<string>();

        }
    }
}