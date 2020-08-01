using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcess.Entities
{
    public class UserProfile
    {
        [Key]
        [ForeignKey("ApplicationUser")]
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public int Age { get; set; }
        public byte[] Avatar { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public string Education { get; set; }
        public string AboutMe { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
