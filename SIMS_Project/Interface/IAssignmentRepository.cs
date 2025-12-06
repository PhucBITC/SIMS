using SIMS_Project.Models;

namespace SIMS_Project.Interface
{
    public interface IAssignmentRepository
    {
        void Add(Assignment assignment);
        List<Assignment> GetByCourse(int courseId);
        Assignment GetById(int id);
        // ... Update/Delete
    }

    public interface ISubmissionRepository
    {
        void Add(Submission submission);
        void UpdateGrade(int submissionId, double score, string feedback);
        List<Submission> GetByAssignment(int assignmentId);
        Submission GetById(int id);
    }

    public interface IMaterialRepository
    {
        void Add(Material material);
        List<Material> GetByCourse(int courseId);
    }
}