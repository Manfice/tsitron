using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using tsitron.Domain.Abstract;
using tsitron.Domain.Entitys.Customers;
using tsitron.Domain.Entitys.Goods;
using tsitron.Domain.Models;

namespace tsitron.Controllers
{
    public class WebSellerController : ApiController
    {
        private readonly ISellerRepository _repository;
        private readonly IWebSellerRepository _seller;

        public WebSellerController(ISellerRepository repository, IWebSellerRepository seller)
        {
            _repository = repository;
            _seller = seller;
        }

        [HttpGet]
        public IEnumerable<Customer> GetCustomers()
        {
            return _repository.GetCustomers;
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetBouquets(int id)
        {
            IEnumerable<ViewModelBouquet> result = null;
            var cust = new Customer();
            if (User.Identity.IsAuthenticated)
            {
                var usrId = int.Parse(User.Identity.Name);
                cust = _repository.GetCustomerByUser(usrId);
            }
            if (cust.Id==id)
            {
                result = await _seller.GetBouquetsByCustomer(id);
            }
            return result == null
            ? (IHttpActionResult)BadRequest("Not auth request.") : Ok(result);
        }


        public async Task<IHttpActionResult> GetCustomer(int id)
        {
            var result = await _repository.GetCustomerAsync(id);

            return result == null
            ? (IHttpActionResult)BadRequest("No customer found") : Ok(result);
        }

        public async Task<IHttpActionResult> GetBouquetData(int id)// id bouq
        {
            var result = await _seller.GetBouqDataAsync(id);
            return result == null
            ? (IHttpActionResult)BadRequest("No customer found") : Ok(result);
        }

        [HttpPost]
        public async Task<IHttpActionResult> Publish(PublishVm model)
        {
            await _seller.PublicBouquetAsync(model);
            var result = await _seller.GetBouqDataAsync(model.Id);

            return result == null
            ? (IHttpActionResult)BadRequest() : Ok(result);
        }

        [HttpPost]
        public async Task<IHttpActionResult> SaveImage()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return BadRequest("Fuck");
            }
            var provider = new MultipartMemoryStreamProvider();

            var root = HttpContext.Current.Server.MapPath("~/PhotoUpload/");

            await Request.Content.ReadAsMultipartAsync(provider);

            var fileName = string.Empty;

            foreach (var file in provider.Contents)
            {
                fileName = file.Headers.ContentDisposition.FileName.Trim('\"');
                var fileArray = await file.ReadAsByteArrayAsync();

                using (var fs = new FileStream(root + fileName, FileMode.Create))
                {
                    await fs.WriteAsync(fileArray, 0, fileArray.Length);
                }
            }
                return Ok(root + fileName);
        }

        [HttpPost]
        public async Task<IHttpActionResult> SaveCustomerData(Customer customer)
        {
            var result = await _repository.SaveCustomer(customer);
            return result == null ? (IHttpActionResult) BadRequest("No customer found") : Ok(result);
        }

        [HttpPost]
        public async Task<IHttpActionResult> SaveRequisites(Requisites requisites)
        {
            var result = await _seller.UpdateRequisites(requisites);
            return result == null ? (IHttpActionResult) BadRequest("No req found") : Ok();
        }

        [HttpPost]
        public async Task<IHttpActionResult> SaveShopData(MyShop shop)
        {
            var result = await _seller.UpdateShopAsync(shop);
            return result == null ? (IHttpActionResult)BadRequest("No shops found") : Ok();
        }

        [HttpPost]
        public async Task<IHttpActionResult> SaveBouquetData(ViewModelBouquet model)
        {
            var result = await _seller.SaveBouqDataAsync(model);
            return result == null ? (IHttpActionResult)BadRequest() : Ok(result);
            //return Ok();
        }

        [HttpPost]
        public async Task<IHttpActionResult> DeleteBouquet(int id)
        {
            var photos = await _seller.DeletePhotosTask(id);
            foreach (var item in photos)
            {
                File.Delete(item.FullPath);
            }
            var result = await _seller.DeleteBouquet(id);
            return Ok(result);
        } 

    }
}
