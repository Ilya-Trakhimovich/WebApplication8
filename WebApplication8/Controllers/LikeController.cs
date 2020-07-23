using AppBLL.DataTransferObject;
using AppBLL.Interfaces;
using AppBLL.Services;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication8.Controllers
{
    public class LikeController : Controller
    {
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
        private ILikeService _likeService
        {
            get
            {
                ServiceCreator serviceCreator = new ServiceCreator();
                var likeService = serviceCreator.CreateLikeService("XConnection");
                return likeService;
                // return HttpContext.GetOwinContext().GetUserManager<IPostService>();
            }
        }
        // GET: Like
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Like(int postId)
        {
            // var post = _postService.GetAllPosts(User.Identity.GetUserId()).Where(x => x.Id == postId).FirstOrDefault();
            var post = _postService.GetPostById(postId);

            if (post.Likes.Where(x => x.UserId == User.Identity.GetUserId()).ToList().Count < 1)
            {
                _likeService.AddLikeToPost(new PostLikeDTO()
                {
                    PostId = postId,
                    UserId = User.Identity.GetUserId()
                });
            }
            else
            {
                _likeService.DislikePost(new PostLikeDTO()
                {
                    PostId = postId,
                    UserId = User.Identity.GetUserId()
                });
            }
            return RedirectToAction($"Index/{_postService.GetPostById(postId).UserPageId}", "Home");
        }
    }
}