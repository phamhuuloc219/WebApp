using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyClass.DAO;
using MyClass.Model;
using UDW.Library;

namespace UDW.Areas.Admin.Controllers
{
    public class MenuController : Controller
    {
        //Goi 4 lop DAO can thuc thi
        CategoriesDAO categoriesDAO = new CategoriesDAO();
        TopicsDAO topicsDAO = new TopicsDAO();
        PostsDAO postsDAO = new PostsDAO();
        MenusDAO menusDAO = new MenusDAO();
        SuppliersDAO suppliersDAO = new SuppliersDAO();//neu thich thi lam

        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Menu
        public ActionResult Index()
        {
            ViewBag.CatList = categoriesDAO.getList("Index");//select * from Categories voi Status !=0
            ViewBag.TopList = topicsDAO.getList("Index");//select * from Topics voi Status !=0
            ViewBag.PosList = postsDAO.getList("Index","Page");//select * from Posts voi Status !=0
            List<Menus> menu = menusDAO.getList("Index");//select * from Menus voi Status !=0
            return View("Index", menu);//truyen menu duoi dang model
        }

        /////////////////////////////////////////////////////////////////////////////////////
        // POST: Admin/Menu/Create
        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            //-------------------------Category------------------------//
            //Xu ly cho nút ThemCategory ben Index
            if (!string.IsNullOrEmpty(form["ThemCategory"]))//nut ThemCategory duoc nhan
            {
                if (!string.IsNullOrEmpty(form["nameCategory"]))//check box được nhấn
                {
                    var listitem = form["nameCategory"];
                    //chuyen danh sach thanh dang mang: vi du 1,2,3,...
                    var listarr = listitem.Split(',');//cat theo dau ,
                    foreach (var row in listarr)//row = id cua các mau tin
                    {
                        int id = int.Parse(row);//ep kieu int
                        //lay 1 ban ghi
                        Categories categories = categoriesDAO.getRow(id);
                        //tao ra menu
                        Menus menu = new Menus();
                        menu.Name = categories.Name;
                        menu.Link = categories.Slug;
                        menu.TableId = categories.Id;
                        menu.TypeMenu = "category";
                        menu.Position = form["Position"];
                        menu.ParentId = 0;
                        menu.Order = 0;
                        menu.CreateBy = Convert.ToInt32(Session["UserId"].ToString());
                        menu.CreateAt = DateTime.Now;
                        menu.Status = 2;//chưa xuất bản
                        menusDAO.Insert(menu);
                    }
                    TempData["message"] = new XMessage("success", "Thêm menu danh mục thành công");
                }
                else
                {
                    TempData["message"] = new XMessage("danger", "Chưa chọn danh mục loại sản phẩm");
                }
            }

            //-------------------------Topic------------------------//
            //Xu ly cho nút ThemTopic ben Index
            if (!string.IsNullOrEmpty(form["ThemTopic"]))//nut ThemCategory duoc nhan
            {
                if (!string.IsNullOrEmpty(form["nameTopic"]))//check box được nhấn
                {
                    var listitem = form["nameTopic"];
                    //chuyen danh sach thanh dang mang: vi du 1,2,3,...
                    var listarr = listitem.Split(',');//cat theo dau ,
                    foreach (var row in listarr)//row = id cua các mau tin
                    {
                        int id = int.Parse(row);//ep kieu int
                                                //lay 1 ban ghi
                        Topics topics = topicsDAO.getRow(id);
                        //tao ra menu
                        Menus menu = new Menus();
                        menu.Name = topics.Name;
                        menu.Link = topics.Slug;
                        menu.TableId = topics.Id;
                        menu.TypeMenu = "topic";
                        menu.Position = form["Position"];
                        menu.ParentId = 0;
                        menu.Order = 0;
                        menu.CreateBy = Convert.ToInt32(Session["UserId"].ToString());
                        menu.CreateAt = DateTime.Now;
                        menu.Status = 2;//chưa xuất bản
                        menusDAO.Insert(menu);
                    }
                    TempData["message"] = new XMessage("success", "Thêm menu chủ đề bài viết thành công");
                }
                else
                {
                    TempData["message"] = new XMessage("danger", "Chưa chọn danh mục chủ đề bài viết");
                }
            }

