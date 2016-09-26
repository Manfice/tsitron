using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using tsitron.Domain.Context;

namespace tsitron.Controllers
{
    
    public class CustomerController : Controller
    {
        // GET: Customer
        public ActionResult Index(int id)
        {
            var db = ContextFabric.Context;
            var cust = db.Customers.Find(id);
            return View(cust);
        }
    }
}