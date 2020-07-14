using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcess.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string Surname { get; set; }
        public int Age { get; set; }
        public virtual UserProfile UserProfile { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<PostLike> LikePosts { get; set; }

        public ApplicationUser()
        {
            Groups = new List<Group>();
            Posts = new List<Post>();
            LikePosts = new List<PostLike>();
        }
    }
}
