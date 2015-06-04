using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace BooksShoppingCart.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult Index()
        {
            return View();
        }

        //取得目前購物車頁面
        public ActionResult GetCart()
        {
            return PartialView("_CartPartial");
        }

        /// <summary>
        /// 將商品加入購物車
        /// </summary>
        /// <param name="Id">商品編號(Product Id)</param>
        /// <returns></returns>
        public ActionResult AddToCart(int? Id)
        {
            var currentCart = BooksShoppingCart.Models.Cart.Operation.GetCurrentCart();
            currentCart.AddProduct(Id);
            return PartialView("_CartPartial");
        }

        /// <summary>
        /// 將商品移除購物車
        /// </summary>
        /// <param name="Id">商品編號(Product Id)</param>
        /// <returns></returns>
        public ActionResult RemoveFromCart(int? Id)
        {
            var currentCart = BooksShoppingCart.Models.Cart.Operation.GetCurrentCart();
            currentCart.RemoveProduct(Id);
            return PartialView("_CartPartial");
        }

        /// <summary>
        /// 清空購物車
        /// </summary>
        /// <returns></returns>
        public ActionResult ClearCart()
        {
            var currentCart = BooksShoppingCart.Models.Cart.Operation.GetCurrentCart();
            currentCart.ClearCart();
            return PartialView("_CartPartial");
        }
    }
}