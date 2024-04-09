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

namespace UDW.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        CategoriesDAO categoryDAO = new CategoriesDAO();
        LinksDAO linksDAO = new LinksDAO();

        /////////////////////////////////////////////////////////////////////////////////////
        // Admin/Category/Index: Tra ve danh sach cac mau tin
        public ActionResult Index()
        {
            return View(categoryDAO.getList("Index"));//hien thi toan bo danh sach loai SP
        }
        
        /////////////////////////////////////////////////////////////////////////////////////
        // Admin/Category/Detail: Hien thi mot mau tin
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categories categories = categoryDAO.getRow(id);
            if (categories == null)
            {
                return HttpNotFound();
            }
            return View(categories);
        }

        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/Create: Them moi mot mau tin
        public ActionResult Create()
        {
            ViewBag.ListCat =  new SelectList(categoryDAO.getList("Index"),"Id","Name");
            ViewBag.OrderList = new SelectList(categoryDAO.getList("Index"), "Order", "Name");
            return View();
        }

        // POST: Admin/Category/Create: Them moi mot mau tin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Categories categories)
        {
            if (ModelState.IsValid)
            {
                //Xu ly cho muc Slug
                categories.Slug = XString.Str_Slug(categories.Name);
                //chuyen doi dua vao truong Name de loai bo dau, khoang cach = dau -

                //Xu ly cho muc ParentId
                if (categories.ParentID == null)
                {
                    categories.ParentID = 0;
                }

                //Xu ly cho muc Order
                if (categories.Order == null)
                {
                    categories.Order = 1;
                }
                else
                {
                    categories.Order = categories.Order + 1;
                }

                //Xu ly cho muc CreateAt
                categories.CreateAt = DateTime.Now;

                //Xu ly cho muc CreateBy
                categories.CreateBy = Convert.ToInt32(Session["UserId"]);

                //xu ly cho muc Topics
                if (categoryDAO.Insert(categories) == 1)//khi them du lieu thanh cong
                {
                    Links links = new Links();
                    links.Slug = categories.Slug;
                    links.TableId = categories.Id;
                    links.Type = "category";
                    linksDAO.Insert(links);
                }
                

                //Thong bao thanh cong
                TempData["message"] = new XMessage("success", "Thêm danh mục thành công");
                return RedirectToAction("Index");
            }
            ViewBag.ListCat = new SelectList(categoryDAO.getList("Index"), "Id", "Name");
            ViewBag.OrderList = new SelectList(categoryDAO.getList("Index"), "Order", "Name");
            return View(categories);
        }

        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/Edit/5: Cap nhat mau tin
        public ActionResult Edit(int? id)
        {
            ViewBag.ListCat = new SelectList(categoryDAO.getList("Index"), "Id", "Name");
            ViewBag.OrderList = new SelectList(categoryDAO.getList("Index"), "Order", "Name");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Categories categories = categoryDAO.getRow(id);

            if (categories == null)
            {
                return HttpNotFound();
            }

            return View(categories);
        }

        // POST: Admin/Category/Edit/5: Cap nhat mau tin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Categories categories)
        {
            if (ModelState.IsValid)
            {
                //Xu ly cho muc Slug
                categories.Slug = XString.Str_Slug(categories.Name);
                //chuyen doi dua vao truong Name de loai bo dau, khoang cach = dau -

                //Xu ly cho muc ParentId
                if (categories.ParentID == null)
                {
                    categories.ParentID = 0;
                }

                //Xu ly cho muc Order
                if (categories.Order == null)
                {
                    categories.Order = 1;
                }
                else
                {
                    categories.Order = categories.Order + 1;
                }

                //Xy ly cho muc UpdateAt
                categories.UpdateAt = DateTime.Now;

                //Xy ly cho muc UpdateBy
                categories.UpdateBy = Convert.ToInt32(Session["UserId"]);

                //Thong bao thanh cong
                TempData["message"] = new XMessage("success", "Sửa danh mục thành công");

                //Cap nhat du lieu, sua them cho phan Links phuc vu cho Topics
                if (categoryDAO.Update(categories)==1) {
                    //Neu trung khop thong tin: Type = category va TableID = categories.ID
                    Links links = linksDAO.getRow(categories.Id, "category");
                    //cap nhat lai thong tin
                    links.Slug = categories.Slug;
                    linksDAO.Update(links);
                } 

                return RedirectToAction("Index");
            }
            return View(categories);
        }

        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/Delete/5:Xoa mot mau tin ra khoi CSDL
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categories categories = categoryDAO.getRow(id);
            if (categories == null)
            {
                return HttpNotFound();
            }
            return View(categories);
        }

        // POST: Admin/Category/Delete/5:Xoa mot mau tin ra khoi CSDL
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Categories categories = categoryDAO.getRow(id);

            //tim thay mau tin thi xoa, cap nhat cho Links
            if (categoryDAO.Delete(categories) ==1)
            {
                Links links = linksDAO.getRow(categories.Id, "category");
                //Xoa luon cho Links
                linksDAO.Delete(links);
            }

            //Thong bao thanh cong
            TempData["message"] = new XMessage("success", "Xóa danh mục thành công");
            //O lai trang thung rac
            return RedirectToAction("Trash");
        }

        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/Staus/5:Thay doi trang thai cua mau tin
        public ActionResult Status(int? id)
        {
            if (id==null)
            {
                //Thong bao that bai
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                //chuyen huong trang
                return RedirectToAction("Index","Category");
            }
            
            //khi nhap nut thay doi Status cho mot mau tin
            Categories categories = categoryDAO.getRow(id);
            //kiem tra id cua categories co ton tai?
            if (categories == null)
            {
                //Thong bao that bai
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");

                //chuyen huong trang
                return RedirectToAction("Index", "Category");
            }
            //thay doi trang thai Status tu 1 thanh 2 va nguoc lai
            categories.Status = (categories.Status == 1) ? 2 : 1;

            //cap nhat gia tri cho UpdateAt/By
            categories.UpdateBy = Convert.ToInt32(Session["UserId"].ToString());
            categories.UpdateAt = DateTime.Now;

            //Goi ham Update trong CategoryDAO
            categoryDAO.Update(categories);

            //Thong bao thanh cong
            TempData["message"] = new XMessage("success", "Cập nhật trạng thái thành công");

            //khi cap nhat xong thi chuyen ve Index
            return RedirectToAction("Index", "Category");
        }

        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/DelTrash/5:Thay doi trang thai cua mau tin = 0
        public ActionResult DelTrash(int? id)
        {
            //khi nhap nut thay doi Status cho mot mau tin
            Categories categories = categoryDAO.getRow(id);
            
            //thay doi trang thai Status tu 1,2 thanh 0
            categories.Status = 0;

            //cap nhat gia tri cho UpdateAt/By
            categories.UpdateBy = Convert.ToInt32(Session["UserId"].ToString());
            categories.UpdateAt = DateTime.Now;

            //Goi ham Update trong CategoryDAO
            categoryDAO.Update(categories);

            //Thong bao thanh cong
            TempData["message"] = new XMessage("success", "Xóa mẩu tin thành công");

            //khi cap nhat xong thi chuyen ve Index
            return RedirectToAction("Index", "Category");
        }

        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/Trash/5:Hien thi cac mau tin có gia tri la 0
        public ActionResult Trash(int? id)
        {
            return View(categoryDAO.getList("Trash"));
        }

        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/Recover/5:Chuyen trang thai Status = 0 thanh =2
        public ActionResult Recover(int? id)
        {
            if (id == null)
            {
                //Thong bao that bai
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                //chuyen huong trang
                return RedirectToAction("Index", "Category");
            }

            //khi nhap nut thay doi Status cho mot mau tin
            Categories categories = categoryDAO.getRow(id);
            //kiem tra id cua categories co ton tai?
            if (categories == null)
            {
                //Thong bao that bai
                TempData["message"] = new XMessage("danger", "Phục hồi dữ liệu thất bại");

                //chuyen huong trang
                return RedirectToAction("Index", "Category");
            }
            //thay doi trang thai Status tu 1 thanh 2 va nguoc lai
            categories.Status = 2;

            //cap nhat gia tri cho UpdateAt/By
            categories.UpdateBy = Convert.ToInt32(Session["UserId"].ToString());
            categories.UpdateAt = DateTime.Now;

            //Goi ham Update trong CategoryDAO
            categoryDAO.Update(categories);

            //Thong bao thanh cong
            TempData["message"] = new XMessage("success", "Phục hồi dữ liệu thành công");

            //khi cap nhat xong thi chuyen ve Trash
            return RedirectToAction("Trash", "Category");
        }
    }
}
