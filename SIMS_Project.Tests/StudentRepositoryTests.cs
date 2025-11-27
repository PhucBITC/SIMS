using Xunit;
using SIMS_Project.Data;
using SIMS_Project.Models;
using System.IO;
using System.Linq;
using System;

namespace SIMS_Project.Tests
{
    public class StudentRepositoryTests
    {
        [Fact]
        public void AddStudent_ShouldIncreaseCount_WhenStudentIsAdded()
        {
            // 1. Arrange (Chuẩn bị)
            // Dùng file riêng để test, không đụng vào dữ liệu thật
            string testFilePath = "students_test.csv";

            // Xóa file test cũ nếu có để đảm bảo sạch sẽ
            if (File.Exists(testFilePath)) File.Delete(testFilePath);

            // Khởi tạo Repository với file test
            var repo = new StudentRepository(testFilePath);

            var newStudent = new Student
            {
                FullName = "Test User",
                DateOfBirth = DateTime.Now,
                Email = "test@gmail.com",
                ClassName = "TEST01"
            };

            // 2. Act (Hành động: Thêm sinh viên)
            repo.AddStudent(newStudent);

            // 3. Assert (Kiểm tra kết quả)
            var list = repo.GetAllStudents();

            // Kiểm tra xem danh sách có đúng là 1 người không
            Assert.Single(list);

            // Kiểm tra xem tên người đó có đúng là "Test User" không
            Assert.Equal("Test User", list.First().FullName);

            // Dọn dẹp: Xóa file test sau khi xong
            if (File.Exists(testFilePath)) File.Delete(testFilePath);
        }
    }
}