using System.Collections.Generic;
using System.Threading.Tasks;
using tsitron.Domain.Entitys.Goods;
using tsitron.Domain.Entitys.Orders;
using tsitron.Domain.Entitys.Secure;
using tsitron.Domain.Models;

namespace tsitron.Domain.Abstract
{
    public interface IAdminRepository
    {
        User GetUser(int id);
        IEnumerable<UsrRole> GetRoles();
        IEnumerable<GoodsColor> GetColors();
        Task<IEnumerable<EventType>> EventTypesAsync();
        IEnumerable<Flower> GetFlowers();
        Flower Flower(int id);
        Task<ValidEvents> AddEventAsync(EventType eventType);
        Task<ValidEvents> DeleteEventAsync(int id);
        Task<ValidEvents> SaveColor(GoodsColor color);
        Task<ValidEvents> DeleteColor(int id); 
        Task<IEnumerable<User>> UsersAsync();
        Task<IEnumerable<FlowerType>> FlowerTypesAsync();
        Task<ValidEvents> AddUserAsync(UserViewModel user);
        Task<ValidEvents> AddFlowerTypeAsync(FlowerType model);
        Task<ValidEvents> DeleteFlowerTypeAsync(int id);
        Task<IEnumerable<GoodsColor>> GetColorsAsync();
        Task<ValidEvents> SaveFlowerAsync(string title);
        Task<UsersViewModel> UsersViewAsync();
        Task<ValidEvents> BlockUserAsync(int id);
        Task<IEnumerable<Bouquet>> GetBouquetsAsync();
        Task<Bouquet> GetBouquetAsync(int id);
        Task<PhotoView> GetAvatarAsync(int id);
        Task<ValidEvents> ModerateAsync(ModerateModel model);
        Task<IEnumerable<Order>> OrdersTask();

    }
}