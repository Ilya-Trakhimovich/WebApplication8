using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication8.Models
{
    public class EditProfileViewModel
    {
        
        [Display(Name = "Имя")]
        public string FirstName { get; set; }
        [Display(Name = "Фамилия")]
        public string SecondName { get; set; }

        [Display(Name = "Аватар")]
        public byte[] Avatar { get; set; }
        [Display(Name = "Возраст")]

        public int Age { get; set; }
        [Display(Name = "Город")]
        public string Address { get; set; }
        [Display(Name = "Пол")]
        public string Gender { get; set; }
        [Display(Name = "Образование")]
        public string Education { get; set; }
        [Display(Name = "Обо мне")]
        public string AboutMe { get; set; }
    }
}