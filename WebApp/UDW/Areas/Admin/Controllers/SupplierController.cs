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
    public class SupplierController : Controller
    {
        private MyDBContext db = new MyDBContext();

        SuppliersDAO suppliersDAO = new SuppliersDAO();
        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Supplier = INDEX
        public ActionResult Index()
        {
            return View(suppliersDAO.getList("Index"));//hien thi toan bo danh sach NCC
        }

        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Supplier/Create
        public ActionResult Create()
        {
            ViewBag.OrderList = new SelectList(suppliersDAO.getList("Index"), "Order", "Name");
            return View();
        }

        // POST: Admin/Supplier/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Suppliers suppliers)
        {
            if (ModelState.IsValid)
            {
                //Xu ly cho muc Slug
                suppliers.Slug = XString.Str_Slug(suppliers.Name);
                //chuyen doi dua vao truong Name de loai bo dau, khoang cach = dau -

                //Xu ly cho muc Order
                if (suppliers.Order == null)
                {
                    suppliers.Order = 1;
                }
                else
                {
                    suppliers.Order = suppliers.Order + 1;
                }

                //xu ly cho phan upload hình ảnh
                var img = Request.Files["img"];//lay thong tin file
                if (img.ContentLength != 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
                    //kiem tra tap tin co hay khong
                    if (FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))//lay phan mo rong cua tap tin
                    {
                        string slug = suppliers.Slug;
                        //ten file = Slug + phan mo rong cua tap tin
                        string imgName = slug + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        suppliers.Image = imgName;
                        //upload hinh
                        string PathDir = "~/Public/img/supplier/";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        img.SaveAs(PathFile);
                    }
                }//ket thuc phan upload hinh anh

                //Xu ly cho muc CreateAt
                suppliers.CreateAt = DateTime.Now;

                //Xu ly cho muc CreateBy
                suppliers.CreateBy = Convert.ToInt32(Session["UserId"]);

                suppliersDAO.Insert(suppliers);

                //Thong bao thanh cong
                TempData["message"] = new XMessage("success", "Thêm danh mục thành công");
                return RedirectToAction("Index");
            }
            ViewBag.OrderList = new SelectList(suppliersDAO.getList("Index"), "Order", "Name");
            return View(suppliers);
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
                return RedirectToAction("Index", "Supplier");
            }

            //khi nhap nut thay doi Status cho mot mau tin
            Suppliers suppliers = suppliersDAO.getRow(id);
            //kiem tra id cua categories co ton tai?
            if (suppliers == null)
            {
                //Thong bao that bai
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");

                //chuyen huong trang
                return RedirectToAction("Index", "Category");
            }
            //thay doi trang thai Status tu 1 thanh 2 va nguoc lai
            suppliers.Status = (suppliers.Status == 1) ? 2 : 1;

            //cap nhat gia tri cho UpdateAt/By
            suppliers.UpdateBy = Convert.ToInt32(Session["UserId"].ToString());
            suppliers.UpdateAt = DateTime.Now;

            //Goi ham Update trong CategoryDAO
            suppliersDAO.Update(suppliers);

            //Thong bao thanh cong
            TempData["message"] = new XMessage("success", "Cập nhật trạng thái thành công");

            //khi cap nhat xong thi chuyen ve Index
            return RedirectToAction("Index", "Supplier");
        }

        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Supplier/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Suppliers suppliers = suppliersDAO.getRow(id);
            if (suppliers == null)
            {
                return HttpNotFound();
            }
            return View(suppliers);
        }

        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Supplier/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.OrderList = new SelectList(suppliersDAO.getList("Index"), "Order", "Name");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Suppliers suppliers = suppliersDAO.getRow(id);
            if (suppliers == null)
            {
                return HttpNotFound();
            }
            return View(suppliers);
        }

        // POST: Admin/Supplier/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Suppliers suppliers)
        {
            if (ModelState.IsValid)
            {
                //Xu ly cho muc Slug
                suppliers.Slug = XString.Str_Slug(suppliers.Name);
                //chuyen doi dua vao truong Name de loai bo dau, khoang cach = dau -

                //Xu ly cho muc Order
                if (suppliers.Order == null)
                {
                    suppliers.Order = 1;
                }
                else
                {
                    suppliers.Order = suppliers.Order + 1;
                }

                //xu ly cho phan upload hình ảnh
                var img = Request.Files["img"];//lay thong tin file
                if (img.ContentLength != 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
                    //kiem tra tap tin co hay khong
                    if (FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))//lay phan mo rong cua tap tin
                    {
                        string slug = suppliers.Slug;
                        //ten file = Slug + phan mo rong cua tap tin
                        string imgName = slug + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        suppliers.Image = imgName;
                        //upload hinh
                        string PathDir = "~/Public/img/supplier/";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);

                        //cap nhat thi phai xoa file cu
                        //Xoa file
                        if (suppliers.Image != null)
                        {
                            string DelPath = Path.Combine(Server.MapPath(PathDir), suppliers.Image);
                            System.IO.File.Delete(DelPath);
                        }

                        img.SaveAs(PathFile);
                    }
                }//ket thuc phan upload hinh anh

                //Xu ly cho muc UpdateAt
                suppliers.UpdateAt = DateTime.Now;

                //Xu ly cho muc UpdateBy
                suppliers.UpdateBy = Convert.ToInt32(Session["UserId"]);

                suppliersDAO.Update(suppliers);

                //Thong bao thanh cong
                TempData["message"] = new XMessage("success", "Sửa danh mục thành công");
                return RedirectToAction("Index");
            }
            return View(suppliers);
        }

        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/DelTrash/5:Thay doi trang thai cua mau tin = 0
        public ActionResult DelTrash(int? id)
        {
            //khi nhap nut thay doi Status cho mot mau tin
            Suppliers suppliers = suppliersDAO.getRow(id);
            //thay doi trang thai Status tu 1,2 thanh 0
            suppliers.Status = 0;

            //cap nhat gia tri cho UpdateAt/By
            suppliers.UpdateBy = Convert.ToInt32(Session["UserId"].ToString());
            suppliers.UpdateAt = DateTime.Now;

            //Goi ham Update trong SupplierDAO
            suppliersDAO.Update(suppliers);

            //Thong bao thanh cong
            TempData["message"] = new XMessage("success", "Xóa mẩu tin thành công");

            //khi cap nhat xong thi chuyen ve Index
            return RedirectToAction("Index", "Supplier");
        }

        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Supplier/Trash/5:Hien thi cac mau tin có gia tri la 0
        public ActionResult Trash(int? id)
        {
            return View(suppliersDAO.getList("Trash"));
        }

        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Supplier/Recover/5:Thay doi trang thai cua mau tin =2
        public ActionResult Recover(int? id)
        {
            if (id == null)
            {
                //Thong bao that bai
                TempData["message"] = new XMessage("danger", "Phục hồi mẩu tin thất bại");
                //chuyen huong trang
                return RedirectToAction("Index", "Supplier");
            }

            //khi nhap nut thay doi Status cho mot mau tin
            Suppliers suppliers = suppliersDAO.getRow(id);
            //kiem tra id cua Supplier co ton tai?
            if (suppliers == null)
            {
                //Thong bao that bai
                TempData["message"] = new XMessage("danger", "Phục hồi mẩu tin thất bại");

                //chuyen huong trang
                return RedirectToAction("Index", "Supplier");
            }
            //thay doi trang thai Status tu 1 thanh 2 va nguoc lai
            suppliers.Status = 2;

            //cap nhat gia tri cho UpdateAt/By
            suppliers.UpdateBy = Convert.ToInt32(Session["UserId"].ToString());
            suppliers.UpdateAt = DateTime.Now;

            //Goi ham Update trong SupplierDAO
            suppliersDAO.Update(suppliers);

            //Thong bao thanh cong
            TempData["message"] = new XMessage("success", "Phục hồi mẩu tin thành công");

            //khi cap nhat xong thi chuyen ve Trash
            return RedirectToAction("Trash", "Supplier");
        }

        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Supplier/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Truy van mau tin theo Id
            Suppliers suppliers = suppliersDAO.getRow(id);

            if (suppliers == null)
            {
                return HttpNotFound();
            }
            return View(suppliers);
        }

        // POST: Admin/Supplier/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //Truy van mau tin theo Id
            Suppliers suppliers = suppliersDAO.getRow(id);

            if (suppliersDAO.Delete(suppliers) == 1)
            {
                //duong dan den anh can xoa
                string PathDir = "~/Public/img/supplier/";
                //cap nhat thi phai xoa file cu
                if (suppliers.Image != null)
                {
                    string DelPath = Path.Combine(Server.MapPath(PathDir), suppliers.Image);
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
