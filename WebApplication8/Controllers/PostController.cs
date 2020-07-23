using AppBLL.DataTransferObject;
using AppBLL.Interfaces;
using AppBLL.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication8.Controllers
{
    public class PostController : Controller
    {
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

        public ActionResult Add(string postContent, string Id)
        {
            if (ModelState.IsValid)
            {
                PostDTO postDTO = new PostDTO();
                postDTO.Content = postContent;
                postDTO.PostDate = DateTime.Now;
                postDTO.UserId = User.Identity.GetUserId();
                postDTO.UserPageId = Id;

                _postService.AddPost(postDTO);
            }

            return RedirectToAction($"Index/{Id}", "Home");
        }
    }
}