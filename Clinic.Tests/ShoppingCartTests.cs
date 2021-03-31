using Clinic.Database;
using Clinic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Clinic.Tests
{
    public class ShoppingCartTests
    {
        [Fact]
        public void Add_New_Lines_To_Shopping_Cart()
        {
            Service p1 = new Service { ServiceId = 1, Name = "P1" };
            Service p2 = new Service { ServiceId = 2, Name = "P2" };

            using (var context = new ApplicationDbContext(InMemoryDatabase.GetOptions("Add_New_Lines_To_Shopping_Cart")))
            {
                ShoppingCart target = new ShoppingCart(context);

                target.AddToCart(p1, 1);
                target.AddToCart(p2, 1);
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(InMemoryDatabase.GetOptions("Add_New_Lines_To_Shopping_Cart")))
            {
                Assert.Equal(2, context.ShoppingCartItems.Count());
            }
        }

        [Fact]
        public void Remove_Lines_From_Shopping_Cart()
        {
            Service p1 = new Service { ServiceId = 1, Name = "P1" };
            Service p2 = new Service { ServiceId = 2, Name = "P2" };

            using (var context = new ApplicationDbContext(InMemoryDatabase.GetOptions("Remove_Lines_From_Shopping_Cart")))
            {
                ShoppingCart target = new ShoppingCart(context);

                target.AddToCart(p1, 1);
                target.AddToCart(p2, 1);
                context.SaveChanges();

                Assert.Equal(2, context.ShoppingCartItems.Count());

                target.RemoveFromCart(p1);
                target.RemoveFromCart(p2);
                context.SaveChanges();

                Assert.Equal(0, context.ShoppingCartItems.Count());
            }
        }

        [Fact]
        public void Calculate_Shopping_Cart_Total()
        {
            Service p1 = new Service { ServiceId = 1, Name = "P1", Price = 1000 };
            Service p2 = new Service { ServiceId = 2, Name = "P2", Price = 2000 };

            using (var context = new ApplicationDbContext(InMemoryDatabase.GetOptions("Calculate_Shopping_Cart_Total")))
            {
                ShoppingCart target = new ShoppingCart(context);

                target.AddToCart(p1, 2);
                target.AddToCart(p2, 3);
                context.SaveChanges();

                Assert.Equal(2, context.ShoppingCartItems.Count());

                decimal result = target.GetShoppingCartTotal();

                Assert.Equal(8000, result);
            }
        }

        [Fact]
        public void Clear_Shopping_Cart()
        {
            Service p1 = new Service { ServiceId = 1, Name = "P1", Price = 1000 };
            Service p2 = new Service { ServiceId = 2, Name = "P2", Price = 2000 };

            using (var context = new ApplicationDbContext(InMemoryDatabase.GetOptions("Clear_Shopping_Cart")))
            {
                ShoppingCart target = new ShoppingCart(context);

                target.AddToCart(p1, 2);
                target.AddToCart(p2, 3);
                context.SaveChanges();

                Assert.Equal(2, context.ShoppingCartItems.Count());

                target.ClearCart();

                Assert.Equal(0, context.ShoppingCartItems.Count());
            }
        }
    }
}
