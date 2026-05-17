using QLThuPhiDienNuoc.DTO;
using System.Data;

namespace QLThuPhiDienNuoc.DAL
{
    public class GhiChiSoDAL
    {
        DatabaseHelper db = new DatabaseHelper();

        public DataTable GetDanhSachGhiChiSo(string loai, int thang, int nam)
        {
            string query = @"
                SELECT 
                    h.MaHoDan, 
                    CASE WHEN @Loai = 'Dien' THEN h.MaDongHoDien ELSE h.MaDongHoNuoc END AS [Mã công tơ],
                    ISNULL(cur.ChiSoCu, 
                        ISNULL((SELECT TOP 1 prev.ChiSoMoi 
                                FROM GhiChiSo prev 
                                WHERE prev.MaHoDan = h.MaHoDan AND prev.Loai = @Loai 
                                  AND (prev.Nam < @Nam OR (prev.Nam = @Nam AND prev.Thang < @Thang))
                                ORDER BY prev.Nam DESC, prev.Thang DESC), 0)
                    ) AS [Chỉ số cũ],
                    ISNULL(cur.ChiSoMoi, 0) AS [Chỉ số mới]
                FROM HoDan h
                LEFT JOIN GhiChiSo cur ON h.MaHoDan = cur.MaHoDan AND cur.Thang = @Thang AND cur.Nam = @Nam AND cur.Loai = @Loai
                WHERE h.TrangThai = 1";

            return db.ExecuteQuery(query, new object[] { loai, nam, thang });
        }

        public bool LuuChiSo(GhiChiSoDTO cs)
        {
            string query = @"
                IF EXISTS (SELECT 1 FROM GhiChiSo WHERE MaHoDan = @MaHD AND Thang = @Thang AND Nam = @Nam AND Loai = @Loai)
                BEGIN
                    UPDATE GhiChiSo 
                    SET ChiSoCu = @ChiSoCu, ChiSoMoi = @ChiSoMoi, NgayGhi = @NgayGhi 
                    WHERE MaHoDan = @MaHD AND Thang = @Thang AND Nam = @Nam AND Loai = @Loai
                END
                ELSE
                BEGIN
                    INSERT INTO GhiChiSo (MaHoDan, Loai, Thang, Nam, NgayGhi, ChiSoCu, ChiSoMoi) 
                    VALUES (@MaHD, @Loai, @Thang, @Nam, @NgayGhi, @ChiSoCu, @ChiSoMoi)
                END";

            int result = db.ExecuteNonQuery(query, new object[] {
                cs.MaHoDan, cs.Thang, cs.Nam, cs.Loai, cs.ChiSoCu, cs.ChiSoMoi, cs.NgayGhi
            });
            return result > 0;
        }
    }
}