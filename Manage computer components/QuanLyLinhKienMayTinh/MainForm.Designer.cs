
namespace QuanLyLinhKienDIenTu
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.panel_top = new System.Windows.Forms.Panel();
            this.label_title = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.panel_body = new System.Windows.Forms.Panel();
            this.panel_form = new System.Windows.Forms.Panel();
            this.btnMore = new System.Windows.Forms.Button();
            this.btnMinimize = new System.Windows.Forms.Button();
            this.btnMaximize = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.menuTranstation = new System.Windows.Forms.Timer(this.components);
            this.sidebarTranstation = new System.Windows.Forms.Timer(this.components);
            this.menuTranstation2 = new System.Windows.Forms.Timer(this.components);
            this.panel_left = new System.Windows.Forms.Panel();
            this.panelOrder = new System.Windows.Forms.Panel();
            this.btnOrder = new System.Windows.Forms.Button();
            this.panel_Product = new System.Windows.Forms.Panel();
            this.btnProduct = new System.Windows.Forms.Button();
            this.flowLayoutPanelEmp = new System.Windows.Forms.FlowLayoutPanel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.btnEmployee = new System.Windows.Forms.Button();
            this.panel6 = new System.Windows.Forms.Panel();
            this.btnRoom = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnLogOut = new System.Windows.Forms.Button();
            this.panelCus = new System.Windows.Forms.Panel();
            this.btnKhachHang = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel_top.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.panel_form.SuspendLayout();
            this.panel_left.SuspendLayout();
            this.panelOrder.SuspendLayout();
            this.panel_Product.SuspendLayout();
            this.flowLayoutPanelEmp.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panelCus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel_top
            // 
            this.panel_top.Controls.Add(this.label_title);
            this.panel_top.Controls.Add(this.pictureBox2);
            this.panel_top.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_top.Location = new System.Drawing.Point(267, 37);
            this.panel_top.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel_top.Name = "panel_top";
            this.panel_top.Size = new System.Drawing.Size(1297, 123);
            this.panel_top.TabIndex = 5;
            // 
            // label_title
            // 
            this.label_title.AutoSize = true;
            this.label_title.BackColor = System.Drawing.Color.Transparent;
            this.label_title.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.label_title.Font = new System.Drawing.Font("Times New Roman", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_title.ForeColor = System.Drawing.Color.Black;
            this.label_title.Image = global::QuanLyLinhKienDIenTu.Properties.Resources.tech1;
            this.label_title.Location = new System.Drawing.Point(968, 53);
            this.label_title.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_title.Name = "label_title";
            this.label_title.Size = new System.Drawing.Size(193, 42);
            this.label_title.TabIndex = 0;
            this.label_title.Text = "Trang Chủ";
            this.label_title.Click += new System.EventHandler(this.label_title_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox2.Image = global::QuanLyLinhKienDIenTu.Properties.Resources.tech_polygonal_background;
            this.pictureBox2.Location = new System.Drawing.Point(0, 0);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(1297, 123);
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // panel_body
            // 
            this.panel_body.BackColor = System.Drawing.Color.White;
            this.panel_body.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_body.Location = new System.Drawing.Point(267, 160);
            this.panel_body.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel_body.Name = "panel_body";
            this.panel_body.Size = new System.Drawing.Size(1297, 672);
            this.panel_body.TabIndex = 6;
            this.panel_body.Paint += new System.Windows.Forms.PaintEventHandler(this.panel_body_Paint);
            // 
            // panel_form
            // 
            this.panel_form.BackColor = System.Drawing.Color.Silver;
            this.panel_form.Controls.Add(this.btnMore);
            this.panel_form.Controls.Add(this.btnMinimize);
            this.panel_form.Controls.Add(this.btnMaximize);
            this.panel_form.Controls.Add(this.btnClose);
            this.panel_form.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_form.Location = new System.Drawing.Point(0, 0);
            this.panel_form.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel_form.Name = "panel_form";
            this.panel_form.Size = new System.Drawing.Size(1564, 37);
            this.panel_form.TabIndex = 7;
            this.panel_form.Paint += new System.Windows.Forms.PaintEventHandler(this.panel_form_Paint);
            // 
            // btnMore
            // 
            this.btnMore.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnMore.Image = global::QuanLyLinhKienDIenTu.Properties.Resources.more;
            this.btnMore.Location = new System.Drawing.Point(0, 0);
            this.btnMore.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnMore.Name = "btnMore";
            this.btnMore.Size = new System.Drawing.Size(40, 37);
            this.btnMore.TabIndex = 3;
            this.btnMore.UseVisualStyleBackColor = true;
            this.btnMore.Click += new System.EventHandler(this.btnMore_Click);
            // 
            // btnMinimize
            // 
            this.btnMinimize.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnMinimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinimize.Image = global::QuanLyLinhKienDIenTu.Properties.Resources.minimize_sign;
            this.btnMinimize.Location = new System.Drawing.Point(1444, 0);
            this.btnMinimize.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.Size = new System.Drawing.Size(40, 37);
            this.btnMinimize.TabIndex = 2;
            this.btnMinimize.UseVisualStyleBackColor = true;
            this.btnMinimize.Click += new System.EventHandler(this.btnMinimize_Click);
            // 
            // btnMaximize
            // 
            this.btnMaximize.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnMaximize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMaximize.Image = global::QuanLyLinhKienDIenTu.Properties.Resources.maximise;
            this.btnMaximize.Location = new System.Drawing.Point(1484, 0);
            this.btnMaximize.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnMaximize.Name = "btnMaximize";
            this.btnMaximize.Size = new System.Drawing.Size(40, 37);
            this.btnMaximize.TabIndex = 1;
            this.btnMaximize.UseVisualStyleBackColor = true;
            this.btnMaximize.Click += new System.EventHandler(this.btnMaximize_Click);
            // 
            // btnClose
            // 
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Image = global::QuanLyLinhKienDIenTu.Properties.Resources.close;
            this.btnClose.Location = new System.Drawing.Point(1524, 0);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(40, 37);
            this.btnClose.TabIndex = 0;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.button4_Click);
            // 
            // sidebarTranstation
            // 
            this.sidebarTranstation.Interval = 10;
            this.sidebarTranstation.Tick += new System.EventHandler(this.sidebarTranstation_Tick);
            // 
            // menuTranstation2
            // 
            this.menuTranstation2.Interval = 10;
            this.menuTranstation2.Tick += new System.EventHandler(this.menuTranslation2_Tick);
            // 
            // panel_left
            // 
            this.panel_left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(24)))), ((int)(((byte)(29)))));
            this.panel_left.BackgroundImage = global::QuanLyLinhKienDIenTu.Properties.Resources.wood_1759566_1280;
            this.panel_left.Controls.Add(this.panelOrder);
            this.panel_left.Controls.Add(this.panel_Product);
            this.panel_left.Controls.Add(this.flowLayoutPanelEmp);
            this.panel_left.Controls.Add(this.panel3);
            this.panel_left.Controls.Add(this.panelCus);
            this.panel_left.Controls.Add(this.pictureBox1);
            this.panel_left.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel_left.Location = new System.Drawing.Point(0, 37);
            this.panel_left.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel_left.Name = "panel_left";
            this.panel_left.Size = new System.Drawing.Size(267, 795);
            this.panel_left.TabIndex = 4;
            this.panel_left.Paint += new System.Windows.Forms.PaintEventHandler(this.panel_left_Paint);
            // 
            // panelOrder
            // 
            this.panelOrder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(24)))), ((int)(((byte)(29)))));
            this.panelOrder.Controls.Add(this.btnOrder);
            this.panelOrder.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelOrder.Location = new System.Drawing.Point(0, 331);
            this.panelOrder.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelOrder.Name = "panelOrder";
            this.panelOrder.Size = new System.Drawing.Size(267, 49);
            this.panelOrder.TabIndex = 4;
            // 
            // btnOrder
            // 
            this.btnOrder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(24)))), ((int)(((byte)(29)))));
            this.btnOrder.BackgroundImage = global::QuanLyLinhKienDIenTu.Properties.Resources.wood_1759566_1280;
            this.btnOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOrder.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOrder.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnOrder.Image = global::QuanLyLinhKienDIenTu.Properties.Resources.checklist;
            this.btnOrder.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOrder.Location = new System.Drawing.Point(-3, -25);
            this.btnOrder.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnOrder.Name = "btnOrder";
            this.btnOrder.Size = new System.Drawing.Size(315, 100);
            this.btnOrder.TabIndex = 1;
            this.btnOrder.Text = "         Đơn hàng";
            this.btnOrder.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOrder.UseVisualStyleBackColor = false;
            this.btnOrder.Click += new System.EventHandler(this.button4_Click_2);
            // 
            // panel_Product
            // 
            this.panel_Product.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(24)))), ((int)(((byte)(29)))));
            this.panel_Product.Controls.Add(this.btnProduct);
            this.panel_Product.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_Product.Location = new System.Drawing.Point(0, 282);
            this.panel_Product.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel_Product.Name = "panel_Product";
            this.panel_Product.Size = new System.Drawing.Size(267, 49);
            this.panel_Product.TabIndex = 1;
            // 
            // btnProduct
            // 
            this.btnProduct.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(24)))), ((int)(((byte)(29)))));
            this.btnProduct.BackgroundImage = global::QuanLyLinhKienDIenTu.Properties.Resources.wood_1759566_1280;
            this.btnProduct.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProduct.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProduct.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnProduct.Image = global::QuanLyLinhKienDIenTu.Properties.Resources.products;
            this.btnProduct.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnProduct.Location = new System.Drawing.Point(-3, -25);
            this.btnProduct.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnProduct.Name = "btnProduct";
            this.btnProduct.Size = new System.Drawing.Size(315, 100);
            this.btnProduct.TabIndex = 1;
            this.btnProduct.Text = "         Sản Phẩm";
            this.btnProduct.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnProduct.UseVisualStyleBackColor = false;
            this.btnProduct.Click += new System.EventHandler(this.button4_Click_1);
            // 
            // flowLayoutPanelEmp
            // 
            this.flowLayoutPanelEmp.BackgroundImage = global::QuanLyLinhKienDIenTu.Properties.Resources.wood_1759566_1280;
            this.flowLayoutPanelEmp.Controls.Add(this.panel5);
            this.flowLayoutPanelEmp.Controls.Add(this.panel6);
            this.flowLayoutPanelEmp.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanelEmp.Location = new System.Drawing.Point(0, 233);
            this.flowLayoutPanelEmp.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.flowLayoutPanelEmp.Name = "flowLayoutPanelEmp";
            this.flowLayoutPanelEmp.Size = new System.Drawing.Size(267, 49);
            this.flowLayoutPanelEmp.TabIndex = 3;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(24)))), ((int)(((byte)(29)))));
            this.panel5.Controls.Add(this.btnEmployee);
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Margin = new System.Windows.Forms.Padding(0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(267, 49);
            this.panel5.TabIndex = 2;
            // 
            // btnEmployee
            // 
            this.btnEmployee.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(24)))), ((int)(((byte)(29)))));
            this.btnEmployee.BackgroundImage = global::QuanLyLinhKienDIenTu.Properties.Resources.wood_1759566_1280;
            this.btnEmployee.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEmployee.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEmployee.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnEmployee.Image = global::QuanLyLinhKienDIenTu.Properties.Resources.division;
            this.btnEmployee.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEmployee.Location = new System.Drawing.Point(-3, -25);
            this.btnEmployee.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnEmployee.Name = "btnEmployee";
            this.btnEmployee.Size = new System.Drawing.Size(315, 98);
            this.btnEmployee.TabIndex = 1;
            this.btnEmployee.Text = "         Nhân Viên";
            this.btnEmployee.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEmployee.UseVisualStyleBackColor = false;
            this.btnEmployee.Click += new System.EventHandler(this.btnEmployee_Click);
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(24)))), ((int)(((byte)(29)))));
            this.panel6.Controls.Add(this.btnRoom);
            this.panel6.Location = new System.Drawing.Point(13, 49);
            this.panel6.Margin = new System.Windows.Forms.Padding(13, 0, 0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(267, 49);
            this.panel6.TabIndex = 4;
            // 
            // btnRoom
            // 
            this.btnRoom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(24)))), ((int)(((byte)(29)))));
            this.btnRoom.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRoom.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRoom.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnRoom.Image = global::QuanLyLinhKienDIenTu.Properties.Resources.division;
            this.btnRoom.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRoom.Location = new System.Drawing.Point(-1, -25);
            this.btnRoom.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnRoom.Name = "btnRoom";
            this.btnRoom.Size = new System.Drawing.Size(315, 100);
            this.btnRoom.TabIndex = 1;
            this.btnRoom.Text = "         Phòng ban";
            this.btnRoom.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRoom.UseVisualStyleBackColor = false;
            this.btnRoom.Click += new System.EventHandler(this.btnRoom_Click);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(24)))), ((int)(((byte)(29)))));
            this.panel3.Controls.Add(this.btnLogOut);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 746);
            this.panel3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(267, 49);
            this.panel3.TabIndex = 2;
            // 
            // btnLogOut
            // 
            this.btnLogOut.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(24)))), ((int)(((byte)(29)))));
            this.btnLogOut.BackgroundImage = global::QuanLyLinhKienDIenTu.Properties.Resources.wood_1759566_1280;
            this.btnLogOut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogOut.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogOut.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnLogOut.Image = global::QuanLyLinhKienDIenTu.Properties.Resources.logout1;
            this.btnLogOut.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLogOut.Location = new System.Drawing.Point(-1, -25);
            this.btnLogOut.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnLogOut.Name = "btnLogOut";
            this.btnLogOut.Size = new System.Drawing.Size(315, 100);
            this.btnLogOut.TabIndex = 1;
            this.btnLogOut.Text = "         Đăng Xuất";
            this.btnLogOut.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLogOut.UseVisualStyleBackColor = false;
            this.btnLogOut.Click += new System.EventHandler(this.btnLogOut_Click);
            // 
            // panelCus
            // 
            this.panelCus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(24)))), ((int)(((byte)(29)))));
            this.panelCus.Controls.Add(this.btnKhachHang);
            this.panelCus.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelCus.Location = new System.Drawing.Point(0, 184);
            this.panelCus.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelCus.Name = "panelCus";
            this.panelCus.Size = new System.Drawing.Size(267, 49);
            this.panelCus.TabIndex = 0;
            // 
            // btnKhachHang
            // 
            this.btnKhachHang.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(24)))), ((int)(((byte)(29)))));
            this.btnKhachHang.BackgroundImage = global::QuanLyLinhKienDIenTu.Properties.Resources.wood_1759566_1280;
            this.btnKhachHang.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKhachHang.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnKhachHang.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnKhachHang.Image = global::QuanLyLinhKienDIenTu.Properties.Resources.customer__1_;
            this.btnKhachHang.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnKhachHang.Location = new System.Drawing.Point(-3, -25);
            this.btnKhachHang.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnKhachHang.Name = "btnKhachHang";
            this.btnKhachHang.Size = new System.Drawing.Size(315, 100);
            this.btnKhachHang.TabIndex = 1;
            this.btnKhachHang.Text = "         Khách Hàng";
            this.btnKhachHang.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnKhachHang.UseVisualStyleBackColor = false;
            this.btnKhachHang.Click += new System.EventHandler(this.btnKhachHang_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Gray;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureBox1.ErrorImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.ErrorImage")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(267, 184);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(1564, 832);
            this.Controls.Add(this.panel_body);
            this.Controls.Add(this.panel_top);
            this.Controls.Add(this.panel_left);
            this.Controls.Add(this.panel_form);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.IsMdiContainer = true;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "MainForm";
            this.Text = "Main";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel_top.ResumeLayout(false);
            this.panel_top.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.panel_form.ResumeLayout(false);
            this.panel_left.ResumeLayout(false);
            this.panelOrder.ResumeLayout(false);
            this.panel_Product.ResumeLayout(false);
            this.flowLayoutPanelEmp.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panelCus.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel_left;
        private System.Windows.Forms.Panel panel_top;
        private System.Windows.Forms.Panel panel_body;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label_title;
        private System.Windows.Forms.Button btnKhachHang;
        private System.Windows.Forms.Panel panel_form;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button btnMinimize;
        private System.Windows.Forms.Button btnMaximize;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnMore;
        private System.Windows.Forms.Panel panelCus;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnLogOut;
        private System.Windows.Forms.Timer menuTranstation;
        private System.Windows.Forms.Timer sidebarTranstation;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Button btnEmployee;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelEmp;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Button btnRoom;
        private System.Windows.Forms.Timer menuTranstation2;
        private System.Windows.Forms.Panel panel_Product;
        private System.Windows.Forms.Button btnProduct;
        private System.Windows.Forms.Panel panelOrder;
        private System.Windows.Forms.Button btnOrder;
    }
}

