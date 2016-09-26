using System.Collections.Generic;
using tsitron.Domain.Entitys.Customers;
using tsitron.Domain.Entitys.Secure;

namespace tsitron.Domain.Models
{
    public class UsersViewModel
    {
        public IEnumerable<User> Users { get; set; }
        public IEnumerable<Customer> Customers { get; set; }
    }
}