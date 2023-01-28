using BookStore.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookStore.API.Data
{
    public class BookStoreContext : IdentityDbContext<ApplicationUser> // DbContext
    {
        public BookStoreContext(DbContextOptions<BookStoreContext> otions)
            : base(otions)
        {

        }

        public DbSet<Book> Books { get; set; }

        // add the db context 
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server=(LocalDb)\\MSSQLLocalDB;Database=ForLet;Trusted_Connection=True;MultipleActiveResultSets=True");
        //    base.OnConfiguring(optionsBuilder);
        //}

    }
}
