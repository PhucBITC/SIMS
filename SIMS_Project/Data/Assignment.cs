using CsvHelper;
using CsvHelper.Configuration;
using SIMS_Project.Interface;
using SIMS_Project.Models;
using System.Globalization;

namespace SIMS_Project.Data
{
    public class AssignmentRepository : BaseCsvRepository<Assignment>, IAssignmentRepository
    {
        public AssignmentRepository(string filePath = "assignments.csv") : base(filePath) { }

        public void Add(Assignment assignment)
        {
            var list = GetAll();
            assignment.Id = list.Count > 0 ? list.Max(x => x.Id) + 1 : 1;
            list.Add(assignment);
            WriteFile(list);
        }

        public List<Assignment> GetByCourse(int courseId)
        {
            return GetAll().Where(a => a.CourseId == courseId).ToList();
        }

        public Assignment GetById(int id)
        {
            return GetAll().FirstOrDefault(a => a.Id == id);
        }

        // Helper to get all records
        private List<Assignment> GetAll()
        {
            if (!File.Exists(_filePath)) return new List<Assignment>();
            var config = new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = true, MissingFieldFound = null };
            using (var reader = new StreamReader(_filePath))
            using (var csv = new CsvReader(reader, config))
            {
                return csv.GetRecords<Assignment>().ToList();
            }
        }

        private void WriteFile(List<Assignment> list)
        {
            using (var writer = new StreamWriter(_filePath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(list);
            }
        }
    }
}