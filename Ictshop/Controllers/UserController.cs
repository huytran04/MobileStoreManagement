using System;
using System.Linq;
using System.Web.Mvc;
using Ictshop.Models;
using BCrypt.Net;
using System.Data.Entity.Validation;
using System.Web.Helpers;
using Ictshop.fonts.WEB_NC_QUOCHUY_V1.Ictshop.Ictshop.Models;

namespace Ictshop.Controllers
{
    public class UserController : Controller
    {
        QLdienthoaiEntitiess db = new QLdienthoaiEntitiess();

        // ĐĂNG KÝ
        public ActionResult Dangky()
        {
            return View();
        }

        // ĐĂNG KÝ PHƯƠNG THỨC POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Dangky(Nguoidung nguoidung)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Kiểm tra nếu người dùng đã tồn tại
                    Nguoidung existingUser = db.Nguoidungs.FirstOrDefault(m => m.Email == nguoidung.Email);
                    if (existingUser == null)
                    {
                        // Mã hóa mật khẩu trước khi lưu
                        string salt = BCrypt.Net.BCrypt.GenerateSalt();
                        nguoidung.Matkhau = BCrypt.Net.BCrypt.HashPassword(nguoidung.Matkhau, salt);

                        // Thêm người dùng mới
                        db.Nguoidungs.Add(nguoidung);

                        // Lưu lại vào cơ sở dữ liệu
                        db.SaveChanges();

                        // Nếu dữ liệu đúng thì trả về trang đăng nhập
                        return RedirectToAction("Dangnhap");
                    }
                    else
                    {
                        ViewBag.TrungLap = "Email đã được sử dụng cho tài khoản khác.";
                        return View(nguoidung);
                    }
                }
                catch (DbEntityValidationException ex)
                {
                    // In ra chi tiết lỗi validation
                    foreach (var validationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            ModelState.AddModelError("", $"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");
                            Console.WriteLine($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Có lỗi xảy ra: " + ex.Message);
                }
            }
            return View(nguoidung);
        }

        // ĐĂNG NHẬP
        public ActionResult Dangnhap()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Dangnhap(FormCollection userlog)
        {
            string userMail = userlog["userMail"].ToString();
            string password = userlog["password"].ToString();
            var user = db.Nguoidungs.SingleOrDefault(x => x.Email.Equals(userMail));

            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Matkhau))
            {
                if (userMail == "Admin@gmail.com")
                {
                    Session["use"] = user;
                    return RedirectToAction("Index", "Admin/Home");
                }
                else
                {
                    Session["use"] = user;
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ViewBag.Fail = "Đăng nhập thất bại";
                return View("Dangnhap");
            }
        }

        // ĐĂNG XUẤT
        public ActionResult DangXuat()
        {
            Session["use"] = null;
            return RedirectToAction("Index", "Home");
        }
    }
}
