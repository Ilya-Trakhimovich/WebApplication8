using System;
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

namespace XConnection.Tests.PL.Controllers
{
    [TestClass]
    public class PostControllerTests
    {
        [TestMethod]
        public void Add_ModelIsNotValid_Returns_RedirectToAction()
        {
            //Arrange
            var inputPostContent = "123";
            var inputId = "1";
            var expectedView = "Index/1";

            var mock = new Mock<IServiceCreator>();
            mock.Setup(x => x.CreatePostService());

            var postController = new PostController(mock.Object);

            postController.ModelState.AddModelError("", "Error");

            //Act
            var result = postController.Add(inputPostContent, inputId) as RedirectToRouteResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedView, result.RouteValues["action"]);
        }

        [TestMethod]
        public void Add_ModelIsValid_AddPost_Ok_()
        {
            //Arrange
            var inputPostContent = "123";
            var inputId = "1";

            var postDTO = new PostDTO()
            {
                Content = "abcd",
                PostDate = DateTime.Now,
                UserId = "12",
                UserPageId = inputId
            };

            var post = new Post()
            {
                Content = "abcd",
                PostDate = DateTime.Now,
                UserPageId = inputId,
                ApplicationUser = new ApplicationUser() { UserProfile = new UserProfile() }
            };

            var mockUOW = new Mock<IUnitOfWork>();
            var mockService = new Mock<IServiceCreator>();
            var mockPostService = new Mock<IPostService>();

            mockPostService.Setup(x => x.AddPost(It.IsAny<PostDTO>()));
            mockUOW.Setup(x => x.PostRepository.Add(post));
            mockService.Setup(x => x.CreatePostService()).Returns(mockPostService.Object);            

            var postController = new PostController(mockService.Object)
            {
                GetUserId = () => "12"
            };

            //Act
            var result = postController.Add(inputPostContent, inputId) as RedirectToRouteResult;

            //Assert
            mockPostService.Verify(x => x.AddPost(It.IsAny<PostDTO>()), Times.AtLeastOnce());
        }

        [TestMethod]
        public void Add_ModelIsVAlid_Returns_RedirectToAction()
        {
            //Arrange
            var inputPostContent = "123";
            var inputId = "1";
            var expected = "Index/1";

            var postDTO = new PostDTO()
            {
                Content = "abcd",
                PostDate = DateTime.Now,
                UserId = "12",
                UserPageId = inputId
            };

            var post = new Post()
            {
                Content = "abcd",
                PostDate = DateTime.Now,
                UserPageId = inputId,
                ApplicationUser = new ApplicationUser() { UserProfile = new UserProfile() }
            };

            var mockUOW = new Mock<IUnitOfWork>();
            var mockService = new Mock<IServiceCreator>();

            mockUOW.Setup(x => x.PostRepository.Add(post));
            mockService.Setup(x => x.CreatePostService()).Returns(new PostService(mockUOW.Object));

            var postController = new PostController(mockService.Object)
            {
                GetUserId = () => "12"
            };

            //Act
            var result = postController.Add(inputPostContent, inputId) as RedirectToRouteResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.RouteValues["action"]);
        }

        [TestMethod]
        public void Add_InputDataIsNull_Throws()
        {
            //Arrange
            string inputPostContent = null;
            string inputId = null;

            var mock = new Mock<IServiceCreator>();
            mock.Setup(x => x.CreatePostService());

            var postController = new PostController(mock.Object)
            {
                GetUserId = () => "12"
            };

            //Act
 
            //Assert
            Assert.ThrowsException<ArgumentNullException>(() => postController.Add(inputPostContent, inputId));
        }
    }
}
