using AppBLL.DataTransferObject;
using AutoMapper;
using DataAcess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBLL.Configs
{
    class MapperConfigs
    {
        public MapperConfiguration ProfileInformationDtoToUserProfile
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<ProfileInformationDTO, UserProfile>().
                ForMember(x=>x.Id,options=>options.Ignore()));
            }
        }

        public MapperConfiguration UserProfileToUserProfileDTO
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<UserProfile, UserProfileDTO>().
                ForMember(x => x.Name, x => x.MapFrom(m => m.FirstName + " " + m.SecondName)));
            }
        }

        public MapperConfiguration ApplicationUserToUserProfileDTO
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<ApplicationUser, UserProfileDTO>().
                ForMember(x => x.Name, x => x.MapFrom(m => m.UserProfile.FirstName + " " + m.UserProfile.SecondName)).
                ForMember(x=>x.Age,x=>x.MapFrom(m=>m.UserProfile.Age)).
                ForMember(x=>x.Avatar, x=>x.MapFrom(m=>m.UserProfile.Avatar)));
            }
        }
        public MapperConfiguration GroupToGroupDto
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<Group, GroupDTO>().
                ForMember(x => x.GroupMembers, options => options.Ignore()).
                ForMember(x=>x.GroupCreatorId, x=>x.MapFrom(m=>m.Admin)).
                ForMember(x => x.GroupPosts, options => options.Ignore()));
            }
        }

        public MapperConfiguration GroupPostToGroupPostDto
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<GroupPost, GroupPostDTO>().
                ForMember(x=>x.AuthorId,x=>x.MapFrom(m=>m.authorId)));
            }
        }

        public MapperConfiguration GroupDtoToGroupWithoutId
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<GroupDTO, Group>().
                ForMember(x => x.Id, options => options.Ignore()).
                ForMember(x => x.Admin, x => x.MapFrom(m => m.GroupCreatorId)));
            }
        }

        public MapperConfiguration GroupDtoToGroup
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<GroupDTO, Group>().
                ForMember(x => x.Admin, x => x.MapFrom(m => m.GroupCreatorId)));
            }
        }

        public MapperConfiguration PostDtoToPostWithoutId
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<PostDTO, Post>().
                ForMember(x => x.Id, options => options.Ignore()).
                ForMember(x => x.ApplicationUserId, x => x.MapFrom(m => m.UserId)));
            }
        }

        public MapperConfiguration PostLikeToPostLikeDto
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<PostLike, PostLikeDTO>().
                ForMember(x => x.UserId, x => x.MapFrom(m => m.User)));
            }
        }

        public MapperConfiguration PostLikeDtoToPostLike
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<PostLikeDTO, PostLike>().
                ForMember(x=>x.User,x=>x.MapFrom(m=>m.UserId)));
            }
        }


        public MapperConfiguration PostToPostDto
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<Post, PostDTO>().
                ForMember(x=>x.Author,x=>x.MapFrom(m=>m.ApplicationUser.UserProfile.FirstName+ " " + m.ApplicationUser.UserProfile.SecondName)).
                ForMember(x=>x.LikeCount,x=>x.MapFrom(m=>m.PostLikes.Count())).
                ForMember(x=>x.Avatar,x=>x.MapFrom(m=>m.ApplicationUser.UserProfile.Avatar)).
                ForMember(x=>x.UserId,x=>x.MapFrom(m=>m.ApplicationUserId)));
            }
        }

        public MapperConfiguration GroupPostDtoToGroupPostWithoutId
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<GroupPostDTO, GroupPost>().
                ForMember(x => x.Id, options => options.Ignore()));
            }
        }

        public MapperConfiguration UserProfileToProfileInformationDto
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<UserProfile, ProfileInformationDTO>());
            }
        }
    }
}
