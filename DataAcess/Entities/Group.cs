using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcess.Entities
{
    public class Group
    {
        [Key]
        public int Id { get; set; }
        public string GroupName { get; set; }
        public string GroupDescription { get; set; }
        //public string ApplicationUserId { get; set; }
        //public virtual ApplicationUser ApplicationUser { get; set; }
        public byte[] Avatar { get; set; }
        public string Admin { get; set; }
        public virtual List<GroupPost> GroupPosts { get; set; }
        public virtual List<ApplicationUser> ApplicationUsers { get; set; }
        public Group()
        {
            GroupPosts = new List<GroupPost>();
            ApplicationUsers = new List<ApplicationUser>();
        }
    }
}
