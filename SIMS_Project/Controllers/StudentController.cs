using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIMS_Project.Interface;
using SIMS_Project.Models;

namespace SIMS_Project.Controllers
{
    [Authorize(Roles = "Admin")] //Chỉ Admin mới được vào
    public class StudentController : Controller
    {
        // Khai báo biến để chứa Repository
        private readonly IStudentRepository _studentRepository;

        // Constructor Injection (Đây là cách Controller nhận "công cụ" từ Program.cs)
        public StudentController(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        // Action hiển thị danh sách (Mặc định khi vào /Student)
        public IActionResult Index()
        {
            // 1. Gọi Repository lấy danh sách từ CSV
            var students = _studentRepository.GetAllStudents();

            // 2. Đưa danh sách đó sang View
            return View(students);
        }

        // 1. GET: Hiển thị form nhập liệu
        public IActionResult Create()
        {
            return View();
        }

        // 2. POST: Xử lý dữ liệu khi người dùng bấm nút "Lưu"
        [HttpPost]
        public IActionResult Create(Student student)
        {
            if (ModelState.IsValid)
            {
                _studentRepository.AddStudent(student); // Repository handles the ID
                return RedirectToAction("Index");
            }
            return View(student);
        }

        // --- CHỨC NĂNG SỬA (EDIT) ---

        // 1. GET: Hiển thị form với dữ liệu cũ
        public IActionResult Edit(int id)
        {
            var student = _studentRepository.GetById(id);
            if (student == null)
            {
                return NotFound(); // Không tìm thấy thì báo lỗi
            }
            return View(student);
        }

        // 2. POST: Lưu dữ liệu đã sửa
        [HttpPost]
        public IActionResult Edit(Student student)
        {
            if (ModelState.IsValid)
            {
                _studentRepository.UpdateStudent(student);
                return RedirectToAction("Index"); // Quay về trang chủ
            }
            return View(student); // Nếu lỗi thì hiện lại form
        }

        // --- CHỨC NĂNG XÓA (DELETE) ---

        // 1. GET: Hiển thị trang xác nhận xóa
        public IActionResult Delete(int id)
        {
            var student = _studentRepository.GetById(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // 2. POST: Thực hiện xóa thật sự
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _studentRepository.DeleteStudent(id);
            return RedirectToAction("Index");
        }
    }
}