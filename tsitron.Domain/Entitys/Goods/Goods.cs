using System.Collections.Generic;
using tsitron.Domain.Entitys.Customers;

namespace tsitron.Domain.Entitys.Goods
{
    public class Bouquet
    {
        public int Id { get; set; }
        public string Articul { get; set; }
        public string Title { get; set; }
        public string FlowerComposition { get; set; }
        public string Description { get; set; }
        public int TimeToMake { get; set; }
        public virtual Price Price { get; set; }
        public virtual Details Details { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ModeratedResult ModeratedResult { get; set; }
        public virtual BouqAttrebutes BouqAttrebutes { get; set; }
        public virtual ICollection<FilteredEvents> EventTypes { get; set; }
        public virtual ICollection<FilterColor> Colors { get; set; }
        public virtual ICollection<Photo> Photos { get; set; } 
    }

    public class BouqAttrebutes
    {
        public int Id { get; set; }
        public bool Published { get; set; }
        public bool Moderated { get; set; }
        public bool Accepted { get; set; }
        public bool SpecialPrice { get; set; }
        public bool NewInLine { get; set; }
    }
    public class ModeratedResult
    {
        public int Id { get; set; }
        public string ResultAnsw { get; set; }
    }

    public class Details
    {
        public int Id { get; set; }
        public decimal Height { get; set; }
        public decimal Width { get; set; }
    }

    public class Flower
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Descr { get; set; }
        public string Articul { get; set; }
        public decimal Price { get; set; }
        public bool Published { get; set; }
        public bool Moderated { get; set; }
        public virtual FlowerType FlowerType { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }
        public virtual ICollection<FilteredEvents> EventTypes { get; set; }  
        public ICollection<Price> Prices { get; set; }
    }

    public class FlowerType
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Color { get; set; }
    }

    public class Accessories
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public bool Published { get; set; }
        public bool Moderated { get; set; }
        public string Comment { get; set; }
        public virtual Photo Image { get; set; }
        public virtual Customer Customer { get; set; }
    }

    public class GoodsColor
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public string StrValue { get; set; }
        public string HexValue { get; set; }
        public bool IsSelected { get; set; }    
    }

    public class FilterColor
    {
        public int Id { get; set; }
        public string GoodsType { get; set; }
        public virtual Bouquet Bouquet { get; set; }
        public virtual GoodsColor Color { get; set; }
    }

    public class FilteredEvents
    {
        public int Id { get; set; }
        public string GoodsType { get; set; }
        public virtual Bouquet Bouquet { get; set; }
        public virtual EventType EventType { get; set; }    
    }

    public class EventType
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsSelected { get; set; }    
    }

    public class Photo
    {
        public int PhotoId { get; set; }
        public string PropductType { get; set; }
        public string Path { get; set; }
        public string FullPath { get; set; }
        public string FileName { get; set; }
        public string AltText { get; set; }
        public ArtGenerator.ImageType ImageType { get; set; }
        public virtual Bouquet Bouquet { get; set; }
        public virtual Flower Flower { get; set; }  
    }

    public class ImagePicture
    {
        public int Id { get; set; }
        public bool MainImage { get; set; }
        public string Descr { get; set; }
        public int ImageSize { get; set; }
        public string FileName { get; set; }
        public byte[] ImageData { get; set; }
        public string ContentType { get; set; }
        public ArtGenerator.ImageType ImageType { get; set; }
        public virtual Bouquet Bouquet { get; set; }
        public virtual Flower Flower { get; set; }
    }
}