using System;
using System.IO;

namespace SalesSystem.DataAccess.Files
{
    public class PhotoStorage
    {
        private const string PhotosFolderPath = @"C:\ProductPhotos";

        public string SavePhotoFromBytes(byte[] photoBytes)
        {
            if(photoBytes is null)
            {
                return null;
            }

            var photoFilename = Guid.NewGuid().ToString() + ".jpeg";
            var photoPath = Path.Combine(PhotosFolderPath, photoFilename);

            File.WriteAllBytes(photoPath, photoBytes);

            return photoFilename;
        }

        public byte[] GetPhotoBytesByFileName(string photoFilename)
        {
            if (photoFilename is null)
            {
                return null;
            }

            var photoPath = Path.Combine(PhotosFolderPath, photoFilename);

            if(!File.Exists(photoPath))
            {
                return null;
            }

            var photoBytes = File.ReadAllBytes(photoPath);

            return photoBytes;
        }

        public void DeletePhotoByFileName(string photoFilename)
        {
            if (photoFilename is null)
            {
                return;
            }

            var photoPath = Path.Combine(PhotosFolderPath, photoFilename);

            if (!File.Exists(photoPath))
            {
                return;
            }

            File.Delete(photoPath);
        }
    }
}
