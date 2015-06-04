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

namespace BooksShoppingCart.Models
{

    public class ImageDeal 
    {

        private string UploadDir;               //上傳目錄
        private string CompressPhotonamePrefix; //壓縮檔案名稱
        private string ShowImageDir;            //前端顯示的資料夾名稱

        public ImageDeal()
        {
            this.UploadDir = "~/FileUploads";
            this.CompressPhotonamePrefix = "Compress_";
            this.ShowImageDir = "/FileUploads";
        }

        /// <summary>
        /// 上傳圖片及壓縮處理
        /// </summary>
        /// <param name="imageUpload"></param>
        /// <returns>imageUrl圖片儲存路徑</returns>
        public string Upload(HttpPostedFileBase imageUpload)
        {
            //儲存之檔案名稱
            string photoName = Guid.NewGuid() + Path.GetExtension(imageUpload.FileName);
            //上傳原始檔案
            string imagePath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(UploadDir), photoName);
            imageUpload.SaveAs(imagePath);
            //壓縮檔案並上傳壓縮檔
            string CompressPhotoname = Compress(imagePath, photoName);
            string imageUrl = Path.Combine(this.ShowImageDir, CompressPhotoname);
            //回傳儲存於資料的前端商品圖片顯示路徑
            return imageUrl;
        }

        /// <summary>
        /// 刪除原始圖檔&壓縮圖檔
        /// </summary>
        /// <param name="deletePhotoUrl">刪除的圖檔路徑</param>
        public void Delete(string deletePhotoUrl)
        {
            //儲存於資料庫路徑的對應圖檔
            var DeletePhotoUrl = deletePhotoUrl;
            //未壓縮的原始檔案
            var DeleteSorcePhotoUrl = deletePhotoUrl.Replace(this.CompressPhotonamePrefix, "");

            if (System.IO.File.Exists(deletePhotoUrl))
            {
                System.IO.File.Delete(deletePhotoUrl);
            }
            if(System.IO.File.Exists(DeleteSorcePhotoUrl))
            {
                System.IO.File.Delete(DeleteSorcePhotoUrl);
            }
        }

        /// <summary>
        /// 壓縮圖檔
        /// </summary>
        /// <param name="imagePath">被壓縮圖檔的路徑</param>
        /// <param name="photoName">圖片檔案名稱</param>
        /// <returns>CompressPhotoName壓縮後的檔案名稱</returns>
        private string Compress(string imagePath, string photoName)
        {
            Image image = Image.FromFile(imagePath);

            //取得影像格式
            ImageFormat thisFormat = image.RawFormat;

            int fixWidth = image.Width;
            int fixHeight = image.Height;

            //壓縮最大尺吋
            int maxPx = Convert.ToInt16(250);

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

            string CompressPhotoName = this.CompressPhotonamePrefix + photoName;
            imageOutput.Save(Path.Combine(System.Web.HttpContext.Current.Server.MapPath(this.UploadDir), CompressPhotoName));

            imageOutput.Dispose();
            image.Dispose();

            return CompressPhotoName;
        }

    }
}