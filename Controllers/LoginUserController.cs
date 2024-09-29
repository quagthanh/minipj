using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using minipj.Models;
using System.EnterpriseServices.CompensatingResourceManager;

namespace minipj.Controllers
{
    public class LoginUserController : Controller
    {
        DBSportStoreEntities1 db = new DBSportStoreEntities1();
        // GET: LoginUser
        // Phương thức tạo view cho Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoginAcount(AdminUser _user)
        {
            var check = db.AdminUsers.FirstOrDefault(s => s.UserAccount == _user.UserAccount && s.PasswordUser == _user.PasswordUser);
            if (check == null)
            {
                ViewBag.ErrorInfo = "Sai Info";
                return View("Index");
            }
            else
            {
                Session["ID"] = check.ID; 
                Session["UserName"] = check.UserName;
                Session["PasswordUser"] = check.PasswordUser; 
                return RedirectToAction("Index", "Home");
            }
        }

        // Regíter
        [HttpGet]
        public ActionResult RegisterUser()
        {
            return View();
        }
        [HttpPost]
        public ActionResult RegisterUser(AdminUser _user)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem UserAccount đã tồn tại hay chưa
                var existingUser = db.AdminUsers.FirstOrDefault(u => u.UserAccount == _user.UserAccount);
                if (existingUser != null)
                {
                    ViewBag.ErrorRegister = "Email này đã được sử dụng!";
                    return View();
                }

                db.Configuration.ValidateOnSaveEnabled = false;
                db.AdminUsers.Add(_user);
                db.SaveChanges();
                Session["UserName"] = _user.UserName;
                ViewBag.Message = "Registered successfully";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ErrorRegister = "Đăng ký không thành công!";
            }
            return View();
        }
        public ActionResult Logout()
        {
            Session["UserName"] = null; // Xóa tên người dùng khỏi session
            return RedirectToAction("Index", "Home"); // Chuyển hướng về trang chủ
        }


    }
}
