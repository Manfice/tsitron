using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using System.Web.Routing;
using tsitron.Domain.Entitys.Customers;
using tsitron.Domain.Entitys.Goods;
using tsitron.Domain.Entitys.Secure;
using tsitron.Domain.Models;

namespace tsitron.Domain.Abstract
{
    public interface IWebSellerRepository
    {
        Task<Requisites> UpdateRequisites(Requisites requisites);
        Task<MyShop> UpdateShopAsync(MyShop shop);
        Task<Bouquet> CreateBouquet(ViewModelBouquet model);
        Task<IEnumerable<ViewModelBouquet>> GetBouquetsByCustomer(int id);
        Task<Bouquet> PublicBouquetAsync(PublishVm model);
        Task<ViewModelBouquet> GetBouqDataAsync(int id);
        Task<ViewModelBouquet> SaveBouqDataAsync(ViewModelBouquet model);
        Task<ValidEvents> DeleteBouquet(int id);
        Task<IEnumerable<Photo>> DeletePhotosTask(int id); // Bouquet ID
    }
}