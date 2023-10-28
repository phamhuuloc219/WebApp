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
using WebAppIT5.Library;
using System.IO;

namespace WebAppIT5.Areas.Admin.Controllers
{
    public class SupplierController : Controller
    {
        SuppliersDAO suppliersDAO = new SuppliersDAO();

        ///////////////////////////////////////////////////////////////////
        // GET: Admin/Supplier/Index
        public ActionResult Index()
        {//hien thi cac mau tin o trang Index (status = 1,2 hay DAO = Index)
            return View(suppliersDAO.getList("Index"));
        }

        ///////////////////////////////////////////////////////////////////
        // GET: Admin/Supplier/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                //thong bao that bai
                TempData["message"] = new XMessage("danger", "Không tìm thấy mẩu tin");
            }
            Suppliers suppliers = suppliersDAO.getRow(id);//hien thi 1 mau tin
            if (suppliers == null)
            {
                //thong bao that bai
                TempData["message"] = new XMessage("danger", "Không tìm thấy mẩu tin");
            }
            return View(suppliers);
        }

        ///////////////////////////////////////////////////////////////////
        // GET: Admin/Supplier/Create
        public ActionResult Create()
        {
            ViewBag.ListOrder = new SelectList(suppliersDAO.getList("Index"), "Order", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Suppliers suppliers)
        {
            if (ModelState.IsValid)
            {
                //xu ly tu dong cho cac truong: CreateAt/By, UpdateAt/By, Slug, OrderBy
                //Xu ly tu dong cho: CreateAt
                suppliers.CreateAt = DateTime.Now;

                //Xu ly tu dong cho: UpdateAt
                suppliers.UpdateAt = DateTime.Now;

                //Xu ly tu dong cho: Order
                if (suppliers.Order == null)
                {
                    suppliers.Order = 1;
                }
                else
                {
                    suppliers.Order += 1;
                }

                //Xu ly tu dong cho: Slug
                suppliers.Slug = XString.Str_Slug(suppliers.Name);

                //xu ly cho phan upload hinh anh
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
                        suppliers.Image = imgName;//abc-def-1.jpg
                        //upload hinh
                        string PathDir = "~/Public/img/supplier/";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        img.SaveAs(PathFile);
                    }
                }//ket thuc phan upload hinh anh

                //them mau tin vao DB
                suppliersDAO.Insert(suppliers);
                //thong bao thanh cong
                TempData["message"] = new XMessage("success", "Thêm mẩu tin thành công");
                return RedirectToAction("Index");
            }
            ViewBag.ListOrder = new SelectList(suppliersDAO.getList("Index"), "Order", "Name");
            return View(suppliers);
        }

        ///////////////////////////////////////////////////////////////////
        // GET: Admin/Supplier/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                //thong bao that bai
                TempData["message"] = new XMessage("danger", "Không tìm thấy mẩu tin");
            }
            Suppliers suppliers = suppliersDAO.getRow(id);//tra ve 1 mau tin
            if (suppliers == null)
            {
                //thong bao that bai
                TempData["message"] = new XMessage("danger", "Không tìm thấy mẩu tin");
            }
            ViewBag.ListOrder = new SelectList(suppliersDAO.getList("Index"), "Order", "Name");
            return View(suppliers);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Suppliers suppliers)
        {
            if (ModelState.IsValid)
            {
                //xu ly tu dong cho cac truong: CreateAt/By, UpdateAt/By, Slug, OrderBy

                //Xu ly tu dong cho: UpdateAt
                suppliers.UpdateAt = DateTime.Now;

                //Xu ly tu dong cho: Order
                if (suppliers.Order == null)
                {
                    suppliers.Order = 1;
                }
                else
                {
                    suppliers.Order += 1;
                }

                //Xu ly tu dong cho: Slug
                suppliers.Slug = XString.Str_Slug(suppliers.Name);

                //truoc khi cap nhat lai anh moi thi xoa anh cu
                var img = Request.Files["img"];//lay thong tin file
                string PathDir = "~/Public/img/supplier/";
                if (img.ContentLength != 0 && suppliers.Image != null)//ton tai mot logo cua NCC tu truoc
                {
                    //xoa anh cu
                    string DelPath = Path.Combine(Server.MapPath(PathDir), suppliers.Image);
                    System.IO.File.Delete(DelPath);
                }
                //upload anh moi cua NCC
                //xu ly cho phan upload hinh anh

                if (img.ContentLength != 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
                    //kiem tra tap tin co hay khong
                    if (FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))//lay phan mo rong cua tap tin
                    {
                        string slug = suppliers.Slug;
                        //ten file = Slug + phan mo rong cua tap tin
                        string imgName = slug + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        suppliers.Image = imgName;//abc-def-1.jpg
                        //upload hinh
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        img.SaveAs(PathFile);
                    }
                }//ket thuc phan upload hinh anh

                //cap nhat mau tin vao DB
                suppliersDAO.Update(suppliers);
                //thong bao thanh cong
                TempData["message"] = new XMessage("success", "Cập nhật mẩu tin thành công");
                return RedirectToAction("Index");
            }
            return View(suppliers);
        }

        ///////////////////////////////////////////////////////////////////
        // GET: Admin/Supplier/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                //thong bao that bai
                TempData["message"] = new XMessage("danger", "Không tìm thấy mẩu tin");
            }
            Suppliers suppliers = suppliersDAO.getRow(id);//tim kiem 1 mau tin co Id = id
            if (suppliers == null)
            {
                //thong bao that bai
                TempData["message"] = new XMessage("danger", "Không tìm thấy mẩu tin");
            }
            return View(suppliers);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Suppliers suppliers = suppliersDAO.getRow(id);//tim kiem 1 mau tin co Id = id
                                                          //xoa hinh anh
                                                          //truoc khi cap nhat lai anh moi thi xoa anh cu
            var img = Request.Files["img"];//lay thong tin file
            string PathDir = "~/Public/img/supplier/";
            if (img.ContentLength != 0 && suppliers.Image != null)//ton tai mot logo cua NCC tu truoc
            {
                //xoa anh cu
                string DelPath = Path.Combine(Server.MapPath(PathDir), suppliers.Image);
                System.IO.File.Delete(DelPath);
            }

            //thong bao thanh cong
            TempData["message"] = new XMessage("success", "Xóa mẩu tin thành công");
            return RedirectToAction("Index");
        }

        ///////////////////////////////////////////////////////////////////
        // GET: Admin/Supplier/Status/5
        public ActionResult Status(int? id)
        {//khong lien quan den hinh anh
            if (id == null)
            {
                //thong bao that bai
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");
            }

            //tim row co id == id cua Nha CC can thay doi Status
            Suppliers suppliers = suppliersDAO.getRow(id);
            if (suppliers == null)
            {
                //thong bao that bai
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");
            }
            //kiem tra trang thai cua status, neu hien tai la 1 ->2 va nguoc lai
            suppliers.Status = (suppliers.Status == 1) ? 2 : 1;
            //cap nhat gia tri cho UpdateAt
            suppliers.UpdateAt = DateTime.Now;
            //cap nhat lai DB
            suppliersDAO.Update(suppliers);
            //thong bao thanh cong
            TempData["message"] = new XMessage("success", "Cập nhật trạng thái thành công");
            //tra ket qua ve Index
            return RedirectToAction("Index");
        }

        ///////////////////////////////////////////////////////////////////
        /// MoveTrash
        // GET: Admin/Supplier/MoveTrash/5
        public ActionResult MoveTrash(int? id)
        {//chua lien quan den hinh anh
            if (id == null)
            {
                //thong bao that bai
                TempData["message"] = new XMessage("danger", "Xóa mẩu tin thất bại");
                return RedirectToAction("Index");
            }

            //tim row co id == id cua loai SP can thay doi Status
            Suppliers suppliers = suppliersDAO.getRow(id);
            if (suppliers == null)
            {
                //thong bao that bai
                TempData["message"] = new XMessage("danger", "Xóa mẩu tin thất bại");
                return RedirectToAction("Index");
            }
            //trang thai cua status = 0
            suppliers.Status = 0;
            //cap nhat gia tri cho UpdateAt
            suppliers.UpdateAt = DateTime.Now;

            //cap nhat lai DB
            suppliersDAO.Update(suppliers);
            //thong bao thanh cong
            TempData["message"] = new XMessage("success", "Mẩu tin được chuyển vào thùng rác");
            //tra ket qua ve Index
            return RedirectToAction("Index");
        }

        ///////////////////////////////////////////////////////////////////
        // GET: Admin/Supplier/Trash
        public ActionResult Trash()
        {
            return View(suppliersDAO.getList("Trash"));//chi hien thi cac dong co status 0
        }

        ///////////////////////////////////////////////////////////////////
        /// Recover: Khong lien quan den hinh anh
        // GET: Admin/Supplier/Recover/5
        public ActionResult Recover(int? id)
        {
            if (id == null)
            {
                //thong bao that bai
                TempData["message"] = new XMessage("danger", "Phục hồi mẩu tin thất bại");
                return RedirectToAction("Index");
            }
            //tim row co id == id cua loai SP can thay doi Status
            Suppliers suppliers = suppliersDAO.getRow(id);
            if (suppliers == null)
            {
                //thong bao that bai
                TempData["message"] = new XMessage("danger", "Phục hồi mẩu tin thất bại");
                return RedirectToAction("Index");
            }
            //trang thai cua status = 2
            suppliers.Status = 2;//truoc recover=0
            //cap nhat gia tri cho UpdateAt
            suppliers.UpdateAt = DateTime.Now;

            //cap nhat lai DB
            suppliersDAO.Update(suppliers);
            //thong bao thanh cong
            TempData["message"] = new XMessage("success", "Phục hồi mẩu tin thành công");
            //tra ket qua ve Index
            return RedirectToAction("Trash");//action trong SupllierDAO
        }
    }
}