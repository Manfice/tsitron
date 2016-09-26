using System;

namespace tsitron.Domain.Entitys.Goods
{
    public class Price
    {
        public int Id { get; set; }
        public string GoodsType { get; set; }
        public string Title { get; set; }
        public decimal PrevPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal PriceValue { get; set; }
        public Flower Flower { get; set; }
    }
}