            //-------------------------Page------------------------//
            //Xử lý cho nut Thempage ben Index
            if (!string.IsNullOrEmpty(form["ThemPage"]))
            {
                if (!string.IsNullOrEmpty(form["namePage"]))//check box được nhấn tu phia Index
                {
                    var listitem = form["namePage"];
                    //chuyen danh sach thanh dang mang: vi du 1,2,3,...
                    var listarr = listitem.Split(',');//cat theo dau ,
                    foreach (var row in listarr)//row = id cua các mau tin
                    {
                        int id = int.Parse(row);//ep kieu int
                        Posts post = postsDAO.getRow(id);
                        //tao ra menu
                        Menus menu = new Menus();
                        menu.Name = post.Title;
                        menu.Link = post.Slug;
                        menu.TableId = post.Id;
                        menu.TypeMenu = "page";
                        menu.Position = form["Position"];
                        menu.ParentId = 0;
                        menu.Order = 0;
                        menu.CreateBy = Convert.ToInt32(Session["UserId"].ToString());
                        menu.CreateAt = DateTime.Now;
                        menu.Status = 2;//chưa xuất bản
                        menusDAO.Insert(menu);
                    }
                    TempData["message"] = new XMessage("success", "Thêm menu bài viết thành công");
                }
                else//check box chưa được nhấn
                {
                    TempData["message"] = new XMessage("danger", "Chưa chọn danh mục trang đơn");
                }
            }

            //-------------------------Custom------------------------//
            //Xử lý cho nút ThemCustom ben Index
            if (!string.IsNullOrEmpty(form["ThemCustom"]))
            {
                if (!string.IsNullOrEmpty(form["name"]) && !string.IsNullOrEmpty(form["link"]))
                //o name, link text được gõ tu phia Index
                {
                    //tao ra menu
                    Menus menus = new Menus();
                    menus.Name = form["name"];//lay tu o nhap du lieu (form)
                    menus.Link = form["link"];//lay tu o nhap du lieu (form)
                    //menu.TableId = post.Id;//vi Table Id allow NULL nen bỏ đi
                    menus.TypeMenu = "custom";
                    menus.Position = form["Position"];
                    menus.ParentId = 0;
                    menus.Order = 0;
                    menus.CreateBy = Convert.ToInt32(Session["UserId"].ToString());
                    menus.CreateAt = DateTime.Now;
                    menus.Status = 2;//chưa xuất bản
                    menusDAO.Insert(menus);

                    TempData["message"] = new XMessage("success", "Thêm danh mục thành công");
                }

                else//check box chưa được nhấn
                {
                    TempData["message"] = new XMessage("danger", "Chưa đủ thông tin cho mục tùy chọn Menu");
                }
            }

