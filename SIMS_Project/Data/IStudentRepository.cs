using SIMS_Project.Models;

namespace SIMS_Project.Data
{
    public interface IStudentRepository
    {
        List<Student> GetAllStudents();
        void AddStudent(Student student);
        // Sau này sẽ thêm Update, Delete ở đây
        // --- BẠN ĐANG THIẾU 3 DÒNG NÀY ---
        Student GetById(int id);           // Để tìm sinh viên
        void UpdateStudent(Student student); // Để sửa
        void DeleteStudent(int id);        // Để xóa (Cái bạn đang lỗi)
    }
}