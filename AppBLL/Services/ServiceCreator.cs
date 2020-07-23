using AppBLL.Interfaces;
using DataAcess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBLL.Services
{
    public class ServiceCreator : IServiceCreator
    {
        public IUserService CreateUserService(string connection)
        {
            return new UserService(new IdentityUnitOfWork(connection));
        }

        public IPostService CreatePostService(string connection)
        {
            return new PostService(new IdentityUnitOfWork(connection));
        }
        public ILikeService CreateLikeService(string connection)
        {
            return new LikeService(new IdentityUnitOfWork(connection));
        }

        public IGroupService CreateGroupService(string connection)
        {
            return new GroupService(new IdentityUnitOfWork(connection));
        }
    }
}
