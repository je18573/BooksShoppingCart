using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BooksShoppingCart.Models;
using System.Web.Services;

namespace BooksShoppingCart.Models.Cart
{
    public class Operation
    {
        /// <summary>
        /// 啟用Session，紀錄購物車目前商品清單
        /// </summary>
        /// <returns>回傳Session["Cart"]</returns>
        [WebMethod(EnableSession=true)] //啟用Session
        public static Cart GetCurrentCart()
        {
            if (HttpContext.Current != null)
            {
                if (HttpContext.Current.Session["Cart"] == null)
                {
                    var order = new Cart();
                    HttpContext.Current.Session["Cart"] = order;
                }
                return (Cart)HttpContext.Current.Session["Cart"];
            }
            else
            {

                throw new InvalidOperationException("System.Web.HttpContext.Current為空");
            }
        }
    }
}