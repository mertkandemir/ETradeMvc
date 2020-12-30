using ETradeBusiness.Models;
using System;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using AppCore.Business.Bases;
using AppCore.DataAccess.Repositories;
using AppCore.DataAccess.Repositories.Bases;
using ETradeBusiness.Services;
using ETradeDataAccess.Contexts;
using ETradeEntities.Entities;
using System.Net;
using System.Web.Security;

namespace ETradeMvcWebUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly DbContext db;
        private readonly RepositoryBase<User> userRepository;
        private readonly RepositoryBase<Role> roleRepository;
        private readonly IService<User, UserModel> userService;

        public AccountController()
        {
            db = new ETradeContext();
            userRepository = new Repository<User>(db);
            roleRepository = new Repository<Role>(db);
            userService = new UserService(userRepository, roleRepository);
        }

        public ActionResult Login()
        {
            var model = new UserModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserModel user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //var query = userService.GetQuery().Where(u => u.UserName == user.UserName && u.Password == user.Password);
                    //var query = userService.GetQuery(u => u.UserName == user.UserName && u.Password == user.Password);
                    //var dbUser = query.FirstOrDefault();

                    //var dbUser = userService.GetQuery().SingleOrDefault(u => u.UserName == user.UserName && u.Password == user.Password);
                    //if (dbUser == null)
                    //{
                    //    ViewBag.Message = "User Name or Password is incorrect!";
                    //    return View(user);
                    //}

                    bool userFound = userService.GetQuery().Any(u => u.UserName == user.UserName && u.Password == user.Password);
                    if (!userFound)
                    {
                        ViewBag.Message = "User Name or Password is incorrect!";
                        return View(user);
                    }

                    user.BirthDay = DateTime.Parse(user.BirthDayText, new CultureInfo("en"));
                    if (DateTime.Now.Year - user.BirthDay.Value.Year < 18)
                    {
                        ViewBag.Message = "User must be over 18 years of age!";
                        return View(user);
                    }

                    FormsAuthentication.SetAuthCookie(user.UserName, true);
                    return RedirectToAction("Index", "Products");

                }

                return View(user);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "An error occured!");
            }
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Products");
        }
    }
}