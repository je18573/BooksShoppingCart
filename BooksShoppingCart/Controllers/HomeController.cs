using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BooksShoppingCart.Models;

namespace BooksShoppingCart.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        private WelcomeBooksEntities db = new WelcomeBooksEntities();
        public ActionResult Index(int? CategoryId, int page =1, int pageSize = 3)
        {
            ViewData["CategoryList"] = (from cate in db.Categories
                                        select cate).ToList();
            if (CategoryId != null)
            {
                var HomeProducts =  (from prod in db.Products
                                    join cate in db.Categories on prod.Category_Id equals cate.Id
                                    where prod.IsPublic == true && prod.Category_Id == CategoryId
                                    orderby prod.PublishDT descending
                                    select prod).ToList();
                return View(HomeProducts);
            }
            else
            {
                var HomeProducts =  (from prod in db.Products
                                    join cate in db.Categories on prod.Category_Id equals cate.Id
                                    where prod.IsPublic == true
                                    orderby prod.PublishDT descending
                                    select prod).ToList();
                return View(HomeProducts);
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "這是一個書店購物車練習網站，僅提供以下功能：";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "我的連絡資訊如下：";

            return View();
        }
    }
}