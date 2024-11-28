using _2TDSPK.Database;
using _2TDSPK.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2TDSPK.Repository
{
    public class MongoDBRepository<T>(MongoDbContext mongoDBContext) : IRepository<T> where T : class
    {
        private readonly MongoDbContext _mongoDBContext = mongoDBContext;
        private readonly DbSet<T> _dbSet;
        public void Add(T entity)
        {
            _mongoDBContext.Add(entity);

            _mongoDBContext.SaveChanges();
        }

        public void Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAll()
        {
            return _mongoDBContext.Set<T>().ToList();
        }

        public T GetById(string? id)
        {
            return _mongoDBContext.Set<T>().Find(id);
        }

        public T GetById(int? id)
        {
            throw new NotImplementedException();
        }

        public void Update(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
