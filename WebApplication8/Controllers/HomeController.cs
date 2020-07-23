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
using WebApplication8.Models;

namespace WebApplication8.Controllers
{
    public class HomeController : Controller
    {
        // IUserService _userService = new UserService();

        //HomeController(IUserService service)
        //{
        //    _userService = service;
        //}

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
                ServiceCreator serviceCreator = new ServiceCreator();
                var postService = serviceCreator.CreatePostService("XConnection");
                return postService;
                // return HttpContext.GetOwinContext().GetUserManager<IPostService>();
            }
        }

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
            var claimsIdentity = User.Identity as ClaimsIdentity;

            if (claimsIdentity != null)
            {
                // the principal identity is a claims identity.
                // now we need to find the NameIdentifier claim
                var userIdClaim = claimsIdentity.Claims
                    .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

                if (userIdClaim != null)
                {
                    var profileInformation = _userService.GetProfileInformation(userIdClaim.Value);

                    var config = new MapperConfiguration(cfg => cfg.CreateMap<ProfileInformationDTO, EditProfileViewModel>());
                    var mapper = new Mapper(config);
                    var editProfileViewModel = mapper.Map<EditProfileViewModel>(profileInformation);

                    return View(editProfileViewModel);

                }
            }

            return RedirectToAction("Register", "Account");
        }

        //[Authorize]
        //public ActionResult Index() // ToDo Поменять вью модель на вью модель с профилем для полного отображения информации профиля
        //{
        //    var UserDetails = _userService.GetUserDetails(User.Identity.GetUserId());
        //    var postsDTO = _postService.GetAllPosts(User.Identity.GetUserId());

        //    var config = new MapperConfiguration(cfg => cfg.CreateMap<PostDTO, PostViewModel>());
        //    var mapper = new Mapper(config);

        //    var posts = mapper.Map<List<PostViewModel>>(postsDTO);

        //    UserViewModel userViewModel = new UserViewModel()
        //    {
        //        Age = UserDetails.Age,
        //        Name = UserDetails.Name,
        //        Posts = posts
        //    };

        //    //return RedirectToAction("Index", "Home", new { @id = id });
        //}


        [Authorize]
        public ActionResult Index(string id)
        {
            ViewBag.MyId = User.Identity.GetUserId();

            if (!_userService.IsUserExist(id))
                id = User.Identity.GetUserId();

            var UserDetails = _userService.GetProfileInformation(id);
            var postsDTO = _postService.GetAllPosts(id);
            var config = new MapperConfiguration(cfg => cfg.CreateMap<PostDTO, PostViewModel>());
            var mapper = new Mapper(config);
            var posts = mapper.Map<List<PostViewModel>>(postsDTO);

            posts.Reverse();

            UserViewModel userViewModel = new UserViewModel()
            {
                Age = UserDetails.Age,
                AboutMe = UserDetails.AboutMe,
                Address = UserDetails.Address,
                Education = UserDetails.Education,
                Gender = UserDetails.Gender,
                Id = id,
                Name = UserDetails.FirstName + " " + UserDetails.SecondName,
                Avatar = UserDetails.Avatar,
                Posts = posts
            };

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

        public ActionResult Users(string searchText)
        {
            var users = _userService.GetAllUsers(); // users type is List<UserProfileDTO>

            if (!String.IsNullOrEmpty(searchText))
                users = users.Where(x => x.Name.Contains(searchText)).ToList();

            return View(users);
        }

        public ActionResult GetUsersByName(string userName)
        {
            var users = _userService.GetUsersByName(userName);

            return View("Users", users);
        }
    }
}