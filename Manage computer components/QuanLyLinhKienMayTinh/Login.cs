using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Button = System.Windows.Forms.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;
using System.Data.SqlClient;


namespace QuanLyLinhKienDIenTu
{
    public partial class Login : Form
    {
        string strCon = Connecting.GetConnectionString();
        SqlConnection con;
        bool isMaximized = false;
        bool isLogoutRequested = true;

        //tạo biến di chuyển cho panel
        private bool isDragging = false;
        private int xOffset = 0;
        private int yOffset = 0;

        public Login()
        {
            InitializeComponent();
            txtUsername.Enter += TxtUsername_Enter;
            txtUsername.Leave += TxtUsername_Leave;
            txtPassword.Enter += TxtPassword_Enter;
            txtPassword.Leave += TxtPassword_Leave;
            this.KeyPreview = true;
            this.KeyDown += Login_KeyDown;
            this.Activated += Login_Activated;
            panel_form.MouseDown += Panel_form_MouseDown;
            panel_form.MouseMove += Panel_form_MouseMove;
            panel_form.MouseUp += Panel_form_MouseUp;
            MaxMiClo();
            this.FormClosing += Login_FormClosing;
            Round.SetRoundedButton(btnLogin);
            Round.SetSharpCornerPanel(panel_UI);
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.FlatAppearance.BorderSize = 0;
        }

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isLogoutRequested == true)
            {
                // Hiển thị hộp thoại xác nhận
                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn thoát khỏi ứng dụng không?", "Xác nhận thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                // Kiểm tra xem người dùng đã chọn Yes hay No
                if (result == DialogResult.No)
                {
                    // Nếu người dùng chọn No, hủy sự kiện đóng Form
                    e.Cancel = true;
                }
                else
                {
                    // Kiểm tra xem đối tượng kết nối đã được khởi tạo chưa
                    if (con != null && con.State == ConnectionState.Open)
                    {
                        // Nếu đã được khởi tạo và đang mở, đóng kết nối
                        con.Close();
                    }
                }
            }
        }

        //gắn cờ false khi thả thuột
        private void Panel_form_MouseUp(object sender, MouseEventArgs e)
        {
            // Ngừng di chuyển khi nhả chuột trái
            if (e.Button == MouseButtons.Left)
            {
                isDragging = false;
            }
        }

        //di chuyển panel
        private void Panel_form_MouseMove(object sender, MouseEventArgs e)
        {
            // Di chuyển form khi đang kéo thanh panel
            if (isDragging)
            {
                Point newLocation = this.Location;
                newLocation.X += e.X - xOffset;
                newLocation.Y += e.Y - yOffset;
                this.Location = newLocation;
            }
        }

        //khi bấm vào panel
        private void Panel_form_MouseDown(object sender, MouseEventArgs e)
        {
            // Bắt đầu di chuyển khi nhấp chuột trái vào thanh panel
            if (e.Button == MouseButtons.Left)
            {
                isDragging = true;
                xOffset = e.X;
                yOffset = e.Y;
            }
        }

        //chỉnh màu cho button close minimize maximize
        private void MaxMiClo()
        {
            //chỉnh màu cho giống panel
            btnClose.BackColor = panel_form.BackColor;
            btnMaximize.BackColor = panel_form.BackColor;
            btnMinimize.BackColor = panel_form.BackColor;

            //tắt viền button
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.FlatAppearance.BorderSize = 0;

            btnMaximize.FlatStyle = FlatStyle.Flat;
            btnMaximize.FlatAppearance.BorderSize = 0;

            btnMinimize.FlatStyle = FlatStyle.Flat;
            btnMinimize.FlatAppearance.BorderSize = 0;

        }

        //bỏ focus
        private void Login_Activated(object sender, EventArgs e)
        {
            // Bỏ focus khỏi trường văn bản txtUsername khi form được kích hoạt
            txtUsername.Focus();
            this.ActiveControl = null;
        }

        //event bấm enter để tự động login
        private void Login_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Ngăn chặn sự kiện KeyDown được gửi đến các điều khiển khác trong form
                e.SuppressKeyPress = true;

                // Thực hiện chức năng đăng nhập
                CheckLogin();
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {
            HideTextBoxBorder();
            TxtUsernameText();
            TxtPasswordText();
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.FlatAppearance.BorderSize = 0;
            
        }
        //ẩn viền textbox

        private void HideTextBoxBorder()
        {
            txtUsername.BorderStyle = BorderStyle.None;
            txtPassword.BorderStyle = BorderStyle.None;
        }

        //hiện chữ mờ trên textbox
        private void TxtUsername_Enter(object sender, EventArgs e)
        {
            if (txtUsername.Text == "Username") // "Username" là placeholder của bạn
            {
                txtUsername.Text = "";
                txtUsername.ForeColor = Color.Black; // Đổi màu văn bản về màu đen
            }
        }

        //hiện chữ mờ trên textbox
        private void TxtUsernameText()
        {
            if (string.IsNullOrEmpty(txtUsername.Text))
            {
                txtUsername.ForeColor = SystemColors.GrayText; // Đổi màu văn bản thành màu xám nhạt
                txtUsername.Text = "Username"; // Hiển thị Placeholder nếu TextBox rỗng
            }
        }

        //hiện chữ mờ trên textbox
        private void TxtUsername_Leave(object sender, EventArgs e)
        {
            TxtUsernameText();
        }

        //hiện chữ mờ trên textbox
        private void TxtPassword_Enter(object sender, EventArgs e)
        {
            if (txtPassword.Text == "Password")
            {
                txtPassword.Text = "";
                txtPassword.ForeColor = Color.Black;
            }
        }

        //hiện chữ mờ trên textbox
        private void TxtPasswordText()
        {
            if (string.IsNullOrEmpty(txtPassword.Text))
            {
                txtPassword.ForeColor = SystemColors.GrayText; // Đổi màu văn bản thành màu xám nhạt
                txtPassword.Text = "Password"; // Hiển thị Placeholder nếu TextBox rỗng
            }
        }

        //hiện chữ mờ trên textbox
        private void TxtPassword_Leave(object sender, EventArgs e)
        {
            TxtPasswordText();
        }

        //bỏ
        private void txtUsername_TextChanged(object sender, EventArgs e)
        {
        }

        // Biến tỉnh lưu UserId và role
        public static class UserSession
        {
            public static int UserId { get; set; }
            public static string role { get; set; }
        }

        // Nút login
        private void btnLogin_Click(object sender, EventArgs e)
        {
            //goi hàm kiểm tra nút login
            CheckLogin();
        }

        //hàm kiểm tra login
        private void CheckLogin()
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            // Truy vấn để kiểm tra sự tồn tại của người dùng trong cơ sở dữ liệu
            string query = "SELECT user_id, role FROM Users WHERE username = @username AND password = @password";

            using (SqlConnection connection = new SqlConnection(strCon))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        // Đăng nhập thành công
                        MessageBox.Show("Đăng nhập thành công!");
                        isLogoutRequested = false;
                        // Lấy user_id từ dữ liệu và đặt vào UserSession
                        UserSession.UserId = reader.GetInt32(0);
                        UserSession.role = reader.GetString(1); // Lấy giá trị từ cột "role"
                        // Hiển thị MainForm và chuyển userId qua
                        MainForm mainForm = new MainForm();
                        mainForm.Show();
                        this.Hide();
                    }
                    else
                    {
                        // Tên đăng nhập hoặc mật khẩu không chính xác
                        MessageBox.Show("Tên đăng nhập hoặc mật khẩu không chính xác. Vui lòng thử lại!");
                    }
                }
            }
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            this.FormBorderStyle = FormBorderStyle.None;
        }

        private void btnMaximize_Click(object sender, EventArgs e)
        {
            if (!isMaximized)
            {
                // Phóng to Form
                this.WindowState = FormWindowState.Maximized;
                this.FormBorderStyle = FormBorderStyle.None;
                isMaximized = true;
                // Xóa hình ảnh cũ trước khi gán hình ảnh mới
                btnMaximize.BackgroundImage = null;
                // Đặt hình ảnh cho Button khi phóng to
                btnMaximize.Image = Image.FromFile(@"H:\DaiHoc\QuanLyLinhKienMayTinh\Icon\minimize.png");
            }
            else
            {
                // Khôi phục kích thước Form bình thường
                this.WindowState = FormWindowState.Normal;
                this.FormBorderStyle = FormBorderStyle.None;
                isMaximized = false;
                // Xóa hình ảnh cũ trước khi gán hình ảnh mới
                btnMaximize.BackgroundImage = null;
                // Đặt hình ảnh cho Button khi khôi phục kích thước bình thường
                btnMaximize.Image = Image.FromFile(@"H:\DaiHoc\QuanLyLinhKienMayTinh\Icon\maximise.png");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
