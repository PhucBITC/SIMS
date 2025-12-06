namespace SIMS_Project.Models
{
    public class InstructorDashboardViewModel
    {
        public User Instructor { get; set; }
        public List<CourseViewModel> Courses { get; set; } = new List<CourseViewModel>();

        // Stats
        public int TotalCourses { get; set; }
        public int TotalStudents { get; set; }
        public int TotalAssignments { get; set; } // Placeholder
        public int PendingGrades { get; set; }    // Placeholder
    }

    public class CourseViewModel
    {
        public int Id { get; set; }
        public string CourseName { get; set; }
        public string CourseCode { get; set; }
        public string Description { get; set; }
        public int StudentCount { get; set; }
        public int AssignmentCount { get; set; } // Placeholder
    }
}