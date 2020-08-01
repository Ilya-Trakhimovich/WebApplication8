using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApplication8.Controllers;

namespace XConnection.Tests.PL.Controllers
{
    [TestClass]
    public class AccountControllerTest
    {
        [TestMethod]
        public void Login_LoginViewEqualsLoginCshtml_Returns_CorrectView()
        {
            AccountController controller = new AccountController();

            var result = controller.Login() as ViewResult;

            Assert.AreEqual("Login", result.ViewName);
        }

        [TestMethod]
        public void Register_RegisterViewEqualsRegisterCshtml_Returns_CorrectView()
        {
            AccountController controller = new AccountController();

            var result = controller.Register() as ViewResult;

            Assert.AreEqual("Register", result.ViewName);
        }

        [TestMethod]
        public void Login_ModelIsNOtValid_Returns_View()
        {
            //Arrange
            var expectedView = "Login";
            AccountController accountController = new AccountController();

            accountController.ModelState.AddModelError("", "Error");

            //Act
            var result = accountController.Login() as ViewResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedView, result.ViewName);
        }

        [TestMethod]
        public void REgister_ModelIsNotValid_Returns_View()
        {
            //Arrange
            var expectedView = "Register";
            AccountController accountController = new AccountController();

            accountController.ModelState.AddModelError("", "Error");

            //Act
            var result = accountController.Register() as ViewResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedView, result.ViewName);
        }


    }
}
