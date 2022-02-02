using Foreman.Server.Data;
using Foreman.Shared.Services;

namespace Foreman.Server.Services
{
    public class CourseService : ICourseService
    {
        private readonly ApplicationContext _db;
        public CourseService(ApplicationContext db)
        {
            _db = db;   
        }

        public int? GetCategoryId(int courseId)
        {
            return _db.Courses.Find(courseId).CourseCategoryId;
        }
    }
}
