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
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static QuanLyLinhKienDIenTu.Login;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace QuanLyLinhKienDIenTu
{
    public partial class Order : Form
    {
        string strCon = Connecting.GetConnectionString();
        public MainForm mf;
        public static string productid;
        public static bool isAddMode;
        string role = UserSession.role;
        int UserID = UserSession.UserId;
        int IntproductId;
        int CusId;

        public Order(MainForm mainForm)
        {
            InitializeComponent();
            BackGroundColor();
            Round.SetSharpCornerPanel(panelAddCart);
            Round.SetSharpCornerPanel(panel6);
            Round.SetRoundedButton(btnCart);
            Round.SetSharpCornerPanel(panel2);
            mf = mainForm;
        }

        private void TxtPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Kiểm tra xem ký tự được nhập vào có phải là chữ không
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                // Nếu là chữ, ngăn việc thêm ký tự đó vào TextBox
                e.Handled = true;
            }
        }

        private void Order_Load(object sender, EventArgs e)
        {
            LoadCustomersToComboBox();
            this.ControlBox = false;
            cboLinhKien.SelectedIndexChanged += CboLinhKien_SelectedIndexChanged;
            cboSupplier.SelectedIndexChanged += CboSupplier_SelectedIndexChanged;
            LoadComponentTypes();
            LoadSuppliers();
            LoadProductData();
            DataGrindview();
            cboCus.TextChanged += CboCus_TextChanged;
            txtSearch.TextChanged += TxtSearch_TextChanged;
            dataGridView1.SelectionChanged += DataGridView1_SelectionChanged;
            btnCart.BackColor = panel2.BackColor;
            if (role == "Customer")
            {
                tableLayoutPanel2.Visible = false;
            }
            else
            {
                panel6.Visible = false;
            }
            Cart.CustomerID = 0;
            dataGridView1.CellDoubleClick += DataGridView1_CellDoubleClick;
        }

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
                        mf.OpenChildForm(new AddSanPham(mf));
                    }
                }
            }
        }


        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            // Kiểm tra xem có cell nào được chọn không
            if (dataGridView1.SelectedCells.Count > 0)
            {
                // Lấy cell đầu tiên được chọn
                DataGridViewCell selectedCell = dataGridView1.SelectedCells[0];

                // Kiểm tra xem cột "product_id" có tồn tại trong DataGridView không
                if (dataGridView1.Columns.Contains("product_id") && selectedCell.OwningRow.Cells["product_id"] != null && selectedCell.OwningRow.Cells["product_id"].Value != null)
                {
                    // Lấy ID sản phẩm từ cột "product_id"
                    string productId = selectedCell.OwningRow.Cells["product_id"].Value.ToString();

                    // Gọi phương thức GetProductID và chuyển ID sản phẩm vào form Cart
                    Cart.ProductID = GetProductID(productId);
                    IntproductId = GetProductID(productId);
                }
            }

        }

        //Lấy ID của product và chuyển nó vào form Cart
        public int GetProductID(string productID)
        {
            int productId = -1; // Giá trị mặc định nếu không tìm thấy sản phẩm

            using (SqlConnection connection = new SqlConnection(strCon))
            {
                SqlCommand command = new SqlCommand("GetProductID", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@productid", productID);

                connection.Open();
                productId = Convert.ToInt32(command.ExecuteScalar()); // Lấy giá trị trả về từ procedure
            }

            return productId;
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
            string query = @"SELECT * FROM ProductInfo 
                     WHERE product_name LIKE @Keyword 
                     OR product_id LIKE @Keyword 
                     OR price LIKE @Keyword 
                     OR component_type LIKE @Keyword 
                     OR quantity LIKE @Keyword";

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

        private void DataGrindview()
        {
            // Ẩn hàng dự thêm mới ở cuối DataGridView
            dataGridView1.AllowUserToAddRows = false;
            // Đặt DataGridView thành chỉ chế độ chỉ đọc
            dataGridView1.ReadOnly = true;
            MainForm mf = new MainForm();
            dataGridView1.BackgroundColor = mf.GetPanelBodyColor();
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.RowHeadersVisible = false;
            this.BackColor = mf.GetPanelBodyColor();
        }

        private void CboLinhKien_SelectedIndexChanged(object sender, EventArgs e)
        {

            string SelectType = "";
            if (SelectType != "Không")
            {
                SelectType = cboLinhKien.SelectedItem.ToString();
                LoadProductsFromTypeOrSupplier(SelectType, "");
            }
        }

        private void CboSupplier_SelectedIndexChanged(object sender, EventArgs e)
        {

            string SelectSupplier = "";

            if (SelectSupplier != "Không")
            {
                SelectSupplier = cboSupplier.Text;
                LoadProductsFromTypeOrSupplier("", SelectSupplier);

            }

        }

        //hàm load dữ liệu theo 2 cbo loại và nhà cung cấp
        private void LoadProductsFromTypeOrSupplier(string SelectType, string SelectSupplier)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(strCon))
                {
                    con.Open();

                    // Gọi stored procedure và truyền tham số
                    SqlCommand cmd = new SqlCommand("GetProductFromTypeAndSupplier", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@TypeName", SelectType);
                    cmd.Parameters.AddWithValue("@SupplierName", SelectSupplier);

                    // Thực thi truy vấn và đổ dữ liệu vào DataTable
                    DataTable dt = new DataTable();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }

                    // Kiểm tra nếu không có dữ liệu nào được trả về
                    if (dt.Rows.Count == 0)
                    {
                        // Xóa dữ liệu hiện tại của DataTable
                        dt.Rows.Clear();
                    }

                    // Đặt DataTable làm nguồn dữ liệu cho DataGridView
                    dataGridView1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message);
            }
        }

        //chỉnh màu nền
        private void BackGroundColor()
        {
            MainForm mf = new MainForm();

            this.BackColor = mf.GetPanelBodyColor();
        }

        //tải sản phẩm
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

        //Tải lên loại linh kiện
        private void LoadComponentTypes()
        {
            try
            {
                // Kết nối đến cơ sở dữ liệu
                using (SqlConnection conn = new SqlConnection(strCon))
                {
                    conn.Open();

                    // Thực hiện truy vấn và tải dữ liệu vào ComboBox
                    string query = "SELECT component_type_name FROM Component_Types";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    cboLinhKien.Items.Clear(); // Xóa các mục cũ trong ComboBox trước khi thêm mới

                    cboLinhKien.Items.Add("Không");
                    cboLinhKien.SelectedIndex = 0; // Index của mục "Không" trong danh sách

                    while (reader.Read())
                    {
                        // Lấy dữ liệu từ SqlDataReader và thêm vào ComboBox
                        string typeName = reader["component_type_name"].ToString();
                        cboLinhKien.Items.Add(typeName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message);
            }
        }
        
        //Tải nhà cung cấp
        private void LoadSuppliers()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(strCon))
                {
                    connection.Open();
                    // Thực hiện truy vấn và tải dữ liệu vào ComboBox
                    string query = "SELECT supplier_name FROM Suppliers";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    SqlDataReader reader = cmd.ExecuteReader();

                    cboSupplier.Items.Clear(); // Xóa các mục cũ trong ComboBox trước khi thêm mới

                    cboSupplier.Items.Add("Không");
                    cboSupplier.SelectedIndex = 0; // Index của mục "Không" trong danh sách

                    while (reader.Read())
                    {
                        // Lấy dữ liệu từ SqlDataReader và thêm vào ComboBox
                        string supplierName = reader["supplier_name"].ToString();
                        cboSupplier.Items.Add(supplierName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi tải danh sách nhà cung cấp: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cboLinhKien_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        // Tạo một danh sách để lưu thông tin khách hàng
        List<string> customerInfoList = new List<string>();

        public class Customer
        {
            public int CustomerID { get; set; }
            public string FullName { get; set; }
            public string PhoneNumber { get; set; }

            public override string ToString()
            {
                return $"{CustomerID} | {FullName} | {PhoneNumber}";
            }
        }

        //load khach hang len combobox
        private void LoadCustomersToComboBox()
        {
            try
            {
                // Kết nối đến cơ sở dữ liệu
                using (SqlConnection conn = new SqlConnection(strCon))
                {
                    conn.Open();

                    // Thực hiện truy vấn để lấy thông tin khách hàng
                    string query = "SELECT customer_id, full_name, phone_number FROM Customers";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    // Xóa các mục cũ trong ComboBox trước khi thêm mới
                    cboCus.Items.Clear();

                    // Duyệt qua từng hàng kết quả từ truy vấn và thêm vào ComboBox và danh sách AutoComplete
                    while (reader.Read())
                    {
                        Customer customer = new Customer
                        {
                            CustomerID = Convert.ToInt32(reader["customer_id"]),
                            FullName = reader["full_name"].ToString(),
                            PhoneNumber = reader["phone_number"].ToString()
                        };

                        // Thêm chuỗi này vào ComboBox
                        cboCus.Items.Add(customer);

                    }

                    // Kích hoạt chức năng AutoComplete
                    cboCus.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    cboCus.AutoCompleteSource = AutoCompleteSource.CustomSource;

                    // Thiết lập danh sách AutoComplete
                    cboCus.AutoCompleteCustomSource.AddRange(customerInfoList.ToArray());

                    // Đóng kết nối
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message);
            }
        }

        private void CboCus_TextChanged(object sender, EventArgs e)
        {

        }

        private void cboCus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboCus.SelectedItem != null)
            {
                // Ép kiểu đối tượng được chọn về Customer
                Customer selectedCustomer = (Customer)cboCus.SelectedItem;

                // Lấy CustomerID từ đối tượng Customer và gán vào biến CusId
                CusId = selectedCustomer.CustomerID;

                // Gán CustomerID cho Cart
                Cart.CustomerID = CusId;
            }
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if(role == "Customer")
            {
                CusId = GetCustomerIdByUserId(UserID);
                // Gọi phương thức InsertCart để thêm sản phẩm vào giỏ hàng và kiểm tra kết quả trả về
                if (InsertCart(IntproductId, CusId, 1))
                {
                    MessageBox.Show("Sản phẩm đã được thêm vào giỏ hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                // Gọi phương thức InsertCart để thêm sản phẩm vào giỏ hàng và kiểm tra kết quả trả về
                if (InsertCart(IntproductId, CusId, 1))
                {
                    MessageBox.Show("Sản phẩm đã được thêm vào giỏ hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        //Lấy CustomerID từ UserID
        private int GetCustomerIdByUserId(int userId)
        {
            string query = "SELECT customer_id FROM Customers WHERE user_id = @user_id";

            using (SqlConnection connection = new SqlConnection(strCon))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@user_id", userId);

                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            return Convert.ToInt32(result);
                        }
                        else
                        {
                            MessageBox.Show("không tìm thấy mã khách hàng.");
                            return -1;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                        return -1;
                    }
                }
            }
        }

        //Thêm sản phẩm vào Cart
        public bool InsertCart(int productId, int customerId, int quantity)
        {
            // Tạo kết nối với cơ sở dữ liệu
            using (SqlConnection connection = new SqlConnection(strCon))
            {
                // Tạo đối tượng SqlCommand để thực thi Stored Procedure
                using (SqlCommand command = new SqlCommand("InsertCart", connection))
                {
                    // Đặt kiểu của SqlCommand là StoredProcedure
                    command.CommandType = CommandType.StoredProcedure;

                    // Thêm các tham số cho Stored Procedure
                    command.Parameters.AddWithValue("@ProductID", productId);
                    command.Parameters.AddWithValue("@CustomerID", customerId);
                    command.Parameters.AddWithValue("@Quantity", quantity);

                    try
                    {
                        // Mở kết nối và thực thi câu lệnh
                        connection.Open();
                        command.ExecuteNonQuery();
                        return true; // Trả về true nếu thêm thành công
                    }
                    catch (Exception ex)
                    {
                        // Xử lý lỗi nếu có
                        MessageBox.Show("Vui lòng chọn khách hàng và sảng phẩm muốn thêm");
                        return false; // Trả về false nếu có lỗi
                    }
                }
            }
        }

        private void btnCart_Click(object sender, EventArgs e)
        {
            if(role == "Customer")
            {
                Cart.CustomerID = GetCustomerIdByUserId(UserID);
                mf.OpenChildForm(new Cart(mf));
                mf.TitleLabel.Text = "Giỏ hàng";
            }
            else
            {
                if (Cart.CustomerID != default(int))
                {
                    mf.OpenChildForm(new Cart(mf));
                    mf.TitleLabel.Text = "Giỏ hàng";
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn khách hàng");
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            mf.OpenChildForm(new ViewOrder(mf));
        }
    }
}
