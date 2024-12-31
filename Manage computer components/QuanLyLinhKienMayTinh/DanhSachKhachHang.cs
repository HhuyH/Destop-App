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
    public partial class DanhSachKhachHang : Form
    {
        string strCon = Connecting.GetConnectionString();
        public MainForm mf;
        string role = UserSession.role;
        public DanhSachKhachHang(MainForm mainForm)
        {
            InitializeComponent();
            LoadCustomerData();
            BackGroundColor();
            mf = mainForm;
            dataGridView1.SelectionChanged += DataGridView1_SelectionChanged;
            dataGridView1.CellDoubleClick += DataGridView1_CellDoubleClick;
            txtSearch.TextChanged += TxtSearch_TextChanged;
            DataGrindview();
            if(role == "Employee" || role == "Manager")
            {
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
            dataGridView1.BackgroundColor = this.BackColor;
            dataGridView1.BorderStyle = BorderStyle.None;
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            // Lấy nội dung tìm kiếm từ TextBox
            string keyword = txtSearch.Text.Trim();

            // Gọi phương thức SearchItems với từ khóa tìm kiếm
            Search(keyword);
        }

        private void Search(string keyword)
        {
            // Tạo câu truy vấn SQL tìm kiếm
            string query = "SELECT customer_id, full_name, address, email, phone_number FROM Customers WHERE customer_id LIKE @Keyword OR full_name LIKE @Keyword OR address LIKE @Keyword OR email LIKE @Keyword OR phone_number LIKE @Keyword";

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

        //tải danh sach khách hàng
        private void LoadCustomerData()
        {
            // Sử dụng câu lệnh using để đảm bảo rằng SqlConnection được giải phóng đúng cách ngay cả khi có ngoại lệ xảy ra
            using (SqlConnection con = new SqlConnection(strCon))
            {
                try
                {
                    // Mở kết nối
                    con.Open();

                    // Câu truy vấn SQL để lấy dữ liệu từ bảng Customers
                    string query = "SELECT customer_id, user_id, full_name, address, email, phone_number FROM Customers";

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
                            dataGridView1.Columns["customer_id"].HeaderText = "ID";
                            dataGridView1.Columns["full_name"].HeaderText = "Họ và tên";
                            dataGridView1.Columns["address"].HeaderText = "Địa chỉ";
                            dataGridView1.Columns["email"].HeaderText = "Email";
                            dataGridView1.Columns["phone_number"].HeaderText = "Số điện thoại";
                            dataGridView1.Columns["user_id"].HeaderText = "User ID";


                            // Căn giữa các cột
                            foreach (DataGridViewColumn column in dataGridView1.Columns)
                            {
                                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            }

                            dataGridView1.Columns["customer_id"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
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
        public static bool CustomerInfo = false;
        private void DataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra xem có hàng được chọn không
            if (dataGridView1.SelectedRows.Count > 0)
            {
                CustomerInfo = true;
                // Lấy dòng được chọn đầu tiên
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                // Tiến hành lấy giá trị của cột "user_id"
                CustomerID.userId = Convert.ToInt32(selectedRow.Cells["user_id"].Value);
                OpenChildForm(new ProfileForCus(mf));
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
        private void XoaKhachHang(int userId)
        {

            // Sử dụng câu lệnh using để đảm bảo rằng SqlConnection được giải phóng đúng cách ngay cả khi có ngoại lệ xảy ra
            using (SqlConnection con = new SqlConnection(strCon))
            {
                try
                {
                    // Mở kết nối
                    con.Open();

                    // Tạo đối tượng SqlCommand và thiết lập loại command là Stored Procedure
                    using (SqlCommand cmd = new SqlCommand("DeleteCustomerAndPaymentMethod", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Thêm tham số vào stored procedure
                        cmd.Parameters.AddWithValue("@user_Id", userId);


                        // Thực thi stored procedure
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Đã xóa khách hàng thành công.");
                            // Cập nhật lại hiển thị trên DataGridView hoặc các điều kiện khác tùy thuộc vào cài đặt của bạn
                        }
                        else
                        {
                            MessageBox.Show("Không có khách hàng nào được xóa.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Không thể xóa khách hàng");
                }
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            XoaKhachHang(userId);
            LoadCustomerData();
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
            CustomerInfo = false;
            AddUser A = new AddUser(mf);

            A.SetLabel("Thêm khách hàng");
            A.role = "Customer";
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
