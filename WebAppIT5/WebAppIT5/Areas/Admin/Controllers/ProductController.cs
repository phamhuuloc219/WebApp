using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyClass.DAO;
using MyClass.Model;
using WebAppIT5.Library;

namespace WebAppIT5.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        ProductsDAO productsDAO = new ProductsDAO();
        CategoriesDAO categoriesDAO = new CategoriesDAO();
        SuppliersDAO suppliersDAO = new SuppliersDAO();
        ////////////////////////////////////////////////////////////
        // GET: Admin/Product/Index
        public ActionResult Index()
        {
            return View(productsDAO.getList("Index"));
        }
        ////////////////////////////////////////////////////////////
        // GET: Admin/Product/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                //thong bao that bai
                TempData["message"] = new XMessage("danger", "Không tồn tại sản phẩm");
                return RedirectToAction("Index");
            }
            Products products = productsDAO.getRow(id);
            if (products == null)
            {
                //thong bao that bai
                TempData["message"] = new XMessage("danger", "Không tồn tại sản phẩm");
                return RedirectToAction("Index");
            }
            return View(products);
        }
        ////////////////////////////////////////////////////////////
        // GET: Admin/Product/Create
        public ActionResult Create()
        {
            ViewBag.ListCatID = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");//sai CatId - truy van tu bang Categories
            ViewBag.ListSupID = new SelectList(suppliersDAO.getList("Index"), "Id", "Name");//sai SupplierID - truy van bang Suppliers
            //dung de lua chon tu danh sach droplist nhu bang Categories: ParentID va Supplier: ParentID
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Products products)
        {
            if (ModelState.IsValid)
            {
                //xu ly thong tin tu dong cho mot so truong
                //Xu ly tu dong: CreateAt
                products.CreateAt = DateTime.Now;
                //Xu ly tu dong: UpdateAt
                products.UpdateAt = DateTime.Now;
                //Xu ly tu dong: CreateBy
                products.CreateBy = Convert.ToInt32(Session["UserId"]);
                //Xu ly tu dong: UpdateBy
                products.UpdateBy = Convert.ToInt32(Session["UserId"]);
                //Xu ly tu dong: Slug
                products.Slug = XString.Str_Slug(products.Name);

                //xu ly cho phan upload hinh anh
                var img = Request.Files["img"];//lay thong tin file
                if (img.ContentLength != 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
                    //kiem tra tap tin co hay khong
                    if (FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))//lay phan mo rong cua tap tin
                    {
                        string slug = products.Slug;
                        //ten file = Slug + phan mo rong cua tap tin
                        string imgName = slug + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        products.Image = imgName;
                        //upload hinh
                        string PathDir = "~/Public/img/product";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        img.SaveAs(PathFile);
                    }
                }//ket thuc phan upload hinh anh

                //luu vao DB
                productsDAO.Insert(products);
                //thong bao tao mau tin thanh cong
                TempData["message"] = new XMessage("success", "Tạo mới sản phẩm thành công");
                return RedirectToAction("Index");
            }
            ViewBag.ListCatID = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");//sai CatId - truy van tu bang Categories
            ViewBag.ListSupID = new SelectList(suppliersDAO.getList("Index"), "Id", "Name");//sai SupplierID - truy van bang Suppliers
            return View(products);
        }
        ////////////////////////////////////////////////////////////
        // GET: Admin/Product/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                //thong bao that bai
                TempData["message"] = new XMessage("danger", "Không tồn tại sản phẩm");
                return RedirectToAction("Index");
            }
            Products products = productsDAO.getRow(id);
            if (products == null)
            {
                //thong bao that bai
                TempData["message"] = new XMessage("danger", "Không tồn tại sản phẩm");
                return RedirectToAction("Index");
            }
            ViewBag.ListCatID = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");//sai CatId - truy van tu bang Categories
            ViewBag.ListSupID = new SelectList(suppliersDAO.getList("Index"), "Id", "Name");//sai SupplierID - truy van bang Suppliers
            return View(products);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Products products)
        {
            if (ModelState.IsValid)
            {
                //xu ly tu dong cho cac truong: Slug, CreateAt/By, UpdateAt/By, Oder
                //Xu ly tu dong: UpdateAt
                products.UpdateAt = DateTime.Now;
                //Xu ly tu dong: Slug
                products.Slug = XString.Str_Slug(products.Name);

                //xu ly cho phan upload hinh anh
                var img = Request.Files["img"];//lay thong tin file
                string PathDir = "~/Public/img/product";
                if (img.ContentLength != 0)
                {
                    //Xu ly cho muc xoa hinh anh
                    if (products.Image != null)
                    {
                        string DelPath = Path.Combine(Server.MapPath(PathDir), products.Image);
                        System.IO.File.Delete(DelPath);
                    }

                    string[] FileExtentions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
                    //kiem tra tap tin co hay khong
                    if (FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))//lay phan mo rong cua tap tin
                    {
                        string slug = products.Slug;
                        //ten file = Slug + phan mo rong cua tap tin
                        string imgName = slug + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        products.Image = imgName;
                        //upload hinh
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        img.SaveAs(PathFile);
                    }
                }//ket thuc phan upload hinh anh

                //cap nhat mau tin vao DB
                productsDAO.Update(products);
                //thong bao tao mau tin thanh cong
                TempData["message"] = new XMessage("success", "Cập nhật sản phẩm thành công");
                return RedirectToAction("Index");
            }
            return View(products);
        }
        ////////////////////////////////////////////////////////////
        // GET: Admin/Product/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                //thong bao that bai
                TempData["message"] = new XMessage("danger", "Không tồn tại sản phẩm");
                return RedirectToAction("Index");
            }
            Products products = productsDAO.getRow(id);
            if (products == null)
            {
                //thong bao that bai
                TempData["message"] = new XMessage("danger", "Không tồn tại sản phẩm");
                return RedirectToAction("Index");
            }
            return View(products);
        }

        // POST: Admin/Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Products products = productsDAO.getRow(id);
            productsDAO.Delete(products);
            //thong bao xoa mau tin thanh cong
            TempData["message"] = new XMessage("success", "Xóa sản phẩm thành công");
            return RedirectToAction("Trash");
        }

        //phat sinh them 1 so action: Status, Trash, DelTrash, Undo: Copy tu Category Controller
        //////////////////////////////////////////////////////////////////////////////////////
        //STATUS
        // GET: Admin/Prducts/Status/5
        public ActionResult Status(int? id)
        {
            if (id == null)
            {
                //thong bao that bai
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");
            }
            //truy van dong co id = id yeu cau
            Products products = productsDAO.getRow(id);
            if (products == null)
            {
                //thong bao that bai
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");
            }
            else
            {
                //chuyen doi trang thai cua Satus tu 1<->2
                products.Status = (products.Status == 1) ? 2 : 1;

                //cap nhat gia tri UpdateAt
                products.UpdateAt = DateTime.Now;

                //cap nhat lai DB
                productsDAO.Update(products);

                //thong bao cap nhat trang thai thanh cong
                TempData["message"] = TempData["message"] = new XMessage("success", "Cập nhật trạng thái thành công");

                return RedirectToAction("Index");
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////
        //DELTRAH
        // GET: Admin/Products/DelTrash/5
        public ActionResult DelTrash(int? id)
        {
            if (id == null)
            {
                //thong bao that bai
                TempData["message"] = new XMessage("danger", "Không tìm thấy mẩu tin");
                return RedirectToAction("Index");
            }
            //truy van dong co id = id yeu cau
            Products products = productsDAO.getRow(id);
            if (products == null)
            {
                //thong bao that bai
                TempData["message"] = new XMessage("danger", "Không tìm thấy mẩu tin");
                return RedirectToAction("Index");
            }
            else
            {
                //chuyen doi trang thai cua Satus tu 1,2 -> 0: Không hiển thị ở Index
                products.Status = 0;

                //cap nhat gia tri UpdateAt
                products.UpdateAt = DateTime.Now;

                //cap nhat lai DB
                productsDAO.Update(products);

                //thong bao cap nhat trang thai thanh cong
                TempData["message"] = TempData["message"] = new XMessage("success", "Xóa mẩu tin vào thùng rác thành công");

                return RedirectToAction("Index");
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////
        //TRASH: Luc thung rac
        // GET: Admin/Products/Trash
        public ActionResult Trash()
        {
            return View(productsDAO.getList("Trash"));
        }

        //////////////////////////////////////////////////////////////////////////////////////
        //RECOVER
        // GET: Admin/Products/Recover/5
        public ActionResult Recover(int? id)
        {
            if (id == null)
            {
                //thong bao that bai
                TempData["message"] = new XMessage("danger", "Phục hồi mẩu tin thất bại");
                return RedirectToAction("Index");
            }
            //truy van dong co id = id yeu cau
            Products products = productsDAO.getRow(id);
            if (products == null)
            {
                //thong bao that bai
                TempData["message"] = new XMessage("danger", "Phục hồi mẩu tin thất bại");
                return RedirectToAction("Index");
            }
            else
            {
                //chuyen doi trang thai cua Satus tu 0 -> 2: Khong xuat ban
                products.Status = 2;

                //cap nhat gia tri UpdateAt
                products.UpdateAt = DateTime.Now;

                //cap nhat lai DB
                productsDAO.Update(products);

                //thong bao phuc hoi du lieu thanh cong
                TempData["message"] = TempData["message"] = new XMessage("success", "Phục hồi mẩu tin thành công");

                return RedirectToAction("Trash");
            }
        }
    }
}
