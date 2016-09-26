using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using tsitron.App_Data.SMS;
using tsitron.Domain.Abstract;
using tsitron.Domain.Context;
using tsitron.Domain.Entitys.Customers;
using tsitron.Domain.Entitys.Goods;
using tsitron.Domain.Entitys.Orders;
using tsitron.Domain.Models;
using Twilio;
using Color = System.Drawing.Color;

namespace tsitron.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly IHomeRepository _repository;

        public HomeController(IHomeRepository repository)
        {
            _repository = repository;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details(int id)
        {
            var bvm = new DetailsBouquetVm
            {
                Bouquet = _repository.GetBouquet(id),
                AskDate = DateTime.Now.ToShortDateString()

            };
            var ad = new List<SelDate>();
            var n = DateTime.Now.Date;
            for (var i = 0; i < 3; i++)
            {
                var sd = new SelDate
                    {
                        AvDate = n
                    };
                    ad.Add(sd);
                
               n = n.AddDays(1);
            }
            bvm.AvaluebleDates = ad;
            return View(bvm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MakeOrder(Order order, DetailsBouquetVm model)
        {
            var d = DateTime.Parse(model.AskDate);

            Customer cust = null;
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("C"))
                {
                    cust = _repository.GetCustomer(int.Parse(User.Identity.Name));
                }
            }
            order.Bouquet = _repository.GetBouquet(model.Bouquet.Id);
            order.Customer = cust ?? new Customer();
            order.OrderDate = DateTime.Now;
            order.Recipient = new Recipient
            {
                Date = d.ToShortDateString()
            };
            return View();
        }

        public JsonResult GetOrder(Order order)
        {
            var jOrder = new PreOrder
            {
                WishDate = order.Recipient.Date,
                Marker = order.Bouquet.Id,
                Avatar = _repository.GetAvatar(order.Bouquet.Id),
                Bouquet = new BouqView
                {
                    Title = order.Bouquet.Title,
                    Quantity = 1,
                    Price = order.Bouquet.Price.PriceValue
                }
        };
            
            return Json(jOrder, JsonRequestBehavior.AllowGet);
        }

    }
}