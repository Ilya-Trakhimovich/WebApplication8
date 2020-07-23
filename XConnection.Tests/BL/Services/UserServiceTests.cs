using System;
using System.Collections.Generic;
using AppBLL.DataTransferObject;
using AppBLL.Services;
using DataAcess.Entities;
using DataAcess.Interfaces;
using DataAcess.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace XConnection.Tests.BL.Services
{
    [TestClass]
    public class UserServiceTests
    {
        [TestMethod]
        public void GetAllUsers_UserProfileDTOIsNotNull()
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(x => x.UserProfileManager.GetAllUsers()).Returns(new List<UserProfile>());

            var userService = new UserService(mock.Object);

            // Act
            var result = userService.GetAllUsers();

            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetAllUsers_CkeckTypeOfReturnData_Returns_TypeIsListOfUserProfile()
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(x => x.UserProfileManager.GetAllUsers()).Returns(new List<UserProfile>());

            var userService = new UserService(mock.Object);

            var users = new List<UserProfileDTO>();

            // Act
            var result = userService.GetAllUsers();

            //Assert
            Assert.AreEqual(users.GetType(), result.GetType());
        }

        [TestMethod]
        public void GetUserDetaitls_CkeckTypeOfReturnData_Returns_TypeIsUserProfileDTO()
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            var testUser = new UserProfile() { Id = "123" };

            mock.Setup(x => x.UserProfileManager.GetUserById(testUser.Id)).Returns(new UserProfile() { ApplicationUser = new ApplicationUser() { UserProfile = new UserProfile() } });

            // Act
            var userService = new UserService(mock.Object);
            var result = userService.GetUserDetails(testUser.Id);
            var expected = new UserProfileDTO();

            //Assert
            Assert.AreEqual(expected.GetType(), result.GetType());
        }

        [TestMethod]
        public void GetUserDetails_InputStringIsCorrect_Returns_UserProfileDTO()
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            var testUser = new UserProfile() { Id = "123" };
            string inputParametr = "123";

            mock.Setup(x => x.UserProfileManager.GetUserById(testUser.Id)).Returns(new UserProfile() { ApplicationUser = new ApplicationUser() { UserProfile = new UserProfile() } });

            // Act
            var userService = new UserService(mock.Object);
            var result = userService.GetUserDetails(inputParametr);
            var expected = new UserProfileDTO();

            //Assert
            Assert.AreEqual(expected.GetType(), result.GetType());
        }

        [TestMethod]
        public void GetUsersByName_CkeckTypeOfReturnData_Returns_ListOfUserProfileDTO()
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            var listOfUserProfileDTO = new List<UserProfileDTO>();
            string inputName = "123";

            mock.Setup(x => x.UserProfileManager.GetAllUsers()).Returns(new List<UserProfile>());

            // Act
            var userService = new UserService(mock.Object);
            var result = userService.GetUsersByName(inputName);
            var expected = new List<UserProfileDTO>();

            //Assert
            Assert.AreEqual(expected.GetType(), result.GetType());
        }

        [TestMethod]
        public void GetUserByName_CheckReturnDataIsNotNull_Returns_ReturnDataIsNotNull()
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            var listOfUserProfileDTO = new List<UserProfileDTO>();
            string inputName = "123";

            mock.Setup(x => x.UserProfileManager.GetAllUsers()).Returns(new List<UserProfile>());

            // Act
            var userService = new UserService(mock.Object);
            var expected = userService.GetUsersByName(inputName);

            //Assert
            Assert.IsNotNull(expected);
        }

        //[TestMethod] DONT WORK!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //public void GetFullName_()
        //{
        //    // Arrange
        //    var mock = new Mock<IUnitOfWork>();
        //    string inputId = "123";

        //    mock.Setup(x => x.UserProfileManager);

        //    // Act
        //    var userService = new UserService(mock.Object);
        //    var userFullname = userService.GetFullName(inputId);
        //    var expected = "";

        //    //Assert
        //    Assert.AreEqual(expected, userFullname);
        //}

        [TestMethod]
        public void GetProfileInformation_CkeckTypeOfReturnData_Returns_TypeIsProfileInformationDTO()
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            var inputId = "123";

            mock.Setup(x => x.UserProfileManager.GetUserById(inputId)).Returns(new UserProfile());

            // Act
            var userService = new UserService(mock.Object);
            var result = userService.GetProfileInformation(inputId);
            var expected = new ProfileInformationDTO();

            //Assert
            Assert.AreEqual(expected.GetType(), result.GetType());
        }

        [TestMethod]
        public void GetProfileInformation_CheckReturnDataIsNotNull_Returns_ReturnDataIsNotNull()
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            var inputId = "123";

            mock.Setup(x => x.UserProfileManager.GetUserById(inputId)).Returns(new UserProfile());

            // Act
            var userService = new UserService(mock.Object);
            var result = userService.GetProfileInformation(inputId);

            //Assert
            Assert.IsNotNull(result);
        }

        //[TestMethod] DONT WORK!!!!!!!!!!!!!!!!
        //public void IsUserExist_()
        //{
        //    // Arrange
        //    var mock = new Mock<IUnitOfWork>();
        //    var inputId = "123";

        //    mock.Setup(;

        //    // Act
        //    var userService = new UserService(mock.Object);
        //    var result = userService.GetProfileInformation(inputId);

        //    //Assert
        //    Assert.IsNotNull(result);
        //}

        //[TestMethod]
        //public void GetDefaultAvatar_()
        //{
        //    // Arrange
        //    var mock = new Mock<IUnitOfWork>();
        //    var expected = new byte[] { };

        //    mock.Setup(x => x.UserProfileManager.);          

        //    // Act
        //    UserService userService = new UserService(new IdentityUnitOfWork("XConnection"));
        //    var result = userService.GetDefaultAvatar();

        //    //Assert
        //    Assert.AreEqual(expected.GetType(), result.GetType());
        //}


    }
}
