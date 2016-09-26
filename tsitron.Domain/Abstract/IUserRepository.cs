using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tsitron.Domain.Entitys.Customers;
using tsitron.Domain.Entitys.Secure;
using tsitron.Domain.Models;

namespace tsitron.Domain.Abstract
{
    public interface IUserRepository
    {
        IEnumerable<User> UserList { get; }
        IEnumerable<UsrRole> Roles { get; } 
        Task<ValidEvents> RegisterAsync(RegisterViewModel model);
        Task<ValidEvents> LoginAsync(LoginViewModel model);
        Task<User> GetUserAsync(int id);
        Task<ValidEvents> AddCustomerAsync(User user);
    }
}
