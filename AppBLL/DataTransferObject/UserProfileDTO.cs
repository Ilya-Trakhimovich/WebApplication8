using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppBLL.DataTransferObject
{
    public class UserProfileDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public byte[] Avatar { get; set; }

    }
}