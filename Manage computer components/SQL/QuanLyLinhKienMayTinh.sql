--TẠO CSDL (KIỂM TRA TỒN TẠI NẾU CÓ SẼ XÓA CSDL CŨ
USE master
GO

ALTER DATABASE QuanLyLinhKienMayTinh SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
GO

--Tạo lệnh tạo CSQL
IF EXISTS (SELECT NAME FROM SYS.DATABASES WHERE NAME='QuanLyLinhKienMayTinh')
	DROP DATABASE QuanLyLinhKienMayTinh
GO


CREATE DATABASE QuanLyLinhKienMayTinh
ON(NAME='QuanLyLinhKienMayTinh_DATA',FILENAME='H:\DaiHoc\QuanLyLinhKienMayTinh\SQL\QuanLyLinhKienMayTinh.MDF')
LOG ON(NAME='QuanLyLinhKienMayTinh_LOG',FILENAME='H:\DaiHoc\QuanLyLinhKienMayTinh\SQL\QuanLyLinhKienMayTinh.LDF')
GO

USE [QuanLyLinhKienMayTinh]
GO

--Xuất file .bak
BACKUP DATABASE QuanLyLinhKienMayTinh TO DISK = 'H:\DaiHoc\QuanLyLinhKienMayTinh\SQL\QuanLyLinhKienMayTinh.bak';

--Nhập file .bak
RESTORE DATABASE QuanLyLinhKienMayTinh
FROM DISK = 'H:\DaiHoc\QuanLyLinhKienMayTinh\SQL\QuanLyLinhKienMayTinh.bak'
WITH REPLACE;
GO

-- Table User bảng người dùng(Users)
CREATE TABLE Users (
    user_id INT PRIMARY KEY IDENTITY,
    username VARCHAR(100) UNIQUE,
    password VARCHAR(255),
    role NVARCHAR(10) NOT NULL CHECK (role IN ('Admin', 'Manager', 'Employee', 'Customer', 'Guest', 'Owner'))
);
GO

-- Bảng loại hợp đồng
CREATE TABLE Contracts (
    contract_type VARCHAR(50) PRIMARY KEY,
    start_date DATE NOT NULL,
    end_date DATE,
    contract_description NVARCHAR(255),
    CONSTRAINT CHK_Contract_Type CHECK (contract_type IN ('Full-time', 'Part-time', 'Internship', 'Fixed-term', 'Temporary', 'Freelance'))
);
GO

ALTER TABLE Contracts
DROP CONSTRAINT CHK_Contract_Type;

-- Bảng Nhân Viên(Employees)
CREATE TABLE Employees (
    employee_id INT PRIMARY KEY IDENTITY,
    user_id INT,
    full_name VARCHAR(255),
    date_of_birth DATE,
    gender NVARCHAR(10),
    email VARCHAR(255),
    phone_number VARCHAR(20),
	address NVARCHAR(255),
    hire_date DATE,
    salary DECIMAL(18, 2),
    contract_type VARCHAR(50), -- Loại hợp đồng
	profile_image varbinary(max),
    FOREIGN KEY (user_id) REFERENCES Users(user_id),
	FOREIGN KEY (contract_type) REFERENCES Contracts(contract_type),
	CONSTRAINT CHK_Salary CHECK (salary >= 0)
);
GO

EXEC sp_helpconstraint 'Employees';

-- Bảng Phòng ban (Departments)
CREATE TABLE Departments (
    department_id INT PRIMARY KEY IDENTITY,
    department_name NVARCHAR(100) UNIQUE,
    department_head_id INT, -- ID của người đứng đầu phòng ban
    department_description NVARCHAR(255),
    FOREIGN KEY (department_head_id) REFERENCES Employees(employee_id)
);
GO

-- Bảng tiền lương của nhân viên
CREATE TABLE EmployeeSalaries (
    salary_id INT PRIMARY KEY IDENTITY,
    employee_id INT,
    contract_type VARCHAR(50),
    base_salary DECIMAL (18,2), -- mức lương cơ bản(full-time)
    hourly_rate DECIMAL (18,2), -- tỷ lệ lương theo giờ (cho part-time)
    monthly_hours INT, -- số giờ làm việc hàng tháng (cho part-time)
    internship_compensation DECIMAL (18,2), -- mức trợ cấp hoặc mức lương thực tập(internship)
	CONSTRAINT CHK_MonthlyHours CHECK (monthly_hours >= 0),
    CONSTRAINT FK_SalaryDetails_EmployeeID FOREIGN KEY (employee_id) REFERENCES Employees(employee_id)
);
GO


-- Bảng Liên kết Nhân viên - Phòng ban (Employee_Department_Assignment)
CREATE TABLE Employee_Department_Assignment (
    assignment_id INT PRIMARY KEY IDENTITY,
    employee_id INT, -- Khóa ngoại tham chiếu đến bảng Employees
    department_id INT, -- Khóa ngoại tham chiếu đến bảng Departments
    start_date DATE NOT NULL,
    end_date DATE,
    CONSTRAINT FK_Employee_Assignment_Employee FOREIGN KEY (employee_id) REFERENCES Employees(employee_id),
    CONSTRAINT FK_Employee_Assignment_Department FOREIGN KEY (department_id) REFERENCES Departments(department_id),
    CONSTRAINT CHK_Assignment_Dates CHECK (start_date <= end_date) -- Đảm bảo ngày bắt đầu không lớn hơn ngày kết thúc
);
GO

-- Bảng Khách hàng
CREATE TABLE Customers (
    customer_id INT PRIMARY KEY IDENTITY,
	user_id INT,
    full_name VARCHAR(255),
	date_of_birth DATE,
    address NVARCHAR(255),
    email VARCHAR(255) UNIQUE,
    phone_number VARCHAR(20) UNIQUE,
	gender NVARCHAR(10),
	profile_image varbinary(max),
	FOREIGN KEY (user_id) REFERENCES Users(user_id)
);
GO

EXEC sp_helpconstraint 'Customers';


-- Phương thức thanh toán của khách hàng
CREATE TABLE PaymentMethods (
    payment_method_id INT PRIMARY KEY IDENTITY,
	user_id INT,
    method_type VARCHAR(50), -- Loại phương thức thanh toán như "Credit Card", "Debit Card", "Cash" hoặc "Bank Transfer".
    card_number VARCHAR(20),
    expiry_date DATE,
    cvv VARCHAR(5), -- Mã bảo mật của thẻ.
	bank_name NVARCHAR(100),
    bank_account_number VARCHAR(50),
    FOREIGN KEY (user_id) REFERENCES Users(user_id)
);
GO


--cách tạo phương thức unique nhưng vẫn chấp nhận null bằng cách bỏ qua kiểm tra null
CREATE UNIQUE INDEX idx_bank_account_number
ON PaymentMethods (bank_account_number)
WHERE bank_account_number IS NOT NULL;

-- Bảng Nhà cung cấp
CREATE TABLE Suppliers (
    supplier_id INT PRIMARY KEY IDENTITY,
    supplier_name VARCHAR(255),
    address VARCHAR(255),
    email VARCHAR(255),
    phone_number VARCHAR(20)
);
GO

--SELECT * FROM CPU

-- Bảng Loại linh kiện
CREATE TABLE Component_Types (
    component_type_id INT PRIMARY KEY IDENTITY,
    component_type_name VARCHAR(100)
);
GO

-- Tạo bảng CPU
CREATE TABLE CPU (
    cpu_id VARCHAR(100) PRIMARY KEY,
    cpu_name VARCHAR(255),
    brand VARCHAR(255),
    model VARCHAR(255),
    speed VARCHAR(100),
    socket_type VARCHAR(50),
    price DECIMAL(18, 2),
    component_type_id INT,
    supplier_id INT,
	image VARCHAR(255),
    FOREIGN KEY (component_type_id) REFERENCES Component_Types(component_type_id),
    FOREIGN KEY (supplier_id) REFERENCES Suppliers(supplier_id)
);
GO

-- Tạo bảng RAM
CREATE TABLE RAM (
    ram_id VARCHAR(100) PRIMARY KEY,
	ram_name VARCHAR(255),
    brand VARCHAR(255),
	model VARCHAR(255),
    capacity VARCHAR(100),
	speed VARCHAR(100),
    type VARCHAR(50),
    price DECIMAL(18, 2),
    component_type_id INT,
    supplier_id INT,
	image VARCHAR(255),
    FOREIGN KEY (component_type_id) REFERENCES Component_Types(component_type_id),
    FOREIGN KEY (supplier_id) REFERENCES Suppliers(supplier_id)
);
GO

-- Tạo bảng Ổ cứng HDD
CREATE TABLE HDD (
    hdd_id VARCHAR(100) PRIMARY KEY,
    hdd_name VARCHAR(255),
    brand VARCHAR(255),
	model VARCHAR(255),
    capacity VARCHAR(100),
    interface_type VARCHAR(50),
    price DECIMAL(18, 2),
    component_type_id INT,
    supplier_id INT,
	image VARCHAR(255),
    FOREIGN KEY (component_type_id) REFERENCES Component_Types(component_type_id),
    FOREIGN KEY (supplier_id) REFERENCES Suppliers(supplier_id)
);
GO

-- Tạo bảng Ổ cứng SSD
CREATE TABLE SSD (
    ssd_id VARCHAR(100) PRIMARY KEY,
    ssd_name VARCHAR(255),
    brand VARCHAR(255),
	model VARCHAR(255),
    capacity VARCHAR(50),
    interface_type VARCHAR(50),
    price DECIMAL(18, 2),
    component_type_id INT,
    supplier_id INT,
	image VARCHAR(255),
    FOREIGN KEY (component_type_id) REFERENCES Component_Types(component_type_id),
    FOREIGN KEY (supplier_id) REFERENCES Suppliers(supplier_id)
);
GO

-- Tạo bảng Card đồ họa (GPU)
CREATE TABLE GPU (
    gpu_id VARCHAR(100) PRIMARY KEY,
    gpu_name VARCHAR(255),
    brand VARCHAR(255),
    model VARCHAR(255),
    vram VARCHAR(100),
    interface_type VARCHAR(50),
    price DECIMAL(18, 2),
    component_type_id INT,
    supplier_id INT,
	image VARCHAR(255),
    FOREIGN KEY (component_type_id) REFERENCES Component_Types(component_type_id),
    FOREIGN KEY (supplier_id) REFERENCES Suppliers(supplier_id)
);
GO

-- Tạo bảng Mainboard
CREATE TABLE Motherboard (
    motherboard_id VARCHAR(100) PRIMARY KEY,
    motherboard_name VARCHAR(255),
    brand VARCHAR(255),
    model VARCHAR(255),
    socket_type VARCHAR(50),
    max_ram_capacity INT,
    price DECIMAL(18, 2),
    component_type_id INT,
    supplier_id INT,
	image VARCHAR(255),
    FOREIGN KEY (component_type_id) REFERENCES Component_Types(component_type_id),
    FOREIGN KEY (supplier_id) REFERENCES Suppliers(supplier_id)
);
GO

-- Tạo bảng quạt tản nhiệt
CREATE TABLE Cooling (
    cooling_id VARCHAR(100) PRIMARY KEY,
    cooling_name VARCHAR(100)NOT NULL,
	brand VARCHAR(255),
	model VARCHAR(255),
    price DECIMAL(18, 2),
	component_type_id INT,
    supplier_id INT,
	image VARCHAR(255),
	FOREIGN KEY (component_type_id) REFERENCES Component_Types(component_type_id),
    FOREIGN KEY (supplier_id) REFERENCES Suppliers(supplier_id)
);
GO

-- Tạo bảng Power Supply Unit - (Điện nguồn máy tính)
CREATE TABLE PSU (
    psu_id VARCHAR(100) PRIMARY KEY,
    psu_name VARCHAR(255),
    brand VARCHAR(255),
	model VARCHAR(255),
    wattage VARCHAR(100), --Công suất của nguồn
    efficiency_rating VARCHAR(50), --Xếp hạng hiệu suất của nguồn 
    price DECIMAL(18, 2),
    component_type_id INT,
    supplier_id INT,
    image VARCHAR(255),
    FOREIGN KEY (component_type_id) REFERENCES Component_Types(component_type_id),
    FOREIGN KEY (supplier_id) REFERENCES Suppliers(supplier_id)
);
GO

-- Tạo bảng Màn hình (Monitor)
CREATE TABLE Monitor (
    monitor_id VARCHAR(100) PRIMARY KEY,
    monitor_name VARCHAR(255),
    brand VARCHAR(255),
    model VARCHAR(255),
    size_inch VARCHAR(100),
    resolution VARCHAR(20),
    price DECIMAL(18, 2),
    component_type_id INT,
    supplier_id INT,
	image VARCHAR(255),
    FOREIGN KEY (component_type_id) REFERENCES Component_Types(component_type_id),
    FOREIGN KEY (supplier_id) REFERENCES Suppliers(supplier_id)
);
GO

-- Bảng phụ kiện (Accessories) Mouse ,keyboard, speaker, webcam, led, mouse pad, scanner....
CREATE TABLE Accessories (
    accessory_id VARCHAR(100) PRIMARY KEY,
    accessory_name VARCHAR(255),
    brand VARCHAR(255),
    model VARCHAR(255),
    price DECIMAL(18, 2),
    supplier_id INT,
    component_type_id INT, -- Loại phụ kiện, ví dụ: Chuột, Bàn phím, Loa, v.v.
    image VARCHAR(255),
	FOREIGN KEY (component_type_id) REFERENCES Component_Types(component_type_id),
    FOREIGN KEY (supplier_id) REFERENCES Suppliers(supplier_id)
);
GO

--Network Adapter (Bộ chuyển đổi mạng):gồm các loại card mạng hoặc các thiết bị mạng khác như bộ định tuyến (router) 
--hoặc bộ khuếch đại tín hiệu (range extender).
CREATE TABLE Network_Adapter (
    adapter_id VARCHAR(100) PRIMARY KEY,
    adapter_name VARCHAR(255),
    brand VARCHAR(255),
    model VARCHAR(255),
    component_type_id INT,
    price DECIMAL(18, 2),
    supplier_id INT,
    image VARCHAR(255),
    FOREIGN KEY (supplier_id) REFERENCES Suppliers(supplier_id)
);
GO

--Expansion Cards (Card mở rộng) các loại card như card âm thanh, card mạng, card PCIe mở rộng, và các loại card khác.
CREATE TABLE Expansion_Cards(
    card_id VARCHAR(100) PRIMARY KEY,
    card_name VARCHAR(255),
    brand VARCHAR(255),
    model VARCHAR(255),
    component_type_id INT,
    price DECIMAL(18, 2),
    supplier_id INT,
    image VARCHAR(255),
    FOREIGN KEY (supplier_id) REFERENCES Suppliers(supplier_id)
);
GO

--Power Backup (Bộ lưu điện) các thiết bị như UPS (Uninterruptible Power Supply) hoặc bộ lưu điện dự phòng.
CREATE TABLE Power_Backup (
    backup_id VARCHAR(100) PRIMARY KEY,
    backup_name VARCHAR(255),
    brand VARCHAR(255),
    model VARCHAR(255),
    component_type_id INT,
    price DECIMAL(18, 2),
    supplier_id INT,
    image VARCHAR(255),
    FOREIGN KEY (supplier_id) REFERENCES Suppliers(supplier_id)
);
GO

CREATE TABLE Computer_Case (
    case_id VARCHAR(100) PRIMARY KEY,
    case_name VARCHAR(255),
    brand VARCHAR(255),
    model VARCHAR(255),
    size VARCHAR(50),
    color VARCHAR(50),
    price DECIMAL(18, 2),
    component_type_id INT,
    supplier_id INT,
    image VARCHAR(255),
    FOREIGN KEY (component_type_id) REFERENCES Component_Types(component_type_id),
    FOREIGN KEY (supplier_id) REFERENCES Suppliers(supplier_id)
);
GO

-- Danh sách liên kết với các linh kiện
CREATE TABLE Products (
    product_id INT PRIMARY KEY IDENTITY,
    cpu_id VARCHAR(100),
    ram_id VARCHAR(100),
    hdd_id VARCHAR(100),
    ssd_id VARCHAR(100),
    gpu_id VARCHAR(100),
    motherboard_id VARCHAR(100),
    case_id VARCHAR(100),
	adapter_id VARCHAR(100),
	card_id VARCHAR(100),
	backup_id VARCHAR(100),
	accessory_id VARCHAR(100),
	psu_id VARCHAR(100),
    monitor_id VARCHAR(100),
    cooling_id VARCHAR(100),
	quantity int,
	discount DECIMAL(18, 2) default null,
    FOREIGN KEY (cpu_id) REFERENCES CPU(cpu_id),
    FOREIGN KEY (ram_id) REFERENCES RAM(ram_id),
    FOREIGN KEY (hdd_id) REFERENCES HDD(hdd_id),
    FOREIGN KEY (ssd_id) REFERENCES SSD(ssd_id),
    FOREIGN KEY (gpu_id) REFERENCES GPU(gpu_id),
    FOREIGN KEY (motherboard_id) REFERENCES Motherboard(motherboard_id),
    FOREIGN KEY (case_id) REFERENCES Computer_Case(case_id),
    FOREIGN KEY (accessory_id) REFERENCES Accessories(accessory_id),
	FOREIGN KEY (adapter_id) REFERENCES Network_Adapter(adapter_id),
	FOREIGN KEY (card_id) REFERENCES Expansion_Cards(card_id),
	FOREIGN KEY (backup_id) REFERENCES Power_Backup(backup_id),
	FOREIGN KEY (psu_id) REFERENCES PSU(psu_id)
	)
GO

-- Tạo bảng Máy tính
CREATE TABLE Computer (
    computer_id INT PRIMARY KEY IDENTITY,
    cpu_id VARCHAR(100),
    ram_id VARCHAR(100),
    hdd_id VARCHAR(100),
    ssd_id VARCHAR(100),
    gpu_id VARCHAR(100),
    motherboard_id VARCHAR(100),
    case_id VARCHAR(100),
    psu_id VARCHAR(100),
    cooling_id VARCHAR(100),
    adapter_id VARCHAR(100),
    card_id VARCHAR(100),
    backup_id VARCHAR(100),
    price DECIMAL(18, 2) NOT NULL,
    FOREIGN KEY (cpu_id) REFERENCES CPU(cpu_id),
    FOREIGN KEY (ram_id) REFERENCES RAM(ram_id),
    FOREIGN KEY (hdd_id) REFERENCES HDD(hdd_id),
    FOREIGN KEY (ssd_id) REFERENCES SSD(ssd_id),
    FOREIGN KEY (gpu_id) REFERENCES GPU(gpu_id),
    FOREIGN KEY (motherboard_id) REFERENCES Motherboard(motherboard_id),
    FOREIGN KEY (case_id) REFERENCES Computer_Case(case_id),
    FOREIGN KEY (psu_id) REFERENCES PSU(psu_id),
    FOREIGN KEY (cooling_id) REFERENCES Cooling(cooling_id),
    FOREIGN KEY (adapter_id) REFERENCES Network_Adapter(adapter_id),
    FOREIGN KEY (card_id) REFERENCES Expansion_Cards(card_id),
    FOREIGN KEY (backup_id) REFERENCES Power_Backup(backup_id)
);
GO

-- Tạo bảng Đơn hàng
CREATE TABLE Orders (
    order_id INT PRIMARY KEY IDENTITY,
    customer_id INT,
    order_date DATE,
    total_price DECIMAL(18, 2),
	Recipient_Name NVARCHAR(255),
	delivery_address VARCHAR(255),
	cancel_status NVARCHAR(50) DEFAULT N'Chưa hủy',
	payment_status NVARCHAR(50) DEFAULT N'Chưa thanh toán',
	accept_status NVARCHAR(50) DEFAULT N'Chưa xác nhận',
    FOREIGN KEY (customer_id) REFERENCES Customers(customer_id),
	CONSTRAINT CHK_OrderDate CHECK (order_date <= GETDATE())
);
GO

-- Tạo bảng Chi tiết đơn hàng
CREATE TABLE Order_Details (
    order_detail_id INT PRIMARY KEY IDENTITY,
    order_id INT,  -- Khóa ngoại tham chiếu đến bảng Orders
    product_id INT,  -- Khóa ngoại tham chiếu đến bảng Products
    quantity INT,
    unit_price DECIMAL(18, 2),  -- Giá của sản phẩm tại thời điểm đặt hàng
    discount DECIMAL(18, 2),  -- Phần trăm giảm giá áp dụng cho sản phẩm (nếu có)
	employee_id INT,
    CONSTRAINT FK_OrderDetails_OrderID FOREIGN KEY (order_id) REFERENCES Orders(order_id),
    CONSTRAINT FK_OrderDetails_ProductID FOREIGN KEY (product_id) REFERENCES Products(product_id),
	CONSTRAINT FK_OrderDetails_EmployeeID FOREIGN KEY (employee_id) REFERENCES Employees(employee_id)
);
GO


--Table giỏ hàng
CREATE TABLE ShoppingCart (
    cart_item_id INT PRIMARY KEY IDENTITY,
    customer_id INT,
    product_id INT,
    quantity INT,
    unit_price DECIMAL(18, 2),
    discount DECIMAL(18, 2),
    CONSTRAINT FK_ShoppingCart_CustomerID FOREIGN KEY (customer_id) REFERENCES Customers(customer_id),
    CONSTRAINT FK_ShoppingCart_ProductID FOREIGN KEY (product_id) REFERENCES Products(product_id)
);

-- Bỏ table này nhưng không thể drop được sẽ bị lỗi
-- Bảng Hoá đơn (Invoices)
CREATE TABLE Invoices (
    invoice_id INT PRIMARY KEY IDENTITY,
    order_id INT, -- Khóa ngoại tham chiếu đến bảng Orders
    customer_id INT, -- Khóa ngoại tham chiếu đến bảng Customers
    invoice_date DATE,
    total_price DECIMAL(18, 2),
    delivery_address VARCHAR(255), -- Địa chỉ giao hàng
    recipient_name VARCHAR(255), -- Tên người nhận
	payment_status VARCHAR(20) DEFAULT 'Chưa thanh toán',
    FOREIGN KEY (order_id) REFERENCES Orders(order_id),
    FOREIGN KEY (customer_id) REFERENCES Customers(customer_id)
);
GO


-- Tạo bảng Lịch sử giá (Price History)
CREATE TABLE Price_History (
    price_id INT PRIMARY KEY IDENTITY,
    cpu_id VARCHAR(100),
    ram_id VARCHAR(100),
    hdd_id VARCHAR(100),
    ssd_id VARCHAR(100),
    gpu_id VARCHAR(100),
    motherboard_id VARCHAR(100),
    case_id VARCHAR(100),
	adapter_id VARCHAR(100),
	card_id VARCHAR(100),
	backup_id VARCHAR(100),
	accessory_id VARCHAR(100),
	psu_id VARCHAR(100),
    monitor_id VARCHAR(100),
    cooling_id VARCHAR(100),
	quantity int,
    initial_price DECIMAL(18, 2),  -- Giá ban đầu của sản phẩm
    price DECIMAL(18, 2),  -- Giá hiện tại của sản phẩm
	changed_by NVARCHAR(100),
    change_date DATE,
    FOREIGN KEY (cpu_id) REFERENCES CPU(cpu_id),
    FOREIGN KEY (ram_id) REFERENCES RAM(ram_id),
    FOREIGN KEY (hdd_id) REFERENCES HDD(hdd_id),
    FOREIGN KEY (ssd_id) REFERENCES SSD(ssd_id),
    FOREIGN KEY (gpu_id) REFERENCES GPU(gpu_id),
    FOREIGN KEY (motherboard_id) REFERENCES Motherboard(motherboard_id),
    FOREIGN KEY (case_id) REFERENCES Computer_Case(case_id),
    FOREIGN KEY (accessory_id) REFERENCES Accessories(accessory_id),
	FOREIGN KEY (adapter_id) REFERENCES Network_Adapter(adapter_id),
	FOREIGN KEY (card_id) REFERENCES Expansion_Cards(card_id),
	FOREIGN KEY (backup_id) REFERENCES Power_Backup(backup_id),
	FOREIGN KEY (psu_id) REFERENCES PSU(psu_id),
    FOREIGN KEY (monitor_id) REFERENCES Monitor(monitor_id),
    FOREIGN KEY (cooling_id) REFERENCES Cooling(cooling_id)
);
GO

-- Bảng Notifications
CREATE TABLE Notifications (
    notification_id INT PRIMARY KEY IDENTITY,
    sender_id INT, -- ID của người gửi thông báo (người quản lý)
    notification_content NVARCHAR(MAX),
    send_date DATETIME,
);
GO

-- Bảng NotificationRecipients
CREATE TABLE NotificationRecipients (
    recipient_id INT PRIMARY KEY IDENTITY,
    notification_id INT, -- ID của thông báo
    recipient_user_id INT, -- ID của người nhận thông báo
    FOREIGN KEY (notification_id) REFERENCES Notifications(notification_id),
    FOREIGN KEY (recipient_user_id) REFERENCES Users(user_id)
);
GO

-- Bảng tài sản lưu động
CREATE TABLE Assets (
    asset_id INT PRIMARY KEY IDENTITY,
	user_id INT,
    asset_type NVARCHAR(100), --Loại tài sản (ví dụ: tiền mặt, tài sản ngân hàng, tài sản đầu tư ngắn hạn, phải thu, v.v.).
    asset_name NVARCHAR(255), --Tên hoặc mô tả tài sản.
    current_value DECIMAL(18, 2), --Giá trị hiện tại của tài sản.
    last_updated DATETIME, --Ngày mà giá trị của tài sản được cập nhật lần cuối.
    description NVARCHAR(MAX),
	FOREIGN KEY (user_id) REFERENCES Users(user_id)
);
GO

-- Bảng yêu cầu nhập kho
CREATE TABLE StockRequests (
    request_id INT PRIMARY KEY IDENTITY,
    user_id INT, -- ID của quản lý kho
    supplier_id INT, -- ID của nhà cung cấp
    product_id INT, -- ID của sản phẩm được yêu cầu
    quantity INT, -- Số lượng sản phẩm yêu cầu
    request_date DATETIME,
    status NVARCHAR(50) -- Trạng thái của yêu cầu (ví dụ: "Đã gửi", "Đã xử lý",...)
	CONSTRAINT FK_stockreq_Users FOREIGN KEY (user_id) REFERENCES Users(user_id)
);
GO

-- Tạo bảng Logs ghi lại lịch sữ đăng nhập
CREATE TABLE Logs (
    log_id INT PRIMARY KEY IDENTITY,
    user_id INT, -- ID của người dùng
    login_time DATETIME NOT NULL, -- Thời gian đăng nhập
    logout_time DATETIME, -- Thời gian đăng xuất (nếu có)
    --ip_address VARCHAR(50) -- Địa chỉ IP của người dùng
	CONSTRAINT FK_Logs_Users FOREIGN KEY (user_id) REFERENCES Users(user_id)
);
GO

-- Tạo bảng ActivityLog
CREATE TABLE ActivityLog (
    log_id INT PRIMARY KEY IDENTITY,
    user_id INT,
    action NVARCHAR(255),
    action_time DATETIME,
    action_details NVARCHAR(MAX),
    FOREIGN KEY (user_id) REFERENCES Users(user_id)
);
GO