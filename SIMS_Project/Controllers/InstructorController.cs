using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIMS_Project.Interface;
using SIMS_Project.Models;
using System.Security.Claims;
using System.Security.Claims;

namespace SIMS_Project.Controllers
{
    [Authorize(Roles = "Instructor")]
    public class InstructorController : Controller
    {
        // We need all these repositories to populate the Dashboard & Manage Content
        private readonly ICourseRepository _courseRepo;
        private readonly IAssignmentRepository _assignmentRepo;
        private readonly ISubmissionRepository _submissionRepo;
        private readonly IMaterialRepository _materialRepo;
        private readonly IUserRepository _userRepo;
        private readonly IEnrollmentRepository _enrollmentRepo;

        public InstructorController(ICourseRepository courseRepo,
                                    IAssignmentRepository assignmentRepo,
                                    ISubmissionRepository submissionRepo,
                                    IMaterialRepository materialRepo,
                                    IUserRepository userRepo,
                                    IEnrollmentRepository enrollmentRepo)
        {
            _courseRepo = courseRepo;
            _assignmentRepo = assignmentRepo;
            _submissionRepo = submissionRepo;
            _materialRepo = materialRepo;
            _userRepo = userRepo;
            _enrollmentRepo = enrollmentRepo;
        }

        // ==========================================
        // 1. DASHBOARD (Fixed to return ViewModel)
        // ==========================================
        public IActionResult Index()
        {
            int instructorId = GetCurrentUserId();
            var instructor = _userRepo.GetUserById(instructorId);
            var myCourses = _courseRepo.GetCoursesByInstructor(instructorId);
            var allEnrollments = _enrollmentRepo.GetAll();

            // Calculate Stats for the Dashboard
            // 1. Total Students (Unique students across all my courses)
            var myCourseIds = myCourses.Select(c => c.Id).ToList();
            var totalStudents = allEnrollments
                                .Where(e => myCourseIds.Contains(e.CourseId))
                                .Select(e => e.StudentId)
                                .Distinct()
                                .Count();

            // 2. Build Course View Models
            var courseViewModels = myCourses.Select(c => new CourseViewModel
            {
                Id = c.Id,
                CourseName = c.CourseName,
                CourseCode = c.CourseCode,
                Description = c.Description,
                StudentCount = allEnrollments.Count(e => e.CourseId == c.Id),
                // Count assignments for this specific course
                AssignmentCount = _assignmentRepo.GetByCourse(c.Id).Count
            }).ToList();

            // 3. Create the Main ViewModel
            var viewModel = new InstructorDashboardViewModel
            {
                Instructor = instructor,
                Courses = courseViewModels,
                TotalCourses = myCourses.Count,
                TotalStudents = totalStudents,
                // Sum of all assignments in my courses
                TotalAssignments = courseViewModels.Sum(c => c.AssignmentCount),
                PendingGrades = 0 // Placeholder logic for now
            };

            return View(viewModel);
        }

        // ==========================================
        // 2. COURSE MANAGER (The "Teacher" View)
        // ==========================================
        public IActionResult CourseManager(int id)
        {
            var course = _courseRepo.GetById(id);
            // Security: Ensure the instructor owns this course
            if (course == null || course.InstructorId != GetCurrentUserId()) return Forbid();

            // Load Content
            ViewBag.Assignments = _assignmentRepo.GetByCourse(id);
            ViewBag.Materials = _materialRepo.GetByCourse(id);

            return View(course);
        }

        // ==========================================
        // 3. CREATE ASSIGNMENT & SET DATE
        // ==========================================
        [HttpPost]
        public IActionResult CreateAssignment(int courseId, string title, DateTime dueDate, string description)
        {
            var course = _courseRepo.GetById(courseId);
            if (course.InstructorId != GetCurrentUserId()) return Forbid();

            var newAsm = new Assignment
            {
                CourseId = courseId,
                Title = title,
                DueDate = dueDate,
                Description = description
            };

            _assignmentRepo.Add(newAsm);
            return RedirectToAction("CourseManager", new { id = courseId });
        }

        // ==========================================
        // 4. UPLOAD MATERIAL
        // ==========================================
        [HttpPost]
        public IActionResult UploadMaterial(int courseId, string title, IFormFile file)
        {
            var course = _courseRepo.GetById(courseId);
            if (course.InstructorId != GetCurrentUserId()) return Forbid();

            if (file != null)
            {
                // Simple file saving logic
                var fileName = Path.GetFileName(file.FileName);
                // Ensure wwwroot/uploads exists
                var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(uploadsPath)) Directory.CreateDirectory(uploadsPath);

                var filePath = Path.Combine(uploadsPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                _materialRepo.Add(new Material { CourseId = courseId, Title = title, FilePath = fileName });
            }

            return RedirectToAction("CourseManager", new { id = courseId });

        }
        // Add this inside the InstructorController class
        private int GetCurrentUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            return claim != null ? int.Parse(claim.Value) : 0;
        }
        // ==========================================
        // 5. GR
    }
}