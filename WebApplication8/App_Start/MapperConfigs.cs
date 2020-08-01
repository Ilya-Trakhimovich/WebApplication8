using AppBLL.DataTransferObject;
using AppBLL.Interfaces;
using AppBLL.Services;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication8.Models;

namespace WebApplication8.App_Start
{
    public class MapperConfigs
    {
        private IUserService UserService
        {
            get
            {
                ServiceCreator serviceCreator = new ServiceCreator();
               return serviceCreator.CreateUserService();
            }
        }

        public MapperConfiguration ProfileInformationDtoToUserViewModel
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<ProfileInformationDTO, UserViewModel>().ForMember(x => x.Name, x => x.MapFrom(m => m.FirstName + " " + m.SecondName)));
            }
        }

        public MapperConfiguration PostDtoToPostViewModel
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<PostDTO, PostViewModel>());
            }
        }

        public MapperConfiguration ProfileInformationDtoToEditProfileViewModel
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<ProfileInformationDTO, EditProfileViewModel>());
            }
        }

        public MapperConfiguration GroupDtoToGroupViewModel
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<GroupDTO, GroupViewModel>().
                ForMember(x => x.GroupPosts, options => options.Ignore()).
                ForMember(x => x.GroupMembers, options => options.Ignore()));
            }
        }

        public MapperConfiguration GroupViewModelToGroupDtoWithoutAvatar
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<GroupViewModel, GroupDTO>().
                ForMember(x => x.GroupPosts, options => options.Ignore()).
                ForMember(x => x.GroupMembers, options => options.Ignore()).
                ForMember(x=>x.Avatar, options => options.Ignore()));
            }
        }

        public MapperConfiguration GroupPostDtoToGroupPostViewModel
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<GroupPostDTO, GroupPostViewModel>());
            }
        }
        public MapperConfiguration RegistraionModelToUserDto
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<RegisterModel, UserDTO>().
                ForMember(x=>x.Role, x=>x.MapFrom(m=>"user")).
                ForMember(x=>x.Avatar,x=>x.MapFrom(m=>UserService.GetDefaultAvatar())));
            }
        }

        public MapperConfiguration UserProfileDtoToUserViewModel
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<UserProfileDTO, UserViewModel>());
            }
        }
    }
}
