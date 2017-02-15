using Microsoft.EntityFrameworkCore;
using ShoppingList.API.Models;

namespace ShoppingList.API.Db
{
    public class ShoppingListContext : DbContext
    {
        public ShoppingListContext(DbContextOptions<ShoppingListContext> options) : base(options)
        {
        }

        public DbSet<Drink> Drinks { get; set; }
    }
}