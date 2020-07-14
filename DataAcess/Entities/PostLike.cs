using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcess.Entities
{
    public class PostLike
    {
        public int Id { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
        public PostLike()
        {
            Users = new List<ApplicationUser>();
        }
    }
}
