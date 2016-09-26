using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using tsitron.Domain.Abstract;
using tsitron.Domain.Entitys.Customers;
using tsitron.Domain.Entitys.Goods;
using tsitron.Domain.Entitys.Secure;
using tsitron.Domain.Models;

namespace tsitron.Controllers
{
    public class WebAdminController : ApiController
    {
        private readonly IAdminRepository _repository;

        public WebAdminController(IAdminRepository repository)
        {
            _repository = repository;
        }

        public async Task<IHttpActionResult> GetColors()
        {
            var col = await _repository.GetColorsAsync();
            IEnumerable<ColorViewModel> result = col.Select(item => new ColorViewModel
            {
                Id = item.Id,
                Title = item.StrValue,
                HexColor = item.HexValue,
                Rainbow = item.HexValue == "#000000"
            }).ToList();

            return col == null
            ? (IHttpActionResult)BadRequest("No colors") : Ok(result);
        }

        [HttpPost]
        public async Task<IHttpActionResult> SaveColor(SaveColor color)
        {
            var col = new GoodsColor
            {
                StrValue = color.Title,
                HexValue = color.SelValue
            };
            var result = await _repository.SaveColor(col);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IHttpActionResult> DeleteColor(ColorViewModel color)
        {
            var result = await _repository.DeleteColor(color.Id);
            return Ok(result);
        }

        public async Task<IHttpActionResult> GetFlowers()
        {
            var result = await _repository.FlowerTypesAsync();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IHttpActionResult> SaveFlower(FlowerType flower)
        {
            var result = new ValidEvents();
            if (string.IsNullOrEmpty(flower.Title))
            {
                result.Message = "Поле с наименованием не может быть пустым.";
            }
            result = await _repository.SaveFlowerAsync(flower.Title);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IHttpActionResult> DeleteFlower(FlowerType ft)
        {
            var result = await _repository.DeleteFlowerTypeAsync(ft.Id);
            return Ok(result);
        }
        public async Task<IHttpActionResult> GetEvents()
        {
            var result = await _repository.EventTypesAsync();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IHttpActionResult> SaveEvent(EventType et)
        {
            var result = new ValidEvents();
            if (string.IsNullOrEmpty(et.Title))
            {
                result.Message = "Поле с наименованием не может быть пустым.";
            }
            result = await _repository.AddEventAsync(et);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IHttpActionResult> DeleteEvent(EventType eventType)
        {
            var result = await _repository.DeleteEventAsync(eventType.Id);
            return Ok(result);
        }

        public async Task<IHttpActionResult> GetUsers()
        {
            return Ok(await _repository.UsersViewAsync());
        }
        [HttpPost]
        public async Task<IHttpActionResult> BlockCustomer(Customer customer)
        {
            return Ok(await _repository.BlockUserAsync(customer.User.Id));
        }

        public async Task<IHttpActionResult> BlockUser(User user)
        {
            return Ok(await _repository.BlockUserAsync(user.Id));
        }

        public async Task<IHttpActionResult> GetBouquets()
        {
            var result = await _repository.GetBouquetsAsync();
            return Ok(result);
        }

        public async Task<IHttpActionResult> GetBouquet(int id)
        {
            return Ok(await _repository.GetBouquetAsync(id));
        }

        public async Task<IHttpActionResult> GetPhotoView(int id)
        {
            return Ok(await _repository.GetAvatarAsync(id));
        }
        [HttpPost]
        public async Task<IHttpActionResult> Moderate(ModerateModel model)
        {
            return Ok(await _repository.ModerateAsync(model));
        } 
    }
}
