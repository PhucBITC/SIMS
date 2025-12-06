namespace SIMS_Project.Models
{
    public class Submission
    {
        public int Id { get; set; }
        public int AssignmentId { get; set; }
        public int StudentId { get; set; } // Who submitted it

        public string SubmissionText { get; set; } // Or FilePath
        public DateTime SubmittedAt { get; set; } = DateTime.Now;

        // Grading (Managed by Instructor)
        public double? Score { get; set; } // Nullable because it starts ungraded
        public string Feedback { get; set; }
    }
}