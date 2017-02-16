using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ShoppingList.API.Db;

namespace ShoppingList.API.Core
{
    public class Repository<T, TKey> : IRepository<T, TKey> where T : class
    {
        private readonly ShoppingListContext dbContext;
        private readonly DbSet<T> dbSet;

        public Repository(ShoppingListContext dbContext)
        {
            this.dbContext = dbContext;
            dbSet = dbContext.Set<T>();
        }

        public void Add(T t)
        {
            dbSet.Add(t);
            Save();
        }

        public IEnumerable<T> GetAll()
        {
            return dbSet.ToList();
        }   

        public T Get(TKey id)
        {
            return dbSet.Find(id);
        }

        public void Update(TKey id, T t)
        {
            var find = dbSet.Find(id);
            dbContext.Entry(find).CurrentValues.SetValues(t);
            Save();
        }
    
        private void Save()
        {
            dbContext.SaveChanges();
        }
    }
}