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
using WebApplication8.SignalRHubs;

namespace WebApplication8.Controllers
{
    public class LikeController : Controller
    {
        ServiceCreator serviceCreator = new ServiceCreator();
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
        private ILikeService _likeService
        {
            get
            {
                return serviceCreator.CreateLikeService();
                // return HttpContext.GetOwinContext().GetUserManager<IPostService>();
            }
        }

        // GET: Like
        public ActionResult Index() => View();

        public ActionResult Like(int postId)
        {
            // var post = _postService.GetAllPosts(User.Identity.GetUserId()).Where(x => x.Id == postId).FirstOrDefault();
            var post = _postService.GetPostById(postId);

            if (post
                .Likes
                .Where(x => x.UserId == User.Identity.GetUserId())
                .ToList()
                .Count < 1)
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
            SendLikeMessage("Ваша публикация понравилась пользователю", post.UserPageId);

            return RedirectToAction($"Index/{_postService.GetPostById(postId).UserPageId}", "Home");
        }

        private void SendLikeMessage(string message, string userid)
        {
            // Получаем контекст хаба
            var context =
                Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            // отправляем сообщение
            context.Clients.All.displayMessage(message);

          //  context.Clients.Users()
        //    context.Clients.All.displayMessage(message);
        }
    }
}