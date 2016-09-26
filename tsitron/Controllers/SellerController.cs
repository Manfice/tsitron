using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using tsitron.Domain.Abstract;
using tsitron.Domain.Entitys.Customers;
using tsitron.Domain.Entitys.Goods;
using tsitron.Domain.Entitys.Orders;
using tsitron.Domain.Models;

namespace tsitron.Controllers
{
    [Authorize(Roles = "S, M, A")]
    public class SellerController : Controller
    {
        private readonly ISellerRepository _repository;
        private Customer _customer;
        private int _userId;

        public SellerController(ISellerRepository repository)
        {
            _repository = repository;
        }

        protected override void Initialize(RequestContext context)
        {
            base.Initialize(context);
            if (!context.HttpContext.User.Identity.IsAuthenticated) return;
            _userId = int.Parse(context.HttpContext.User.Identity.Name);
        }

        public ActionResult Index()
        {
            _customer = _repository.GetCustomerByUser(_userId);
            return View(_customer);
        }

        public PartialViewResult Navigation()
        {
            _customer = _repository.GetCustomerByUser(_userId);
            return PartialView(_customer);
        }


        #region Букет

        /*Букет и все, что с ним связано*/
        public PartialViewResult BouquetData(int bouqId)
        {
            return PartialView(_repository.Bouquet(bouqId));
        }

        public ActionResult BouquetIndex()
        {
            _customer = _repository.GetCustomerByUser(_userId);
            var vmb = new ViewModelBouquet
            {
                Customer = _customer
            };
            return View(vmb);
        }

        public PartialViewResult Bouqets(int id)
        {
            _customer = _repository.GetCustomerByUser(_userId);
            var bouquets = _repository.Bouquets(_customer.Id);
            return PartialView(bouquets);
        }

