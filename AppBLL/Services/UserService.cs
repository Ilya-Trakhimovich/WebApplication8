using AppBLL.Configs;
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
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AppBLL.Services
{
    public class UserService : IUserService
    {
        IUnitOfWork Database { get; set; }

        readonly MapperConfigs mapperConfigs = new MapperConfigs();

        public UserService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public async Task<OperationDetails> Create(UserDTO userDto)
        {
            ApplicationUser user = await Database
                                        .UserManager
                                        .FindByEmailAsync(userDto.Email);
            
            if (user == null)
            {
                user = new ApplicationUser 
                {
                    Email = userDto.Email, 
                    UserName = userDto.Email 
                };

                var result = await Database
                                  .UserManager
                                  .CreateAsync(user, userDto.Password);

                if (result.Errors.Count() > 0)
                    return new OperationDetails(false, result.Errors.FirstOrDefault(), "");

                // добавляем роль
                await Database.UserManager.AddToRoleAsync(user.Id, userDto.Role);

                // создаем профиль клиента
                UserProfile userProfile = new UserProfile
                {
                    Id = user.Id,
                    Avatar = userDto.Avatar, 
                    FirstName = userDto.FirstName,
                    SecondName = userDto.SecondName 
                };

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
                claim = await Database
                             .UserManager
                             .CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);

            return claim;
        }

        public string GetUserIdByEmail(string mail)
        {
            return Database.UserManager.FindByEmail(mail).Id;
        }

        public void SaveProfileInformationById(string id, ProfileInformationDTO informationDTO)
        {
            var user = Database.UserManager.FindById(id);

            //Mapper userProfileMapper = new Mapper(mapperConfigs.ProfileInformationDtoToUserProfile);
            //user.UserProfile = userProfileMapper.Map<UserProfile>(informationDTO);

            user.UserProfile.AboutMe = informationDTO.AboutMe;
            user.UserProfile.Avatar = informationDTO.Avatar;
            user.UserProfile.Age = informationDTO.Age;
            user.UserProfile.Address = informationDTO.Address;
            user.UserProfile.FirstName = informationDTO.FirstName;
            user.UserProfile.SecondName = informationDTO.SecondName;
            user.UserProfile.Gender = informationDTO.Gender;
            user.UserProfile.Education = informationDTO.Education;


            Database.UserManager.Update(user);
            Database.UserProfileManager.SaveChanges();
        }
       
        public List<UserProfileDTO> GetAllUsers()
        {
            var users = Database.UserProfileManager.GetAllUsers();

            if (users is null)
            {
                throw new Exception();
            }

            Mapper userProfileDtoMapper = new Mapper(mapperConfigs.UserProfileToUserProfileDTO);

            return userProfileDtoMapper.Map<List<UserProfileDTO>>(users);
        }

        public string GetFullName(string id)
        {
            var userProfile = Database
                             .UserManager
                             .FindByIdAsync(id)
                             .Result
                             .UserProfile;

            return $"{userProfile.FirstName} {userProfile.SecondName}";
        }

        public ProfileInformationDTO GetProfileInformation(string id)
        {
            var profileManager = Database.UserProfileManager;
            var profile = profileManager.GetUserById(id);

            if (profile is null)
            {
                throw new Exception();
            }

            Mapper profileInformationDtoMapper = new Mapper(mapperConfigs.UserProfileToProfileInformationDto);

            return profileInformationDtoMapper.Map<ProfileInformationDTO>(profile);
        }

        public void Dispose() => Database.Dispose();

        public bool IsUserExist(string id)
        {
            if (Database.UserManager.FindById(id) != null)
                return true;

            return false;
        }

        public byte[] GetAvatar(string id) => Database.UserManager.FindById(id).UserProfile.Avatar;

        public byte[] GetDefaultAvatar()
        {
            var image = File.OpenRead("default.jpg");
            byte[] imageData = null;

            using (var binaryReader = new BinaryReader(image))
                imageData = binaryReader.ReadBytes((int)image.Length);

            return imageData;
        }

        public IEnumerable<GroupDTO> GetUserGroups(string userid)
        {
            var usergroups = Database.UserManager.FindByIdAsync(userid).Result.Groups;

            if (usergroups is null)
            {
                throw new Exception();
            }

            Mapper groupDtoMapper = new Mapper(mapperConfigs.GroupToGroupDto);

            return groupDtoMapper.Map<List<GroupDTO>>(usergroups);
        }
    }
}