           return RedirectToAction("Index", "Menu");
        }

        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Menu/Status
        public ActionResult Status(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Cập nhật không thành công");
                return RedirectToAction("Index");
            }
            Menus menus = menusDAO.getRow(id);
            if (menus == null)
            {
                TempData["message"] = new XMessage("danger", "Cập nhật không thành công");
                return RedirectToAction("Index");
            }
            menus.UpdateAt = DateTime.Now;
            menus.UpdateBy = Convert.ToInt32(Session["UserId"].ToString());
            menus.Status = (menus.Status == 1) ? 2 : 1;
            menusDAO.Update(menus);
            TempData["message"] = new XMessage("success", "Cập nhật thành công");
            return RedirectToAction("Index");
        }

        /////////////////////////////////////////////////////////////////////////////////////
        // Admin/Menus/Detail: Hien thi mot mau tin
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menus menus = menusDAO.getRow(id);
            if (menus == null)
            {
                return HttpNotFound();
            }
            return View(menus);
        }

        /////////////////////////////////////////////////////////////////////////////////////
        // Admin/Menu/Edit: Thay doi mot mau tin
        public ActionResult Edit(int? id)
        {

            ViewBag.ParentList = new SelectList(menusDAO.getList("Index"), "Id", "Name");
            ViewBag.OrderList = new SelectList(menusDAO.getList("Index"), "Order", "Name");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Menus menus = menusDAO.getRow(id);

            if (menus == null)
            {
                return HttpNotFound();
            }
            return View("Edit",menus);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Menus menus)
        {
            if (ModelState.IsValid)
            {

                if (menus.ParentId == null)
                {
                    menus.ParentId = 0;
                }
                if (menus.Order == null)
                {
                    menus.Order = 1;
                }
                else
                {
                    menus.Order += 1;
                }

                //Xy ly cho muc UpdateAt
                menus.UpdateAt = DateTime.Now;

                //Xy ly cho muc UpdateBy
                menus.UpdateBy = Convert.ToInt32(Session["UserId"]);

                //Thong bao thanh cong
                TempData["message"] = new XMessage("success", "Cập nhật thành công");

                //Cap nhat du lieu
                menusDAO.Update(menus);

                return RedirectToAction("Index");
            }

            ViewBag.ParentList = new SelectList(menusDAO.getList("Index"), "Id", "Name");
            ViewBag.OrderList = new SelectList(menusDAO.getList("Index"), "Order", "Name");
            return View(menus);
        }

        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Menu/DelTrash/5:Thay doi trang thai cua mau tin = 0
        public ActionResult DelTrash(int? id)
        {
            //khi nhap nut thay doi Status cho mot mau tin
            Menus menus = menusDAO.getRow(id);

            //thay doi trang thai Status tu 1,2 thanh 0
            menus.Status = 0;

            //cap nhat gia tri cho UpdateAt/By
            menus.UpdateBy = Convert.ToInt32(Session["UserId"].ToString());
            menus.UpdateAt = DateTime.Now;

            //Goi ham Update trong MenusDAO
            menusDAO.Update(menus);

            //Thong bao thanh cong
            TempData["message"] = new XMessage("success", "Xóa Menu thành công");

            //khi cap nhat xong thi chuyen ve Index
            return RedirectToAction("Index", "Menu");
        }
        
        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Menus/Trash/5:Hien thi cac mau tin có gia tri la 0
        public ActionResult Trash(int? id)
        {
            return View(menusDAO.getList("Trash"));
        }

        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Menu/Recover/5:Thay doi trang thai cua mau tin
        public ActionResult Recover(int? id)
        {
            if (id == null)
            {
                //Thong bao that bai
                TempData["message"] = new XMessage("danger", "Phục hồi menu thất bại");
                //chuyen huong trang
                return RedirectToAction("Index", "Page");
            }

            //khi nhap nut thay doi Status cho mot mau tin
            Menus menus = menusDAO.getRow(id);
            //kiem tra id cua menus co ton tai?
            if (menus == null)
            {
                //Thong bao that bai
                TempData["message"] = new XMessage("danger", "Phục hồi menu thất bại");

                //chuyen huong trang
                return RedirectToAction("Index");
            }
            //thay doi trang thai Status = 2
            menus.Status = 2;

            //cap nhat gia tri cho UpdateAt/By
            menus.UpdateBy = Convert.ToInt32(Session["UserId"].ToString());
            menus.UpdateAt = DateTime.Now;

            //Goi ham Update trong menusDAO
            menusDAO.Update(menus);

            //Thong bao thanh cong
            TempData["message"] = new XMessage("success", "Phục hồi menu thành công");

            //khi cap nhat xong thi chuyen ve Trash de phuc hoi tiep
            return RedirectToAction("Trash");
        }

        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Menu/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menus menus = menusDAO.getRow(id);
            if (menus == null)
            {
                return HttpNotFound();
            }
            return View(menus);
        }

        // POST: Admin/Menu/Delete/5:Xoa mot mau tin ra khoi CSDL
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Menus menus = menusDAO.getRow(id);

            //tim thay mau tin thi xoa, cap nhat cho Links
            menusDAO.Delete(menus);
        
            //Thong bao thanh cong
            TempData["message"] = new XMessage("success", "Xóa menu thành công");
            //O lai trang thung rac
            return RedirectToAction("Trash");
        }


    }
}