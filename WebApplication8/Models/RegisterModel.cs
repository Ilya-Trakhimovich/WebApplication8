using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication8.Models
{
    public class RegisterModel
    {
        [Required]
        [Display(Name = "Эл. почта")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
        [Display(Name = "Повторите пароль")]
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        [Display(Name = "Имя")]
        [Required]
        public string FirstName { get; set; }
        [Display(Name = "Фамилия")]
        [Required]
        public string SecondName { get; set; }
    }
}