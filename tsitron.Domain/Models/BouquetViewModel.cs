using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using tsitron.Domain.Entitys.Customers;
using tsitron.Domain.Entitys.Goods;
using Color = System.Drawing.Color;

namespace tsitron.Domain.Models
{
    public class BouquetViewModel
    {
        public Bouquet Bouquet { get; set; }
        public Customer Customer { get; set; }
        public EventType EventType { get; set; }
        [Required(ErrorMessage = "Укажите название букета.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Укажите цену.")]
        [DataType(DataType.Currency)]
        [Range(10,100000,ErrorMessage = "Укажите цену в диапазоне от 10р. до 100 000р.")]
        public decimal Price { get; set; }
        public Photo Image { get; set; }
        public int ColorId { get; set; }
        public IEnumerable<ColorViewModel> Colors { get; set; }  

        [Required(ErrorMessage = "Не выбран фаил с картинкой.")]
        public HttpPostedFileBase UploadImage { get; set; }
        public IEnumerable<Photo> Images { get; set; }
        public Details Details { get; set; } 
    }

    public class BvmForHome
    {
        public string Marker { get; set; }
        public string Monger { get; set; }
        public string Title { get; set; }
        public string Price { get; set; }
        public PhotoView Avatar { get; set; }
        public IEnumerable<Photo> Photos { get; set; }
    }

    public class DetailsBouquetVm
    {
        public string AskDate { get; set; }
        public Bouquet Bouquet { get; set; }
        public IEnumerable<SelDate> AvaluebleDates { get; set; }
    }

    public class SelDate
    {
        public DateTime AvDate { get; set; }
    }

    public class ReqData
    {
        public string Region { get; set; }
    }
    public class ViewModelBouquet
    {
        public int Id { get; set; }
        public string Art { get; set; } 
        public string Title { get; set; }
        public decimal Price { get; set; }
        public int TimeToMake { get; set; }
        public string Description { get; set; }
        public string Composition { get; set; }
        public ModeratedResult ModeratedResult { get; set; }
        public Details Details { get; set; }
        public BouqAttrebutes BouqAttrebutes { get; set; }
        public HttpPostedFileBase Image { get; set; }
        public Customer Customer { get; set; }
        public Photo Photo { get; set; }
        public PhotoView PhotoView { get; set; }    
        public IEnumerable<GoodsColor> Colors { get; set; }
        public IEnumerable<EventType> EventTypes { get; set; }
        public IEnumerable<Photo> Photos { get; set; }  
    }

    public class EditModelBouq
    {
        public Bouquet Bouquet { get; set; }
        public Details Details { get; set; }
        public Customer Customer { get; set; }
        public HttpPostedFileBase Image { get; set; }
        public PhotoView PhotoView { get; set; }
        public IEnumerable<GoodsColor> Colors { get; set; }
        public IEnumerable<EventType> EventTypes { get; set; }
    }

    public class PhotoView
    {
        public string Alt { get; set; }
        public string Path { get; set; }
    }

    public class PublishVm
    {
        public int Id { get; set; }
        public bool Publish { get; set; }
    }
}