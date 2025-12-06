using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIMS_Project.Interface;
using SIMS_Project.Models;
using System.Security.Claims;

namespace SIMS_Project.Controllers
{
    [Authorize(Roles = "Instructor")]
    public class InstructorController : Controller
    {
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

        // 1. DASHBOARD
        public IActionResult Index()
        {
            int instructorId = GetCurrentUserId();
            var instructor = _userRepo.GetUserById(instructorId);
            var myCourses = _courseRepo.GetCoursesByInstructor(instructorId);

            var myCourseIds = myCourses.Select(c => c.Id).ToList();

            var totalStudents = _enrollmentRepo.GetTotalStudentsForInstructor(myCourseIds);
            var allEnrollments = _enrollmentRepo.GetAll();

            var courseViewModels = myCourses.Select(c => new CourseViewModel
            {
                Id = c.Id,
                CourseName = c.CourseName,
                CourseCode = c.CourseCode,
                Description = c.Description,
                StudentCount = allEnrollments.Count(e => e.CourseId == c.Id),
                AssignmentCount = _assignmentRepo.GetByCourse(c.Id).Count
            }).ToList();

            var viewModel = new InstructorDashboardViewModel
            {
                Instructor = instructor,
                Courses = courseViewModels,
                TotalCourses = myCourses.Count,
                TotalStudents = totalStudents, 
                TotalAssignments = courseViewModels.Sum(c => c.AssignmentCount),
            };

            return View(viewModel);
        }


        public IActionResult CourseManager(int id)
        {
            var course = _courseRepo.GetById(id);
            if (course == null || course.InstructorId != GetCurrentUserId()) return Forbid();

            ViewBag.Assignments = _assignmentRepo.GetByCourse(id);
            ViewBag.Materials = _materialRepo.GetByCourse(id);

            return View(course);
        }

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

        [HttpPost]
        public IActionResult UploadMaterial(int courseId, string title, IFormFile file)
        {
            var course = _courseRepo.GetById(courseId);
            if (course.InstructorId != GetCurrentUserId()) return Forbid();

            if (file != null)
            {
                var fileName = Path.GetFileName(file.FileName);
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

        public IActionResult ViewSubmissions(int assignmentId)
        {
            var assignment = _assignmentRepo.GetById(assignmentId);
            if (assignment == null) return NotFound();

            var course = _courseRepo.GetById(assignment.CourseId);
            if (course.InstructorId != GetCurrentUserId()) return Forbid();

            var submissions = _submissionRepo.GetByAssignment(assignmentId);

            ViewBag.Assignment = assignment;
            ViewBag.CourseId = course.Id;

            return View(submissions);
        }

        [HttpPost]
        public IActionResult GradeSubmission(int submissionId, double score, string feedback)
        {
            var submission = _submissionRepo.GetById(submissionId);
            if (submission != null)
            {
                _submissionRepo.UpdateGrade(submissionId, score, feedback);
                return RedirectToAction("ViewSubmissions", new { assignmentId = submission.AssignmentId });
            }
            return NotFound();
        }

        private int GetCurrentUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            return claim != null ? int.Parse(claim.Value) : 0;
        }
    }
}