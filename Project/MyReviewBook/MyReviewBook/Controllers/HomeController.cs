using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyReviewBook.Models;

namespace MyReviewBook.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index(int limit, string search)
        {
            DashboardModel dashboard = new DashboardModel();
            if(limit == 0)
            {
                limit = 5;
            }
            else
            {
                limit += 5;
            }
            if(search == null)
            {
                search = "";
            }
            TempData["limit"] = limit;
            TempData["search"] = search;
            TempData["totalRegister"] = dashboard.UserBook.totalRegisterIndex(search);
            ViewBag.ListReview = dashboard.UserBook.getReviewsForIndex(limit, search);
            return View();
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }


        public IActionResult ForgotPasswordStep01(string user)
        {
            UserModel localUser = new UserModel();
            bool flag = localUser.validUser(user);
            if (flag)
            {
                char isFirstAccess = localUser.getFirstAccess(user);
                if(isFirstAccess == '0')
                {
                    string[] typeData = localUser.getValidData(user);
                    TempData["typeData"] = typeData[0];
                    TempData["message"] = "";
                    TempData["step"] = "step02";
                }
                else
                {
                    TempData["step"] = "step01";
                    TempData["message"] = "noHaveQuestion";
                }
                
            }
            else
            {
                TempData["step"] = "step01";
                TempData["message"] = "errorUser";
            }
            TempData["userValid"] = user;
            return RedirectToAction("ForgotPassword", "Home");
        }

        public IActionResult ForgotPasswordStep02(string user, string validData)
        {
            UserModel localUser = new UserModel();
            string[] typeData = localUser.getValidData(user);
            TempData["typeData"] = typeData[0];
            TempData["dataValid"] = validData;
            if (typeData[1] == validData)
            {
                TempData["dataValid"] = typeData[1];
                TempData["step"] = "step03";
                TempData["message"] = "";
                string[] question = localUser.getAnwser(user);
                TempData["question"] = question[0];
            }
            else
            {
                TempData["step"] = "step02";
                TempData["message"] = "errorData";
            }
            TempData["userValid"] = user;
            return RedirectToAction("ForgotPassword", "Home");
        }

        public IActionResult ForgotPasswordStep03(string user, string answer)
        {
            UserModel localUser = new UserModel();
            string[] typeData = localUser.getValidData(user);
            TempData["typeData"] = typeData[0];
            TempData["dataValid"] = typeData[1];
            string[] question = localUser.getAnwser(user);
            TempData["question"] = question[0];
            if (question[1] == answer)
            {
                TempData["step"] = "showPassword";
                TempData["message"] = "";
                TempData["answer"] = question[1];
                TempData["password"] = localUser.getPassword(user);
            }
            else
            {
                TempData["step"] = "step03";
                TempData["message"] = "errorAnwser";
            }
            TempData["userValid"] = user;
            return RedirectToAction("ForgotPassword", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
