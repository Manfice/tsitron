using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Threading.Tasks;
using tsitron.Domain.Abstract;
using tsitron.Domain.Entitys.Customers;
using tsitron.Domain.Entitys.Goods;
using tsitron.Domain.Entitys.Orders;
using tsitron.Domain.Entitys.Secure;
using tsitron.Domain.Models;
using Color = System.Drawing.Color;

namespace tsitron.Domain.Context
{
    public class EfSellerRypository : ISellerRepository
    {
        private readonly TsitronContext _context = new TsitronContext();

        public IEnumerable<Customer> GetCustomers => _context.Customers.ToList(); 

        public async Task<Customer> GetCustomerAsync(int id)
        {
            return await _context.Customers.FindAsync(id);
        }

        public Customer GetCustomerByUser(int id)
        {
            return _context.Customers.FirstOrDefault(c => c.User.Id == id);
        }

        public IEnumerable<Bouquet> Bouquets (int id)
        {
            return _context.Bouquets.Where(b => b.Customer.Id == id).ToList();
        }

        public IEnumerable<Price> FlowerPrices => _context.Prices.Where(p => p.GoodsType == "flower").ToList();
        public IEnumerable<Customer> Sellers => _context.Customers.Where(c=>c.Seller).ToList();

        public IEnumerable<FlowerType> FlowerTypes => _context.FlowerTypes.ToList();

        public Customer Customer(int id) => _context.Customers.FirstOrDefault(c=>c.Id==id);

     
        public ValidEvents AddBouquet(ViewModelBouquet model)
        {
           
            var bouq = new Bouquet
            {
                Customer = model.Customer,
                Title = "Новый букет",
                Details = new Details {Height = 0, Width = 0},
                Articul = ArtGenerator.Atricul(typeof(Bouquet)),
                TimeToMake = 0,
                BouqAttrebutes = new BouqAttrebutes()
                {
                    Published = false,
                    Moderated = false,
                    Accepted = false,
                    NewInLine = true,
                    SpecialPrice = false
                },
                Description = "Нет описания",
                Price = new Price
                {
                    Discount = 0,
                    GoodsType = "b",
                    PrevPrice = 0,
                    PriceValue = 0,
                    Title = "Цена еще не установлена"
                },
                FlowerComposition = "Не задано",
                ModeratedResult = new ModeratedResult { ResultAnsw = "Новый букет."}
        };
            _context.Bouquets.Add(bouq);
            model.Photo.Bouquet = bouq;
            _context.Photos.Add(model.Photo);
            _context.SaveChanges();
            return new ValidEvents {Code = bouq.Id, Message = "Ok"};
        }

        public ValidEvents UpdateBouquet(Bouquet bouquet)
        {
            var bouq = _context.Bouquets.FirstOrDefault(b => b.Id == bouquet.Id);
            if (bouq == null)
                return new ValidEvents
                {
                    Code = -1,
                    Message = "Ошибка, проверьте вводимые данные или обратитесь в службу поддержки"
                };
            bouq.Title = bouquet.Title;
            var prUpdate = SetPrice(bouquet.Price.Id, bouquet.Price.PriceValue);
            _context.SaveChanges();
            return new ValidEvents {Code = bouq.Id,Message = prUpdate.Message};
        }

        public Bouquet Bouquet(int id) => _context.Bouquets.FirstOrDefault(b => b.Id == id);

        
        public ValidEvents DeleteImage(int image)
        {
            var img = _context.Photos.FirstOrDefault(i => i.PhotoId == image);
            if (img != null)
            {
                var bouqId = img.Bouquet.Id;
                _context.Photos.Remove(img);
                _context.SaveChanges();
                return new ValidEvents {Code = bouqId, Message = "Фото удалено."};
            }
            return new ValidEvents { Code = -1, Message = "Warning" };
        }

