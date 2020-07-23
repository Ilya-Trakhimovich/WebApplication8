using AppBLL.DataTransferObject;
using AppBLL.Interfaces;
using AppBLL.Services;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using WebApplication8.Models;

namespace WebApplication8.Controllers
{
    [Authorize]
    public class GroupController : Controller
    {
        public Guid myGroup = new Guid();
        private IGroupService _groupService
        {
            get
            {
                ServiceCreator serviceCreator = new ServiceCreator();
                var groupService = serviceCreator.CreateGroupService("XConnection");
                return groupService;
            }
        }

        private IUserService _userService
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<IUserService>();
            }
        }

        public ActionResult CreateGroup()
        {
            return View("CreateGroup");
        }

        public ActionResult GroupAdd(GroupViewModel groupViewModel)
        {
            GroupDTO groupDTO = new GroupDTO();
            if (ModelState.IsValid)
            {
                //  groupDTO.Id = groupViewModel.Id;
                groupDTO.GroupName = groupViewModel.GroupName;
                groupDTO.GroupDescription = groupViewModel.GroupDescription;
                groupDTO.GroupCreatorId = User.Identity.GetUserId();
                _groupService.Create(groupDTO);

                return RedirectToAction("Groups", "Group");
            }
            return View("CreateGroup", groupViewModel);
        }

        public ActionResult MyGroups(string searchText)
        {
            List<GroupViewModel> groups = new List<GroupViewModel>();
            var groupsDTO = _userService.GetUserGroups(User.Identity.GetUserId());

            if (!searchText.IsNullOrWhiteSpace())
                groupsDTO = groupsDTO.Where(x => x.GroupName.ToUpper().Contains(searchText.ToUpper()));

            groupsDTO.ToList();

            foreach (var groupDTO in groupsDTO)
            {
                groups.Add(new GroupViewModel()
                {
                    Avatar = groupDTO.Avatar,
                    Id = groupDTO.Id,
                    GroupName = groupDTO.GroupName,
                    GroupDescription = groupDTO.GroupDescription
                });
            }

            return View(groups);
        }

        public ActionResult Groups(string searchText)
        {
            List<GroupViewModel> groupModels = new List<GroupViewModel>();
            var groupsDTO = _groupService.GetAllGroups();

            if (!searchText.IsNullOrWhiteSpace())
                groupsDTO = groupsDTO.Where(x => x.GroupName.ToUpper().Contains(searchText.ToUpper()));

            groupsDTO.ToList();

            if (groupsDTO != null)
            {
                foreach (var groupDTO in groupsDTO)
                {
                    groupModels.Add(new GroupViewModel()
                    {
                        Id = groupDTO.Id,
                        GroupName = groupDTO.GroupName,
                        GroupDescription = groupDTO.GroupDescription,
                        Admin = groupDTO.Admin,
                        Avatar = groupDTO.Avatar

                    });
                }
                return View(groupModels);
            }
            else
            {
                ViewBag.Message = "No groups";

                return View();
            }
        }

        public ActionResult Group(int id)
        {
            string myId = User.Identity.GetUserId();
            List<UserViewModel> userViewModels = new List<UserViewModel>();
            var groupDTO = _groupService.GetGroupById(id);
            List<PostViewModel> groupPosts = new List<PostViewModel>();

            foreach (var postDTO in groupDTO.GroupPosts)
            {
                groupPosts.Add(new PostViewModel()
                {
                    Content = postDTO.Content,
                    PostDate = postDTO.PostDate,
                    Author = _userService.GetFullName(postDTO.authorId),
                    Avatar = _userService.GetAvatar(postDTO.authorId)
                });
            }
            groupPosts.Reverse();

            foreach (var member in groupDTO.GroupMembers)
            {
                userViewModels.Add(new UserViewModel()
                {
                    Id = member.Id,
                    Name = member.Name,
                    Avatar = member.Avatar
                });
            }
            var viewModel = new GroupViewModel()
            {
                GroupName = groupDTO.GroupName,
                Id = groupDTO.Id,
                GroupCreatorId = groupDTO.GroupCreatorId,
                GroupDescription = groupDTO.GroupDescription,
                GroupMembers = userViewModels,
                GroupPosts = groupPosts,
                Admin = groupDTO.Admin,
                Avatar = groupDTO.Avatar

            };

            ViewBag.IsMember = (viewModel.GroupMembers.Where(x => x.Id == myId).FirstOrDefault() == null) ? false : true;
            ViewBag.IsGroupAdmin = viewModel.GroupCreatorId == User.Identity.GetUserId() ? true : false;

            return View(viewModel);
        }

        public ActionResult SaveGroupInformation(GroupViewModel group, HttpPostedFileBase uploadImage)
        {
            if (ModelState.IsValid)
            {
                GroupDTO groupDTO = new GroupDTO();

                groupDTO = new GroupDTO()
                {
                    Id = group.Id,
                    GroupDescription = group.GroupDescription,
                    GroupName = group.GroupName
                };

                if (uploadImage != null)
                {
                    byte[] imageData = null;
                    // считываем переданный файл в массив байтов
                    using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                    }
                    groupDTO.Avatar = imageData;
                }

                _groupService.UpdateGroupInformation(groupDTO);

                return RedirectToAction($"Group/{group.Id}", "Group");
            }
            return View("ChangeGroupInformation", group);
        }

        public ActionResult ChangeGroupButton(int id)
        {
            return RedirectToAction($"ChangeGroupInformation/{id}", "Group");
        }


        public ActionResult ChangeGroupInformation(int id)
        {
            if (_groupService.GetAdmin(id) != User.Identity.GetUserId())
                return RedirectToAction("Index/1", "Home");

            var group = _groupService.GetGroupById(id);
            GroupViewModel groupViewModel = new GroupViewModel()
            {
                GroupName = group.GroupName,
                GroupDescription = group.GroupDescription,
                Avatar = group.Avatar
            };

            return View(groupViewModel);
        }

        [HttpPost]
        public ActionResult InviteToGroup(int id)
        {
            _groupService.AddUser(User.Identity.GetUserId(), id);
            return RedirectToAction($"Group/{id}", "Group");
        }

        [HttpGet]
        public ActionResult InviteToGroup()
        {
            return RedirectToAction("Index/1", "Home");
        }

        [HttpPost]
        public ActionResult DeleteUserFromGroup(int id)
        {
            _groupService.DeleteUserFromGroup(User.Identity.GetUserId(), id);
            return RedirectToAction($"Group/{id}", "Group");
        }

        public ActionResult AddPostToGroup(string postContent, int groupId)
        {
            GroupPostDTO groupDTO = new GroupPostDTO()
            {
                authorId = User.Identity.GetUserId(),
                Content = postContent,
                GroupId = groupId,
                PostDate = DateTime.Now
            };

            _groupService.AddPost(groupDTO);

            return RedirectToAction($"Group/{groupId}", "Group");
        }

        public ActionResult GetGroupsByName(string groupName)
        {
            if (groupName == null)
            {
                return View("GroupSearchView");
            }
            var groupsDTO = _groupService.GetGroupsByName(groupName).ToList();
            List<GroupViewModel> groupModels = new List<GroupViewModel>();
            if (groupsDTO != null && groupsDTO.Count != 0)
            {
                foreach (var groupDTO in groupsDTO)
                {
                    groupModels.Add(new GroupViewModel()
                    {
                        Id = groupDTO.Id,
                        GroupName = groupDTO.GroupName,
                        GroupDescription = groupDTO.GroupDescription,
                        Admin = groupDTO.Admin,
                        Avatar = groupDTO.Avatar
                    });
                }
                return View("GroupSearchView", groupModels);
            }
            else
            {
                return View("GroupSearchView");
            }
        }
    }
}