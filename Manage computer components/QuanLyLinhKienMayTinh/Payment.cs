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

namespace QuanLyLinhKienDIenTu
{
    public partial class Payment : Form
    {
        string strCon = Connecting.GetConnectionString();
        int userId = UserSession.UserId;
        int cusId = CustomerID.userId;
        int empID = EmployeeID.userId;
        bool CustomerInfo = DanhSachKhachHang.CustomerInfo;
        bool EmployeeInfo = DanhSachNhanVien.EmployeeInfo;

        int IDForUpdate = 0;
        public Payment()
        {
            InitializeComponent();
            Round.SetRoundedButton(btnFix);
            Round.SetSharpCornerPanel(panel_fix);
            if (CustomerInfo)
            {
                LoadPaymentMethods(cusId);
                CustomerInfo = false;
                IDForUpdate = 1;
            }else if (EmployeeInfo)
            {
                IDForUpdate = 3;
                LoadPaymentMethods(empID);
            }
            else
            {
                LoadPaymentMethods(userId);
                IDForUpdate = 2;
            }
            HideTextBox();
            cboPayment.SelectedIndexChanged += CboPayment_SelectedIndexChanged;
            txtNumCard.KeyPress += TxtNumCard_KeyPress;
            txtCVV.KeyPress += TxtCVV_KeyPress;
            txtNumBank.KeyPress += TxtNumBank_KeyPress;
        }