        public ValidEvents SetAvatar(int bouquetId)
        {
            var images = _context.Photos.Where(b => b.Bouquet.Id == bouquetId).ToList();
            if (images.Count <= 0) return new ValidEvents {Code = -1, Message = "Error"};
            foreach (var image in images)
            {
                image.ImageType=ArtGenerator.ImageType.Photo;
            }
            _context.SaveChanges();
            return new ValidEvents {Code = 1, Message = "Ok"};
        }

        public Details GetBouquetDetails(int id)
        {
            var details = _context.Detailses.FirstOrDefault(d => d.Id == id);
            return details ?? new Details();
        }

        public ValidEvents UpdateBouquetDetails(Details details)
        {
            var det = _context.Detailses.FirstOrDefault(d => d.Id == details.Id);
            if (det == null) return new ValidEvents {Code = -1, Message = "Ошибка вставки данных."};
            det.Height = details.Height;
            det.Width = details.Width;
            _context.SaveChanges();
            return new ValidEvents {Code = det.Id, Message = "Данные обновлены"};
        }

        public IEnumerable<Flower> Flowers(int id)
        {
            return _context.Flowers.Where(c => c.Customer.Id == id).ToList();
        }

        public ValidEvents AddFlower(FlowerViewModel model)
        {
            var art = ArtGenerator.Atricul(typeof(Flower));
            var flowerType = _context.FlowerTypes.FirstOrDefault(i => i.Id == model.FlowerType.Id);

            return new ValidEvents {Code = -1, Message = "Цветок успешно добавлен в базу."};
        }

        public ValidEvents DeleteFlower(int id)
        {
            var flower = _context.Flowers.FirstOrDefault(i => i.Id == id);
            if (flower==null)
            {
                return new ValidEvents {Code = -1, Message = "ошибка, цветок не найден в БД"};
            }
            _context.Flowers.Remove(flower);
            _context.SaveChanges();
            return new ValidEvents {Code = flower.Id, Message = "Цветок: "+flower.Articul+" "+flower.Title+" удален."};
        }

        public IEnumerable<EventType> GetEventTypes()
        {
            return _context.EventTypes.OrderBy(i=>i.Title).ToList();
        }

        public ValidEvents SetPrice(int pId, decimal price)
        {
            var newPrice = _context.Prices.Find(pId);
            if (newPrice==null)
            {
                return new ValidEvents {Code = -1, Message = "Цена не найдена"};
            }
            if (newPrice.PriceValue > price)
            {
                newPrice.PrevPrice = newPrice.PriceValue;
                newPrice.PriceValue = price;
                newPrice.Discount = decimal.Truncate(100 - (price*100/newPrice.PrevPrice));
                _context.SaveChanges();
                return new ValidEvents
                {
                    Code = newPrice.Id,
                    Message = "Установлена новая более низкая цена. Товар стал дешевле на: " + newPrice.Discount + "%"
                };
            }
            newPrice.PrevPrice = newPrice.PriceValue;
            newPrice.Discount = 0;
            newPrice.PriceValue = price;
            _context.SaveChanges();
            return new ValidEvents
            {
                Code = newPrice.Id,
                Message = "Установлена новая цена."
            };
        }

        public IEnumerable<ColorViewModel> GetColors()
        {
            var col = _context.Colors.ToList();
            return col.Select(color => new ColorViewModel
            {
                Id = color.Id, Title = color.StrValue, HexColor = Color.FromArgb(color.Value).ToString(), Rainbow = color.Value == 0
            }).ToList();
        }

