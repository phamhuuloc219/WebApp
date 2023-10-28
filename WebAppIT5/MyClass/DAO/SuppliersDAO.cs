using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyClass.Model;

namespace MyClass.DAO
{
    public class SuppliersDAO
    {
        private MyDBContext db = new MyDBContext();

        ///////////////////////////////////////////////////////////////
        ///INDEX
        public List<Suppliers> getList()
        {
            return db.Suppliers.ToList();
        }

        /////////////////////////////////////////////////////////////
        ///INDEX voi gia tri Status 1,2 - 0: An khoi trang giao dien
        public List<Suppliers> getList(string status = "All")
        {//tra ve danh sach cac NCC co staus =1 hoac 2 0 hoac tat ca
            List<Suppliers> list = null;
            switch (status)
            {
                case "Index": //status == 1,2
                    {
                        list = db.Suppliers.Where(m => m.Status != 0).ToList();
                        break;
                    }
                case "Trash": //status == 0
                    {
                        list = db.Suppliers.Where(m => m.Status == 0).ToList();
                        break;
                    }
                default:
                    {
                        list = db.Suppliers.ToList();
                        break;
                    }
            }
            return list;
        }

        /////////////////////////////////////////////////////////////
        ///DETAILS hien thi 1 dong du lieu
        public Suppliers getRow(int? id)
        {
            if (id == null)
            {
                return null;
            }
            else
            {
                return db.Suppliers.Find(id);
            }
        }

        /////////////////////////////////////////////////////////////
        ///CREATE = Insert 1 dong DB
        public int Insert(Suppliers row)
        {
            db.Suppliers.Add(row);
            return db.SaveChanges();
        }

        /////////////////////////////////////////////////////////////
        ///EDIT = Update 1 dong DB
        public int Update(Suppliers row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }

        /////////////////////////////////////////////////////////////
        ///DELETE = Delete 1 dong DB
        public int Delete(Suppliers row)
        {
            db.Suppliers.Remove(row);
            return db.SaveChanges();
        }
    }
}
