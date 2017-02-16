using System.Collections.Generic;
using FluentAssertions;
using FluentAssertions.Mvc;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using ShoppingList.API.Controllers;
using ShoppingList.API.Core;
using ShoppingList.API.Models;

namespace ShoppingList.Tests.Controllers
{
    [TestFixture]
    public class DrinksControllerShould
    {
        private readonly string aDrinkName = "Pepsi";
        private readonly string anotherDrinkName = "Fanta";
        private readonly int aQuantity = 1;
        private IRepository<Drink, string> drinkRepository;
        private DrinksController controller;

        [SetUp]
        public void SetUp()
        {
            drinkRepository = Substitute.For<IRepository<Drink, string>>();
            controller = new DrinksController(drinkRepository);
        }

        [Test]
        public void store_a_drink_and_return_resource_route()
        {
            Drink aDrink = new Drink {Name = aDrinkName, Quantity = aQuantity };

            var result = controller.Post(aDrink);

            drinkRepository.Received(1).Add(aDrink);
            result.Should().BeOfType<CreatedAtRouteResult>();
            var routeResult = (CreatedAtRouteResult) result;
            routeResult.StatusCode.ShouldBeEquivalentTo(201);
            routeResult.RouteName.ShouldBeEquivalentTo("GetDrink");
            routeResult.RouteValues["id"].ShouldBeEquivalentTo(aDrinkName);
        }

        [Test]
        public void retrieve_a_drink_by_its_name()
        {
            Drink aDrink = new Drink {Name = aDrinkName, Quantity = aQuantity };
            drinkRepository.Get(aDrinkName).Returns(aDrink);

            var result = controller.Get(aDrinkName);

            drinkRepository.Received(1).Get(aDrinkName);
            result.Should().BeOfType<JsonResult>();
            var jsonResult = (JsonResult) result;
            jsonResult.StatusCode.ShouldBeEquivalentTo(200);
            jsonResult.Value.Should().BeOfType<Drink>();
        }

        [Test]
        public void retrieve_all_drinks()
        {
            Drink aDrink = new Drink {Name = aDrinkName, Quantity = aQuantity };
            Drink anotherDrink = new Drink {Name = anotherDrinkName, Quantity = aQuantity };
            drinkRepository.GetAll().Returns(new List<Drink> {aDrink, anotherDrink});

            var result = controller.Get();

            drinkRepository.Received(1).GetAll();
            result.Should().BeOfType<JsonResult>();
            result.StatusCode.ShouldBeEquivalentTo(200);
            result.Value.Should().BeOfType<List<Drink>>();
        }

        [Test]
        public void update_a_drink()
        {
            Drink aDrink = new Drink {Name = aDrinkName, Quantity = aQuantity };

            var result = controller.Update(aDrink);

            drinkRepository.Received(1).Update(aDrinkName, aDrink);
            result.Should().BeOfType<NoContentResult>();
            ((NoContentResult)result).StatusCode.ShouldBeEquivalentTo(204);
        }

        [Test]
        public void not_update_non_existing_drink_and_return_bad_request()
        {
            Drink aDrink = new Drink {Name = aDrinkName, Quantity = aQuantity};

            var result = controller.Update(aDrink);

            drinkRepository.Received(0).Update(aDrinkName, aDrink);
            result.Should().BeOfType<BadRequestResult>();
            ((BadRequestResult)result).StatusCode.ShouldBeEquivalentTo(400);
        }
    }
}