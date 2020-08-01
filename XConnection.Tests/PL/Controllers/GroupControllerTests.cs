using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using AppBLL.DataTransferObject;
using AppBLL.Interfaces;
using AppBLL.Services;
using DataAcess.Entities;
using DataAcess.Interfaces;
using DataAcess.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebApplication8.Controllers;
using WebApplication8.Models;

namespace XConnection.Tests.PL.Controllers
{
    [TestClass]
    public class GroupControllerTests
    {
        [TestMethod]
        public void CreateGroup_CreateGroupViewEqualsCreateGroupCshtml_Returns_CorrectView()
        {
            //Arrange
            GroupController groupController = new GroupController();

            //Act
            var result = groupController.CreateGroup() as ViewResult;

            //Assert
            Assert.AreEqual("CreateGroup", result.ViewName);
        }

        [TestMethod]
        public void GroupAdd_ModelIsNotValid_Returns_View()
        {
            //Arrange
            var expectedView = "CreateGroup";
            GroupController groupController = new GroupController();
            var groupViewModel = new GroupViewModel();
            groupController.ModelState.AddModelError("", "Error");

            //Act
            var result = groupController.GroupAdd(groupViewModel) as ViewResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedView, result.ViewName);
        }

        //[TestMethod]
        //public void GroupAdd_ModelIsValid_Returns_RedirectToAction()
        //{
        //    // Arrange         
        //    var groupViewModel = new GroupViewModel() { Id = 1};
        //    var groupDTO = new GroupDTO();
        //    var group = new Group() { ApplicationUsers = new List<ApplicationUser>() { new ApplicationUser() {  UserProfile = new UserProfile() } } };
        //    var expectedView = "Groups";

        //    var mock = new Mock<IServiceCreator>();
        //    var mockUOW = new Mock<IUnitOfWork>();

        //    mockUOW.Setup(x => x.GroupRepository.Create(group));
        //    mock.Setup(x => x.CreateGroupService()).Returns(new GroupService(mockUOW.Object));

        //    var groupController = new GroupController(mock.Object)
        //    {
        //        GetUserIdIdentity = () => "12",
        //    };

        //    //Act
        //    var result = groupController.GroupAdd(groupViewModel) as RedirectToRouteResult;

        //    //Assert
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(expectedView, result.RouteValues["action"]);
        //}

        //[TestMethod]
        //public void Groups_InputDataIsCorrect_Returns_ViewGroups()
        //{
            //Arrange
        //    var expected = "Groups";
        //    GroupController groupController = new GroupController();
        //    var inputData = "123";
        //    var postServiceMock = new Mock<IPostService>();
        //    var userServiceMock = new Mock<IUserService>();
        //    var groupServiceMock = new Mock<IGroupService>();
        //    var groupDTO = new List<GroupDTO>();

        //    groupServiceMock.Setup(x => x.GetAllGroups()).Returns(groupDTO);

        //    //Act
        //    var result = groupController.Groups(inputData) as ViewResult;

        //    //Assert
        //    Assert.AreEqual(expected, result.ViewName);
        //}

        [TestMethod]
        public void SaveGroupInformation_ModelIsNotValid_Returns_View()
        {
            // Arrange
            var expectedView = "ChangeGroupInformation";
            GroupController groupController = new GroupController();
            var groupViewModel = new GroupViewModel();
            HttpPostedFileBase uploadImage = null;
            groupController.ModelState.AddModelError("", "Error");

            //Act
            var result = groupController.SaveGroupInformation(groupViewModel, uploadImage) as ViewResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedView, result.ViewName);
        }

        //[TestMethod]
        //public void SaveGroupInformation_ModelIsValid_Returns_RedirectToAction()
        //{
        //    // Arrange         
        //    var groupViewModel = new GroupViewModel() {Id = 1};
        //    HttpPostedFileBase uploadImage = null;
        //    var groupDTO = new GroupDTO();
        //    var group = new Group();
        //    var expectedView = "Group/1";

        //    var mock = new Mock<IServiceCreator>();
        //    var mockUOW = new Mock<IUnitOfWork>();

        //    mockUOW.Setup(x => x.GroupRepository.UpdateGroupInformation(group));
        //    mock.Setup(x => x.CreateGroupService()).Returns(new GroupService(mockUOW.Object));

        //    var groupController = new GroupController(mock.Object);

        //    //Act
        //    var result = groupController.SaveGroupInformation(groupViewModel, uploadImage) as RedirectToRouteResult;

        //    //Assert
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(expectedView, result.RouteValues["action"]);
        //}

        //[TestMethod]
        //public void GetGroupsByName_InputDataIsNull_Returns_View()
        //{
        //    // Arrange
        //    var expectedView = "GroupSearchView";
        //    string inputName = null;
        //    var groupController = new GroupController();

        //    //Act
        //    var result = groupController.GetGroupsByName(inputName) as ViewResult;

        //    //Assert
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(expectedView, result.ViewName);
        //}
    }
}
