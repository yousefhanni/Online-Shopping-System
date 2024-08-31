using MyShop.Domain.Models;


namespace MyShop.Domain.Repositories.Contract
{
    public interface IOrderDetailRepository:IGenericRepository<OrderDetails>
    {
        void Update(OrderDetails orderDetail);
    }
}
