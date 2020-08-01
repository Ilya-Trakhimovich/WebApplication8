using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApplication8.Controllers;
using System.Web.Mvc;
using Moq;
using AppBLL.DataTransferObject;
using AppBLL.Interfaces;
using System.Web;
using WebApplication8.Models;

namespace XConnection.Tests.PL.Controllers
{
    /// <summary>
    /// Summary description for HomeControllerTest
    /// </summary>
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void AvatarAdd_ModelIsNotValid_Returns_RedirectToAction()
        {
            //Arrange
            var expected = "Index/1";
            HttpPostedFileBase inputFile = null;
            var homeController = new HomeController();
            homeController.ModelState.AddModelError("", "Error");

            //Act
            var result = homeController.AvatarAdd(inputFile) as RedirectToRouteResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.RouteValues["action"]);
        }

        [TestMethod]
        public void AvatarAdd_UploadImageIsNull_Returns_RedirectToAction()
        {
            //Arrange
            var expected = "Index/1";
            HttpPostedFileBase inputFile = null;
            var homeController = new HomeController();

            //Act
            var result = homeController.AvatarAdd(inputFile) as RedirectToRouteResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.RouteValues["action"]);
        }

        [TestMethod]
        public void SaveInformatiomForm_ModelIsNotValid_Returns_RedirectToAction()
        {
            //Arrange
            var expected = "Index";
            var profileInformation = new EditProfileViewModel();
            HttpPostedFileBase inputFile = null;
            var homeController = new HomeController();
            homeController.ModelState.AddModelError("", "Error");

            //Act
            var result = homeController.SaveInformationForm(profileInformation, inputFile) as RedirectToRouteResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.RouteValues["action"]);
        }
    }
}
