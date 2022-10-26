using Microsoft.EntityFrameworkCore;

namespace UdemyIdentityServer.AuthServer.Models
{
    public class CustomDbContext:DbContext
    {
        public CustomDbContext(DbContextOptions<CustomDbContext> opts):base(opts)
        {
            
        }

        public DbSet<CustomUser> CustomUser { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomUser>().HasData(
                new CustomUser(){ Id=1,Email="ugur@outlook.com",Password="password",City="İstanbul",UserName="UğurCengiz"},
                new CustomUser() { Id = 2, Email = "ahmet@outlook.com", Password = "password", City = "Ankara", UserName = "Ahmet16" },
                new CustomUser() { Id = 3, Email = "mehmet@outlook.com", Password = "password", City = "Konya", UserName = "Mehmet16" }

            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
