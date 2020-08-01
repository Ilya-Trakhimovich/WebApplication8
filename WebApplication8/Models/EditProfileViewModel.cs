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
        [MaxLength(20, ErrorMessage = "20 символов")]
        public string FirstName { get; set; }

        [Display(Name = "Фамилия")]
        [MaxLength(30, ErrorMessage = "30 символов")]
        public string SecondName { get; set; }


        [Display(Name = "Аватар")]
        public byte[] Avatar { get; set; }

        [Display(Name = "Возраст")]
        [Range(0, 120, ErrorMessage = "Недопустимый возраст")]
        public int Age { get; set; }

        [Display(Name = "Город")]
        [MaxLength(20, ErrorMessage = "20 символов")]
        public string Address { get; set; }

        [Display(Name = "Пол")]
        public string Gender { get; set; }

        [Display(Name = "Образование")]
        [MaxLength(100, ErrorMessage = "100 символов")]
        public string Education { get; set; }

        [Display(Name = "Обо мне")]
        public string AboutMe { get; set; }
    }
}