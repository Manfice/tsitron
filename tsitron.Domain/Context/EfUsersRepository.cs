using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tsitron.Domain.Abstract;
using tsitron.Domain.Entitys.Customers;
using tsitron.Domain.Entitys.Secure;
using tsitron.Domain.Models;

namespace tsitron.Domain.Context
{
    public class EfUsersRepository : IUserRepository
    {
        private readonly TsitronContext _db = new TsitronContext();

        public IEnumerable<User> UserList => _db.Users.ToList();
        public IEnumerable<UsrRole> Roles => _db.UsrRoles.ToList(); 
        public async Task<ValidEvents> LoginAsync(LoginViewModel model)
        {
            var passhash = SecureLogic.EncodePassToHash(model.Password);
            var user = await _db.Users.FirstOrDefaultAsync(u => (u.Login == model.Telephone) && (u.PasswordHash == passhash));
            if (user != null)
            {
                return user.Block
                    ? new ValidEvents {Code = -1, Message = "Ваша учетная запись находится в режиме блокировки."}
                    : new ValidEvents {Code = user.Id, Message = "Вход выполнен успешно"};
            }
            return new ValidEvents {Code = -1, Message = "Ошибка логина или пароля"};

            /*Возвращаем код пользователя ЮЗЕРА*/
        }

        public async Task<User> GetUserAsync(int id)
        {
            return await _db.Users.FirstOrDefaultAsync(l=>l.Id==id);
        }

        public async Task<ValidEvents> RegisterAsync(RegisterViewModel model)
        {
            if (await _db.Users.AnyAsync(u => u.Login == model.Login))
                return new ValidEvents
                {
                    Code = -1,
                    Message = "Пользователь с таким номером телефона уже зарегистрирован в базе данных."
                };

            var defRole =  model.Seller ? _db.UsrRoles.FirstAsync(r => r.Title == "S") : _db.UsrRoles.FirstAsync(r => r.Title == "C");

            var user = new User
            {
                Login = model.Login,
                Name = model.Name,
                Email = model.Email,
                ConfirmEmail = false,
                PasswordHash = SecureLogic.EncodePassToHash(model.Password),
                RegisterDate = DateTime.Now,
                UserRole = await defRole,
                Block = true
            };

            _db.Users.Add(user);

            var customer = new Customer
            {
                Seller = true,
                Email = model.Email,
                Location = model.CityIndex+" "+model.City,
                Ranck = 0,
                Title = model.Name+" "+model.LastName,
                User = user,
                Register = DateTime.Now,
                Requisites = new Requisites(),
                MyShop = new MyShop {Title = model.ShopName},
                Telephone = model.Login
            };

            _db.Customers.Add(customer);
            await _db.SaveChangesAsync();

            return customer.Id > 0 ? new ValidEvents {Code = user.Id, Message = "Выполнена регистрация нового пользователя, проверьте почту, для подтверждения адреса электронной почты."} : new ValidEvents {Code = -1, Message = "Регистрация не выполнена. Проверьте введенные данные или обратитесь в службу поддержки."};
        }

        public async Task<ValidEvents> AddCustomerAsync(User user)
        {
            var cust = new Customer
            {
                Title = user.Name,
                Email = user.Email,
                Telephone = user.Login,
                Location = "",
                Seller = user.UserRole.Title=="Seller",
                Ranck = 0,
                Register = DateTime.Now,
                MyShop = new MyShop(),
                Requisites = new Requisites(),
                User = user
            };
            _db.Customers.Add(cust);
            await _db.SaveChangesAsync();

            return cust.Id != 0 ? new ValidEvents {Code = cust.Id, Message = "Регистрация клиента выполнена успешно."} : new ValidEvents {Code = -1, Message = "При регистрации клиента произошел сбой. Обратитесь в службу поддержки."};
        }

        public ValidEvents GetCustomer(User user)
        {
            var cust = _db.Customers.FirstOrDefault(u => u.User.Id == user.Id);
            return cust != null ? new ValidEvents {Code = cust.Id, Message = "Вход выполнен успешно."} : new ValidEvents {Code = -1, Message = "За пользователем не закрепленно ни одного клиента."};
        }
    }
}
