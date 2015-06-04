using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BooksShoppingCart.Models
{
    public class WelcomeBooksMetaData
    {
    }

    [MetadataType(typeof(CategoriesMetaData))]
    public partial class Categories
    {
        public class CategoriesMetaData
        {
            [Required]
            [Display(Name = "分類名稱")]
            public string Name { get; set; }
        }
    }

    [MetadataType(typeof(ProductsMetaData))]
    public partial class Products
    {
        public class ProductsMetaData
        {
            [Display(Name="商品編號")]
            public int Id { get; set; }

            [Required]
            [Display(Name="書名")]
            public string Name { get; set; }

            [Display(Name="說明")]
            public string Description { get; set; }

            [Required]
            [Display(Name="分類")]
            public int Category_Id { get; set; }

            [Required]
            [Display(Name="定價")]
            public decimal Price { get; set; }

            [Display(Name="封面")]
            public string PhotoUrl { get; set; }

            [Required]
            [Display(Name="作者")]
            public string Author { get; set; }

            [Required]
            [Display(Name="出版社")]
            public string Publisher { get; set; }

            [Required]
            [Display(Name="出版日期")]
            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
            public Nullable<System.DateTime> PublishDT { get; set; }

            [Display(Name="是否上架")]
            public bool IsPublic { get; set; }

        }

    }

}