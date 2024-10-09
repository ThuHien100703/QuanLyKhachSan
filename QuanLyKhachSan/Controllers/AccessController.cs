using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using QuanLyKhachSan.Models;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace QuanLyKhachSan.Controllers
{
    public class AccessController : Controller
    {

        QuanLyKhachSanContext db = new QuanLyKhachSanContext();
        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("Email") == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public IActionResult Login(Nhanvien nv)
        {
            if (HttpContext.Session.GetString("Email") == null)
            {
                var u = db.Nhanviens
                    .Where(x => x.Email.Equals(nv.Email) && x.Pass.Equals(nv.Pass))
                    .FirstOrDefault();

                if (u != null)
                {
                    HttpContext.Session.SetString("Email", u.Email.ToString());

                    // Kiểm tra đuôi email
                    if (u.Email.EndsWith("@ql.com"))
                    {
                        return RedirectToAction("Index", "Manage");
                    }
                    else if (u.Email.EndsWith("@nv.com"))
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                    else
                    {
                        // Xử lý trường hợp email không hợp lệ
                        ModelState.AddModelError("", "Email không hợp lệ.");
                        return View();
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
                }
            }
            else
            {
                return RedirectToAction("Index", "Admin");
            }

            return View();
        }

        public IActionResult Logout()
        {
            // Clear the session
            HttpContext.Session.Remove("Email");

            // Redirect to the Login action
            return RedirectToAction("Login");
        }
    }
}