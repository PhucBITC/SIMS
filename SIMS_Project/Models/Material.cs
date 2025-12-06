namespace SIMS_Project.Models
{
    public class Material
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string Title { get; set; }
        public string FilePath { get; set; } // Path to the uploaded file
    }
}