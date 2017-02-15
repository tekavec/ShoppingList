using System;
using System.Linq;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using ShoppingList.API.Core;
using ShoppingList.API.Db;
using ShoppingList.API.Models;

namespace ShoppingList.Tests.Models
{
    [TestFixture]
    public class DrinkRepositoryShould
    {
        private readonly string aDrinkName = "Pepsi";
        private readonly string anotherDrinkName = "Coca-Cola";
        private readonly int aQuantity = 1;
        private ShoppingListContext context;
        private IRepository<Drink, string> drinkRepository;

        [SetUp]
        public void SetUp()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ShoppingListContext>();
            optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            context = new ShoppingListContext(optionsBuilder.Options);
            drinkRepository = new Repository<Drink, string>(context);
        }

        [Test]
        public void store_a_drink()
        {
            var drinkDto = new Drink {Name = aDrinkName, Quantity = aQuantity};

            drinkRepository.Add(drinkDto);

            drinkRepository.Get(drinkDto.Name).ShouldBeEquivalentTo(drinkDto);
        }

        [Test]
        public void not_store_more_than_one_drink_with_the_same_name()
        {
            var drinkDto = new Drink {Name = aDrinkName, Quantity = aQuantity};

            drinkRepository.Add(drinkDto);

            Assert.Throws<ArgumentException>(() => drinkRepository.Add(drinkDto));
        }

        [Test]
        public void retrieve_a_drink_by_a_name()
        {
            var drinkDto = new Drink { Name = aDrinkName, Quantity = aQuantity };
            drinkRepository.Add(drinkDto);
            drinkRepository.Add(new Drink { Name = anotherDrinkName, Quantity = aQuantity });

            var drink = drinkRepository.Get(aDrinkName);

            drink.ShouldBeEquivalentTo(drinkDto);
        }

        [Test]
        public void not_retrieve_not_stored_drink()
        {
            drinkRepository.Add(new Drink { Name = anotherDrinkName, Quantity = aQuantity });

            var drink = drinkRepository.Get(aDrinkName);

            drink.ShouldBeEquivalentTo(null);
        }

        [Test]
        public void retrieve_all_drinks()
        {
            drinkRepository.Add(new Drink { Name = aDrinkName, Quantity = aQuantity });
            drinkRepository.Add(new Drink { Name = anotherDrinkName, Quantity = aQuantity });

            var drinks = drinkRepository.GetAll();

            drinks.Count().ShouldBeEquivalentTo(2);
        }


    }
}