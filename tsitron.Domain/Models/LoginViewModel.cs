using System;
using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;

namespace tsitron.Domain.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Укажите свой номер телефона!")]
        [Display(Name = "Телефон")]
        [RegularExpression(@"^((\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{10}$",ErrorMessage = "Телефон нужно вводить в формате:+79991112233")]
        public string Telephone { get; set; }

        [Required(ErrorMessage = "Введите пароль!")]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
         
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Название магазина обязательно!")]
        [Display(Name = "Название магазина:")]
        public string ShopName { get; set; }    

        [Required(ErrorMessage = "Введите ваше имя.")]
        [Display(Name = "Как вас зовут:")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Введите вашу фамилию.")]
        [Display(Name = "Как вас зовут:")]
        public string LastName { get; set; }    

        [Required(AllowEmptyStrings = false,ErrorMessage = "Введите номер вашего мобильного телефона.")]
        [Display(Name = "Мобильный телефон:")]
        [RegularExpression(@"^((\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{10}$", ErrorMessage = "Телефон нужно вводить в формате:+79991112233")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Введите ваш город.")]
        [Display(Name = "Город:")]
        public string City { get; set; }

        [Required(ErrorMessage = "Введите индекс.")]
        [Display(Name = "Индекс:")]
        public string CityIndex { get; set; }   

        [Required(ErrorMessage = "Укажите адрес электронной почты.")]
        [Display(Name = "Ваш E-mail")]
        //  [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^[-a-z0-9!#$%&'*+/=?^_`{|}~]+(?:\.[-a-z0-9!#$%&'*+/=?^_`{|}~]+)*@(?:[a-z0-9]([-a-z0-9]{0,61}[a-z0-9])?\.)*(?:aero|arpa|asia|biz|cat|com|coop|edu|gov|info|int|jobs|mil|mobi|museum|name|net|org|pro|tel|travel|[a-z][a-z])$", ErrorMessage = "Проверьте правильность адреса эл. почты.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Введите пароль.")]
        [MinLength(6,ErrorMessage = "Минимум 6 символов!")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль:")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Введите подтверждение пароля.")]
        [Compare("Password",ErrorMessage = "Пароли не совпадают!")]
        [Display(Name = "Подтверждение пароля:")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Я поставщик.")]
        public bool Seller { get; set; }
    }

    public class UserViewModel
    {
        [Required(ErrorMessage = "Укажите Ф.И.О.")]
        [Display(Name = "Ф.И.О.")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Введите номер вашего мобильного телефона.")]
        [Display(Name = "Мобильный телефон:")]
        [RegularExpression(@"^((\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{10}$", ErrorMessage = "Телефон нужно вводить в формате:+79991112233")]
        public string Telephone { get; set; }

        [Required(ErrorMessage = "Укажите адрес электронной почты.")]
        [Display(Name = "Ваш E-mail")]
        //  [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^[-a-z0-9!#$%&'*+/=?^_`{|}~]+(?:\.[-a-z0-9!#$%&'*+/=?^_`{|}~]+)*@(?:[a-z0-9]([-a-z0-9]{0,61}[a-z0-9])?\.)*(?:aero|arpa|asia|biz|cat|com|coop|edu|gov|info|int|jobs|mil|mobi|museum|name|net|org|pro|tel|travel|[a-z][a-z])$", ErrorMessage = "Проверьте правильность адреса эл. почты.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Введите пароль.")]
        [MinLength(6, ErrorMessage = "Минимум 6 символов!")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль:")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Введите подтверждение пароля.")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают!")]
        [Display(Name = "Подтверждение пароля:")]
        public string ConfirmPassword { get; set; }

        public int RoleId { get; set; }
    }
}