using AppBLL.Configs;
using AppBLL.DataTransferObject;
using AppBLL.Infrastructure;
using AppBLL.Interfaces;
using AutoMapper;
using AutoMapper.Internal;
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
        readonly MapperConfigs mapperConfigs = new MapperConfigs();

        IUnitOfWork Database { get; set; }

        public PostService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public void AddPost(PostDTO post)
        {
            Mapper postMapper = new Mapper(mapperConfigs.PostDtoToPostWithoutId);
            var postToDb = postMapper.Map<Post>(post);

            Database.PostRepository.Add(postToDb);
        }

        public void AddGroupPost(GroupPostDTO post)
        {
            Mapper groupPostMapper = new Mapper(mapperConfigs.GroupPostDtoToGroupPostWithoutId);
            var groupPost = groupPostMapper.Map<GroupPost>(post);

            Database.GroupRepository.GetGroupById(post.GroupId).GroupPosts.Add(groupPost);
            Database.UserProfileManager.SaveChanges();
        }

        public IEnumerable<PostDTO> GetAllPosts(string id)
        {
            var posts = Database.PostRepository.GetAll(id);

            if (posts is null)
            {
                throw new Exception();
            }

            Mapper postDtoMapper = new Mapper(mapperConfigs.PostToPostDto);

            return postDtoMapper.Map<List<PostDTO>>(posts);
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        public Task<OperationDetails> Create(PostDTO postDTO)
        {
            throw new NotImplementedException();
        }

        public void DeletePost(int id) =>  Database.PostRepository.Delete(id);

        public void DeleteGroupPost(int groupPostId) => Database.GroupPostRepository.Delete(groupPostId);

        public PostDTO GetPostById(int id)
        {
            var post = Database.PostRepository.GetById(id);

            if (post is null)
            {
                throw new Exception();
            }

            List<PostLikeDTO> postLikeDTOs = new List<PostLikeDTO>();

            Mapper postLikeDtoMapper = new Mapper(mapperConfigs.PostLikeToPostLikeDto);
            Mapper postDtoMapper = new Mapper(mapperConfigs.PostToPostDto);

            postLikeDTOs = postLikeDtoMapper.Map<List<PostLikeDTO>>(post.PostLikes);
            PostDTO postDTO = postDtoMapper.Map<PostDTO>(post);

            postDTO.Likes = postLikeDTOs;

            return postDTO;
        }        
    }
}

