using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using tsitron.Domain.Abstract;
using tsitron.Domain.Entitys.Customers;
using tsitron.Domain.Entitys.Goods;
using tsitron.Domain.Entitys.Secure;
using tsitron.Domain.Models;

namespace tsitron.Domain.Context
{
    public class EfWebSellerRepository : IWebSellerRepository
    {
        private readonly TsitronContext _context = new TsitronContext();
        public async Task<Requisites> UpdateRequisites(Requisites requisites)
        {
            var result = await _context.Requisiteses.FindAsync(requisites.Id);
            if (result==null)
            {
                return null;
            }
            result.Title = requisites.Title;
            result.UrName = requisites.UrName;
            result.Inn = requisites.Inn;
            result.Kpp = requisites.Kpp;
            result.Address = requisites.Address;
            result.Bank = requisites.Bank;
            result.Account = requisites.Account;
            result.KorrAccount = requisites.KorrAccount;
            result.Bik = requisites.Bik;
            await _context.SaveChangesAsync();
            return result;
        }

        public async Task<MyShop> UpdateShopAsync(MyShop shop)
        {
            var dbShop = await _context.MyShops.FindAsync(shop.Id);
            if (dbShop==null)
            {
                return null;
            }
            dbShop.AroundTheClock = shop.AroundTheClock;
            dbShop.EndWork = shop.EndWork;
            dbShop.StartWork = shop.StartWork;
            dbShop.Title = shop.Title;
            await _context.SaveChangesAsync();
            return dbShop;
        }

        public async Task<Bouquet> CreateBouquet(ViewModelBouquet model)
        {
            var cust = _context.Customers.Find(model.Customer.Id);
            var b = new Bouquet
            {
                Articul = ArtGenerator.Atricul(typeof(Bouquet)),
                Customer = cust,
                Price = new Price { PriceValue = model.Price, Discount = 0,GoodsType = "bouquet", PrevPrice = 0, Title = DateTime.Now.ToString(CultureInfo.InvariantCulture)},
                Title = model.Title,
                Details = new Details(),
                TimeToMake = model.TimeToMake,
                Description = ""
            };
            _context.Bouquets.Add(b);
            await _context.SaveChangesAsync();
            return b;
        }

        public async Task<IEnumerable<ViewModelBouquet>> GetBouquetsByCustomer(int id)
        {
            var bouq = await _context.Bouquets.Where(b => b.Customer.Id == id).OrderByDescending(n=>n.Id).ToListAsync();
            var bvmList = new List<ViewModelBouquet>();

            foreach (var item in bouq)
            {
                var filtCol = await _context.FilterColors.Where(x => x.Bouquet.Id == item.Id).ToListAsync();
                var col = await _context.Colors.ToListAsync();

                foreach (var color in col.Where(color => filtCol.Any(y=>y.Color.Id==color.Id)))
                {
                    color.IsSelected = true;
                }

                var filtEvn = await _context.FilteredEventses.Where(x => x.Bouquet.Id == item.Id).ToListAsync();
                var fe = await _context.EventTypes.ToListAsync();

                foreach (var eventType in fe.Where(eventType => filtEvn.Any(ev=>ev.EventType.Id==eventType.Id)))
                {
                    eventType.IsSelected = true;
                }

                var photo = item.Photos.FirstOrDefault(p => p.ImageType == ArtGenerator.ImageType.Avatar);

                    var b = new ViewModelBouquet
                    {
                        Id = item.Id,
                        Art = item.Articul,
                        Customer = null,
                        Title = item.Title,
                        Price = item.Price?.PriceValue ?? new Price().PriceValue,
                        Colors = col,
                        Image = null,
                        TimeToMake = item.TimeToMake,
                        EventTypes = fe,
                        PhotoView = new PhotoView
                        {
                            Path = photo!=null ? photo.Path : "/Content/img/no_image.jpg",
                            Alt = photo!=null ? photo.AltText : "Фото не задано."
                        },
                        BouqAttrebutes = item.BouqAttrebutes??new BouqAttrebutes(),
                        Details = item.Details,
                        Photos = item.Photos,
                        Composition = item.FlowerComposition,
                        Description = item.Description,
                        ModeratedResult = item.ModeratedResult
                    };
                    bvmList.Add(b);
                
            }

            return bvmList;
        }

        public async Task<Bouquet> PublicBouquetAsync(PublishVm model)
        {
            var bouq = await _context.Bouquets.FindAsync(model.Id);
            if (bouq!=null)
            {
                bouq.BouqAttrebutes.Published = model.Publish;
                bouq.BouqAttrebutes.Moderated = false;
                bouq.BouqAttrebutes.Accepted = false;
            }
            await _context.SaveChangesAsync();
            return bouq;
        }

