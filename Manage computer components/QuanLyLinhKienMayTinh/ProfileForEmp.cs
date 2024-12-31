using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static QuanLyLinhKienDIenTu.Login;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.IO;

namespace QuanLyLinhKienDIenTu
{
    public partial class ProfileForEmp : Form
    {
        string strCon = Connecting.GetConnectionString();
        string imagePath;
        public MainForm mf;
        int UserID = UserSession.UserId;
        int userId = AddUser.UserID;
        bool isAddMode = AddUser.IsAddMode;
        bool EmployeeInfo = DanhSachNhanVien.EmployeeInfo;
        int EmpID = EmployeeID.userId;
        string role = UserSession.role;
        public static string UserRole { get; set; }


        public ProfileForEmp(MainForm mainForm)
        {
            InitializeComponent();
            mf = mainForm;
            Round.SetRoundedButton(btnFix);
            Round.SetRoundedButton(btnUpdImage);
            Round.SetSharpCornerPanel(panel_fix);
            Round.SetSharpCornerPanel(panel1);
            txtNumber.KeyPress += TxtNumber_KeyPress;
            txtAge.KeyPress += TxtAge_KeyPress;
        }

        private void TxtAge_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Kiểm tra xem ký tự được nhấn có phải là một số không
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                // Nếu không phải là số, thì không cho phép nhập ký tự này vào TextBox
                e.Handled = true;
            }
        }

        private void TxtNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Kiểm tra xem phím được nhấn có phải là số không
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                // Nếu không phải số, hủy sự kiện KeyPress
                e.Handled = true;
            }
        }

        private void Profile_Load(object sender, EventArgs e)
        {
            this.ControlBox = false;
            LoadContractTypes();
            tmpPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            if (EmployeeInfo)
            {
                DisplayProfile(EmpID);
                HideTextBox();
                EmployeeInfo = false;
                panel1.Visible = false;
                isEditMode = true;
            }
            else if (isAddMode)
            {
                cboRank.SelectedItem = UserRole;
                isAddMode = false;
                FixMode();
                isEditMode = false;
            }
            Profile_picture(UserID);
            if(role == "Employee")
            {
                pPayment.Visible = false;
                pSalary.Visible = false;
                panel_fix.Visible = false;
            }
        }

        private void Profile_picture(int userId)
        {
            tmpPictureBox.Image = Image.FromFile(@"H:\DaiHoc\QuanLyLinhKienMayTinh\Image\user.png");
            tmpPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
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
                                tmpPictureBox.Image = Image.FromFile(@"H:\DaiHoc\QuanLyLinhKienMayTinh\Image\boy.png");// Hình ảnh mặc định cho Nam
                            }
                            else if (gender == "Nữ")
                            {
                                tmpPictureBox.Image = Image.FromFile(@"H:\DaiHoc\QuanLyLinhKienMayTinh\Image\woman.png"); // Hình ảnh mặc định cho Nữ
                            }
                            else
                            {
                                tmpPictureBox.Image = Image.FromFile(@"H:\DaiHoc\QuanLyLinhKienMayTinh\Image\user.png");
                            }
                            tmpPictureBox.SizeMode = PictureBoxSizeMode.Zoom;

                            if (profileImageBytes != null)
                            {
                                // Hiển thị hình ảnh từ dữ liệu
                                using (MemoryStream ms = new MemoryStream(profileImageBytes))
                                {
                                    tmpPictureBox.Image = Image.FromStream(ms);
                                }
                            }
                        }
                    }
                }
            }
        }


        //Load thông tin Contract từ sql
        private void LoadContractTypes()
        {
            // Kết nối đến cơ sở dữ liệu
            using (SqlConnection connection = new SqlConnection(strCon))
            {
                // Mở kết nối
                connection.Open();

                // Tạo câu lệnh truy vấn SQL để lấy danh sách các loại hợp đồng
                string query = "SELECT contract_type FROM Contracts";

                // Tạo đối tượng SqlCommand
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Thực thi truy vấn và lấy dữ liệu
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Đọc dữ liệu từ SqlDataReader và thêm vào ComboBox
                        while (reader.Read())
                        {
                            // Lấy giá trị từ cột contract_type
                            string contractType = reader["contract_type"].ToString();

                            // Thêm giá trị vào ComboBox
                            cboContractType.Items.Add(contractType);
                        }
                    }
                }
            }
        }

        //láy thông tin từ database
        private void DisplayProfile(int userId)
        {
            string query = @"
                            SELECT e.full_name, e.date_of_birth, e.gender, e.phone_number, e.email, e.contract_type, e.address, u.role, e.hire_date 
                            FROM Employees e
                            INNER JOIN Users u ON e.user_id = u.user_id 
                            WHERE e.user_id = @userId
                            UNION
                            SELECT c.full_name, c.date_of_birth, c.gender, c.phone_number, c.email, NULL AS contract_type, c.address, u.role, NULL AS hire_date
                            FROM Customers c
                            INNER JOIN Users u ON c.user_id = u.user_id 
                            WHERE c.user_id = @userId";

            using (SqlConnection connection = new SqlConnection(strCon))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@userId", userId);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        txtName.Text = reader["full_name"].ToString();
                        string gender = reader["gender"].ToString();
                        cboGender.Text = gender;

                        // Xử lý ngày sinh và tuổi
                        if (reader["date_of_birth"] != DBNull.Value)
                        {
                            DateTime dateOfBirth = Convert.ToDateTime(reader["date_of_birth"]);
                            txtAge.Text = CalculateAge(dateOfBirth).ToString();
                        }
                        else
                        {
                            txtAge.Text = "";
                        }

                        txtNumber.Text = reader["phone_number"].ToString();
                        txtEmail.Text = reader["email"].ToString();

                        // Xử lý contract_type
                        if (reader["contract_type"] != DBNull.Value)
                        {
                            cboContractType.Text = reader["contract_type"].ToString();
                        }
                        else
                        {
                            cboContractType.Text = "";
                        }

                        txtAdress.Text = reader["address"].ToString();
                        string role = reader["role"].ToString();
                        cboRank.SelectedItem = role;

                        // Xử lý hire_date và contract_type
                        if (role != "Customer")
                        {
                            if (reader["hire_date"] != DBNull.Value)
                            {
                                DateTime hireDate = Convert.ToDateTime(reader["hire_date"]);
                                txtHireDate.Text = hireDate.ToString("dd/MM/yyyy");
                            }
                            else
                            {
                                txtHireDate.Text = "";
                            }
                        }
                        else
                        {
                            // Ẩn các control không cần thiết cho khách hàng
                            txtHireDate.Visible = false;
                            label8.Visible = false;
                            cboContractType.Visible = false;
                            label6.Visible = false;
                            btnFix.Visible = false;
                        }
                    }
                }
            }
        }

        // Phương thức tính tuổi từ ngày sinh
        private int CalculateAge(DateTime birthDate)
        {
            DateTime currentDate = DateTime.Now;
            int age = currentDate.Year - birthDate.Year;
            if (currentDate < birthDate.AddYears(age))
            {
                age--;
            }
            return age;
        }

        //Ẩn textbox chỉ hiện nội dung
        private void HideTextBox()
        {
            //Xóa viền
            txtName.BorderStyle = BorderStyle.None;
            txtAge.BorderStyle = BorderStyle.None;
            txtNumber.BorderStyle = BorderStyle.None;
            txtEmail.BorderStyle = BorderStyle.None;
            txtAdress.BorderStyle = BorderStyle.None;
            txtHireDate.BorderStyle = BorderStyle.None;

            //Vô hiệu hóa nút
            txtName.Enabled = false;
            txtAge.Enabled = false;
            cboGender.Enabled = false;
            txtNumber.Enabled = false;
            txtEmail.Enabled = false;
            cboContractType.Enabled = false;
            txtAdress.Enabled = false;
            txtHireDate.Enabled = false;
            cboRank.Enabled = false;

            //Đặt nền trong suất
            txtName.BackColor = this.BackColor;
            txtAge.BackColor = this.BackColor;
            cboGender.BackColor = this.BackColor;
            cboRank.BackColor = this.BackColor;
            txtNumber.BackColor = this.BackColor;
            txtEmail.BackColor = this.BackColor;
            cboContractType.BackColor = this.BackColor;
            txtAdress.BackColor = this.BackColor;
            txtHireDate.BackColor = this.BackColor;

        }
        
        //Hiện thị lại Textbox
        private void ShowTextBox()
        {
            //Hiện viền
            txtName.BorderStyle = BorderStyle.FixedSingle;
            txtAge.BorderStyle = BorderStyle.FixedSingle;
            txtNumber.BorderStyle = BorderStyle.FixedSingle;
            txtEmail.BorderStyle = BorderStyle.FixedSingle;
            txtAdress.BorderStyle = BorderStyle.FixedSingle;
            txtHireDate.BorderStyle = BorderStyle.FixedSingle;

            //Vô hiệu hóa nút
            txtName.Enabled = true;
            txtAge.Enabled = true;
            cboGender.Enabled = true;
            txtNumber.Enabled = true;
            txtEmail.Enabled = true;
            cboRank.Enabled = true;
            cboContractType.Enabled = true;
            txtAdress.Enabled = true;
            txtHireDate.Enabled = true;

            // Đặt lại màu nền mặc định
            txtName.BackColor = SystemColors.Window;
            txtAge.BackColor = SystemColors.Window;
            cboGender.BackColor = SystemColors.Window;
            txtNumber.BackColor = SystemColors.Window;
            txtEmail.BackColor = SystemColors.Window;
            cboRank.BackColor = SystemColors.Window;
            cboContractType.BackColor = SystemColors.Window;
            txtAdress.BackColor = SystemColors.Window;
            txtHireDate.BackColor = SystemColors.Window;
        }

        private void txtHireDate_TextChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        bool isEditMode = true;
        private void btnFix_Click(object sender, EventArgs e)
        {
            if (isEditMode)
            {
                FixMode();
            }
            else
            {
                ViewMode();
            }

        }

        //chế độ sữa 
        private void FixMode()
        {
            ShowTextBox();
            isEditMode = false;
            btnFix.Text = "     Lưu";
            panel1.Visible = true;
            txtHireDate.BorderStyle = BorderStyle.None;
            txtHireDate.Enabled = false;
            txtHireDate.BackColor = this.BackColor;
            tmpPictureBox.Visible = true;
            pPayment.Visible = false;
            pSalary.Visible = false;
            if (role == "Manager")
            {
                cboRank.Enabled = false;
            }
            tmpPictureBox.Image = Image.FromFile(@"H:\DaiHoc\QuanLyLinhKienMayTinh\Image\upload.png");
            btnFix.Image = Image.FromFile(@"H:\DaiHoc\QuanLyLinhKienMayTinh\Icon\diskette.png");
        }

        //chế độ xem và lưu
        private void ViewMode()
        {
            UpdateProfileInfo();
            DisplayProfile(EmpID);
            HideTextBox();
            isEditMode = true;
            panel1.Visible = false;
            tmpPictureBox.Visible = false;
            btnFix.Text = "     Sữa";
            pPayment.Visible = true;
            pSalary.Visible = true;
            tmpPictureBox.Image = Image.FromFile(@"H:\DaiHoc\QuanLyLinhKienMayTinh\Image\upload.png");
            btnFix.Image = Image.FromFile(@"H:\DaiHoc\QuanLyLinhKienMayTinh\Icon\service.png");
        }

        //Cập nhập
        private void UpdateProfileInfo()
        {
            int age;
            if (string.IsNullOrEmpty(txtAge.Text))
            {
                age = 100;
            }
            age = int.Parse(txtAge.Text);

            // Tính năm sinh bằng cách trừ tuổi từ năm hiện tại
            int birthYear = DateTime.Now.Year - age;

            // Tạo ngày sinh từ năm sinh tìm được
            DateTime birthDate = new DateTime(birthYear, 1, 1);

            string fullName = txtName.Text;
            string gender = cboGender.SelectedItem != null ? cboGender.SelectedItem.ToString() : "khác"; // Kiểm tra null
            string phoneNumber = txtNumber.Text != null ? txtNumber.Text : null;
            string email = txtEmail.Text;
            string contractType = cboContractType.SelectedItem != null ? cboContractType.SelectedItem.ToString() : "Full-Time"; // Kiểm tra null
            string address = txtAdress.Text;
            string hireDate = txtHireDate.Text;

            // Cập nhật thông tin vào cơ sở dữ liệu
            string query = @"
                UPDATE Employees
                SET full_name = @fullName,
                    date_of_birth = @dateOfBirth,
                    gender = @gender,
                    phone_number = @phoneNumber,
                    email = @email,
                    contract_type = @contractType,
                    address = @address,
                    hire_date = @hireDate";

            if (!string.IsNullOrEmpty(imagePath))
            {
                query += ", profile_image = CONVERT(VARBINARY(MAX), @imageData)";
            }

            query += " WHERE user_id = @userId;";

            query += @"
                UPDATE Users
                SET role = @role
                WHERE user_id = @userId";


            using (SqlConnection connection = new SqlConnection(strCon))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Thêm các tham số
                    command.Parameters.AddWithValue("@fullName", fullName);
                    command.Parameters.AddWithValue("@dateOfBirth", birthDate.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@gender", gender);
                    command.Parameters.AddWithValue("@phoneNumber", phoneNumber);
                    command.Parameters.AddWithValue("@email", email);
                    command.Parameters.AddWithValue("@contractType", contractType);
                    command.Parameters.AddWithValue("@address", address);
                    command.Parameters.AddWithValue("@hireDate", hireDate);
                    command.Parameters.AddWithValue("@userId", EmpID);
                    command.Parameters.AddWithValue("@role", cboRank.SelectedItem.ToString());

                    // Kiểm tra xem có ảnh được chọn không
                    if (!string.IsNullOrEmpty(imagePath))
                    {
                        try
                        {
                            // Đọc dữ liệu từ tệp hình ảnh thành một byte array
                            byte[] imageBytes = File.ReadAllBytes(imagePath);
                            command.Parameters.AddWithValue("@imageData", imageBytes);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Đã xảy ra lỗi khi đọc tệp hình ảnh: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    else
                    {
                        // Nếu không có ảnh được chọn, thì sử dụng giá trị NULL cho cột image_data
                        command.Parameters.AddWithValue("@imageData", DBNull.Value);
                    }

                    // Mở kết nối và thực thi truy vấn
                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        MessageBox.Show("Thông tin hồ sơ đã được cập nhật thành công.", "Cập nhật thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Đã xảy ra lỗi khi cập nhật thông tin hồ sơ: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnUpdImage_Click(object sender, EventArgs e)
        {
            UploadImage();
        }

        private void UploadImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg, *.jpeg, *.png)|*.jpg; *.jpeg; *.png";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                imagePath = openFileDialog.FileName;
                try
                {
                    // Đọc dữ liệu từ tệp hình ảnh thành một byte array
                    byte[] imageBytes = File.ReadAllBytes(imagePath);

                    // Hiển thị hình ảnh trên PictureBox
                    tmpPictureBox.ImageLocation = imagePath;
                    tmpPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                    // Lưu đường dẫn tệp hình ảnh để sử dụng trong UpdateProfileInfo
                    this.imagePath = imagePath;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Đã xảy ra lỗi khi đọc tệp hình ảnh: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void tmpPictureBox_Click(object sender, EventArgs e)
        {
            if (!isEditMode)
            {
                UploadImage();
            }
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel4_Click(object sender, EventArgs e)
        {
            OpenChildChildForm(new Payment());
        }

        private void OpenChildChildForm(Form childForm)
        {
            if (mf != null)
            {
                mf.OpenChildChildForm(childForm); // Gọi phương thức của MainForm từ tham chiếu
            }
        }

        private void txtNumber_TextChanged(object sender, EventArgs e)
        {

        }

        private void pSalary_Click(object sender, EventArgs e)
        {
            OpenChildChildForm(new Salary());
        }

        private void pSalary_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }
    }

}
