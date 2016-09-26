using System.Collections.Generic;
using tsitron.Domain.Entitys.Goods;

namespace tsitron.Domain.Models
{
    public class FCompositionViewModel
    {
        public IEnumerable<FlowerComposition> FlowerCompositions { get; set; }
        public FlowerComposition FlowerComposition { get; set; }
    }
}