-- Chèn dữ liệu cho 2 admin với mật khẩu đã được băm
-- Chèn dữ liệu cho người quản trị với mật khẩu không được băm
INSERT INTO Users (username, password, role)
VALUES ('ad1', '123', 'Admin'),
       ('ad2', '123', 'Admin');

-- Chèn dữ liệu cho 3 người quản lý với mật khẩu không được băm
INSERT INTO Users (username, password, role)
VALUES 
    ('man1', '123', 'Manager'),
    ('man2', '123', 'Manager'),
    ('man3', '123', 'Manager');

-- Chèn dữ liệu cho 5 nhân viên với mật khẩu không được băm
INSERT INTO Users (username, password, role)
VALUES ('emp1', '123', 'Employee'),
       ('emp2', '123', 'Employee'),
       ('emp3', '123', 'Employee'),
       ('emp4', '123', 'Employee'),
       ('emp5', '123', 'Employee');

-- Chèn dữ liệu cho 5 khách hàng với mật khẩu không được băm
INSERT INTO Users (username, password, role)
VALUES ('cus1', '123', 'Customer'),
       ('cus2', '123', 'Customer'),
       ('cus3', '123', 'Customer'),
       ('cus4', '123', 'Customer'),
       ('cus5', '123', 'Customer');

GO

-- Thêm dữ liệu vào bảng Contracts
INSERT INTO Contracts (contract_type, start_date, end_date, contract_description) VALUES 
	('Full-time', '2024-01-01', NULL, N'Hợp đồng lao động toàn thời gian'),
	('Part-time', '2024-02-15', NULL, N'Hợp đồng lao động bán thời gian'),
	('Internship', '2024-03-10', N'2024-06-30', N'Hợp đồng thực tập'),
    ('Fixed-term', '2024-04-15', '2025-04-15', 'Fixed-term contract for project manager position.'),
    ('Temporary', '2024-03-20', '2024-06-30', 'Temporary contract for administrative assistant position.'),
    ('Freelance', '2024-04-01', NULL, 'Freelance contract for graphic designer.');
GO

-- Chèn dữ liệu cho 2 admin
INSERT INTO Employees (user_id, full_name, gender, email, phone_number, hire_date, salary, contract_type, date_of_birth)
VALUES 
    (1, 'Admin 1', N'Nam', 'admin1@example.com', '123456789', '2023-01-01', 5000.00, 'Full-time', '1990-01-01'),
    (2, 'Admin 2', N'Nữ', 'admin2@example.com', '987654321', '2023-02-01', 5500.00, 'Full-time', '1985-01-01');

-- Chèn dữ liệu cho 3 người quản lý
INSERT INTO Employees (user_id, full_name, gender, email, phone_number, hire_date, salary, contract_type, date_of_birth)
VALUES 
    (3, 'Manager 1', N'Nam', 'manager1@example.com', '111111111', '2023-03-01', 4500.00, 'Full-time', '1980-01-01'),
    (4, 'Manager 2', N'Nữ', 'manager2@example.com', '222222222', '2023-04-01', 4700.00, 'Full-time', '1983-01-01'),
    (5, 'Manager 3', N'Nam', 'manager3@example.com', '333333333', '2023-05-01', 4800.00, 'Full-time', '1979-01-01');

-- Chèn dữ liệu cho 5 nhân viên
INSERT INTO Employees (user_id, full_name, gender, email, phone_number, hire_date, salary, contract_type, date_of_birth)
VALUES 
    (6, 'Employee 1', N'Nam', 'employee1@example.com', '444444444', '2023-06-01', 3000.00, 'Full-time', '1998-01-01'),
    (7, 'Employee 2', N'Nữ', 'employee2@example.com', '555555555', '2023-07-01', 3200.00, 'Full-time', '1997-01-01'),
    (8, 'Employee 3', N'Nam', 'employee3@example.com', '666666666', '2023-08-01', 3300.00, 'Full-time', '1996-01-01'),
    (9, 'Employee 4', N'Nam', 'employee4@example.com', '777777777', '2023-09-01', 3100.00, 'Full-time', '1995-01-01'),
    (10, 'Employee 5', N'Nữ', 'employee5@example.com', '888888888', '2023-10-01', 3400.00, 'Full-time', '1994-01-01');
