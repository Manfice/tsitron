using System;

namespace tsitron.Domain.Entitys.Secure
{
    public class RegistersLog
    {
        public int Id { get; set; }
        public User User { get; set; }
        public DateTime LoginDate { get; set; }
        public DateTime LogOff { get; set; } 
    }
}