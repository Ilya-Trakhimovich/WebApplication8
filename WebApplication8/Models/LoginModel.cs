using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication8.Models
{
    public class LoginModel
    {
        [Required]
        [Display(Name = "Эл. почта")]
        [MaxLength(20, ErrorMessage = "Дурачочек, короче. Максы 20 символов")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [MaxLength(20, ErrorMessage = "Дурачочек, короче. Максы 20 символов")]
        [Display(Name ="Пароль")]
        public string Password { get; set; }
    }
}