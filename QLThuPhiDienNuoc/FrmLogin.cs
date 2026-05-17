using QLThuPhiDienNuoc.BLL;
using QLThuPhiDienNuoc.DTO;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;

namespace QLThuPhiDienNuoc
{
    public partial class FrmLogin : Form
    {
        TaiKhoanBLL bll = new TaiKhoanBLL();
        public FrmLogin()
        {
            InitializeComponent();
        }

        private void BtnDangNhap_Click(object sender, EventArgs e)
        {
            string sdt = TxtSDT.Text.Trim();
            string matKhau = TxtMatKhau.Text.Trim();

            if (string.IsNullOrEmpty(sdt) || string.IsNullOrEmpty(matKhau))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Số điện thoại và Mật khẩu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                DataTable dt = bll.Login(sdt, matKhau);

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    TaiKhoanDTO tkLogin = new TaiKhoanDTO()
                    {
                        SoDienThoai = sdt,
                        MatKhau = matKhau,
                        HoTen = row["HoTen"].ToString(),
                        VaiTro = row["VaiTro"].ToString(),

                        DiaChi = row["DiaChi"].ToString(),
                        NgaySinh = row["NgaySinh"] != DBNull.Value ? Convert.ToDateTime(row["NgaySinh"]) : DateTime.Now

                    };


                    MessageBox.Show($"Đăng nhập thành công! Xin chào {tkLogin.HoTen}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (tkLogin.VaiTro == "Admin" || tkLogin.VaiTro == "Staff")
                    {
                        var admin = new FrmAdmin(tkLogin);
                        admin.FormClosed += (s, arg) => this.Close();
                        admin.Show();
                    }
                    else if (tkLogin.VaiTro == "Client")
                    {
                    }

                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Số điện thoại hoặc mật khẩu không đúng, hoặc tài khoản đã bị khóa!", "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hệ thống: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
 
        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TxtDKSDT.Text) || string.IsNullOrEmpty(TxtPassword.Text) ||
                string.IsNullOrEmpty(TxtHoTen.Text) || string.IsNullOrEmpty(TxtMaHoDan.Text))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin để đăng ký!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            TaiKhoanDTO tkMoi = new TaiKhoanDTO
            {
                SoDienThoai = TxtDKSDT.Text.Trim(),
                MatKhau = TxtPassword.Text.Trim(),
                HoTen = TxtHoTen.Text.Trim(),
                NgaySinh = DtpNgaySinh.Value,
                DiaChi = TxtAddress.Text.Trim(),
                MaHoDan = TxtMaHoDan.Text.Trim(),

            };
            try
            {
                bll.RegisterClient(tkMoi);
                MessageBox.Show("Đăng ký thành công! Bạn có thể đăng nhập ngay bây giờ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                pnlDangKy.Visible = false;
                pnlDangNhap.Visible = true;
                pnlDangNhap.BringToFront();
            }
            catch(SqlException sqlEx)
            {
                if(sqlEx.Number == 547)
                    MessageBox.Show("Mã hộ dân không tồn tại trên hệ thống. Vui lòng kiểm tra lại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if(sqlEx.Number== 2627 || sqlEx.Number == 2601)
                {
                    if(sqlEx.Message.Contains("UQ_TaiKhoan_MaHoDan"))
                        MessageBox.Show("Mã hộ dân đã có tài khoản đăng ký. Vui lòng kiểm tra lại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                        MessageBox.Show("Số điện thoại đã được sử dụng. Vui lòng chọn số khác!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                    MessageBox.Show("Lỗi hệ thống: " + sqlEx.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void TxtMatKhau_TextChanged(object sender, EventArgs e)
        {

        }

        private void pnlDangNhap_Paint(object sender, PaintEventArgs e)
        {

        }

        private void TxtSDT_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void pnlDangKy_Paint(object sender, PaintEventArgs e)
        {

        }
        private void BtnDangKy_Click(object sender, EventArgs e)
        {
            pnlDangNhap.Visible = false;
            pnlDangKy.Visible = true;
            pnlDangKy.BringToFront();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
