using AppBLL.DataTransferObject;
using AppBLL.Infrastructure;
using AppBLL.Interfaces;
using AutoMapper;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication8.App_Start;
using WebApplication8.Models;

namespace WebApplication8.Controllers
{
    public class AccountController : Controller
    {
        MapperConfigs mapperConfigs = new MapperConfigs();

        private IUserService UserService
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<IUserService>();
            }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public ActionResult Login() => View("Login");

        public ActionResult Register() => View("Register");

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model) // тут пользователь авторизуется
        {
            if (ModelState.IsValid)
            {
                UserDTO userDto = new UserDTO 
                { 
                    Email = model.Email, 
                    Password = model.Password 
                };

                ClaimsIdentity claim = await UserService.Authenticate(userDto);

                if (claim == null)
                {
                    ModelState.AddModelError("", "Неверный логин или пароль.");
                }
                else
                {
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true
                    }, claim);

                    return RedirectToAction("Index", "Home");
                }
            }

            return View(model);
        }

        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();

            return RedirectToAction("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                Mapper userRegisterMapper = new Mapper(mapperConfigs.RegistraionModelToUserDto);

                var userDto = userRegisterMapper.Map<UserDTO>(model);

                OperationDetails operationDetails = await UserService.Create(userDto);

                if (operationDetails.Succedeed)
                {
                    ClaimsIdentity claim = await UserService.Authenticate(userDto);

                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true
                    }, claim);

                    return RedirectToAction("Index", "Home");

                }
                else
                    ModelState.AddModelError(operationDetails.Property, operationDetails.Message);
            }

            return View(model);
        }
    }
}
