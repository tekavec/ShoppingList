using System.ComponentModel.DataAnnotations;

namespace ShoppingList.API.Models
{
    public class Drink
    {
        [Key]
        public string Name { get; set; }

        public int Quantity { get; set; }
    }
}