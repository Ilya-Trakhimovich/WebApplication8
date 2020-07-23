using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcess.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public string Headline { get; set; }
        public string Content { get; set; }
        public DateTime PostDate { get; set; }
        public string UserPageId { get; set; }
        public virtual List<PostLike> PostLikes { get; set; }
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        public Post()
        {
            PostLikes = new List<PostLike>();
        }
    }
}
