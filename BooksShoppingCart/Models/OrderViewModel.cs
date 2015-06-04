using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using BooksShoppingCart.Models;
using BooksShoppingCart.Areas.Admin.Models;
namespace BooksShoppingCart.Models
{
    public class OrderViewModel
    {
        [Required]
        [Display(Name="收貨人姓名")]
        [StringLength(50, ErrorMessage="{0} 的長度至少必須為 {2} 個字元。", MinimumLength=2)]
        public string ContactName { get; set; }

        [Required]
        [Display(Name="收貨人聯絡電話")]
        [Phone]
        [RegularExpression(@"^\(?([0-9]{4})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{3})$", ErrorMessage = "輸入的手機號碼格式不正確(ex: 0123-456-789)")]
        public string ContactPhone { get; set; }

        [Required]
        [Display(Name="收貨人地址")]
        [StringLength(250, ErrorMessage = "{0} 的長度至少必須為 {2} 個字元。", MinimumLength = 8)]
        public string ContactAddress { get; set; }

        [Required]
        [Display(Name="總計")]
        public decimal TotalAmount { get; set; }

        [Display(Name="備註")]
        public string Memo { get; set; }
    }
}