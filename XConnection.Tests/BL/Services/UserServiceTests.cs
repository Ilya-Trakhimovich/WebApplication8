using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppBLL.DataTransferObject;
using AppBLL.Services;
using DataAcess.Entities;
using DataAcess.Interfaces;
using DataAcess.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace XConnection.Tests.BL.Services
{
    [TestClass]
    public class UserServiceTests
    {
        [TestMethod]
        public void GetAllUsers_UserProfileDTOIsNotNull_Ok()
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

        [TestMethod]
        public void GetDefaultAvatar_OK()
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            var expected = new byte[] { };

            mock.Setup(x => x.UserProfileManager.GetUserById("123")).Returns(new UserProfile() { Avatar = new byte[] { } });

            // Act
            UserService userService = new UserService(mock.Object);
            var result = userService.GetDefaultAvatar();

            //Assert
            Assert.AreEqual(expected.GetType(), result.GetType());
        }

        [TestMethod]
        public void GetFullName_OK()
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            var userStore = new Mock<IUserStore<ApplicationUser>>();
            var userManagerMock = new Mock<ApplicationUserManager>(userStore.Object);
            string inputId = "123";
            var userProfile = new UserProfile() { FirstName = "1", SecondName = "2" };

            mock.Setup(x => x.UserManager).Returns(userManagerMock.Object);
            userManagerMock.Setup(x => x.FindByIdAsync(inputId)).ReturnsAsync(new ApplicationUser() { UserProfile = userProfile });

            // Act
            var userService = new UserService(mock.Object);
            var userFullname = userService.GetFullName(inputId);
            var expected = $"{userProfile.FirstName} {userProfile.SecondName}";


            //Assert
            Assert.AreEqual(expected, userFullname);
        }

        [TestMethod]
        public void GetProfileInformation_UserExists_ReturnDTO()
        {
            // Arrange
            var inputId = "123";
            var mock = new Mock<IUnitOfWork>();
            var userProfileManagerMock = new Mock<IUserProfileManager>();
            var expectedUserProfile = new UserProfile() { FirstName = "Oleg", SecondName = "Ra", Age = 25, Address = "Minsk" };

            mock.Setup(x => x.UserProfileManager).Returns(userProfileManagerMock.Object);
            userProfileManagerMock.Setup(x => x.GetUserById(inputId)).Returns(expectedUserProfile);

            // Act
            var userService = new UserService(mock.Object);
            var userProfileInformationDTO = userService.GetProfileInformation(inputId);

            // Assert
            Assert.AreEqual(expectedUserProfile.FirstName, userProfileInformationDTO.FirstName);
            Assert.AreEqual(expectedUserProfile.SecondName, userProfileInformationDTO.SecondName);
            Assert.AreEqual(expectedUserProfile.Age, userProfileInformationDTO.Age);
            Assert.AreEqual(expectedUserProfile.Address, userProfileInformationDTO.Address);
        }

        [TestMethod]
        public void GetProfileInformation_UserNotExsist_Throws()
        {
            // Arrange
            var inputId = "123";
            var mock = new Mock<IUnitOfWork>();
            var userProfileManagerMock = new Mock<IUserProfileManager>();
            UserProfile expectedUserProfile = null;

            mock.Setup(x => x.UserProfileManager).Returns(userProfileManagerMock.Object);
            userProfileManagerMock.Setup(x => x.GetUserById(inputId)).Returns(expectedUserProfile);

            // Act
            var userService = new UserService(mock.Object);

            // Assert
            Assert.ThrowsException<Exception>(() => userService.GetProfileInformation(inputId));
        }

        [TestMethod]
        public void GetAllUsers_UsersIsNull_Throws()
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            var userProfileMock = new Mock<IUserProfileManager>();
            List<UserProfile> userProfiles = null;

            mock.Setup(x => x.UserProfileManager).Returns(userProfileMock.Object);
            userProfileMock.Setup(x => x.GetAllUsers()).Returns(userProfiles);

            // Act
            var userService = new UserService(mock.Object);

            // Assert
            Assert.ThrowsException<Exception>(() => userService.GetAllUsers());
        }

        [TestMethod]
        public void GetUserGroup_GroupsIsNull_Throws()
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            var userStore = new Mock<IUserStore<ApplicationUser>>();
            var userManagerMock = new Mock<ApplicationUserManager>(userStore.Object);
            string inputId = "123";
            ICollection<Group> expectedGroups = null;

            mock.Setup(x => x.UserManager).Returns(userManagerMock.Object);
            userManagerMock.Setup(x => x.FindByIdAsync(inputId)).ReturnsAsync(new ApplicationUser() { Groups = expectedGroups});

            // Act
            var userService = new UserService(mock.Object);

            //Assert
            Assert.ThrowsException<Exception>(() => userService.GetUserGroups(inputId));
        }

        [TestMethod]
        public void GetUserGroup_GroupsAreExist_ReturnGroupDTO()
        {
            // Arrange
            var inputId = "123";
            var mock = new Mock<IUnitOfWork>();
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            var userManagerMock = new Mock<ApplicationUserManager>(userStoreMock.Object);
            var expectedGroups = new List<Group> { new Group() {  GroupName = "Songs", GroupDescription = "Songs Test"} };

            mock.Setup(x => x.UserManager).Returns(userManagerMock.Object);
            userManagerMock.Setup(x => x.FindByIdAsync(inputId)).ReturnsAsync(new ApplicationUser() { Groups = expectedGroups});

            // Act
            var userService = new UserService(mock.Object);
            var result = userService.GetUserGroups(inputId) as List<GroupDTO>;

            // Assert
            Assert.AreEqual(expectedGroups.FirstOrDefault().GroupName, result.FirstOrDefault().GroupName);
            Assert.AreEqual(expectedGroups.FirstOrDefault().GroupDescription, result.FirstOrDefault().GroupDescription);
        }
    }
}
