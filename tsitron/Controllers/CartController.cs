using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using tsitron.Domain.Abstract;
using tsitron.Domain.Entitys.Orders;
using tsitron.Domain.Models;

namespace tsitron.Controllers
{
    public class CartController : Controller
    {
        private ICartRepository _cartRepository;
        private IGoodsRepository _goodsRepository;

        public CartController(ICartRepository repository, IGoodsRepository goodsRepository)
        {
            _cartRepository = repository;
            _goodsRepository = goodsRepository;
        }

        public Cart GetCart()
        {
            var cart = (Cart) Session["Cart"];
            if (cart != null) return cart;
            cart=new Cart();
            Session["Cart"] = cart;
            return cart;
        }

        // GET: Cart
        public ActionResult CartIndex(string returnUrl)
        {
            return View(new CartViewModel {Cart = GetCart(), ReturnUrl = returnUrl});
        }

        public ActionResult OrderData(int id, string returnUrl)// Bouquet data
        {
            var bouq = _goodsRepository.GetBouquet(id);
            var cItem = new CartItem
            {
                Bouquet = bouq,
                Recipient = new Recipient(),
                ReturnUrl = returnUrl
            };
            @ViewBag.returnUrl = returnUrl;
            return View(cItem);
        }

        public RedirectToRouteResult AddToCart(int id, string returnUrl, Recipient recipient)
        {
            var bouq = _goodsRepository.GetBouquet(id);
            if (bouq!=null)
            {
                GetCart().AddItem(bouq, recipient);
            }
            return RedirectToAction("CartIndex", new {returnUrl});
        }
    }
}