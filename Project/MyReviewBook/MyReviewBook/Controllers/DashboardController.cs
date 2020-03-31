using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyReviewBook.Models;

namespace MyReviewBook.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            string user = HttpContext.Session.GetString("userSession");
            if(user == "" || user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                UserModel userModel = new UserModel();
                char isActivated = userModel.getUserActive(user);
                string picture = userModel.getPictureUser(user);
                char firstAcess = userModel.GetFirstAccess(user);
                TempData["picture"] = picture;
                TempData["isUserActived"] = isActivated;
                //Verify first access
                if (firstAcess == '1')
                {
                    return RedirectToAction("FirstAccess", "Dashboard");
                }
                else
                {
                    return View();
                }
            }
        }

        public IActionResult FirstAccess()
        {
            string user = HttpContext.Session.GetString("userSession");
            UserModel userModel = new UserModel();
            char isActivated = userModel.getUserActive(user);
            string picture = userModel.getPictureUser(user);
            char firstAcess = userModel.GetFirstAccess(user);
            TempData["picture"] = picture;
            TempData["isUserActived"] = isActivated;
            if(firstAcess == '0')
            {
                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                return View();
            }
        }

        public IActionResult UpdateFirstAccess(string typeData, string validDate, string question, string answer)
        {
            string user = HttpContext.Session.GetString("userSession");
            UserModel localUser = new UserModel();
            bool flag = localUser.updateForgetPassword(user, typeData, validDate, question, answer);
            if (flag)
            {
                flag = localUser.updateFirstAccess(user);
                if (flag)
                {
                    return RedirectToAction("Index", "Dashboard");
                }
            }
            return RedirectToAction("FirstAccess", "Dashboard");
        }
    }
}