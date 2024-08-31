using MyShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Domain.ViewModels
{
    public class ProductDetailsViewModel
    {
        public Product Product { get; set; }

        [Range(1, 10, ErrorMessage = "You must enter a value between 1 and 10")]
        public int Count { get; set; }

        // هذه الخاصية لإضافة المنتجات ذات الصلة التي ستظهر في صفحة التفاصيل
        public List<Product> RelatedProducts { get; set; }
    }
}
