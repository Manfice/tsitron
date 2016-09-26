using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using tsitron.Domain.Abstract;
using tsitron.Domain.Entitys.Goods;
using tsitron.Domain.Entitys.Orders;
using tsitron.Domain.Entitys.Secure;
using tsitron.Domain.Models;

namespace tsitron.Domain.Context
{
    public class EfAdminRepository:IAdminRepository
    {
        private readonly TsitronContext _context = new TsitronContext();

        public async Task<ValidEvents> AddEventAsync(EventType eventType)
        {
            if (await _context.EventTypes.AnyAsync(e=>e.Title==eventType.Title))
            {
                return new ValidEvents {Code = -1, Message = "Такое событие уже есть."};
            }
            var et = new EventType {Title = eventType.Title};
            _context.EventTypes.Add(et);
            await _context.SaveChangesAsync();
            return new ValidEvents {Code = et.Id, Message = "Событие добавлено."};
        }

        public async Task<ValidEvents> AddFlowerTypeAsync(FlowerType model)
        {
            if (_context.FlowerTypes.Any(f => f.Title == model.Title))
                return new ValidEvents {Code = -1, Message = "Цветок с таким наименованием уже существует."};
            var ft = new FlowerType {Title = model.Title};
            _context.FlowerTypes.Add(ft);
            await _context.SaveChangesAsync();
            return new ValidEvents {Code = ft.Id, Message = "Данные сохранены."};
        }

        public async Task<ValidEvents> AddUserAsync(UserViewModel user)
        {
            if (_context.Users.Any(u=>u.Login==user.Telephone))
            {
                return new ValidEvents {Code = -1, Message = "Пользователь с таким номером телефона уже существует."};
            }
            var role = _context.UsrRoles.Find(user.RoleId);
            var usr = new User
            {
                Name = user.Name,
                Login = user.Telephone,
                ConfirmEmail = false,
                Email = user.Email,
                PasswordHash = SecureLogic.EncodePassToHash(user.Password),
                RegisterDate = DateTime.Now,
                UserRole = role
            };
            _context.Users.Add(usr);
            await _context.SaveChangesAsync();
            return new ValidEvents
            {
                Code = usr.Id,
                Message = "Пользователь успешно внесен в базу данных"
            };
        }

        public async Task<ValidEvents> DeleteColor(int id)
        {
            if (!await _context.Colors.AnyAsync(i => i.Id == id)) return new ValidEvents {Code = -1, Message = "Error"};
            var col = await _context.Colors.FirstOrDefaultAsync(i => i.Id == id);
            _context.Colors.Remove(col);
            await _context.SaveChangesAsync();
            return new ValidEvents {Code = id, Message = "Удалено."};
        }

        public async Task<ValidEvents> DeleteEventAsync(int id)
        {
            if (!await _context.EventTypes.AnyAsync(i => i.Id == id))
                return new ValidEvents {Code = -1, Message = "Ошибка. События не существует."};
            var et = await _context.EventTypes.FirstOrDefaultAsync(i => i.Id == id);
            _context.EventTypes.Remove(et);
            await _context.SaveChangesAsync();
            return new ValidEvents {Code = id, Message = "Событие удалено"};
        }

        public async Task<ValidEvents> DeleteFlowerTypeAsync(int id)
        {
            if (!_context.FlowerTypes.Any(f=>f.Id==id))
            {
                return new ValidEvents {Code = -1, Message = "Ошибка удаления. Записи не существует."};
            }
            if (await _context.Flowers.AnyAsync(flt => flt.FlowerType.Id == id))
            {
                return new ValidEvents
                {
                    Code = -2,
                    Message =
                        "Запись имеет связь с следующими данными. Предварительно нужно удалить эти данные или ссылки."
                };
            }
            var ft = _context.FlowerTypes.FirstOrDefault(f => f.Id == id);
            _context.FlowerTypes.Remove(ft);
            await _context.SaveChangesAsync();
            return new ValidEvents {Code = id, Message = "Удалено."};
        }

        public async Task<IEnumerable<EventType>> EventTypesAsync()
        {
            return await _context.EventTypes.ToListAsync();
        }

        public Flower Flower(int id)
        {
            return _context.Flowers.FirstOrDefault(f => f.Id == id);

        }

        public async Task<IEnumerable<FlowerType>> FlowerTypesAsync() => await _context.FlowerTypes.OrderBy(t=>t.Title).ToListAsync();

