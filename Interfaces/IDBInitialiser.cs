using MyBlogApplication.Data;

namespace MyBlogApplication.Interfaces
{
    public interface IDBInitialiser
    {
        void Initialise(AppDBContext context);
    }
}
