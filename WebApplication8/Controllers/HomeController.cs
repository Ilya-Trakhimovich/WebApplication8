using AppBLL.DataTransferObject;
using AppBLL.Interfaces;
using AppBLL.Services;
using AutoMapper;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using WebApplication8.App_Start;
using WebApplication8.Models;

namespace WebApplication8.Controllers
{
    public class HomeController : Controller
    {
        ServiceCreator serviceCreator = new ServiceCreator();
        MapperConfigs mapperConfigs = new MapperConfigs();

        private IUserService _userService
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<IUserService>();
            }
        }

        private IPostService _postService
        {
            get
            {
                return serviceCreator.CreatePostService();
                // return HttpContext.GetOwinContext().GetUserManager<IPostService>();
            }
        }

        public ActionResult Users() => View(_userService.GetAllUsers());

        public ActionResult AvatarAdd(HttpPostedFileBase uploadImage)
        {
            if (ModelState.IsValid && uploadImage != null)
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index/1", "Home");
        }

        [Authorize]
        public ActionResult ChangeProfileInformation()
        {
            if (User.Identity.GetUserId() != null)
            {
                var profileInformation = _userService.GetProfileInformation(User.Identity.GetUserId());

                var mapper = new Mapper(mapperConfigs.ProfileInformationDtoToEditProfileViewModel);
                var editProfileViewModel = mapper.Map<EditProfileViewModel>(profileInformation);

                return View(editProfileViewModel);
            }
            else
                return RedirectToAction("Register", "Account");
        }

        [Authorize]
        public ActionResult Index(string id)
        {
            ViewBag.MyId = User.Identity.GetUserId();

            if (!_userService.IsUserExist(id))
                id = User.Identity.GetUserId();

            var UserDetails = _userService.GetProfileInformation(id);
            var postsDTO = _postService.GetAllPosts(id);

            var mapper = new Mapper(mapperConfigs.PostDtoToPostViewModel);
            var posts = mapper.Map<List<PostViewModel>>(postsDTO);

            posts.Reverse();

            var mapper1 = new Mapper(mapperConfigs.ProfileInformationDtoToUserViewModel);
            var userViewModel = mapper1.Map<UserViewModel>(UserDetails);
            userViewModel.Id = id;
            userViewModel.Posts = posts;

            return View(userViewModel);
        }

        [Authorize(Roles = "admin")]
        public ActionResult ShowPost()
        {
            var posts = _postService.GetAllPosts(User.Identity.GetUserId());

            return View(posts);
        }

        public ActionResult PostDelete(int postId)
        {
            _postService.DeletePost(postId);

            return RedirectToAction("Index/1", "Home");
        }

        public ActionResult SaveInformationForm(EditProfileViewModel profileInformation, HttpPostedFileBase uploadImage)
        {
            if (profileInformation.Age < 0 || profileInformation.Age > 120)
            {
                ModelState.AddModelError("Age", "Недопустимый возраст");
            }

            if (ModelState.IsValid)
            {
                MapperConfiguration config;
                ProfileInformationDTO profileInformationDTO = new ProfileInformationDTO();

                if (uploadImage != null)
                {
                    config = new MapperConfiguration(cfg => cfg.CreateMap<EditProfileViewModel, ProfileInformationDTO>());
                    byte[] imageData = null;
                    // считываем переданный файл в массив байтов
                    using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                    }
                    profileInformation.Avatar = imageData;
                    var mapper = new Mapper(config);
                    profileInformationDTO = mapper.Map<ProfileInformationDTO>(profileInformation);
                }
                else
                {
                    config = new MapperConfiguration(cfg => cfg.CreateMap<EditProfileViewModel, ProfileInformationDTO>().ForMember(x => x.Avatar, (options) => options.Ignore()));
                    var mapper = new Mapper(config);
                    profileInformationDTO = mapper.Map<ProfileInformationDTO>(profileInformation);
                    profileInformationDTO.Avatar = _userService.GetAvatar(User.Identity.GetUserId());
                }

                _userService.SaveProfileInformationById(User.Identity.GetUserId(), profileInformationDTO);
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult UserSearch(string name)
        {
            var users = _userService.GetAllUsers(); // users type is List<UserProfileDTO>

            if (!String.IsNullOrEmpty(name))
                users = users.Where(x => x.Name.ToUpper().Contains(name.ToUpper())).ToList();

            return PartialView(users);
        }
    }
}