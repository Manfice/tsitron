using System.Collections.Generic;

namespace tsitron.Domain.Entitys.Goods
{
    public class Cleshe
    {
         
    }

    public class SingleFlower
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public virtual ICollection<FlowerImage> FlowerImage { get; set; }
        public virtual ICollection<FlowerHeight> FlowerHeight { get; set; }
    }

    public class FlowerHeight
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public string QuantityRange { get; set; }
        public decimal Price { get; set; }
        public bool OnStock { get; set; }
        public virtual SingleFlower SingleFlower { get; set; }
    }

    public class FlowerImage
    {
        public int Id { get; set; }
        public string ColorValue { get; set; }
        public int ImageSize { get; set; }
        public string FileName { get; set; }
        public byte[] ImageData { get; set; }
        public string ContentType { get; set; }
        public virtual SingleFlower SingleFlower { get; set; }
    }
}