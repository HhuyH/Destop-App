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
    public partial class DetailOrder : Form
    {
        string strCon = Connecting.GetConnectionString();
        public static int OrderID;
        public MainForm mf;
        int UserID = UserSession.UserId;
        string role = UserSession.role;

        public DetailOrder(MainForm mainForm)
        {
            InitializeComponent();
            BackGroundColor();
            mf = mainForm;
            DataGrindview();
            UserRole();
            Status();
        }

        //Kiểm tra người dùng
        private void UserRole()
        {
            if(role == "Customer")
            {
                panelAcc.Visible = false;
                panel7.Visible = false;
                panel5.Visible = true;
                panelOrder.Visible = true;
            }
            else if (role != "Employee")
            {
                panelAcc.Visible = true;
                panel7.Visible = true;
                panel5.Visible = false;
                panelOrder.Visible = false;
            }
            else if(role == "Employee")
            {
                panel5.Visible = false;
                panelOrder.Visible = false;
                panelAcc.Visible = false;
                panel7.Visible = false;
            }

        }
        private void DanhSachKhachHang_Load(object sender, EventArgs e)
        {
            this.ControlBox = false;
            LoadDetailOrders(OrderID);
            LoadOrders(OrderID);
            this.Width = 975;
        }

        //Xác nhận trạng thái
        private void Status()
        {
            if (AbortST == "Hủy")
            {
                panelAcc.Visible = false;
                panel7.Visible = false;
            }
            else if (AccST == "Xác nhận")
            {
                panel5.Visible = false;
                panelOrder.Visible = false;
            }
        }

        //Chỉnh datagrindvie
        private void DataGrindview()
        {
            // Ẩn hàng dự thêm mới ở cuối DataGridView
            dataGridView1.AllowUserToAddRows = false;
            // Đặt DataGridView thành chỉ chế độ chỉ đọc
            dataGridView1.ReadOnly = true;
            dataGridView1.BackgroundColor = this.BackColor;
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.RowHeadersVisible = false;
        }

        //chỉnh màu nền
        private void BackGroundColor()
        {
            MainForm mf = new MainForm();
            dataGridView1.BackgroundColor = mf.GetPanelBodyColor();
            dataGridView1.ReadOnly = true;
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.RowHeadersVisible = false;
            this.BackColor = mf.GetPanelBodyColor();
        }

        //tải chi tiết hóa đơn
        private void LoadDetailOrders(int OrderID)
        {

            using (SqlConnection connection = new SqlConnection(strCon))
            {
                using (SqlCommand command = new SqlCommand("GetProductNameForDetail", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@OrderID", OrderID); // Chỉ truyền một tham số

                    connection.Open();

                    // Thực thi stored procedure và lấy dữ liệu vào DataTable
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // Đặt dữ liệu vào DataGridView
                    dataGridView1.DataSource = dataTable;

                    dataGridView1.Columns["order_detail_id"].HeaderText = "STT";
                    dataGridView1.Columns["Name"].HeaderText = "Tên sản phẩm";
                    dataGridView1.Columns["unit_price"].HeaderText = "Giá";
                    dataGridView1.Columns["discount"].HeaderText = "Ưu đải";
                    dataGridView1.Columns["quantity"].HeaderText = "Số lượng";

                    // Căn giữa các cột
                    foreach (DataGridViewColumn column in dataGridView1.Columns)
                    {
                        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
            }
        }

        string AbortST;
        string AccST;
        //tải hóa đơn
        private void LoadOrders(int OrderID)
        {
            string query = "SELECT order_date, total_price, " +
                "delivery_address, cancel_status, accept_status FROM Orders WHERE order_id = @OrderID";

            using (SqlConnection connection = new SqlConnection(strCon))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OrderID", OrderID);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Đặt dữ liệu vào các Label
                            lbOrderDate.Text = "Ngày đặt hàng: " + reader["order_date"].ToString();
                            lbPrice.Text = "Tổng giá: " + reader["total_price"].ToString();
                            lbAdress.Text = reader["delivery_address"].ToString();
                            lbAbortSt.Text = "Trạng thái: " + reader["cancel_status"].ToString();
                            lbAcceptSt.Text = "Trạng thái Xác nhận: " + reader["accept_status"].ToString();
                            AbortST = reader["cancel_status"].ToString();
                            AccST = reader["accept_status"].ToString();

                            if (AbortST == "Hủy" || AccST == "Xác nhận")
                            {
                                panelAcc.Visible = false;
                                panel7.Visible = false;
                                panel5.Visible = false;
                                panelOrder.Visible = false;
                            }
                        }
                        else
                        {
                            // Xử lý trường hợp không tìm thấy dữ liệu
                            MessageBox.Show("Không tìm thấy đơn hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
        }

        private void panel2_Click(object sender, EventArgs e)
        {
            this.Close();
            mf.OpenChildForm(new ViewOrder(mf));
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnAccAbort_Click(object sender, EventArgs e)
        {
            // Hiển thị hộp thoại xác nhận
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xác nhận đơn hàng không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Kiểm tra kết quả của hộp thoại xác nhận
            if (result == DialogResult.Yes)
            {
                // Nếu người dùng chọn "Yes", thực hiện cập nhật trạng thái đơn hàng
                UpdateOrderStatus(OrderID, "Chưa xác nhận", "Hủy");
                LoadOrders(OrderID);
            }
        }

        private void BtnAcc_Click(object sender, EventArgs e)
        {
            // Hiển thị hộp thoại xác nhận
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xác nhận đơn hàng không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Kiểm tra kết quả của hộp thoại xác nhận
            if (result == DialogResult.Yes)
            {
                // Nếu người dùng chọn "Yes", thực hiện cập nhật trạng thái đơn hàng
                UpdateOrderStatus(OrderID, "Xác nhận", "Chưa hủy");
                LoadOrders(OrderID);
            }
        }

        //chuyền trạng thái hủy từ khách hàng hoặc xác nhận từ shop đơn hàng

        public void UpdateOrderStatus(int orderId, string acceptStatus, string cancelStatus)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(strCon))
                {
                    connection.Open();

                    string updateQuery = @"UPDATE Orders 
                                           SET accept_status = @accept_status, cancel_status = @cancel_status 
                                           WHERE order_id = @order_id";

                    SqlCommand command = new SqlCommand(updateQuery, connection);

                    // Điền thông tin cho các tham số
                    command.Parameters.AddWithValue("@accept_status", acceptStatus);
                    command.Parameters.AddWithValue("@cancel_status", cancelStatus);
                    command.Parameters.AddWithValue("@order_id", orderId);

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
