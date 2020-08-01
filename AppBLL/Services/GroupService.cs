using AppBLL.Configs;
using AppBLL.DataTransferObject;
using AppBLL.Infrastructure;
using AppBLL.Interfaces;
using AutoMapper;
using AutoMapper.Internal;
using DataAcess.Entities;
using DataAcess.Interfaces;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBLL.Services
{
    public class GroupService : IGroupService
    {
        IUnitOfWork Database { get; set; }

        readonly MapperConfigs mapperConfigs = new MapperConfigs();

        public GroupService(IUnitOfWork now)
        {
            Database = now;
        }

        public void Create(GroupDTO groupDTO)
        {
            ApplicationUser creator = Database.UserManager
                .FindById(groupDTO.GroupCreatorId);

            Mapper groupMapper = new Mapper(mapperConfigs.GroupDtoToGroupWithoutId);
            Group newGroup = groupMapper.Map<Group>(groupDTO);

            newGroup.Avatar = GetDefaultAvatar();
            newGroup.ApplicationUsers.Add(creator);

            Database.GroupRepository.Create(newGroup);
        }

        public IEnumerable<GroupDTO> GetAllGroups()
        {
            var groups = Database.GroupRepository.GetAllGroups();

            Mapper groupDtoMapper = new Mapper(mapperConfigs.GroupToGroupDto);

            var groupsDTO = groupDtoMapper.Map<List<GroupDTO>>(groups);
            groupsDTO.ForAll(x => x.Admin = Database.UserManager.FindById(x.Admin).UserProfile.FirstName + " " + Database.UserManager.FindById(x.Admin).UserProfile.SecondName);

            return groupsDTO;
        }

        public GroupDTO GetGroupById(int id)
        {
            var group = Database.GroupRepository.GetGroupById(id);
            Mapper GroupDtoMapper = new Mapper(mapperConfigs.GroupToGroupDto);
            Mapper groupPostDtoMapper = new Mapper(mapperConfigs.GroupPostToGroupPostDto);
            Mapper userProfileDtoMapper = new Mapper(mapperConfigs.ApplicationUserToUserProfileDTO);

            List<UserProfileDTO> userProfileDTOs = new List<UserProfileDTO>();
            List<GroupPostDTO> groupPostDTOs = new List<GroupPostDTO>();

            groupPostDTOs = groupPostDtoMapper.Map<List<GroupPostDTO>>(group.GroupPosts.ToList());
            userProfileDTOs = userProfileDtoMapper.Map<List<UserProfileDTO>>(group.ApplicationUsers.ToList());

            var groupDto = GroupDtoMapper.Map<GroupDTO>(group);
            groupDto.Admin = Database.UserManager.FindById(group.Admin).UserProfile.FirstName + " " + Database.UserManager.FindById(group.Admin).UserProfile.SecondName;
            groupDto.GroupMembers = userProfileDTOs;
            groupDto.GroupPosts = groupPostDTOs;

            return groupDto;
        }

        public byte[] GetDefaultAvatar()
        {
            var image = File.OpenRead("GroupDefaultAvatar.jpg");
            byte[] imageData = null;

            using (var binaryReader = new BinaryReader(image))
                imageData = binaryReader.ReadBytes((int)image.Length);

            return imageData;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void AddUser(string userId, int groupId)
        {
            Database.UserManager
                .FindById(userId)
                .Groups.Add(Database.GroupRepository.GetGroupById(groupId));

            Database.UserProfileManager.SaveChanges();
        }

        public void DeleteUserFromGroup(string userId, int groupId)
        {
            Database.UserManager
                .FindById(userId)
                .Groups.Remove(Database.GroupRepository.GetGroupById(groupId));

            Database.UserProfileManager.SaveChanges();
        }

        public void UpdateGroupInformation(GroupDTO groupDTO)
        {
            Mapper GroupDtoToGroupMapper = new Mapper(mapperConfigs.GroupDtoToGroup);
            var group = GroupDtoToGroupMapper.Map<Group>(groupDTO);

            Database.GroupRepository.UpdateGroupInformation(group);
        }

        public byte[] GetAvatar(int id)
        {
            return Database.GroupRepository.GetGroupById(id).Avatar;
        }

        public string GetAdmin(int id)
        {
            return Database.GroupRepository.GetGroupById(id).Admin;
        }
        public IEnumerable<GroupDTO> GetGroupsByName(string groupName)
        {
            List<GroupDTO> groupsDTO = new List<GroupDTO>();

            groupName.Trim();
            var groups = Database.GroupRepository
                .GetGroupsByName(groupName)
                .ToList();

            foreach (var group in groups)
            {
                if (group.GroupName.Trim() == groupName)
                {
                    groupsDTO.Add(new GroupDTO()
                    {
                        Id = group.Id,
                        GroupName = group.GroupName,
                        GroupDescription = group.GroupDescription,
                        // GroupCreatorId = group.GroupCreatorId,
                        Admin = Database.UserManager.FindById(group.Admin).UserProfile.FirstName + " " + Database.UserManager.FindById(group.Admin).UserProfile.SecondName,
                        Avatar = group.Avatar
                        // GroupMembers = group.ApplicationUsers.ToList(),

                    });
                }
            }

            return groupsDTO;
        }
    }
}// 201
