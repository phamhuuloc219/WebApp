using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyClass.Model;

namespace MyClass.DAO
{
    public class CategoriesDAO
    {
        private MyDBContext db = new MyDBContext();
        /// ////////////////////////////////////////////////////////////////////////////////////
        /// Index
        public List<Categories> getList()
        {
            return db.Categories.ToList();
        }

        /// ////////////////////////////////////////////////////////////////////////////////////
        /// index voi gia tri Status 1,2- 0: an khoi trang giao dien
        public List<Categories> getList(string status="ALL")
        {
            List<Categories> list = null;
            switch (status)
            {
                case "Index": // status =1,2
                    {
                        list = db.Categories.Where(m => m.Status != 0).ToList();
                        break;
                    }
                case "Trash": //status=0
                    {
                        list = db.Categories.Where(m => m.Status == 0).ToList();
                        break;
                    }
                default:
                    {
                        list = db.Categories.ToList();
                        break;
                    }
            }
            return list;
        }
        /// ////////////////////////////////////////////////////////////////////////////////////
        /// Details hien thi 1 dong du lieu
        public Categories getRow(int? id)
        {
            if (id == null)
            {
                return null;
            }
            else
            {
                return db.Categories.Find(id);
            }
        }
        /// ////////////////////////////////////////////////////////////////////////////////////
        /// create = insert 1 dong database
        public int Insert(Categories row)
        {
            db.Categories.Add(row);
            return db.SaveChanges();
        }
        /// ////////////////////////////////////////////////////////////////////////////////////
        /// edit= update 1 dong DB
        public int Update(Categories row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }
        /// ////////////////////////////////////////////////////////////////////////////////////
        /// delete 1 dong DB
        public int Delete(Categories row)
        {
            db.Categories.Remove(row);
            return db.SaveChanges();
        }
    }
}
