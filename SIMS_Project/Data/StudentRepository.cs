using CsvHelper;
using CsvHelper.Configuration; // Cần dùng để config CSV
using SIMS_Project.Interface;
using SIMS_Project.Models;
using System.Globalization;
using System.IO;

namespace SIMS_Project.Data
{
    public class StudentRepository : IStudentRepository
    {
        // Bỏ chữ readonly đi để có thể thay đổi đường dẫn nếu cần
        private string _filePath;

        // Constructor: Cho phép truyền tên file vào (nếu không truyền thì mặc định là students.csv)
        // If a relative filename is provided, it's stored under CSV_DATA folder.
        public StudentRepository(string filePath = "students.csv")
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
                    csv.WriteHeader<Student>();
                    csv.NextRecord();
                }
            }
        }

        public List<Student> GetAllStudents()
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true, // File có dòng tiêu đề
            };

            using (var reader = new StreamReader(_filePath))
            using (var csv = new CsvReader(reader, config))
            {
                return csv.GetRecords<Student>().ToList();
            }
        }

        public void AddStudent(Student student)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true, // Ghi tiếp thì phải cẩn thận header
            };

            // Chế độ Append = true (Ghi nối tiếp vào cuối file)
            using (var stream = File.Open(_filePath, FileMode.Append))
            using (var writer = new StreamWriter(stream))
            using (var csv = new CsvWriter(writer, config))
            {
                csv.WriteRecord(student);
                csv.NextRecord(); // Xuống dòng
            }
        }
        // Hàm tìm sinh viên theo ID (Để dùng cho Sửa/Xóa)
        public Student GetById(int id)
        {
            return GetAllStudents().FirstOrDefault(s => s.Id == id);
        }

        // CHỨC NĂNG SỬA: Tìm vị trí -> Thay thế -> Ghi lại toàn bộ file
        public void UpdateStudent(Student student)
        {
            var students = GetAllStudents();
            var index = students.FindIndex(s => s.Id == student.Id);

            if (index != -1)
            {
                students[index] = student; // Thay thế thông tin mới
                WriteFile(students);       // Ghi đè lại file CSV
            }
        }

        // CHỨC NĂNG XÓA: Tìm -> Xóa khỏi list -> Ghi lại toàn bộ file
        public void DeleteStudent(int id)
        {
            var students = GetAllStudents();
            var studentToRemove = students.FirstOrDefault(s => s.Id == id);

            if (studentToRemove != null)
            {
                students.Remove(studentToRemove);
                WriteFile(students); // Ghi đè lại file sau khi xóa
            }
        }

        // --- HÀM PHỤ ĐỂ GHI ĐÈ FILE CSV (Quan trọng) ---
        // Hàm này giúp ghi lại toàn bộ danh sách mới vào file (dùng cho cả Sửa và Xóa)
        private void WriteFile(List<Student> students)
        {
            // FileMode.Create sẽ xóa trắng file cũ và ghi mới
            using (var writer = new StreamWriter(_filePath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(students);
            }
        }
    }
}