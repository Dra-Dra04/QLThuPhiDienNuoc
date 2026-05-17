using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLThuPhiDienNuoc.DTO;

namespace QLThuPhiDienNuoc.DAL
{
    public class BangGiaDAL
    {
        DatabaseHelper db = new DatabaseHelper();

        public DataTable GetAllBangGia()
        {
            string query = @"
            SELECT 
                    ID AS [Mã bảng giá], 
                    TenBangGia AS [Tên bảng giá], 
                    NgayApDung AS [Ngày áp dụng],
                    PhanTramThue AS [Thuế VAT (%)]
                FROM BangGia 
                ORDER BY ID DESC";
            return db.ExecuteQuery(query);
        }
        public List<ChiTietBangGiaDTO> GetChiTietBangGiaList(int idBangGia, string loai)
        {
            List<ChiTietBangGiaDTO> danhSach = new List<ChiTietBangGiaDTO>();
            string query = "SELECT ID, IDBangGia, Loai, Bac, TuChiSo, DenChiSo, DonGia " +
                           "FROM ChiTietBangGia WHERE IDBangGia = @id AND Loai = @loai ORDER BY Bac ASC";

            DataTable dt = db.ExecuteQuery(query, new object[] { idBangGia, loai });

            foreach (DataRow row in dt.Rows)
            {
                ChiTietBangGiaDTO ct = new ChiTietBangGiaDTO
                {
                    ID = Convert.ToInt32(row["ID"]),
                    IDBangGia = Convert.ToInt32(row["IDBangGia"]),
                    Loai = row["Loai"].ToString(),
                    Bac = Convert.ToInt32(row["Bac"]),
                    TuChiSo = Convert.ToInt32(row["TuChiSo"]),

                    DenChiSo = row["DenChiSo"] == DBNull.Value ? (int?)null : Convert.ToInt32(row["DenChiSo"]),

                    DonGia = Convert.ToDecimal(row["DonGia"])
                };

                danhSach.Add(ct);
            }


            return danhSach;
        }
        public int ThemBangGiaChinh(BangGiaDTO bg)
        {
            string query = "INSERT INTO BangGia (TenBangGia, NgayApDung, PhanTramThue) " +
                           "OUTPUT INSERTED.ID " +
                           "VALUES (@TenBangGia, @NgayApDung, @PhanTramThue)";

            DataTable dt = db.ExecuteQuery(query, new object[] { bg.TenBangGia, bg.NgayApDung, bg.PhanTramThue });

            if (dt.Rows.Count > 0)
            {
                return Convert.ToInt32(dt.Rows[0][0]);
            }
            
            return 0;
            
        }

        public bool ThemChiTietBangGia(ChiTietBangGiaDTO ct)
        {
            string query = "INSERT INTO ChiTietBangGia (IDBangGia, Loai, Bac, TuChiSo, DenChiSo, DonGia) " +
                           "VALUES (@IDBangGia, @Loai, @Bac, @TuChiSo, @DenChiSo, @DonGia)";

            // Chỗ này quan trọng: Nếu DenChiSo là null (bậc cuối), ta phải truyền DBNull.Value vào SQL
            object valDenChiSo = ct.DenChiSo.HasValue ? (object)ct.DenChiSo.Value : DBNull.Value;

            int result = db.ExecuteNonQuery(query, new object[] { ct.IDBangGia, ct.Loai, ct.Bac, ct.TuChiSo, valDenChiSo, ct.DonGia });
            return result > 0;
        }
        public DataTable GetBangGiaApDungTheoThangNam(int thang, int nam)
        {
            string query = @"
                            SELECT TOP 1 ID, PhanTramThue 
                            FROM BangGia 
                            WHERE YEAR(NgayApDung) < @Nam 
                               OR (YEAR(NgayApDung) = @Nam AND MONTH(NgayApDung) <= @Thang)
                            ORDER BY NgayApDung DESC";

            return db.ExecuteQuery(query, new object[] { nam, thang });
        }
        public DataTable GetChiTietBangGia(int maBangGia, string loai)
        {
            string query = "SELECT * FROM ChiTietBangGia WHERE IDBangGia = @id AND Loai = @loai ORDER BY Bac ASC";

            // Truyền 2 tham số: ID Bảng giá và Loại (Điện/Nước)
            return db.ExecuteQuery(query, new object[] { maBangGia, loai });
        }
    }
}
