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
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AppBLL.Services
{
    public class UserService : IUserService
    {
        IUnitOfWork Database { get; set; }

        public UserService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public async Task<OperationDetails> Create(UserDTO userDto)
        {
            ApplicationUser user = await Database.UserManager.FindByEmailAsync(userDto.Email);
            if (user == null)
            {
                user = new ApplicationUser { Email = userDto.Email, UserName = userDto.Email };
                var result = await Database.UserManager.CreateAsync(user, userDto.Password);

                if (result.Errors.Count() > 0)
                    return new OperationDetails(false, result.Errors.FirstOrDefault(), "");

                // добавляем роль
                await Database.UserManager.AddToRoleAsync(user.Id, userDto.Role);

                // создаем профиль клиента
                UserProfile userProfile = new UserProfile { Id = user.Id, Avatar = userDto.Avatar, FirstName = userDto.FirstName, SecondName = userDto.SecondName };

                Database.UserProfileManager.Create(userProfile);

                await Database.SaveAsync();

                return new OperationDetails(true, "Регистрация успешно пройдена", "");
            }
            else
            {
                return new OperationDetails(false, "Пользователь с таким логином уже существует", "Email");
            }
        }

        public async Task<ClaimsIdentity> Authenticate(UserDTO userDto)
        {
            ClaimsIdentity claim = null;

            // находим пользователя
            ApplicationUser user = await Database.UserManager.FindAsync(userDto.Email, userDto.Password);

            // авторизуем его и возвращаем объект ClaimsIdentity
            if (user != null)
                claim = await Database.UserManager.CreateIdentityAsync(user,
                                            DefaultAuthenticationTypes.ApplicationCookie);

            return claim;
        }

        // начальная инициализация бд
        public async Task SetInitialData(UserDTO adminDto, List<string> roles)
        {
            //foreach (string roleName in roles)
            //{
            //    var role = await Database.RoleManager.FindByNameAsync(roleName);

            //    if (role == null)
            //    {
            //        role = new ApplicationRole { Name = roleName };

            //        await Database.RoleManager.CreateAsync(role);
            //    }
            //}
            //await Create(adminDto);
        }

        public void SaveProfileInformationById(string id, ProfileInformationDTO informationDTO)
        {
            var user = Database.UserManager.FindById(id);

            user.UserProfile.AboutMe = informationDTO.AboutMe;
            user.UserProfile.Address = informationDTO.Address;
            user.UserProfile.Age = informationDTO.Age;
            user.UserProfile.Avatar = informationDTO.Avatar;
            user.UserProfile.Gender = informationDTO.Gender;
            user.UserProfile.FirstName = informationDTO.FirstName;
            user.UserProfile.SecondName = informationDTO.SecondName;
            user.UserProfile.Education = informationDTO.Education;

            Database.UserManager.Update(user);
            Database.UserProfileManager.SaveChanges();
        }

        public UserProfileDTO GetUserDetails(string id)
        {
            var profileManager = Database.UserProfileManager;

            var profile = profileManager.GetUserById(id);

            return new UserProfileDTO()
            {
                Name = profile.FirstName + " " + profile.SecondName,
                Age = profile.ApplicationUser.UserProfile.Age
            };
        }

        public List<UserProfileDTO> GetAllUsers()
        {
            var users = Database.UserProfileManager.GetAllUsers();

            List<UserProfileDTO> userProfileDTOs = new List<UserProfileDTO>();

            foreach (var user in users)
            {
                userProfileDTOs.Add(new UserProfileDTO()
                {
                    Id = user.Id,
                    Age = user.Age,
                    Avatar = user.Avatar,
                    Name = user.FirstName + " " + user.SecondName
                });
            }

            return userProfileDTOs;
        }
        public List<UserProfileDTO> GetUsersByName(string userName)
        {
            var users = Database.UserProfileManager.GetAllUsers().Where(x => GetFullName(x.Id).Contains(userName));
            List<UserProfileDTO> usersProfileDTO = new List<UserProfileDTO>();

            foreach (var user in users)
            {
                usersProfileDTO.Add(new UserProfileDTO()
                {
                    Id = user.Id,
                    Age = user.Age,
                    Avatar = user.Avatar,
                    Name = GetFullName(user.Id)
                });
            }

            return usersProfileDTO;
        }

        public string GetFullName(string id)
        {
            return Database.UserManager.FindById(id).UserProfile.FirstName + " " +
                Database.UserManager.FindById(id).UserProfile.SecondName;
        }

        public ProfileInformationDTO GetProfileInformation(string id)
        {
            var profileManager = Database.UserProfileManager;

            var profile = profileManager.GetUserById(id);

            return new ProfileInformationDTO()
            {
                FirstName = profile.FirstName,
                SecondName = profile.SecondName,
                Age = profile.Age,
                Avatar = profile.Avatar,
                Address = profile.Address,
                Gender = profile.Gender,
                Education = profile.Education,
                AboutMe = profile.AboutMe

            };
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        public bool IsUserExist(string id)
        {
            if (Database.UserManager.FindById(id) != null)
                return true;

            return false;
        }

        UserProfileDTO IUserService.GetUserDetails(string id)
        {
            var profileManager = Database.UserProfileManager;
            var profile = profileManager.GetUserById(id);

            return new UserProfileDTO()
            {
                Name = profile.FirstName + " " + profile.SecondName,
                Age = profile.Age,
                Avatar = profile.Avatar
            };
        }

        public byte[] GetAvatar(string id)
        {
            return Database.UserManager.FindById(id).UserProfile.Avatar;
        }

        public byte[] GetDefaultAvatar()
        {
            var image = File.OpenRead("default.jpg");
            byte[] imageData = null;

            // считываем переданный файл в массив байтов
            using (var binaryReader = new BinaryReader(image))
            {
                imageData = binaryReader.ReadBytes((int)image.Length);
            }

            return imageData;
        }

        public IEnumerable<GroupDTO> GetUserGroups(string userid)
        {
            var usergroups = Database.UserManager.FindById(userid).Groups.ToList();

            List<GroupDTO> userGroups = new List<GroupDTO>();

            foreach (var group in usergroups)
            {
                userGroups.Add(new GroupDTO()
                {
                    Avatar = group.Avatar,
                    GroupDescription = group.GroupDescription,
                    GroupName = group.GroupName,
                    Id = group.Id
                });
            }
            return userGroups;
        }
    }
}

