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
    public partial class AddUser : Form
    {
        string strCon = Connecting.GetConnectionString();
        SqlConnection con;
        bool isMaximized = false;
        public string role;
        
        public MainForm mf;
        //tạo biến di chuyển cho panel
        private bool isDragging = false;
        private int xOffset = 0;
        private int yOffset = 0;

        public AddUser(MainForm mainForm)
        {
            InitializeComponent();
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
            mf = mainForm;
            txtPassword.KeyPress += TxtPassword_KeyPress;
        }

        private void Login_Load(object sender, EventArgs e)
        {
            cboRole.SelectedItem = role;


            TxtUsernameText();
            TxtPasswordText();
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.FlatAppearance.BorderSize = 0;
            HideBox();
        }

        //Nút enter
        private void TxtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                // Gọi sự kiện Click của nút Login
                btnLogin.PerformClick();
            }
        }

        //Tạo tài khoản mới và lấy ID
        public static int UserID { get; set; }

        bool SuccReg = false;
        //thêm tài khoản cho customer
        private void AddUserCus()
        {
            // Kiểm tra dữ liệu đã nhập
            if (string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show("Vui lòng nhập tên người dùng và mật khẩu.");
                return;
            }

            // Tạo chuỗi truy vấn SQL để kiểm tra xem tên người dùng đã tồn tại chưa
            string checkQuery = "SELECT COUNT(*) FROM Users WHERE username = @username";

            using (SqlConnection connection = new SqlConnection(strCon))
            {
                // Mở kết nối
                connection.Open();

                // Tạo và cấu hình đối tượng Command để kiểm tra tên người dùng
                using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                {
                    // Thêm tham số cho truy vấn kiểm tra
                    checkCommand.Parameters.AddWithValue("@username", txtUsername.Text);

                    // Kiểm tra xem tên người dùng đã tồn tại chưa
                    int existingUserCount = (int)checkCommand.ExecuteScalar();

                    // Nếu tên người dùng đã tồn tại, hiển thị thông báo và không tiếp tục thêm mới
                    if (existingUserCount > 0)
                    {
                        MessageBox.Show("Tài khoản đã tồn tại. Vui lòng chọn một tên người dùng khác.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        SuccReg = false;
                        return;
                    }
                }

                // Nếu tên người dùng chưa tồn tại, tiếp tục thêm mới vào cơ sở dữ liệu
                string insertQuery = "INSERT INTO Users (username, password, role) VALUES (@username, @password, @role); SELECT SCOPE_IDENTITY();";

                // Tạo và cấu hình đối tượng Command để thêm mới người dùng
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    // Thêm các tham số cho truy vấn thêm mới
                    command.Parameters.AddWithValue("@username", txtUsername.Text);
                    command.Parameters.AddWithValue("@password", txtPassword.Text);
                    command.Parameters.AddWithValue("@role", role);

                    // Thực thi truy vấn thêm mới và lấy user_id của người dùng mới thêm vào
                    UserID = Convert.ToInt32(command.ExecuteScalar());

                    MessageBox.Show("Tài khoản đã được tạo thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    SuccReg = true;
                }
            }
        }

        //thêm tài khoản cho customer
        private void AddUserEmp()
        {
            // Kiểm tra dữ liệu đã nhập
            if (string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show("Vui lòng nhập tên người dùng và mật khẩu.");
                return;
            }

            // Tạo chuỗi truy vấn SQL để kiểm tra xem tên người dùng đã tồn tại chưa
            string checkQuery = "SELECT COUNT(*) FROM Users WHERE username = @username";

            using (SqlConnection connection = new SqlConnection(strCon))
            {
                // Mở kết nối
                connection.Open();

                // Tạo và cấu hình đối tượng Command để kiểm tra tên người dùng
                using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                {
                    // Thêm tham số cho truy vấn kiểm tra
                    checkCommand.Parameters.AddWithValue("@username", txtUsername.Text);

                    // Kiểm tra xem tên người dùng đã tồn tại chưa
                    int existingUserCount = (int)checkCommand.ExecuteScalar();

                    // Nếu tên người dùng đã tồn tại, hiển thị thông báo và không tiếp tục thêm mới
                    if (existingUserCount > 0)
                    {
                        MessageBox.Show("Tài khoản đã tồn tại. Vui lòng chọn một tên người dùng khác.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        SuccReg = false;
                        return;
                    }
                }

                // Nếu tên người dùng chưa tồn tại, tiếp tục thêm mới vào cơ sở dữ liệu
                string insertQuery = "INSERT INTO Users (username, password, role) VALUES (@username, @password, @role); SELECT SCOPE_IDENTITY();";

                // Tạo và cấu hình đối tượng Command để thêm mới người dùng
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    // Thêm các tham số cho truy vấn thêm mới
                    command.Parameters.AddWithValue("@username", txtUsername.Text);
                    command.Parameters.AddWithValue("@password", txtPassword.Text);
                    command.Parameters.AddWithValue("@role", cboRole.Text);

                    // Thực thi truy vấn thêm mới và lấy user_id của người dùng mới thêm vào
                    UserID = Convert.ToInt32(command.ExecuteScalar());

                    MessageBox.Show("Tài khoản đã được tạo thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    SuccReg = true;
                }
            }
        }

        public static bool IsAddMode = false;
        // Nút Tạo tài khoản
        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (role == "Customer")
            {
                AddUserCus();
                CustomerID.userId = UserID;
                if (SuccReg == true)
                {
                    // Truy cập MainForm thông qua tham chiếu mf
                    if (mf != null)
                    {
                        // Gán giá trị mới cho thuộc tính label_title của MainForm
                        mf.TitleLabel.Text = "Thêm Khách Hàng";
                        IsAddMode = true;
                        this.Close();
                    }
                    OpenChildForm(new ProfileForCus(mf));
                }

            }
            else
            {
                AddUserEmp();
                EmployeeID.userId = UserID;
                if (SuccReg == true)
                {
                    // Truy cập MainForm thông qua tham chiếu mf
                    if (mf != null)
                    {
                        // Gán giá trị mới cho thuộc tính label_title của MainForm
                        mf.TitleLabel.Text = "Thêm nhân viên";
                        IsAddMode = true;
                        this.Close();
                    }
                    // Gán giá trị của cboRole cho UserRole, nếu cboRole.SelectedItem là null thì sử dụng giá trị từ role
                    //?? (null-coalescing operator) cho phép bạn gán giá trị của role cho UserRole khi cboRole.SelectedItem là null. Nếu cboRole.SelectedItem không phải là null
                    ProfileForEmp.UserRole = cboRole.SelectedItem?.ToString() ?? role;
                    OpenChildForm(new ProfileForEmp(mf));
                }
            }

        }

        private void HideBox()
        {
            if (role == "Customer")
            {
                cboRole.Visible = false;
            }
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

        //chuyền label phù hợp
        public void SetLabel(string text)
        {
            labelAdd.Text = text;
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

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {

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

        }

        //bỏ
        private void txtUsername_TextChanged(object sender, EventArgs e)
        {
        }

        private void OpenChildForm(Form childForm)
        {
            if (mf != null)
            {
                mf.OpenChildForm(childForm); // Gọi phương thức của MainForm từ tham chiếu
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
                btnMaximize.Image = Image.FromFile(@"D:\Code\SQL\QuanLyLinhKienMayTinh\Icon\minimize.png");
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
                btnMaximize.Image = Image.FromFile(@"D:\Code\SQL\QuanLyLinhKienMayTinh\Icon\maximise.png");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cboRole_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void panel_UI_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
