namespace SIMS_Project.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string ClassName { get; set; } // Ví dụ: SE1601
    }
}