using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BooksShoppingCart.Models;

namespace BooksShoppingCart.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrdersController : Controller
    {
        private WelcomeBooksEntities db = new WelcomeBooksEntities();
        // GET: Admin/Order
        public ActionResult Index()
        {
            return View(db.Orders.ToList());
        }
        
        // GET: Admin/Details/8
        public ActionResult Details(int? Id)
        {
            var result = (from d in db.OrderDetails
                          where d.Order_Id == Id
                          select d).ToList();
            if (result.Count == 0)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(result);
            }
        }
    }
}