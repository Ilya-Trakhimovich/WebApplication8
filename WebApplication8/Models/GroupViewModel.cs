using AppBLL.DataTransferObject;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication8.Models
{
    public class GroupViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Имя группы")]
        [Required(ErrorMessage = "Ты , это, заполни поле, куда ж без названия")]
        [MaxLength(20,ErrorMessage = "максимально 20 символов")]
        public string GroupName { get; set; }

        [Required(ErrorMessage = "Ты , это, заполни поле, куда ж без названия")]
        [Display(Name = "Описание группы")]
        [MaxLength(250,ErrorMessage = "Уважаемый, 250 символов")]
        public string GroupDescription { get; set; }

        [Display(Name = "Создатель группы")]
        public string GroupCreatorId { get; set; }

        [Display(Name = "Администратор")]
        public string Admin { get; set; }

        [Display(Name = "Участники сообщества")]
        public List<UserViewModel> GroupMembers { get; set; }
        public List<GroupPostViewModel> GroupPosts { get; set; }
        public byte[] Avatar { get; set; }
    }
}