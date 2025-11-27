using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIMS_Project.Data;
using SIMS_Project.Models;

namespace SIMS_Project.Controllers
{
    [Authorize(Roles = "Admin")] //Chỉ Admin mới được vào
    public class CourseController : Controller
    {
        private readonly ICourseRepository _repo;

        public CourseController(ICourseRepository repo)
        {
            _repo = repo;
        }

        public IActionResult Index() => View(_repo.GetAllCourses());

        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(Course course)
        {
            if (ModelState.IsValid) { _repo.AddCourse(course); return RedirectToAction("Index"); }
            return View(course);
        }

        public IActionResult Edit(int id)
        {
            var c = _repo.GetById(id);
            return c == null ? NotFound() : View(c);
        }

        [HttpPost]
        public IActionResult Edit(Course course)
        {
            if (ModelState.IsValid) { _repo.UpdateCourse(course); return RedirectToAction("Index"); }
            return View(course);
        }

        public IActionResult Delete(int id)
        {
            var c = _repo.GetById(id);
            return c == null ? NotFound() : View(c);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _repo.DeleteCourse(id);
            return RedirectToAction("Index");
        }
    }
}