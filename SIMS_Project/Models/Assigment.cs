using System.ComponentModel.DataAnnotations;

namespace SIMS_Project.Models
{
    public class Assignment
    {
        public int Id { get; set; }
        public int CourseId { get; set; } // Belongs to a specific course

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public DateTime DueDate { get; set; }
    }
}