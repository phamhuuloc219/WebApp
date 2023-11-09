using MyClass.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.DAO
{
    public class ProductsDAO
    {
        private MyDBContext db = new MyDBContext();

        ///////////////////////////////////////////////////////////////
        ///INDEX
        public List<Products> getList()
        {
            return db.Products.ToList();
        }

        /////////////////////////////////////////////////////////////
        ///INDEX voi gia tri Status 1,2 - 0: An khoi trang giao dien
        public List<Products> getList(string status = "All")
        {//tra ve danh sach cac NCC co staus =1 hoac 2 0 hoac tat ca
            List<Products> list = null;
            switch (status)
            {
                case "Index": //status == 1,2
                    {
                        list = db.Products.Where(m => m.Status != 0).ToList();
                        break;
                    }
                case "Trash": //status == 0
                    {
                        list = db.Products.Where(m => m.Status == 0).ToList();
                        break;
                    }
                default:
                    {
                        list = db.Products.ToList();
                        break;
                    }
            }
            return list;
        }

        /////////////////////////////////////////////////////////////
        ///DETAILS hien thi 1 dong du lieu
        public Products getRow(int? id)
        {
            if (id == null)
            {
                return null;
            }
            else
            {
                return db.Products.Find(id);
            }
        }

        /////////////////////////////////////////////////////////////
        ///CREATE = Insert 1 dong DB
        public int Insert(Products row)
        {
            db.Products.Add(row);
            return db.SaveChanges();
        }

        /////////////////////////////////////////////////////////////
        ///EDIT = Update 1 dong DB
        public int Update(Products row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }

        /////////////////////////////////////////////////////////////
        ///DELETE = Delete 1 dong DB
        public int Delete(Products row)
        {
            db.Products.Remove(row);
            return db.SaveChanges();
        }
    }
}