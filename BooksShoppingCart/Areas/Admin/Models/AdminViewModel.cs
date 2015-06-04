using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BooksShoppingCart.Areas.Admin.Models
{
    public class RoleViewModel
    {
        public string Id { get; set; }

        [Required(AllowEmptyStrings=false)]
        [Display(Name="角色名稱")]
        public string Name { get; set; }
    }

    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Required(AllowEmptyStrings=false)]
        [Display(Name="電子信箱")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(AllowEmptyStrings=false)]
        [Display(Name="暱稱")]
        [StringLength(100, ErrorMessage = "{0} 的長度至少必須為 {2} 個字元。", MinimumLength = 2)]
        public string NickName { get; set; }

        public IEnumerable<SelectListItem> RolesList { get; set; }
    }
}