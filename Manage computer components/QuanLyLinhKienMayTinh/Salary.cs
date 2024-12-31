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
    public partial class Salary : Form
    {
        string strCon = Connecting.GetConnectionString();
        int EmpID = EmployeeID.userId;
        int UserID = UserSession.UserId;
        string role;
        public Salary()
        {
            InitializeComponent();
            role = UserSession.role;
        }

        private void Salary_Load(object sender, EventArgs e)
        {
            HideTextBox();
            panel_fix.Visible = false;
            ID();
            CheckContract();
            Roler();
            Round.SetSharpCornerPanel(panel_fix);

        }


        //Kiểm tra id
        private void ID()
        {
            int employeeId = 0;

            if (EmpID != 0)
            {
                employeeId = GetEmployeeId(EmpID);
            }
            else if (UserID != 0)
            {
                employeeId = GetEmployeeId(UserID);
            }

            if (employeeId != 0)
            {
                LoadEmployeeSalaryData(employeeId);
            }
            else
            {
                MessageBox.Show("Không tìm thấy ID nhân viên trong bảng tiền lương", "lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //set quyền
        private void Roler()
        {
            int employeeId = GetEmployeeId(EmpID);

            if (role == "Admin")
            {
                panel_fix.Visible = true;
            }
        }

        //hàm hiện lại thất cả nội dung
        private void ShowAll()
        {
            label1.Visible = true;
            label2.Visible = true;
            label3.Visible = true;
            label4.Visible = true;

            txtBaseSalary.Visible = true;
            txtHourRate.Visible = true;
            txtMonthHour.Visible = true;
            txtCompensation.Visible = true;
        }

        private int GetEmployeeId(int userId)
        {
            int employeeId = 0;

            string query = "SELECT employee_id FROM Employees WHERE user_id = @userId";

            using (SqlConnection connection = new SqlConnection(strCon))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@userId", userId);
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        employeeId = Convert.ToInt32(reader["employee_id"]);
                    }
                }
            }

            return employeeId;
        }

        //kiểm tra xem nhân viên này có du lieu ve luong chua
        private bool EmployeeSalaryExists(int employeeId)
        {
            string query = "SELECT COUNT(*) FROM EmployeeSalaries WHERE employee_id = @employeeId";

            using (SqlConnection connection = new SqlConnection(strCon))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@employeeId", employeeId);
                    connection.Open();

                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        //lấy dữ liệu lương của nhân viên
        private void LoadEmployeeSalaryData(int employeeId)
        {
            string query = "SELECT contract_type, base_salary, hourly_rate, monthly_hours, internship_compensation FROM EmployeeSalaries WHERE employee_id = @employeeId";

            using (SqlConnection connection = new SqlConnection(strCon))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@employeeId", employeeId);
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        label10.Text = reader["contract_type"].ToString();
                        txtBaseSalary.Text = reader["base_salary"].ToString();
                        txtHourRate.Text = reader["hourly_rate"].ToString();
                        txtMonthHour.Text = reader["monthly_hours"].ToString();
                        txtCompensation.Text = reader["internship_compensation"].ToString();
                    }
                }
            }
        }

        private void InsertEmployeeSalaryData(int employeeId)
        {
            // Tạo câu lệnh SQL để chèn một bản ghi mới vào bảng EmployeeSalaries
            string query = @"INSERT INTO EmployeeSalaries (employee_id, contract_type, base_salary, hourly_rate, monthly_hours, internship_compensation) 
                     VALUES (@employeeId, @contractType, @baseSalary, @hourlyRate, @monthlyHours, @internshipCompensation)";

            using (SqlConnection connection = new SqlConnection(strCon))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Thiết lập các tham số cho câu lệnh SQL
                    command.Parameters.AddWithValue("@employeeId", employeeId);
                    command.Parameters.AddWithValue("@contractType", DBNull.Value); // Giá trị mặc định cho các trường dữ liệu mới
                    command.Parameters.AddWithValue("@baseSalary", DBNull.Value);
                    command.Parameters.AddWithValue("@hourlyRate", DBNull.Value);
                    command.Parameters.AddWithValue("@monthlyHours", DBNull.Value);
                    command.Parameters.AddWithValue("@internshipCompensation", DBNull.Value);

                    // Mở kết nối và thực thi câu lệnh SQL
                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        MessageBox.Show("Đã thêm thông tin lương cho nhân viên mới.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Đã xảy ra lỗi khi thêm thông tin lương cho nhân viên mới: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        //cập nhập lương nhân viên
        private void UpdateEmployeeSalaryData(int employeeId)
        {
            string query = @"UPDATE EmployeeSalaries 
                    SET base_salary = @baseSalary, 
                        hourly_rate = @hourlyRate, 
                        monthly_hours = @monthlyHours, 
                        internship_compensation = @internshipCompensation 
                    WHERE employee_id = @employeeId";

            using (SqlConnection connection = new SqlConnection(strCon))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@baseSalary", string.IsNullOrEmpty(txtBaseSalary.Text) ? (object)DBNull.Value : Convert.ToDecimal(txtBaseSalary.Text));
                    command.Parameters.AddWithValue("@hourlyRate", string.IsNullOrEmpty(txtHourRate.Text) ? (object)DBNull.Value : Convert.ToDecimal(txtHourRate.Text));
                    command.Parameters.AddWithValue("@monthlyHours", string.IsNullOrEmpty(txtMonthHour.Text) ? (object)DBNull.Value : Convert.ToInt32(txtMonthHour.Text));
                    command.Parameters.AddWithValue("@internshipCompensation", string.IsNullOrEmpty(txtCompensation.Text) ? (object)DBNull.Value : Convert.ToDecimal(txtCompensation.Text));
                    command.Parameters.Add("@employeeId", SqlDbType.Int).Value = employeeId;

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Dữ liệu lương của nhân viên đã được cập nhật thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Không có dữ liệu nào được cập nhật.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void lbContract_Click(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Bật lại các nút
        private void ShowTextBox()
        {
            label1.Visible = true;
            label2.Visible = true;
            label3.Visible = true;
            label4.Visible = true;

            txtBaseSalary.Visible = true;
            txtHourRate.Visible= true;
            txtMonthHour.Visible= true;
            txtCompensation.Visible= true;

            txtBaseSalary.Enabled = true;
            txtHourRate.Enabled = true;
            txtMonthHour.Enabled = true;
            txtCompensation.Enabled = true;
        }

        //Vô hiểu hóa nút
        private void HideTextBox()
        {
            txtBaseSalary.Enabled = false;
            txtHourRate.Enabled = false;
            txtMonthHour.Enabled = false;
            txtCompensation.Enabled = false;
        }

        //kiểm tra loại hợp đồng và ẩn
        private void CheckContract()
        {
            //nếu loại hợp đồng là full time thì chỉ có thể thấy tiền lương cơ bản và trợ cấp
            if (label10.Text == "Full-time")
            {
                label2.Visible = false;
                label3.Visible = false;

                txtBaseSalary.Enabled = false;
                txtCompensation.Enabled = false;

                txtHourRate.Visible = false;
                txtMonthHour.Visible = false;
            }//nếu loại hợp đồng là part time thì không thấy được lương cơ bản
            else if (label10.Text == "Part-time")
            {
                label1.Visible = false;

                txtHourRate.Enabled = false;
                txtMonthHour.Enabled = false;
                txtCompensation.Enabled = false;

                txtBaseSalary.Visible = false;

            }//nếu loại hợp đồng là Internship thì không thể thấy lương cơ bản và lương theo giờ
            else if (label10.Text == "Internship")
            {
                label1.Visible = false;
                label3.Visible = false;

                txtCompensation.Enabled = false;

                txtBaseSalary.Visible = false;
                txtMonthHour.Visible = false;
            }
        }

        bool isEditMode = true;
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
                int employeeId = 0;

                if (EmpID != 0)
                {
                    employeeId = GetEmployeeId(EmpID);
                }
                else if (UserID != 0)
                {
                    employeeId = GetEmployeeId(UserID);
                }
                // Kiểm tra xem employeeId có tồn tại trong bảng EmployeeSalaries hay không
                if (!EmployeeSalaryExists(employeeId))
                {
                    insertpayment();
                }
                else
                {
                    UpdateEmployeeSalaryData(employeeId);
                }

                // chuyển qua chức năng xem và thực hiện lưu
                HideTextBox();
                isEditMode = true;
                btnFix.Text = "     Sữa";
                btnFix.Image = Image.FromFile(@"H:\DaiHoc\QuanLyLinhKienMayTinh\Icon\service.png");
            }
        }

        //hàm lưu khi không có trong SQL
        private void insertpayment()
        {
            int employeeId = 0;

            if (EmpID != 0)
            {
                employeeId = GetEmployeeId(EmpID);
            }
            else if (UserID != 0)
            {
                employeeId = GetEmployeeId(UserID);
            }

            if (role == "Admin")
            {
                // Kiểm tra xem employeeId có tồn tại trong bảng EmployeeSalaries hay không
                if (!EmployeeSalaryExists(employeeId))
                {
                    // Nếu không tồn tại, thực hiện chèn một bản ghi mới
                    InsertEmployeeSalaryData(employeeId);
                }
            }
        }
    }
}
