   using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OnlineStore.Domain.Abstract;
using OnlineStore.Domain.Entities;
using OnlineStore.WebUI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace OnlineStore.UnitTests {
    [TestClass]
    public class AdminTests {

        [TestMethod]
        public void Index_Contains_All_Products() {
            // przygotowanie - tworzenie imitacji repozytorium
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product {ProductID = 1, Name = "P1"},
                new Product {ProductID = 2, Name = "P2"},
                new Product {ProductID = 3, Name = "P3"},
            });

            // przygotowanie - utworzenie kontrolera 
            AdminController target = new AdminController(mock.Object);


            // działanie
            Product[] result = ((IEnumerable<Product>)target.Index().
                ViewData.Model).ToArray();

            // asercje
            Assert.AreEqual(result.Length, 3);
            Assert.AreEqual("P1", result[0].Name);
            Assert.AreEqual("P2", result[1].Name);
            Assert.AreEqual("P3", result[2].Name);
        }

        [TestMethod]
        public void Can_Edit_Product() {

            // przygotowanie - tworzenie imitacji repozytorium
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product {ProductID = 1, Name = "P1"},
                new Product {ProductID = 2, Name = "P2"},
                new Product {ProductID = 3, Name = "P3"},
            });

            // przygotowanie - utworzenie kontrolera
            AdminController target = new AdminController(mock.Object);

            // działanie
            Product p1 = target.Edit(1).ViewData.Model as Product;
            Product p2 = target.Edit(2).ViewData.Model as Product;
            Product p3 = target.Edit(3).ViewData.Model as Product;

            // asercje
            Assert.AreEqual(1, p1.ProductID);
            Assert.AreEqual(2, p2.ProductID);
            Assert.AreEqual(3, p3.ProductID);
        }

        [TestMethod]
        public void Cannot_Edit_Nonexistent_Product() {

            // przygotowanie - tworzenie imitacji repozytorium
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product {ProductID = 1, Name = "P1"},
                new Product {ProductID = 2, Name = "P2"},
                new Product {ProductID = 3, Name = "P3"},
            });

            // przygotowanie - utworzenie kontrolera
            AdminController target = new AdminController(mock.Object);

            // działanie
            Product result = (Product)target.Edit(4).ViewData.Model;

            // asercje
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Can_Save_Valid_Changes() {

            // przygotowanie - tworzenie imitacji repozytorium
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            // przygotowanie - utworzenie kontrolera
            AdminController target = new AdminController(mock.Object);
            // przygotowanie - tworzenie produktu
            Product product = new Product { Name = "Test" };

            // działanie - próba zapisania produktu
            ActionResult result = target.Edit(product);

            // asercje - sprawdzenie, czy zostało wywołane repozytorium
            mock.Verify(m => m.SaveProduct(product));
            // asercje - sprawdzenie typu zwracanego z metody
            Assert.IsNotInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Cannot_Save_Invalid_Changes() {

            // przygotowanie - tworzenie imitacji repozytorium
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            // przygotowanie - utworzenie kontrolera
            AdminController target = new AdminController(mock.Object);
            // przygotowanie - tworzenie produktu
            Product product = new Product { Name = "Test" };
            // przygotowanie - dodanie błędu do stanu modelu
            target.ModelState.AddModelError("error", "error");

            // działanie - próba zapisania produktu
            ActionResult result = target.Edit(product);

            // asercje - sprawdzenie, czy nie zostało wywołane repozytorium
            mock.Verify(m => m.SaveProduct(It.IsAny<Product>()), Times.Never());
            // asercje - sprawdzenie typu zwracanego z metody
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Can_Delete_Valid_Products() {

            // przygotowanie - tworzenie produktu
            Product prod = new Product { ProductID = 2, Name = "Test" };

            // przygotowanie - tworzenie imitacji repozytorium
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product {ProductID = 1, Name = "P1"},
                prod,
                new Product {ProductID = 3, Name = "P3"},
            });

            // przygotowanie - utworzenie kontrolera
            AdminController target = new AdminController(mock.Object);

            // działanie - usunięcie produktu
            target.Delete(prod.ProductID);

            // asercje - upewnienie się, że metoda repozytorium
            // została wywołana z właściwym produktem 
            mock.Verify(m => m.DeleteProduct(prod.ProductID));
        }

    }
}