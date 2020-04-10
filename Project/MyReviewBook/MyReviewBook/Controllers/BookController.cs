using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using MyReviewBook.Models;
using MyReviewBook.Services;
using MyReviewBook.Useful;
using Newtonsoft.Json;

namespace MyReviewBook.Controllers
{
    public class BookController : Controller
    {

        IHttpContextAccessor HttpContextAccessor;
        ITempDataDictionaryFactory myTempData;
        public BookController(IHttpContextAccessor httpContextAccessor, ITempDataDictionaryFactory tempData)
        {
            HttpContextAccessor = httpContextAccessor;
            myTempData = tempData;
            DashboardModel dashboard = new DashboardModel(HttpContextAccessor, myTempData);
            bool flag = dashboard.validUserSession();
            if (flag)
            {
                dashboard.loadDataTemp(dashboard);
            }
        }

        public IActionResult Index()
        {
            DashboardModel dashboard = new DashboardModel(HttpContextAccessor);
            bool flag = dashboard.validUserSession();
            if (flag)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult searchBook(string titleBook)
        {
            DashboardModel dashboard = new DashboardModel(HttpContextAccessor);
            string userNameSession = dashboard.getUserSession();
            int userId = dashboard.User.validUserId(userNameSession);
            //Verify which books have in own database
            //Get List from books
            List<DashboardModel> listBooksOwn = dashboard.Book.getListBook(titleBook, userId);
            List<DashboardModel> listBooksService = new List<DashboardModel>();
            try
            {
                //Get List from service
               listBooksService = dashboard.GoogleAPIBook.getListBook(titleBook);
            }
            catch { }
            
            
            //Merge these list and delete the duplicates itens
            listBooksService.RemoveAll(serv => listBooksOwn.Exists(own => own.Book.GoogleId == serv.Book.GoogleId && own.Book.IdBook != serv.Book.IdBook));
            //Add the database List
            //listBooksService.AddRange(listBooksOwn);
            listBooksService.InsertRange(0, listBooksOwn);

            TempData["textSearched"] = titleBook;
            TempData["userId"] = userId;

            ViewBag.ListBook = listBooksService;
            return View("Index");
        }
    }
}