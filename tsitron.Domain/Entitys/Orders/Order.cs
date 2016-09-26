using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using tsitron.Domain.Entitys.Customers;
using tsitron.Domain.Entitys.Goods;
using tsitron.Domain.Models;

namespace tsitron.Domain.Entitys.Orders
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal Summ { get; set; }
        public OrderStatus Status { get; set; }
        public virtual Recipient Recipient { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Bouquet Bouquet { get; set; }
        public string Info { get; set; }

    }

    public class Recipient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public bool RecipientMe { get; set; }
        public string Adres { get; set; }
        public string Date { get; set; }
        public bool UnknownAddress { get; set; }
        public bool Incognito { get; set; }

    }

    public class PreOrder
    {
        public string WishDate { get; set; }
        public int Marker { get; set; }
        public PhotoView Avatar { get; set; }
        public BouqView Bouquet { get; set; }
    }

    public class BouqView
    {
        public string Title { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

    public class OrderAccept
    {
        public string OrderDate { get; set; }
        public string Info { get; set; }
        public Bouquet Bouquet { get; set; }
        public Customer Customer { get; set; }
        public Recipient Recipient { get; set; }

    }

    public enum OrderStatus
    {
        Новый,Принят,Отказ,Доставка,Исполнен,Просрочен
    }
}