using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using tsitron.Domain.Entitys.Customers;

namespace tsitron.Domain.Models
{
    public class ShopViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Укажите наименование вашего магазина")]
        [MinLength(3, ErrorMessage = "Наименование должно быть длиннее 3х символов")]
        public string Title { get; set; }

        public int StartWork { get; set; }
        public int EndWork { get; set; }
        public bool AroundTheClock { get; set; } // Круглосуточно
    }

    public class ShopVm
    {
        public MyShop Shop { get; set; }
        public List<ShopGraphic> WeekEnds { get; set; }
    }
}