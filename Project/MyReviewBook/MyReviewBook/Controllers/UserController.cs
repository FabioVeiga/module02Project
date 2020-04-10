using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using MyReviewBook.Models;

namespace MyReviewBook.Controllers
{
    public class UserController : Controller
    {
        IHttpContextAccessor HttpContextAccessor;
        ITempDataDictionaryFactory myTempData;
        public UserController(IHttpContextAccessor httpContextAccessor, ITempDataDictionaryFactory tempData)
        {
            HttpContextAccessor = httpContextAccessor;
            myTempData = tempData;
            DashboardModel dashboard = new DashboardModel(HttpContextAccessor, myTempData);
            bool flag = dashboard.validUserSession();
            if (flag)
            {
                dashboard.loadDataTemp(dashboard);
            }
            RedirectToAction("Index", "Home");
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

        [HttpPost]
        public IActionResult ChangePassword(string user, string password)
        {
            UserModel localUser = new UserModel();
            bool flag = localUser.updatePassword(user, password);
            if (flag)
            {
                TempData["message"] = "userPasswordUpdated";
            }
            else
            {
                TempData["message"] = "userPasswordError";
            }
            TempData["idModalShow"] = "changePassword";
            TempData["passTyped"] = "password";
            return RedirectToAction("Index", "Dashboard");
        }
        
        public IActionResult ChangeActive()
        {
            UserModel localUser = new UserModel(HttpContextAccessor);
            string user = localUser.GetUserSession();
            char flagActive = localUser.updateActiveUser(user);
            TempData["isUserActived"] = flagActive;
            return RedirectToAction("Index", "Dashboard");
        }

        [HttpPost]
        public IActionResult ChangePicture(string urlPicture)
        {
            UserModel localUser = new UserModel(HttpContextAccessor);
            string user = localUser.GetUserSession();
            localUser.updatePictureUser(user, urlPicture);
            string picture = localUser.getPictureUser(user);
            TempData["picture"] = picture;
            return RedirectToAction("Index", "Dashboard");
        }

    }
}