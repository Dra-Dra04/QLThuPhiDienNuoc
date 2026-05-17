using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLThuPhiDienNuoc.DAL;
using QLThuPhiDienNuoc.DTO;
using System.Data;

namespace QLThuPhiDienNuoc.BLL
{
    public class BangGiaBLL
    {
        BangGiaDAL dal = new BangGiaDAL();
        public DataTable LayDanhSachBangGia()
        {
            return dal.GetAllBangGia();
        }
        public DataTable LayBangGiaApDung(int thang, int nam)
        {
            return dal.GetBangGiaApDungTheoThangNam(thang, nam);
        }
        public List<ChiTietBangGiaDTO> LayDanhSachChiTiet(int idBangGia, string loai)
        {
            return dal.GetChiTietBangGiaList(idBangGia, loai);
        }
        public bool LuuBangGiaHoanChinh(BangGiaDTO bg, List<ChiTietBangGiaDTO> danhSachChiTiet)
        {
            if (string.IsNullOrWhiteSpace(bg.TenBangGia)) throw new Exception("Tên bảng giá không được để trống!");

            int newID = dal.ThemBangGiaChinh(bg);

            if (newID > 0)
            {
                foreach (var chiTiet in danhSachChiTiet)
                {
                    chiTiet.IDBangGia = newID;
                    dal.ThemChiTietBangGia(chiTiet);
                }
                return true;
            }
            return false;
        }
        public DataTable LayChiTietBangGia(int maBangGia, string loai)
        {
            // Rào chặn lỗi cơ bản (nếu cần)
            if (maBangGia <= 0 || string.IsNullOrEmpty(loai))
            {
                throw new Exception("Thông tin mã bảng giá hoặc loại chỉ số không hợp lệ!");
            }

            // Gọi xuống tầng DAL
            return dal.GetChiTietBangGia(maBangGia, loai);
        }
    }
}
