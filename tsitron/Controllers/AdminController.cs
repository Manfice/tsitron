using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using tsitron.Domain.Abstract;
using tsitron.Domain.Context;
using tsitron.Domain.Entitys.Goods;
using tsitron.Domain.Entitys.Secure;
using tsitron.Domain.Models;

namespace tsitron.Controllers
{
    [Authorize(Roles = "A, M, O")]
    public class AdminController : Controller
    {
        private readonly IAdminRepository _repository;
        private User _user;

        public AdminController(IAdminRepository repository)
        {
            _repository = repository;
        }


        protected override void Initialize(System.Web.Routing.RequestContext context)
        {
            base.Initialize(context);
            if (!context.HttpContext.User.Identity.IsAuthenticated) return;
            var userId = int.Parse(context.HttpContext.User.Identity.Name);
            _user = _repository.GetUser(userId);
        }

        // GET: Admin
        public ActionResult MainPage()
        {
            return View();
        }

        public PartialViewResult TitleAdmin()
        {
            return PartialView(_user);
        }

        #region Left navigation

        public PartialViewResult Navigation()
        {
            return PartialView(_user);
        }

        #endregion
        #region Users

        public async Task<ActionResult> UserList()
        {
            
            return View(await _repository.UsersAsync());
        }

        public ActionResult CreateUser(string returnUrl)
        {
            var usr = new UserViewModel();
            var ur = new SelectList(_repository.GetRoles(),"Id","Descr");
            ViewBag.Roles = ur;
            ViewBag.ReturnUrl = returnUrl;
            return View(usr);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateUser(UserViewModel user)
        {
            var result = await _repository.AddUserAsync(user);
            if (result.Code > 0)
            {
                TempData["sAddUser"] = result.Message;
                return RedirectToAction("UserList");

            }
            TempData["eAddUser"] = result.Message;
            var ur = new SelectList(_repository.GetRoles(), "Id", "Descr");
            ViewBag.Roles = ur;
            return View(user);
        } 

        #endregion
        #region directory

        public async Task<ActionResult> Directorys()
        {
           return View(await _repository.FlowerTypesAsync());
        }

        public PartialViewResult AddFlowerType()
        {
            return PartialView(new FlowerType());
        }

        public PartialViewResult ColorsList()
        {
            var col = _repository.GetColors();
            IEnumerable<ColorViewModel> colmodel = col.Select(item => new ColorViewModel
            {
                Id = item.Id, Title = item.StrValue, HexColor = System.Drawing.Color.FromArgb(item.Value).ToString(), Rainbow = item.Value==0
            }).ToList();

            return PartialView(colmodel);
        }

        public PartialViewResult AddColor()
        {

            return PartialView(new GoodsColor());
        }

        public PartialViewResult AddEvent()
        {
            return PartialView(new EventType());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddEventTypeAsync(EventType eventType)
        {
            var result = await _repository.AddEventAsync(eventType);
            if (result.Code > 0)
            {
                TempData["sAddEvent"] = result.Message;
                return RedirectToAction("Directorys");

            }
            TempData["eAddEvent"] = result.Message;
            return RedirectToAction("Directorys");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteEventTypeAsync(int id)
        {
            var result = await _repository.DeleteEventAsync(id);
            if (result.Code > 0)
            {
                TempData["sDeleteEvent"] = result.Message;
                return RedirectToAction("Directorys");

            }
            TempData["eDeleteEvent"] = result.Message;
            return RedirectToAction("Directorys");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddColorAsync(int color, string title)
        {
            var col = new GoodsColor {Value = color, StrValue = title};
            var result = await _repository.SaveColor(col);
            if (result.Code > 0)
            {
                TempData["sAddColor"] = result.Message;
                return RedirectToAction("Directorys");

            }
            TempData["eAddColor"] = result.Message;
            return RedirectToAction("Directorys");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteColorAsync(int id)
        {
            var result = await _repository.DeleteColor(id);
            if (result.Code > 0)
            {
                TempData["sDeleteColor"] = result.Message;
                return RedirectToAction("Directorys");

            }
            TempData["eDeleteColor"] = result.Message;
            return RedirectToAction("Directorys");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddFlowerTypeAsync(FlowerType model)
        {
            var result = await _repository.AddFlowerTypeAsync(model);
            if (result.Code > 0)
            {
                TempData["sAddFlowerType"] = result.Message;
                return RedirectToAction("Directorys");

            }
            TempData["eAddFlowerType"] = result.Message;
            return View("Directorys");

        }
        [Authorize(Roles = "A")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteFlowerType(int id)
        {
            var result = await _repository.DeleteFlowerTypeAsync(id);
            if (result.Code > 0)
            {
                TempData["sDeleteFlowerType"] = result.Message;
                return RedirectToAction("Directorys");

            }
            TempData["eDeleteFlowerType"] = result.Message;
            switch (result.Code)
            {
                case -1: return View("Directorys");
                case -2:
                    return RedirectToAction("ConflictFlowers", new {ftId=id});
            }
            return View("Directorys");
        }

        public ActionResult ConflictFlowers(int ftId)
        {
            return View(_repository.GetFlowers());
        }

        public ActionResult FlowerDetails(int id, string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View(_repository.Flower(id));
        }
        #endregion
        #region Products

        public ActionResult Products()
        {
            return View();
        }
        #endregion
        #region Orders

        public async Task<ActionResult> Orders()
        {

            return View(await _repository.OrdersTask());
        }
#endregion
    }
}