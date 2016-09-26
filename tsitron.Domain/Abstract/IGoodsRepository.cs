using System.Collections.Generic;
using tsitron.Domain.Entitys.Goods;

namespace tsitron.Domain.Abstract
{
    public interface IGoodsRepository
    {
        Photo GetImage(int id);
        IEnumerable<Bouquet> GetBouquets { get; }
        Photo GetBouqImage(int imgId);
        Bouquet GetBouquet(int id);
    }
}