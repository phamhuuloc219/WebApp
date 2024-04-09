using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyClass.Model;

namespace MyClass.DAO
{
    public class PostsDAO
    {
        private MyDBContext db = new MyDBContext();
        /////////////////////////////////////////////////////////////////////////////////////
        //Hien thi danh sach toan bo Loai san pham: SELCT * FROM
        public List<Posts> getList(string status = "All", string type = "Post")
        {
            List<Posts> list = null;
            switch (status)
            {
                case "Index":
                    {
                        list = db.Posts
                        .Where(m => m.Status != 0 && m.PostType == type)
                        .ToList();
                        break;
                    }
                case "Trash":
                    {
                        list = db.Posts
                        .Where(m => m.Status == 0 && m.PostType == type)
                        .ToList();
                        break;
                    }
                default:
                    {
                        list = db.Posts
                            .Where(m => m.PostType == type)
                            .ToList();
                        break;
                    }
            }
            return list;
        }

        /////////////////////////////////////////////////////////////////////////////////////
        //Hien thi danh sach 1 mau tin (ban ghi)
        public Posts getRow(int? id)
        {
            if (id == null)
            {
                return null;
            }
            else
            {
                return db.Posts.Find(id);
            }
        }

        /////////////////////////////////////////////////////////////////////////////////////
        //Hien thi danh sach 1 mau tin (ban ghi)
        public Posts getRow(string slug)
        {
            return db.Posts
                .Where(m => m.Slug == slug && m.Status == 1)
                .FirstOrDefault();
        }

        /////////////////////////////////////////////////////////////////////////////////////
        ///Them moi mot mau tin
        public int Insert(Posts row)
        {
            db.Posts.Add(row);
            return db.SaveChanges();
        }

        /////////////////////////////////////////////////////////////////////////////////////
        ///Cap nhat mot mau tin
        public int Update(Posts row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }

        /////////////////////////////////////////////////////////////////////////////////////
        ///Xoa mot mau tin Xoa ra khoi CSDL
        public int Delete(Posts row)
        {
            db.Posts.Remove(row);
            return db.SaveChanges();
        }
    }
}