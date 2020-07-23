using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication8.Models
{
    public class PostViewModel
    {
        public int Id { get; set; }
        public string Headline { get; set; }

        public string Author { get; set; }
        public byte[] Avatar { get; set; }

        [Required]
        public string Content { get; set; }
        public DateTime PostDate { get; set; }
        public int LikeCount { get; set; }

        public string UserPageId { get; set; }

        //  public 
        public string UserId { get; set; }
    }
}