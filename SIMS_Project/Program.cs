var builder = WebApplication.CreateBuilder(args);
// Đăng ký dịch vụ Repository
// Scoped: Mỗi lần có request HTTP mới sẽ tạo lại Repository này
builder.Services.AddScoped<SIMS_Project.Data.IStudentRepository, SIMS_Project.Data.StudentRepository>();
builder.Services.AddScoped<SIMS_Project.Data.ICourseRepository, SIMS_Project.Data.CourseRepository>();
builder.Services.AddScoped<SIMS_Project.Data.IEnrollmentRepository, SIMS_Project.Data.EnrollmentRepository>();
builder.Services.AddScoped<SIMS_Project.Data.IUserRepository, SIMS_Project.Data.UserRepository>();

// Add services to the container.
builder.Services.AddControllersWithViews();
// Cấu hình Đăng nhập bằng Cookie
builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", options =>
    {
        options.LoginPath = "/Account/Login"; // Chưa đăng nhập thì đá về trang này
        options.AccessDeniedPath = "/Account/AccessDenied";
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication(); // Bật tính năng xác thực
app.UseAuthorization();  // Bật tính năng phân quyền

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
