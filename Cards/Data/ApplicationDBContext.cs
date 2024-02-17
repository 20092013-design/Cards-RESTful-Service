using Cards.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Cards.Data
{
    public class ApplicationDBContext:IdentityDbContext<ApplicationUser>
    {
       
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext>Options):base (Options)
        {
                
        }
        public DbSet<CardModel> Cards { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
       

    }
}
