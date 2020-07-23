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
        public string User { get; set; }
        public int PostId { get; set; }
        public virtual Post Post { get; set; }
    }
}
