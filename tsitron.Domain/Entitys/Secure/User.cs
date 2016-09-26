using System;

namespace tsitron.Domain.Entitys.Secure
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public bool ConfirmEmail { get; set; }
        public bool Block { get; set; } 
        public DateTime RegisterDate { get; set; }
        public virtual UsrRole UserRole { get; set; }
    }
}
