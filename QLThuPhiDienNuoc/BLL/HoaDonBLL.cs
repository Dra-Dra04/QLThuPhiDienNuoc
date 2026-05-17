using QLThuPhiDienNuoc.DAL;
using QLThuPhiDienNuoc.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLThuPhiDienNuoc.BLL
{
    public class HoaDonBLL
    {
        HoaDonDAL dal = new HoaDonDAL();
        BangGiaBLL bllBangGia = new BangGiaBLL();

        public DataTable LayHoaDonDaTonTai(string maHoDan, int thang, int nam)
        {
            return dal.GetHoaDonTheoThangNam(maHoDan, thang, nam);
        }

        public DataTable LayThongTinGhiChiSo(string maHoDan, int thang, int nam)
        {
            return dal.GetThongTinTinhTien(maHoDan, thang, nam);
        }

        public bool XuatHoaDonMoi(HoaDonDTO hd)
        {
            if (dal.KiemTraTonTaiHoaDon(hd.MaHoDan, hd.Thang, hd.Nam))
            {
                throw new Exception($"Hóa đơn tháng {hd.Thang}/{hd.Nam} của hộ {hd.MaHoDan} đã tồn tại!");
            }

            return dal.InsertHoaDon(hd);
        }
        public bool CapNhatTrangThaiHoaDon(string maHo, int thang, int nam, string trangThai)
        {
            return dal.CapNhatTrangThai(maHo, thang, nam, trangThai);
        }
        public DataTable ThongKeTheoThang(int thang, int nam)
        {
            return dal.GetThongKeTheoThang(thang, nam);
        }
        public DataTable LayLichSuTieuThuHoDan(string maHo)
        {
            return dal.GetLichSuTieuThuHoDan(maHo);
        }
        public bool XacNhanThanhToan(string maHo, int thang, int nam)
        {
            return dal.ThanhToanHoaDon(maHo, thang, nam);
        }
    }
}
