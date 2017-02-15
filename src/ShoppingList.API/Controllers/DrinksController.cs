using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ShoppingList.API.Core;
using ShoppingList.API.Models;

namespace ShoppingList.API.Controllers
{
    [Route("api/drinks")]
    public class DrinksController
    {
        private readonly IRepository<Drink, string> drinkRepository;

        public DrinksController(IRepository<Drink, string> drinkRepository)
        {
            this.drinkRepository = drinkRepository;
        }

        [HttpPost]
        public IActionResult Post([FromBody] Drink drink)
        {
            drinkRepository.Add(drink);
            return new CreatedAtRouteResult("GetDrink", new {id = drink.Name}, drink);
        }

        [HttpGet]
        public JsonResult Get()
        {
            return new JsonResult(drinkRepository.GetAll()) { StatusCode = 200 };
        }

        [HttpGet("{id}", Name="GetDrink")]
        public JsonResult Get(string id)
        {
            return new JsonResult(drinkRepository.Get(id)) {StatusCode = 200};
        }
    }
}