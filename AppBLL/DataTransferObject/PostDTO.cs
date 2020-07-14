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
        public DateTime PostDate { get; set; }
        public int? UserId { get; set; }
    }
}
