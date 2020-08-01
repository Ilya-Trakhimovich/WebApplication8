using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication8.Models
{
    public class GroupPostViewModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime PostDate { get; set; }
        public string Author { get; set; }
        public string authorId { get; set; }
        public byte[] Avatar { get; set; }
        public int GroupId { get; set; }
    }
}