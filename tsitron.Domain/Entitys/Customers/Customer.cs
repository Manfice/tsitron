using System;
using System.Collections.Generic;
using tsitron.Domain.Entitys.Secure;

namespace tsitron.Domain.Entitys.Customers
{
    public class Customer
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string Location { get; set; }
        public int Ranck { get; set; }
        public bool Seller { get; set; }
        public DateTime Register { get; set; }
        public virtual User User { get; set; }
        public virtual Requisites Requisites { get; set; }
        public virtual MyShop MyShop { get; set; }
    }

    public class Requisites
    {
        public int Id { get; set; }
        public string Title { get; set; } // Наименование юр лица
        public string UrName { get; set; }
        public string Inn { get; set; }
        public string Kpp { get; set; }
        public string Address { get; set; }
        public string Bank { get; set; }
        public string Account { get; set; }
        public string KorrAccount { get; set; }
        public string Bik { get; set; }
    }

    public class MyShop
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Descr { get; set; }
        public string Site { get; set; }
        public bool FreePfoto { get; set; }//бесплатное фото с букетом
        public bool GreetingCard { get; set; }//Отправляет открытку в с букетом  
        public virtual ImageForCust Logotip { get; set; }//Логотип магазина
        public virtual ImageForCust ShopPhoto { get; set; }//Фото магазина
        public virtual ImageForCust Florist { get; set; }//Фото флориста
        public virtual Contacts Contacts { get; set; }
        public virtual ICollection<ShopGraphic> WorkDayses { get; set; }
        public int StartWork { get; set; }//
        public int EndWork { get; set; }//
        public bool AroundTheClock { get; set; } // Круглосуточно
    }

    public class Contacts
    {
        public int Id { get; set; }
        public string Town { get; set; }
        public string Adres { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Email { get; set; }
        public string ZapasnoiEmail { get; set; }
        public string TimeToMakeAverage { get; set; }//Среднее время создания букета
        public decimal DeliverInTown { get; set; }
        public decimal DeliverOutTown { get; set; }//руб. за километр
    }

    public class ShopGraphic
    {
        public int Id { get; set; }
        public string WeekDayWork { get; set; }
        public bool WeekEnd { get; set; }
        public int StartWork { get; set; }
        public int EndWork { get; set; }
        public bool AroundTheClock { get; set; }
        public virtual MyShop MyShop { get; set; }
    }

    public class WorkDays
    {
        public int Id { get; set; }
        public string WeekDayWork { get; set; }
        public bool WeekEnd { get; set; }
        public int StartWork { get; set; }
        public int EndWork { get; set; }
        public bool AroundTheClock { get; set; }
        public virtual MyShop MyShop { get; set; }
    }

    public class MyHolydays
    {
        public int Id { get; set; }
        public DateTime DayOff { get; set; }
        public DateTime DayOffEnd { get; set; }
        public int DayOffCount { get; set; }
        public virtual MyShop MyShop { get; set; }
    }

    public class ImageForCust
    {
        public int Id { get; set; }
        public string Descr { get; set; }
        public PhotoDescr PhotoDescr { get; set; }
        public int ImageSize { get; set; }
        public string FileName { get; set; }
        public byte[] ImageData { get; set; }
        public string ContentType { get; set; }
    }

    public enum PhotoDescr
    {
        ShopLogo,
        ShopPhoto,
        FloristPhoto
    }
}