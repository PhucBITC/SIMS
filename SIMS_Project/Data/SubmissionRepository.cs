using CsvHelper;
using CsvHelper.Configuration;
using SIMS_Project.Interface;
using SIMS_Project.Models;
using System.Globalization;

namespace SIMS_Project.Data
{
    public class SubmissionRepository : BaseCsvRepository<Submission>, ISubmissionRepository
    {
        public SubmissionRepository(string filePath = "submissions.csv") : base(filePath) { }

        public void Add(Submission submission)
        {
            var list = GetAll();
            submission.Id = list.Count > 0 ? list.Max(x => x.Id) + 1 : 1;
            list.Add(submission);
            WriteFile(list);
        }

        public List<Submission> GetByAssignment(int assignmentId)
        {
            return GetAll().Where(s => s.AssignmentId == assignmentId).ToList();
        }

        public Submission GetById(int id)
        {
            return GetAll().FirstOrDefault(s => s.Id == id);
        }

        public void UpdateGrade(int submissionId, double score, string feedback)
        {
            var list = GetAll();
            var sub = list.FirstOrDefault(s => s.Id == submissionId);
            if (sub != null)
            {
                sub.Score = score;
                sub.Feedback = feedback;
                // sub.GradedAt = DateTime.Now; // Optional if you added this field
                WriteFile(list);
            }
        }

        private List<Submission> GetAll()
        {
            if (!File.Exists(_filePath)) return new List<Submission>();
            var config = new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = true, MissingFieldFound = null };
            using (var reader = new StreamReader(_filePath))
            using (var csv = new CsvReader(reader, config))
            {
                return csv.GetRecords<Submission>().ToList();
            }
        }

        private void WriteFile(List<Submission> list)
        {
            using (var writer = new StreamWriter(_filePath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(list);
            }
        }
    }
}