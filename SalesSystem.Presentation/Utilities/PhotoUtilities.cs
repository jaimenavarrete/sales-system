using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;

namespace SalesSystem.Presentation.Utilities
{
    public static class PhotoUtilities
    {
        public static byte[] ConvertPhotoToJpegBytes(HttpPostedFileBase photo)
        {
            if (photo is null)
            {
                return null;
            }

            using(var oldPhoto = new MemoryStream())
            using(var newPhoto = new MemoryStream())
            {
                photo.InputStream.CopyTo(oldPhoto);

                using(var imageStream = Image.FromStream(oldPhoto))
                {
                    imageStream.Save(newPhoto, ImageFormat.Jpeg);

                    return newPhoto.ToArray();
                }
            }
        }

        public static byte[] ConvertPhotoToBytes(HttpPostedFileBase photo)
        {
            if (photo is null)
            {
                return null;
            }

            using(var reader = new BinaryReader(photo.InputStream))
            {
                byte[] photoBytes = reader.ReadBytes((int)photo.ContentLength);

                return photoBytes;
            }
        }

        public static string ConvertBytesToBase64(byte[] photoBytes)
        {
            if (photoBytes is null)
            {
                return null;
            }

            var base64String = Convert.ToBase64String(photoBytes);
            var photoBase64 = "data:image/jpeg;base64," + base64String;

            return photoBase64;
        }
    }
}