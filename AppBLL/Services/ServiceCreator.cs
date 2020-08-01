using AppBLL.Interfaces;
using DataAcess.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBLL.Services
{
    public class ServiceCreator : IServiceCreator
    {
        readonly string connection = ConfigurationManager.AppSettings["connectionString"];

        public IUserService CreateUserService()
        {
            return new UserService(new IdentityUnitOfWork(connection));
        }

        public IPostService CreatePostService()
        {
            return new PostService(new IdentityUnitOfWork(connection));
        }
        public ILikeService CreateLikeService()
        {
            return new LikeService(new IdentityUnitOfWork(connection));
        }

        public IGroupService CreateGroupService()
        {
            return new GroupService(new IdentityUnitOfWork(connection));
        }
    }
}
