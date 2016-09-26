using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using tsitron.Domain.Abstract;
using tsitron.Domain.Entitys.Customers;
using tsitron.Domain.Entitys.Goods;
using tsitron.Domain.Entitys.Orders;
using tsitron.Domain.Entitys.Secure;
using tsitron.Domain.Models;

namespace tsitron.Domain.Context
{
    public class EfHomeRepository:IHomeRepository
    {
        private readonly TsitronContext _context = new TsitronContext();

        public async Task<IEnumerable<BvmForHome>> GetBouquets(string region)
        {
            var bouquets = string.IsNullOrEmpty(region)
                ?  await _context.Bouquets.ToListAsync()
                :  await _context.Bouquets.Where(b => b.Customer.Location.ToLower().Contains(region)).ToListAsync();

            return bouquets.Select(bouquet => new BvmForHome
            {
                Marker = bouquet.Id.ToString(), Monger = bouquet.Customer.Id.ToString(), Title = bouquet.Title, Avatar = new PhotoView
                {
                    Path = bouquet.Photos.FirstOrDefault(p => p.ImageType == ArtGenerator.ImageType.Avatar)?.Path, Alt = bouquet.Photos.FirstOrDefault(p => p.ImageType == ArtGenerator.ImageType.Avatar)?.AltText
                },
                Price = bouquet.Price.PriceValue.ToString("####"), Photos = bouquet.Photos.Where(photo => photo.ImageType == ArtGenerator.ImageType.Photo).ToList()
            }).ToList();
        }

        public Bouquet GetBouquet(int id)
        {
            return _context.Bouquets.Find(id);
        }

        public Customer GetCustomer(int id)
        {
            return _context.Customers.Find(id);
        }

        public PhotoView GetAvatar(int id)
        {
            var avatar =
                _context.Bouquets.Find(id).Photos.FirstOrDefault(p => p.ImageType == ArtGenerator.ImageType.Avatar);
            if (avatar!=null)
            {
                return new PhotoView
                {
                    Path = avatar.Path,
                    Alt = avatar.AltText??"pic"
                };
            }
            return new PhotoView {Path = "", Alt = "No Avatar"};
        }

        public async Task<ValidEvents> CreateOrder(OrderAccept accept)
        {
            var ve = new ValidEvents();
            var bouq = await _context.Bouquets.FindAsync(accept.Bouquet.Id);
            Customer cust = null;
            if (accept.Customer.Id == 0)
            {
                cust = new Customer
                {
                    Email = accept.Customer.Email,
                    Seller = false,
                    Register = DateTime.Now,
                    Telephone = accept.Customer.Telephone,
                    Title = accept.Customer.Title,
                    Requisites = new Requisites(),
                    User = new User
                    {
                        Block = false,
                        ConfirmEmail = false,
                        Email = accept.Customer.Email,
                        Login = accept.Customer.Telephone,
                        Name = accept.Customer.Title,
                        UserRole = _context.UsrRoles.FirstOrDefault(r => r.Title == "C"),
                        RegisterDate = DateTime.Now,
                        PasswordHash = SecureLogic.EncodePassToHash("1")
                    }
                };
                _context.Customers.Add(cust);
            }
            else
            {
                cust = await _context.Customers.FindAsync(accept.Customer.Id);
            }
            var recipient = new Recipient
            {
                Adres = accept.Recipient.Adres,
                Date = accept.Recipient.Date,
                Incognito = accept.Recipient.Incognito,
                Name = accept.Recipient.Name,
                Phone = accept.Recipient.Phone,
                RecipientMe = accept.Recipient.RecipientMe,
                UnknownAddress = accept.Recipient.UnknownAddress
            };
            var order = new Order
            {
                Bouquet = bouq,
                Customer = cust,
                Recipient = recipient,
                OrderDate = DateTime.Now,
                Info = accept.Info
            };
            _context.Orders.Add(order);

            try
            {
               await _context.SaveChangesAsync();
                ve.Code = order.Id;
                ve.Message = "Заказ № от сформирован, дождитесь ответа поставщика.";
            }
            catch (Exception e)
            {
                ve.Code = -1;
                ve.Message=e.Message;
            }

            return ve;
        }
    }
}