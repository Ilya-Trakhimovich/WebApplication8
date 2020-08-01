using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBLL.DataTransferObject
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string SecondName { get; set; }
        public byte[] Avatar { get; set; }
        public string FirstName { get; set; }        
        public string Role { get; set; }
    }
}
