using AppBLL.DataTransferObject;
using AppBLL.Infrastructure;
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
    }
}
