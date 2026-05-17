using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLThuPhiDienNuoc.DTO
{
    public class ChiTietBangGiaDTO
    {
        public int ID { get; set; }
        public int IDBangGia { get; set; }
        public string Loai { get; set; }
        public int Bac { get; set; }
        public int TuChiSo { get; set; }

        public int? DenChiSo { get; set; }

        public decimal DonGia { get; set; }

        public string DenChiSoHienThi => DenChiSo.HasValue ? DenChiSo.Value.ToString() : "Trở lên";
    }
}

