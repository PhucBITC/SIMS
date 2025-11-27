using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; // Cần cái này để làm Dropdown list
using SIMS_Project.Data;
using SIMS_Project.Models;

namespace SIMS_Project.Controllers
{
    [Authorize(Roles = "Admin")] // Chỉ Admin mới được vào 
    public class EnrollmentController : Controller
    {
        private readonly IEnrollmentRepository _enrollmentRepo;
        private readonly IStudentRepository _studentRepo; // Cần để lấy tên SV
        private readonly ICourseRepository _courseRepo;   // Cần để lấy tên Khóa học

        public EnrollmentController(IEnrollmentRepository eRepo, IStudentRepository sRepo, ICourseRepository cRepo)
        {
            _enrollmentRepo = eRepo;
            _studentRepo = sRepo;
            _courseRepo = cRepo;
        }

        // Hiện danh sách đăng ký
        public IActionResult Index()
        {
            var enrollments = _enrollmentRepo.GetAll();
            var students = _studentRepo.GetAllStudents();
            var courses = _courseRepo.GetAllCourses();

            // Ghép tên Sinh viên và Khóa học vào để hiển thị cho đẹp
            foreach (var e in enrollments)
            {
                var s = students.FirstOrDefault(x => x.Id == e.StudentId);
                var c = courses.FirstOrDefault(x => x.Id == e.CourseId);

                e.StudentName = s != null ? s.FullName : "Không tìm thấy";
                e.CourseName = c != null ? c.CourseName : "Không tìm thấy";
            }

            return View(enrollments);
        }

        // 1. GET: Hiện form đăng ký mới
        public IActionResult Create()
        {
            // Lấy danh sách SV và Khóa học đưa vào ViewBag để bên View tạo Dropdown list
            ViewBag.Students = new SelectList(_studentRepo.GetAllStudents(), "Id", "FullName");
            ViewBag.Courses = new SelectList(_courseRepo.GetAllCourses(), "Id", "CourseName");
            return View();
        }

        // 2. POST: Lưu đăng ký mới
        [HttpPost]
        public IActionResult Create(Enrollment enrollment)
        {
            // Lưu vào CSV
            _enrollmentRepo.Add(enrollment);
            return RedirectToAction("Index");
        }

        // --- CHỨC NĂNG SỬA (EDIT) ---

        // 1. GET: Hiện form sửa (kèm dữ liệu cũ)
        public IActionResult Edit(int id)
        {
            var enrollment = _enrollmentRepo.GetById(id);
            if (enrollment == null) return NotFound();

            // Load lại danh sách sinh viên và khóa học để chọn lại
            // Tham số thứ 4 của SelectList là "SelectedValue" -> Tự động chọn giá trị cũ
            ViewBag.Students = new SelectList(_studentRepo.GetAllStudents(), "Id", "FullName", enrollment.StudentId);
            ViewBag.Courses = new SelectList(_courseRepo.GetAllCourses(), "Id", "CourseName", enrollment.CourseId);

            return View(enrollment);
        }

        // 2. POST: Lưu thay đổi sau khi sửa
        [HttpPost]
        public IActionResult Edit(Enrollment enrollment)
        {
            _enrollmentRepo.Update(enrollment);
            return RedirectToAction("Index");
        }

        // --- CHỨC NĂNG XÓA (DELETE) - Đã cập nhật ---

        // 1. GET: Hiện trang xác nhận xóa (Hiển thị thông tin trước khi xóa)
        public IActionResult Delete(int id)
        {
            var enrollment = _enrollmentRepo.GetById(id);
            if (enrollment == null) return NotFound();

            // Phải lấy tên SV và Khóa học để hiện lên trang xác nhận cho người dùng biết đang xóa ai
            var student = _studentRepo.GetAllStudents().FirstOrDefault(s => s.Id == enrollment.StudentId);
            var course = _courseRepo.GetAllCourses().FirstOrDefault(c => c.Id == enrollment.CourseId);

            enrollment.StudentName = student?.FullName ?? "Unknown";
            enrollment.CourseName = course?.CourseName ?? "Unknown";

            return View(enrollment);
        }

        // 2. POST: Thực hiện xóa thật sự khi bấm nút "Xác nhận Hủy"
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _enrollmentRepo.Delete(id);
            return RedirectToAction("Index");
        }
    }
}