        public IEnumerable<GoodsColor> GetColors()
        {
           return _context.Colors.ToList();
        }

        public async Task<IEnumerable<GoodsColor>> GetColorsAsync()
        {
            return await _context.Colors.ToListAsync();
        }

        public IEnumerable<Flower> GetFlowers()
        {
            return _context.Flowers.ToList();
        }

        public async Task<ValidEvents> SaveFlowerAsync(string title)
        {
            if (await _context.FlowerTypes.AnyAsync(f => f.Title == title))
            {
                return new ValidEvents
                {
                    Code = -1,
                    Message = "Такое название уже присутствует в списке."
                };
            }
            var fl = new FlowerType
            {
                Title = title
            };
            _context.FlowerTypes.Add(fl);
            await _context.SaveChangesAsync();
            return new ValidEvents
            {
                Code = fl.Id,
                Message = "Данные сохранены"
            };

        } 

        public IEnumerable<UsrRole> GetRoles() => _context.UsrRoles.ToList();

        public User GetUser(int id)
        {
            return _context.Users.Find(id);
        }

        public async Task<ValidEvents> SaveColor(GoodsColor color)
        {
            var col = color;
            if (await _context.Colors.AnyAsync(c => c.StrValue == color.StrValue))
            {
                return new ValidEvents { Code = -1, Message = "Такой цвет уже существует." };
            }
            if (string.IsNullOrEmpty(color.StrValue))
            {
                return new ValidEvents { Code = -1, Message = "Поле \" НАИМЕНОВАНИЕ\" не должно быть пустым." };
            }
            _context.Colors.Add(col);
            await _context.SaveChangesAsync();
            return new ValidEvents {Code = col.Id, Message = "Цвет сохранен."};
        }

        public async Task<IEnumerable<User>> UsersAsync() => await _context.Users.ToListAsync();

        public async Task<UsersViewModel> UsersViewAsync()
        {
            return new UsersViewModel
            {
                Users = await _context.Users.ToListAsync(),
                Customers = await _context.Customers.Where(c=>c.Seller).ToListAsync()
            };
        }

        public async Task<ValidEvents> BlockUserAsync(int id)
        {
            var dbUser = await _context.Users.FindAsync(id);
            if (dbUser.Login == "+79034441684")
            {
                return new ValidEvents
                {
                    Code = -1,
                    Message = "Данного пользователя нельзя блокировать"
                };
            }
            var rs = dbUser.Block;
            dbUser.Block = !rs;
            await _context.SaveChangesAsync();
            return new ValidEvents
            {
                Code = dbUser.Id,
                Message = dbUser.Block ? "Пользователь " + dbUser.Name + " заблокирован." : "Пользователь " + dbUser.Name + " разблокирован."
            };
        }

        public async Task<IEnumerable<Bouquet>> GetBouquetsAsync()
        {
            var result = await _context.Bouquets.Where(b => (b.BouqAttrebutes.Published)&&(b.BouqAttrebutes.Moderated==false)).ToListAsync();

            return result;
        }

        public async Task<Bouquet> GetBouquetAsync(int id)
        {
            return await _context.Bouquets.FindAsync(id);
        }

        public async Task<PhotoView> GetAvatarAsync(int id)
        {
            var pv = await _context.Photos.Where(p => (p.Bouquet.Id == id) && (p.ImageType == ArtGenerator.ImageType.Avatar))
                    .FirstOrDefaultAsync();
            return new PhotoView
            {
                Path = pv.Path,
                Alt = pv.AltText
            };
        }

        public async Task<ValidEvents> ModerateAsync(ModerateModel model)
        {
            var b = await _context.Bouquets.FindAsync(model.BouqId);
            b.BouqAttrebutes.Accepted = model.Moderated;
            b.BouqAttrebutes.Moderated = true;
            if (b.ModeratedResult == null)
            {
                b.ModeratedResult = new ModeratedResult
                {
                    ResultAnsw = model.ResultAnsw
                };
            }
            b.ModeratedResult.ResultAnsw = model.ResultAnsw;
            await _context.SaveChangesAsync();
            return new ValidEvents
            {
                Code = b.Id,
                Message = b.Articul
            };
        }

        public async Task<IEnumerable<Order>> OrdersTask()
        {
            return await _context.Orders.ToListAsync();
        }
    }
}