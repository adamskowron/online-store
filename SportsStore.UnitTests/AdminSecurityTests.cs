using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OnlineStore.WebUI.Controllers;
using OnlineStore.WebUI.Infrastructure.Abstract;
using OnlineStore.WebUI.Models;

namespace OnlineStore.UnitTests {
    [TestClass]
    public class AdminSecurityTests {

        [TestMethod]
        public void Can_Login_With_Valid_Credentials() {

            // przygotowanie - utworzenie imitacji dostawcy uwierzytelniania
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("admin", "secret")).Returns(true);

            // przygotowanie - utworzenie modelu widoku
            LoginViewModel model = new LoginViewModel {
                UserName = "admin",
                Password = "secret"
            };

            // przygotowanie - utworzenie kontrolera
            AccountController target = new AccountController(mock.Object);

            // działanie - uwierzytelnienie z użyciem prawidłowych danych
            ActionResult result = target.Login(model, "/MyURL");

            // asercje
            Assert.IsInstanceOfType(result, typeof(RedirectResult));
            Assert.AreEqual("/MyURL", ((RedirectResult)result).Url);
        }

        [TestMethod]
        public void Cannot_Login_With_Invalid_Credentials() {

            // przygotowanie - utworzenie imitacji dostawcy uwierzytelniania
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("badUser", "badPass")).Returns(false);

            // przygotowanie - utworzenie modelu widoku
            LoginViewModel model = new LoginViewModel {
                UserName = "badUser",
                Password = "badPass"
            };

            // przygotowanie - utworzenie kontrolera
            AccountController target = new AccountController(mock.Object);

            // działanie - uwierzytelnienie z użyciem prawidłowych danych
            ActionResult result = target.Login(model, "/MyURL");

            // asercje
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsFalse(((ViewResult)result).ViewData.ModelState.IsValid);
        }
    }
}
