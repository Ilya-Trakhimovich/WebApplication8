using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppBLL.DataTransferObject
{
    public class ProfileInformationDTO
    {
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public byte[] Avatar { get; set; }
        public string Education { get; set; }
        public string AboutMe { get; set; }

    }
}