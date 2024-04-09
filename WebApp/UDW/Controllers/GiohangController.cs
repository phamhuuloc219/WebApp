using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyClass.DAO;
using MyClass.Model;
using UDW.Library;

namespace UDW.Controllers
{
    public class GiohangController : Controller
    {
        ProductsDAO productsDAO = new ProductsDAO();
        XCart xcart = new XCart();
        // GET: Cart
        public ActionResult Index()
        {
            //da co thong tin trong gio hang, lay thong tin cua session -> ep  kieu ve list 
            List<CartItem> list = xcart.GetCart();
            return View("Index", list);
        }

        //////////////////////////////////////////////////////////////////
        ///Them vao gio hang
        public ActionResult AddCart(int productid)
        {
            Products products = productsDAO.getRow(productid);
            CartItem cartitem = new CartItem(products.Id, products.Name, products.Image, products.Price, 1);
            //Them vao gio hang voi danh sách list phan tu = Session = MyCart
            XCart xcart = new XCart();
            List<CartItem> list = xcart.AddCart(cartitem, productid);
            //chuyen huong trang
            return RedirectToAction("Index", "Giohang");
        }

        //////////////////////////////////////////////////////////////////
        ///DelCart
        public ActionResult CartDel(int productid)
        {
            xcart.DelCart(productid);
            return RedirectToAction("Index","Giohang");
        }

        //////////////////////////////////////////////////////////////////
        ///CartUpdate
        public ActionResult CartUpdate(FormCollection form)
        {
            if (!string.IsNullOrEmpty(form["capnhat"]))//nut ThemCategory duoc nhan
            {
                var listamount = form["amount"];
                //chuyen danh sach thanh dang mang: vi du 1,2,3,...
                var listarr = listamount.Split(',');//cat theo dau ,
                xcart.UpdateCart(listarr);
            }
            return RedirectToAction("Index", "Giohang");
        }

        //////////////////////////////////////////////////////////////////
        ///CartUpdate
        public ActionResult CartDelAll()
        {
            xcart.DelCart();
            return RedirectToAction("Index", "Giohang");
        }

        //////////////////////////////////////////////////////////////////
        ///ThanhToan
        public ActionResult ThanhToan()
        {
            //Kiem tra thong tin dang nhap trang nguoi dung = Khach hang
            if (Session["UserCustomer"].Equals(""))
            {
                return Redirect("~/dang-nhap");//chuyen huong den URL
            }
            return View("ThanhToan");
        }
    }
}