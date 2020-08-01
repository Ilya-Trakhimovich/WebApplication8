using DataAcess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBLL.DataTransferObject
{
    public class GroupDTO
    {
        public int Id { get; set; }
        public string GroupName { get; set; }
        public string GroupDescription { get; set; }
        public string GroupCreatorId { get; set; }       
        public string Admin { get; set; }
        public byte[] Avatar { get; set; } 
        public List<GroupPostDTO> GroupPosts { get; set; }
        public List<UserProfileDTO> GroupMembers { get; set; }

        public GroupDTO()
        {
            GroupMembers = new List<UserProfileDTO>();
        }
    }
}
