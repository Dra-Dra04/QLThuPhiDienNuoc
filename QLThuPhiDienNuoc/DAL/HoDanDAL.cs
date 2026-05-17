using QLThuPhiDienNuoc.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLThuPhiDienNuoc.DAL
{
    public class HoDanDAL
    {
        DatabaseHelper db = new DatabaseHelper();
        public DataTable GetAllHoDan()
        {
            string query = "SELECT MaHoDan AS N'Mã Hộ', TenChuHo AS N'Tên Chủ Hộ', SoDienThoai AS N'Số Điện Thoại', DiaChi AS N'Địa Chỉ', MaDongHoDien AS N'Mã CT Điện', MaDongHoNuoc AS N'Mã CT Nước' FROM HoDan WHERE TrangThai = 1";
            return db.ExecuteQuery(query);
        }
        public DataTable GetAllMaHoDan()
        {
            string query = "SELECT MaHoDan FROM HoDan WHERE TrangThai = 1";
            return db.ExecuteQuery(query);
        }

        public string KiemTraTrung(string maHoDan, string maCongToDien, string maCongToNuoc)
        {
            string query1 = "SELECT * FROM HoDan WHERE MaHoDan = @MaHoDan ";
            DataTable dt1 = db.ExecuteQuery(query1, new object[] { maHoDan });
            if (dt1.Rows.Count > 0) return "Mã hộ dân đã tồn tại!";

            if (!string.IsNullOrWhiteSpace(maCongToDien))
            {
                string query2 = "SELECT * FROM HoDan WHERE MaDongHoDien = @MaDongHoDien ";
                DataTable dt2 = db.ExecuteQuery(query2, new object[] { maCongToDien });
                if (dt2.Rows.Count > 0) return "Mã điện đã tồn tại!";
            }

            if (!string.IsNullOrWhiteSpace(maCongToNuoc))
            {
                string query3 = "SELECT * FROM HoDan WHERE MaDongHoNuoc = @MaDongHoNuoc ";
                DataTable dt3 = db.ExecuteQuery(query3, new object[] { maCongToNuoc });
                if (dt3.Rows.Count > 0) return "Mã nước đã tồn tại!";
            }

            return "";
        }

        public bool ThemHoDan(HoDanDTO hd)
        {
            string query = "INSERT INTO HoDan (MaHoDan, TenChuHo, SoDienThoai, DiaChi, MaDongHoDien, MaDongHoNuoc, TrangThai) " +
                           "VALUES ( @MaHoDan , @TenChuHo , @SoDienThoai , @DiaChi , @MaDongHoDien , @MaDongHoNuoc , 1)";
            object[] parameters = new object[] {
                hd.MaHoDan, hd.TenChuHo, hd.SoDienThoai, hd.DiaChi, hd.MaDongHoDien, hd.MaDongHoNuoc
            };

            int result = db.ExecuteNonQuery(query, parameters);
            return result > 0;
        }
        public bool SuaHoDan(HoDanDTO hd)
        {
            string query = "UPDATE HoDan SET TenChuHo = @TenChuHo , SoDienThoai = @SoDienThoai , DiaChi = @DiaChi , MaDongHoDien = @MaDongHoDien , MaDongHoNuoc = @MaDongHoNuoc WHERE MaHoDan = @MaHoDan";

            object[] parameters = new object[] {
        hd.TenChuHo,
        hd.SoDienThoai,
        hd.DiaChi,
        hd.MaDongHoDien,
        hd.MaDongHoNuoc,
        hd.MaHoDan
        };

            int result = db.ExecuteNonQuery(query, parameters);

            return result > 0;
        }
        public string GetTenChuHo(string maHo)
        {
            string query = "SELECT TenChuHo FROM HoDan WHERE MaHoDan = '" + maHo + "'";
            DataTable dt = db.ExecuteQuery(query);

            if (dt != null && dt.Rows.Count > 0)
            {
                return dt.Rows[0][0].ToString();
            }
            return "";
        }
        
    }
}

