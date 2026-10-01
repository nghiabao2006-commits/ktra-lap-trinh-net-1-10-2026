using System;

namespace Ứng_dụng_quản_lí_danh_mục_thiết_bị_công_nghệ
{
    public class Product
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string Category { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string ImagePath { get; set; }
    }
}
