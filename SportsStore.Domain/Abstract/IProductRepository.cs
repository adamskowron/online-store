using System.Collections.Generic;
using OnlineStore.Domain.Entities;

namespace OnlineStore.Domain.Abstract {
    public interface IProductRepository {

        IEnumerable<Product> Products { get; }

        void SaveProduct(Product product);

        Product DeleteProduct(int productID);
    }
}