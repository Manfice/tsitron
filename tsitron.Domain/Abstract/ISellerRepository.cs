using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using tsitron.Domain.Entitys.Customers;
using tsitron.Domain.Entitys.Goods;
using tsitron.Domain.Entitys.Orders;
using tsitron.Domain.Entitys.Secure;
using tsitron.Domain.Models;

namespace tsitron.Domain.Abstract
{
    public interface ISellerRepository
    {
        //WEB API Controller

        IEnumerable<Customer> GetCustomers { get; }
        Task<Customer> GetCustomerAsync(int id);
        Task<Customer> SaveCustomer(Customer customer);


            //MVC Controller
        IEnumerable<Flower> Flowers(int id);
        ValidEvents AddFlower(FlowerViewModel model);
        IEnumerable<FlowerType> FlowerTypes { get; }
        IEnumerable<Price> FlowerPrices { get; } 
        IEnumerable<Customer> Sellers { get; }
        Customer Customer(int id);
        Customer GetCustomerByUser(int id);
        IEnumerable<Bouquet> Bouquets(int id);
        Bouquet Bouquet(int id);
        ValidEvents DeleteImage(int image);
        ValidEvents DeleteFlower(int id);
        ValidEvents UpdateBouquet(Bouquet bouquet);
        ValidEvents AddBouquet(ViewModelBouquet model);
        IEnumerable<ColorViewModel> GetColors();
        ValidEvents SetAvatar(int bouqetId);
        Details GetBouquetDetails(int id);
        ValidEvents UpdateBouquetDetails(Details details);
        IEnumerable<EventType> GetEventTypes();
        ValidEvents SetPrice(int pId,decimal price);
        MyShop GetShopData(int id);
        ViewModelBouquet GetBouquetData(int id);
        ValidEvents AddPhotoToBouquet(Photo photo);
        IEnumerable<Order> GetOrdersByCustomer(int id);
        void ChangeStatus(int id, OrderStatus status);
        Order GetOrder(int id);
        ValidEvents SaveShopData(ShopVm model);
        Contacts GetCustContacts(int id);// Customer id
        ValidEvents SaveContacts(Contacts contacts);
        Requisites GetRequisites(int id);//Customer id
        void SaveRequisites(Requisites requisites);
        List<ShopGraphic> GetWorkDayses();
        List<MyHolydays> Holydayses(int id);
        void AddHilyday(int id, MyHolydays holyDate);
        void RemoveHolyday(int id);
        IEnumerable<Accessories> GetAccessories(int id);//seller id
        void SaveAccessory(Accessories model);
        void RemoveAccessory(int id);
    }
}