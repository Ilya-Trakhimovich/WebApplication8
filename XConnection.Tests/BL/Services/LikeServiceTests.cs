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
    public class LikeServiceTests
    {
        [TestMethod]
        public void GetAllPostLikes_PostAreExist_Returns_ListOfPostLike()
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            var likeManagerMock = new Mock<IPostRepository<Post>>();
            int inputId = 1;
            var expectedPost = new Post()
            {
                ApplicationUser = new ApplicationUser()
                {
                    UserProfile = new UserProfile()
                },
                PostLikes = new List<PostLike>()
                {
                    new PostLike()
                    {
                        Id = 1, PostId = 1, User = "ABBA", Post = new Post()
                        {
                            ApplicationUser = new ApplicationUser()
                            {
                                UserProfile = new UserProfile()
                            }
                        }
                    }
                }
            };

            mock.Setup(x => x.PostRepository).Returns(likeManagerMock.Object);
            likeManagerMock.Setup(x => x.GetById(inputId)).Returns(expectedPost);

            // Act
            var likeService = new LikeService(mock.Object);
            var result = likeService.GetAllPostLikes(inputId);

            // Assert
            Assert.AreEqual(expectedPost.PostLikes.FirstOrDefault().Id, result.FirstOrDefault().Id);
            Assert.AreEqual(expectedPost.PostLikes.FirstOrDefault().Post, result.FirstOrDefault().Post);
            Assert.AreEqual(expectedPost.PostLikes.FirstOrDefault().User, result.FirstOrDefault().User);
            Assert.AreEqual(expectedPost.PostLikes.FirstOrDefault().PostId, result.FirstOrDefault().PostId);
        }

        [TestMethod]
        public void AddLikeToPost_InputDataIsNull_Throws()
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            PostLikeDTO postLikeDTO = null;

            // Act
            var likeService = new LikeService(mock.Object);

            // Assert
            Assert.ThrowsException<ArgumentNullException>(() => likeService.AddLikeToPost(postLikeDTO));
        }

        [TestMethod]
        public void DislikePost_InputDataIsNull_Throws()
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            PostLikeDTO postLikeDTO = null;

            // Act
            var likeService = new LikeService(mock.Object);

            // Assert

            Assert.ThrowsException<ArgumentNullException>(() => likeService.DislikePost(postLikeDTO));
        }
    }
}
