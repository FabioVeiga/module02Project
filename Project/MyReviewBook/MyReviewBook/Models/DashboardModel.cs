using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using MyReviewBook.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyReviewBook.Models
{
    public class DashboardModel
    {
        public UserModel User { get; set; }
        public BookModel Book { get; set; }
        public googleAPIBook GoogleAPIBook { get; set; }
        public UserBookModel UserBook { get; set; }
        public IHttpContextAccessor HttpContextAccessor { get; set; }
        public ITempDataDictionaryFactory MyTempData { get; set; }
        public IActionResult myActionResult { get; set; }

        public DashboardModel()
        {
            User = new UserModel();
            Book = new BookModel();
            GoogleAPIBook = new googleAPIBook();
            UserBook = new UserBookModel();
        }

        public DashboardModel(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
            User = new UserModel();
            Book = new BookModel();
            GoogleAPIBook = new googleAPIBook();
            UserBook = new UserBookModel();
        }

        public DashboardModel(IHttpContextAccessor httpContextAccessor, ITempDataDictionaryFactory tempData)
        {
            HttpContextAccessor = httpContextAccessor;
            MyTempData = tempData;
            User = new UserModel();
            Book = new BookModel();
            GoogleAPIBook = new googleAPIBook();
            UserBook = new UserBookModel();
        }

        public string getUserSession()
        {
            return @HttpContextAccessor.HttpContext.Session.GetString("userSession");
        }

        public bool validUserSession()
        {
            string user = getUserSession();
            if (user == "" || user == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void loadDataTemp(DashboardModel dashboard)
        {
            string userSession = dashboard.getUserSession();
            int userId = dashboard.User.validUserId(userSession);
            char isActivated = dashboard.User.getUserActive(userSession);
            string picture = dashboard.User.getPictureUser(userSession);
            var httpContext = HttpContextAccessor.HttpContext;
            var tempData = MyTempData.GetTempData(httpContext);
            tempData["picture"] = picture;
            tempData["isUserActived"] = isActivated;
            tempData["userSession"] = userSession;
            tempData["userId"] = userId;
        }

    }
}