        private void TxtNumBank_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Kiểm tra xem ký tự được nhập có phải là số không
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                // Nếu không phải là số, không cho phép nhập
                e.Handled = true;
            }
        }

        private void TxtCVV_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Kiểm tra xem ký tự được nhập có phải là số không
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                // Nếu không phải là số, không cho phép nhập
                e.Handled = true;
            }
        }

        private void TxtNumCard_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Kiểm tra xem ký tự được nhập có phải là số không
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                // Nếu không phải là số, không cho phép nhập
                e.Handled = true;
            }
        }

        private void CboPayment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (String.Equals(cboPayment.SelectedItem?.ToString(), "Cash", StringComparison.OrdinalIgnoreCase))
            {
                HideTextBox();
                cboPayment.Enabled = true;
                cboPayment.BackColor = Color.White;
                txtBankName.Text = "";
                txtCVV.Text = "";
                txtNumBank.Text = "";
                txtNumCard.Text = "";
            }
            else
            {
                ShowTextBox();
            }
        }

        //Ẩn các chức năng khác khi chon phuong thuc thanh toan la Cash
        private void HideTextBoxForCash()
        {

        }

        private void Payment_Load(object sender, EventArgs e)
        {
            this.ControlBox = false;
            HideTextBoxForCash();
        }

        bool isEditMode = true;
        bool UpdSucc = false;
        private void btnFix_Click(object sender, EventArgs e)
        {
            if (isEditMode)
            {
                // chuyển qua chức năng lưu và sữa
                ShowTextBox();
                isEditMode = false;
                btnFix.Text = "     Lưu";

                btnFix.Image = Image.FromFile(@"H:\DaiHoc\QuanLyLinhKienMayTinh\Icon\diskette.png");
            }
            else
            {
                // chuyển qua chức năng xem và thực hiện lưu

                if(IDForUpdate == 1)
                {
                    UpdateOrInsertPaymentMethod(cusId);
                    if (UpdSucc)
                    {
                        HideTextBox();
                        isEditMode = true;
                        btnFix.Text = "     Sữa";
                        btnFix.Image = Image.FromFile(@"H:\DaiHoc\QuanLyLinhKienMayTinh\Icon\service.png");
                    }
                }
                else if(IDForUpdate == 2)
                {
                    UpdateOrInsertPaymentMethod(userId);
                    if (UpdSucc)
                    {
                        HideTextBox();
                        isEditMode = true;
                        btnFix.Text = "     Sữa";
                        btnFix.Image = Image.FromFile(@"H:\DaiHoc\QuanLyLinhKienMayTinh\Icon\service.png");
                    }
                }
                else if (IDForUpdate == 3)
                {
                    UpdateOrInsertPaymentMethod(empID);
                    if (UpdSucc)
                    {
                        HideTextBox();
                        isEditMode = true;
                        btnFix.Text = "     Sữa";
                        btnFix.Image = Image.FromFile(@"H:\DaiHoc\QuanLyLinhKienMayTinh\Icon\service.png");
                    }
                }
            }

        }

        //Update và insert vào bảng payment nếu đã tòn tại user id thi chi can update con chua thi se tao moi
        private void UpdateOrInsertPaymentMethod(int userId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(strCon))
                {
                    connection.Open();

                    // Kiểm tra xem user_id đã tồn tại trong bảng PaymentMethods hay chưa
                    string checkQuery = "SELECT COUNT(*) FROM PaymentMethods WHERE user_id = @userId";
                    SqlCommand checkCommand = new SqlCommand(checkQuery, connection);
                    checkCommand.Parameters.AddWithValue("@userId", userId);
                    int count = (int)checkCommand.ExecuteScalar();

                    // Nếu user_id chưa tồn tại trong bảng PaymentMethods, thêm mới phương thức thanh toán
                    if (count == 0)
                    {
                        string insertQuery = "INSERT INTO PaymentMethods (user_id, method_type, card_number, expiry_date, cvv, bank_name, bank_account_number) VALUES (@userId, @methodType, @cardNumber, @expiryDate, @cvv, @bankName, @bankAccountNumber)";
                        SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                        insertCommand.Parameters.AddWithValue("@userId", userId);
                        insertCommand.Parameters.AddWithValue("@methodType", cboPayment.SelectedItem.ToString());
                        if(cboPayment.SelectedItem == null)
                        {
                            cboPayment.SelectedItem = "Cash";
                        }
                        insertCommand.Parameters.AddWithValue("@cardNumber", txtNumCard.Text);
                        insertCommand.Parameters.AddWithValue("@expiryDate", dtpExpire.Value);
                        insertCommand.Parameters.AddWithValue("@cvv", txtCVV.Text);
                        insertCommand.Parameters.AddWithValue("@bankName", txtBankName.Text);
                        insertCommand.Parameters.AddWithValue("@bankAccountNumber", txtNumBank.Text);

                        int rowsAffected = insertCommand.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Thêm mới phương thức thanh toán thành công.");
                            UpdSucc = true;
                        }
                        else
                        {
                            MessageBox.Show("Thêm mới phương thức thanh toán thất bại.");
                        }
                    }
                    else
                    {
                        // Nếu user_id đã tồn tại trong bảng PaymentMethods, cập nhật thông tin phương thức thanh toán
                        string updateQuery = "UPDATE PaymentMethods SET method_type = @methodType, card_number = @cardNumber, expiry_date = @expiryDate, " +
                            "cvv = @cvv, bank_name = @bankName, bank_account_number = @bankAccountNumber WHERE user_id = @userId";
                        SqlCommand updateCommand = new SqlCommand(updateQuery, connection);
                        updateCommand.Parameters.AddWithValue("@userId", userId);
                        updateCommand.Parameters.AddWithValue("@methodType", cboPayment.SelectedItem.ToString());
                        updateCommand.Parameters.AddWithValue("@cardNumber", txtNumCard.Text);
                        updateCommand.Parameters.AddWithValue("@expiryDate", dtpExpire.Value);
                        updateCommand.Parameters.AddWithValue("@cvv", txtCVV.Text);
                        updateCommand.Parameters.AddWithValue("@bankName", txtBankName.Text);
                        updateCommand.Parameters.AddWithValue("@bankAccountNumber", txtNumBank.Text);

                        int rowsAffected = updateCommand.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Cập nhật phương thức thanh toán thành công.");
                            UpdSucc = true;
                        }
                        else
                        {
                            MessageBox.Show("Cập nhật phương thức thanh toán thất bại.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void ShowTextBox()
        {
            // Hiển thị các ô TextBox và cho phép chỉnh sửa
            cboPayment.Enabled = true;
            txtNumCard.Enabled = true;
            dtpExpire.Enabled = true;
            txtCVV.Enabled = true;
            txtBankName.Enabled = true;
            txtNumBank.Enabled = true;

            // Hiển thị viền cho các ô TextBox
            txtNumCard.BorderStyle = BorderStyle.FixedSingle;
            txtCVV.BorderStyle = BorderStyle.FixedSingle;
            txtBankName.BorderStyle = BorderStyle.FixedSingle;
            txtNumBank.BorderStyle = BorderStyle.FixedSingle;

            // Vô hiệu hóa textbox và đặt màu nền tương ứng
            cboPayment.BackColor = Color.White;
            txtNumCard.BackColor = Color.White;
            txtCVV.BackColor = Color.White;
            txtBankName.BackColor = Color.White;
            txtNumBank.BackColor = Color.White;
        }

        //chức năng load cho phuong thuc thanh toan
        private void LoadPaymentMethods(int userId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(strCon))
                {
                    connection.Open();

                    string query = "SELECT method_type, card_number, expiry_date, cvv, bank_name, bank_account_number FROM PaymentMethods WHERE user_id = @userId";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@userId", userId);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        cboPayment.SelectedItem = reader["method_type"].ToString();
                        txtNumCard.Text = reader["card_number"].ToString();
                        dtpExpire.Value = Convert.ToDateTime(reader["expiry_date"]);
                        txtCVV.Text = reader["cvv"].ToString();
                        txtBankName.Text = reader["bank_name"].ToString();
                        txtNumBank.Text = reader["bank_account_number"].ToString();
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading payment methods: " + ex.Message);
            }
        }

        private void HideTextBox()
        {
            // Ẩn các ô TextBox và vô hiệu hóa chỉnh sửa
            cboPayment.Enabled = false;
            txtNumCard.Enabled = false;
            dtpExpire.Enabled = false;
            txtCVV.Enabled = false;
            txtBankName.Enabled = false;
            txtNumBank.Enabled = false;

            // Ẩn viền cho các ô TextBox
            txtNumCard.BorderStyle = BorderStyle.None;
            txtCVV.BorderStyle = BorderStyle.None;
            txtBankName.BorderStyle = BorderStyle.None;
            txtNumBank.BorderStyle = BorderStyle.None;

            // Đặt lại màu nền của các ô TextBox thành màu mặc định
            cboPayment.BackColor = SystemColors.Control;
            txtNumCard.BackColor = SystemColors.Control;
            txtCVV.BackColor = SystemColors.Control;
            txtBankName.BackColor = SystemColors.Control;
            txtNumBank.BackColor = SystemColors.Control;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            CustomerID.userId = 0;
            EmployeeID.userId = 0;
            DanhSachKhachHang.CustomerInfo = false;
            DanhSachNhanVien.EmployeeInfo = false;
            this.Close();
        }

    }
}