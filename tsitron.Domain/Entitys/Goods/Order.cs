using System.Security.AccessControl;

namespace tsitron.Domain.Entitys.Goods
{
    public class Order
    {
        public int Id { get; set; }
        public string GoodsType { get; set; } // Чё берем, букет или цветы?
        public int GoodsId { get; set; }
         
    }
}