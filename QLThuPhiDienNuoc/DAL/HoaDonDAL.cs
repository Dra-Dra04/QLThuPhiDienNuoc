using QLThuPhiDienNuoc.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLThuPhiDienNuoc.DAL
{
    public class HoaDonDAL
    {
        DatabaseHelper db = new DatabaseHelper();

        public DataTable GetThongTinTinhTien(string maHoDan, int thang, int nam)
        {
            string query = @"
                            SELECT 
                                h.TenChuHo,
                                ISNULL(SUM(CASE WHEN g.Loai = 'Dien' THEN g.TieuThu ELSE 0 END), 0) AS SoDien,
                                ISNULL(SUM(CASE WHEN g.Loai = 'Nuoc' THEN g.TieuThu ELSE 0 END), 0) AS SoNuoc
                            FROM HoDan h
                            LEFT JOIN GhiChiSo g ON h.MaHoDan = g.MaHoDan AND g.Thang = @Thang AND g.Nam = @Nam
                            WHERE h.MaHoDan = @MaHoDan
                            GROUP BY h.TenChuHo";

            return db.ExecuteQuery(query, new object[] { thang, nam, maHoDan });
        }
        public bool CapNhatTrangThai(string maHo, int thang, int nam, string trangThai)
        {
            string query = @"UPDATE HoaDon 
                     SET TrangThai = @TrangThai, 
                         NgayThanhToan = @NgayThanhToan
                     WHERE MaHoDan = @MaHo AND Thang = @Thang AND Nam = @Nam";

            object ngayNop = (trangThai == "Đã nộp") ? (object)DateTime.Now : DBNull.Value;

            int result = db.ExecuteNonQuery(query, new object[] { trangThai, ngayNop, maHo, thang, nam });

            return result > 0;
        }
        public DataTable GetHoaDonTheoThangNam(string maHoDan, int thang, int nam)
        {
            string query = "SELECT * FROM HoaDon WHERE MaHoDan = @maHo AND Thang = @thang AND Nam = @nam";
            return db.ExecuteQuery(query, new object[] { maHoDan, thang, nam });
        }

        public bool KiemTraTonTaiHoaDon(string maHo, int thang, int nam)
        {
            string query = "SELECT COUNT(*) FROM HoaDon WHERE MaHoDan = @maHo AND Thang = @thang AND Nam = @nam";
            DataTable dt = db.ExecuteQuery(query, new object[] { maHo, thang, nam });
            return Convert.ToInt32(dt.Rows[0][0]) > 0;
        }

        public bool InsertHoaDon(HoaDonDTO hd)
        {
            string query = @"INSERT INTO HoaDon (MaHoaDon, MaHoDan, Thang, Nam, TongSoDien, TongSoNuoc, 
                                             TienDien, TienNuoc, PhanTramThue, ThueGTGT, TongTien, TrangThai, NgayThanhToan)
                         VALUES (@MaHoaDon, @MaHoDan, @Thang, @Nam, @TongSoDien, @TongSoNuoc, 
                                 @TienDien, @TienNuoc, @PhanTramThue, @ThueGTGT, @TongTien, @TrangThai, @NgayThanhToan)";

            object ngayThanhToan = hd.NgayThanhToan.HasValue ? (object)hd.NgayThanhToan.Value : DBNull.Value;

            int result = db.ExecuteNonQuery(query, new object[] {
            hd.MaHoaDon, hd.MaHoDan, hd.Thang, hd.Nam, hd.TongSoDien, hd.TongSoNuoc,
            hd.TienDien, hd.TienNuoc, hd.PhanTramThue, hd.ThueGTGT, hd.TongTien, hd.TrangThai, ngayThanhToan
        });
            return result > 0;
        }
        public DataTable GetThongKeTheoThang(int thang, int nam)
        {
            string query = "SELECT MaHoDan, TongSoDien, TongSoNuoc FROM HoaDon WHERE Thang = @Thang AND Nam = @Nam ORDER BY MaHoDan ASC";
            return db.ExecuteQuery(query, new object[] { thang, nam });
        }
        public DataTable GetLichSuTieuThuHoDan(string maHo)
        {
            string query = "SELECT Thang, Nam, TongSoDien, TongSoNuoc FROM HoaDon WHERE MaHoDan = @MaHo ORDER BY Nam ASC, Thang ASC";
            return db.ExecuteQuery(query, new object[] { maHo });
        }
    }
}