GO

-- Chèn dữ liệu cho bộ phận bán hàng (Sales Department)
INSERT INTO Departments (department_name, department_head_id, department_description)
VALUES (N'Bộ phận Bán hàng', 6, N'Chịu trách nhiệm về các hoạt động bán hàng và dịch vụ khách hàng.');

-- Chèn dữ liệu cho phòng kho (Warehouse Department)
INSERT INTO Departments (department_name, department_head_id, department_description)
VALUES (N'Phòng Kho', 7, N'Chịu trách nhiệm về quản lý tồn kho và logistics.');

-- Chèn dữ liệu cho bộ phận kế toán (Accounting Department)
INSERT INTO Departments (department_name, department_head_id, department_description)
VALUES (N'Bộ phận Kế toán', 8, N'Chịu trách nhiệm về các giao dịch tài chính và báo cáo.');
GO

select * from Employees

-- Chèn dữ liệu vào bảng EmployeeSalaries
INSERT INTO EmployeeSalaries (employee_id, contract_type, base_salary, hourly_rate, monthly_hours, internship_compensation)
VALUES
    (1, 'Full-time', 4500.00, NULL, NULL, NULL), -- Mức lương cơ bản của Manager 1 là $4500
    (2, 'Full-time', 4700.00, NULL, NULL, NULL), -- Mức lương cơ bản của Manager 2 là $4700
    (3, 'Full-time', 4800.00, NULL, NULL, NULL), -- Mức lương cơ bản của Manager 3 là $4800
    (4, 'Full-time', 3000.00, NULL, NULL, NULL), -- Mức lương cơ bản của Employee 1 là $3000
    (5, 'Full-time', 3200.00, NULL, NULL, NULL), -- Mức lương cơ bản của Employee 2 là $3200
    (6, 'Full-time', 3300.00, NULL, NULL, NULL), -- Mức lương cơ bản của Employee 3 là $3300
    (7, 'Full-time', 3100.00, NULL, NULL, NULL), -- Mức lương cơ bản của Employee 4 là $3100
    (8, 'Full-time', 3400.00, NULL, NULL, NULL); -- Mức lương cơ bản của Employee 5 là $3400
GO

-- Chèn dữ liệu cho bộ phận bán hàng (Sales Department)
INSERT INTO Employee_Department_Assignment (employee_id, department_id, start_date, end_date)
VALUES 
    (6, 4, '2023-06-01', NULL),
	(9, 4, '2023-01-01', NULL);

-- Chèn dữ liệu cho phòng kho (Warehouse Department)
INSERT INTO Employee_Department_Assignment (employee_id, department_id, start_date, end_date)
VALUES 
    (1, 5, '2023-07-01', NULL);

-- Chèn dữ liệu cho bộ phận kế toán (Accounting Department)
INSERT INTO Employee_Department_Assignment (employee_id, department_id, start_date, end_date)
VALUES 
    (2, 6, '2023-08-01', NULL),
	(3, 6, '2023-08-01', NULL);
GO

-- Chèn dữ liệu cho 5 khách hàng
INSERT INTO Customers (user_id, full_name, address, email, phone_number, date_of_birth)
VALUES 
    (11, 'Customer 1', 'Address 1', 'cus1@example.com', '123456789', '1990-01-01'),
    (12, 'Customer 2', 'Address 2', 'cus2@example.com', '987654321', '1985-01-01'),
    (13, 'Customer 3', 'Address 3', 'cus3@example.com', '111222333', '1980-01-01'),
    (14, 'Customer 4', 'Address 4', 'cus4@example.com', '444555666', '1979-01-01'),
    (15, 'Customer 5', 'Address 5', 'cus5@example.com', '777888999', '1998-01-01');
GO

