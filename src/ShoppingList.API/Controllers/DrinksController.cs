using Microsoft.AspNetCore.Mvc;
using ShoppingList.API.Core;
using ShoppingList.API.Models;

namespace ShoppingList.API.Controllers
{
    [Route("api/drinks")]
    public class DrinksController : Controller
    {
        private readonly IRepository<Drink, string> drinkRepository;

        public DrinksController(IRepository<Drink, string> drinkRepository)
        {
            this.drinkRepository = drinkRepository;
        }

        [HttpPost]
        public IActionResult Post([FromBody] Drink drink)
        {
            if (drinkRepository.Get(drink.Name) != null)
                ModelState.AddModelError("Description", "Drink with the same name already exist.");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            drinkRepository.Add(drink);
            return new CreatedAtRouteResult("GetDrink", new {id = drink.Name}, drink);
        }

        [HttpGet]
        public JsonResult Get()
        {
            return new JsonResult(drinkRepository.GetAll()) { StatusCode = 200 };
        }

        [HttpGet("{id}", Name="GetDrink")]
        public IActionResult Get(string id)
        {
            var drink = drinkRepository.Get(id);
            if (drink == null)
                ModelState.AddModelError("Description", $"No drink with the name '{id}'.");
            if (!ModelState.IsValid)
                return NotFound(ModelState);
            return new JsonResult(drink) {StatusCode = 200};
        }

        [HttpPut]
        public IActionResult Update([FromBody] Drink drink)
        {
            if (drink == null || drinkRepository.Get(drink.Name) == null)
                return BadRequest();
            drinkRepository.Update(drink.Name, drink);
            return NoContent();
        }
    }
}