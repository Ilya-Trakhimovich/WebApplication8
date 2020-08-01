using AppBLL.DataTransferObject;
using AppBLL.Interfaces;
using AppBLL.Services;
using AutoMapper;
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
using WebApplication8.App_Start;
using WebApplication8.Models;

namespace WebApplication8.Controllers
{
    [Authorize]
    public class GroupController : Controller
    {
        ServiceCreator serviceCreator = new ServiceCreator();
        MapperConfigs mapperConfigs = new MapperConfigs();


        //ServiceCreator _serviceCreator = new ServiceCreator();
        //IGroupService _groupService;
        //IUserService _userService;
        //IPostService _postService;

        //public Func<string> GetUserId;

        //public GroupController()
        //{
        //    _groupService = _serviceCreator.CreateGroupService();
        //    _userService = _serviceCreator.CreateUserService();
        //    _postService = _serviceCreator.CreatePostService();

        //    GetUserIdIdentity = () => User.Identity.GetUserId();
        //}

        //public GroupController(IServiceCreator _service)
        //{
        //    _groupService = _service.CreateGroupService();
        //    _userService = _service.CreateUserService(); ;
        //    _postService = _service.CreatePostService();

        //    GetUserIdIdentity = () => User.Identity.GetUserId();
        //}

        //public Func<string> GetUserIdIdentity;
        private Mapper _groupViewMapper
        {
            get
            {
                return new Mapper(mapperConfigs.GroupDtoToGroupViewModel);
            }
        }

        private IUserService GetUser()
        {
            return HttpContext.GetOwinContext().GetUserManager<IUserService>();
        }