-- Chèn dữ liệu cho các phương thức thanh toán của khách hàng
INSERT INTO PaymentMethods (user_id, method_type, card_number, expiry_date, cvv, bank_account_number)
VALUES 
    (1, 'Credit Card', '1234567890123456', '2025-12-31', '123', NULL), -- Ví dụ cho thẻ tín dụng
    (3, 'Debit Card', '9876543210987654', '2026-12-31', '456', NULL), -- Ví dụ cho thẻ ghi nợ
    (6, 'Credit Card', '1111222233334444', '2024-12-31', '789', NULL), -- Ví dụ khác cho thẻ tín dụng
    (2, 'Cash', NULL, NULL, NULL, NULL), -- Ví dụ cho thanh toán bằng tiền mặt
    (4, 'Bank Transfer', NULL, NULL, NULL, '123456789'); -- Ví dụ cho chuyển khoản ngân hàng
GO

-- Chèn dữ liệu vào bảng Nhà cung cấp
INSERT INTO Suppliers (supplier_name, address, email, phone_number)
VALUES 
    ('Supplier 1', '123 ABC Street, City A', 'supplier1@example.com', '123456789'),
    ('Supplier 2', '456 XYZ Street, City B', 'supplier2@example.com', '987654321'),
    ('Supplier 3', '789 LMN Street, City C', 'supplier3@example.com', '456789123');
GO

-- Chèn dữ liệu vào bảng Loại linh kiện
INSERT INTO Component_Types (component_type_name)
VALUES 
    ('CPU'), -- Bộ xử lý (CPU)
    ('GPU'), -- Card đồ họa (GPU)
    ('RAM'), -- Bộ nhớ RAM (RAM)
    ('Storage'), -- Thiết bị lưu trữ (Storage)
    ('Motherboard'), -- Bo mạch chủ (Motherboard)
    ('PSU'), -- Nguồn máy tính (PSU)
    ('Cooling'), -- Hệ thống làm mát (Cooling)
    ('Peripherals'), -- Thiết bị ngoại vi (Peripherals)
    ('HDD'), -- Ổ cứng cơ (HDD)
    ('SSD'), -- Ổ cứng SSD (SSD)
    ('Monitor'), -- Màn hình (Monitor)
    ('Computer Case'), -- Vỏ máy tính (Computer Case)
    ('Network Adapter'), -- Bộ chuyển mạng (Network Adapter)
    ('Expansion Cards'), -- Thẻ mở rộng (Expansion Cards)
    ('Mouse'), -- Chuột (Mouse)
    ('Keyboard'), -- Bàn phím (Keyboard)
    ('Microphone'), -- Micro (Microphone)
    ('Webcam'), -- Webcam (Webcam)
    ('Headset'), -- Tai nghe (Headset)
    ('Speaker'), -- Loa (Speaker)
    ('Controller'), -- Bộ điều khiển (Controller)
    ('Mouse Pad'), -- Thảm chuột (Mouse Pad)
    ('Scanner'), -- Máy quét (Scanner)
    ('Adapter'), -- Bộ chuyển đổi (Adapter)
    ('Pop filter'), -- Lọc âm (Pop filter)
    ('USB hub'), -- Hub USB (USB hub)
    ('Laptop cooling pad'), -- Gối làm mát laptop (Laptop cooling pad)
    ('Stylus pen'), -- Bút cảm ứng (Stylus pen)
    ('Computer cleaning kit'), -- Bộ dụng cụ làm sạch máy tính (Computer cleaning kit)
	('Power Backup'),
    ('Laptop cooling stand'); -- Giá đỡ làm mát laptop (Laptop cooling stand)
GO

SELECT * FROM Component_Types

-- Thêm dữ liệu vào bảng CPU
INSERT INTO CPU (cpu_name, brand, model, speed, socket_type, price, component_type_id, supplier_id, image)
VALUES
	('Intel Core i7', 'Intel', 'i7', 3.2, 'LGA1200', 7414871, 1, 1, 'cpu_image1.jpg'), -- CPU từ Supplier 1
	('AMD Ryzen 5 3600', 'AMD', 'Ryzen 5 3600', 3.6, 'AM4', 5430959, 1, 2, 'cpu_image2.jpg'), -- CPU từ Supplier 2
	('AMD Ryzen 9 5950X', 'AMD', 'Ryzen 9 5950X', 4.9, 'AM4', 11499999, 1, 3, 'cpu_image4.jpg'), -- CPU từ Supplier 3
	('Intel Core i9', 'Intel', 'i9', 3.6, 'LGA1200', 12374651, 1, 1, 'cpu_image3.jpg'), -- CPU từ Supplier 1
    ('Intel Core i9-9900K', 'Intel', 'i9-9900K', 3.6, 'LGA1151', 4899000, 1, 1, 'cpu_image1.jpg');
