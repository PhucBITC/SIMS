using CsvHelper;
using CsvHelper.Configuration;
using SIMS_Project.Interface;
using SIMS_Project.Models;
using System.Globalization;

namespace SIMS_Project.Data
{
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly string _filePath = "enrollments.csv";

        public EnrollmentRepository()
        {
            if (!File.Exists(_filePath))
            {
                using (var writer = new StreamWriter(_filePath))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteHeader<Enrollment>();
                    csv.NextRecord();
                }
            }
        }

        public List<Enrollment> GetAll()
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = true, MissingFieldFound = null };
            using (var reader = new StreamReader(_filePath))
            using (var csv = new CsvReader(reader, config))
            {
                return csv.GetRecords<Enrollment>().ToList();
            }
        }

        public void Add(Enrollment enrollment)
        {
            var list = GetAll();
            enrollment.Id = list.Count > 0 ? list.Max(e => e.Id) + 1 : 1;
            list.Add(enrollment);
            WriteFile(list);
        }

        public void Delete(int id)
        {
            var list = GetAll();
            var item = list.FirstOrDefault(e => e.Id == id);
            if (item != null) { list.Remove(item); WriteFile(list); }
        }

        private void WriteFile(List<Enrollment> list)
        {
            using (var writer = new StreamWriter(_filePath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(list);
            }
        }

        // Thêm hàm này vào trong class
        public Enrollment GetById(int id)
        {
            return GetAll().FirstOrDefault(e => e.Id == id);
        }

        // Thêm hàm này vào trong class
        public void Update(Enrollment enrollment)
        {
            var list = GetAll();
            var index = list.FindIndex(e => e.Id == enrollment.Id);
            if (index != -1)
            {
                // Chỉ cập nhật StudentId và CourseId, giữ nguyên ID
                list[index].StudentId = enrollment.StudentId;
                list[index].CourseId = enrollment.CourseId;
                WriteFile(list);
            }
        }
    }
}