        private IGroupService _groupService
        {
            get
            {
                return serviceCreator.CreateGroupService();
            }
        }

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
            }
        }

        public ActionResult CreateGroup() => View("CreateGroup");

        public ActionResult ChangeGroupButton(int id) => RedirectToAction($"ChangeGroupInformation/{id}", "Group");

        [HttpGet]
        public ActionResult InviteToGroup() => RedirectToAction("Index/1", "Home");

        public ActionResult GroupAdd(GroupViewModel groupViewModel)
        {
            GroupDTO groupDTO = new GroupDTO();

            if (ModelState.IsValid)
            {
                //groupDTO.Id = groupViewModel.Id;
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
            var groupsDTO = _userService.GetUserGroups(User.Identity.GetUserId());

            if (!searchText.IsNullOrWhiteSpace())
                groupsDTO = groupsDTO.Where(x => x.GroupName.ToUpper().Contains(searchText.ToUpper()));

            groupsDTO.ToList();

            var groups = _groupViewMapper.Map<List<GroupViewModel>>(groupsDTO);

            return View(groups);
        }

        public ActionResult Groups(string searchText)
        {
            List<GroupViewModel> groupModels = new List<GroupViewModel>();
            var groupsDTO = _groupService.GetAllGroups();

            if (!searchText.IsNullOrWhiteSpace())
            {
                groupsDTO = groupsDTO
                    .Where(x => x.GroupName.ToUpper()
                    .Contains(searchText.ToUpper()));
            }

            groupsDTO.ToList();

            if (groupsDTO != null)
            {
                groupModels = _groupViewMapper.Map<List<GroupViewModel>>(groupsDTO);

                return View("Groups", groupModels);
            }
            else
            {
                ViewBag.Message = "No groups";

                return View();
            }
        }

        public ActionResult Group(int id)
        {
            List<GroupPostViewModel> groupPosts = new List<GroupPostViewModel>();

            string myId = User.Identity.GetUserId();
            var groupDTO = _groupService.GetGroupById(id);

            foreach (var postDTO in groupDTO.GroupPosts)
            {
                var groupPostMapper = new Mapper(mapperConfigs.GroupPostDtoToGroupPostViewModel);
                var groupPostViewModel = groupPostMapper.Map<GroupPostViewModel>(postDTO);

                groupPostViewModel.Author = _userService.GetFullName(postDTO.AuthorId);
                groupPostViewModel.Avatar = _userService.GetAvatar(postDTO.AuthorId);

                groupPosts.Add(groupPostViewModel);
            }

            groupPosts.Reverse();

            Mapper userViewMapper = new Mapper(mapperConfigs.UserProfileDtoToUserViewModel);

            var userViewModels = userViewMapper.Map<List<UserViewModel>>(groupDTO.GroupMembers);
            var viewModel = _groupViewMapper.Map<GroupViewModel>(groupDTO);

            viewModel.GroupMembers = userViewModels;
            viewModel.GroupPosts = groupPosts;

            ViewBag.MyId = User.Identity.GetUserId();
            ViewBag.GroupId = id;
            ViewBag.IsMember = (viewModel.GroupMembers.Where(x => x.Id == myId).FirstOrDefault() == null) ? false : true;
            ViewBag.IsGroupAdmin = viewModel.GroupCreatorId == User.Identity.GetUserId() ? true : false;

            return View(viewModel);
        }

        public ActionResult SaveGroupInformation(GroupViewModel group, HttpPostedFileBase uploadImage)
        {
            if (ModelState.IsValid)
            {
                Mapper groupDtoMapper = new Mapper(mapperConfigs.GroupViewModelToGroupDtoWithoutAvatar);
                GroupDTO groupDTO = groupDtoMapper.Map<GroupDTO>(group);

                if (uploadImage != null)
                {
                    byte[] imageData = null;

                    using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                        imageData = binaryReader.ReadBytes(uploadImage.ContentLength);

                    groupDTO.Avatar = imageData;
                }

                _groupService.UpdateGroupInformation(groupDTO);

                return RedirectToAction($"Group/{group.Id}", "Group");
            }

            return View("ChangeGroupInformation", group);
        }

        public ActionResult ChangeGroupInformation(int id)
        {
            if (_groupService.GetAdmin(id) != User.Identity.GetUserId())
                return RedirectToAction("Index/1", "Home");

            var groupViewModel = _groupViewMapper.Map<GroupViewModel>(_groupService.GetGroupById(id));

            return View(groupViewModel);
        }

        [HttpPost]
        public ActionResult UserSearch(string name, int groupId)
        {
            var users = _groupService.
                 GetGroupById(groupId).
                 GroupMembers.
                 Where(x => x.Name.ToUpper().Contains(name.ToUpper())).
                 ToList();

            Mapper userViewMapper = new Mapper(mapperConfigs.UserProfileDtoToUserViewModel);
            var userViewModels = userViewMapper.Map<List<UserViewModel>>(users);

            if (users.Count < 1)
                return HttpNotFound();

            ViewBag.GroupId = groupId;
            ViewBag.IsGroupAdmin = _groupService.GetAdmin(groupId) == User.Identity.GetUserId() ? true : false;
            return PartialView(userViewModels);
        }

        [HttpPost]
        public ActionResult InviteToGroup(int id)
        {
            _groupService.AddUser(User.Identity.GetUserId(), id);

            return RedirectToAction($"Group/{id}", "Group");
        }

        [HttpPost]
        public ActionResult DeleteUserFromGroup(int id)
        {
            _groupService.DeleteUserFromGroup(User.Identity.GetUserId(), id);

            return RedirectToAction($"Group/{id}", "Group");
        }

        [HttpPost]
        public ActionResult DeleteGroupMember(string userId, int groupId)
        {
            _groupService.DeleteUserFromGroup(userId, groupId);

            return RedirectToAction($"Group/{groupId}", "Group");
        }

        public ActionResult AddPostToGroup(string postContent, int groupId)
        {
            GroupPostDTO groupPostDTO = new GroupPostDTO()
            {
                AuthorId = User.Identity.GetUserId(),
                Content = postContent,
                GroupId = groupId,
                PostDate = DateTime.Now
            };

            _postService.AddGroupPost(groupPostDTO);

            return RedirectToAction($"Group/{groupId}", "Group");
        }

        public ActionResult PostDelete(int postId)
        {
            ViewBag.MyId = User.Identity.GetUserId();
            _postService.DeletePost(postId);

            return RedirectToAction("Index/1", "Home");
        }
    }
}