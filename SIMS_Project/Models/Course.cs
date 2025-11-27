using System.ComponentModel.DataAnnotations;

namespace SIMS_Project.Models
{
    public class Course
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên khóa học")]
        [Display(Name = "Tên khóa học")]
        public string CourseName { get; set; } // Ví dụ: Lập trình C#

        [Required(ErrorMessage = "Vui lòng nhập mã môn")]
        [Display(Name = "Mã môn")]
        public string CourseCode { get; set; } // Ví dụ: PRN211

        [Required]
        [Range(1, 5, ErrorMessage = "Số tín chỉ từ 1 đến 5")]
        [Display(Name = "Số tín chỉ")]
        public int Credits { get; set; }
    }
}