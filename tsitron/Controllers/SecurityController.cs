using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using tsitron.Domain.Abstract;
using tsitron.Domain.Models;

namespace tsitron.Controllers
{
    public class SecurityController : Controller
    {
        private readonly IUserRepository _repository;

        public SecurityController(IUserRepository repository)
        {
            _repository = repository;
        }

        public ViewResult LoginPage()
        {
            return View(new LoginViewModel());
        }
        public PartialViewResult Login()
        {
            return PartialView(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel login)
        {
            if (!ModelState.IsValid) return View("LoginPage", login);
            var log = await _repository.LoginAsync(login);

            if (log.Code<0)
            {
                TempData["secureMessage"]=log.Message;
                return RedirectToAction("LoginPage", "Security");
            }

            FormsAuthentication.SetAuthCookie(log.Code.ToString(), false);// В куки передается код пользователя.
            TempData["secureMessage"] = log.Message;

            var usr = await _repository.GetUserAsync(log.Code);

            if (usr.UserRole.Title=="S")
            {
                return RedirectToAction("Index", "Seller");
            }
            if (usr.UserRole.Title == "A" || usr.UserRole.Title == "M" || usr.UserRole.Title == "O")
            {
                return RedirectToAction("MainPage", "Admin");
            }
            return RedirectToAction("Index", "Home");
        }


        public ActionResult Register()
        {
            return View(new RegisterViewModel {Seller = true});
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel reg)
        {
            if (!ModelState.IsValid) return View(reg);
            var log = await _repository.RegisterAsync(reg);

            if (log.Code <= 0)
            {
                TempData["secureMessage"] = log.Message;
                return View(reg);
            }

           // FormsAuthentication.SetAuthCookie(log.Code.ToString(),false);
            TempData["secureMessage"] = log.Message;

            return RedirectToAction("ThankYou", "Security");
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult ThankYou()
        {
            return View();
        }
    }
}