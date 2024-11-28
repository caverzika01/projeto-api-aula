using _2TDSPK.Database;
using _2TDSPK.Database.Models;
using _2TDSPK.Repository.Interface;

namespace _2TDSPK.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly MongoDbContext _productsDbContext;

        public ProductRepository(MongoDbContext productsDbContext)
        {
            _productsDbContext = productsDbContext;
        }

        public void Add(Product entity)
        {
           _productsDbContext.Add(entity);
           
            _productsDbContext.SaveChanges();
        }
    }
}
