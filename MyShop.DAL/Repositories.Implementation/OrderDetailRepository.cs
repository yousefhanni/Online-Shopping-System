using Myshop.DAL.Data;
using MyShop.Domain.Models;
using MyShop.Domain.Repositories.Contract;


namespace MyShop.DAL.Repositories.Implementation
{
    public class OrderDetailRepository : GenericRepository<OrderDetails>, IOrderDetailRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderDetailRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(OrderDetails Orderdetail)
        {
            _context.OrderDetails.Update(Orderdetail);
        }
    }
}
