using System;
using System.Collections.Generic;
using System.Linq;
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
        public void GetAllPosts_PostsFromDBIsNull_Throws()
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            var postManagerMock = new Mock<IPostRepository<Post>>();
            var inputId = "123";
            IEnumerable<Post> expected = null;

            mock.Setup(x => x.PostRepository).Returns(postManagerMock.Object);
            postManagerMock.Setup(x => x.GetAll(inputId)).Returns(expected);

            // Act
            var postService = new PostService(mock.Object);

            // Assert
            Assert.ThrowsException<Exception>(() => postService.GetAllPosts(inputId));
        }

        [TestMethod]
        public void GetAllPosts_PostsFromDBAreExist_ReturnGroupDTO()
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            var postManagerMock = new Mock<IPostRepository<Post>>();
            var inputId = "123";
            IEnumerable<Post> expectedPosts = new List<Post>() { new Post() { Id = 1, Content = "WWW", PostLikes = new List<PostLike>(), ApplicationUser = new ApplicationUser() { UserProfile = new UserProfile() }, PostDate = DateTime.Now } };

            mock.Setup(x => x.PostRepository).Returns(postManagerMock.Object);
            postManagerMock.Setup(x => x.GetAll(inputId)).Returns(expectedPosts);

            // Act
            var postService = new PostService(mock.Object);
            var result = postService.GetAllPosts(inputId) as List<PostDTO>;

            // Assert
            Assert.AreEqual(result.FirstOrDefault().Id, expectedPosts.FirstOrDefault().Id);
            Assert.AreEqual(result.FirstOrDefault().Content, expectedPosts.FirstOrDefault().Content);
        }

        [TestMethod]
        public void GetPostById_CkeckTypeOfReturnData_Returns_TypeIsPostDTO()
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            var inputId = 1;
            var expected = new PostDTO();

            mock.Setup(x => x.PostRepository.GetById(inputId)).Returns(new Post() { ApplicationUser = new ApplicationUser() { UserProfile = new UserProfile() } });

            // Act
            var postService = new PostService(mock.Object);
            var result = postService.GetPostById(inputId);

            // Assert
            Assert.AreEqual(expected.GetType(), result.GetType());
        }

        [TestMethod]
        public void GetPostById_PostFromDbIsNull_Throws()
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            var postManagerMock = new Mock<IPostRepository<Post>>();
            int inputId = 1;
            Post expected = null;

            mock.Setup(x => x.PostRepository).Returns(postManagerMock.Object);
            postManagerMock.Setup(x => x.GetById(inputId)).Returns(expected);

            // Act
            var postService = new PostService(mock.Object);

            // Assert
            Assert.ThrowsException<Exception>(() => postService.GetPostById(inputId));
        }
    }
}
