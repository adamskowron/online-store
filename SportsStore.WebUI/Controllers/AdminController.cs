using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineStore.Domain.Abstract;
using OnlineStore.Domain.Entities;

namespace OnlineStore.WebUI.Controllers {

    [Authorize]
    public class AdminController : Controller {
        private IProductRepository repository;

        public AdminController(IProductRepository repo) {
            repository = repo;
        }

        public ViewResult Index() {
            return View(repository.Products);
        }

        public ViewResult Edit(int productId) {
            Product product = repository.Products
                .FirstOrDefault(p => p.ProductID == productId);
            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(Product product, HttpPostedFileBase image = null) {
            if (ModelState.IsValid) {
                if (image != null) {
                    product.ImageMimeType = image.ContentType;
                    product.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(product.ImageData, 0, image.ContentLength);
                }
                repository.SaveProduct(product);
                TempData["message"] = string.Format("Zapisano {0} ", product.Name);
                return RedirectToAction("Index");
            } else {
                // błąd w wartościach danych
                return View(product);
            }
        }

        public ViewResult Create() {
            return View("Edit", new Product());
        }

        [HttpPost]
        public ActionResult Delete(int productId) {
            Product deletedProduct = repository.DeleteProduct(productId);
            if (deletedProduct != null) {
                TempData["message"] = string.Format("Usunięto {0}",
                    deletedProduct.Name);
            }
            return RedirectToAction("Index");
        }
    }
}
