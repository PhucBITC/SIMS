using System.ComponentModel.DataAnnotations;

namespace SIMS_Project.Models
{
    public class Course
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Course Name is required")]
        [Display(Name = "Course Name")]
        public string CourseName { get; set; }

        [Required(ErrorMessage = "Course Code is required")]
        [Display(Name = "Course Code")]
        public string CourseCode { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Instructor is required")]
        [Display(Name = "Instructor")]
        public int InstructorId { get; set; }
    }
}