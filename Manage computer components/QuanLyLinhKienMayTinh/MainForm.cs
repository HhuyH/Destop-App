using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using static QuanLyLinhKienDIenTu.Login;
using System.IO;

namespace QuanLyLinhKienDIenTu
{
    public partial class MainForm : Form
    {
        public static MainForm Instance { get; set; }
        //Label_title
        DanhSachKhachHang danhSachKhachHang;
        DanhSachNhanVien danhSachNhanVien;
        PhongBan phongban;
        SanPham sanpham;
        public Label TitleLabel
        {
            // Phương thức Get của thuộc tính
            get { return label_title; }
            // Phương thức Set của thuộc tính
            set { label_title = value; }
        }

        bool isLogoutRequested = false;

        string strCon = Connecting.GetConnectionString();
        SqlConnection con;
        //tạo biến di chuyển cho panel
        private bool isDragging = false;
        private int xOffset = 0;
        private int yOffset = 0;
        bool isMaximized = false;
        int userId = UserSession.UserId;
        string role = UserSession.role;
        public MainForm()
        {
            InitializeComponent();
            BackGroundColor();
            Profile_picture(userId);
            MaxMiClo();
            Instance = this;
            panel_form.MouseDown += Panel_form_MouseDown;
            panel_form.MouseMove += Panel_form_MouseMove;
            panel_form.MouseUp += Panel_form_MouseUp;
            mdiProp();
        }
        //không biết là gì có dính tới bên file mdiProperties
        private void mdiProp()
        {
            this.SetBevel(false);
            Controls.OfType<MdiClient>().FirstOrDefault().BackColor = Color.FromArgb(232, 234, 237);
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

        //bỏ
        private void BackGroundColor()
        {
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            con = new SqlConnection(strCon);
            GetProfile();
            if (role == "Employee")
            {
                flowLayoutPanelEmp.Visible = false;
            }
        }

        private Form currentFormChild;
        private Form currentFormChildChild;
        // Chèn form con thành form chính
        public void OpenChildForm(Form childForm)
        {
            if (currentFormChild != null)
            {
                currentFormChild.Close();
            }
            if (currentFormChildChild != null)
            {
                currentFormChildChild.Close();
            }
            currentFormChild = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panel_body.Controls.Add(childForm);
            panel_body.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }


        // Chèn form con của form con
        public void OpenChildChildForm(Form childForm)
        {
            if (currentFormChildChild != null)
            {
                currentFormChildChild.Close();
            }
            currentFormChildChild = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Right;
            panel_body.Controls.Add(childForm);
            panel_body.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        //Láy danh sách khách hàng
        public void DanhSachKhachHang()
        {
            OpenChildForm(new DanhSachKhachHang(this));
            label_title.Text = btnKhachHang.Text;
        }

        //Nút hiện danh sách khách hàng
        private void btnKhachHang_Click(object sender, EventArgs e)
        {
            DanhSachKhachHang();
            CloseChildChildForm.CloseFormSalary = true;
            CloseChildChildForm.CloseFormMoney = true;
            if(danhSachKhachHang == null)
            {
                danhSachKhachHang = new DanhSachKhachHang(this);
                danhSachKhachHang.FormClosed += DanhSachKhachHang_FormClosed;
                danhSachKhachHang.MdiParent = this;
                danhSachKhachHang.Show();
            }
            else
            {
                danhSachKhachHang.Activate();
            }
        }

        private void DanhSachKhachHang_FormClosed(object sender, FormClosedEventArgs e)
        {
            danhSachKhachHang = null;
        }

        private void btnEmployee_Click(object sender, EventArgs e)
        {
            menuTranstation2.Start();
            OpenChildForm(new DanhSachNhanVien(this));
            label_title.Text = btnEmployee.Text;
            if (danhSachNhanVien == null)
            {
                danhSachNhanVien = new DanhSachNhanVien(this);
                danhSachNhanVien.FormClosed += DanhSachNhanVien_FormClosed;
                danhSachNhanVien.MdiParent = this;
                danhSachNhanVien.Show();
            }
            else
            {
                danhSachNhanVien.Activate();
            }

            CloseChildChildForm.CloseFormSalary = true;
            CloseChildChildForm.CloseFormMoney = true;
        }

        private void DanhSachNhanVien_FormClosed(object sender, FormClosedEventArgs e)
        {
            danhSachNhanVien = null;
        }



        //Màu Panel giống màu form
        public Color GetPanelBodyColor()
        {
            return panel_body.BackColor;
        }

        //Bỏ
        private void label_title_Click(object sender, EventArgs e)
        {

        }

        //lấy ảnh profile tương ứng với userID
        private void Profile_picture(int userId)
        {
            pictureBox1.Image = Image.FromFile(@"H:\DaiHoc\QuanLyLinhKienMayTinh\Image\user.png");
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            // Lấy thông tin từ bảng Customers và bảng Employees
            using (SqlConnection conn = new SqlConnection(strCon))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(
                    "SELECT c.gender AS gender, c.profile_image AS profile_image " +
                    "FROM Customers c " +
                    "INNER JOIN Users u ON c.user_id = u.user_id " +
                    "WHERE u.user_id = @userId " +
                    "UNION " +
                    "SELECT e.gender AS gender, e.profile_image AS profile_image " +
                    "FROM Employees e " +
                    "INNER JOIN Users u ON e.user_id = u.user_id " +
                    "WHERE u.user_id = @userId", conn))
                {
                    cmd.Parameters.AddWithValue("@userId", userId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string gender = reader["gender"].ToString();
                            byte[] profileImageBytes = reader["profile_image"] as byte[];

                            if (gender == "Nam")
                            {
                                pictureBox1.Image = Image.FromFile(@"H:\DaiHoc\QuanLyLinhKienMayTinh\Image\boy.png");// Hình ảnh mặc định cho Nam
                            }
                            else if (gender == "Nữ")
                            {
                                pictureBox1.Image = Image.FromFile(@"H:\DaiHoc\QuanLyLinhKienMayTinh\Image\woman.png"); // Hình ảnh mặc định cho Nữ
                            }
                            else
                            {
                                pictureBox1.Image = Image.FromFile(@"H:\DaiHoc\QuanLyLinhKienMayTinh\Image\user.png");
                            }
                            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

                            if (profileImageBytes != null)
                            {
                                // Hiển thị hình ảnh từ dữ liệu
                                using (MemoryStream ms = new MemoryStream(profileImageBytes))
                                {
                                    pictureBox1.Image = Image.FromStream(ms);
                                }
                            }
                        }
                    }
                }
            }
        }

