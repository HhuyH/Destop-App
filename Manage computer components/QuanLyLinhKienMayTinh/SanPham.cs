using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static QuanLyLinhKienDIenTu.Login;

namespace QuanLyLinhKienDIenTu
{
    public partial class SanPham : Form
    {
        string strCon = Connecting.GetConnectionString();
        public MainForm mf;
        public static string productID;
        string role = UserSession.role;

        public SanPham(MainForm mainForm)
        {
            InitializeComponent();
            LoadProductData();
            BackGroundColor();
            Round.SetSharpCornerPanel(panelAdd);
            mf = mainForm;
            dataGridView1.SelectionChanged += DataGridView1_SelectionChanged;
            dataGridView1.CellDoubleClick += DataGridView1_CellDoubleClick;
            dataGridView1.CellClick += DataGridView1_CellClick;
            txtSearch.TextChanged += TxtSearch_TextChanged;
            DataGrindview();
            dataGridView1.CellEndEdit += DataGridView1_CellEndEdit;
        }

        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (role != "Customer" && role != "Employee")
            {
                // Kiểm tra xem người dùng double click vào cột "quantity"
                if (dataGridView1.Columns[e.ColumnIndex].Name == "quantity")
                {
                    dataGridView1.ReadOnly = false;
                    // Đặt chỉ đọc cho các ô trừ "quantity"
                    foreach (DataGridViewCell cell in dataGridView1.Rows[e.RowIndex].Cells)
                    {
                        if (cell.OwningColumn.Name != "quantity")
                        {
                            cell.ReadOnly = true;
                        }
                    }

                    // Bắt đầu chỉnh sửa trên ô "quantity"
                    dataGridView1.BeginEdit(true);
                    dataGridView1.EditMode = DataGridViewEditMode.EditOnEnter;
                }
            }
        }

        private void DanhSachKhachHang_Load(object sender, EventArgs e)
        {
            AddSanPham.isAddMode = false;
            this.ControlBox = false;
            if(role == "Customer" || role =="Employee")
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
        }

        //Tìm kiếm theo khi chữ trên textbox được thay đổi 
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
            // Tạo câu truy vấn SQL tìm kiếm sản phẩm
            string query = "SELECT STT, product_id, product_name, price, component_type, quantity FROM ProductInfo " +
                            "WHERE STT LIKE @Keyword OR product_id LIKE @Keyword OR product_name LIKE @Keyword OR price LIKE @Keyword OR component_type LIKE @Keyword OR quantity LIKE @Keyword";

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

