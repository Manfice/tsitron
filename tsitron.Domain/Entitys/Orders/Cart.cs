using System.Collections.Generic;
using System.Linq;
using tsitron.Domain.Entitys.Goods;

namespace tsitron.Domain.Entitys.Orders
{
    public class Cart
    {
        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        private List<CartItem> _cartCollection = new List<CartItem>();
        public IEnumerable<CartItem> GetCartItems => _cartCollection;

        public void AddItem(Bouquet bouquet, Recipient recipient)
        {
            var item = _cartCollection.FirstOrDefault(b => b.Bouquet.Id == bouquet.Id);
            if (item==null)
            {
                _cartCollection.Add(new CartItem {Bouquet = bouquet, Quantity = 1, Recipient = recipient});
            }
            else
            {
                item.Quantity ++;
            }
        }

        public decimal CulcPrice()
        {
            return _cartCollection.Sum(s => s.Bouquet.Price.PriceValue * s.Quantity);
        }
        public void RemoveItem(Bouquet bouquet)
        {
            _cartCollection.RemoveAll(b => b.Bouquet.Id == bouquet.Id);
        }

        public void ClearCart()
        {
            _cartCollection.Clear();
        }

    }

    public class CartItem
    {
        public Bouquet Bouquet { get; set; }
        public Recipient Recipient { get; set; }
        public int Quantity { get; set; }
        public string ReturnUrl { get; set; }
    }
}