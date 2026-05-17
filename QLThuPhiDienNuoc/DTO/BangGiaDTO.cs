using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLThuPhiDienNuoc.DTO
{
    public class BangGiaDTO
    {
        public int ID { get; set; }
        public string TenBangGia { get; set; }
        public DateTime NgayApDung { get; set; }
        public int PhanTramThue { get; set; }

        public List<ChiTietBangGiaDTO> DanhSachChiTiet { get; set; }
        public BangGiaDTO()
        {
            DanhSachChiTiet = new List<ChiTietBangGiaDTO>();
        }
    }
}
