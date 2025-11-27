namespace SIMS_Project.Models
{
    public class Enrollment
    {
        public int Id { get; set; }
        public int StudentId { get; set; } // Khóa ngoại trỏ về Sinh viên
        public int CourseId { get; set; }  // Khóa ngoại trỏ về Khóa học

        // Hai trường này dùng để lưu tạm tên hiển thị lên màn hình (không cần lưu vào CSV cũng được)
        public string StudentName { get; set; }
        public string CourseName { get; set; }
    }
}