        public async Task<ViewModelBouquet> GetBouqDataAsync(int id)
        {
            var bouq = await _context.Bouquets.FindAsync(id);
            var col = await _context.Colors.ToListAsync();
            var selectedColors = col.ToList();
            var eve = await _context.EventTypes.ToListAsync();
            var selectedEvents = eve.ToList();

            foreach (var color in selectedColors)
            {
                var isSel = await _context.FilterColors.FirstOrDefaultAsync(x=>(x.Bouquet.Id==bouq.Id)&&(x.Color.Id==color.Id));
                if (isSel!=null)
                {
                    color.IsSelected = true;
                }
            }

            foreach (var @event in selectedEvents)
            {
                var isSel = await _context.FilteredEventses.FirstOrDefaultAsync(x=>(x.Bouquet.Id==bouq.Id)&&(x.EventType.Id==@event.Id));
                if (isSel!=null)
                {
                    @event.IsSelected = true;
                }
            }


            var photo = bouq.Photos.FirstOrDefault(p => p.ImageType == ArtGenerator.ImageType.Avatar);

            var b = new ViewModelBouquet
            {
                Id = bouq.Id,
                Art = bouq.Articul,
                Customer = null,
                Title = bouq.Title,
                Description = bouq.Description,
                Composition = bouq.FlowerComposition,
                Details = bouq.Details,
                Price = bouq.Price?.PriceValue ?? new Price().PriceValue,
                Colors = selectedColors,
                Image = null,
                TimeToMake = bouq.TimeToMake,
                EventTypes = selectedEvents,
                PhotoView = new PhotoView
                {
                    Path = photo != null ? photo.Path : "/Content/img/no_image.jpg",
                    Alt = photo != null ? photo.AltText : "Фото не задано."
                },
                Photos = bouq.Photos,
                ModeratedResult = bouq.ModeratedResult,
                BouqAttrebutes = bouq.BouqAttrebutes
            };
            return b;
        }

        public async Task<ViewModelBouquet> SaveBouqDataAsync(ViewModelBouquet model)
        {
            var dbBouquet = await _context.Bouquets.FindAsync(model.Id);
            dbBouquet.Description = model.Description;
            dbBouquet.FlowerComposition = model.Composition;
            dbBouquet.Title = model.Title;
            dbBouquet.TimeToMake = model.TimeToMake;
            dbBouquet.Details.Height = model.Details.Height;
            dbBouquet.Details.Width = model.Details.Width;
            dbBouquet.Price.PrevPrice = dbBouquet.Price.PriceValue;
            dbBouquet.Price.PriceValue = model.Price;
            dbBouquet.BouqAttrebutes.Moderated = false;
            dbBouquet.BouqAttrebutes.Accepted = false;
            dbBouquet.BouqAttrebutes.NewInLine = false;
            dbBouquet.BouqAttrebutes.Published = model.BouqAttrebutes.Published;

            var @events = _context.FilteredEventses.Where(x => x.Bouquet.Id == dbBouquet.Id).ToList();

            foreach (var filteredEventse in @events)
            {
                _context.FilteredEventses.Remove(filteredEventse);
            }

            var colors = _context.FilterColors.Where(x => x.Bouquet.Id == dbBouquet.Id).ToList();
            foreach (var color in colors)
            {
                _context.FilterColors.Remove(color);
            }

            foreach (var eventType in model.EventTypes)
            {
                if (eventType.IsSelected)
                {
                    var ev = _context.EventTypes.Find(eventType.Id);
                    var e = new FilteredEvents
                    {
                        Bouquet = dbBouquet,
                        GoodsType = "b",
                        EventType = ev
                    };
                    _context.FilteredEventses.Add(e);
                }
            }

            foreach (var goodsColor in model.Colors)
            {
                if (goodsColor.IsSelected)
                {
                    var c = _context.Colors.Find(goodsColor.Id);
                    var co = new FilterColor
                    {
                        Bouquet = dbBouquet,
                        GoodsType = "b",
                        Color = c
                    };
                _context.FilterColors.Add(co);
                }
            }

            await _context.SaveChangesAsync();

            return await GetBouqDataAsync(model.Id);
        }

        public async Task<ValidEvents> DeleteBouquet(int id)
        {
            var dbBouq = await _context.Bouquets.FindAsync(id);

            if (dbBouq == null)
            {
                return new ValidEvents {Code = -1, Message = "Букет не найден."};
            }

            var result = new ValidEvents { Code = dbBouq.Customer.Id, Message = "Букет " + dbBouq.Title + " успешно уделен." };

            _context.Bouquets.Remove(dbBouq);
            await _context.SaveChangesAsync();
            return result;
        }

        public async Task<IEnumerable<Photo>> DeletePhotosTask(int id)
        {
            return await _context.Photos.Where(p=>p.Bouquet.Id==id).ToListAsync();
        }
    }
}