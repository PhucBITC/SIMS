using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using SIMS_Project.Interface;
using SIMS_Project.Models;
using System.Security.Claims;

namespace SIMS_Project.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepo;

        public UserController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        // 1. Hiện form đăng nhập
        public IActionResult Login()
        {
            return View();
        }

        // 2. Xử lý đăng nhập
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _userRepo.Login(username, password);

            if (user != null) // Đăng nhập thành công
            {
                // Tạo thông tin định danh (Claim)
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.FullName),
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                };

                var identity = new ClaimsIdentity(claims, "CookieAuth");
                var principal = new ClaimsPrincipal(identity);

                // Ghi Cookie vào trình duyệt (Đăng nhập xong)
                HttpContext.SignInAsync("CookieAuth", principal);

                if (user.Role == "Admin")
                {
                    return RedirectToAction("Index", "Admin");
                }
                else if (user.Role == "Instructor")
                {
                    return RedirectToAction("Index", "Instructor");
                }
                else
                {
                    return RedirectToAction("Index", "Instructor");
                }


            }
            ViewBag.Error = "Sai tên đăng nhập hoặc mật khẩu!";
            return View();
        }

        // 1. GET: Show the Registration Form
        public IActionResult Register()
        {
            return View();
        }

        // 2. POST: Handle the submitted data
        [HttpPost]
        public IActionResult Register(User user)
        {
            // Basic validation
            if (ModelState.IsValid)
            {
                // Check if username already exists (Optional but good practice)
                if (_userRepo.UsernameExists(user.Username))
                {
                    ViewBag.Error = "Username already exists!";
                    return View(user);
                }

                // Save the new user
                // Note: The Repo will handle Auto-Incrementing the ID
                _userRepo.AddUser(user);

                // Redirect to Login page after successful registration
                return RedirectToAction("Login");
            }

            return View(user);
        }

        // 4. Đăng xuất
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync("CookieAuth");
            return RedirectToAction("Index", "Home");
        }

        // 5. Trang báo lỗi khi không có quyền
        public IActionResult AccessDenied()
        {
            return View(); // This looks for Views/User/AccessDenied.cshtml
        }
    }
}