        public MyShop GetShopData(int id)
        {
            if (_context.Customers.Find(id).MyShop != null)
            {
                var dbshop = _context.Customers.Find(id).MyShop;
                if (!dbshop.WorkDayses.Any())
                {
                    var wd = GetWorkDayses();
                    foreach (var workDayse in wd)
                    {
                        var day = new ShopGraphic
                        {
                            AroundTheClock = false,
                            EndWork = 23,
                            StartWork = 1,
                            MyShop = dbshop,
                            WeekDayWork = workDayse.WeekDayWork,
                            WeekEnd = false
                        };
                        _context.ShopGraphics.Add(day);
                    }
                    _context.SaveChanges();
                }
                return dbshop;
            }
            var cust = _context.Customers.Find(id);
            var shop = new MyShop
            {
                AroundTheClock = false,
                Contacts = new Contacts(),
                Descr = "",
                EndWork = 0,
                StartWork = 0,
                FreePfoto = false,
                GreetingCard = false,
                Site = "www.",
                Title = "Без названия",
            };
            cust.MyShop = shop;
            _context.SaveChanges();
            return shop;
        }

        public ViewModelBouquet GetBouquetData(int id)
        {
            var bouq = _context.Bouquets.Find(id);
            var col = _context.Colors.ToList();
            var selectedColors = col.ToList();
            var eve = _context.EventTypes.ToList();
            var selectedEvents = eve.ToList();

            foreach (var color in selectedColors)
            {
                var isSel = _context.FilterColors.FirstOrDefault(x => (x.Bouquet.Id == bouq.Id) && (x.Color.Id == color.Id));
                if (isSel != null)
                {
                    color.IsSelected = true;
                }
            }

            foreach (var @event in selectedEvents)
            {
                var isSel = _context.FilteredEventses.FirstOrDefault(x => (x.Bouquet.Id == bouq.Id) && (x.EventType.Id == @event.Id));
                if (isSel != null)
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
                BouqAttrebutes = bouq.BouqAttrebutes,
                ModeratedResult = bouq.ModeratedResult
            };
            return b;
        }

        public async Task<Customer> SaveCustomer(Customer customer)
        {
            var cust = await _context.Customers.FindAsync(customer.Id);
            if (cust==null)
            {
                return null;
            }

            cust.Email = customer.Email;
            cust.Location = customer.Location;
            cust.Telephone = customer.Telephone;
            cust.Title = customer.Title;

            await _context.SaveChangesAsync();
            return cust;
        }

        public ValidEvents AddPhotoToBouquet(Photo photo)
        {
            if (photo == null) return new ValidEvents {Code = -1, Message = "Error"};
            var ph = photo;
            _context.Photos.Add(ph);
            _context.SaveChanges();
            return new ValidEvents {Code = ph.PhotoId, Message = "Фотография добавлена"};
        }

        public IEnumerable<Order> GetOrdersByCustomer(int id) => _context.Orders.Where(o => o.Bouquet.Customer.Id == id).ToList();
        public void ChangeStatus(int id, OrderStatus status)
        {
            var order = _context.Orders.Find(id);
            if (order!=null)
            {
                order.Status = status;
            }
            _context.SaveChanges();
        }

        public Order GetOrder(int id)
        {
            return _context.Orders.Find(id);
        }

        public ValidEvents SaveShopData(ShopVm model)
        {
            var dbShop = _context.MyShops.Find(model.Shop.Id);
            dbShop.Descr = model.Shop.Descr;
            dbShop.FreePfoto = model.Shop.FreePfoto;
            dbShop.GreetingCard = model.Shop.GreetingCard;
            dbShop.Site = model.Shop.Site;
            dbShop.Title = model.Shop.Title;
            foreach (var item in model.WeekEnds)
            {
                var dbDay = _context.ShopGraphics.Find(item.Id);
                dbDay.WeekEnd = item.WeekEnd;
                dbDay.AroundTheClock = item.AroundTheClock;
                dbDay.StartWork = item.StartWork;
                dbDay.EndWork = item.EndWork;
            }
            _context.SaveChanges();
            return new ValidEvents {Code = dbShop.Id, Message = "saved"};
        }

