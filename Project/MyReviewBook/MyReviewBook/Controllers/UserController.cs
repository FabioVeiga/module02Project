using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyReviewBook.Models;

namespace MyReviewBook.Controllers
{
    public class UserController : Controller
    {

        [HttpPost]
        public IActionResult Login(string user, string password)
        {
            UserModel localUser = new UserModel();
            bool flagLogin = localUser.getLogin(user, password);
            if (flagLogin)
            {
                return RedirectToAction("Index", "Dashboard");
            }
            TempData["message"] = "noLogin";
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult Newuser(string user, string password)
        {
            UserModel localUser = new UserModel();
            //fisrt verify if this user exists
            bool flag = localUser.getUser(user);
            //It bring true if has no user registered
            if (flag)
            {
                flag = localUser.insertUser(user, password);
                if (flag)
                {
                    TempData["message"] = "userInsertSucess";
                }
            }
            else
            {
                TempData["message"] = "userExists";
                TempData["userTyped"] = user;
                TempData["pasTyped"] = password;
            }
            return RedirectToAction("Index", "Home");
        }
    }
}