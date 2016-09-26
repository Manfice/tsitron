using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using tsitron.Domain.Abstract;
using tsitron.Domain.Context;
using tsitron.Domain.Entitys.Goods;
using tsitron.Domain.Models;

namespace tsitron.Controllers
{
    [AllowAnonymous]
    public class GoodsController : Controller
    {
        private readonly IGoodsRepository _repository;

        public GoodsController(IGoodsRepository repository)
        {
            _repository = repository;
        }

        // GET: Goods
        public ActionResult Index(int id)
        {
            var cust = ContextFabric.Context.Customers.FirstOrDefault(c => c.Id == id);
            return View(cust);
        }

        //public FileContentResult Avatar(int flId)
        //{
        //    var image = _repository.GetImage(flId);
        //    return image != null ? File(image.ImageData, image.ContentType) : null;
        //}

        public PartialViewResult TopBouqets()
        {
            var bouq = _repository.GetBouquets;
            var bvm = new List<BouquetViewModel>();

            //foreach (var bouquet in bouq)
            //{
            //    var bv = new BouquetViewModel
            //    {
            //        Title = bouquet.Title,
            //        Bouquet = bouquet
                    
            //    };
            //    foreach (var image in bouquet.Images.Where(image => image.ImageType==ArtGenerator.ImageType.Avatar))
            //    {
            //        bv.Image = image;
            //    }
            //    bvm.Add(bv);
            //}

            return PartialView(bvm);
        }
        //public FileContentResult BouqPhoto(int imgId)
        //{
        //    var img = _repository.GetBouqImage(imgId);

        //    return img == null ? null : File(img.ImageData, img.ContentType);
        //}
    }
}