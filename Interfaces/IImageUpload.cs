namespace MyBlogApplication.Interfaces
{
    public interface IImageUpload
    {
        Task<string> UploadImageAsync(IFormFile formFile, string folder);        
    }
}
