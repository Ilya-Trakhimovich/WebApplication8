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
    public interface ILikeService : IDisposable
    {
        void AddLikeToPost(PostLikeDTO postLike); // create post
        void DislikePost(PostLikeDTO postLike);
        List<PostLike> GetAllPostLikes(int postId);
    }
}
