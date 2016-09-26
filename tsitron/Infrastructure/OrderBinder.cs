using System;
using System.Web.Mvc;
using tsitron.Domain.Entitys.Orders;

namespace tsitron.Infrastructure
{
    public class OrderBinder : IModelBinder
    {
        private const string SessionKey = "Order";
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            Order order = null;

            if (controllerContext.HttpContext.Session != null)
            {
                order = (Order) controllerContext.HttpContext.Session[SessionKey];
            }
            if (order==null)
            {
                order = new Order
                {
                    OrderDate = DateTime.Now
                };
                if (controllerContext.HttpContext.Session!=null)
                {
                    controllerContext.HttpContext.Session[SessionKey] = order;
                }
            }

            return order;
        }
    }
}