using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLThuPhiDienNuoc.DTO
{
    public class HoaDonDTO
    {
        public string MaHoaDon { get; set; }
        public string MaHoDan { get; set; }
        public int Thang { get; set; }
        public int Nam { get; set; }
        public int TongSoDien { get; set; }
        public int TongSoNuoc { get; set; }
        public decimal TienDien { get; set; }
        public decimal TienNuoc { get; set; }
        public int PhanTramThue { get; set; }
        public decimal ThueGTGT { get; set; }
        public decimal TongTien { get; set; }

        public string TrangThai { get; set; }
        public DateTime? NgayThanhToan { get; set; }
    }
}
