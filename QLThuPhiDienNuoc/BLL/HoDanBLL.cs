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
   public class HoDanBLL
    {
        HoDanDAL dal = new HoDanDAL();

        public DataTable LayDanhSachHoDan()
        {
            return dal.GetAllHoDan();
        }
        public DataTable LayDanhSachMaHoDan()
        {
            return dal.GetAllMaHoDan();
        }
        public string LayTenChuHo(string maHo)
        {
            return dal.GetTenChuHo(maHo);
        }

        public string KiemTraVaThemHoDan(HoDanDTO hd)
        {
            if(string.IsNullOrWhiteSpace(hd.MaHoDan)|| string.IsNullOrWhiteSpace(hd.TenChuHo))
            {
                return "Vui lòng nhập đầy đủ các thông tin cần thiết!";
            }
            string check = dal.KiemTraTrung(hd.MaHoDan, hd.MaDongHoDien, hd.MaDongHoNuoc);
            if (check != "")
            {
                return check;
            }
            if (dal.ThemHoDan(hd))
                return "Thành công";
            else
                return "Thêm thất bại";
        }
        public bool SuaHoDan(HoDanDTO hd)
        {
            if(string.IsNullOrWhiteSpace(hd.MaHoDan) || string.IsNullOrWhiteSpace(hd.TenChuHo))
            {
                return false;
            }
            return dal.SuaHoDan(hd);
        }
    }
}
