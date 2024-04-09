using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyClass.Model;
using MyClass.DAO;
using UDW.Library;
using System.IO;

namespace UDW.Areas.Admin.Controllers
{
    public class SliderController : Controller
    {
        SlidersDAO slidersDAO = new SlidersDAO();
        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Supplier = INDEX
        public ActionResult Index()
        {
            return View(slidersDAO.getList("Index"));//hien thi toan bo danh sach Slider
        }

        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Slider/Create
        public ActionResult Create()
        {
            ViewBag.OrderList = new SelectList(slidersDAO.getList("Index"), "Order", "Name");
            return View();
        }

        // POST: Admin/Slider/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Sliders sliders)
        {
            if (ModelState.IsValid)
            {
                //Xu ly cho muc Order
                if (sliders.Order == null)
                {
                    sliders.Order = 1;
                }
                else
                {
                    sliders.Order = sliders.Order + 1;
                }

                //Xu ly cho muc Slug
                string slug = XString.Str_Slug(sliders.Name);

                //xu ly cho phan upload hình ảnh
                var img = Request.Files["img"];//lay thong tin file
                if (img.ContentLength != 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
                    //kiem tra tap tin co hay khong
                    if (FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))//lay phan mo rong cua tap tin
                    {
                        //ten file = Slug + phan mo rong cua tap tin
                        string imgName = slug + sliders.Id+ img.FileName.Substring(img.FileName.LastIndexOf("."));
                        sliders.Image = imgName;
                        //upload hinh
                        string PathDir = "~/Public/img/slider/";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        img.SaveAs(PathFile);
                    }
                }//ket thuc phan upload hinh anh

                //Xu ly cho muc CreateAt
                sliders.CreateAt = DateTime.Now;

                //Xu ly cho muc CreateBy
                sliders.CreateBy = Convert.ToInt32(Session["UserId"]);

                slidersDAO.Insert(sliders);

                //Thong bao thanh cong
                TempData["message"] = new XMessage("success", "Thêm danh mục thành công");
                return RedirectToAction("Index");
            }
            ViewBag.OrderList = new SelectList(slidersDAO.getList("Index"), "Order", "Name");
            return View(sliders);
        }

        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Supplier/Staus/5:Thay doi trang thai cua mau tin
        public ActionResult Status(int? id)
        {
            if (id == null)
            {
                //Thong bao that bai
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                //chuyen huong trang
                return RedirectToAction("Index", "Slider");
            }

            //khi nhap nut thay doi Status cho mot mau tin
            Sliders sliders = slidersDAO.getRow(id);
            //kiem tra id cua sliders co ton tai?
            if (sliders == null)
            {
                //Thong bao that bai
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");

                //chuyen huong trang
                return RedirectToAction("Index", "Slider");
            }
            //thay doi trang thai Status tu 1 thanh 2 va nguoc lai
            sliders.Status = (sliders.Status == 1) ? 2 : 1;

            //cap nhat gia tri cho UpdateAt/By
            sliders.UpdateBy = Convert.ToInt32(Session["UserId"].ToString());
            sliders.UpdateAt = DateTime.Now;

            //Goi ham Update trong CategoryDAO
            slidersDAO.Update(sliders);

            //Thong bao thanh cong
            TempData["message"] = new XMessage("success", "Cập nhật trạng thái thành công");

            //khi cap nhat xong thi chuyen ve Index
            return RedirectToAction("Index", "Slider");
        }

        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Slider/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sliders sliders = slidersDAO.getRow(id);
            if (sliders == null)
            {
                return HttpNotFound();
            }
            return View(sliders);
        }

        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Slider/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.OrderList = new SelectList(slidersDAO.getList("Index"), "Order", "Name");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sliders sliders = slidersDAO.getRow(id);
            if (sliders == null)
            {
                return HttpNotFound();
            }
            return View(sliders);
        }

        // POST: Admin/Slider/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Sliders sliders)
        {
            if (ModelState.IsValid)
            {
                //Xu ly cho muc Slug
                string slug = XString.Str_Slug(sliders.Name);
                //chuyen doi dua vao truong Name de loai bo dau, khoang cach = dau -

                //Xu ly cho muc Order
                if (sliders.Order == null)
                {
                    sliders.Order = 1;
                }
                else
                {
                    sliders.Order = sliders.Order + 1;
                }

                //xu ly cho phan upload hình ảnh
                var img = Request.Files["img"];//lay thong tin file
                if (img.ContentLength != 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
                    //kiem tra tap tin co hay khong
                    if (FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))//lay phan mo rong cua tap tin
                    {
                        //ten file = Slug + phan mo rong cua tap tin
                        string imgName = slug + sliders.Id +img.FileName.Substring(img.FileName.LastIndexOf("."));
                        sliders.Image = imgName;
                        //upload hinh
                        string PathDir = "~/Public/img/slider/";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);

                        //cap nhat thi phai xoa file cu
                        //Xoa file
                        if (sliders.Image != null)
                        {
                            string DelPath = Path.Combine(Server.MapPath(PathDir), sliders.Image);
                            System.IO.File.Delete(DelPath);
                        }

                        img.SaveAs(PathFile);
                    }
                }//ket thuc phan upload hinh anh

                //Xu ly cho muc UpdateAt
                sliders.UpdateAt = DateTime.Now;

                //Xu ly cho muc UpdateBy
                sliders.UpdateBy = Convert.ToInt32(Session["UserId"]);

                slidersDAO.Update(sliders);

                //Thong bao thanh cong
                TempData["message"] = new XMessage("success", "Sửa danh mục thành công");
                return RedirectToAction("Index");
            }
            return View(sliders);
        }

        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Slider/DelTrash/5:Thay doi trang thai cua mau tin = 0
        public ActionResult DelTrash(int? id)
        {
            //khi nhap nut thay doi Status cho mot mau tin
            Sliders sliders = slidersDAO.getRow(id);
            //thay doi trang thai Status tu 1,2 thanh 0
            sliders.Status = 0;

            //cap nhat gia tri cho UpdateAt/By
            sliders.UpdateBy = Convert.ToInt32(Session["UserId"].ToString());
            sliders.UpdateAt = DateTime.Now;

            //Goi ham Update trong SupplierDAO
            slidersDAO.Update(sliders);

            //Thong bao thanh cong
            TempData["message"] = new XMessage("success", "Xóa mẩu tin thành công");

            //khi cap nhat xong thi chuyen ve Index
            return RedirectToAction("Index", "Slider");
        }

        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Slider/Trash/5:Hien thi cac mau tin có gia tri la 0
        public ActionResult Trash(int? id)
        {
            return View(slidersDAO.getList("Trash"));
        }

        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Slider/Recover/5:Thay doi trang thai cua mau tin
        public ActionResult Recover(int? id)
        {
            if (id == null)
            {
                //Thong bao that bai
                TempData["message"] = new XMessage("danger", "Phục hồi danh mục thất bại");
                //chuyen huong trang
                return RedirectToAction("Index", "Slider");
            }

            //khi nhap nut thay doi Status cho mot mau tin
            Sliders sliders = slidersDAO.getRow(id);
            //kiem tra id cua topics co ton tai?
            if (sliders == null)
            {
                //Thong bao that bai
                TempData["message"] = new XMessage("danger", "Phục hồi danh mục thất bại");

                //chuyen huong trang
                return RedirectToAction("Index", "Page");
            }
            //thay doi trang thai Status = 2
            sliders.Status = 2;

            //cap nhat gia tri cho UpdateAt/By
            sliders.UpdateBy = Convert.ToInt32(Session["UserId"].ToString());
            sliders.UpdateAt = DateTime.Now;

            //Goi ham Update trong PostsDAO
            slidersDAO.Update(sliders);

            //Thong bao thanh cong
            TempData["message"] = new XMessage("success", "Phục hồi danh mục thành công");

            //khi cap nhat xong thi chuyen ve Trash de phuc hoi tiep
            return RedirectToAction("Trash", "Slider");
        }

        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Slider/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Truy van mau tin theo Id
            Sliders sliders = slidersDAO.getRow(id);

            if (sliders == null)
            {
                return HttpNotFound();
            }
            return View(sliders);
        }

        // POST: Admin/Slider/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //Truy van mau tin theo Id
            Sliders sliders = slidersDAO.getRow(id);

            if (slidersDAO.Delete(sliders) == 1)
            {
                //duong dan den anh can xoa
                string PathDir = "~/Public/img/slider/";
                //cap nhat thi phai xoa file cu
                if (sliders.Image != null)
                {
                    string DelPath = Path.Combine(Server.MapPath(PathDir), sliders.Image);
                    System.IO.File.Delete(DelPath);
                }
            }
            //Thong bao thanh cong
            TempData["message"] = new XMessage("success", "Xóa danh mục thành công");
            //O lai trang thung rac
            return RedirectToAction("Trash");
        }
    }
}
