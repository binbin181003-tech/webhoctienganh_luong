using Microsoft.EntityFrameworkCore;
using webhoctienganh.Data;
using webhoctienganh.Models;

var builder = WebApplication.CreateBuilder(args);

// Add Razor Pages
builder.Services.AddRazorPages();

// Add Database
builder.Services.AddDbContext<du_lieu>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Tao database + seed admin/teacher
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<du_lieu>();
    db.Database.EnsureCreated();

    // === Seed Admin ===
    if (!db.nguoi_dung.Any(n => n.email == "admin@web.com"))
    {
        var adminId = "seed-admin-001";
        db.nguoi_dung.Add(new nguoi_dung
        {
            ma_nguoi_dung = adminId,
            ho_ten = "Admin He Thong",
            email = "admin@web.com",
            so_dien_thoai = "0900000001",
            mat_khau_hash = BCrypt.Net.BCrypt.HashPassword("234234"),
            ngay_tao = DateTime.Now,
            trang_thai = "active"
        });
        db.nguoi_dung_vai_tro.Add(new nguoi_dung_vai_tro
        {
            ma_nguoi_dung = adminId,
            ma_vai_tro = "admin"
        });
    }

    db.SaveChanges();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();
app.MapRazorPages();

app.Run();