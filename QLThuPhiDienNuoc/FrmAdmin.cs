using OfficeOpenXml;
using System.IO;
using QLThuPhiDienNuoc.BLL;
using QLThuPhiDienNuoc.DTO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.WinForms;
using LicenseContext = OfficeOpenXml.LicenseContext;

namespace QLThuPhiDienNuoc
{
    public partial class FrmAdmin : Form
    {
        HoDanBLL bllHoDan = new HoDanBLL();
        bool isThemMoi = true;

        GhiChiSoBLL bllGhiChiSo = new GhiChiSoBLL();
        string loaiDangChon = "Dien";

        BangGiaBLL bllBangGia = new BangGiaBLL();

        public TaiKhoanDTO taiKhoanHienTai;
        TaiKhoanBLL bllTaiKhoan = new TaiKhoanBLL();
        HoaDonBLL bllHoaDon = new HoaDonBLL();
        public FrmAdmin(TaiKhoanDTO tk)
        {
            InitializeComponent();
            taiKhoanHienTai = tk;
            if (lblTenNguoiDung != null)
            {
                lblTenNguoiDung.Text = taiKhoanHienTai.HoTen;
            }

        }
        private void FrmAdmin_Load(object sender, EventArgs e)
        {
            pnlQuanLyHoDan.Visible = true;
            pnlQuanLyHoDan.BringToFront();
            LoadDanhSachHoDan();
            LoadThongTinCaNhan();
            LoadComboBoxHoDan_TabThongKe();
        }


