using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BooksShoppingCart.Models;
using Microsoft.AspNet.Identity;

namespace BooksShoppingCart.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private WelcomeBooksEntities db = new WelcomeBooksEntities();

        // GET: Order
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(OrderViewModel orderView)
        {
            if (ModelState.IsValid)
            {
                //取得目前購物車資料
                var currentCart = Models.Cart.Operation.GetCurrentCart();

                //取得目前登入使用者Id
                var UserId = HttpContext.User.Identity.GetUserId();

                var order = new Orders()
                {
                    AspNetUser_Id = UserId,
                    ContactName = orderView.ContactName,
                    ContactPhone = orderView.ContactPhone,
                    ContactAddress = orderView.ContactAddress,
                    BuyDT = DateTime.Now,
                    TotalPrice = orderView.TotalAmount,
                    Memo = orderView.Memo
                    
                };
                //存入Orders資料表
                db.Orders.Add(order);
                db.SaveChanges();

                //取得購物車OrderDetail物件
                var orderDetails = currentCart.ToOrderDetailList(order.Id);
                //存入OrderDetails資料表
                db.OrderDetails.AddRange(orderDetails);
                db.SaveChanges();

                //移除購物車Session資料
                HttpContext.Session.Remove("Cart");

                //轉至訂單完成頁面
                return RedirectToAction("FinishOrdered");
            }
            return View();
        }
        
        public ActionResult FinishOrdered()
        {
            return View();
        }

        public ActionResult MyOrder()
        {
            var UserId = HttpContext.User.Identity.GetUserId();
            var result = (from ord in db.Orders
                          where ord.AspNetUser_Id == UserId
                          select ord).ToList();
            return View(result);
        }

        public ActionResult MyOrderDetail(int? Id)
        {
            var result = (from d in db.OrderDetails
                          where d.Order_Id == Id
                          select d).ToList();
            if (result.Count == 0)
            {
                return RedirectToAction("MyOrder");
            }
            else
            {
                return View(result);
            }
        }
    }
}