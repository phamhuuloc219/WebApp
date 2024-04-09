using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyClass.Model;

namespace MyClass.DAO
{
    public class LinksDAO
    {
        private MyDBContext db = new MyDBContext();
        /////////////////////////////////////////////////////////////////////////////////////
        //Hien thi danh sach toan bo Loai san pham: SELCT * FROM
        public List<Links> getList(string status = "All")
        {
            List<Links> list = null;
            
            
            return list;
        }
        /////////////////////////////////////////////////////////////////////////////////////
        //Hien thi danh sach 1 mau tin (ban ghi)
        public Links getRow(int tableid, string typelink)
        {
            return db.Links
                .Where(m => m.TableId == tableid && m.Type == typelink)
                .FirstOrDefault();
        }

        /////////////////////////////////////////////////////////////////////////////////////
        //Hien thi danh sach 1 mau tin (ban ghi voi kieu du lieu la string slug cho URL)
        public Links getRow(string slug)
        {
            return db.Links
                .Where(m => m.Slug == slug)
                .FirstOrDefault();
        }
        /////////////////////////////////////////////////////////////////////////////////////
        ///Them moi mot mau tin
        public int Insert(Links row)
        {
            db.Links.Add(row);
            return db.SaveChanges();
        }

        /////////////////////////////////////////////////////////////////////////////////////
        ///Cap nhat mot mau tin
        public int Update(Links row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }

        /////////////////////////////////////////////////////////////////////////////////////
        ///Xoa mot mau tin Xoa ra khoi CSDL
        public int Delete(Links row)
        {
            db.Links.Remove(row);
            return db.SaveChanges();
        }
    }
}
