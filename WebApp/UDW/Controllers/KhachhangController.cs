using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UDW.Library;
using MyClass.DAO;
using MyClass.Model;

namespace UDW.Controllers
{
    public class KhachhangController : Controller
    {
        //////////////////////////////////////////////////////////////////////////
        // GET: Khachhang DangNhap
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection field)
        {
            UsersDAO usersDAO = new UsersDAO();

            String username = field["username"];
            String password = XString.ToMD5(field["password"]);
            //so sanh thong tin nguoi dung
            Users row_user = usersDAO.getRow(username,"customer");
            String strErr = "";
            if (row_user==null)
            {
                strErr = "Tên đăng nhập không tồn tại";
            }
            ViewBag.Error = "<span class='text-danger'>"+strErr+"</div";
            return View("DangNhap");
        }

        //////////////////////////////////////////////////////////////////////////
        // GET: Khachhang DangKy
        public ActionResult DangKy()
        {
            return View();
        }
    }
}