GO

-- Thêm dữ liệu vào bảng RAM
INSERT INTO RAM (ram_name, brand, model, capacity, speed, type, price, component_type_id, supplier_id, image)
VALUES 
	('HyperX Fury Beast RGB', 'Kingston', 'HX432C16FB3K232', 8, 3200, 'DDR4', 2985000, 3, 2, 'ram_image.jpg'),
	('Trident Z RGB', 'G.Skill', 'F4-3600C16D-16GTZR', 16, 3600, 'DDR4', 4599000, 3, 3, 'ram_image2.jpg'),
	('Vengeance LPX', 'Corsair', 'CMK16GX4M2Z3600C18', 16, 3600, 'DDR4', 4137000, 3, 1, 'ram_image3.jpg'),
	('Kingston HyperX Fury RGB', 'Kingston', 'HX432C16FB3K2/16', 16, 3200, 'DDR4', 1799000, 3, 1, 'ram_image1.jpg');

GO

-- Thêm dữ liệu vào bảng HDD
INSERT INTO HDD (hdd_name, brand, model, capacity, interface_type, price, component_type_id, supplier_id, image)
VALUES 
	('Seagate Barracuda', 'Seagate', 'ST2000DM008', '2TB', 'SATA III', 1379770, 10, 1, 'hdd_image.jpg'), -- 59.99 USD
	('WD Blue WD10EZEX', 'Western Digital', 'WD10EZEX', '1TB', 'SATA III', 1149770, 10, 2, 'hdd_image2.jpg'), -- 49.99 USD
	('Toshiba P300', 'Toshiba', 'HDWD130EZSTA', '1.3TB', 'SATA III', 1264770, 10, 2, 'hdd_image3.jpg'); -- 54.99 USD
GO

-- Thêm dữ liệu vào bảng SSD
INSERT INTO SSD (ssd_name, brand, model, capacity, interface_type, price, component_type_id, supplier_id, image)
VALUES 
	('Samsung 870 EVO', 'Samsung', 'MZ-76E500B/AM', '500GB', 'SATA III', 2985270, 11, 3, 'ssd_image.jpg'), -- 129.99 USD
	('Crucial MX500', 'Crucial', 'CT500MX500SSD1', '500GB', 'SATA III', 2529270, 11, 1, 'ssd_image2.jpg'), -- 109.99 USD
	('Kingston A400', 'Kingston', 'SA400S37/240G', '240GB', 'SATA III', 1839270, 11, 3, 'ssd_image3.jpg'); -- 79.99 USD
GO

-- Thêm dữ liệu vào bảng GPU
INSERT INTO GPU (gpu_name, brand, model, vram, interface_type, price, component_type_id, supplier_id, image)
VALUES 
    ('NVIDIA GeForce GTX 1660', 'NVIDIA', 'GTX 1660', 6, 'PCIe 3.0 x16', 6439900, 2, 1, 'gpu_image.jpg'),
    ('AMD Radeon RX 5700 XT', 'AMD', 'RX 5700 XT', 8, 'PCIe 4.0 x16', 9199900, 2, 2, 'gpu_image2.jpg'),
    ('NVIDIA GeForce RTX 3080', 'NVIDIA', 'RTX 3080', 10, 'PCIe 4.0 x16', 16098900, 2, 3, 'gpu_image3.jpg');
GO

INSERT INTO GPU (gpu_name, brand, model, vram, interface_type, price, component_type_id, supplier_id, image)
VALUES 
    ('NVIDIA GeForce GTX 1050Ti', 'NVIDIA', 'GTX 1050Ti', 6, 'PCIe 3.0 x16', 6439900, 2, 1, 'gpu_image.jpg')
