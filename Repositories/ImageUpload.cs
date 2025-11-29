using MyBlogApplication.Interfaces;

namespace MyBlogApplication.Repositories
{
    public class ImageUpload : IImageUpload
    {
        /// <summary>
        /// Image upload service for Asp.Net Core Mvc Applications,
        /// takes in a IFormFile and string folder parameter. Folder must
        /// be relative e.g. assets/images
        /// </summary>
        /// <param name="formFile"></param>
        /// <param name="folder"></param>
        /// <returns>Image Url</returns>    
        public async Task<string> UploadImageAsync(IFormFile formFile, string folder)
        {
            // Get full path
            string phsicalPath = Path.Combine("wwwroot", folder);

            // Create directory is it does not exist
            if (!Directory.Exists(phsicalPath))
            {
                Directory.CreateDirectory(phsicalPath);
            }

            // Create file name and path
            var fileName = $"{Guid.NewGuid()}_{formFile.FileName}";
            var filePath = Path.Combine(phsicalPath, fileName);

            // store file to directory
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            }

            // return file image url
            return $"/{folder}/{fileName}";
        }
    }
}
