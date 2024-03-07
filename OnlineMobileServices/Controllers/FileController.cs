using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace OnlineMobileServices.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class FileController
    {

        public FileController()
        {
        }

        public static bool CheckImageIsValid(IFormFile image)
        {
            if (image == null)
            {
                return false;
            }

            var allowedExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                            { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tif", ".tiff", ".webp" };

            var extension = Path.GetExtension(image.FileName);
            if (!allowedExtensions.Contains(extension))
            {
                return false;
            }

            var allowedMimeTypes = new HashSet<string> { "image/jpeg", "image/png", "image/gif" };
            return allowedMimeTypes.Contains(image.ContentType);
        }

        public static String UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return String.Empty;
            }

            //   //rename file: date + hash file name + extension
            var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var path = Path.Combine("../OnlineMobileServices", "wwwroot/images/uploads", fileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                //Đổi file stream thành file
                file.CopyTo(stream);
            }
            fileName = "/images/uploads/" + fileName;
            return fileName;
        }
    }
}