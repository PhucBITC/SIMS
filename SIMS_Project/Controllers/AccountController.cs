using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using SIMS_Project.Data;
using System.Security.Claims;

namespace SIMS_Project.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository _userRepo;

        public AccountController(IUserRepository userRepo)
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
                    new Claim(ClaimTypes.Role, user.Role)
                };

                var identity = new ClaimsIdentity(claims, "CookieAuth");
                var principal = new ClaimsPrincipal(identity);

                // Ghi Cookie vào trình duyệt (Đăng nhập xong)
                HttpContext.SignInAsync("CookieAuth", principal);

                return RedirectToAction("Index", "Home"); // Chuyển về trang chủ
            }

            ViewBag.Error = "Sai tên đăng nhập hoặc mật khẩu!";
            return View();
        }

        // 3. Đăng xuất
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync("CookieAuth");
            return RedirectToAction("Login");
        }

        // 4. Trang báo lỗi khi không có quyền
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}