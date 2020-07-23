using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.UI.WebControls;

namespace WebApplication8.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Имя")]
        public string Name { get; set; }

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

        public IEnumerable<PostViewModel> Posts { get; set; }

    }
}