GO

-- Thêm dữ liệu vào bảng Motherboard
INSERT INTO Motherboard (motherboard_id, motherboard_name, brand, model, socket_type, max_ram_capacity, price, component_type_id, supplier_id, image)
VALUES 
    ('ASUS_Z490', 'ASUS Prime Z490-P', 'ASUS', 'Prime Z490-P', 'LGA1200', 128, 3199900, 5, 1, 'motherboard_image.jpg'),
    ('GIGABYTE_B450', 'GIGABYTE B450 AORUS ELITE', 'GIGABYTE', 'B450 AORUS ELITE', 'AM4', 64, 2199900, 5, 2, 'motherboard_image2.jpg'),
    ('MSI_X570', 'MSI MPG X570 GAMING PLUS', 'MSI', 'MPG X570 GAMING PLUS', 'AM4', 128, 3599900, 5, 3, 'motherboard_image3.jpg');
GO

-- Thêm dữ liệu vào bảng Cooling
INSERT INTO Cooling (cooling_name, brand, model, price, component_type_id, supplier_id)
VALUES 
    ('Cooler Master Hyper 212 RGB', 'Cooler Master', 'Hyper 212 RGB', 999900, 8, 1),
    ('NZXT Kraken X63', 'NZXT', 'Kraken X63', 3499900, 8, 2),
    ('Corsair H100i RGB Platinum SE', 'Corsair', 'H100i RGB Platinum SE', 3999900, 8, 3);
GO

-- Thêm dữ liệu vào bảng PSU
INSERT INTO PSU (psu_id, psu_name, brand, model, wattage, efficiency_rating, price, component_type_id, supplier_id, image)
VALUES 
    ('EVGA_600W', 'EVGA 600W', 'EVGA', '600W', 600, '80 Plus', 1439900, 6, 1, 'psu_image.jpg'),
    ('Corsair_750W', 'Corsair CX750M', 'Corsair', 'CX750M', 750, '80 Plus Bronze', 2159900, 6, 2, 'psu_image2.jpg'),
    ('Seasonic_850W', 'Seasonic Focus GX-850', 'Seasonic', 'Focus GX-850', 850, '80 Plus Gold', 3359900, 6, 3, 'psu_image3.jpg');
GO

-- Thêm dữ liệu vào bảng Monitor
INSERT INTO Monitor (monitor_id, monitor_name, brand, model, size_inch, resolution, price, component_type_id, supplier_id, image)
VALUES 
    ('Acer_24inch', 'Acer SB220Q', 'Acer', 'SB220Q', 21.5, '1920x1080', 2279900, 12, 1, 'monitor_image.jpg'),
    ('ASUS_27inch', 'ASUS TUF Gaming VG279QM', 'ASUS', 'VG279QM', 27, '1920x1080', 5999900, 12, 2, 'monitor_image2.jpg'),
    ('Dell_34inch', 'Dell Ultrasharp U3419W', 'Dell', 'U3419W', 34, '3440x1440', 14399900, 12, 3, 'monitor_image3.jpg');
GO

-- Thêm dữ liệu vào bảng Computer_Case
INSERT INTO Computer_Case (case_id, case_name, brand, model, size, color, price, component_type_id, supplier_id, image)
VALUES 
    ('NZXT_H510', 'NZXT H510', 'NZXT', 'H510', 'Mid-Tower', 'Black', 1699900, 13, 1, 'case_image.jpg'),
    ('Corsair_500D', 'Corsair Obsidian 500D', 'Corsair', '500D', 'Mid-Tower', 'Black', 3399900, 13, 2, 'case_image2.jpg'),
    ('CoolerMaster_MasterBox', 'Cooler Master MasterBox Q300L', 'Cooler Master', 'MasterBox Q300L', 'Micro-ATX', 'Black', 999900, 13, 3, 'case_image3.jpg');
GO

