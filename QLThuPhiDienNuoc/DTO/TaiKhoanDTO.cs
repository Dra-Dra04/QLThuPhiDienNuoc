using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLThuPhiDienNuoc.DTO
{
    public class TaiKhoanDTO
    {
        public string SoDienThoai { get; set; }
        public string MatKhau { get; set; }
        public string HoTen { get; set; }
        public DateTime NgaySinh { get; set; }
        public string DiaChi { get; set; }
        public string VaiTro { get; set; }
        public string MaHoDan { get; set; }
        public bool TrangThai { get; set; }
    }
}
