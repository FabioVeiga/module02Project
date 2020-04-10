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
    public class DashboardController : Controller
    {
        IHttpContextAccessor HttpContextAccessor;
        ITempDataDictionaryFactory MyTempData;
        public DashboardController(IHttpContextAccessor httpContextAccessor, ITempDataDictionaryFactory tempData)
        {
            HttpContextAccessor = httpContextAccessor;
            MyTempData = tempData;
            DashboardModel dashboard = new DashboardModel(HttpContextAccessor, MyTempData);
            bool flag = dashboard.validUserSession();
            if (flag)
            {
                dashboard.loadDataTemp(dashboard);
            }
        }

        public IActionResult Login(string user, string password)
        {
            DashboardModel dashboard = new DashboardModel();
            bool flagLogin = dashboard.User.getLogin(user, password);
            if (flagLogin)
            {
                //Register user in session
                HttpContext.Session.SetString("userSession", user);
                return RedirectToAction("Index", "Dashboard");
            }
            TempData["message"] = "noLogin";
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logoff()
        {
            HttpContext.Session.SetString("userSession", "");
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Index()
        {
            DashboardModel dashboard = new DashboardModel(HttpContextAccessor);
            string userSession = dashboard.getUserSession();
            bool flag = dashboard.validUserSession();
            if (flag)
            {
                char firstAcess = dashboard.User.getFirstAccess(userSession);
                if (firstAcess == '1')
                {
                    return RedirectToAction("FirstAccess", "Dashboard");
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult FirstAccess()
        {
            DashboardModel dashboard = new DashboardModel(HttpContextAccessor);
            string userSession = dashboard.getUserSession();
            bool flag = dashboard.validUserSession();
            if (flag)
            {
                char firstAcess = dashboard.User.getFirstAccess(userSession);
                if (firstAcess == '0')
                {
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult UpdateFirstAccess(string typeData, string validDate, string question, string answer)
        {
            DashboardModel dashboard = new DashboardModel(HttpContextAccessor);
            string userSession = dashboard.getUserSession();
            UserModel localUser = new UserModel();
            bool flag = dashboard.User.updateForgetPassword(userSession, typeData, validDate, question, answer);
            if (flag)
            {
                flag = localUser.updateFirstAccess(userSession);
                if (flag)
                {
                    return RedirectToAction("Index", "Dashboard");
                }
            }
            return RedirectToAction("FirstAccess", "Dashboard");
        }
    }
}