using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcess.Entities
{
    public class GroupPost
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime PostDate { get; set; }
        public string authorId { get; set; }
        public int GroupId { get; set; }
        public virtual Group Group { get; set; }
        //   public byte[] Avatar { get; set; }
     //   public string ApplicationUserId { get; set; }
    ///   public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
