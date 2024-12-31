using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static QuanLyLinhKienDIenTu.Login;

namespace QuanLyLinhKienDIenTu
{
    public partial class DanhSachNhanVien : Form
    {
        string strCon = Connecting.GetConnectionString();
        public MainForm mf;
        string role = UserSession.role;

        public DanhSachNhanVien(MainForm mainForm)
        {
            InitializeComponent();
            LoadEmployeeData();
            BackGroundColor();
            mf = mainForm;
            dataGridView1.SelectionChanged += DataGridView1_SelectionChanged;
            dataGridView1.CellDoubleClick += DataGridView1_CellDoubleClick;
            txtSearch.TextChanged += TxtSearch_TextChanged;
            DataGrindview();
            if (role == "Employee")
            {
                panelAdd.Visible = false;
                panel5.Visible = false;
            }
        }

        //Chỉnh datagrindvie
        private void DataGrindview()
        {
            // Ẩn hàng dự thêm mới ở cuối DataGridView
            dataGridView1.AllowUserToAddRows = false;
            // Đặt DataGridView thành chỉ chế độ chỉ đọc
            dataGridView1.ReadOnly = true;
            // ẩn cột userid
            dataGridView1.Columns["user_id"].Visible = false;
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            // Lấy nội dung tìm kiếm từ TextBox
            string keyword = txtSearch.Text.Trim();

            // Gọi phương thức SearchItems với từ khóa tìm kiếm
            Search(keyword);
        }

