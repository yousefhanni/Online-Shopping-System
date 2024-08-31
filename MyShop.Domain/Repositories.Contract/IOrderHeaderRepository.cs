using MyShop.Domain.Models;

namespace MyShop.Domain.Repositories.Contract
{
    public interface IOrderHeaderRepository : IGenericRepository<OrderHeader>
    {
        void Update(OrderHeader orderHeader);
        void UpdateStatus(int id, string? OrderStatus, string? PaymentStatus);
    }
}
