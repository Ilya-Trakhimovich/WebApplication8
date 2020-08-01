using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBLL.Interfaces
{
    public interface IServiceCreator
    {
        IUserService CreateUserService();
        IPostService CreatePostService();
        ILikeService CreateLikeService();
        IGroupService CreateGroupService();
    }
}