-- Thêm dữ liệu vào bảng Accessories
INSERT INTO Accessories (accessory_name, brand, model, price, supplier_id, component_type_id, image)
VALUES 
    ('Logitech G502 Hero', 'Logitech', 'G502 Hero', 1999900, 1, 16, 'accessory_image.jpg'),
    ('Corsair K95 RGB Platinum XT', 'Corsair', 'K95 RGB Platinum XT', 4999900, 2, 17, 'accessory_image2.jpg'),
    ('Bose Companion 2 Series III', 'Bose', 'Companion 2 Series III', 2499900, 3, 21, 'accessory_image3.jpg');
GO

-- INSERT INTO Network_Adapter
INSERT INTO Network_Adapter (adapter_name, brand, model, component_type_id, price, supplier_id, image)
VALUES 
    ('Ethernet Adapter', 'TP-Link', 'TL-WN725N', 14, 1399000, 1, 'ethernet_adapter.jpg'),
    ('Wireless USB Adapter', 'D-Link', 'DWA-123', 14, 399000, 2, 'wireless_adapter.jpg'),
    ('Bluetooth Adapter', 'ASUS', 'USB-BT400', 14, 599000, 3, 'bluetooth_adapter.jpg');
GO

-- INSERT INTO Expansion_Cards
INSERT INTO Expansion_Cards (card_name, brand, model, component_type_id, price, supplier_id, image)
VALUES 
    ('Sound Card', 'Creative', 'Sound Blaster Audigy FX', 15, 899000, 1, 'sound_card.jpg'),
    ('Wireless Network Card', 'TP-Link', 'Archer T9E', 15, 1399000, 2, 'network_card.jpg'),
    ('PCIe USB Expansion Card', 'Inateck', 'KT4006', 15, 699000, 3, 'pcie_usb_card.jpg');
GO

-- INSERT INTO Power_Backup
INSERT INTO Power_Backup (backup_name, brand, model, component_type_id, price, supplier_id, image)
VALUES 
    ('Uninterruptible Power Supply', 'APC', 'BE600M1', 32, 2399000, 1, 'ups.jpg'),
    ('Power Bank', 'Anker', 'PowerCore 10000', 32, 499000, 2, 'power_bank.jpg'),
    ('Portable UPS', 'CyberPower', 'CP350SLG', 32, 1599000, 3, 'portable_ups.jpg');
GO

-- Chèn dữ liệu cho máy tính thứ nhất
INSERT INTO Computer (cpu_id, ram_id, hdd_id, ssd_id, gpu_id, motherboard_id, case_id, psu_id, cooling_id, adapter_id, card_id, backup_id, price)
VALUES ('Intel_C1_i7', 'Corsair_R3_CMK16GX4M2Z3600C18', 'Western Digital_H2_WD10EZEX', NULL, 'NVIDIA_G4_GTX 1050Ti', 'ASUS_Z490', 'Corsair_500D', 'Corsair_750W', 'Cooler Master_CL1_Hyper 212 RGB', 'TP-Link_NA1_TL-WN725N', 'TP-Link_EC2_Archer T9E', 'Anker_PB2_PowerCore 10000', 7414871.00);
GO

select * from Computer


--Dữ liệu cho bảng đơn hàng
INSERT INTO Orders (customer_id, order_date, delivery_address)
VALUES 
    (2, GETDATE(), '12 Main St')

--Chi tiết đơn hàng
INSERT INTO Order_Details (order_id, product_id, quantity, unit_price, discount)
VALUES
    (1, 1, 2, 4000.00, 0.00)

--Tài sản lưu động
INSERT INTO Assets (user_id, asset_type, asset_name, current_value, last_updated, description)
VALUES 
    (1, N'Tiền mặt', N'Tiền mặt trong tài khoản ngân hàng', 50000.00, GETDATE(), N'Số tiền mặt hiện có trong tài khoản ngân hàng'),
    (1, N'Tài sản ngân hàng', N'Khoản vay', 10000.00, GETDATE(), N'Số tiền vay từ ngân hàng ABC'),
    (1, N'Tài sản đầu tư ngắn hạn', N'Cổ phiếu ABC', 8000.00, GETDATE(), N'Đầu tư vào cổ phiếu ABC'),
    (1, N'Phải thu', N'Công nợ khách hàng X', 15000.00, GETDATE(), N'Số tiền mà khách hàng X đang nợ');
GO
