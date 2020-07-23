using AppBLL.DataTransferObject;
using AppBLL.Infrastructure;
using AppBLL.Interfaces;
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

        public GroupService(IUnitOfWork now)
        {
            Database = now;
        }
        public void Create(GroupDTO groupDTO)
        {
            ApplicationUser creator = Database.UserManager.FindById(groupDTO.GroupCreatorId);
            var avatar = GetDefaultAvatar();

            Group newGroup = new Group()
            {
               // Id = groupDTO.Id,
                GroupName = groupDTO.GroupName,
                GroupDescription = groupDTO.GroupDescription,
               
                //ApplicationUserId = groupDTO.GroupCreatorId,
                Admin = creator.Id,
                Avatar = avatar
            };
            newGroup.ApplicationUsers.Add(creator);
            Database.GroupRepository.Create(newGroup);
           // Database.GroupRepository.AddMember(newGroup)
        }

        public IEnumerable<GroupDTO> GetAllGroups()
        {

            List<GroupDTO> groupsDTO = new List<GroupDTO>();
            var groups = Database.GroupRepository.GetAllGroups();

            foreach (var group in groups)
            {
                groupsDTO.Add(new GroupDTO()
                {
                    Id = group.Id,
                    GroupName = group.GroupName,
                    GroupDescription = group.GroupDescription,
                 //   GroupMembers = group.ApplicationUsers.ToList(),
                //   GroupCreatorId = group.GroupCreatorId,
                   Admin = Database.UserManager.FindById(group.Admin).UserProfile.FirstName + " " + Database.UserManager.FindById(group.Admin).UserProfile.SecondName,
                    Avatar = group.Avatar
                }) ;
            }
            return groupsDTO;
        }

        public GroupDTO GetGroupById(int id)
        {
            var group = Database.GroupRepository.GetGroupById(id);
            List<UserProfileDTO> userProfileDTOs = new List<UserProfileDTO>();
            List<GroupPostDTO> groupPostDTOs = new List<GroupPostDTO>();

            foreach (var post in group.GroupPosts.ToList())
            {
                groupPostDTOs.Add(new GroupPostDTO()
                {
                    authorId = post.authorId,
                    Content = post.Content,
                    GroupId = post.GroupId,
                    Id = post.Id,
                    PostDate = post.PostDate
                });
            }

            foreach (var user in group.ApplicationUsers.ToList())
            {
                userProfileDTOs.Add(new UserProfileDTO()
                {
                    Age = user.UserProfile.Age,
                    Avatar = user.UserProfile.Avatar,
                    Id = user.UserProfile.Id,
                    Name = user.UserProfile.FirstName + " " + user.UserProfile.SecondName
                });
            }

            return new GroupDTO()
            {
                GroupName = group.GroupName,
                Id = group.Id,
                GroupDescription = group.GroupDescription,
                GroupMembers = userProfileDTOs,
                GroupPosts = groupPostDTOs,
                GroupCreatorId = group.Admin,
                //   GroupCreatorId = group.GroupCreatorId,
                Admin = Database.UserManager.FindById(group.Admin).UserProfile.FirstName + " " + Database.UserManager.FindById(group.Admin).UserProfile.SecondName,
                Avatar = group.Avatar
            };
        }
        public byte[] GetDefaultAvatar()
        {
            var image = File.OpenRead("GroupDefaultAvatar.jpg");
            byte[] imageData = null;
            // считываем переданный файл в массив байтов
            using (var binaryReader = new BinaryReader(image))
            {
                imageData = binaryReader.ReadBytes((int)image.Length);
            }
            return imageData;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void AddUser(string userId, int groupId)
        {
          //  Database.GroupRepository.AddToGroup();
            Database.UserManager.FindById(userId).Groups.Add(Database.GroupRepository.GetGroupById(groupId));
            Database.UserProfileManager.SaveChanges();
           // throw new NotImplementedException();
        }

        public void AddPost(GroupPostDTO post)
        {
            Database.GroupRepository.GetGroupById(post.GroupId).GroupPosts.Add(new GroupPost()
            {
                authorId = post.authorId,
                Content = post.Content,
                GroupId = post.GroupId,
                PostDate = post.PostDate
            });
            Database.UserProfileManager.SaveChanges();
            //Database.UserProfileManager.GetUserById(post.UserId).ApplicationUser.Posts.Add(postToDb);
            //Database.UserProfileManager.SaveChanges();

      //      Database.PostRepository.Add(postToDb);
        }

        public void DeleteUserFromGroup(string userId, int groupId)
        {
            Database.UserManager.FindById(userId).Groups.Remove(Database.GroupRepository.GetGroupById(groupId));
            Database.UserProfileManager.SaveChanges();
        }

        public void UpdateGroupInformation(GroupDTO groupDTO)
        {
            Group group = new Group()
            {
                Id = groupDTO.Id,
                GroupDescription = groupDTO.GroupDescription,
                GroupName = groupDTO.GroupName,
                Avatar = groupDTO.Avatar
            };

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
            groupName.Trim();
            List<GroupDTO> groupsDTO = new List<GroupDTO>();
            var groups = Database.GroupRepository.GetGroupsByName(groupName).ToList();
            foreach (var group in groups)
            {
                if (group.GroupName.Trim() == groupName)
                {
                    groupsDTO.Add(new GroupDTO()
                    {
                        Id = group.Id,
                        GroupName = group.GroupName,
                        GroupDescription = group.GroupDescription,
                        //   GroupMembers = group.ApplicationUsers.ToList(),
                        //   GroupCreatorId = group.GroupCreatorId,
                        Admin = Database.UserManager.FindById(group.Admin).UserProfile.FirstName + " " + Database.UserManager.FindById(group.Admin).UserProfile.SecondName,
                        Avatar = group.Avatar
                    });
                }
            }
            return groupsDTO;
        }        
    }
}