        public Contacts GetCustContacts(int id)
        {
            var cont = _context.Customers.Find(id).MyShop.Contacts;
            if (cont != null) return cont;
            var cust = _context.Customers.Find(id);
            cust.MyShop.Contacts=new Contacts
            {
                Town = cust.Location,
                Email = cust.Email,
                Phone1 = cust.Telephone,
                TimeToMakeAverage = "3:00"
            };
            _context.SaveChanges();
            return cust.MyShop.Contacts;
        }

        public ValidEvents SaveContacts(Contacts contacts)
        {
            var cont = _context.Contactses.Find(contacts.Id);
            cont.Adres = contacts.Adres;
            cont.DeliverInTown = contacts.DeliverInTown;
            cont.DeliverOutTown = contacts.DeliverOutTown;
            cont.Email = contacts.Email;
            cont.ZapasnoiEmail = contacts.ZapasnoiEmail;
            cont.Phone1 = contacts.Phone1;
            cont.Phone2 = contacts.Phone2;
            cont.TimeToMakeAverage = contacts.TimeToMakeAverage;
            cont.Town = contacts.Town;
            _context.SaveChanges();
            return new ValidEvents {Code = cont.Id, Message = "Save"};
        }

        public Requisites GetRequisites(int id)
        {
            return _context.Customers.Find(id).Requisites;
        }

        public List<ShopGraphic> GetWorkDayses()
        {
            return new List<ShopGraphic>
            {
                new ShopGraphic {WeekDayWork = "Понедельник",WeekEnd = false},
                new ShopGraphic {WeekDayWork = "Вторник",WeekEnd = false},
                new ShopGraphic {WeekDayWork = "Среда",WeekEnd = false},
                new ShopGraphic {WeekDayWork = "Четверг",WeekEnd = false},
                new ShopGraphic {WeekDayWork = "Пятница",WeekEnd = false},
                new ShopGraphic {WeekDayWork = "Суббота",WeekEnd = false},
                new ShopGraphic {WeekDayWork = "Воскресенье",WeekEnd = false},
            };
        }

        public void SaveRequisites(Requisites requisites)
        {
            var req = _context.Requisiteses.Find(requisites.Id);
            req.Title = requisites.Title;
            req.UrName = requisites.UrName;
            req.Address = requisites.Address;
            req.Inn = requisites.Inn;
            req.Kpp = requisites.Kpp;
            req.Bank = requisites.Bank;
            req.Account = requisites.Account;
            req.KorrAccount = requisites.KorrAccount;
            req.Bik = requisites.Bik;
            _context.SaveChanges();
        }

        public List<MyHolydays> Holydayses(int id)
        {
            return _context.Holydayses.Where(holydays => holydays.MyShop.Id == id).ToList();
        }

        public void AddHilyday(int id, MyHolydays holyDate)
        {
            if (holyDate.DayOff>holyDate.DayOffEnd)
            {
                return;
            }
            var hd = new MyHolydays
            {
                DayOff = holyDate.DayOff,
                DayOffEnd = holyDate.DayOffEnd,
                DayOffCount = (holyDate.DayOffEnd - holyDate.DayOff).Days,
                MyShop = _context.MyShops.Find(id)
            };
            _context.Holydayses.Add(hd);
            _context.SaveChanges();
        }

        public void RemoveHolyday(int id)
        {
            var dbH = _context.Holydayses.Find(id);
            _context.Holydayses.Remove(dbH);
            _context.SaveChanges();
        }

        public IEnumerable<Accessories> GetAccessories(int id)
        {
            return _context.Accessorieses.Where(accessories => accessories.Customer.Id==id).ToList();
        }

        public void SaveAccessory(Accessories model)
        {
            _context.Accessorieses.Add(model);
            _context.SaveChanges();
        }

        public void RemoveAccessory(int id)
        {
            var ac = _context.Accessorieses.Find(id);
            _context.Accessorieses.Remove(ac);
            _context.SaveChanges();
        }
    }
}