using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcess.Entities
{
    public class Group
    {
        public int Id { get; set; }
        public string GroupName { get; set; }
        public string GroupDescription { get; set; }
        public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }

        public Group()
        {
            ApplicationUsers = new List<ApplicationUser>();
        }
    }
}
