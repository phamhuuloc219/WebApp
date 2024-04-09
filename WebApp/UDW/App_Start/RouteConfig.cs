using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace UDW
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //Khai bao cho URL co dinh: tat-ca-san-pham
            routes.MapRoute(
               name: "Tatcasanpham",
               url: "tat-ca-san-pham",
               defaults: new { controller = "Site", action = "Product", id = UrlParameter.Optional }
           );

            //Khai bao cho URL co dinh: tat-ca-bai-viet
            routes.MapRoute(
               name: "Tatcabaiviet",
               url: "tat-ca-bai-viet",
               defaults: new { controller = "Site", action = "Post", id = UrlParameter.Optional }
           );

            //Khai bao cho URL co dinh: lien-he
            routes.MapRoute(
               name: "Lienhe",
               url: "lien-he",
               defaults: new { controller = "Lienhe", action = "Index", id = UrlParameter.Optional }
           );

            //Khai bao cho URL co dinh: gio-hang
            routes.MapRoute(
               name: "Giohang",
               url: "gio-hang",
               defaults: new { controller = "Giohang", action = "Index", id = UrlParameter.Optional }
           );

            //Khai bao cho URL co dinh: thanh-toan
            routes.MapRoute(
               name: "Thanhtoan",
               url: "thanh-toan",
               defaults: new { controller = "Giohang", action = "ThanhToan", id = UrlParameter.Optional }
           );

            //Khai bao cho URL co dinh: dăng-nhap
            routes.MapRoute(
               name: "DangNhap",
               url: "dang-nhap",
               defaults: new { controller = "Khachhang", action = "DangNhap", id = UrlParameter.Optional }
           );

            //Khai bao cho URL co dinh: tim-kiem
            routes.MapRoute(
               name: "Timkiem",
               url: "tim-kiem",
               defaults: new { controller = "Timkiem", action = "Index", id = UrlParameter.Optional }
           );

            //khai bao cho URL dong
            routes.MapRoute(
              name: "Siteslug",
              url: "{slug}",
              defaults: new { controller = "Site", action = "Index", id = UrlParameter.Optional }
          );

            //gia tri mac dinh
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Site", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
