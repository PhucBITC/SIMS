using CsvHelper;
using CsvHelper.Configuration;
using SIMS_Project.Interface;
using SIMS_Project.Models;
using System.Globalization;
using System.IO;

namespace SIMS_Project.Data
{
    public class CourseRepository : ICourseRepository
    {
        private string _filePath;

        // Allow optional file name; if relative it will be placed under CSV_DATA
        public CourseRepository(string filePath = "courses.csv")
        {
            _filePath = Path.IsPathRooted(filePath) ? filePath : Path.Combine("CSV_DATA", filePath);

            var dir = Path.GetDirectoryName(_filePath);
            if (string.IsNullOrEmpty(dir))
            {
                dir = "CSV_DATA";
                _filePath = Path.Combine(dir, Path.GetFileName(_filePath));
            }

            Directory.CreateDirectory(dir);

            if (!File.Exists(_filePath))
            {
                using (var writer = new StreamWriter(_filePath))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteHeader<Course>();
                    csv.NextRecord();
                }
            }
        }

        public List<Course> GetAllCourses()
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = true, MissingFieldFound = null };
            using (var reader = new StreamReader(_filePath))
            using (var csv = new CsvReader(reader, config))
            {
                return csv.GetRecords<Course>().ToList();
            }
        }

        public Course GetById(int id) => GetAllCourses().FirstOrDefault(c => c.Id == id);

        public void AddCourse(Course course)
        {
            var list = GetAllCourses();
            course.Id = list.Count > 0 ? list.Max(c => c.Id) + 1 : 1;
            list.Add(course);
            WriteFile(list);
        }

        public void UpdateCourse(Course course)
        {
            var list = GetAllCourses();
            var index = list.FindIndex(c => c.Id == course.Id);
            if (index != -1) { list[index] = course; WriteFile(list); }
        }

        public void DeleteCourse(int id)
        {
            var list = GetAllCourses();
            var item = list.FirstOrDefault(c => c.Id == id);
            if (item != null) { list.Remove(item); WriteFile(list); }
        }

        private void WriteFile(List<Course> list)
        {
            using (var writer = new StreamWriter(_filePath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(list);
            }
        }
    }
}