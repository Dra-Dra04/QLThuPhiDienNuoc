using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLThuPhiDienNuoc.DTO;
using QLThuPhiDienNuoc.DAL;
using System.Data;

namespace QLThuPhiDienNuoc.BLL
{
    public class TaiKhoanBLL
    {
        TaiKhoanDAL dal = new TaiKhoanDAL();

        public DataTable Login(string sdt, string matKhau)
        {
            return dal.CheckLogin(sdt, matKhau);
        }
        public DataTable GetAllTaiKhoan()
        {
            return dal.LayDanhSachTaiKhoan();
        }
        public void RegisterClient(TaiKhoanDTO tk)
        {
            dal.InsertTaiKhoanClient(tk);
        }
        public bool UpDateThongTin(TaiKhoanDTO tk)
        {
            if (string.IsNullOrWhiteSpace(tk.HoTen) || string.IsNullOrWhiteSpace(tk.MatKhau))
            {
                throw new Exception("Họ tên và mật khẩu không được để trống.");
            }
            return dal.CapNhatThongTinCaNhan(tk);
        }

        public bool ThemTaiKhoanMoi(TaiKhoanDTO tk)
        {
            if (string.IsNullOrWhiteSpace(tk.SoDienThoai) || string.IsNullOrWhiteSpace(tk.MatKhau)
                || string.IsNullOrWhiteSpace(tk.HoTen) || string.IsNullOrWhiteSpace(tk.VaiTro))
            {
                throw new Exception("Vui lòng nhập đầy đủ các thông tin bắt buộc (Vai trò, Họ tên, SĐT, Mật khẩu)!");
            }
            string[] validRoles = { "Admin", "Staff", "Client" };
            if (!validRoles.Contains(tk.VaiTro))
            {
                throw new Exception("Vai trò không hợp lệ");
            }

            if (dal.CheckTonTaiSDT(tk.SoDienThoai))
            {
                throw new Exception("Số điện thoại đã tồn tại. Vui lòng chọn số khác.");
            }
            return dal.InsertTaiKhoan(tk);
        }

        public DataTable LayChiTietTaiKhoan(string sdt)
        {
            return dal.GetTaiKhoanBySDT(sdt);
        }
        public bool AdminSuaTaiKhoan(TaiKhoanDTO tk)
        {
            if (string.IsNullOrWhiteSpace(tk.HoTen) || string.IsNullOrWhiteSpace(tk.MatKhau))
            {
                throw new Exception("Họ tên và mật khẩu không được để trống.");
            }
            return dal.AdminCapNhatTaiKhoan(tk);
        }

        // CLIENT//
        public bool CapNhatThongTinClient(TaiKhoanDTO tk)
        {
            return dal.CapNhatThongTin(tk);
        }
    }
}
