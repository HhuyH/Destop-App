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
using System.Xml.Linq;
using static QuanLyLinhKienDIenTu.Login;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace QuanLyLinhKienDIenTu
{
    public partial class AddSanPham : Form
    {
        string strCon = Connecting.GetConnectionString();
        public MainForm mf;
        public static string productid;
        public static bool isAddMode;
        public static bool isViewMode;
        public static bool isEditMode;
        string imagePath;
        string role = UserSession.role;

        public AddSanPham(MainForm mainForm)
        {
            InitializeComponent();
            BackGroundColor();
            Round.SetSharpCornerPanel(panelAdd);
            Round.SetSharpCornerPanel(PImage);
            mf = mainForm;
            txtPrice.KeyPress += TxtPrice_KeyPress;
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

        private void DanhSachKhachHang_Load(object sender, EventArgs e)
        {
            // Đặt lại size của các cột panel để chúng hiển thị lại
            resizePanel();

            // Cập nhật lại layout của TableLayoutPanel
            tableLayoutPanel1.ResumeLayout(true);
            tableLayoutPanel1.PerformLayout();

            this.ControlBox = false;
            LoadComponentTypes();
            LoadSuppliers();
            PImage.Visible = false;
            if (isAddMode)
            {
                //khi vùa vào form ẩn hết chỉ chừa lại mục chọn loại linh kiện
                HideBox();
                cboLinhKien.SelectedIndexChanged += CboLinhKien_SelectedIndexChanged;
                EnableBox();
                //hiện lại nút tải hình ảnh
                PImage.Visible = true;

                //chỉnh nút thành chức năng lưu
                btnAdd.Text = "     Lưu";
                btnAdd.Image = Image.FromFile(@"H:\DaiHoc\QuanLyLinhKienMayTinh\Icon\diskette.png");
            }
            else if (isViewMode)
            {
                LoadProductData(productid);
                ComponentType();
                cboLinhKien.Enabled = false;
                DisableBox();
                btnAdd.Text = "     Sữa";
                btnAdd.Image = Image.FromFile(@"H:\DaiHoc\QuanLyLinhKienMayTinh\Icon\service.png");
            }
            if(role == "Customer" || role == "Employee")
            {
                panelAdd.Visible = false;
            }
        }

        //đạt lại size cho table panel
        private void resizePanel()
        {
            // Tổng tỷ lệ phần trăm của tất cả các hàng
            int totalPercentage = 0;

            // Tính tổng số phần trăm của tất cả các hàng
            for (int i = 0; i < tableLayoutPanel1.RowCount; i++)
            {
                totalPercentage += 100; // Ví dụ: bạn có thể thay đổi giá trị này thành phần trăm bạn muốn
            }

            // Đặt SizeType của mỗi hàng thành Percent và đặt phần trăm tương ứng
            for (int i = 0; i < tableLayoutPanel1.RowCount; i++)
            {
                tableLayoutPanel1.RowStyles[i].SizeType = SizeType.Percent;
                tableLayoutPanel1.RowStyles[i].Height = (float)100 / tableLayoutPanel1.RowCount;
            }
        }

        //nút chuyen chuc nang va thuc hien no
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (isAddMode)
            {
                AddMode();
            }
            else if (isViewMode)
            {
                ViewMode();               
            }
            else if (isEditMode)
            {
                EditMode();
            }
        }

        bool SuccAdd = false;
        bool SuccEdit = true;
        //chế độ sữa và lưu sau đó chuyển qua chế độ xem
        private void AddMode()
        {
            //chuc nang add san phẩm
            if (isAddMode)
            {
                InsertProduct();
                if (SuccAdd)
                {
                    //Tải lại dữ liệu vừa được add vào
                    LoadProductData(GetLatestProductID());
                    resizePanel();
                    ComponentType();
                    DisableBox();
                    //Chỉnh lại nút Lưu thành sữa
                    btnAdd.Text = "     Sữa";
                    btnAdd.Image = Image.FromFile(@"H:\DaiHoc\QuanLyLinhKienMayTinh\Icon\service.png");
                    isViewMode = true;
                    isAddMode = false;
                    PImage.Visible = false;
                    cboLinhKien.Enabled = false;
                }
            }
        }

        //chế độ sữa
        private void EditMode()
        {
            if (isEditMode)
            {
                UpdateProduct();
                if (SuccEdit)
                {
                    //Tải lại dữ liệu vừa được add vào
                    LoadProductData(productid);
                    resizePanel();
                    ComponentType();
                    DisableBox();
                    //Chỉnh lại nút Lưu thành sữa
                    btnAdd.Text = "     Sữa";
                    btnAdd.Image = Image.FromFile(@"H:\DaiHoc\QuanLyLinhKienMayTinh\Icon\service.png");
                    isViewMode = true;
                    isEditMode = false;
                    PImage.Visible = false;
                }
                else
                {
                    MessageBox.Show("khong the cap nhap");
                }
            }
        }

        //chế độ xem và chuyên qua chế độ sữa
        private void ViewMode()
        {
            EnableBox();
            //chỉnh lại nút sữa thành lưu
            btnAdd.Text = "     Lưu";
            btnAdd.Image = Image.FromFile(@"H:\DaiHoc\QuanLyLinhKienMayTinh\Icon\diskette.png");
            isEditMode = true;
            isViewMode = false;
        }

        //Hàm thêm sản phẩm
        private void InsertProduct()
        {
            string componentTypeName = cboLinhKien.SelectedItem.ToString();
            string name = txtName.Text;
            string brand = txtBrand.Text;
            string model = txtModel.Text;
            string cap = txtCap.Text;
            string speed = txtSpeed.Text;
            string type = txtType.Text;
            decimal price = decimal.TryParse(txtPrice.Text, out decimal result) ? result : 0;

            string supplierName;
            if (cboSupplier.SelectedItem != null)
            {
                supplierName = cboSupplier.SelectedItem.ToString();
                // Tiếp tục với xử lý khi đã chọn nhà cung cấp
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một nhà cung cấp.", "Lưu ý", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(strCon))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("InsertProduct", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@component_type_name", componentTypeName);
                        cmd.Parameters.AddWithValue("@name", string.IsNullOrWhiteSpace(name) ? DBNull.Value : (object)name);
                        cmd.Parameters.AddWithValue("@brand", string.IsNullOrWhiteSpace(brand) ? DBNull.Value : (object)brand);
                        cmd.Parameters.AddWithValue("@model", string.IsNullOrEmpty(model) ? DBNull.Value : (object)model);
                        cmd.Parameters.AddWithValue("@cap", string.IsNullOrEmpty(cap) ? DBNull.Value : (object)cap);
                        cmd.Parameters.AddWithValue("@speed", string.IsNullOrEmpty(speed) ? DBNull.Value : (object)speed);
                        cmd.Parameters.AddWithValue("@type", string.IsNullOrEmpty(type) ? DBNull.Value : (object)type);
                        cmd.Parameters.AddWithValue("@price", price);
                        cmd.Parameters.AddWithValue("@supplier_name", string.IsNullOrEmpty(supplierName) ? DBNull.Value : (object)supplierName);

                        // Kiểm tra xem có ảnh được chọn không
                        if (!string.IsNullOrEmpty(imagePath))
                        {
                            try
                            {
                                cmd.Parameters.AddWithValue("@image", imagePath);
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
                            cmd.Parameters.AddWithValue("@image", DBNull.Value);
                        }

                        cmd.ExecuteNonQuery();
                        SuccAdd = true;
                        MessageBox.Show("Thêm sản phẩm thành công!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm sản phẩm: " + ex.Message);
            }
        }

        //lấy id vùa được add vào
        private string GetLatestProductID()
        {
            string latestProductID = "";

            try
            {
                using (SqlConnection connection = new SqlConnection(strCon))
                {
                    connection.Open();
                    string query = "SELECT TOP 1 product_id FROM ProductInfo ORDER BY STT DESC";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    latestProductID = cmd.ExecuteScalar()?.ToString(); // Chuyển kết quả thành chuỗi
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi lấy ID sản phẩm mới nhất: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return latestProductID;
        }

        private void CboLinhKien_SelectedIndexChanged(object sender, EventArgs e)
        {
            pictureBox1.Image = Image.FromFile(@"H:\DaiHoc\QuanLyLinhKienMayTinh\Image\camera128.png");
            // Đặt lại lai size của các cột để chúng hiển thị lại
            resizePanel();

            // Cập nhật lại layout của TableLayoutPanel
            tableLayoutPanel1.ResumeLayout(true);
            tableLayoutPanel1.PerformLayout();

            //tùy theo loại linh kiện mà hiện thị label và box tương ứng
            ComponentType();
            //Ẩn ID vi ID được tự tạo
            tableLayoutPanel1.RowStyles[1].Height = 0;
            label2.Visible = false;
            txtID.Visible = false;

        }

        // xét theo combobox loại linh kiện để hiện label và textbox phù hợp với table
        private void ComponentType()
        {
            if (cboLinhKien.SelectedItem != null && !string.IsNullOrEmpty(cboLinhKien.SelectedItem.ToString()))
            {
                string ComponentTypes = cboLinhKien.SelectedItem.ToString();

                if (ComponentTypes == "CPU")
                {
                    ShowBox();
                    label10.Visible = false;
                    txtCap.Visible = false; //Table CPU không có dung lượng
                    tableLayoutPanel1.RowStyles[5].Height = 0;
                }
                else if (ComponentTypes == "RAM")
                {
                    ShowBox();
                }
                else if (ComponentTypes == "HDD" || ComponentTypes == "SSD")
                {
                    ShowBox();
                    label6.Visible = false;
                    txtSpeed.Visible = false;
                    label7.Text = "Loại giao diện"; //txtType ở 2 table HDD và SSD tương dương với interface_type
                    tableLayoutPanel1.RowStyles[6].Height = 0;
                }
                else if (ComponentTypes == "GPU")
                {
                    ShowBox();
                    label6.Visible = false;
                    txtSpeed.Visible = false;
                    label10.Text = "Vram"; //txtCap tương đương vram ở table GPU
                    txtCap.KeyPress += TxtCap_KeyPressIntNumber;
                    label7.Text = "Loại giao diện"; //txtType tương đương với interface_type ở table GPU
                    tableLayoutPanel1.RowStyles[6].Height = 0;
                }
                else if (ComponentTypes == "Motherboard")
                {
                    ShowBox();
                    label6.Visible = false;
                    txtSpeed.Visible = false;
                    label10.Text = "Dung lượng tối da"; //txtCap Tương đương max_ram_capacity ở table Motherboard
                    tableLayoutPanel1.RowStyles[6].Height = 0;
                }
                else if (ComponentTypes == "Cooling" || ComponentTypes == "Network Adapter" ||
                    ComponentTypes == "Expansion Cards" || ComponentTypes == "Power Backup")
                {
                    ShowBox();
                    label6.Visible = false;
                    label7.Visible = false;
                    label10.Visible = false;

                    txtSpeed.Visible = false;
                    txtCap.Visible = false;
                    txtType.Visible = false;
                    tableLayoutPanel1.RowStyles[5].Height = 0;
                    tableLayoutPanel1.RowStyles[6].Height = 0;
                    tableLayoutPanel1.RowStyles[7].Height = 0;
                }
                else if (ComponentTypes == "PSU")
                {
                    ShowBox();
                    label7.Visible = false;
                    txtType.Visible = false;
                    tableLayoutPanel1.RowStyles[7].Height = 0;

                    label10.Text = "Công suất nguồn"; //txtCap Tương đương wattage table PSU
                    label6.Text = "Xếp hạng hiệu suất";//txtSpeed Tương đương efficiency_rating table PSU
                }
                else if (ComponentTypes == "Monitor")
                {
                    ShowBox();
                    label7.Visible = false;
                    txtType.Visible = false;
                    tableLayoutPanel1.RowStyles[7].Height = 0;

                    label10.Text = "Kích thước"; //txtCap Tương đương size_inch table Monitor
                    label6.Text = "Độ phân giải ";//txtSpeed Tương đương resolution table Monitor
                }
                else if (ComponentTypes == "Computer Case")
                {
                    ShowBox();
                    label7.Visible = false;
                    txtType.Visible = false;
                    tableLayoutPanel1.RowStyles[7].Height = 0;

                    label10.Text = "Kích thước"; //txtCap Tương đương size table Computer_Case
                    label6.Text = "Màu ";//txtSpeed Tương đương color table Computer_Case
                }
                else
                {
                    tableLayoutPanel1.RowStyles[5].Height = 0;
                    tableLayoutPanel1.RowStyles[6].Height = 0;
                    tableLayoutPanel1.RowStyles[7].Height = 0;

                    ShowBox();
                    label6.Visible = false;
                    label7.Visible = false;
                    label10.Visible = false;

                    txtSpeed.Visible = false;
                    txtCap.Visible = false;
                    txtType.Visible = false;
                }
            }
        }

        private void TxtCap_KeyPressIntNumber(object sender, KeyPressEventArgs e)
        {
            // Kiểm tra xem ký tự được nhập vào có phải là một số hoặc phím Backspace không
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                // Nếu không phải là số hoặc phím Backspace, hủy sự kiện KeyPress
                e.Handled = true;
                // Hiển thị một cảnh báo cho người dùng
                MessageBox.Show("Vui lòng chỉ nhập số.");
            }
        }

        //Bật box lên lại
        private void EnableBox()
        {
            cboSupplier.Enabled = true;
            txtPrice.Enabled = true;
            txtType.Enabled = true;
            txtSpeed.Enabled = true;
            txtModel.Enabled = true;
            txtBrand.Enabled = true;
            txtName.Enabled = true;
            txtID.Enabled = true;
            txtCap.Enabled = true;
        }

        //Vô hiệu hóa box
        private void DisableBox()
        {
            cboSupplier.Enabled = false;
            txtPrice.Enabled = false;
            txtType.Enabled = false;
            txtSpeed.Enabled = false;
            txtModel.Enabled = false;
            txtBrand.Enabled = false;
            txtName.Enabled = false;
            txtID.Enabled = false;
            txtCap.Enabled = false;
        }

        //ẩn tất cả các giá trị trước khi chọn component type
        private void HideBox()
        {
            //ẩn tên
            label2.Visible = false;
            label3.Visible = false;
            label4.Visible = false;
            label5.Visible = false;
            label6.Visible = false;
            label7.Visible = false;
            label8.Visible = false;
            label9.Visible = false;
            label10.Visible = false;

            //ẩn textbox và combobox
            cboSupplier.Visible = false;
            txtPrice.Visible = false;
            txtType.Visible = false;
            txtSpeed.Visible = false;
            txtModel.Visible = false;
            txtBrand.Visible = false;
            txtName.Visible = false;
            txtID.Visible = false;
            txtCap.Visible = false;
        }
        
        //hiện thị tất cả giá trị
        private void ShowBox()
        {
            // Hiển thị các nhãn
            label2.Visible = true;
            label3.Visible = true;
            label4.Visible = true;
            label5.Visible = true;
            label6.Visible = true;
            label7.Visible = true;
            label8.Visible = true;
            label9.Visible = true;
            label10.Visible = true;

            // Hiển thị các textbox và combobox
            cboSupplier.Visible = true;
            txtPrice.Visible = true;
            txtType.Visible = true;
            txtSpeed.Visible = true;
            txtModel.Visible = true;
            txtBrand.Visible = true;
            txtName.Visible = true;
            txtID.Visible = true;
            txtCap.Visible = true;
        }

        //tải sản phẩm
        private void LoadProductData(string productId)
        {
            // Sử dụng câu lệnh using để đảm bảo rằng SqlConnection được giải phóng đúng cách ngay cả khi có ngoại lệ xảy ra
            using (SqlConnection con = new SqlConnection(strCon))
            {
                try
                {
                    con.Open();

                    string query = @"EXEC GetAllProductInfo @productid";
                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@productid", productId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            cboLinhKien.SelectedItem = reader["LinhKien"].ToString();
                            txtID.Text = reader["ID"].ToString();
                            txtName.Text = reader["Name"].ToString();
                            txtBrand.Text = reader["Brand"].ToString();
                            txtModel.Text = reader["Model"].ToString();
                            txtCap.Text = reader["DungLuong"].ToString();
                            txtSpeed.Text = reader["Speed"].ToString();
                            txtType.Text = reader["Type"].ToString();
                            txtPrice.Text = reader["Price"].ToString();
                            cboSupplier.SelectedItem = reader["Supplier"].ToString();

                            string imagePathforupload = reader["image"].ToString();
                            // Kiểm tra xem có hình ảnh được trả về không
                            if (!reader.IsDBNull(reader.GetOrdinal("image")))
                            {
                                // Tải hình ảnh từ đường dẫn tệp cục bộ và hiển thị trong PictureBox
                                pictureBox1.Image = Image.FromFile(imagePathforupload);
                                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                                imagePath = imagePathforupload;
                            }
                            else
                            {
                                // Hiển thị một hình ảnh mặc định nếu không có hình ảnh được trả về
                                pictureBox1.Image = Image.FromFile(@"H:\DaiHoc\QuanLyLinhKienMayTinh\Image\camera128.png");
                            }
                        }
                    }
                    else
                    {
                        // Không có dữ liệu được trả về từ câu truy vấn
                        MessageBox.Show("Không tìm thấy sản phẩm có ID: " + productId);
                    }
                }
                catch (Exception ex)
                {
                    // Xử lý ngoại lệ nếu có
                    MessageBox.Show("Đã xảy ra lỗi: " + ex.Message);
                }
            }
        }


        // Hàm cập nhật sản phẩm
        private void UpdateProduct()
        {
            string productId = txtID.Text;
            string componentTypeName = cboLinhKien.SelectedItem.ToString();
            string name = txtName.Text;
            string brand = txtBrand.Text;
            string model = txtModel.Text;
            string cap = txtCap.Text;
            string speed = txtSpeed.Text;
            string type = txtType.Text;
            decimal price = decimal.TryParse(txtPrice.Text, out decimal result) ? result : 0;

            string supplierName;
            if (cboSupplier.SelectedItem != null)
            {
                supplierName = cboSupplier.SelectedItem.ToString();
                // Tiếp tục với xử lý khi đã chọn nhà cung cấp
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một nhà cung cấp.", "Lưu ý", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(strCon))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("UpdateProduct", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@product_id", productId);
                        cmd.Parameters.AddWithValue("@component_type_name", componentTypeName);
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@brand", brand);
                        cmd.Parameters.AddWithValue("@model", model);
                        cmd.Parameters.AddWithValue("@cap", cap);
                        cmd.Parameters.AddWithValue("@speed", speed);
                        cmd.Parameters.AddWithValue("@type", type);
                        cmd.Parameters.AddWithValue("@price", price);
                        cmd.Parameters.AddWithValue("@supplier_name", supplierName);

                        if (!string.IsNullOrEmpty(imagePath))
                        {
                            try
                            {
                                cmd.Parameters.AddWithValue("@image", imagePath);
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
                            cmd.Parameters.AddWithValue("@image", DBNull.Value);
                        }

                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Cập nhật sản phẩm thành công!");
                        SuccEdit = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật sản phẩm: " + ex.Message);
            }
        }

        //chỉnh màu nền
        private void BackGroundColor()
        {
            MainForm mf = new MainForm();

            this.BackColor = mf.GetPanelBodyColor();
        }

        //tải ảnh lên
        private void UploadImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg, *.jpeg, *.png)|*.jpg; *.jpeg; *.png";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                imagePath = openFileDialog.FileName;
                try
                {

                    // Hiển thị hình ảnh trên PictureBox
                    pictureBox1.ImageLocation = imagePath;
                    pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                    // Lưu đường dẫn tệp hình ảnh để sử dụng trong Update
                    this.imagePath = imagePath;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Đã xảy ra lỗi khi đọc tệp hình ảnh: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        
        //nha cung cap
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

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (isAddMode || isEditMode)
            {
                UploadImage();
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cboLinhKien_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
