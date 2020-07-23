using AppBLL.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBLL.Interfaces
{
    public interface IGroupService : IDisposable
    {
        void Create(GroupDTO groupDTO);
        IEnumerable<GroupDTO> GetAllGroups();
        GroupDTO GetGroupById(int id);
        byte[] GetDefaultAvatar();
        void AddUser(string userId, int groupId);
        void DeleteUserFromGroup(string userId, int groupId);

        void AddPost(GroupPostDTO groupPostDTO);

        void UpdateGroupInformation(GroupDTO groupDTO);

        byte[] GetAvatar(int id);

        string GetAdmin(int id);
        IEnumerable<GroupDTO> GetGroupsByName(string groupName);

    }
}
