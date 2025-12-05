using CsvHelper;
using System.Globalization;
using System.IO;

namespace SIMS_Project.Data
{
    // The <T> allows this to work for Course, Student, User, etc.
    public abstract class BaseCsvRepository<T>
    {
        protected string _filePath;

        public BaseCsvRepository(string filePath)
        {
            // 1. Resolve the absolute path
            _filePath = Path.IsPathRooted(filePath) ? filePath : Path.Combine("CSV_DATA", filePath);

            var dir = Path.GetDirectoryName(_filePath);

            // 2. Default to CSV_DATA if no directory is provided
            if (string.IsNullOrEmpty(dir))
            {
                dir = "CSV_DATA";
                _filePath = Path.Combine(dir, Path.GetFileName(_filePath));
            }

            // 3. Create Directory if it doesn't exist
            Directory.CreateDirectory(dir);

            // 4. Create File and Header if it doesn't exist
            if (!File.Exists(_filePath))
            {
                using (var writer = new StreamWriter(_filePath))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteHeader<T>(); // This writes the header for Course, Student, etc.
                    csv.NextRecord();
                }
            }
        }
    }
}