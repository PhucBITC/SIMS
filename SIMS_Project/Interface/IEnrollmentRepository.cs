using SIMS_Project.Models;

namespace SIMS_Project.Interface
{
    public interface IEnrollmentRepository
    {
        List<Enrollment> GetAll();
        void Add(Enrollment enrollment);
        void Delete(int id);
        Enrollment GetById(int id);       // Cần cái này để load dữ liệu cũ lên form sửa
        void Update(Enrollment enrollment); // Hàm sửa
        int GetTotalStudentsForInstructor(List<int> courseIds);
    }
}