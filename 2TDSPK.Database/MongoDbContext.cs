using _2TDSPK.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace _2TDSPK.Database
{
    public class MongoDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserLike> UserLikes { get; set; }

        public MongoDbContext(DbContextOptions<MongoDbContext> options) : base(options) 
        {
            
        }      
    }
}
