using tsitron.Domain.Entitys.Goods;

namespace tsitron.Domain.Models
{
    public class ColorViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string HexColor { get; set; }
        public bool Rainbow { get; set; }
        public bool IsSelected { get; set; }    

    }

    public class SaveColor
    {
        public string Title { get; set; }
        public string SelValue { get; set; }    
    }
}