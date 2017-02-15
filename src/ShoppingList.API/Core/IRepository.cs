using System.Collections.Generic;

namespace ShoppingList.API.Core
{
    public interface IRepository<T, in TKey> where T : class
    {
        void Add(T t);
        IEnumerable<T> GetAll();
        T Get(TKey id);
    }
}