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
    public class PageController : Controller
    {
        PostsDAO postsDAO = new PostsDAO();
        LinksDAO linksDAO = new LinksDAO();
        //doi voi page thi khong co chu de:
        //TopicsDAO topicsDAO = new TopicsDAO();

        /////////////////////////////////////////////////////////////////////////////////////
        // Admin/Post/Index: Tra ve danh sach cac mau tin
        public ActionResult Index()
        {
            return View(postsDAO.getList("Index", "Page"));//hien thi toan bo danh sach loai SP
        }

        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Page/Create: Them moi mot mau tin
        public ActionResult Create()
        {
            //doi voi page thi khong co chu de:
           //ViewBag.TopList = new SelectList(topicsDAO.getList("Index"), "Id", "Name");
            return View();
        }

        // POST: Admin/Post/Create: Them moi mot mau tin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Posts posts)
        {
            if (ModelState.IsValid)
            {
                //Xu ly cho muc Slug
                posts.Slug = XString.Str_Slug(posts.Title);
                //chuyen doi dua vao truong Name de loai bo dau, khoang cach = dau -

                //xu ly cho phan upload hình ảnh
                var img = Request.Files["img"];//lay thong tin file
                if (img.ContentLength != 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
                    //kiem tra tap tin co hay khong
                    if (FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))//lay phan mo rong cua tap tin
                    {
                        string slug = posts.Slug;
                        string id = posts.Id.ToString();
                        //Chinh sua sau khi phat hien dieu chua dung cua Edit: them Id
                        //ten file = Slug + Id + phan mo rong cua tap tin
                        string imgName = slug + id + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        posts.Image = imgName;

                        string PathDir = "~/Public/img/page/";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        //upload hinh
                        img.SaveAs(PathFile);
                    }
                }//ket thuc phan upload hinh anh

                //xu ly cho muc PostType = page (doi voi Page)
                posts.PostType = "page";

                //Xu ly cho muc CreateAt
                posts.CreateAt = DateTime.Now;

                //Xu ly cho muc CreateBy
                posts.CreateBy = Convert.ToInt32(Session["UserId"]);

                //xu ly cho muc Topics
                if (postsDAO.Insert(posts) == 1)//khi them du lieu thanh cong
                {
                    Links links = new Links();
                    links.Slug = posts.Slug;
                    links.TableId = posts.Id;
                    //cap nhat link cho page
                    links.Type = "page";
                    linksDAO.Insert(links);
                }
                //Thong bao thanh cong
                TempData["message"] = new XMessage("success", "Thêm trang đơn thành công");
                return RedirectToAction("Index");
            }
            //doi voi page thi khong co chu de:
            //ViewBag.TopList = new SelectList(topicsDAO.getList("Index"), "Id", "Name");
            return View(posts);
        }

        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Page/Staus/5:Thay doi trang thai cua mau tin
        public ActionResult Status(int? id)
        {
            if (id == null)
            {
                //Thong bao that bai
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                //chuyen huong trang
                return RedirectToAction("Index", "Page");
            }

            //khi nhap nut thay doi Status cho mot mau tin
            Posts posts = postsDAO.getRow(id);
            //kiem tra id cua posts co ton tai?
            if (posts == null)
            {
                //Thong bao that bai
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");

                //chuyen huong trang
                return RedirectToAction("Index", "Page");
            }
            //thay doi trang thai Status tu 1 thanh 2 va nguoc lai
            posts.Status = (posts.Status == 1) ? 2 : 1;

            //cap nhat gia tri cho UpdateAt/By
            posts.UpdateBy = Convert.ToInt32(Session["UserId"].ToString());
            posts.UpdateAt = DateTime.Now;

            //Goi ham Update trong PostDAO
            postsDAO.Update(posts);

            //Thong bao thanh cong
            TempData["message"] = new XMessage("success", "Cập nhật trạng thái thành công");

            //khi cap nhat xong thi chuyen ve Index
            return RedirectToAction("Index", "Page");
        }

        /////////////////////////////////////////////////////////////////////////////////////
        // Admin/Post/Detail: Hien thi mot mau tin
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Posts posts = postsDAO.getRow(id);
            if (posts == null)
            {
                return HttpNotFound();
            }
            return View(posts);
        }

        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Page/Edit/5: Cap nhat mau tin
        public ActionResult Edit(int? id)
        {
            //doi voi page thi khong co chu de:
            //ViewBag.TopList = new SelectList(topicsDAO.getList("Index"), "Id", "Name");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Posts posts = postsDAO.getRow(id);

            if (posts == null)
            {
                return HttpNotFound();
            }

            return View(posts);
        }

        // POST: Admin/Page/Edit/5: Cap nhat mau tin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Posts posts)
        {
            if (ModelState.IsValid)
            {
                //Xu ly cho muc Slug
                posts.Slug = XString.Str_Slug(posts.Title);
                //chuyen doi dua vao truong Name de loai bo dau, khoang cach = dau -

                //xu ly cho phan upload hình ảnh
                var img = Request.Files["img"];//lay thong tin file
                if (img.ContentLength != 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
                    //kiem tra tap tin co hay khong
                    if (FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))//lay phan mo rong cua tap tin
                    {
                        string slug = posts.Slug;
                        string id = posts.Id.ToString();
                        //Chinh sua sau khi phat hien dieu chua dung cua Edit: them Id
                        //ten file = Slug + Id + phan mo rong cua tap tin
                        string imgName = slug + id + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        posts.Image = imgName;

                        string PathDir = "~/Public/img/page/";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        //upload hinh
                        img.SaveAs(PathFile);
                    }
                }//ket thuc phan upload hinh anh

                ////xu ly cho muc PostType = page (doi voi Page)
                //posts.PostType = "page";

                //Xu ly cho muc UpdateAt
                posts.UpdateAt = DateTime.Now;

                //Xu ly cho muc UpdateBy
                posts.UpdateBy = Convert.ToInt32(Session["UserId"]);

                //xu ly cho muc Links
                if (postsDAO.Update(posts) == 1)//khi sua du lieu thanh cong
                {
                    Links links = new Links();
                    links.Slug = posts.Slug;
                    links.TableId = posts.Id;
                    //thoi doi thong tin kieu Page
                    links.Type = "page";
                    linksDAO.Insert(links);
                }
                //Thong bao thanh cong
                TempData["message"] = new XMessage("success", "Sửa trang đơn thành công");
                return RedirectToAction("Index");
            }
            //doi voi page thi khong co chu de:
            //ViewBag.TopList = new SelectList(topicsDAO.getList("Index"), "Id", "Name");
            return View(posts);
        }

        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Page/DelTrash/5:Thay doi trang thai cua mau tin = 0
        public ActionResult DelTrash(int? id)
        {
            //khi nhap nut thay doi Status cho mot mau tin
            Posts posts = postsDAO.getRow(id);

            //thay doi trang thai Status tu 1,2 thanh 0
            posts.Status = 0;

            //cap nhat gia tri cho UpdateAt/By
            posts.UpdateBy = Convert.ToInt32(Session["UserId"].ToString());
            posts.UpdateAt = DateTime.Now;

            //Goi ham Update trong PostDAO
            postsDAO.Update(posts);

            //Thong bao thanh cong
            TempData["message"] = new XMessage("success", "Xóa trang đơn thành công");

            //khi cap nhat xong thi chuyen ve Index
            return RedirectToAction("Index", "Page");
        }

        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Posts/Trash/5:Hien thi cac mau tin có gia tri la 0
        public ActionResult Trash(int? id)
        {
            return View(postsDAO.getList("Trash","page"));
        }

        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Page/Recover/5:Chuyen trang thai Status = 0 thanh =2
        public ActionResult Recover(int? id)
        {
            if (id == null)
            {
                //Thong bao that bai
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                //chuyen huong trang
                return RedirectToAction("Index", "Page");
            }

            //khi nhap nut thay doi Status cho mot mau tin
            Posts posts = postsDAO.getRow(id);
            //kiem tra id cua categories co ton tai?
            if (posts == null)
            {
                //Thong bao that bai
                TempData["message"] = new XMessage("danger", "Phục hồi dữ liệu thất bại");

                //chuyen huong trang
                return RedirectToAction("Index", "Page");
            }
            //thay doi trang thai Status tu 1 thanh 2 va nguoc lai
            posts.Status = 2;

            //cap nhat gia tri cho UpdateAt/By
            posts.UpdateBy = Convert.ToInt32(Session["UserId"].ToString());
            posts.UpdateAt = DateTime.Now;

            //Goi ham Update trong postsDAO
            postsDAO.Update(posts);

            //Thong bao thanh cong
            TempData["message"] = new XMessage("success", "Phục hồi dữ liệu thành công");

            //khi cap nhat xong thi chuyen ve Trash
            return RedirectToAction("Trash", "Page");
        }

        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Page/Delete/5:Xoa mot mau tin ra khoi CSDL
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Posts posts = postsDAO.getRow(id);
            if (posts == null)
            {
                return HttpNotFound();
            }
            return View(posts);
        }

        // POST: Admin/Page/Delete/5:Xoa mot mau tin ra khoi CSDL
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Posts posts = postsDAO.getRow(id);

            //tim thay mau tin thi xoa, cap nhat cho Links
            if (postsDAO.Delete(posts) == 1)
            {
                Links links = linksDAO.getRow(posts.Id, "page");
                //Xoa luon cho Links
                linksDAO.Delete(links);

                //duong dan den anh can xoa
                string PathDir = "~/Public/img/page/";
                //cap nhat thi phai xoa file cu
                if (posts.Image != null)
                {
                    string DelPath = Path.Combine(Server.MapPath(PathDir), posts.Image);
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
