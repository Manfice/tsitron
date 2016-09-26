using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using tsitron.Domain.Abstract;
using tsitron.Domain.Entitys.Orders;
using tsitron.Domain.Models;

namespace tsitron.Controllers
{
    public class WHomeController : ApiController
    {
        private readonly IHomeRepository _repository;

        public WHomeController(IHomeRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public async Task<IHttpActionResult> GetBouquets(ReqData region)
        {
            return Ok(await _repository.GetBouquets(region.Region));
        }

        [HttpPost]
        public async Task<IHttpActionResult> MakeOrder(OrderAccept order)
        {
            return Ok(await _repository.CreateOrder(order));
        } 
    }
}
