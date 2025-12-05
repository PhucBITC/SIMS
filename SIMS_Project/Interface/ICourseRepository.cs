using SIMS_Project.Models;

namespace SIMS_Project.Interface
{
    public interface ICourseRepository
    {
        List<Course> GetAllCourses();
        Course GetById(int id);
        void AddCourse(Course course);
        void UpdateCourse(Course course);
        void DeleteCourse(int id);
    }
}