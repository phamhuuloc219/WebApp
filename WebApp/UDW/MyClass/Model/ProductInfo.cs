using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.Model
{
    public class ProductInfo
    {
        public int Id { get; set; }

        public int CatID { get; set; }

        public string Name { get; set; }

        public string CatName { get; set; }

        //bo sung them truong Slug cua Categories: detail product
        public string CategorySlug { get; set; }

        public int SupplierId { get; set; }

        public string SupplierName { get; set; }

        public string Slug { get; set; }

        public string Detail { get; set; }

        public string Image { get; set; }

        public decimal Price { get; set; }

        public decimal SalePrice { get; set; }

        public int Amount { get; set; }

        public string MetaDesc { get; set; }

        public string MetaKey { get; set; }

        public int CreateBy { get; set; }

        public DateTime CreateAt { get; set; }

        public int? UpdateBy { get; set; }

        public DateTime? UpdateAt { get; set; }

        public int Status { get; set; }
    }
}

