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

namespace QuanLyLinhKienDIenTu
{
    public partial class Form1 : Form
    {
        string strcon = @"Data Source=DESKTOP-4AOOEVT\SQLEXPRESS;Initial Catalog=QuanLyLinhKienMayTinh;Integrated Security=True";
        SqlConnection con;
        SqlDataAdapter adapter;
        DataTable dt;
        // Khai báo biến để lưu trữ node cuối cùng được chọn trên TreeView
        private TreeNode lastSelectedNode;
        private int customerId;
        private KhachHang kh;
        public Form1()
        {
            InitializeComponent();
            this.FormClosing += Form1_FormClosing;
            dataGridView1.ColumnAdded += DataGridView1_ColumnAdded;
            dataGridView1.CellDoubleClick += DataGridView1_CellDoubleClick;
            dataGridView1.SelectionChanged += DataGridView1_SelectionChanged;
            KhachHang kh = new KhachHang(null);
            kh.FormClosing += Kh_FormClosing;
            kh.FormClosed += Kh_FormClosed;
        }

        private void Kh_FormClosed(object sender, FormClosedEventArgs e)
        {
                // Thực hiện các hành động cần thiết khi form KhachHang đóng do người dùng thực hiện
                LoadCustomerData();
        }

        private void Kh_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                // Lấy giá trị của cột "customer_id" của dòng hiện tại đang được chọn
                customerId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["customer_id"].Value);
            }
        }

        private void DataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Lấy ID của khách hàng được chọn từ DataGridView
            int customerId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["customer_id"].Value);

            // Mở Form ThongTinChiTiet và truyền ID qua
            KhachHang CinFor = new KhachHang(customerId);
            CinFor.ShowDialog();
        }

        private void DataGridView1_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
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

        private void Form1_Load(object sender, EventArgs e)
        {
            con = new SqlConnection(strcon);
            //Ẩn datagrindview
            HideDataGridView();

            //Ẩn cho Treeview
            HideTreeView();
        }

        //Ẩn datagrindview
        private void HideDataGridView()
        {
            dataGridView1.BackgroundColor = this.BackColor;
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.RowHeadersVisible = false;
        }

        //Ẩn Treeview
        private void HideTreeView()
        {
            treeView1.BorderStyle = BorderStyle.None;
            treeView1.AfterSelect += TreeView1_AfterSelect;
        }

        private void TreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            lastSelectedNode = e.Node;
            if (e.Node.Text == "Khách Hàng")
            {
                LoadCustomerData();
            }
        }

        private void LoadTreeViewData()
        {

        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {

        }

        //láy dữ liệu khách hàng
        private void LoadCustomerData()
        {
            try
            {
                // Mở kết nối đến CSDL
                con.Open();

                // Tạo câu lệnh SQL để lấy dữ liệu từ bảng Customers
                string query = "SELECT customer_id, full_name, address, email, phone_number FROM Customers";

                // Tạo đối tượng SqlCommand
                SqlCommand cmd = new SqlCommand(query, con);

                // Tạo đối tượng SqlDataAdapter
                adapter = new SqlDataAdapter(cmd);

                // Khởi tạo DataTable để lưu trữ dữ liệu
                dt = new DataTable();

                // Đổ dữ liệu từ SqlDataAdapter vào DataTable
                adapter.Fill(dt);

                // Gán DataTable làm nguồn dữ liệu cho DataGridView
                dataGridView1.DataSource = dt;

                // Cấu hình các cột
                dataGridView1.Columns["customer_id"].HeaderText = "ID";
                dataGridView1.Columns["full_name"].HeaderText = "Họ và tên";
                dataGridView1.Columns["address"].HeaderText = "Địa chỉ";
                dataGridView1.Columns["email"].HeaderText = "Email";
                dataGridView1.Columns["phone_number"].HeaderText = "Số điện thoại";

                // Thiết lập căn giữa cho các cột
                foreach (DataGridViewColumn column in dataGridView1.Columns)
                {
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

                dataGridView1.Columns["customer_id"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message);
            }
            finally
            {
                // Đóng kết nối sau khi sử dụng
                con.Close();
            }
        }

        private void treeView1_AfterSelect_1(object sender, TreeViewEventArgs e)
        {

        }

        // Định nghĩa một lớp để lưu trữ thông tin chi tiết của khách hàng
        public class CustomerDetails
        {
            public string CustomerId { get; set; }
            public string FullName { get; set; }
            public string Address { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem biến lastSelectedNode có giá trị hay không
            if (lastSelectedNode != null)
            {
                // Kiểm tra loại của node được chọn trước đó
                if (lastSelectedNode.Text == "Khách Hàng")
                {
                    // Thực hiện hành động xóa cho node khách hàng
                    XoaKhachHang(customerId);
                    LoadCustomerData();
                }
                else if (lastSelectedNode.Text == "Nhân Viên")
                {
                    // Thực hiện hành động xóa cho node nhân viên
                    XoaNhanVien();
                }
                // them loại node khác

                // Sau khi xử lý xong, đặt biến lastSelectedNode về null để sẵn sàng cho lần chọn tiếp theo
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một node trên TreeView trước khi thực hiện xóa.");
            }
        }

        //Hàm xóa khách hàng
        private void XoaKhachHang(int customerId)
        {
            try
            {
                // Kiểm tra xem customerId và lastSelectedNode.Tag có giá trị không
                if (customerId > 0 && lastSelectedNode != null)
                {
                    // Mở kết nối đến CSDL
                    con.Open();

                    // Tạo đối tượng SqlCommand và thiết lập loại command là Stored Procedure
                    SqlCommand cmd = new SqlCommand("DeleteCustomerAndPaymentMethod", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Thêm tham số vào stored procedure
                    cmd.Parameters.AddWithValue("@customerId", customerId);

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
                else
                {
                    MessageBox.Show("Không thể xóa vì không có khách hàng được chọn.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message);
            }
            finally
            {
                // Đóng kết nối sau khi sử dụng
                con.Close();
            }
        }

        //Hàm xóa Nhân Viên chưa viết
        private void XoaNhanVien()
        {

        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem biến lastSelectedNode có giá trị hay không
            if (lastSelectedNode != null)
            {
                // Kiểm tra loại của node được chọn trước đó
                if (lastSelectedNode.Text == "Khách Hàng")
                {
                    KhachHang aKhachHang = new KhachHang(null);
                    aKhachHang.Show();
                }
                else if (lastSelectedNode.Text == "Nhân Viên")
                {

                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một node trên TreeView trước khi thực hiện xóa.");
            }
        }

        private void btnRef_Click(object sender, EventArgs e)
        {
            LoadCustomerData();
        }
    }
}
