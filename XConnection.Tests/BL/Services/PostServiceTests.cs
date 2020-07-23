using System;
using System.Collections.Generic;
using AppBLL.DataTransferObject;
using AppBLL.Services;
using DataAcess.Entities;
using DataAcess.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace XConnection.Tests.BL.Services
{
    [TestClass]
    public class PostServiceTests
    {
        //[TestMethod]
        //public void AddPost_()
        //{
        //    // Arrange
        //    var mock = new Mock<IUnitOfWork>();
        //    var testPost = new Post() { };

        //    mock.Setup(x => x.PostRepository.Add(testPost));

        //    // Act
        //    var postDTO = new PostDTO();
        //    var postService = new PostService(mock.Object);
        //    postService.AddPost(postDTO);

        //    //Assert
        //    Assert.Fail();
        //}

        [TestMethod]
        public void GetAllPosts_CkeckTypeOfReturnData_Returns_TypeIsListOfPostDTO()
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            var inputId = "1";
            var expected = new List<PostDTO>();

            mock.Setup(x => x.PostRepository.GetAll(inputId)).Returns(new List<Post>());

            // Act
            var postService = new PostService(mock.Object);
            var result = postService.GetAllPosts(inputId);

            // Assert
            Assert.AreEqual(expected.GetType(), result.GetType());
        }

        [TestMethod]
        public void GetPostById_CkeckTypeOfReturnData_Returns_TypeIsPostDTO()
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            var inputId = 1;
            var expected = new PostDTO();

            mock.Setup(x => x.PostRepository.GetById(inputId)).Returns(new Post() { ApplicationUser = new ApplicationUser() {  UserProfile = new UserProfile()} });

            // Act
            var postService = new PostService(mock.Object);
            var result = postService.GetPostById(inputId);

            // Assert
            Assert.AreEqual(expected.GetType(), result.GetType());
        }
    }
}
