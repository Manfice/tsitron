using tsitron.Domain.Entitys.Orders;

namespace tsitron.Domain.Models
{
    public class CartViewModel
    {
        public Cart Cart { get; set; }
        public string ReturnUrl { get; set; }
    }
}