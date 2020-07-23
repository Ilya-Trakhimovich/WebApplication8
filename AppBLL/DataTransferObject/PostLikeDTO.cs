using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBLL.DataTransferObject
{
    public class PostLikeDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int PostId { get; set; }
     //   public PostDTO Post { get; set; }
    }
}
