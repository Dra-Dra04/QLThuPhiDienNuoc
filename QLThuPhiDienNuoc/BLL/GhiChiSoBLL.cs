using QLThuPhiDienNuoc.DAL;
using QLThuPhiDienNuoc.DTO;
using System;
using System.Data;

namespace QLThuPhiDienNuoc.BLL
{
    public class GhiChiSoBLL
    {
        GhiChiSoDAL dal = new GhiChiSoDAL();

        public DataTable LayDanhSachNhapChiSo(string loai, int thang, int nam)
        {
            return dal.GetDanhSachGhiChiSo(loai, thang, nam);
        }

        public bool LuuChiSo(GhiChiSoDTO cs)
        {
            if (cs.ChiSoMoi > 0 && cs.ChiSoMoi < cs.ChiSoCu)
            {
                throw new Exception($"Chỉ số mới ({cs.ChiSoMoi}) đang nhập nhỏ hơn chỉ số cũ ({cs.ChiSoCu}) của hộ {cs.MaHoDan}!");
            }
            return dal.LuuChiSo(cs);
        }
    }
}