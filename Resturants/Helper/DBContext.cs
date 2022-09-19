using Microsoft.EntityFrameworkCore;
using Resturants.Models;

namespace Resturants.Helper
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions options) : base(options)
        {

        }

       public DbSet<User> Users { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Tokens> Tokens { get; set; }


        public DbSet<Cart> Carts { get; set; }

    }
}