        private void LoadDanhSachHoDan()
        {
            try
            {
                DataTable dt = bllHoDan.LayDanhSachHoDan();
                DgvDanhSachHoDan.DataSource = dt;

                DgvDanhSachHoDan.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                DgvDanhSachHoDan.AllowUserToAddRows = false;
                DgvDanhSachHoDan.ReadOnly = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách hộ dân: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadDanhSachGhiChiSo()
        {
            try
            {
                int thang = guna2DateTimePicker1.Value.Month;
                int nam = guna2DateTimePicker1.Value.Year;

                DataTable dt = bllGhiChiSo.LayDanhSachNhapChiSo(loaiDangChon, thang, nam);
                DgvGhiChiSo.DataSource = dt;

                DgvGhiChiSo.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                DgvGhiChiSo.AllowUserToAddRows = false;

                DgvGhiChiSo.Columns["MaHoDan"].Visible = false;

                DgvGhiChiSo.Columns["Mã công tơ"].ReadOnly = true;
                DgvGhiChiSo.Columns["Chỉ số cũ"].ReadOnly = true;
                DgvGhiChiSo.Columns["Chỉ số mới"].ReadOnly = false;

                DgvGhiChiSo.Columns["Chỉ số mới"].DefaultCellStyle.BackColor = Color.LightYellow;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách ghi chỉ số: " + ex.Message);
            }
        }
        private void LoadDanhSachBangGia()
        {
            try
            {
                DataTable dt = bllBangGia.LayDanhSachBangGia();

                guna2DataGridView1.DataSource = dt;

                guna2DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                guna2DataGridView1.AllowUserToAddRows = false;
                guna2DataGridView1.ReadOnly = true;

                guna2DataGridView1.Columns["Mã bảng giá"].Width = 100;
                guna2DataGridView1.Columns["Thuế VAT (%)"].Width = 120;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách bảng giá: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadThongTinCaNhan()
        {
            if (taiKhoanHienTai != null)
            {
                TxtRoleProfile.ReadOnly = true;
                TxtSDTProfile.ReadOnly = true;

                TxtRoleProfile.FillColor = Color.LightGray;
                TxtSDTProfile.FillColor = Color.LightGray;

                TxtRoleProfile.Text = taiKhoanHienTai.VaiTro;
                TxtHoTenProfile.Text = taiKhoanHienTai.HoTen;
                guna2DateTimePicker4.Value = taiKhoanHienTai.NgaySinh;
                TxtSDTProfile.Text = taiKhoanHienTai.SoDienThoai;
                TxtAddressProfile.Text = taiKhoanHienTai.DiaChi;
                TxtPasswordProfile.Text = taiKhoanHienTai.MatKhau;

                if (taiKhoanHienTai.VaiTro == "Admin")
                {
                    BtnQuanLyHoDan.Enabled = true;
                    BtnBangGia.Enabled = true;
                    BtnQuanLyTK.Enabled = true;
                }
                else
                {
                    BtnBangGia.Enabled = false;
                    BtnQuanLyTK.Enabled = false;
                    BtnBangGia.DisabledState.FillColor = Color.SeaGreen;
                    BtnBangGia.DisabledState.ForeColor = Color.Silver;
                }

            }
            else
            {
                MessageBox.Show("Lỗi: Hệ thống chưa nhận được thông tin người đăng nhập (Biến taiKhoanHienTai đang bị NULL)!",
                        "Thông báo lỗi logic", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadDanhSachQuanLyTaiKhoan()
        {
            try
            {
                DataTable dt = bllTaiKhoan.GetAllTaiKhoan();
                guna2DataGridView4.DataSource = dt;

                guna2DataGridView4.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                guna2DataGridView4.AllowUserToAddRows = false;
                guna2DataGridView4.ReadOnly = true;

                guna2DataGridView4.Columns["ID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                guna2DataGridView4.Columns["Chức vụ"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                guna2DataGridView4.Columns["Mã Hộ Dân"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                guna2DataGridView4.Columns["ID"].Width = 50;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách tài khoản: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDuLieuTinhTien()
        {
            // 1. Kiểm tra rào chặn
            if (CbbMaHoDan.SelectedItem == null || CbbThang.SelectedItem == null || CbbNam.SelectedItem == null) return;

            string maHo = CbbMaHoDan.Text;
            int thang = Convert.ToInt32(CbbThang.Text);
            int nam = Convert.ToInt32(CbbNam.Text);

            try
            {
                DataTable dtBangGia = bllBangGia.LayBangGiaApDung(thang, nam);
                int maBangGia = 0;
                int phanTramThue = 0;

                if (dtBangGia != null && dtBangGia.Rows.Count > 0)
                {
                    maBangGia = Convert.ToInt32(dtBangGia.Rows[0]["ID"]);
                    phanTramThue = Convert.ToInt32(dtBangGia.Rows[0]["PhanTramThue"]);
                    TxtMaBangGia.Text = maBangGia.ToString();
                    TxtPhanTramThue.Text = phanTramThue.ToString() + "%";
                }

                DataTable dtChiSo = bllHoaDon.LayThongTinGhiChiSo(maHo, thang, nam);
                if (dtChiSo == null || dtChiSo.Rows.Count == 0)
                {
                    // NẾU CHƯA CÓ CHỈ SỐ: Xóa trắng form, Khóa mọi thứ
                    TxtSoDien.Text = ""; TxtSoNuoc.Text = "";
                    BtnLuu.Enabled = false;
                    BtnXemChiTiet.Enabled = false;
                    lblTrangThaiTinhTien.Text = "Hộ này chưa ghi chỉ số tháng này!";
                    return;
                }

                TxtTTTenChuHo.Text = dtChiSo.Rows[0]["TenChuHo"].ToString();
                double soDien = Convert.ToDouble(dtChiSo.Rows[0]["SoDien"]);
                double soNuoc = Convert.ToDouble(dtChiSo.Rows[0]["SoNuoc"]);
                TxtSoDien.Text = soDien.ToString();
                TxtSoNuoc.Text = soNuoc.ToString();

                // Kiểm tra Hóa đơn trong CSDL
                DataTable dtHoaDonCu = bllHoaDon.LayHoaDonDaTonTai(maHo, thang, nam);

                if (dtHoaDonCu != null && dtHoaDonCu.Rows.Count > 0)
                {
                    // =========================================================
                    // TRẠNG THÁI 1: ĐÃ LƯU TRONG CSDL
                    // =========================================================
                    DataRow hd = dtHoaDonCu.Rows[0];
                    TxtThanhTienDien.Text = Convert.ToDecimal(hd["TienDien"]).ToString("N0") + " VNĐ";
                    TxtThanhTienNuoc.Text = Convert.ToDecimal(hd["TienNuoc"]).ToString("N0") + " VNĐ";
                    TxtTongThu.Text = Convert.ToDecimal(hd["TongTien"]).ToString("N0") + " VNĐ";
                    CbbTrangThai.Text = hd["TrangThai"].ToString();

                    lblTrangThaiTinhTien.Text = "Hóa đơn này đã lưu";
                    lblTrangThaiTinhTien.ForeColor = Color.Green;

                    BtnLuu.Text = "Đã lưu";
                    BtnLuu.Enabled = false;       
                    BtnXemChiTiet.Enabled = true; 
                    BtnXuatHoaDon.Enabled = true;
                }
                else
                {
                    // =========================================================
                    // TRẠNG THÁI 2: CHƯA LƯU (BẢNG TÍNH NHÁP)
                    // =========================================================
                    if (maBangGia > 0)
                    {
                        decimal tienDien = TinhTienTheoBacThang(maBangGia, "Dien", soDien);
                        decimal tienNuoc = TinhTienTheoBacThang(maBangGia, "Nuoc", soNuoc);

                        TxtThanhTienDien.Text = tienDien.ToString("N0") + " VNĐ";
                        TxtThanhTienNuoc.Text = tienNuoc.ToString("N0") + " VNĐ";
                        decimal tongTien = (tienDien + tienNuoc) + ((tienDien + tienNuoc) * phanTramThue / 100);
                        TxtTongThu.Text = tongTien.ToString("N0") + " VNĐ";
                        CbbTrangThai.Text = "Chưa nộp";

                        lblTrangThaiTinhTien.Text = "Bảng tính nháp";
                        lblTrangThaiTinhTien.ForeColor = Color.Orange;

                        BtnLuu.Text = "Lưu";
                        BtnLuu.Enabled = true;        
                        BtnXemChiTiet.Enabled = false;
                        BtnXuatHoaDon.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hiển thị dữ liệu: " + ex.Message);
            }
        }

        private void BtnClosed_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn đóng hệ thống?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }
        private void BtnThemHoDan_Click(object sender, EventArgs e)
        {
            isThemMoi = true;
            TxtMaHoDan.Enabled = true;
            BtnAddHoDan.Text = "Thêm Hộ Dân";

            TxtMaHoDan.Clear();
            TxtTenChuHo.Clear();
            TxtSoDienThoai.Clear();
            TxtDiaChi.Clear();
            TxtMaCTDien.Clear();
            TxtMaCTNuoc.Clear();
            TxtTrangThai.Text = "1";

            TxtMaHoDan.Focus();

            pnlThemHoDan.Visible = true;
            pnlThemHoDan.BringToFront();
        }
        private void BtnTHDTroVe_Click(object sender, EventArgs e)
        {
            pnlThemHoDan.Visible = false;
            pnlQuanLyHoDan.Visible = true;
            pnlQuanLyHoDan.BringToFront();
        }

        private void BtnQuanLyHoDan_Click(object sender, EventArgs e)
        {
            pnlQuanLyHoDan.Visible = true;
            pnlQuanLyHoDan.BringToFront();
        }

        private void BtnAddHoDan_Click(object sender, EventArgs e)
        {
            HoDanDTO hdMoi = new HoDanDTO();
            hdMoi.MaHoDan = TxtMaHoDan.Text.Trim();
            hdMoi.TenChuHo = TxtTenChuHo.Text.Trim();
            hdMoi.SoDienThoai = TxtSoDienThoai.Text.Trim();
            hdMoi.DiaChi = TxtDiaChi.Text.Trim();
            hdMoi.MaDongHoDien = TxtMaCTDien.Text.Trim();
            hdMoi.MaDongHoNuoc = TxtMaCTNuoc.Text.Trim();

            string ketQua = bllHoDan.KiemTraVaThemHoDan(hdMoi);

            if (ketQua == "Thành công")
            {
                MessageBox.Show("Thêm hộ dân thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDanhSachHoDan();
                //pnlThemHoDan.Visible = false;
                //pnlQuanLyHoDan.Visible = true;
                //pnlQuanLyHoDan.BringToFront();

            }
            else
            {
                MessageBox.Show(ketQua, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void BtnSuaHoDan_Click(object sender, EventArgs e)
        {
            if (DgvDanhSachHoDan.SelectedRows.Count > 0)
            {
                DataGridViewRow row = DgvDanhSachHoDan.SelectedRows[0];

                TxtFixMaHoDan.Text = row.Cells["Mã Hộ"].Value.ToString();
                TxtFixTenChuHo.Text = row.Cells["Tên Chủ Hộ"].Value.ToString();
                TxtFixSoDienThoai.Text = row.Cells["Số Điện Thoại"].Value.ToString();
                TxtFixDiaChi.Text = row.Cells["Địa Chỉ"].Value.ToString();
                TxtFixMaCTDien.Text = row.Cells["Mã CT Điện"].Value.ToString();
                TxtFixMaCTNuoc.Text = row.Cells["Mã CT Nước"].Value.ToString();

                TxtFixMaHoDan.Enabled = false;

                pnlSuaHoDan.Visible = true;
                pnlSuaHoDan.BringToFront();
            }
            else
            {
                MessageBox.Show("Vui lòng click chọn toàn bộ một dòng trên bảng để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void BtnSuaHD_Click(object sender, EventArgs e)
        {
            HoDanDTO hdSua = new HoDanDTO
            {
                MaHoDan = TxtFixMaHoDan.Text.Trim(),
                TenChuHo = TxtFixTenChuHo.Text.Trim(),
                SoDienThoai = TxtFixSoDienThoai.Text.Trim(),
                DiaChi = TxtFixDiaChi.Text.Trim(),
                MaDongHoDien = TxtFixMaCTDien.Text.Trim(),
                MaDongHoNuoc = TxtFixMaCTNuoc.Text.Trim()
            };

            bool ketQua = bllHoDan.SuaHoDan(hdSua);

            if (ketQua == true)
            {
                MessageBox.Show("Cập nhật thông tin hộ dân thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadDanhSachHoDan();

                pnlQuanLyHoDan.BringToFront();
            }
            else
            {
                MessageBox.Show("Cập nhật thất bại. Vui lòng kiểm tra lại thông tin!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            pnlQuanLyHoDan.BringToFront();
        }

        private void BtnDien_Click(object sender, EventArgs e)
        {
            loaiDangChon = "Dien";
            BtnDien.FillColor = Color.Goldenrod;
            BtnNuoc.FillColor = Color.LightGray;
            LoadDanhSachGhiChiSo();
        }

        private void BtnNuoc_Click(object sender, EventArgs e)
        {
            loaiDangChon = "Nuoc";
            BtnNuoc.FillColor = Color.DeepSkyBlue;
            BtnDien.FillColor = Color.LightGray;
            LoadDanhSachGhiChiSo();
        }

        private void guna2DateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            LoadDanhSachGhiChiSo();
        }

        private void BtnCapNhatChiSo_Click(object sender, EventArgs e)
        {
            try
            {
                int thang = guna2DateTimePicker1.Value.Month;
                int nam = guna2DateTimePicker1.Value.Year;
                DateTime ngayGhi = guna2DateTimePicker1.Value;
                int soDongThanhCong = 0;

                foreach (DataGridViewRow row in DgvGhiChiSo.Rows)
                {
                    int chiSoMoi = 0;
                    if (row.Cells["Chỉ số mới"].Value != DBNull.Value && row.Cells["Chỉ số mới"].Value != null)
                    {
                        int.TryParse(row.Cells["Chỉ số mới"].Value.ToString(), out chiSoMoi);
                    }
                    if (chiSoMoi > 0)
                    {
                        GhiChiSoDTO csDTO = new GhiChiSoDTO
                        {
                            MaHoDan = row.Cells["MaHoDan"].Value.ToString(),
                            Loai = loaiDangChon,
                            Thang = thang,
                            Nam = nam,
                            NgayGhi = ngayGhi,
                            ChiSoCu = Convert.ToInt32(row.Cells["Chỉ số cũ"].Value),
                            ChiSoMoi = chiSoMoi
                        };

                        bllGhiChiSo.LuuChiSo(csDTO);
                        soDongThanhCong++;
                    }
                }

                MessageBox.Show($"Lưu thành công {soDongThanhCong} chỉ số hộ dân!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDanhSachGhiChiSo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi cập nhật", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnGhiChiSo_Click(object sender, EventArgs e)
        {
            pnlGhiChiSo.Visible = true;
            pnlGhiChiSo.BringToFront();
        }

        private void BtnXuatMauExcel_Click(object sender, EventArgs e)
        {
            int thang = guna2DateTimePicker1.Value.Month;
            int nam = guna2DateTimePicker1.Value.Year;

            DataTable dt = bllGhiChiSo.LayDanhSachNhapChiSo(loaiDangChon, thang, nam);

            if (dt == null || dt.Rows.Count == 0)
            {
                MessageBox.Show("Không có danh sách hộ dân để xuất file!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
            saveFileDialog.FileName = $"Mau_Ghi_Chi_So_{loaiDangChon}_Thang_{thang}_{nam}.xlsx";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    ExcelPackage.License.SetNonCommercialPersonal("Long");

                    using (ExcelPackage package = new ExcelPackage())
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("GhiChiSo");

                        worksheet.Cells[1, 1].Value = "Mã Hộ Dân";
                        worksheet.Cells[1, 2].Value = "Mã Công Tơ";
                        worksheet.Cells[1, 3].Value = "Chỉ Số Cũ";
                        worksheet.Cells[1, 4].Value = "Chỉ Số Mới (Nhân viên nhập vào đây)";

                        using (var range = worksheet.Cells[1, 1, 1, 4])
                        {
                            range.Style.Font.Bold = true;
                            range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        }

                        int rowIndex = 2;
                        foreach (DataRow row in dt.Rows)
                        {
                            worksheet.Cells[rowIndex, 1].Value = row["MaHoDan"].ToString();
                            worksheet.Cells[rowIndex, 2].Value = row["Mã công tơ"].ToString();
                            worksheet.Cells[rowIndex, 3].Value = Convert.ToInt32(row["Chỉ số cũ"]);

                            int chiSoMoi = Convert.ToInt32(row["Chỉ số mới"]);
                            worksheet.Cells[rowIndex, 4].Value = chiSoMoi > 0 ? (object)chiSoMoi : "";

                            rowIndex++;
                        }

                        worksheet.Cells.AutoFitColumns();

                        FileInfo fileInfo = new FileInfo(saveFileDialog.FileName);
                        package.SaveAs(fileInfo);
                    }

                    MessageBox.Show("Xuất file mẫu Excel thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xuất file Excel: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnThemTuExcel_Click(object sender, EventArgs e)
        {
            int thang = guna2DateTimePicker1.Value.Month;
            int nam = guna2DateTimePicker1.Value.Year;
            DateTime ngayGhi = guna2DateTimePicker1.Value;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    ExcelPackage.License.SetNonCommercialPersonal("Long");
                    int soDongThanhCong = 0;
                    int soDongThatBai = 0;
                    string danhSachLoi = "";

                    using (ExcelPackage package = new ExcelPackage(new FileInfo(openFileDialog.FileName)))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                        int rowCount = worksheet.Dimension.Rows;

                        for (int row = 2; row <= rowCount; row++)
                        {
                            var cellMaHD = worksheet.Cells[row, 1].Value;
                            var cellChiSoCu = worksheet.Cells[row, 3].Value;
                            var cellChiSoMoi = worksheet.Cells[row, 4].Value;

                            if (cellMaHD == null) continue;

                            string maHD = cellMaHD.ToString().Trim();
                            int chiSoCu = cellChiSoCu != null ? Convert.ToInt32(cellChiSoCu) : 0;
                            int chiSoMoi = 0;

                            if (cellChiSoMoi != null && !string.IsNullOrWhiteSpace(cellChiSoMoi.ToString()))
                            {
                                int.TryParse(cellChiSoMoi.ToString(), out chiSoMoi);
                            }

                            if (chiSoMoi > 0)
                            {
                                try
                                {
                                    GhiChiSoDTO csDTO = new GhiChiSoDTO
                                    {
                                        MaHoDan = maHD,
                                        Loai = loaiDangChon,
                                        Thang = thang,
                                        Nam = nam,
                                        NgayGhi = ngayGhi,
                                        ChiSoCu = chiSoCu,
                                        ChiSoMoi = chiSoMoi
                                    };

                                    bllGhiChiSo.LuuChiSo(csDTO);
                                    soDongThanhCong++;
                                }
                                catch (Exception exBll)
                                {
                                    soDongThatBai++;
                                    danhSachLoi += $"\n- Hộ {maHD}: {exBll.Message}";
                                }
                            }
                        }
                    }

                    string thongBao = $"Nhập dữ liệu hoàn tất!\n- Thành công: {soDongThanhCong} hộ.";
                    if (soDongThatBai > 0)
                    {
                        thongBao += $"\n- Thất bại: {soDongThatBai} hộ.{danhSachLoi}";
                        MessageBox.Show(thongBao, "Kết quả nhập dữ liệu có lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        MessageBox.Show(thongBao, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    LoadDanhSachGhiChiSo();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Đã xảy ra lỗi khi đọc file Excel: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void DinhDangGridChiTiet(DataGridView dgv)
        {
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.AllowUserToAddRows = false;
            dgv.ReadOnly = true;

            if (dgv.Columns["ID"] != null) dgv.Columns["ID"].Visible = false;
            if (dgv.Columns["IDBangGia"] != null) dgv.Columns["IDBangGia"].Visible = false;
            if (dgv.Columns["Loai"] != null) dgv.Columns["Loai"].Visible = false;
            if (dgv.Columns["DenChiSo"] != null) dgv.Columns["DenChiSo"].Visible = false;

            if (dgv.Columns["Bac"] != null)
            {
                dgv.Columns["Bac"].HeaderText = "Bậc";
                dgv.Columns["Bac"].DisplayIndex = 0;
            }

            if (dgv.Columns["TuChiSo"] != null)
            {
                dgv.Columns["TuChiSo"].HeaderText = "Từ chỉ số";
                dgv.Columns["TuChiSo"].DisplayIndex = 1;
            }

            if (dgv.Columns["DenChiSoHienThi"] != null)
            {
                dgv.Columns["DenChiSoHienThi"].HeaderText = "Đến chỉ số";
                dgv.Columns["DenChiSoHienThi"].DisplayIndex = 2;
            }

            if (dgv.Columns["DonGia"] != null)
            {
                dgv.Columns["DonGia"].HeaderText = "Đơn giá (VNĐ)";
                dgv.Columns["DonGia"].DefaultCellStyle.Format = "N0";
                dgv.Columns["DonGia"].DisplayIndex = 3;
            }
        }

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất khỏi hệ thống?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Restart();
            }

        }

        private void BtnBangGia_Click(object sender, EventArgs e)
        {
            pnlBangGia.Visible = true;
            pnlBangGia.BringToFront();

            LoadDanhSachBangGia();
        }
        private void BtnChiTietBangGia_Click(object sender, EventArgs e)
        {
            if (guna2DataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow row = guna2DataGridView1.SelectedRows[0];

                int idBangGia = Convert.ToInt32(row.Cells["Mã bảng giá"].Value);
                string tenBang = row.Cells["Tên bảng giá"].Value.ToString();
                DateTime ngayApDung = Convert.ToDateTime(row.Cells["Ngày áp dụng"].Value);
                string thue = row.Cells["Thuế VAT (%)"].Value.ToString();

                LblTenBang.Text = tenBang;
                guna2DateTimePicker2.Value = ngayApDung;
                TxtShowMucThue.Text = thue + "%";

                DgvChiTietBangGiaDien.DataSource = bllBangGia.LayDanhSachChiTiet(idBangGia, "Dien");
                DinhDangGridChiTiet(DgvChiTietBangGiaDien);
                DgvChiTietBangGiaNuoc.DataSource = bllBangGia.LayDanhSachChiTiet(idBangGia, "Nuoc");
                DinhDangGridChiTiet(DgvChiTietBangGiaNuoc);

                pnlChiTietBangGia.Visible = true;
                pnlChiTietBangGia.BringToFront();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một hàng trên danh sách bảng giá!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void KhoiTaoLuoiThemBangGia()
        {
            void SetupGrid(DataGridView dgv)
            {
                dgv.Columns.Clear();
                dgv.Columns.Add("Bac", "Bậc");
                dgv.Columns.Add("TuChiSo", "Từ chỉ số");
                dgv.Columns.Add("DenChiSo", "Đến chỉ số (Trống = Trở lên)");
                dgv.Columns.Add("DonGia", "Đơn giá (VNĐ)");

                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgv.AllowUserToAddRows = false;
                dgv.Columns["Bac"].ReadOnly = true;

                dgv.Columns["Bac"].DefaultCellStyle.BackColor = Color.LightGray;
            }

            SetupGrid(guna2DataGridView2);
            for (int i = 1; i <= 6; i++)
            {
                guna2DataGridView2.Rows.Add(i, "", "", "");
            }

            SetupGrid(guna2DataGridView3);
            for (int i = 1; i <= 4; i++)
            {
                guna2DataGridView3.Rows.Add(i, "", "", "");
            }
        }

        private void BtnTroVeBangGia_Click(object sender, EventArgs e)
        {
            pnlBangGia.Visible = true;
            pnlBangGia.BringToFront();
        }

        private void BtnThemBangGia_Click(object sender, EventArgs e)
        {
            try
            {
                BangGiaDTO bgMoi = new BangGiaDTO
                {
                    TenBangGia = TxtTenBang.Text.Trim(),
                    NgayApDung = guna2DateTimePicker3.Value,
                    PhanTramThue = int.Parse(TxtNhapMucThue.Text.Replace("%", "").Trim())
                };

                List<ChiTietBangGiaDTO> listChiTiet = new List<ChiTietBangGiaDTO>();

                void QuetLuoiLayDuLieu(DataGridView dgv, string loai)
                {
                    foreach (DataGridViewRow row in dgv.Rows)
                    {
                        if (row.Cells["TuChiSo"].Value == null || string.IsNullOrWhiteSpace(row.Cells["TuChiSo"].Value.ToString())) continue;

                        ChiTietBangGiaDTO ct = new ChiTietBangGiaDTO();
                        ct.Loai = loai;
                        ct.Bac = Convert.ToInt32(row.Cells["Bac"].Value);
                        ct.TuChiSo = Convert.ToInt32(row.Cells["TuChiSo"].Value);
                        ct.DonGia = Convert.ToDecimal(row.Cells["DonGia"].Value);

                        var cellDen = row.Cells["DenChiSo"].Value;
                        if (cellDen == null || string.IsNullOrWhiteSpace(cellDen.ToString()))
                        {
                            ct.DenChiSo = null;
                        }
                        else
                        {
                            ct.DenChiSo = Convert.ToInt32(cellDen);
                        }

                        listChiTiet.Add(ct);
                    }
                }

                QuetLuoiLayDuLieu(guna2DataGridView2, "Dien");
                QuetLuoiLayDuLieu(guna2DataGridView3, "Nuoc");

                if (listChiTiet.Count == 0)
                {
                    MessageBox.Show("Vui lòng nhập ít nhất 1 bậc giá!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                bool thanhCong = bllBangGia.LuuBangGiaHoanChinh(bgMoi, listChiTiet);

                if (thanhCong)
                {
                    MessageBox.Show("Thêm bảng giá mới thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDanhSachBangGia();
                    pnlBangGia.BringToFront();
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Vui lòng nhập đúng định dạng số cho Thuế, Chỉ số và Đơn giá!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnThemBangGiaMoi_Click(object sender, EventArgs e)
        {
            pnlThemBangGiaMoi.Visible = true;
            pnlThemBangGiaMoi.BringToFront();

            KhoiTaoLuoiThemBangGia();

            TxtTenBang.Clear();
            TxtNhapMucThue.Clear();
            guna2DateTimePicker3.Value = DateTime.Now;
        }
        private void BtnReload_Click(object sender, EventArgs e)
        {
            LoadThongTinCaNhan();
        }

        private void BtnUpdateProfile_Click(object sender, EventArgs e)
        {
            try
            {
                TaiKhoanDTO tkSua = new TaiKhoanDTO
                {
                    SoDienThoai = TxtSDTProfile.Text.Trim(),
                    HoTen = TxtHoTenProfile.Text.Trim(),
                    NgaySinh = guna2DateTimePicker4.Value,
                    DiaChi = TxtAddressProfile.Text.Trim(),
                    MatKhau = TxtPasswordProfile.Text.Trim(),
                    VaiTro = TxtRoleProfile.Text
                };
                bool ketQua = bllTaiKhoan.UpDateThongTin(tkSua);

                if (ketQua)
                {
                    MessageBox.Show("Cập nhật thông tin cá nhân thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    taiKhoanHienTai = tkSua;

                    LoadThongTinCaNhan();
                }
                else
                {
                    MessageBox.Show("Cập nhật thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi cập nhật thông tin cá nhân", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnQuanLyTaiKhoan_Click(object sender, EventArgs e)
        {
            pnlProfile.Visible = true;
            pnlProfile.BringToFront();
        }

        private void BtnTroVeProfile_Click(object sender, EventArgs e)
        {
            pnlProfile.BringToFront();
        }

        private void BtnQuanLyTK_Click(object sender, EventArgs e)
        {
            pnlQuanLyTaiKhoan.Visible = true;
            pnlQuanLyTaiKhoan.BringToFront();
            LoadDanhSachQuanLyTaiKhoan();
        }

        private void BtnTaoTaiKhoan_Click(object sender, EventArgs e)
        {
            try
            {
                TaiKhoanDTO tkMoi = new TaiKhoanDTO
                {
                    VaiTro = CboAddRole.Text,
                    HoTen = TxtAddHoTen.Text.Trim(),
                    NgaySinh = guna2DateTimePicker5.Value,
                    SoDienThoai = TxtAddSDT.Text.Trim(),
                    DiaChi = TxtAddDiaChi.Text.Trim(),
                    MatKhau = TxtAddMatKhau.Text.Trim(),
                };
                if (bllTaiKhoan.ThemTaiKhoanMoi(tkMoi))
                {
                    MessageBox.Show("Tạo tài khoản mới thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CboAddRole.SelectedIndex = 0;
                    TxtAddHoTen.Clear();
                    TxtAddDiaChi.Clear();
                    TxtAddMatKhau.Clear();
                    TxtAddSDT.Clear();
                    guna2DateTimePicker5.Value = DateTime.Now;

                    pnlQuanLyTaiKhoan.Visible = true;
                    pnlQuanLyTaiKhoan.BringToFront();
                    LoadDanhSachQuanLyTaiKhoan();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnThemTK_Click(object sender, EventArgs e)
        {
            pnlThemTaiKhoan.Visible = true;
            pnlThemTaiKhoan.BringToFront();
        }

        private void BtnSuaTk_Click(object sender, EventArgs e)
        {
            if (guna2DataGridView4.SelectedRows.Count > 0)
            {
                string sdt = guna2DataGridView4.SelectedRows[0].Cells["SDT"].Value.ToString();

                DataTable dt = bllTaiKhoan.LayChiTietTaiKhoan(sdt);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                    CbbSuaVaiTro.Text = row["VaiTro"].ToString();
                    TxtSuaHoTen.Text = row["HoTen"].ToString();
                    guna2DateTimePicker6.Value = Convert.ToDateTime(row["NgaySinh"]);
                    TxtSuaDiaChi.Text = row["DiaChi"].ToString();
                    TxtSuaMatKhau.Text = row["MatKhau"].ToString();

                    TxtSuaSDT.Text = row["SoDienThoai"].ToString();
                    TxtSuaSDT.ReadOnly = true;
                    TxtSuaSDT.FillColor = Color.LightGray;

                    bool trangThai = Convert.ToBoolean(row["TrangThai"]);
                    CbbSuaTrangThai.Text = trangThai ? "1" : "0";
                    pnlSuaTaiKhoan.Visible = true;
                    pnlSuaTaiKhoan.BringToFront();
                }
                else
                {
                    MessageBox.Show("Chọn tài khoản để sửa", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnSuaTaiKhoan_Click(object sender, EventArgs e)
        {
            try
            {
                TaiKhoanDTO tkSua = new TaiKhoanDTO
                {
                    VaiTro = CbbSuaVaiTro.Text,
                    HoTen = TxtSuaHoTen.Text.Trim(),
                    NgaySinh = guna2DateTimePicker6.Value,
                    SoDienThoai = TxtSuaSDT.Text.Trim(),
                    DiaChi = TxtSuaDiaChi.Text.Trim(),
                    MatKhau = TxtSuaMatKhau.Text.Trim(),

                    TrangThai = CbbSuaTrangThai.Text == "1" ? true : false
                };
                if (bllTaiKhoan.AdminSuaTaiKhoan(tkSua))
                {
                    MessageBox.Show("Cập nhật tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    pnlQuanLyTaiKhoan.BringToFront();
                    LoadDanhSachQuanLyTaiKhoan();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi cập nhật tài khoản", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void XuatHoaDonRaPDF(HoaDonDTO hd, string tenChuHo)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "PDF (*.pdf)|*.pdf";
            sfd.FileName = hd.MaHoaDon + ".pdf";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string fontPath = Environment.GetFolderPath(Environment.SpecialFolder.Fonts) + "\\arial.ttf";
                    iTextSharp.text.pdf.BaseFont bf = iTextSharp.text.pdf.BaseFont.CreateFont(fontPath, iTextSharp.text.pdf.BaseFont.IDENTITY_H, iTextSharp.text.pdf.BaseFont.EMBEDDED);

                    // THÊM iTextSharp.text. 
                    iTextSharp.text.Font fontTitle = new iTextSharp.text.Font(bf, 16, iTextSharp.text.Font.BOLD);
                    iTextSharp.text.Font fontBold = new iTextSharp.text.Font(bf, 12, iTextSharp.text.Font.BOLD);
                    iTextSharp.text.Font fontNormal = new iTextSharp.text.Font(bf, 12, iTextSharp.text.Font.NORMAL);

                    // Khởi tạo Document PDF
                    iTextSharp.text.Document doc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A5, 30, 30, 30, 30);
                    iTextSharp.text.pdf.PdfWriter.GetInstance(doc, new System.IO.FileStream(sfd.FileName, System.IO.FileMode.Create));

                    doc.Open();

                    // Vẽ nội dung hóa đơn
                    iTextSharp.text.Paragraph title = new iTextSharp.text.Paragraph("HÓA ĐƠN TIỀN ĐIỆN NƯỚC", fontTitle);
                    title.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                    doc.Add(title);
                    doc.Add(new iTextSharp.text.Paragraph(" "));

                    // Thông tin chung
                    doc.Add(new iTextSharp.text.Paragraph($"Mã hóa đơn: {hd.MaHoaDon}", fontNormal));
                    doc.Add(new iTextSharp.text.Paragraph($"Kỳ thanh toán: Tháng {hd.Thang}/{hd.Nam}", fontNormal));
                    doc.Add(new iTextSharp.text.Paragraph($"Mã hộ dân: {hd.MaHoDan}", fontNormal));
                    doc.Add(new iTextSharp.text.Paragraph($"Tên chủ hộ: {tenChuHo}", fontBold));
                    doc.Add(new iTextSharp.text.Paragraph("--------------------------------------------------", fontNormal));

                    // Bảng chi tiết tiền
                    iTextSharp.text.pdf.PdfPTable table = new iTextSharp.text.pdf.PdfPTable(2);
                    table.WidthPercentage = 100;
                    table.SpacingBefore = 10f;
                    table.SpacingAfter = 10f;

                    table.AddCell(new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase($"Tiền điện ({hd.TongSoDien} kWh)", fontNormal)) { Border = 0 });
                    table.AddCell(new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase($"{hd.TienDien:N0} VNĐ", fontNormal)) { Border = 0, HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT });

                    table.AddCell(new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase($"Tiền nước ({hd.TongSoNuoc} khối)", fontNormal)) { Border = 0 });
                    table.AddCell(new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase($"{hd.TienNuoc:N0} VNĐ", fontNormal)) { Border = 0, HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT });

                    table.AddCell(new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase($"Thuế GTGT ({hd.PhanTramThue}%)", fontNormal)) { Border = 0 });
                    table.AddCell(new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase($"{hd.ThueGTGT:N0} VNĐ", fontNormal)) { Border = 0, HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT });

                    doc.Add(table);
                    doc.Add(new iTextSharp.text.Paragraph("--------------------------------------------------", fontNormal));

                    iTextSharp.text.Paragraph tongTien = new iTextSharp.text.Paragraph($"TỔNG THANH TOÁN: {hd.TongTien:N0} VNĐ", fontBold);
                    tongTien.Alignment = iTextSharp.text.Element.ALIGN_RIGHT;
                    doc.Add(tongTien);

                    doc.Add(new iTextSharp.text.Paragraph(" "));
                    iTextSharp.text.Paragraph footer = new iTextSharp.text.Paragraph("Cảm ơn quý khách đã sử dụng dịch vụ!", fontNormal);
                    footer.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                    doc.Add(footer);

                    doc.Close();

                    MessageBox.Show("Đã xuất file PDF thành công tại:\n" + sfd.FileName, "Hoàn tất", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tạo file PDF: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void BtnXuatHoaDon_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtTongThu.Text) || TxtTongThu.Text == "0 VNĐ")
            {
                MessageBox.Show("Chưa có dữ liệu tính tiền để xuất hóa đơn!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                decimal tienDien = Convert.ToDecimal(TxtThanhTienDien.Text.Replace(" VNĐ", "").Replace(",", ""));
                decimal tienNuoc = Convert.ToDecimal(TxtThanhTienNuoc.Text.Replace(" VNĐ", "").Replace(",", ""));
                decimal tongTien = Convert.ToDecimal(TxtTongThu.Text.Replace(" VNĐ", "").Replace(",", ""));
                int thue = Convert.ToInt32(TxtPhanTramThue.Text.Replace("%", ""));

                HoaDonDTO hdInAn = new HoaDonDTO
                {
                    MaHoaDon = $"HD_{CbbMaHoDan.Text}_{CbbThang.Text}{CbbNam.Text}",
                    MaHoDan = CbbMaHoDan.Text,
                    Thang = Convert.ToInt32(CbbThang.Text),
                    Nam = Convert.ToInt32(CbbNam.Text),
                    TongSoDien = Convert.ToInt32(TxtSoDien.Text),
                    TongSoNuoc = Convert.ToInt32(TxtSoNuoc.Text),
                    TienDien = tienDien,
                    TienNuoc = tienNuoc,
                    PhanTramThue = thue,
                    ThueGTGT = (tienDien + tienNuoc) * thue / 100,
                    TongTien = tongTien
                };

                XuatHoaDonRaPDF(hdInAn, TxtTTTenChuHo.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi khi gom dữ liệu in ấn", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private decimal TinhTienTheoBacThang(int maBangGia, string loai, double tongTieuThu)
        {
            decimal tongTien = 0;
            double soKWhConLai = tongTieuThu;

            DataTable dtChiTiet = bllBangGia.LayChiTietBangGia(maBangGia, loai);

            if (dtChiTiet != null)
            {
                foreach (DataRow row in dtChiTiet.Rows)
                {
                    if (soKWhConLai <= 0) break;

                    double tuChiSo = Convert.ToDouble(row["TuChiSo"]);

                    double denChiSo = 999999;
                    if (row["DenChiSo"] != DBNull.Value && !string.IsNullOrWhiteSpace(row["DenChiSo"].ToString()))
                    {
                        denChiSo = Convert.ToDouble(row["DenChiSo"]);
                    }
                    decimal donGia = Convert.ToDecimal(row["DonGia"]);

                    double sucChuaCuaBac = denChiSo - tuChiSo + 1;

                    double soKWhTinhVaoBacNay = Math.Min(soKWhConLai, sucChuaCuaBac);

                    tongTien += (decimal)soKWhTinhVaoBacNay * donGia;

                    soKWhConLai -= soKWhTinhVaoBacNay;
                }
            }
            return tongTien;
        }

        private void CbbNam_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDuLieuTinhTien();
            if (CbbMaHoDan.SelectedItem == null || CbbThang.SelectedItem == null || CbbNam.SelectedItem == null) return;

            string maHo = CbbMaHoDan.Text;
            int thang = Convert.ToInt32(CbbThang.Text);
            int nam = Convert.ToInt32(CbbNam.Text);
            try
            {
                DataTable dtBangGia = bllBangGia.LayBangGiaApDung(thang, nam);
                int maBangGia = 0;
                int phanTramThue = 0;

                if (dtBangGia != null && dtBangGia.Rows.Count > 0)
                {
                    maBangGia = Convert.ToInt32(dtBangGia.Rows[0]["ID"]);
                    phanTramThue = Convert.ToInt32(dtBangGia.Rows[0]["PhanTramThue"]);
                    TxtMaBangGia.Text = maBangGia.ToString();
                    TxtPhanTramThue.Text = phanTramThue.ToString() + "%";
                }
                DataTable dtChiSo = bllHoaDon.LayThongTinGhiChiSo(maHo, thang, nam);
                if (dtChiSo == null || dtChiSo.Rows.Count == 0) return;

                TxtTTTenChuHo.Text = dtChiSo.Rows[0]["TenChuHo"].ToString();
                double soDien = Convert.ToDouble(dtChiSo.Rows[0]["SoDien"]);
                double soNuoc = Convert.ToDouble(dtChiSo.Rows[0]["SoNuoc"]);
                TxtSoDien.Text = soDien.ToString();
                TxtSoNuoc.Text = soNuoc.ToString();

                DataTable dtHoaDonCu = bllHoaDon.LayHoaDonDaTonTai(maHo, thang, nam);

                if (dtHoaDonCu != null && dtHoaDonCu.Rows.Count > 0)
                {
                    // === KỊCH BẢN 1: ĐÃ CÓ HÓA ĐƠN TRONG DB ===
                    DataRow hd = dtHoaDonCu.Rows[0];
                    TxtThanhTienDien.Text = Convert.ToDecimal(hd["TienDien"]).ToString("N0") + " VNĐ";
                    TxtThanhTienNuoc.Text = Convert.ToDecimal(hd["TienNuoc"]).ToString("N0") + " VNĐ";
                    TxtTongThu.Text = Convert.ToDecimal(hd["TongTien"]).ToString("N0") + " VNĐ";
                    CbbTrangThai.Text = hd["TrangThai"].ToString();

                    BtnLuu.Text = "Đã lưu";
                    BtnLuu.Enabled = false;
                    BtnXuatHoaDon.Enabled = true;
                    BtnUpdateTinhTien.Enabled = true;

                    if (lblTrangThaiTinhTien != null)
                    {
                        lblTrangThaiTinhTien.Text = "Hóa đơn này đã lưu";
                        lblTrangThaiTinhTien.ForeColor = Color.Green;
                    }
                }
                else
                {
                    // === KỊCH BẢN 2: CHƯA CÓ HÓA ĐƠN (TÍNH NHÁP NHƯNG KHÔNG LƯU) ===
                    if (maBangGia > 0)
                    {
                        decimal tienDien = TinhTienTheoBacThang(maBangGia, "Dien", soDien);
                        decimal tienNuoc = TinhTienTheoBacThang(maBangGia, "Nuoc", soNuoc);

                        TxtThanhTienDien.Text = tienDien.ToString("N0") + " VNĐ";
                        TxtThanhTienNuoc.Text = tienNuoc.ToString("N0") + " VNĐ";
                        decimal tongTien = (tienDien + tienNuoc) + ((tienDien + tienNuoc) * phanTramThue / 100);
                        TxtTongThu.Text = tongTien.ToString("N0") + " VNĐ";
                        CbbTrangThai.Text = "Chưa nộp";

                        BtnLuu.Text = "Lưu";
                        BtnLuu.Enabled = true;

                        BtnXuatHoaDon.Enabled = false;
                        BtnUpdateTinhTien.Enabled = false;

                        if (lblTrangThaiTinhTien != null)
                        {
                            lblTrangThaiTinhTien.Text = "Bảng tính nháp";
                            lblTrangThaiTinhTien.ForeColor = Color.Orange;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load dữ liệu: " + ex.Message);
            }
        }

        private void BtnTinhTien_Click(object sender, EventArgs e)
        {
            pnlTinhTien.Visible = true;
            pnlTinhTien.BringToFront();
            LamMoiGiaoDienTinhTien();
            LoadComboBoxTinhTien();
        }
        private void LoadComboBoxTinhTien()
        {
            try
            {
                CbbThang.Items.Clear();
                for (int i = 1; i <= 12; i++)
                {
                    CbbThang.Items.Add(i.ToString());
                }
                CbbNam.Items.Clear();
                int namHienTai = DateTime.Now.Year;
                for (int i = 2023; i <= namHienTai + 2; i++)
                {
                    CbbNam.Items.Add(i.ToString());
                }
                DataTable dtHoDan = bllHoDan.LayDanhSachMaHoDan();
                if (dtHoDan != null && dtHoDan.Rows.Count > 0)
                {
                    CbbMaHoDan.DataSource = dtHoDan;
                    CbbMaHoDan.DisplayMember = "MaHoDan";
                    CbbMaHoDan.ValueMember = "MaHoDan";
                }
                CbbMaHoDan.SelectedIndex = -1;
                CbbThang.SelectedIndex = -1;
                CbbNam.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách ComboBox: " + ex.Message);
            }
        }
        private void LoadComboBoxHoDan_TabThongKe()
        {
            try
            {
                DataTable dtHoDan = bllHoDan.LayDanhSachHoDan();

                if (dtHoDan != null && dtHoDan.Rows.Count > 0)
                {
                    CbMaHoDan.DataSource = null;
                    CbMaHoDan.Items.Clear();

                    foreach (DataRow row in dtHoDan.Rows)
                    {
                        CbMaHoDan.Items.Add(row[0].ToString());
                    }

                    if (CbMaHoDan.Items.Count > 0)
                    {
                        CbMaHoDan.SelectedIndex = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách hộ dân: " + ex.Message);
            }
        }

        private void BtnUpdateTinhTien_Click(object sender, EventArgs e)
        {
            if (CbbMaHoDan.SelectedItem == null || CbbThang.SelectedItem == null || CbbNam.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn Mã hộ dân, Tháng và Năm để cập nhật!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                string maHo = CbbMaHoDan.Text;
                int thang = Convert.ToInt32(CbbThang.Text);
                int nam = Convert.ToInt32(CbbNam.Text);

                string trangThaiMoi = CbbTrangThai.Text;

                if (bllHoaDon.CapNhatTrangThaiHoaDon(maHo, thang, nam, trangThaiMoi))
                {
                    MessageBox.Show($"Cập nhật trạng thái thành [{trangThaiMoi}] thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (lblTrangThaiTinhTien != null)
                    {
                        lblTrangThaiTinhTien.Text = $"Hóa đơn này {trangThaiMoi.ToLower()}";
                        lblTrangThaiTinhTien.ForeColor = trangThaiMoi == "Đã nộp" ? Color.Green : Color.Red;
                    }
                }
                else
                {
                    MessageBox.Show("Không tìm thấy hóa đơn trong cơ sở dữ liệu để cập nhật!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi trong quá trình cập nhật: " + ex.Message, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnLuu_Click(object sender, EventArgs e)
        {
            if (TxtSoDien.Text == "0" && TxtSoNuoc.Text == "0")
            {
                DialogResult dialog = MessageBox.Show("Hộ dân này chưa có chỉ số tiêu thụ điện nước (đều bằng 0). Bạn có chắc chắn muốn lưu hóa đơn 0 đồng này không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialog == DialogResult.No) return;
            }
            try
            {
                decimal tienDien = Convert.ToDecimal(TxtThanhTienDien.Text.Replace(" VNĐ", "").Replace(",", ""));
                decimal tienNuoc = Convert.ToDecimal(TxtThanhTienNuoc.Text.Replace(" VNĐ", "").Replace(",", ""));
                int phanTramThue = Convert.ToInt32(TxtPhanTramThue.Text.Replace("%", ""));

                HoaDonDTO hdMoi = new HoaDonDTO
                {
                    MaHoaDon = $"HD_{CbbMaHoDan.Text}_{CbbThang.Text}{CbbNam.Text}",
                    MaHoDan = CbbMaHoDan.Text,
                    Thang = Convert.ToInt32(CbbThang.Text),
                    Nam = Convert.ToInt32(CbbNam.Text),
                    TongSoDien = Convert.ToInt32(TxtSoDien.Text),
                    TongSoNuoc = Convert.ToInt32(TxtSoNuoc.Text),
                    TienDien = tienDien,
                    TienNuoc = tienNuoc,
                    PhanTramThue = phanTramThue,
                    ThueGTGT = (tienDien + tienNuoc) * phanTramThue / 100,
                    TongTien = Convert.ToDecimal(TxtTongThu.Text.Replace(" VNĐ", "").Replace(",", "")),
                    TrangThai = "Chưa nộp",
                    NgayThanhToan = null
                };
                if (bllHoaDon.XuatHoaDonMoi(hdMoi))
                {
                    MessageBox.Show("Đã lưu hóa đơn vào cơ sở dữ liệu thành công!", "Hoàn tất", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    BtnLuu.Text = "Đã lưu";
                    BtnLuu.Enabled = false;
                    BtnXuatHoaDon.Enabled = true;
                    BtnUpdateTinhTien.Enabled = true;

                    if (lblTrangThaiTinhTien != null)
                    {
                        lblTrangThaiTinhTien.Text = " Hóa đơn này đã lưu";
                        lblTrangThaiTinhTien.ForeColor = Color.Green;
                    }
                    LoadDuLieuTinhTien();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private DataTable TaoBangChiTietBacThang(int maBangGia, string loai, double tongTieuThu)
        {
            DataTable dtChiTietIn = new DataTable();
            dtChiTietIn.Columns.Add("Bậc", typeof(string));
            dtChiTietIn.Columns.Add("Số lượng", typeof(double));
            dtChiTietIn.Columns.Add("Đơn giá", typeof(decimal));
            dtChiTietIn.Columns.Add("Thành tiền", typeof(decimal));

            double soKWhConLai = tongTieuThu;
            DataTable dtChiTietDB = bllBangGia.LayChiTietBangGia(maBangGia, loai);

            if (dtChiTietDB != null)
            {
                int sttBac = 1;
                foreach (DataRow row in dtChiTietDB.Rows)
                {
                    if (soKWhConLai <= 0) break;

                    double tuChiSo = Convert.ToDouble(row["TuChiSo"]);
                    double denChiSo = 999999;
                    if (row["DenChiSo"] != DBNull.Value && !string.IsNullOrWhiteSpace(row["DenChiSo"].ToString()))
                    {
                        denChiSo = Convert.ToDouble(row["DenChiSo"]);
                    }

                    decimal dongGia = Convert.ToDecimal(row["DonGia"]);

                    double sucChuaCuaBac = denChiSo - tuChiSo + 1;
                    double soLuongTinh = Math.Min(soKWhConLai, sucChuaCuaBac);
                    decimal thanhTien = (decimal)soLuongTinh * dongGia;

                    dtChiTietIn.Rows.Add($"Bậc {sttBac}", Math.Round(soLuongTinh, 2), dongGia, Math.Round(thanhTien, 0));

                    soKWhConLai -= soLuongTinh;
                    sttBac++;
                }
            }
            return dtChiTietIn;
        }

        private void BtnXemChiTiet_Click(object sender, EventArgs e)
        {
            if (CbbMaHoDan.SelectedItem == null || CbbThang.SelectedItem == null || CbbNam.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn Mã hộ dân, tháng, năm cần xem chi tiết!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maHo = CbbMaHoDan.Text;
            int thang = Convert.ToInt32(CbbThang.Text);
            int nam = Convert.ToInt32(CbbNam.Text);

            try
            {
                DataTable dtHoaDon = bllHoaDon.LayHoaDonDaTonTai(maHo, thang, nam);
                if (dtHoaDon == null || dtHoaDon.Rows.Count == 0)
                {
                    MessageBox.Show("Hóa đơn này chưa được lưu, không thể xem chi tiết!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                DataRow hd = dtHoaDon.Rows[0];

                TbBangMa.Text = hd["MaHoaDon"].ToString();
                TbTenChuHo.Text = TxtTTTenChuHo.Text;
                TbSoDien.Text = hd["TongSoDien"].ToString() + " kWh";
                TbSoNuoc.Text = hd["TongSoNuoc"].ToString() + " Khối";
                TbTongThu.Text = Convert.ToDecimal(hd["TongTien"]).ToString("N0") + " VNĐ";
                TbTongThue.Text = Convert.ToDecimal(hd["ThueGTGT"]).ToString("N0") + " VNĐ";

                DataTable dtBangGia = bllBangGia.LayBangGiaApDung(thang, nam);
                if (dtBangGia != null && dtBangGia.Rows.Count > 0)
                {
                    int maBangGia = Convert.ToInt32(dtBangGia.Rows[0]["ID"]);

                    guna2DataGridView6.DataSource = TaoBangChiTietBacThang(maBangGia, "Dien", Convert.ToDouble(hd["TongSoDien"]));
                    guna2DataGridView5.DataSource = TaoBangChiTietBacThang(maBangGia, "Nuoc", Convert.ToDouble(hd["TongSoNuoc"]));

                    // Format cho cột tiền tệ hiển thị dấu phẩy đẹp mắt
                    guna2DataGridView6.Columns["Đơn giá"].DefaultCellStyle.Format = "N0";
                    guna2DataGridView6.Columns["Thành tiền"].DefaultCellStyle.Format = "N0";
                    guna2DataGridView5.Columns["Đơn giá"].DefaultCellStyle.Format = "N0";
                    guna2DataGridView5.Columns["Thành tiền"].DefaultCellStyle.Format = "N0";
                }

                pnlTinhTien.Visible = false;
                PnlChiTietHoaDon.Visible = true;
                PnlChiTietHoaDon.BringToFront();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hiển thị chi tiết: " + ex.Message);
            }
        }

        private void TroVe_Click(object sender, EventArgs e)
        {
            PnlChiTietHoaDon.Visible = false;
            pnlTinhTien.Visible = true;
            pnlTinhTien.BringToFront();
        }
        private void VeBieuDoDien(List<string> danhSachHo, ChartValues<double> soLieuDien)
        {
            LiveCharts.WinForms.CartesianChart chart = new LiveCharts.WinForms.CartesianChart();
            chart.Dock = DockStyle.Fill;

            chart.Series = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Điện tiêu thụ (kWh)",
                    Values = soLieuDien,
                    DataLabels = true
                }
            };

            chart.AxisX.Add(new Axis { Title = "Mã hộ dân", Labels = danhSachHo });
            chart.AxisY.Add(new Axis { Title = "Số kWh", LabelFormatter = value => value.ToString("N0") });

            PnlBieuDoDien.Controls.Clear();
            PnlBieuDoDien.Controls.Add(chart);
        }

        private void VeBieuDoNuoc(List<string> danhSachHo, ChartValues<double> soLieuNuoc)
        {
            LiveCharts.WinForms.CartesianChart chart = new LiveCharts.WinForms.CartesianChart();
            chart.Dock = DockStyle.Fill;

            chart.Series = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Nước tiêu thụ (Khối)",
                    Values = soLieuNuoc,
                    DataLabels = true
                }
            };

            chart.AxisX.Add(new Axis { Title = "Mã hộ dân", Labels = danhSachHo });
            chart.AxisY.Add(new Axis { Title = "Số Khối", LabelFormatter = value => value.ToString("N0") });

            PnlBieuDoNuoc.Controls.Clear();
            PnlBieuDoNuoc.Controls.Add(chart);
        }
        private void VeBieuDoLichSuDien(List<string> danhSachThangNam, ChartValues<double> soLieuDien)
        {
            LiveCharts.WinForms.CartesianChart chart = new LiveCharts.WinForms.CartesianChart();
            chart.Dock = DockStyle.Fill;

            chart.Series = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Điện tiêu thụ (kWh)",
                    Values = soLieuDien,
                    DataLabels = true
                }
            };

            chart.AxisX.Add(new Axis { Title = "Thời gian (Tháng/Năm)", Labels = danhSachThangNam });
            chart.AxisY.Add(new Axis { Title = "Số kWh", LabelFormatter = value => value.ToString("N0") });

            PnlThongKeDienHoDan.Controls.Clear();
            PnlThongKeDienHoDan.Controls.Add(chart);
        }

        private void VeBieuDoLichSuNuoc(List<string> danhSachThangNam, ChartValues<double> soLieuNuoc)
        {
            LiveCharts.WinForms.CartesianChart chart = new LiveCharts.WinForms.CartesianChart();
            chart.Dock = DockStyle.Fill;

            chart.Series = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Nước tiêu thụ (Khối)",
                    Values = soLieuNuoc,
                    DataLabels = true
                }
            };

            chart.AxisX.Add(new Axis { Title = "Thời gian (Tháng/Năm)", Labels = danhSachThangNam });
            chart.AxisY.Add(new Axis { Title = "Số Khối", LabelFormatter = value => value.ToString("N0") });

            PnlThongKeNuocHoDan.Controls.Clear();
            PnlThongKeNuocHoDan.Controls.Add(chart);
        }

        private void BtnLoc_Click(object sender, EventArgs e)
        {
            if (CbThang.SelectedItem == null || CbNam.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn Tháng và Năm để xem thống kê!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int thang = Convert.ToInt32(CbThang.Text);
                int nam = Convert.ToInt32(CbNam.Text);

                DataTable dtThongKe = bllHoaDon.ThongKeTheoThang(thang, nam);

                if (dtThongKe == null || dtThongKe.Rows.Count == 0)
                {
                    MessageBox.Show($"Không có dữ liệu hóa đơn nào được lưu trong Tháng {thang}/{nam}!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    PnlBieuDoDien.Controls.Clear();
                    PnlBieuDoNuoc.Controls.Clear();
                    return;
                }
                List<string> dsHoDan = new List<string>();
                ChartValues<double> dsDien = new ChartValues<double>();
                ChartValues<double> dsNuoc = new ChartValues<double>();

                foreach (DataRow row in dtThongKe.Rows)
                {
                    dsHoDan.Add(row["MaHoDan"].ToString());
                    dsDien.Add(Convert.ToDouble(row["TongSoDien"]));
                    dsNuoc.Add(Convert.ToDouble(row["TongSoNuoc"]));
                }

                VeBieuDoDien(dsHoDan, dsDien);
                VeBieuDoNuoc(dsHoDan, dsNuoc);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi vẽ biểu đồ thống kê: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnThongKe_Click(object sender, EventArgs e)
        {
            pnlThongKe.Visible = true;
            pnlThongKe.BringToFront();
        }

        private void BtnLocHoDan_Click(object sender, EventArgs e)
        {
            if (CbMaHoDan.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn Mã hộ dân để xem lịch sử thống kê!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string maHo = CbMaHoDan.Text;
                DataTable dtLichSu = bllHoaDon.LayLichSuTieuThuHoDan(maHo);

                if (dtLichSu == null || dtLichSu.Rows.Count == 0)
                {
                    MessageBox.Show($"Hộ dân {maHo} này chưa từng phát sinh hóa đơn nào trong hệ thống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    PnlThongKeDienHoDan.Controls.Clear();
                    PnlThongKeNuocHoDan.Controls.Clear();
                    return;
                }

                List<string> dsThangNam = new List<string>();
                ChartValues<double> dsDien = new ChartValues<double>();
                ChartValues<double> dsNuoc = new ChartValues<double>();

                foreach (DataRow row in dtLichSu.Rows)
                {
                    string thoiGian = $"T{row["Thang"]}/{row["Nam"]}";
                    dsThangNam.Add(thoiGian);

                    dsDien.Add(Convert.ToDouble(row["TongSoDien"]));
                    dsNuoc.Add(Convert.ToDouble(row["TongSoNuoc"]));
                }

                VeBieuDoLichSuDien(dsThangNam, dsDien);
                VeBieuDoLichSuNuoc(dsThangNam, dsNuoc);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải biểu đồ lịch sử: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnBackQuanLyTaiKhoam_Click(object sender, EventArgs e)
        {
            pnlQuanLyTaiKhoan.Visible = true;
            pnlSuaTaiKhoan.Visible = false;
            pnlQuanLyTaiKhoan.BringToFront();
        }

        private void BtnBackPnlQuanLyTaiKhoan_Click(object sender, EventArgs e)
        {
            pnlQuanLyTaiKhoan.Visible = true;
            pnlThemTaiKhoan.Visible = false;
            pnlQuanLyTaiKhoan.BringToFront();
        }

        private void BtnBackBangGia_Click(object sender, EventArgs e)
        {
            pnlThemBangGiaMoi.Visible = false;
            pnlBangGia.Visible = true;
            pnlBangGia.BringToFront();
        }
        private void LamMoiGiaoDienTinhTien()
        {
            CbbNam.SelectedIndexChanged -= CbbNam_SelectedIndexChanged;

            CbbMaHoDan.SelectedIndex = -1;
            CbbThang.SelectedIndex = -1;
            CbbNam.SelectedIndex = -1;

            TxtTTTenChuHo.Text = "";
            TxtMaBangGia.Text = "";
            TxtSoDien.Text = "";
            TxtSoNuoc.Text = "";
            TxtThanhTienDien.Text = "";
            TxtThanhTienNuoc.Text = "";
            TxtTongThu.Text = "";
            TxtPhanTramThue.Text = "";
            CbbTrangThai.SelectedIndex = -1;

            BtnLuu.Text = "Lưu";
            BtnLuu.Enabled = false;
            BtnXemChiTiet.Enabled = false;
            BtnXuatHoaDon.Enabled = false;
            if (BtnUpdateTinhTien != null) BtnUpdateTinhTien.Enabled = false;

            if (lblTrangThaiTinhTien != null)
            {
                lblTrangThaiTinhTien.Text = "Vui lòng chọn Mã hộ dân, Tháng và Năm";
                lblTrangThaiTinhTien.ForeColor = Color.Black;
            }

            CbbNam.SelectedIndexChanged += CbbNam_SelectedIndexChanged;
        }
    }
}
