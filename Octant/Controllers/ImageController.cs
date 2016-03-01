using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Octant.Models;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Octant.Controllers
{
    public class ImageController : Controller
    {
        public static string GetImage(string dir, string id)
        {
            var container = BlobStorageService.GetCloudBlobContainer();
            var directory = container.GetDirectoryReference(dir).GetDirectoryReference(id);
            var blob = directory.GetBlockBlobReference(id);
            var image = "";
            if (blob.Exists())
            {
                image = blob.Uri.ToString();
            }
            if (image != "") return image;
            var defaultImg = container.GetDirectoryReference(dir).GetDirectoryReference("Default").GetBlockBlobReference("Default.jpg");
            if (defaultImg.Exists())
            {
                image = defaultImg.Uri.ToString();
            }

            return image;
        }

        public static byte[] GetImage(string dir, string id, bool toByteArray)
        {
            //if (!toByteArray) GetImage(dir, id);
            var container = BlobStorageService.GetCloudBlobContainer();
            var blob = container.GetDirectoryReference(dir).GetDirectoryReference(id).GetBlockBlobReference(id);
            
            if (blob.Exists())
            {
                blob.FetchAttributes();
                var image = new byte[blob.Properties.Length];
                blob.DownloadToByteArray(image, 0);
                return image;
            }
            var defaultImg = container.GetDirectoryReference(dir).GetDirectoryReference("Default").GetBlockBlobReference("Default.jpg");
            if (defaultImg.Exists())
            {
                defaultImg.FetchAttributes();
                var image = new byte[defaultImg.Properties.Length];
                defaultImg.DownloadToByteArray(image, 0);
                return image;
            }

            return null;
        }

        [HttpPost]
        public ActionResult SetImage(HttpPostedFileBase fileBase, string dir, string id)
        {
            if (fileBase.ContentLength <= 0) return RedirectToAction("Index", "UserAdmin");
            var container = BlobStorageService.GetCloudBlobContainer();
            var path = dir + "/" + id;
            var directory = container.GetDirectoryReference(path);
            directory.GetBlockBlobReference(id).DeleteIfExists();
            var blob = directory.GetBlockBlobReference(id);
            blob.UploadFromStream(fileBase.InputStream);

            return View("Index", "UserAdmin");
        }
    }
}