        //tải danh sách sản phẩm
        private void LoadProductData()
        {
            // Sử dụng câu lệnh using để đảm bảo rằng SqlConnection được giải phóng đúng cách ngay cả khi có ngoại lệ xảy ra
            using (SqlConnection con = new SqlConnection(strCon))
            {
                try
                {
                    // Mở kết nối
                    con.Open();
                    // Câu truy vấn SQL để lấy dữ liệu từ bảng ProductInfo
                    string query = "SELECT STT, product_id, product_name, price, component_type, quantity FROM ProductInfo";

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
                            dataGridView1.Columns["STT"].HeaderText = "STT";
                            dataGridView1.Columns["product_id"].HeaderText = "ID";
                            dataGridView1.Columns["product_name"].HeaderText = "Tên sản phẩm";
                            dataGridView1.Columns["price"].HeaderText = "Giá";
                            dataGridView1.Columns["component_type"].HeaderText = "Loại thành phần";
                            dataGridView1.Columns["quantity"].HeaderText = "Số lượng";

                            // Căn giữa các cột
                            foreach (DataGridViewColumn column in dataGridView1.Columns)
                            {
                                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            }
                            dataGridView1.Columns["STT"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                            dataGridView1.Columns["product_id"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Đã xảy ra lỗi: " + ex.Message);
                }
            }
        }

        //Cập nhập số lượng
        private void UpdateQuantityInDatabase(int productId, int newQuantity)
        {
            // Tạo câu lệnh SQL để cập nhật số lượng trong bảng Products
            string query = "UPDATE Products SET quantity = @NewQuantity WHERE product_id = @ProductId";

            // Tạo kết nối đến cơ sở dữ liệu
            using (SqlConnection con = new SqlConnection(strCon))
            {
                try
                {
                    // Mở kết nối
                    con.Open();

                    // Tạo đối tượng SqlCommand
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        // Thêm tham số cho câu lệnh SQL
                        cmd.Parameters.AddWithValue("@NewQuantity", newQuantity);
                        cmd.Parameters.AddWithValue("@ProductId", productId);

                        // Thực thi câu lệnh SQL
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Đã xảy ra lỗi khi cập nhật số lượng sản phẩm: " + ex.Message);
                }
            }
        }

        private void DataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["quantity"].Index)
            {
                // Lấy ID sản phẩm từ cột product_id
                int productId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["STT"].Value);
                int newQuantity = 0;
                // Kiểm tra nếu giá trị của ô "quantity" không phải là DBNull
                if (dataGridView1.Rows[e.RowIndex].Cells["quantity"].Value != DBNull.Value)
                {
                    // Chuyển đổi giá trị của ô "quantity" sang kiểu int
                    newQuantity = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["quantity"].Value);
                    // Tiếp tục xử lý dữ liệu ở đây
                }
                else
                {

                }
                // Cập nhật số lượng vào cơ sở dữ liệu
                UpdateQuantityInDatabase(productId, newQuantity);
                dataGridView1.ReadOnly = true;
            }
        }

        //lấy ID sản phẩm được chọn
        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSearch.Text))
            {
                // Kiểm tra xem có hàng được chọn không
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    // Lấy dòng được chọn đầu tiên
                    DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                    // Kiểm tra xem cột "product_id" có tồn tại trong DataGridView không
                    if (dataGridView1.Columns.Contains("product_id") && selectedRow.Cells["product_id"] != null && selectedRow.Cells["product_id"].Value != null)
                    {
                        // Gán giá trị của cột "product_id" cho biến AddSanPham.productid (kiểu string)
                        productID = selectedRow.Cells["product_id"].Value.ToString();
                    }
                }
            }
        }

        //double click event cần sữa
        private void DataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (string.IsNullOrEmpty(txtSearch.Text))
            {
                // Kiểm tra xem có hàng được chọn không
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    // Lấy dòng được chọn đầu tiên
                    DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                    // Kiểm tra xem cột "product_id" có tồn tại trong DataGridView không
                    if (dataGridView1.Columns.Contains("product_id") && selectedRow.Cells["product_id"] != null && selectedRow.Cells["product_id"].Value != null)
                    {
                        // Gán giá trị của cột "product_id" cho biến AddSanPham.productid (kiểu string)
                        AddSanPham.productid = selectedRow.Cells["product_id"].Value.ToString();
                        AddSanPham.isViewMode = true;
                        // Mở cửa sổ AddSanPham
                        OpenChildForm(new AddSanPham(mf));
                    }
                }
            }
        }

        //gọi child form từ mainform rồi sữ dụng bên này
        private void OpenChildForm(Form childForm)
        {
            if (mf != null)
            {
                mf.OpenChildForm(childForm); // Gọi phương thức của MainForm từ tham chiếu
            }
        }

        bool SuccDel = false;
        //Hàm xóa khách hàng
        private void XoaSanPham()
        {
            // Sử dụng câu lệnh using để đảm bảo rằng SqlConnection được giải phóng đúng cách ngay cả khi có ngoại lệ xảy ra
            using (SqlConnection con = new SqlConnection(strCon))
            {
                try
                {
                    // Mở kết nối
                    con.Open();

                    // Tạo đối tượng SqlCommand và thiết lập loại command là Stored Procedure
                    using (SqlCommand cmd = new SqlCommand("DeleteProductByID", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Thêm tham số vào stored procedure
                        cmd.Parameters.AddWithValue("@product_id", productID);


                        // Thực thi stored procedure
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected >= -1)
                        {
                            MessageBox.Show("Đã xóa sản phẩm thành công.");
                            // Cập nhật lại hiển thị trên DataGridView hoặc các điều kiện khác tùy thuộc vào cài đặt của bạn
                            SuccDel = true;
                        }
                        else
                        {
                            MessageBox.Show("Không có sản phẩm nào được xóa.");
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
            XoaSanPham();
            if (SuccDel)
            {
                    LoadProductData();
            }
            else
            {
                    MessageBox.Show("Xoa that bai");
            }

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
            AddSanPham.isViewMode = false;
            AddSanPham.isAddMode = true;
            OpenChildForm(new AddSanPham(mf));
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
