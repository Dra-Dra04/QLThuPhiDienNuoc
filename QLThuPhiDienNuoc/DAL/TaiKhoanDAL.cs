using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLThuPhiDienNuoc.DTO;
using QLThuPhiDienNuoc.DAL;
using System.Data;

namespace QLThuPhiDienNuoc.DAL
{
    public class TaiKhoanDAL
    {
        DatabaseHelper db = new DatabaseHelper();

        public DataTable CheckLogin(string sdt, string matKhau)
        {
            string query = "SELECT * FROM TaiKhoan WHERE SoDienThoai = @sdt AND MatKhau = @mk AND TrangThai = 1";
            return db.ExecuteQuery(query, new object[] { sdt, matKhau });
        }
        public DataTable LayDanhSachTaiKhoan()
        {
            string query = @"
                            SELECT 
                                MaTK AS [ID],  
                                HoTen AS [Họ và tên], 
                                SoDienThoai AS [SDT], 
                                VaiTro AS [Chức vụ], 
                                ISNULL(MaHoDan, 'Null') AS [Mã hộ dân],
                                CASE WHEN TrangThai = 1 THEN N'Hoạt động' ELSE N'Bị khóa' END AS [Trạng thái]
                            FROM TaiKhoan
                            ORDER BY TrangThai DESC, VaiTro ASC, MaTK ASC";

            return db.ExecuteQuery(query);
        }
        public int InsertTaiKhoanClient(TaiKhoanDTO tk)
        {
            string query = @"INSERT INTO TaiKhoan (SoDienThoai, MatKhau, HoTen, NgaySinh, DiaChi, VaiTro, MaHoDan) 
                             VALUES (@sdt, @mk, @hoten, @ngaysinh, @diachi, 'Client', @mahodan)";

            return db.ExecuteNonQuery(query, new object[] {
                tk.SoDienThoai,
                tk.MatKhau,
                tk.HoTen,
                tk.NgaySinh,
                tk.DiaChi,
                tk.MaHoDan
            });
        }
        public bool CapNhatThongTinCaNhan(TaiKhoanDTO tk)
        {
            string query = @"UPDATE TaiKhoan 
                             SET HoTen = @HoTen, NgaySinh = @NgaySinh, DiaChi = @Diachi
                             WHERE SoDienThoai =@SoDienThoai";
            int result = db.ExecuteNonQuery(query, new object[] {
                tk.HoTen,
                tk.NgaySinh,
                tk.DiaChi,
                tk.SoDienThoai,
                tk.MatKhau
            });
            return result > 0;

        }

        public bool CheckTonTaiSDT(string sdt)
        {
            string query = "SELECT * FROM TaiKhoan WHERE SoDienThoai = '" + sdt + "'";
            DataTable dt = db.ExecuteQuery(query);
            if (dt != null && dt.Rows.Count > 0)
            {
                return true;
            }
            return false;
        }

        public bool InsertTaiKhoan(TaiKhoanDTO tk)
        {
            string query = @"INSERT INTO TaiKhoan (SoDienThoai, MatKhau, HoTen, NgaySinh, DiaChi, VaiTro, TrangThai) 
                             VALUES (@sdt, @mk, @hoten, @ngaysinh, @diachi, @vaitro, 1)";
            int result = db.ExecuteNonQuery(query, new object[] {
                tk.SoDienThoai,
                tk.MatKhau,
                tk.HoTen,
                tk.NgaySinh,
                tk.DiaChi,
                tk.VaiTro
            });
            return result > 0;
        }

        public DataTable GetTaiKhoanBySDT(string sdt)
        {
            string query = "SELECT * FROM TaiKhoan WHERE SoDienThoai = '" + sdt + "'";
            return db.ExecuteQuery(query);
        }
        public bool AdminCapNhatTaiKhoan(TaiKhoanDTO tk)
        {
            string query = @"UPDATE TaiKhoan 
                     SET HoTen = @HoTen, NgaySinh = @NgaySinh, DiaChi = @DiaChi, 
                         MatKhau = @MatKhau, VaiTro = @VaiTro, TrangThai = @TrangThai
                     WHERE SoDienThoai = @SoDienThoai";
            int result = db.ExecuteNonQuery(query, new object[] {
        tk.HoTen, tk.NgaySinh, tk.DiaChi, tk.MatKhau, tk.VaiTro, tk.TrangThai, tk.SoDienThoai
           });
        
            return result > 0;
        }
    }
}
