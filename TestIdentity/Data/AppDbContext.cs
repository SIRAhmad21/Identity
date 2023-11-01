using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TestIdentity.Models;

namespace TestIdentity.Data
{
    public class AppDbContext :IdentityDbContext
    {
        public AppDbContext(DbContextOptions <AppDbContext>options):base(options)   
        {
            
        }
        public DbSet<Prodect> Prodectes { get; set;}    

    }
}
