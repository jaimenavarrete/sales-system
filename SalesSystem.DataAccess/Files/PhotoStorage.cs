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
    }
}
