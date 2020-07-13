using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using System.Linq;
using System.Web.Mvc;

namespace SportsStore.UnitTests {
    [TestClass]
    public class ImageTests {

        [TestMethod]
        public void Can_Retrieve_Image_Data() {

            // przygotowanie - tworzenie produktu z danymi zdjęcia
            Product prod = new Product {
                ProductID = 2,
                Name = "Test",
                ImageData = new byte[] { },
                ImageMimeType = "image/png"
            };

            // przygotowanie - tworzenie imitacji repozytorium
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product {ProductID = 1, Name = "P1"},
                prod,                            
                new Product {ProductID = 3, Name = "P3"}
            }.AsQueryable());

            // przygotowanie - utworzenie kontrolera
            ProductController target = new ProductController(mock.Object);

            // działanie - wywołanie metody akcji GetImage
            ActionResult result = target.GetImage(2);

            // asercje
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(FileResult));
            Assert.AreEqual(prod.ImageMimeType, ((FileResult)result).ContentType);
        }

        [TestMethod]
        public void Cannot_Retrieve_Image_Data_For_Invalid_ID() {

            // przygotowanie - tworzenie imitacji repozytorium
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product {ProductID = 1, Name = "P1"},
                new Product {ProductID = 2, Name = "P2"}
            }.AsQueryable());

            // przygotowanie - utworzenie kontrolera
            ProductController target = new ProductController(mock.Object);

            // działanie - wywołanie metody akcji GetImage
            ActionResult result = target.GetImage(100);

            // asercje
            Assert.IsNull(result);
        }
    }
}
