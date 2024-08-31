using MyShop.Domain.Models;

namespace MyShop.Domain.ViewModels
{
	public class ShoppingCartVM
	{
        public IEnumerable<ShoppingCart> CartsList { get; set; }
        public OrderHeader OrderHeader { get; set; }

    }
}
