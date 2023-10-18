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

namespace WebAppIT5.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        CategoriesDAO categoriesDAO = new CategoriesDAO();
        /// ////////////////////////////////////////////////////////////////////////////////////
        /// index
        // GET: Admin/Category
        public ActionResult Index()
        {
            return View(categoriesDAO.getList("Index"));// chi hien thi cac dong co status =1,2
        }
        /// ////////////////////////////////////////////////////////////////////////////////////
        /// details
        // GET: Admin/Category/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                return HttpNotFound();
            }
            return View(categories);
        }
        
        // GET: Admin/Category/Create
        public ActionResult Create()
        {
            ViewBag.ListCat = new SelectList(categoriesDAO.getList("Index"),"Id","Name");
            ViewBag.ListOrder = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");
            return View();
        }
        /// ////////////////////////////////////////////////////////////////////////////////////
        /// create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Categories categories)
        {
            if (ModelState.IsValid)
            {
                // xử lý tự động cho : createAt
                categories.CreateAt = DateTime.Now;
                // xử lý tự động cho : updateAt
                categories.UpdateAt = DateTime.Now;
                // xử lý tự động cho : parentId
                if (categories.ParentID==null)
                {
                    categories.ParentID = 0;
                }
                // xử lý tự động cho : order
                if (categories.Order==null)
                {
                    categories.Order = 1;
                }
                else
                {
                    categories.Order += 1;
                }
                // xử lý tự động cho : slug
                categories.Slug = XString.Str_Slug(categories.Name);
                // thêm dòng dữ liệu cho database
                categoriesDAO.Insert(categories);
                return RedirectToAction("Index");
            }
            ViewBag.ListCat = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");
            ViewBag.ListOrder = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");
            return View(categories);
        }
        
        // GET: Admin/Category/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // tim dong DB can chinh sua
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                return HttpNotFound();
            }
            return View(categories);
        }
        /// ////////////////////////////////////////////////////////////////////////////////////
        /// edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Categories categories)
        {
            if (ModelState.IsValid)
            {
                categoriesDAO.Update(categories);
                return RedirectToAction("Index");
            }
            return View(categories);
        }
        /// ////////////////////////////////////////////////////////////////////////////////////
        /// delete
        // GET: Admin/Category/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                return HttpNotFound();
            }
            return View(categories);
        }

        // POST: Admin/Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Categories categories = categoriesDAO.getRow(id);
            categoriesDAO.Delete(categories);
            return RedirectToAction("Index");
        }

        /// ////////////////////////////////////////////////////////////////////////////////////
        /// status
        // GET: Admin/Category/status/5 
        public ActionResult Status(int? id)
        {
            if (id==null)
            {
                // thong bao that bai
                TempData["message"] = ("Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");
            }
            // tim dong co ID = ID cua loai san pham can thay doi status
            Categories categories = categoriesDAO.getRow(id);
            // kiem tra trang thai cua status, neu hien tai la 1 -> 2 va nguoc lai (2->1)
            categories.Status = (categories.Status == 1) ? 2 : 1;
            // cap nhat gia tri cho updateAt
            categories.UpdateAt = DateTime.Now;
            // cap nhat lai database
            categoriesDAO.Update(categories);
            // tra ket qua ve index
            // thong bao thanh cong
            TempData["message"] = new XMessage ("success","Cập nhật trạng thái thành công");
            return RedirectToAction("Index");
        }
    }
}
