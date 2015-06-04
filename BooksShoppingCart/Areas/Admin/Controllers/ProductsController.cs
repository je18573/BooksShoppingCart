using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BooksShoppingCart.Models;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace BooksShoppingCart.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    {
        private WelcomeBooksEntities db = new WelcomeBooksEntities();

        // GET: Admin/Products
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Categories);
            return View(products.ToList());
        }

        // GET: Admin/Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // GET: Admin/Products/Create
        public ActionResult Create()
        {
            ViewBag.Category_Id = new SelectList(db.Categories, "Id", "Name");
            return View(new ProductsViewModel());
        }

        // POST: Admin/Products/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductsViewModel model)
        {
            //傳進去ProductsViewModel model, 回傳
            var validImageTpes = new string[]
            {
                "image/gif",
                "image/jpeg",
                "image/pjpeg",
                "image/png"
            };

            if (　model.ImageUpload != null　&& !validImageTpes.Contains(model.ImageUpload.ContentType))
            {
                ModelState.AddModelError("ImageUpload", "請選擇 GIF, JPG or PNG 圖片檔案格式。");
            }

            if (ModelState.IsValid)
            {
                var product = new Products
                {
                    Name = model.Name,
                    Description = model.Description,
                    Category_Id = model.Category_Id,
                    Price = model.Price,
                    Author = model.Author,
                    Publisher = model.Publisher,
                    PublishDT = Convert.ToDateTime(model.PublishDT),
                    IsPublic = model.IsPublic
                };

                if (model.ImageUpload != null && model.ImageUpload.ContentLength > 0)
                {
                    ImageDeal imageDeal = new ImageDeal();
                    string imageUrl = imageDeal.Upload(model.ImageUpload);             
                    product.PhotoUrl = imageUrl;

                }

                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Category_Id = new SelectList(db.Categories, "Id", "Name", model.Category_Id);
            return View(model);
        }

        // GET: Admin/Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            var model = new ProductsViewModel
            {
                Id = products.Id,
                Name = products.Name,
                Description = products.Description,
                Category_Id = products.Category_Id,
                Price = products.Price,
                PhotoUrl = products.PhotoUrl,
                Author = products.Author,
                Publisher = products.Publisher,
                PublishDT = products.PublishDT,
                IsPublic = products.IsPublic
            };

            ViewBag.Category_Id = new SelectList(db.Categories, "Id", "Name", products.Category_Id);
            return View(model);
        }

        // POST: Admin/Products/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, ProductsViewModel model)
        {
            var validImageTypes = new string[]
            {
                "image/gif",
                "image/jpeg",
                "image/pjpeg",
                "image/png"
            };

            if ( model.ImageUpload != null && !validImageTypes.Contains(model.ImageUpload.ContentType))
            {
                ModelState.AddModelError("ImageUpload", "請選擇 GIF, JPG or PNG 圖片檔案格式。");
            }

            if (ModelState.IsValid)
            {
                var product = db.Products.Find(id);
                if (product == null)
                {
                    return new HttpNotFoundResult();
                }

                product.Name = model.Name;
                product.Description = model.Description;
                product.Category_Id = model.Category_Id;
                product.Price = model.Price;
                product.Author = model.Author;
                product.Publisher = model.Publisher;
                product.PublishDT = Convert.ToDateTime(model.PublishDT);
                product.IsPublic = model.IsPublic;

                if (model.ImageUpload != null && model.ImageUpload.ContentLength > 0)
                {
                    ImageDeal imageDeal = new ImageDeal();
                    //上傳圖檔&壓縮圖檔處理
                    string imageUrl = imageDeal.Upload(model.ImageUpload);     
                    product.PhotoUrl = imageUrl;

                    //刪除舊檔案&壓縮檔案
                    var deletePhotoUrl = Server.MapPath(model.PhotoUrl);
                    imageDeal.Delete(deletePhotoUrl);
                }

                db.Entry(product).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Category_Id = new SelectList(db.Categories, "Id", "Name", model.Category_Id);
            return View(model);
        }

        // GET: Admin/Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Products products = db.Products.Find(id);
            db.Products.Remove(products);
            db.SaveChanges();

            //刪除舊檔案
            //刪除舊檔案&壓縮檔案
            var deletePhotoUrl = Server.MapPath(products.PhotoUrl);
            ImageDeal imageDeal = new ImageDeal();
            imageDeal.Delete(deletePhotoUrl);
           

            return RedirectToAction("Index");

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private string CompressImage(string imagePath, string photoName, string uploadDir)
        {
            Image image = Image.FromFile(imagePath);
            
            //取得影像格式
            ImageFormat thisFormat = image.RawFormat;
            
            int fixWidth = image.Width;
            int fixHeight = image.Height;

            int maxPx = Convert.ToInt16(200);
            
            if (image.Width > maxPx || image.Height > maxPx)
            {
                if (image.Width >= image.Height)
                {
                    fixWidth = maxPx;
                    fixHeight = Convert.ToInt32((Convert.ToDouble(fixWidth)) / (Convert.ToDouble(image.Width)) * Convert.ToDouble(image.Height));
                }
                else
                {
                    fixHeight = maxPx;
                    fixWidth = Convert.ToInt32((Convert.ToDouble(fixHeight)) / (Convert.ToDouble(image.Height)) * Convert.ToDouble(image.Width));
                }
            }

            Bitmap imageOutput = new Bitmap(image, fixWidth, fixHeight);

            string CompressPhotoName = "Compress_" + photoName ;
            imageOutput.Save(Path.Combine(Server.MapPath(uploadDir), CompressPhotoName));

            imageOutput.Dispose();
            image.Dispose();

            return CompressPhotoName;
        }
    }
}
