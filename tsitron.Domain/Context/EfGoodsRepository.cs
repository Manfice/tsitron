using System;
using System.Collections.Generic;
using System.Linq;
using tsitron.Domain.Abstract;
using tsitron.Domain.Entitys.Goods;

namespace tsitron.Domain.Context
{
    public class EfGoodsRepository : IGoodsRepository
    {
        private readonly TsitronContext _context = ContextFabric.Context;

        public IEnumerable<Bouquet> GetBouquets => _context.Bouquets.ToList();

        public Photo GetBouqImage(int imgId)
        {
            var image = _context.Photos.FirstOrDefault(i => i.PhotoId==imgId);
            return image ?? new Photo();
        }

        public Bouquet GetBouquet(int id) => _context.Bouquets.Find(id);

        public Photo GetImage(int id)
        {
            throw new NotImplementedException();
        }

        //public Photo GetImage(int id)
        //{
        //    var image = _context.Photos.FirstOrDefault(i => i..Id == id);
        //    return image ?? new Image();
        //}



    }
}