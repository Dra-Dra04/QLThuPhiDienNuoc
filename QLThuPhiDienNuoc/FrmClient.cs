using LiveCharts;
using LiveCharts.Wpf;
using Org.BouncyCastle.Tls;
using QLThuPhiDienNuoc.BLL;
using QLThuPhiDienNuoc.DAL;
using QLThuPhiDienNuoc.DTO;
using QLThuPhiDienNuoc.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLThuPhiDienNuoc
{
    public partial class FrmClient : Form
    {

        private TaiKhoanDTO taiKhoanHienTai;
        private TaiKhoanBLL bllTaiKhoan = new TaiKhoanBLL();
        private HoaDonBLL bllHoaDon = new HoaDonBLL();
        private BangGiaBLL bllBangGia = new BangGiaBLL();
        public FrmClient(TaiKhoanDTO tkLogin)
        {
            InitializeComponent();
            taiKhoanHienTai = tkLogin;
            if (lblTenNguoiDung != null)
            {
                lblTenNguoiDung.Text = taiKhoanHienTai.HoTen;
            }

        }
        private void FrmClient_Load(object sender, EventArgs e)
        {
            LoadThongTinCaNhan();
            LoadComboBoxThangNam();
            pnlQuanLyTaiKhoan.BringToFront();
            LoadHoaDonClient();
            LoadCbbNamThongKe();
        }
        private void LoadThongTinCaNhan()
        {
            if (taiKhoanHienTai != null)
            {
                TxtMaHD.Text = taiKhoanHienTai.MaHoDan;
                TxtHoTenProfile.Text = taiKhoanHienTai.HoTen;
                TxtSDTProfile.Text = taiKhoanHienTai.SoDienThoai;
                TxtAddressProfile.Text = taiKhoanHienTai.DiaChi;
                TxtPasswordProfile.Text = taiKhoanHienTai.MatKhau;

                if (taiKhoanHienTai.NgaySinh != null && taiKhoanHienTai.NgaySinh >= guna2DateTimePicker4.MinDate)
                {
                    guna2DateTimePicker4.Value = taiKhoanHienTai.NgaySinh;
                }
            }
        }
        private void LoadComboBoxThangNam()
        {
            CbbThang.Items.Clear();
            CbbNam.Items.Clear();

            for (int i = 1; i <= 12; i++)
            {
                CbbThang.Items.Add(i.ToString());
            }

            int namHienTai = DateTime.Now.Year;
            for (int i = namHienTai - 5; i <= namHienTai + 2; i++)
            {
                CbbNam.Items.Add(i.ToString());
            }

            CbbThang.SelectedItem = DateTime.Now.Month.ToString();
            CbbNam.SelectedItem = namHienTai.ToString();
        }

        private void LoadCbbNamThongKe()
        {
            CbbNamThongKe.Items.Clear();
            int namHienTai = DateTime.Now.Year;
            for (int i = namHienTai - 5; i <= namHienTai + 2; i++)
            {
                CbbNamThongKe.Items.Add(i.ToString());
            }
            CbbNamThongKe.SelectedItem = namHienTai.ToString();
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
                    if (soKWhConLai <= 0)
                    {
                        break;
                    }
                    double tuChiSo = Convert.ToDouble(row["TuChiSo"]);
                    double denChiSo = 999999;
                    if (row["DenChiSo"] != DBNull.Value && !string.IsNullOrWhiteSpace(row["DenChiSo"].ToString()))
                    {
                        denChiSo = Convert.ToDouble(row["DenChiSo"]);
                    }
                    decimal donGia = Convert.ToDecimal(row["DonGia"]);
                    double sucChuaCuaBac = denChiSo - tuChiSo + 1;
                    double soLuongTinh = Math.Min(soKWhConLai, sucChuaCuaBac);
                    decimal thanhTien = (decimal)soLuongTinh * donGia;

                    dtChiTietIn.Rows.Add($"Bậc{sttBac}", Math.Round(soLuongTinh, 2), donGia, Math.Round(thanhTien, 0));
                    soKWhConLai -= soLuongTinh;
                    sttBac++;

                }
            }
            return dtChiTietIn;
        }
        private void LamMoiTruongHoaDon()
        {
            TxtSoDien.Text = "";
            TxtSoNuoc.Text = "";
            TxtThanhTienDien.Text = "";
            TxtThanhTienNuoc.Text = "";
            TxtTongThu.Text = "";
            TxtTrangThai.Text = "";
            TxtPhanTramThue.Text = "";

            BtnXemChiTiet.Enabled = false;
            BtnThanhToan.Enabled = false;
        }

        private void LoadHoaDonClient()
        {
            if (CbbThang.SelectedItem == null || CbbNam.SelectedItem == null) return;

            string maHo = taiKhoanHienTai.MaHoDan;
            TxtMaHoDan.Text = maHo;
            TxtTTTenChuHo.Text = taiKhoanHienTai.HoTen;

            int thang = Convert.ToInt32(CbbThang.Text);
            int nam = Convert.ToInt32(CbbNam.Text);

            try
            {
                DataTable dtHoaDon = bllHoaDon.LayHoaDonDaTonTai(maHo, thang, nam);

                if (dtHoaDon != null && dtHoaDon.Rows.Count > 0)
                {
                    DataRow hd = dtHoaDon.Rows[0];

                    TxtSoDien.Text = hd["TongSoDien"].ToString();
                    TxtSoNuoc.Text = hd["TongSoNuoc"].ToString();
                    TxtThanhTienDien.Text = Convert.ToDecimal(hd["TienDien"]).ToString("N0") + " VNĐ";
                    TxtThanhTienNuoc.Text = Convert.ToDecimal(hd["TienNuoc"]).ToString("N0") + " VNĐ";
                    TxtPhanTramThue.Text = hd["PhanTramThue"].ToString() + "%";
                    TxtTongThu.Text = Convert.ToDecimal(hd["TongTien"]).ToString("N0") + " VNĐ";
                    TxtTrangThai.Text = hd["TrangThai"].ToString();

                    BtnXemChiTiet.Enabled = true;

                    if (hd["TrangThai"].ToString() == "Chưa nộp")
                    {
                        BtnThanhToan.Enabled = true;
                        lblTrangThaiTinhTien.Text = "Bạn chưa thanh toán hóa đơn này!";
                        lblTrangThaiTinhTien.ForeColor = Color.Red;
                    }
                    else
                    {
                        BtnThanhToan.Enabled = false;
                        lblTrangThaiTinhTien.Text = " Hóa đơn đã được thanh toán.";
                        lblTrangThaiTinhTien.ForeColor = Color.Green;
                    }
                }
                else
                {
                    LamMoiTruongHoaDon();
                    lblTrangThaiTinhTien.Text = "Tháng này chưa có hóa đơn được chốt.";
                    lblTrangThaiTinhTien.ForeColor = Color.Gray;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải hóa đơn: " + ex.Message);
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

        private void BtnClosed_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn đóng hệ thống?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void BtnTinhTien_Click(object sender, EventArgs e)
        {
            TxtMaHoDan.Text = taiKhoanHienTai.MaHoDan;
            pnlTinhTien.Visible = true;
            pnlTinhTien.BringToFront();
        }

        private void BtnQuanLyTaiKhoan_Click(object sender, EventArgs e)
        {
            pnlQuanLyTaiKhoan.Visible = true;
            pnlQuanLyTaiKhoan.BringToFront();
        }

        private void BtnUpdateProfile_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtHoTenProfile.Text) || string.IsNullOrWhiteSpace(TxtPasswordProfile.Text))
            {
                MessageBox.Show("Họ tên và Mật khẩu không được để trống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                TaiKhoanDTO tkUpdate = new TaiKhoanDTO
                {
                    SoDienThoai = TxtSDTProfile.Text,
                    HoTen = TxtHoTenProfile.Text,
                    NgaySinh = guna2DateTimePicker4.Value,
                    DiaChi = TxtAddressProfile.Text,
                    MatKhau = TxtPasswordProfile.Text
                };

                if (bllTaiKhoan.CapNhatThongTinClient(tkUpdate))
                {
                    MessageBox.Show("Cập nhật thông tin cá nhân thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    taiKhoanHienTai.HoTen = tkUpdate.HoTen;
                    taiKhoanHienTai.NgaySinh = tkUpdate.NgaySinh;
                    taiKhoanHienTai.DiaChi = tkUpdate.DiaChi;
                    taiKhoanHienTai.MatKhau = tkUpdate.MatKhau;
                }
                else
                {
                    MessageBox.Show("Cập nhật thất bại. Vui lòng thử lại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hệ thống: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnReload_Click(object sender, EventArgs e)
        {
            LoadThongTinCaNhan();
        }

        private void BtnThongKe_Click(object sender, EventArgs e)
        {
            pnlThongKe.Visible = true;
            pnlThongKe.BringToFront();
        }

        private void CbbThang_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadHoaDonClient();
        }

        private void TroVe_Click(object sender, EventArgs e)
        {
            pnlTinhTien.Visible = true;
            pnlTinhTien.BringToFront();
        }

        private void BtnXemChiTiet_Click(object sender, EventArgs e)
        {
            if (CbbThang.SelectedItem == null || CbbNam.SelectedItem == null) return;

            string maHo = taiKhoanHienTai.MaHoDan;
            int thang = Convert.ToInt32(CbbThang.Text);
            int nam = Convert.ToInt32(CbbNam.Text);

            try
            {
                DataTable dtHoaDon = bllHoaDon.LayHoaDonDaTonTai(maHo, thang, nam);
                if (dtHoaDon == null && dtHoaDon.Rows.Count == 0) return;

                DataRow dh = dtHoaDon.Rows[0];

                TbBangMa.Text = dh["MaHoaDon"].ToString();
                TbTenChuHo.Text = taiKhoanHienTai.HoTen;
                TbSoDien.Text = dh["TongSoDien"].ToString() + "kWh";
                TbSoNuoc.Text = dh["TongSoNuoc"].ToString() + "Khối";
                TbTongThu.Text = Convert.ToDecimal(dh["TongTien"]).ToString("N0") + " VNĐ";
                TbTongThue.Text = Convert.ToDecimal(dh["ThueGTGT"]).ToString("N0") + " VNĐ";

                DataTable dtBangGia = bllBangGia.LayBangGiaApDung(thang, nam);
                if (dtBangGia != null && dtBangGia.Rows.Count > 0)
                {
                    int maBangGia = Convert.ToInt32(dtBangGia.Rows[0]["ID"]);
                    guna2DataGridView6.DataSource = TaoBangChiTietBacThang(maBangGia, "Dien", Convert.ToDouble(dh["TongSoDien"]));
                    guna2DataGridView5.DataSource = TaoBangChiTietBacThang(maBangGia, "Nuoc", Convert.ToDouble(dh["TongSoNuoc"]));

                    guna2DataGridView6.Columns["Đơn giá"].DefaultCellStyle.Format = "N0";
                    guna2DataGridView6.Columns["Thành tiền"].DefaultCellStyle.Format = "N0";
                    guna2DataGridView5.Columns["Đơn giá"].DefaultCellStyle.Format = "N0";
                    guna2DataGridView5.Columns["Thành tiền"].DefaultCellStyle.Format = "N0";
                }

                pnlTinhTien.Visible = false;
                pnlChiTietHoaDon.Visible = true;
                pnlChiTietHoaDon.BringToFront();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hiển thị chi tiết: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void VeBieuDoThongKeClient(int nam)
        {
            try
            {
                string maHo = taiKhoanHienTai.MaHoDan;

                ChartValues<double> dsDien = new ChartValues<double> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                ChartValues<double> dsNuoc = new ChartValues<double> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                List<string> dsThang = new List<string> { "T1", "T2", "T3", "T4", "T5", "T6", "T7", "T8", "T9", "T10", "T11", "T12" };

                DataTable dtLichSu = bllHoaDon.LayLichSuTieuThuHoDan(maHo);

                if (dtLichSu != null && dtLichSu.Rows.Count > 0)
                {
                    foreach (DataRow row in dtLichSu.Rows)
                    {
                        int rowNam = Convert.ToInt32(row["Nam"]);
                        if (rowNam == nam)
                        {
                            int thang = Convert.ToInt32(row["Thang"]);

                            dsDien[thang - 1] = Convert.ToDouble(row["TongSoDien"]);
                            dsNuoc[thang - 1] = Convert.ToDouble(row["TongSoNuoc"]);
                        }
                    }
                }
                // ================= VẼ BIỂU ĐỒ ĐIỆN =================
                LiveCharts.WinForms.CartesianChart chartDien = new LiveCharts.WinForms.CartesianChart();
                chartDien.Dock = DockStyle.Fill;
                chartDien.Series = new SeriesCollection
                {
                    new ColumnSeries { Title = "Điện tiêu thụ (kWh)", Values = dsDien, DataLabels = true }
                };
                chartDien.AxisX.Add(new Axis { Title = "Tháng", Labels = dsThang });
                chartDien.AxisY.Add(new Axis { Title = "Số kWh", LabelFormatter = value => value.ToString("N0") });

                guna2Panel3.Controls.Clear();
                guna2Panel3.Controls.Add(chartDien);

                // ================= VẼ BIỂU ĐỒ NƯỚC =================
                LiveCharts.WinForms.CartesianChart chartNuoc = new LiveCharts.WinForms.CartesianChart();
                chartNuoc.Dock = DockStyle.Fill;
                chartNuoc.Series = new SeriesCollection
                {
                    new ColumnSeries { Title = "Nước tiêu thụ (Khối)", Values = dsNuoc, DataLabels = true }
                };
                chartNuoc.AxisX.Add(new Axis { Title = "Tháng", Labels = dsThang });
                chartNuoc.AxisY.Add(new Axis { Title = "Số Khối", LabelFormatter = value => value.ToString("N0") });

                guna2Panel2.Controls.Clear();
                guna2Panel2.Controls.Add(chartNuoc);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi vẽ biểu đồ thống kê: " + ex.Message);
            }


        }

        private void CbbNamThongKe_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CbbNamThongKe.SelectedItem != null)
            {
                int nam = Convert.ToInt32(CbbNamThongKe.Text);
                VeBieuDoThongKeClient(nam);
            }
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            pnlThanhToan.Visible = false;
            pnlTinhTien.Visible = true;
            pnlTinhTien.BringToFront();
        }

        private void BtnThanhToan_Click(object sender, EventArgs e)
        {

            TxtMaHDThanhToan.Text = TxtMaHoDan.Text;
            TxtThangThanhToan.Text = CbbThang.Text;
            TxtThanhToanNam.Text = CbbNam.Text;

            TxtTenChuHo.Text = TxtTTTenChuHo.Text;
            TxtSoDienThoai.Text = taiKhoanHienTai.SoDienThoai;
            TxtTongSoDien.Text = TxtSoDien.Text + " kWh";
            TxtTongSoNuoc.Text = TxtSoNuoc.Text + " Khối";


            TxtTongThanhToan.Text = TxtTongThu.Text;

            string soTienChuoi = TxtTongThu.Text.Replace(" VNĐ", "").Replace(",", "").Trim();

            string thongTinGiaoDich = $"Thanh toan dien nuoc T{CbbThang.Text}/{CbbNam.Text}";
            string urlQR = $"https://img.vietqr.io/image/BIDV-1290581326-compact.png?amount={soTienChuoi}&addInfo={thongTinGiaoDich}";

            PicQRCode.Load(urlQR);

            PicQRCode.Visible = true;
            label15.Visible = false;
            BtnHoanThanh.Enabled = false;

            pnlTinhTien.Visible = false;
            pnlThanhToan.Visible = true;
            pnlThanhToan.BringToFront();
        }

        private void PicQRCode_Click(object sender, EventArgs e)
        {
            DialogResult dialog = DialogResult.Yes;

            if (dialog == DialogResult.Yes)
            {
                string maHo = TxtMaHDThanhToan.Text;
                int thang = Convert.ToInt32(TxtThangThanhToan.Text);
                int nam = Convert.ToInt32(TxtThanhToanNam.Text);

                if (bllHoaDon.XacNhanThanhToan(maHo, thang, nam))
                {
                    PicQRCode.Visible = false;

                    label15.Text = "Thanh toán thành công!";
                    label15.ForeColor = Color.Green;
                    label15.Visible = true;

                    BtnHoanThanh.Enabled = true;
                }
            }
        }

        private void BtnHoanThanh_Click(object sender, EventArgs e)
        {
            pnlThanhToan.Visible = false;
            pnlTinhTien.Visible = true;
            LoadHoaDonClient();
        }

        private void BtnXuatHoaDon_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtTongThu.Text) || TxtTongThu.Text == "0 VNĐ")
            {
                MessageBox.Show("Tháng này chưa có hóa đơn để xuất!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "PDF Document (*.pdf)|*.pdf",
                Title = "Lưu hóa đơn Điện Nước",
                FileName = $"HoaDon_T{CbbThang.Text}_{CbbNam.Text}_{TxtMaHoDan.Text}.pdf"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
                    iTextSharp.text.pdf.BaseFont bf = iTextSharp.text.pdf.BaseFont.CreateFont(fontPath, iTextSharp.text.pdf.BaseFont.IDENTITY_H, iTextSharp.text.pdf.BaseFont.EMBEDDED);
                    iTextSharp.text.Font fontTitle = new iTextSharp.text.Font(bf, 16, iTextSharp.text.Font.BOLD);
                    iTextSharp.text.Font fontNormal = new iTextSharp.text.Font(bf, 12, iTextSharp.text.Font.NORMAL);

                    using (FileStream stream = new FileStream(sfd.FileName, FileMode.Create))
                    {
                        iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A5, 30f, 30f, 30f, 30f); // Dùng A5 cho giống biên lai thực tế
                        iTextSharp.text.pdf.PdfWriter.GetInstance(pdfDoc, stream);
                        pdfDoc.Open();

                        pdfDoc.Add(new iTextSharp.text.Paragraph("HÓA ĐƠN TIỀN ĐIỆN NƯỚC", fontTitle) { Alignment = iTextSharp.text.Element.ALIGN_CENTER });
                        pdfDoc.Add(new iTextSharp.text.Paragraph($"Tháng {CbbThang.Text} Năm {CbbNam.Text}\n\n", fontNormal) { Alignment = iTextSharp.text.Element.ALIGN_CENTER });

                        pdfDoc.Add(new iTextSharp.text.Paragraph($"Mã hộ dân: {TxtMaHoDan.Text}", fontNormal));
                        pdfDoc.Add(new iTextSharp.text.Paragraph($"Tên khách hàng: {TxtTTTenChuHo.Text}", fontNormal));
                        pdfDoc.Add(new iTextSharp.text.Paragraph($"Số điện thoại: {taiKhoanHienTai.SoDienThoai}", fontNormal));
                        pdfDoc.Add(new iTextSharp.text.Paragraph("---------------------------------------------------------", fontNormal));

                        pdfDoc.Add(new iTextSharp.text.Paragraph($"Số điện tiêu thụ: {TxtSoDien.Text} kWh", fontNormal));
                        pdfDoc.Add(new iTextSharp.text.Paragraph($"Số nước tiêu thụ: {TxtSoNuoc.Text} Khối", fontNormal));
                        pdfDoc.Add(new iTextSharp.text.Paragraph($"Thuế GTGT áp dụng: {TxtPhanTramThue.Text}", fontNormal));
                        pdfDoc.Add(new iTextSharp.text.Paragraph("---------------------------------------------------------", fontNormal));

                        pdfDoc.Add(new iTextSharp.text.Paragraph($"TỔNG THANH TOÁN: {TxtTongThu.Text}", fontTitle));
                        pdfDoc.Add(new iTextSharp.text.Paragraph($"Trạng thái: {TxtTrangThai.Text}", fontNormal));
                        pdfDoc.Add(new iTextSharp.text.Paragraph($"Ngày xuất biên lai: {DateTime.Now.ToString("dd/MM/yyyy HH:mm")}", fontNormal));

                        pdfDoc.Close();
                        stream.Close();
                    }

                    MessageBox.Show("Xuất hóa đơn PDF thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                    {
                        FileName = sfd.FileName,
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xuất PDF: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
