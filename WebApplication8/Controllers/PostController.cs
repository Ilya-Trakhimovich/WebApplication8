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
        readonly private IPostService _postService;
        readonly private ServiceCreator _service = new ServiceCreator();

        public Func<string> GetUserId;

        public PostController()
        {
            _postService = _service.CreatePostService();
            GetUserId = () => User.Identity.GetUserId();
        }

        public PostController(IServiceCreator service)
        {
            _postService = service.CreatePostService();
            GetUserId = () => User.Identity.GetUserId();
        }

        public ActionResult Add(string postContent, string Id)
        {
            if (postContent is null)
            {
                throw new ArgumentNullException();
            }

            if (Id is null)
            {
                throw new ArgumentNullException();
            }

            if (ModelState.IsValid)
            {
                PostDTO postDTO = new PostDTO
                {
                    Content = postContent,
                    PostDate = DateTime.Now,
                    UserId = GetUserId(),
                    UserPageId = Id
                };

                _postService.AddPost(postDTO);
            }

            return RedirectToAction($"Index/{Id}", "Home");
        }

        public ActionResult GroupPostDelete(int groupId, int groupPostId)
        {
            _postService.DeleteGroupPost(groupPostId);

            return RedirectToAction($"Group/{groupId}", "Group");
        }
    }
}