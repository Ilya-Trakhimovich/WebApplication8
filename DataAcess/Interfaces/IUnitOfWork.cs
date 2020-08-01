using DataAcess.Entities;
using DataAcess.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcess.Interfaces
{
    public interface IUnitOfWork: IDisposable
    {
        Task SaveAsync();
        ApplicationUserManager UserManager { get; }
        ApplicationRoleManager RoleManager { get; }
        IUserProfileManager UserProfileManager { get; }
        IGroupManager<Group> GroupRepository { get; }
        IPostRepository<Post> PostRepository { get;  }
        IGroupPostRepository<GroupPost> GroupPostRepository { get; }
    }
}
