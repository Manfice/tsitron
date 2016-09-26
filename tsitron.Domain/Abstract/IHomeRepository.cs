using System.Collections.Generic;
using System.Threading.Tasks;
using tsitron.Domain.Entitys.Customers;
using tsitron.Domain.Entitys.Goods;
using tsitron.Domain.Entitys.Orders;
using tsitron.Domain.Entitys.Secure;
using tsitron.Domain.Models;

namespace tsitron.Domain.Abstract
{
    public interface IHomeRepository
    {
        Task<IEnumerable<BvmForHome>> GetBouquets(string region);
        Bouquet GetBouquet(int id);
        Customer GetCustomer(int id);
        PhotoView GetAvatar(int id);
        Task<ValidEvents> CreateOrder(OrderAccept accept);

    }
}