        public void UpdateProfilePicture(int userId)
        {
            Profile_picture(userId);
        }

        // Lấy vai trò của user từ user id
        private string GetUserRole(int userId)
        {
            string role = "";

            // Thực hiện truy vấn để lấy vai trò từ cơ sở dữ liệu dựa trên userId
            string query = "SELECT role FROM Users WHERE user_id = @userId";

            using (SqlConnection connection = new SqlConnection(strCon))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@userId", userId);

                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        role = result.ToString();
                    }
                }
            }

            return role;
        }

        // Mở profile
        private void GetProfile()
        {
            string role = GetUserRole(UserSession.UserId);

            // Đặt tên tương ứng cho label_title dựa vào vai trò
            if (role == "Admin")
            {
                label_title.Text = "Hồ sơ quản trị viên";
            }
            else if(role == "Manager")
            {
                label_title.Text = "Hồ sơ quản lý";
            }
            else if (role == "Employee")
            {
                label_title.Text = "Hồ sơ nhân viên";
            }
            else if (role == "Customer")
            {
                label_title.Text = "Hồ sơ khách hàng";
                flowLayoutPanelEmp.Visible = false;
                panelCus.Visible = false;
            }

            // Mở form Profile
            OpenChildForm(new Profile(this));
        }

        //Click vào ảnh thì hiện thị hồ sơ của nhân viên đó
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            GetProfile();
            EmployeeID.userId = 0;
            CloseChildChildForm.CloseFormSalary = true;
            CloseChildChildForm.CloseFormMoney = true;
        }

        private void panel_body_Paint(object sender, PaintEventArgs e)
        {

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

        //Nut dong titlebar
        private void button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //Nut phong to mang hinh
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

        //nut thu nho mang hinh
        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            this.FormBorderStyle = FormBorderStyle.None;
        }

        private void panel_left_Paint(object sender, PaintEventArgs e)
        {

        }

        //nut thoat dang xuat
        private void btnLogOut_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", "Xác nhận đăng xuất", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                isLogoutRequested = true;
                UserSession.UserId = -1;

                // Mở form đăng nhập
                Login Log = new Login();
                Log.Show();

                // Đóng Profile form
                this.Close();
            }
        }

        //Lệnh xác nhận đống App
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isLogoutRequested == false)
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
                    // Đóng kết nối CSDL nếu đang mở
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                }
            }
        }

        bool menuExpend = false;

        bool menuExpend2 = false;
        //Menu có menu con cho 2 button
        private void menuTranslation2_Tick(object sender, EventArgs e)
        {
            if (menuExpend2 == false)
            {
                flowLayoutPanelEmp.Height += 10;
                if (flowLayoutPanelEmp.Height >= 80)
                {
                    menuTranstation2.Stop();
                    menuExpend2 = true;
                }
            }
            else
            {
                flowLayoutPanelEmp.Height -= 10;
                if (flowLayoutPanelEmp.Height <= 40)
                {
                    menuTranstation2.Stop();
                    menuExpend2 = false;
                }
            }
        }

        private bool SidebarExpanded = true;
        private void btnMore_Click(object sender, EventArgs e)
        {
            sidebarTranstation.Start();
        }

        //Điều chỉnh thu gọn cho sidebar
        private void sidebarTranstation_Tick(object sender, EventArgs e)
        {
            if (!SidebarExpanded)
            {
                // Điều chỉnh chiều cao của pictureBox1 hoặc 150
                panel_left.Width += 10;
                pictureBox1.Height += 7;
                if (panel_left.Width >= 200 && pictureBox1.Height >= 150)
                {
                    SidebarExpanded = true;
                    sidebarTranstation.Stop();
                }
            }
            else
            {
                // Điều chỉnh chiều cao của pictureBox1 thành 40
                panel_left.Width -= 10;
                pictureBox1.Height -= 7;
                if (panel_left.Width <= 50 && pictureBox1.Height <= 40)
                {
                    SidebarExpanded = false;
                    sidebarTranstation.Stop();
                }
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            menuTranstation.Start();
        }

        private void btnRoom_Click(object sender, EventArgs e)
        {
            OpenChildForm(new PhongBan());
            label_title.Text = btnRoom.Text;
            if(phongban == null)
            {
                phongban = new PhongBan();
                phongban.FormClosed += Phongban_FormClosed;
                phongban.MdiParent = this;
                phongban.Dock = DockStyle.Fill;
                phongban.Show();
            }
            else
            {
                phongban.Activate();
            }
        }

        private void Phongban_FormClosed(object sender, FormClosedEventArgs e)
        {
            phongban = null;
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            OpenChildForm(new SanPham(this));
            label_title.Text = btnProduct.Text;
        }

        private void Sanpham_FormClosed(object sender, FormClosedEventArgs e)
        {
            sanpham = null;
        }

        private void button4_Click_2(object sender, EventArgs e)
        {
            if(role == "Customer")
            {
                OpenChildForm(new Order(this));
                label_title.Text = "Đặt hàng";
            }
            else
            {
                OpenChildForm(new ViewOrder(this));
                label_title.Text = btnOrder.Text;
            }
        }

        private void panel_form_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }
    }
}
