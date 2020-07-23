using DataAcess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBLL.DataTransferObject
{
    public class PostDTO
    {
        public int Id { get; set; }
        public string Headline { get; set; }
        public string Content { get; set; }
        public string UserPageId { get; set; }
        public string Author { get; set; }
        public byte[] Avatar { get; set; }
        public DateTime PostDate { get; set; }
        public int LikeCount { get; set; }
       // public List<PostLike> Likes { get; set; }
       public List<PostLikeDTO> Likes { get; set; }
        public string UserId { get; set; }
    }
}
