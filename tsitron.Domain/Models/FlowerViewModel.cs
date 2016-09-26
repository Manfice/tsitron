using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using tsitron.Domain.Entitys.Goods;

namespace tsitron.Domain.Models
{
    public class FlowerViewModel
    {
        public int CustomerId { get; set; }
        public string Articul { get; set; }
        public int FlowerId { get; set; }

        [Required(ErrorMessage = "Выберите фото цветка.")]
        public HttpPostedFileBase Image { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public FlowerType FlowerType { get; set; }
        public IEnumerable<Price> Prices { get; set; }
    }
}