        public PartialViewResult TitleSeller()
        {
            _customer = _repository.GetCustomerByUser(_userId);
            return PartialView(_customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddBouquet(ViewModelBouquet model)
        {
            _customer = _repository.GetCustomerByUser(_userId);
            if (!ModelState.IsValid) return RedirectToAction("BouquetIndex");

            var flUp = new FileUploda {FileRatio = 1.5M, FileSize = 512*1024};
            var result = flUp.UploadFile(model.Image);
            if (result=="OK")
            {
                var path = Path.Combine(Server.MapPath("~/PhotoUpload"), model.Image.FileName);
                model.Customer = _customer;
                model.Photo = new Photo
                {
                    AltText = model.Title,
                    FileName = model.Image.FileName,
                    Flower = null,
                    FullPath = path,
                    ImageType = ArtGenerator.ImageType.Avatar,
                    Path = "/PhotoUpload/"+model.Image.FileName,
                    PropductType = "b"
                };
                var b = _repository.AddBouquet(model);
                if (b.Code>0)
                {
                    //Save photo
                    model.Image.SaveAs(path);
                    TempData["secureMessage"] = b.Message;
                    return RedirectToAction("BouquetIndex");
                }
            }
            TempData["secureMessage"] = result;
            return RedirectToAction("BouquetIndex");
        }
        //public async Task<ActionResult> EditBouquet(int id)
        //{
        //    var bouq = await _repository.GetBouquetDataAsync(id);

        //    return View(bouq);
        //}

        //public async Task<JsonResult> GetBouquetJson(int id)
        //{
        //    var b = await _repository.GetBouquetDataAsync(id);
        //    return Json(b, JsonRequestBehavior.AllowGet);
        //}

        //Добавить картинку к букету
        public PartialViewResult UploadImage(int bouqId)
        {
            var b = _repository.Bouquet(bouqId);
            var bvm = new ImageViewModel
            {
                Customer = _customer.Id,
                Bouquet = b.Id,
            };
            return PartialView(bvm);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult UploadImage(EditModelBouq model)
        //{
        //    if (!ModelState.IsValid) return RedirectToAction("EditBouquet", new { id = model.Bouquet.Id });

        //    var flUp = new FileUploda { FileRatio = 1.5M, FileSize = 512 * 1024 };
        //    var result = flUp.UploadFile(model.Image);
        //    if (result == "OK")
        //    {
        //        var path = Path.Combine(Server.MapPath("~/PhotoUpload"), model.Image.FileName);
        //        var bouq = _repository.Bouquet(model.Bouquet.Id);
        //        var photo = new Photo
        //        {
        //            AltText = bouq.Title,
        //            FileName = model.Image.FileName,
        //            Flower = null,
        //            FullPath = path,
        //            ImageType = ArtGenerator.ImageType.Photo,
        //            Path = "/PhotoUpload/" + model.Image.FileName,
        //            PropductType = "b",
        //            Bouquet = bouq
        //        };
        //        var b = _repository.AddPhotoToBouquet(photo);
        //        if (b.Code > 0)
        //        {
        //            //Save photo
        //            model.Image.SaveAs(path);
        //            TempData["secureMessage"] = b.Message;
        //            return RedirectToAction("EditBouquet", "Seller", new { id = model.Bouquet.Id });
        //        }

        //    }
        //    return RedirectToAction("EditBouquet", new { id = model.Bouquet.Id });
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteFoto(int imgId)
        //{
        //    var result = _repository.DeleteImage(imgId);
        //    return result.Code > 0 ? RedirectToAction("EditBouquet", new { id = result.Code }) : RedirectToAction("Index");
        //}

        //Конец букета
        //----------------------
        #endregion

        #region Цветы поштучно
        /*Цветы поштучно*/
        public ActionResult Flowers()
        {
            return View();
        }

        public PartialViewResult FlowersList()
        {
            _customer = _repository.GetCustomerByUser(_userId);
            var flowers = _repository.Flowers(_customer.Id);
            var fvm = new List<FlowerViewModel>();
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var item in flowers)
            {
                var prices = _repository.FlowerPrices;
                    
                var f = new FlowerViewModel
                {
                    Title = item.Title,
                    Prices = prices.ToList(),
                    FlowerId = item.Id,
                    Description = item.Descr,
                    Articul = item.Articul
                };
                fvm.Add(f);
            }
            return PartialView(fvm);
        }

        public PartialViewResult AddFlower()
        {
            _customer = _repository.GetCustomerByUser(_userId);
            var fwt = _repository.FlowerTypes; 
            var fvm = new FlowerViewModel
            {
                CustomerId = _customer.Id,
                Prices = new List<Price>
                {
                    new Price {Id = 0, Title = "до 10 шт."},
                    new Price {Id = 1, Title = "от 10 шт."},
                    new Price {Id = 2, Title = "от 20 шт."},
                    new Price {Id = 3, Title = "от 30 и более шт."}
                }
            };
            ViewBag.ft = new SelectList(fwt, "Id", "Title");
            return PartialView(fvm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddFlower(FlowerViewModel model)
        {
            var result = _repository.AddFlower(model);
            if (result.Code>0)
            {
                TempData["sFl"] = result.Message;
                return RedirectToAction("Flowers");
            }
                TempData["eFl"] = result.Message;
                return RedirectToAction("Flowers");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteFlower(int id)
        {
            var result = _repository.DeleteFlower(id);
            if (result.Code > 0)
            {
                TempData["sFl"] = result.Message;
                return RedirectToAction("Flowers");
            }
            TempData["eFl"] = result.Message;
            return RedirectToAction("Flowers");
        }
        //Конец тветы поштучно
        //------------------------------
        #endregion

        #region Детали букета
        //Детали букета, начало
        public PartialViewResult Details(int bouqId)
        {
            return PartialView(_repository.GetBouquetDetails(bouqId));
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult UpdateDetails(EditModelBouq model)
        //{

        //    var result = _repository.UpdateBouquetDetails(model.Details);

        //    if (result.Code > 0)
        //    {
        //        TempData["successDet"] = result.Message;
        //        return RedirectToAction("EditBouquet", new { id = model.Bouquet.Id });
        //    }
        //    TempData["errorDet"] = result.Message;
        //    return RedirectToAction("EditBouquet", new { id = model.Bouquet.Id });
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult UpdateBouquetData(Bouquet b)
        //{
        //    var result = _repository.UpdateBouquet(b);
        //    if (result.Code > 0)
        //    {
        //        TempData["success"] = result.Message;
        //        return RedirectToAction("EditBouquet", new { id = result.Code });
        //    }
        //    TempData["error"] = result.Message;
        //    return RedirectToAction("EditBouquet", new { id = result.Code });
        //}
        //Конец детализации букета
        //-----------------------------------
        #endregion

        #region Композиция букета
        //Композиция букета (состав) - начало
        //public PartialViewResult FlowerComposition(int detId)
        //{
        //    var flowers = _repository.FlowerTypes.OrderBy(f=>f.Title).Distinct().ToList();
        //    ViewBag.fl = new SelectList(flowers,"Id","Title");
        //    var fc = new FCompositionViewModel
        //    {
        //        FlowerCompositions = _repository.FlowerCompositions(detId),
        //        FlowerComposition = new FlowerComposition
        //        {
        //            Details = _repository.GetBouquetDetails(detId)
        //        }
        //    };
        //    return PartialView(fc);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult AddToComposition(FCompositionViewModel model)
        //{
        //    var det = _repository.GetBouquetDetails(model.FlowerComposition.Details.Id);
        //    var b = det.Bouquet.Id;
        //    _repository.AddToCompose(model);
        //    return RedirectToAction("EditBouquet", new {id=b});
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteFromConpose(int id)
        //{
        //    var fc = _repository.GetFLowerCompose(id);
        //    var b = fc.Details.Bouquet.Id;
        //    _repository.DeleteFComp(fc.Id);
        //    return RedirectToAction("EditBouquet", new {id=b});

        //}
        #endregion

        #region Аксессуары
        //Get list
        public ActionResult Accessories()//Возвращает страницу со списком аксессуаров для данного продавца
        {
            _customer = _repository.GetCustomerByUser(_userId);
            return View(_repository.GetAccessories(_customer.Id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveAccessories(Accessories model, HttpPostedFileBase photo)
        {
            if (photo==null)
            {
                model.Customer = _repository.GetCustomerByUser(_userId);
                model.Image = new Photo
                {
                    AltText = "Нет фотографии",
                    FileName = "",
                    Flower = null,
                    FullPath = "",
                    ImageType = ArtGenerator.ImageType.Avatar,
                    Path = "/Content/img/no_image.jpg",
                    PropductType = "a"
                };
                _repository.SaveAccessory(model);
                return RedirectToAction("Accessories");
            }
            var path = Path.Combine(Server.MapPath("~/PhotoUpload"), photo.FileName);
            photo.SaveAs(path);
            model.Customer = _repository.GetCustomerByUser(_userId);
            model.Image = new Photo
                {
                    AltText = model.Title,
                    FileName = photo.FileName,
                    Flower = null,
                    FullPath = path,
                    ImageType = ArtGenerator.ImageType.Avatar,
                    Path = "/PhotoUpload/" + photo.FileName,
                    PropductType = "a"
                };
            _repository.SaveAccessory(model);
            
            return RedirectToAction("Accessories");
        }

        public ActionResult RomoveAccessory(int id)
        {
            _repository.RemoveAccessory(id);
            return RedirectToAction("Accessories");
        }
        #endregion
        #region MyShop
        //get
        public PartialViewResult MyShop()
        {
            _customer = _repository.GetCustomerByUser(_userId);
            var shopVm = new ShopVm
            {
                Shop = _repository.GetShopData(_customer.Id),
                WeekEnds = _repository.GetShopData(_customer.Id).WorkDayses.ToList()
            };
            return PartialView(shopVm);
        }
        public PartialViewResult Contacts()
        {
            _customer = _repository.GetCustomerByUser(_userId);
            var cont = _repository.GetCustContacts(_customer.Id);
            return PartialView(cont);
        }

        public PartialViewResult Requisites()
        {
            _customer = _repository.GetCustomerByUser(_userId);
            var req = _repository.GetRequisites(_customer.Id);
            return PartialView(req);
        }
        //post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveShopData(ShopVm model, HttpPostedFileBase shopLogo, HttpPostedFileBase shopPhoto, HttpPostedFileBase florist)
        {
            _repository.SaveShopData(model);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveContacts(Contacts contacts)
        {
            _repository.SaveContacts(contacts);
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveRequisits(Requisites requisites)
        {
            _repository.SaveRequisites(requisites);
            return RedirectToAction("Index");
        }

        public PartialViewResult WeekEnd(int id)
        {
            ViewBag.Id = id;
            var hod = _repository.Holydayses(id);
            return PartialView(hod);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddWeekEnd(int id, MyHolydays holydays)
        {
            _repository.AddHilyday(id,holydays);
            return RedirectToAction("Index");
        }

        public ActionResult RemoveHolyday(int id)
        {
            _repository.RemoveHolyday(id);
            return RedirectToAction("Index");
        }
        #endregion
        #region Orders

        public ActionResult MyOrders(string status=null)
        {
            _customer = _repository.GetCustomerByUser(_userId);
            ViewBag.Total = _repository.GetOrdersByCustomer(_customer.Id).Count();
            List<Order> orders;
            if (status == null)
            {
                orders = _repository.GetOrdersByCustomer(_customer.Id).ToList();
            }
            else
            {
                orders =
                    _repository.GetOrdersByCustomer(_customer.Id)
                        .Where(order => order.Status == (OrderStatus)Enum.Parse(typeof (OrderStatus), status))
                        .ToList();
            }
            foreach (var order in orders)
            {
                DateTime respDate;
                DateTime.TryParse(order.Recipient.Date, out respDate);
                if (DateTime.Today>respDate)
                {
                    if (order.Status != OrderStatus.Исполнен & order.Status != OrderStatus.Отказ)
                    {
                        order.Status=OrderStatus.Просрочен;
                    }
                }
            }
            return View(orders.OrderByDescending(order => order.OrderDate));
        }

        public ActionResult ChangeStatus(int id, OrderStatus status)
        {
            _repository.ChangeStatus(id,status);
            return RedirectToAction("MyOrders");
        }

        public ActionResult OrdserDetails(int id)
        {

            return View(_repository.GetOrder(id));
        }
#endregion
    }
}