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
    public class TopicController : Controller
    {
        TopicsDAO topicsDAO = new TopicsDAO();
        LinksDAO linksDAO = new LinksDAO();

        /////////////////////////////////////////////////////////////////////////////////////
        // Admin/Topic/Index: Tra ve danh sach cac mau tin
        public ActionResult Index()
        {
            return View(topicsDAO.getList("Index"));//hien thi toan bo danh sach loai SP
        }

        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Topic/Create: Them moi mot mau tin
        public ActionResult Create()
        {
            ViewBag.ListTopic = new SelectList(topicsDAO.getList("Index"), "Id", "Name");
            ViewBag.OrderTopic = new SelectList(topicsDAO.getList("Index"), "Order", "Name");
            return View();
        }

        // POST: Admin/Topic/Create: Them moi mot mau tin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Topics topics)
        {
            if (ModelState.IsValid)
            {
                //Xu ly cho muc Slug
                topics.Slug = XString.Str_Slug(topics.Name);
                //chuyen doi dua vao truong Name de loai bo dau, khoang cach = dau -

                //Xu ly cho muc ParentId
                if (topics.ParentId == null)
                {
                    topics.ParentId = 0;
                }

                //Xu ly cho muc Order
                if (topics.Order == null)
                {
                    topics.Order = 1;
                }
                else
                {
                    topics.Order = topics.Order + 1;
                }

                //Xu ly cho muc CreateAt
                topics.CreateAt = DateTime.Now;

                //Xu ly cho muc CreateBy
                topics.CreateBy = Convert.ToInt32(Session["UserId"]);

                //xu ly cho muc Topics
                if (topicsDAO.Insert(topics) == 1)//khi them du lieu thanh cong
                {
                    Links links = new Links();
                    links.Slug = topics.Slug;
                    links.TableId = topics.Id;
                    links.Type = "topic";
                    linksDAO.Insert(links);
                }
                //Thong bao thanh cong
                TempData["message"] = new XMessage("success", "Thêm chủ đề thành công");
                return RedirectToAction("Index");
            }
            ViewBag.ListTopic = new SelectList(topicsDAO.getList("Index"), "Id", "Name");
            ViewBag.OrderTopic = new SelectList(topicsDAO.getList("Index"), "Order", "Name");
            return View(topics);
        }

        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Topic/Staus/5:Thay doi trang thai cua mau tin
        public ActionResult Status(int? id)
        {
            if (id == null)
            {
                //Thong bao that bai
                TempData["message"] = new XMessage("danger", "Cập nhật chủ đề thất bại");
                //chuyen huong trang
                return RedirectToAction("Index", "Topic");
            }

            //khi nhap nut thay doi Status cho mot mau tin
            Topics topics = topicsDAO.getRow(id);

            //kiem tra id cua topics co ton tai?
            if (topics == null)
            {
                //Thong bao that bai
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");

                //chuyen huong trang
                return RedirectToAction("Index", "Topic");
            }
            //thay doi trang thai Status tu 1 thanh 2 va nguoc lai
            topics.Status = (topics.Status == 1) ? 2 : 1;

            //cap nhat gia tri cho UpdateAt/By
            topics.UpdateBy = Convert.ToInt32(Session["UserId"].ToString());
            topics.UpdateAt = DateTime.Now;

            //Goi ham Update trong TopicDAO
            topicsDAO.Update(topics);

            //Thong bao thanh cong
            TempData["message"] = new XMessage("success", "Cập nhật trạng thái thành công");

            //khi cap nhat xong thi chuyen ve Index
            return RedirectToAction("Index", "Topic");
        }

        /////////////////////////////////////////////////////////////////////////////////////
        // Admin/Topic/Detail: Hien thi mot mau tin
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topics topics = topicsDAO.getRow(id);
            if (topics == null)
            {
                return HttpNotFound();
            }
            return View(topics);
        }

        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Topic/Edit/5: Cap nhat mau tin
        public ActionResult Edit(int? id)
        {
            ViewBag.ListTopic = new SelectList(topicsDAO.getList("Index"), "Id", "Name");
            ViewBag.OrderTopic = new SelectList(topicsDAO.getList("Index"), "Order", "Name");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Topics topics = topicsDAO.getRow(id);

            if (topics == null)
            {
                return HttpNotFound();
            }

            return View(topics);
        }

        // POST: Admin/Topic/Edit/5: Cap nhat mau tin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Topics topics)
        {
            if (ModelState.IsValid)
            {
                //Xu ly cho muc Slug
                topics.Slug = XString.Str_Slug(topics.Name);
                //chuyen doi dua vao truong Name de loai bo dau, khoang cach = dau -

                //Xu ly cho muc ParentId
                if (topics.ParentId == null)
                {
                    topics.ParentId = 0;
                }

                //Xu ly cho muc Order
                if (topics.Order == null)
                {
                    topics.Order = 1;
                }
                else
                {
                    topics.Order = topics.Order + 1;
                }

                //Xy ly cho muc UpdateAt
                topics.UpdateAt = DateTime.Now;

                //Xy ly cho muc UpdateBy
                topics.UpdateBy = Convert.ToInt32(Session["UserId"]);

                //Thong bao thanh cong
                TempData["message"] = new XMessage("success", "Sửa danh mục thành công");

                //Cap nhat du lieu, sua them cho phan Links phuc vu cho Topics
                if (topicsDAO.Update(topics) == 1)
                {
                    //Neu trung khop thong tin: Type = category va TableID = categories.ID
                    Links links = linksDAO.getRow(topics.Id, "topic");
                    //cap nhat lai thong tin
                    links.Slug = topics.Slug;
                    linksDAO.Update(links);
                }

                return RedirectToAction("Index");
            }
            return View(topics);
        }

        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Topic/DelTrash/5:Thay doi trang thai cua mau tin = 0
        public ActionResult DelTrash(int? id)
        {
            //khi nhap nut thay doi Status cho mot mau tin
            Topics topics = topicsDAO.getRow(id);

            //thay doi trang thai Status tu 1,2 thanh 0
            topics.Status = 0;

            //cap nhat gia tri cho UpdateAt/By
            topics.UpdateBy = Convert.ToInt32(Session["UserId"].ToString());
            topics.UpdateAt = DateTime.Now;

            //Goi ham Update trong TopicDAO
            topicsDAO.Update(topics);

            //Thong bao thanh cong
            TempData["message"] = new XMessage("success", "Xóa mẩu tin thành công");

            //khi cap nhat xong thi chuyen ve Index
            return RedirectToAction("Index", "Topic");
        }

        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Topic/Trash/5:Hien thi cac mau tin có gia tri la 0
        public ActionResult Trash(int? id)
        {
            return View(topicsDAO.getList("Trash"));
        }

        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Topic/Recover/5:Chuyen trang thai Status = 0 thanh =2
        public ActionResult Recover(int? id)
        {
            if (id == null)
            {
                //Thong bao that bai
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                //chuyen huong trang
                return RedirectToAction("Index", "Topic");
            }

            //khi nhap nut thay doi Status cho mot mau tin
            Topics topics = topicsDAO.getRow(id);
            //kiem tra id cua categories co ton tai?
            if (topics == null)
            {
                //Thong bao that bai
                TempData["message"] = new XMessage("danger", "Phục hồi dữ liệu thất bại");

                //chuyen huong trang
                return RedirectToAction("Index", "Topic");
            }
            //thay doi trang thai Status tu 1 thanh 2 va nguoc lai
            topics.Status = 2;

            //cap nhat gia tri cho UpdateAt/By
            topics.UpdateBy = Convert.ToInt32(Session["UserId"].ToString());
            topics.UpdateAt = DateTime.Now;

            //Goi ham Update trong TopicDAO
            topicsDAO.Update(topics);

            //Thong bao thanh cong
            TempData["message"] = new XMessage("success", "Phục hồi dữ liệu thành công");

            //khi cap nhat xong thi chuyen ve Trash
            return RedirectToAction("Trash", "Topic");
        }

        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Topic/Delete/5:Xoa mot mau tin ra khoi CSDL
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topics topics = topicsDAO.getRow(id);
            if (topics == null)
            {
                return HttpNotFound();
            }
            return View(topics);
        }

        // POST: Admin/Category/Delete/5:Xoa mot mau tin ra khoi CSDL
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Topics topics = topicsDAO.getRow(id);

            //tim thay mau tin thi xoa, cap nhat cho Links
            if (topicsDAO.Delete(topics) == 1)
            {
                Links links = linksDAO.getRow(topics.Id, "topic");
                //Xoa luon cho Links
                linksDAO.Delete(links);
            }

            //Thong bao thanh cong
            TempData["message"] = new XMessage("success", "Xóa chủ đề thành công");
            //O lai trang thung rac
            return RedirectToAction("Trash");
        }
    }
}
