using OnlineStore.Domain.Entities;

namespace OnlineStore.Domain.Abstract {

    public interface IOrderProcessor {

        void ProcessOrder(Cart cart, ShippingDetails shippingDetails);
    }
}
