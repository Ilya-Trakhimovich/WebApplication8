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
    public class PostService : IPostService
    {
        IUnitOfWork Database { get; set; }

        public PostService(IUnitOfWork uow)
        {
            Database = uow;
        }

      public void AddPost(PostDTO post)
        {
            Post postToDb = new Post()
            {
                Content = post.Content,
                ApplicationUserId = post.UserId,
                UserPageId = post.UserPageId,
                PostDate = post.PostDate
            };
            //Database.UserProfileManager.GetUserById(post.UserId).ApplicationUser.Posts.Add(postToDb);
            //Database.UserProfileManager.SaveChanges();

            Database.PostRepository.Add(postToDb);
        }

        public IEnumerable<PostDTO> GetAllPosts(string id)
        {
            List<PostDTO> allPosts = new List<PostDTO>();
            List<PostLikeDTO> postLikeDTOs = new List<PostLikeDTO>();
            var posts = Database.PostRepository.GetAll(id);
            //var posts = Database.UserProfileManager.GetUserById(id).ApplicationUser.Posts;
            
            foreach (var post in posts)
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<PostLike, PostLikeDTO>());
                var mapper = new Mapper(config);
                var editProfileViewModel = mapper.Map<List<PostLikeDTO>>(post.PostLikes);

                allPosts.Add(new PostDTO()
                {
                    Content = post.Content,
                    Author = post.ApplicationUser.UserProfile.FirstName + " " + post.ApplicationUser.UserProfile.SecondName,
                    Headline = post.Headline,
                    Id = post.Id,
                    LikeCount = post.PostLikes.Count(),
                    Avatar = post.ApplicationUser.UserProfile.Avatar,
                    PostDate = post.PostDate,
                    UserId = post.ApplicationUserId,
                    UserPageId = id
                }) ;
            }

            return allPosts;
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        public Task<OperationDetails> Create(PostDTO postDTO)
        {
            throw new NotImplementedException();
        }

        public void DeletePost(int id)
        {
            Database.PostRepository.Delete(id);
        }

     

        public PostDTO GetPostById(int id)
        {
            var post = Database.PostRepository.GetById(id);
            List<PostLikeDTO> postLikeDTOs = new List<PostLikeDTO>();

            foreach (var postLike in post.PostLikes)
            {
                postLikeDTOs.Add(new PostLikeDTO()
                {
                    Id = postLike.Id,
                    PostId = postLike.PostId,
                    UserId = postLike.User
                });
            }

            return new PostDTO()
            {
                Content = post.Content,
                Author = post.ApplicationUser.UserProfile.FirstName + " " + post.ApplicationUser.UserProfile.SecondName,
                Headline = post.Headline,
                Id = post.Id,
                Likes = postLikeDTOs,
                Avatar = post.ApplicationUser.UserProfile.Avatar,
                PostDate = post.PostDate,
                UserId = post.ApplicationUserId,
                UserPageId = post.UserPageId
            };
        }
    }
}

