using AppBLL.DataTransferObject;
using AppBLL.Infrastructure;
using AppBLL.Interfaces;
using AutoMapper;
using DataAcess.Entities;
using DataAcess.Interfaces;
using DataAcess.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AppBLL.Services
{
    public class LikeService : ILikeService
    {
        IUnitOfWork Database { get; set; }

        public LikeService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public void AddLikeToPost(PostLikeDTO postLikeDTO)
        {
            PostLike postLike = new PostLike()
            {
                Id = postLikeDTO.Id,
                PostId = postLikeDTO.PostId,
                User = postLikeDTO.UserId
            };
          
            Database.PostRepository.PostLike(postLike);
        }

        public void DislikePost(PostLikeDTO postLikeDTO)
        {
            PostLike postLike = new PostLike()
            {
                Id = postLikeDTO.Id,
                PostId = postLikeDTO.PostId,
                User = postLikeDTO.UserId
            };

            Database.PostRepository.PostDislike(postLike);
        }

        public List<PostLike> GetAllPostLikes(int postId)
        {
           return Database.PostRepository.GetById(postId).PostLikes;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}

