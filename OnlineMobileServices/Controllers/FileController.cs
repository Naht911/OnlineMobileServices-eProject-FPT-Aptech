using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        /// <summary>
        /// Check if the image size is valid (In MBs)
        /// </summary>
        public static bool CheckImageSize(IFormFile image, int MaxSize)
        {
            if (image == null)
            {
                return false;
            }
            if (image.Length > MaxSize * 1024 * 1024)
            {
                return false;
            }
            return true;
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
            byte[] header = new byte[8];
            using (var stream = image.OpenReadStream())
            {
                stream.Read(header, 0, header.Length);
            }
            var imageSignature = Encoding.UTF8.GetString(header);

            var BMP = Encoding.UTF8.GetString(new byte[] { 0x42, 0x4D });
            var GIF87a = Encoding.UTF8.GetString(new byte[] { 0x47, 0x49, 0x46, 0x38, 0x37, 0x61 });
            var GIF89a = Encoding.UTF8.GetString(new byte[] { 0x47, 0x49, 0x46, 0x38, 0x39, 0x61 });
            var PNG = Encoding.UTF8.GetString(new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A });
            var TIFF = Encoding.UTF8.GetString(new byte[] { 0x49, 0x49, 0x2A, 0x00 });
            var TIFF2 = Encoding.UTF8.GetString(new byte[] { 0x4D, 0x4D, 0x00, 0x2A });
            var WEBP = Encoding.UTF8.GetString(new byte[] { 0x52, 0x49, 0x46, 0x46 });           
            var JEPG = Encoding.UTF8.GetString(new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 });
            bool isImage = imageSignature.StartsWith(BMP) || imageSignature.StartsWith(GIF87a) || imageSignature.StartsWith(GIF89a) || imageSignature.StartsWith(PNG) || imageSignature.StartsWith(TIFF) || imageSignature.StartsWith(TIFF2) || imageSignature.StartsWith(WEBP) || imageSignature.StartsWith(JEPG);

            return isImage;
        }

        //check mp3 file
        public static bool CheckMp3IsValid(IFormFile file)
        {
            if (file == null)
            {
                return false;
            }
            var allowedExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                            { ".mp3", ".wav", ".wma", ".flac", ".aac", ".m4a", ".ogg", ".oga", ".opus", ".webm"};

            var extension = Path.GetExtension(file.FileName).ToLower();
            if (!allowedExtensions.Contains(extension))
            {
                return false;
            }
           
            return true;
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

         public static String UploadMp3(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return String.Empty;
            }

            //   //rename file: date + hash file name + extension
            var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var path = Path.Combine("../OnlineMobileServices", "wwwroot/mp3s/uploads", fileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                //Đổi file stream thành file
                file.CopyTo(stream);
            }
            fileName = "/mp3s/uploads/" + fileName;
            return fileName;
        }
    }
}