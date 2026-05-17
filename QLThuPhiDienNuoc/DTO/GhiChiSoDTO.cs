using System;

namespace QLThuPhiDienNuoc.DTO
{
    public class GhiChiSoDTO
    {
        public int MaGhi { get; set; }
        public string MaHoDan { get; set; }
        public string Loai { get; set; }
        public int Thang { get; set; }
        public int Nam { get; set; }
        public DateTime NgayGhi { get; set; }
        public int ChiSoCu { get; set; }
        public int ChiSoMoi { get; set; }
    }
}