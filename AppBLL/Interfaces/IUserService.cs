using AppBLL.DataTransferObject;
using AppBLL.Infrastructure;
using DataAcess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AppBLL.Interfaces
{
    public interface IUserService : IDisposable
    {
        Task<OperationDetails> Create(UserDTO userDto); // create users
        Task<ClaimsIdentity> Authenticate(UserDTO userDto); // user authentication
        Task SetInitialData(UserDTO adminDto, List<string> roles); // installation of initial data in the database - admin and list of roles
        UserProfileDTO GetUserDetails(string id);
        ProfileInformationDTO GetProfileInformation(string id);

        List<UserProfileDTO> GetAllUsers();
        List<UserProfileDTO> GetUsersByName(string userName);

        bool IsUserExist(string id);
        byte[] GetAvatar(string id);
        byte[] GetDefaultAvatar();
        string GetFullName(string id);
        IEnumerable<GroupDTO> GetUserGroups(string userid);
        void SaveProfileInformationById(string id, ProfileInformationDTO informationDTO);
    }
}