        //hàm tìm kiếm
        private void Search(string keyword)
        {
            // Tạo câu truy vấn SQL tìm kiếm nhân viên
            string query = "SELECT employee_id, user_id, full_name, gender, email, phone_number, address, hire_date, contract_type FROM Employees " +
                            "WHERE employee_id LIKE @Keyword OR full_name LIKE @Keyword OR gender LIKE @Keyword OR email LIKE @Keyword " +
                            "OR phone_number LIKE @Keyword OR address LIKE @Keyword OR hire_date LIKE @Keyword OR contract_type LIKE @Keyword";

            // Tạo kết nối đến cơ sở dữ liệu
            using (SqlConnection connection = new SqlConnection(strCon))
            {
                // Mở kết nối
                connection.Open();

                // Tạo đối tượng SqlCommand
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Thêm tham số cho từ khóa tìm kiếm
                    command.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");

                    // Tạo đối tượng SqlDataAdapter để lấy dữ liệu từ SQL Server và đổ vào DataTable
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        // Khởi tạo DataTable để lưu trữ dữ liệu
                        DataTable dt = new DataTable();

                        // Đổ dữ liệu từ SqlDataAdapter vào DataTable
                        adapter.Fill(dt);

                        // Đặt DataTable làm nguồn dữ liệu cho DataGridView
                        dataGridView1.DataSource = dt;
                    }
                }
            }
        }

        //tải danh sách khách hàng
        private void LoadEmployeeData()
        {
            // Sử dụng câu lệnh using để đảm bảo rằng SqlConnection được giải phóng đúng cách ngay cả khi có ngoại lệ xảy ra
            using (SqlConnection con = new SqlConnection(strCon))
            {
                try
                {
                    // Mở kết nối
                    con.Open();

                    // Câu truy vấn SQL để lấy dữ liệu từ bảng Employees
                    string query = "SELECT employee_id, user_id, full_name, date_of_birth, gender, email, phone_number, address, hire_date, contract_type FROM Employees";

                    // Tạo đối tượng SqlCommand
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        // Tạo đối tượng SqlDataAdapter
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            // Khởi tạo DataTable để lưu trữ dữ liệu
                            DataTable dt = new DataTable();

                            // Đổ dữ liệu từ SqlDataAdapter vào DataTable
                            adapter.Fill(dt);

                            // Đặt DataTable làm nguồn dữ liệu cho DataGridView
                            dataGridView1.DataSource = dt;

                            // Cấu hình các cột
                            dataGridView1.Columns["employee_id"].HeaderText = "ID";
                            dataGridView1.Columns["full_name"].HeaderText = "Họ và tên";
                            dataGridView1.Columns["date_of_birth"].HeaderText = "Ngày sinh";
                            dataGridView1.Columns["gender"].HeaderText = "Giới tính";
                            dataGridView1.Columns["email"].HeaderText = "Email";
                            dataGridView1.Columns["phone_number"].HeaderText = "Số điện thoại";
                            dataGridView1.Columns["address"].HeaderText = "Địa chỉ";
                            dataGridView1.Columns["hire_date"].HeaderText = "Ngày nhận việc";
                            dataGridView1.Columns["contract_type"].HeaderText = "Loại hợp đồng";

                            // Căn giữa các cột
                            foreach (DataGridViewColumn column in dataGridView1.Columns)
                            {
                                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            }

                            dataGridView1.Columns["employee_id"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                            dataGridView1.Columns["user_id"].Visible = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Đã xảy ra lỗi: " + ex.Message);
                }
            }
        }

        int userId;
        //lay userId duoc chon
        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            // Kiểm tra nếu ô tìm kiếm trống
            if (string.IsNullOrEmpty(txtSearch.Text))
            {
                // Kiểm tra xem có hàng được chọn không
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    // Lấy dòng được chọn đầu tiên
                    DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                    // Kiểm tra xem cột "user_id" có tồn tại trong DataGridView không
                    if (dataGridView1.Columns.Contains("user_id") && selectedRow.Cells["user_id"] != null && selectedRow.Cells["user_id"].Value != null)
                    {
                        // Tiến hành lấy giá trị của cột "user_id"
                        userId = Convert.ToInt32(selectedRow.Cells["user_id"].Value);
                    }
                }
            }
        }


        //double click để hiện thi form profile cho customer va chuyen id vao
        public static bool EmployeeInfo = false;
        private void DataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra xem có hàng được chọn không
            if (dataGridView1.SelectedRows.Count > 0)
            {
                EmployeeInfo = true;
                // Lấy dòng được chọn đầu tiên
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                // Tiến hành lấy giá trị của cột "user_id"
                EmployeeID.userId = Convert.ToInt32(selectedRow.Cells["user_id"].Value);
                OpenChildForm(new ProfileForEmp(mf));
            }
        }

        private void OpenChildForm(Form childForm)
        {
            if (mf != null)
            {
                mf.OpenChildForm(childForm); // Gọi phương thức của MainForm từ tham chiếu
            }
        }

        //Hàm xóa khách hàng
        private void XoaNhanVien(int userId)
        {

            // Sử dụng câu lệnh using để đảm bảo rằng SqlConnection được giải phóng đúng cách ngay cả khi có ngoại lệ xảy ra
            using (SqlConnection con = new SqlConnection(strCon))
            {
                try
                {
                    // Mở kết nối
                    con.Open();

                    // Tạo đối tượng SqlCommand và thiết lập loại command là Stored Procedure
                    using (SqlCommand cmd = new SqlCommand("DeleteEmployee", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Thêm tham số vào stored procedure
                        cmd.Parameters.AddWithValue("@user_Id", userId);


                        // Thực thi stored procedure
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Đã xóa nhân viên thành công.");
                            // Cập nhật lại hiển thị trên DataGridView hoặc các điều kiện khác tùy thuộc vào cài đặt của bạn
                        }
                        else
                        {
                            MessageBox.Show("Không có nhân viên nào được xóa.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Đã xảy ra lỗi: " + ex.Message);
                }
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            XoaNhanVien(userId);
            LoadEmployeeData();
        }

        //chỉnh màu nền
        private void BackGroundColor()
        {
            MainForm mf = new MainForm();
            dataGridView1.BackgroundColor = mf.GetPanelBodyColor();
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.RowHeadersVisible = false;
            this.BackColor = mf.GetPanelBodyColor();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            EmployeeInfo = false;
            AddUser A = new AddUser(mf);

            A.SetLabel("Thêm nhân viên");
            A.role = "Employee";
            A.ShowDialog();
        }

        private void DanhSachKhachHang_Load(object sender, EventArgs e)
        {
            this.ControlBox = false;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
             
        }

    }
}
