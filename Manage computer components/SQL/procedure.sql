--Lệnh thêm khách hàng
CREATE PROC InsertCustomerVsPaymentMethod
    @full_name VARCHAR(255),
    @address VARCHAR(255),
    @email VARCHAR(255),
    @phone_number VARCHAR(20),
    @method_type VARCHAR(50),
    @card_number VARCHAR(20),
    @expiry_date DATE,
    @cvv VARCHAR(5),
    @bank_account_number VARCHAR(20)
AS
BEGIN
    DECLARE @customerId INT;

    -- Thêm khách hàng mới vào bảng Customers
    INSERT INTO Customers (full_name, address, email, phone_number)
    VALUES (@full_name, @address, @email, @phone_number);

    -- Lấy customer_id của khách hàng vừa được thêm vào
    SET @customerId = SCOPE_IDENTITY();

    -- Thêm phương thức thanh toán cho khách hàng vào bảng PaymentMethods
    INSERT INTO PaymentMethods (user_id, method_type, card_number, expiry_date, cvv, bank_account_number)
    VALUES (@customerId, @method_type, @card_number, @expiry_date, @cvv, @bank_account_number);
END;
GO

--Xóa khách hàng
CREATE OR ALTER PROC DeleteCustomerAndPaymentMethod
    @user_id INT
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

		-- Xóa thông tin khách hàng từ bảng Customers
        DELETE FROM Customers 
		WHERE user_id = @user_id;

        -- Xóa phương thức thanh toán của khách hàng từ bảng PaymentMethods
		DELETE FROM PaymentMethods
		WHERE user_id = @user_id;

		--Xóa tài khoản khách hàng
		DELETE FROM Users 
		WHERE user_id = @user_id;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;

GO

-- xóa nhân viên
CREATE OR ALTER PROCEDURE DeleteEmployee
    @user_id INT
AS
BEGIN
    -- Lấy employee_id từ user_id
    DECLARE @employee_id INT;
    SELECT @employee_id = employee_id FROM Employees WHERE user_id = @user_id;

    IF @employee_id IS NOT NULL
    BEGIN
        -- Xóa các phương thức thanh toán liên quan đến nhân viên
        DELETE FROM PaymentMethods
        WHERE user_id = @user_id;

        -- Xóa bản ghi lương của nhân viên
        DELETE FROM EmployeeSalaries
        WHERE employee_id = @employee_id;

        -- Xóa các phân công của nhân viên trong các phòng ban
        DELETE FROM Employee_Department_Assignment
        WHERE employee_id = @employee_id;

        -- Kiểm tra xem nhân viên có là trưởng phòng không
        IF EXISTS (SELECT 1 FROM Departments WHERE department_head_id = @employee_id)
        BEGIN
            -- Nếu nhân viên là trưởng phòng, chỉ cập nhật department_head_id thành NULL
            UPDATE Departments
            SET department_head_id = NULL
            WHERE department_head_id = @employee_id;
        END;

        -- Xóa thông tin nhân viên từ bảng Employees
        DELETE FROM Employees
        WHERE employee_id = @employee_id;
    END
    ELSE
    BEGIN
        PRINT 'Không tìm thấy nhân viên có user_id = ' + CAST(@user_id AS NVARCHAR);
    END;
END;
GO

-- lấy hình ảnh từ product ID
CREATE OR ALTER PROCEDURE GetProductImage
    @productid VARCHAR(100) -- Đảm bảo kiểu dữ liệu đúng cho product ID
AS
BEGIN
    SELECT 
        COALESCE(cpu.image, ram.image, hdd.image, ssd.image, gpu.image, motherboard.image, 
                 computer_case.image, accessories.image, network_adapter.image, expansion_cards.image, 
                 power_backup.image, psu.image, monitor.image, cooling.image) AS profile_image
    FROM Products p
    LEFT JOIN CPU ON p.cpu_id = CPU.cpu_id
    LEFT JOIN RAM ON p.ram_id = RAM.ram_id
    LEFT JOIN HDD ON p.hdd_id = HDD.hdd_id
    LEFT JOIN SSD ON p.ssd_id = SSD.ssd_id
    LEFT JOIN GPU ON p.gpu_id = GPU.gpu_id
    LEFT JOIN Motherboard ON p.motherboard_id = Motherboard.motherboard_id
    LEFT JOIN Computer_Case ON p.case_id = Computer_Case.case_id
    LEFT JOIN Accessories ON p.accessory_id = Accessories.accessory_id
    LEFT JOIN Network_Adapter ON p.adapter_id = Network_Adapter.adapter_id
    LEFT JOIN Expansion_Cards ON p.card_id = Expansion_Cards.card_id
    LEFT JOIN Power_Backup ON p.backup_id = Power_Backup.backup_id
    LEFT JOIN PSU ON p.psu_id = PSU.psu_id
    LEFT JOIN Monitor ON p.monitor_id = Monitor.monitor_id
    LEFT JOIN Cooling ON p.cooling_id = Cooling.cooling_id
    WHERE p.cpu_id = @productid 
       OR p.ram_id = @productid 
       OR p.hdd_id = @productid 
       OR p.ssd_id = @productid 
       OR p.gpu_id = @productid 
       OR p.motherboard_id = @productid 
       OR p.case_id = @productid 
       OR p.accessory_id = @productid 
       OR p.adapter_id = @productid 
       OR p.card_id = @productid 
       OR p.backup_id = @productid 
       OR p.psu_id = @productid 
       OR p.monitor_id = @productid 
       OR p.cooling_id = @productid;
END
GO

-- lấy thông tin từ tất cả table
CREATE OR ALTER PROCEDURE GetAllProductInfo
    @productid VARCHAR(100) -- Đảm bảo kiểu dữ liệu đúng cho product ID
AS
BEGIN
    SELECT 
		COALESCE(CPU.cpu_id , RAM.ram_id , HDD.hdd_id , SSD.ssd_id , GPU.gpu_id , Motherboard.motherboard_id , Cooling.cooling_id , PSU.psu_id , Monitor.monitor_id , Accessories.accessory_id , Network_Adapter.adapter_id , Expansion_Cards.card_id , Power_Backup.backup_id , Computer_Case.case_id) AS ID , --txtID
		COALESCE(CPU.cpu_name , RAM.ram_name , HDD.hdd_name , SSD.ssd_name , GPU.gpu_name , Motherboard.motherboard_name , Cooling.cooling_name , PSU.psu_name , Monitor.monitor_name , Accessories.accessory_name , Network_Adapter.adapter_name , Expansion_Cards.card_name , Power_Backup.backup_name , Computer_Case.case_name) AS Name, --txtName
		COALESCE(CPU.brand , RAM.brand , HDD.brand , SSD.brand , GPU.brand , Motherboard.brand , Cooling.brand , PSU.brand , Monitor.brand , Accessories.brand , Network_Adapter.brand , Expansion_Cards.brand , Power_Backup.brand , Computer_Case.brand) AS Brand, --txtBrand
		COALESCE(CPU.model , RAM.model , HDD.model , SSD.model , GPU.model , Motherboard.model , Cooling.model , PSU.model , Monitor.model , Accessories.model , Network_Adapter.model , Expansion_Cards.model , Power_Backup.model , Computer_Case.model) AS Model, --txtModel
		CT.component_type_name AS LinhKien, --cboLinhKien
		S.supplier_name AS Supplier,--cboSupplier
		COALESCE(CPU.price, RAM.price, HDD.price , SSD.price , GPU.price , Motherboard.price , Cooling.price , PSU.price , Monitor.price , Accessories.price , Network_Adapter.price , Expansion_Cards.price , Power_Backup.price , Computer_Case.price) AS Price, --txtPrice
		COALESCE(RAM.capacity , HDD.capacity , SSD.capacity , GPU.vram , Motherboard.max_ram_capacity, PSU.wattage , Monitor.size_inch, Computer_Case.size) AS DungLuong, --label10 --txtCap
		COALESCE(CPU.speed, RAM.speed, PSU.efficiency_rating , Monitor.resolution, Computer_Case.color) AS Speed, --txtSpeed --lable6
		COALESCE(CPU.socket_type, RAM.type, HDD.interface_type , SSD.interface_type , GPU.interface_type , Motherboard.socket_type) AS Type, --label7 --txtType
		COALESCE(CPU.image , RAM.image , HDD.image , SSD.image , GPU.image , Motherboard.image , Cooling.image , PSU.image , Monitor.image , Accessories.image , Network_Adapter.image , Expansion_Cards.image , Power_Backup.image , Computer_Case.image) AS image
	FROM Products AS P
    LEFT JOIN CPU ON P.cpu_id = CPU.cpu_id
    LEFT JOIN RAM ON P.ram_id = RAM.ram_id
    LEFT JOIN HDD ON P.hdd_id = HDD.hdd_id
    LEFT JOIN SSD ON P.ssd_id = SSD.ssd_id
    LEFT JOIN GPU ON P.gpu_id = GPU.gpu_id
    LEFT JOIN Motherboard ON P.motherboard_id = Motherboard.motherboard_id
    LEFT JOIN Cooling ON P.cooling_id = Cooling.cooling_id
    LEFT JOIN PSU ON P.psu_id = PSU.psu_id
    LEFT JOIN Monitor ON P.monitor_id = Monitor.monitor_id
    LEFT JOIN Accessories ON P.accessory_id = Accessories.accessory_id
    LEFT JOIN Network_Adapter ON P.adapter_id = Network_Adapter.adapter_id
    LEFT JOIN Expansion_Cards ON P.card_id = Expansion_Cards.card_id
    LEFT JOIN Power_Backup ON P.backup_id = Power_Backup.backup_id
    LEFT JOIN Computer_Case ON P.case_id = Computer_Case.case_id
	LEFT JOIN Component_Types CT 
	ON CT.component_type_id = COALESCE(CPU.component_type_id, RAM.component_type_id, HDD.component_type_id, SSD.component_type_id, 
	GPU.component_type_id, Motherboard.component_type_id, Cooling.component_type_id, PSU.component_type_id, Monitor.component_type_id, 
	Accessories.component_type_id, Network_Adapter.component_type_id, Expansion_Cards.component_type_id, Power_Backup.component_type_id, Computer_Case.component_type_id)
	LEFT JOIN Suppliers S 
	ON S.supplier_id = COALESCE(CPU.supplier_id, RAM.supplier_id, HDD.supplier_id, SSD.supplier_id, 
    GPU.supplier_id, Motherboard.supplier_id, Cooling.supplier_id, PSU.supplier_id, Monitor.supplier_id, 
    Accessories.supplier_id, Network_Adapter.supplier_id, Expansion_Cards.supplier_id, Power_Backup.supplier_id, Computer_Case.supplier_id)

    WHERE p.cpu_id = @productid 
       OR p.ram_id = @productid 
       OR p.hdd_id = @productid 
       OR p.ssd_id = @productid 
       OR p.gpu_id = @productid 
       OR p.motherboard_id = @productid 
       OR p.case_id = @productid 
       OR p.accessory_id = @productid 
       OR p.adapter_id = @productid 
       OR p.card_id = @productid 
       OR p.backup_id = @productid 
       OR p.psu_id = @productid 
       OR p.monitor_id = @productid 
       OR p.cooling_id = @productid;
END
GO

select * from ProductInfo

EXEC GetAllProductInfo @productid = 'Western Digital_H2_WD10EZEX'

-- Insert thong tin vao san pham
CREATE OR ALTER PROCEDURE InsertProduct
    @component_type_name VARCHAR(100),
    @name VARCHAR(255),
    @brand VARCHAR(255),
    @model VARCHAR(255),
    @cap VARCHAR(255),
    @speed VARCHAR(255),
    @type VARCHAR(255),
    @price DECIMAL(18, 2),
    @supplier_name NVARCHAR(100),
    @image VARCHAR(255)

AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @component_type_id INT;
    DECLARE @supplier_id INT;

    -- Lấy component_type_id từ Component_Types bảng dựa trên component_type_name
    SELECT @component_type_id = component_type_id
    FROM Component_Types
    WHERE component_type_name = @component_type_name;

    -- Lấy supplier_id từ Suppliers bảng dựa trên @supplier_name
    SELECT @supplier_id = supplier_id
    FROM Suppliers
    WHERE supplier_name = @supplier_name;

    -- Kiểm tra xem component_type_id có tồn tại hay không
    IF @component_type_id IS NOT NULL
    BEGIN
        -- Insert sản phẩm vào bảng phù hợp
		IF @component_type_name = 'RAM'
        BEGIN
            INSERT INTO RAM (ram_name, brand, model, capacity, speed, type, price, component_type_id, supplier_id, image)
            VALUES (@name, @brand, @model, @cap, @speed, @type, @price, @component_type_id, @supplier_id, @image);
		END
        ELSE IF @component_type_name = 'CPU'
        BEGIN
			INSERT INTO CPU (cpu_name, brand, model, speed, socket_type, price, component_type_id, supplier_id, image)
			VALUES (@name, @brand, @model, @speed, @type , @price, @component_type_id, @supplier_id,@image);
        END
        ELSE IF @component_type_name = 'HDD' OR @component_type_name = 'SSD'
        BEGIN
            INSERT INTO HDD (hdd_name, brand, model, capacity, interface_type, price, component_type_id, supplier_id, image)
            VALUES (@name, @brand, @model, @cap, @type, @price, @component_type_id, @supplier_id, @image);

            INSERT INTO SSD (ssd_name, brand, model, capacity, interface_type, price, component_type_id, supplier_id, image)
            VALUES (@name, @brand, @model, @cap, @type, @price, @component_type_id, @supplier_id, @image);
        END
        ELSE IF @component_type_name = 'GPU'
        BEGIN
            INSERT INTO GPU (gpu_name, brand, model, vram, interface_type, price, component_type_id, supplier_id, image)
            VALUES (@name, @brand, @model, @cap, @type, @price, @component_type_id, @supplier_id, @image);
        END
        ELSE IF @component_type_name = 'Motherboard'
        BEGIN
            INSERT INTO Motherboard (motherboard_name, brand, model, socket_type, max_ram_capacity, price, component_type_id, supplier_id, image)
            VALUES (@name, @brand, @model, @type, @cap, @price, @component_type_id, @supplier_id, @image);
        END
        ELSE IF @component_type_name = 'Cooling'
        BEGIN
            INSERT INTO Cooling (cooling_name, brand, model, price, component_type_id, supplier_id, image)
            VALUES (@name, @brand, @model, @price, @component_type_id, @supplier_id, @image);
        END
        ELSE IF @component_type_name = 'Network Adapter'
        BEGIN
            INSERT INTO Network_Adapter (adapter_name, brand, model, price, component_type_id, supplier_id, image)
            VALUES (@name, @brand, @model, @price, @component_type_id, @supplier_id, @image);
        END
        ELSE IF @component_type_name = 'Expansion Cards'
        BEGIN
            INSERT INTO Expansion_Cards (card_name, brand, model, price, component_type_id, supplier_id, image)
            VALUES (@name, @brand, @model, @price, @component_type_id, @supplier_id, @image);
        END
        ELSE IF @component_type_name = 'Power Backup'
        BEGIN
            INSERT INTO Power_Backup (backup_name, brand, model, price, component_type_id, supplier_id, image)
            VALUES (@name, @brand, @model, @price, @component_type_id, @supplier_id, @image);
        END
        ELSE IF @component_type_name = 'PSU'
        BEGIN
            INSERT INTO PSU (psu_name, brand, model, wattage, efficiency_rating, price, component_type_id, supplier_id, image)
            VALUES (@name, @brand, @model, @cap, @speed, @price, @component_type_id, @supplier_id, @image);
        END
        ELSE IF @component_type_name = 'Monitor'
        BEGIN
            INSERT INTO Monitor (monitor_name, brand, model, size_inch, resolution, price, component_type_id, supplier_id, image)
            VALUES (@name, @brand, @model, @cap, @speed, @price, @component_type_id, @supplier_id, @image);
        END
        ELSE IF @component_type_name = 'Computer Case'
        BEGIN
            INSERT INTO Computer_Case (case_name, brand, model, size, color, price, component_type_id, supplier_id, image)
            VALUES (@name, @brand, @model, @cap, @speed, @price, @component_type_id, @supplier_id, @image);
        END
        ELSE 
        BEGIN
            INSERT INTO Accessories(accessory_name, brand, model, price, supplier_id, component_type_id, image)
            VALUES (@name, @brand, @model, @price, @supplier_id, @component_type_id, @image);
        END
    END
    ELSE
    BEGIN
        PRINT 'Không tìm thấy component_type_id cho ' + @component_type_name;
    END
END
GO

-- Update thong tin san pham
CREATE OR ALTER PROCEDURE UpdateProduct
    @product_id VARCHAR(100),
    @component_type_name VARCHAR(100),
    @name VARCHAR(255),
    @brand VARCHAR(255),
    @model VARCHAR(255),
    @cap VARCHAR(255),
    @speed VARCHAR(255),
    @type VARCHAR(255),
    @price DECIMAL(18, 2),
    @supplier_name NVARCHAR(100),
    @image VARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @component_type_id INT;
    DECLARE @supplier_id INT;

    -- Lấy component_type_id từ Component_Types bảng dựa trên component_type_name
    SELECT @component_type_id = component_type_id
    FROM Component_Types
    WHERE component_type_name = @component_type_name;

    -- Lấy supplier_id từ Suppliers bảng dựa trên @supplier_name
    SELECT @supplier_id = supplier_id
    FROM Suppliers
    WHERE supplier_name = @supplier_name;

    -- Kiểm tra xem component_type_id có tồn tại hay không
    IF @component_type_id IS NOT NULL
    BEGIN
        -- Update sản phẩm vào bảng phù hợp
		IF @component_type_name = 'RAM'
        BEGIN
            UPDATE RAM
            SET ram_name = @name, brand = @brand, model = @model, capacity = @cap, speed = @speed, type = @type, price = @price, component_type_id = @component_type_id, supplier_id = @supplier_id, image = @image
            WHERE ram_id = @product_id;
		END
        ELSE IF @component_type_name = 'CPU'
        BEGIN
			UPDATE CPU
			SET cpu_name = @name, brand = @brand, model = @model, speed = @speed, socket_type = @type, price = @price, component_type_id = @component_type_id, supplier_id = @supplier_id, image = @image
			WHERE cpu_id = @product_id;
        END
        ELSE IF @component_type_name = 'HDD'
        BEGIN
            UPDATE HDD
            SET hdd_name = @name, brand = @brand, model = @model, capacity = @cap, interface_type = @type, price = @price, component_type_id = @component_type_id, supplier_id = @supplier_id, image = @image
            WHERE hdd_id = @product_id;
        END
        ELSE IF @component_type_name = 'SSD'
        BEGIN
            UPDATE SSD
            SET ssd_name = @name, brand = @brand, model = @model, capacity = @cap, interface_type = @type, price = @price, component_type_id = @component_type_id, supplier_id = @supplier_id, image = @image
            WHERE ssd_id = @product_id;
        END
        ELSE IF @component_type_name = 'GPU'
        BEGIN
            UPDATE GPU
            SET gpu_name = @name, brand = @brand, model = @model, vram = @cap, interface_type = @type, price = @price, component_type_id = @component_type_id, supplier_id = @supplier_id, image = @image
            WHERE gpu_id = @product_id;
        END
        ELSE IF @component_type_name = 'Motherboard'
        BEGIN
            UPDATE Motherboard
            SET motherboard_name = @name, brand = @brand, model = @model, socket_type = @type, max_ram_capacity= @cap, price = @price, component_type_id = @component_type_id, supplier_id = @supplier_id, image = @image
            WHERE motherboard_id = @product_id;
        END
        ELSE IF @component_type_name = 'Cooling'
        BEGIN
            UPDATE Cooling
            SET cooling_name = @name, brand = @brand, model = @model, price = @price, component_type_id = @component_type_id, supplier_id = @supplier_id, image = @image
            WHERE cooling_id = @product_id;
        END
        ELSE IF @component_type_name = 'Network Adapter'
        BEGIN
            UPDATE Network_Adapter
            SET adapter_name = @name, brand = @brand, model = @model, price = @price, component_type_id = @component_type_id, supplier_id = @supplier_id, image = @image
            WHERE adapter_id = @product_id;
        END
        ELSE IF @component_type_name = 'Expansion Cards'
        BEGIN
            UPDATE Expansion_Cards
            SET card_name = @name, brand = @brand, model = @model, price = @price, component_type_id = @component_type_id, supplier_id = @supplier_id, image = @image
            WHERE card_id = @product_id;
        END
        ELSE IF @component_type_name = 'Power Backup'
        BEGIN
            UPDATE Power_Backup
            SET backup_name = @name, brand = @brand, model = @model, price = @price, component_type_id = @component_type_id, supplier_id = @supplier_id, image = @image
            WHERE backup_id = @product_id;
        END
        ELSE IF @component_type_name = 'PSU'
        BEGIN
            UPDATE PSU
            SET psu_name = @name, brand = @brand, model = @model, wattage = @cap, efficiency_rating = @speed, price = @price, component_type_id = @component_type_id, supplier_id = @supplier_id, image = @image
            WHERE psu_id = @product_id;
        END
        ELSE IF @component_type_name = 'Monitor'
        BEGIN
            UPDATE Monitor
            SET monitor_name = @name, brand = @brand, model = @model, size_inch = @cap, resolution = @speed, price = @price, component_type_id = @component_type_id, supplier_id = @supplier_id, image = @image
            WHERE monitor_id = @product_id;
        END
        ELSE IF @component_type_name = 'Computer Case'
        BEGIN
            UPDATE Computer_Case
            SET case_name = @name, brand = @brand, model = @model, size = @cap, color = @speed, price = @price, component_type_id = @component_type_id, supplier_id = @supplier_id, image = @image
            WHERE case_id = @product_id;
        END
        ELSE 
        BEGIN
            UPDATE Accessories
            SET accessory_name = @name, brand = @brand, model = @model, price = @price, supplier_id = @supplier_id, component_type_id = @component_type_id, image = @image
            WHERE accessory_id = @product_id;
        END
    END
    ELSE
    BEGIN
        PRINT 'Không tìm thấy component_type_id cho ' + @component_type_name;
    END
END
GO


CREATE OR ALTER PROCEDURE DeleteProductByID
    @product_id VARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

	DELETE FROM Products 
	WHERE COALESCE(cpu_id, ram_id, hdd_id, ssd_id, gpu_id, motherboard_id, cooling_id, psu_id, monitor_id, accessory_id, adapter_id, card_id, backup_id, case_id) = @product_id;

    -- Xóa sản phẩm từ bảng CPU
    DELETE FROM CPU WHERE cpu_id = @product_id;

	--Xóa ản phẩm từ bảng RAM
	DELETE FROM RAM WHERE ram_id = @product_id;

    -- Xóa sản phẩm từ bảng HDD
    DELETE FROM HDD WHERE hdd_id = @product_id;

    -- Xóa sản phẩm từ bảng SSD
    DELETE FROM SSD WHERE ssd_id = @product_id;

    -- Xóa sản phẩm từ bảng GPU
    DELETE FROM GPU WHERE gpu_id = @product_id;

    -- Xóa sản phẩm từ bảng Motherboard
    DELETE FROM Motherboard WHERE motherboard_id = @product_id;

	-- Xóa sản phẩm từ bảng Cooling
    DELETE FROM Cooling WHERE cooling_id = @product_id;

    -- Xóa sản phẩm từ bảng PSU
    DELETE FROM PSU WHERE psu_id = @product_id;

    -- Xóa sản phẩm từ bảng Monitor
    DELETE FROM Monitor WHERE monitor_id = @product_id;

    -- Xóa sản phẩm từ bảng Accessories
    DELETE FROM Accessories WHERE accessory_id = @product_id;

    -- Xóa sản phẩm từ bảng Network_Adapter
    DELETE FROM Network_Adapter WHERE adapter_id = @product_id;

    -- Xóa sản phẩm từ bảng Expansion_Cards
    DELETE FROM Expansion_Cards WHERE card_id = @product_id;

    -- Xóa sản phẩm từ bảng Power_Backup
    DELETE FROM Power_Backup WHERE backup_id = @product_id;

    -- Xóa sản phẩm từ bảng Computer_Case
    DELETE FROM Computer_Case WHERE case_id = @product_id;
END
GO

-- Tim kiem san pham theo loai va nha san xuat
CREATE OR ALTER PROCEDURE GetProductFromTypeAndSupplier
    @TypeName VARCHAR(255),
	@SupplierName VARCHAR(255)
AS
BEGIN
    SELECT 
		P.product_id AS STT,
		COALESCE(C.cpu_id, R.ram_id, H.hdd_id, S.ssd_id, G.gpu_id, M.motherboard_id, 
		CC.case_id, A.adapter_id, EC.card_id, PB.backup_id, CO.cooling_id, PSU.psu_id, MON.monitor_id, AC.accessory_id) AS ID,

		COALESCE(C.cpu_name, R.ram_name, H.hdd_name, S.ssd_name, G.gpu_name, M.motherboard_name, 
		CC.case_name, A.adapter_name, EC.card_name, PB.backup_name, CO.cooling_name, 
		PSU.psu_name, MON.monitor_name, AC.accessory_name) AS product_name,

		COALESCE(C.price, R.price, H.price, S.price, G.price, M.price, CC.price, A.price, EC.price, PB.price, 
		CO.price, PSU.price, MON.price, AC.price) AS price,


		CT.component_type_name AS component_type,

		P.quantity

	FROM Products AS P
		LEFT JOIN CPU C ON P.cpu_id = C.cpu_id
		LEFT JOIN RAM R ON P.ram_id = R.ram_id
		LEFT JOIN HDD H ON P.hdd_id = H.hdd_id
		LEFT JOIN SSD S ON P.ssd_id = S.ssd_id
		LEFT JOIN GPU G ON P.gpu_id = G.gpu_id
		LEFT JOIN Motherboard M ON P.motherboard_id = M.motherboard_id
		LEFT JOIN Cooling CO ON P.cooling_id = CO.cooling_id
		LEFT JOIN PSU PSU ON P.psu_id = PSU.psu_id
		LEFT JOIN Monitor MON ON P.monitor_id = MON.monitor_id
		LEFT JOIN Accessories AC ON P.accessory_id = AC.accessory_id
		LEFT JOIN Network_Adapter A ON P.adapter_id = A.adapter_id
		LEFT JOIN Expansion_Cards EC ON P.card_id = EC.card_id
		LEFT JOIN Power_Backup PB ON P.backup_id = PB.backup_id
		LEFT JOIN Computer_Case CC ON P.case_id = CC.case_id

		LEFT JOIN Component_Types CT 
			ON CT.component_type_id = COALESCE(C.component_type_id, R.component_type_id, H.component_type_id, S.component_type_id, 
			G.component_type_id, M.component_type_id, CO.component_type_id, PSU.component_type_id, MON.component_type_id, 
			AC.component_type_id, A.component_type_id, EC.component_type_id, PB.component_type_id, CC.component_type_id)
			LEFT JOIN Suppliers Sp 
			ON Sp.supplier_id = COALESCE(C.supplier_id, R.supplier_id, H.supplier_id, S.supplier_id, 
			G.supplier_id, M.supplier_id, CO.supplier_id, PSU.supplier_id, MON.supplier_id, 
			AC.supplier_id, A.supplier_id, EC.supplier_id, PB.supplier_id, CC.supplier_id)

        WHERE (@TypeName NOT IN ('CPU', 'RAM', 'HDD', 'SSD', 'GPU', 'Motherboard', 'Cooling', 'Network Adapter', 'Expansion Cards', 'Power Backup', 'PSU', 'Monitor', 'Computer Case')
        AND AC.component_type_id IS NOT NULL)
        OR CT.component_type_name = @TypeName 
        OR Sp.supplier_name = @SupplierName
END
GO

-- Lấy ID sản phẩm từ ID của từng linh kiện
CREATE OR ALTER PROC GetProductID
	@productid VARCHAR(255)
AS
BEGIN
		SELECT p.product_id
		FROM Products P
		LEFT JOIN CPU C ON P.cpu_id = C.cpu_id
		LEFT JOIN RAM R ON P.ram_id = R.ram_id
		LEFT JOIN HDD H ON P.hdd_id = H.hdd_id
		LEFT JOIN SSD S ON P.ssd_id = S.ssd_id
		LEFT JOIN GPU G ON P.gpu_id = G.gpu_id
		LEFT JOIN Motherboard M ON P.motherboard_id = M.motherboard_id
		LEFT JOIN Cooling CO ON P.cooling_id = CO.cooling_id
		LEFT JOIN PSU PSU ON P.psu_id = PSU.psu_id
		LEFT JOIN Monitor MON ON P.monitor_id = MON.monitor_id
		LEFT JOIN Accessories AC ON P.accessory_id = AC.accessory_id
		LEFT JOIN Network_Adapter A ON P.adapter_id = A.adapter_id
		LEFT JOIN Expansion_Cards EC ON P.card_id = EC.card_id
		LEFT JOIN Power_Backup PB ON P.backup_id = PB.backup_id
		LEFT JOIN Computer_Case CC ON P.case_id = CC.case_id
		    WHERE C.cpu_id = @productid 
				   OR R.ram_id = @productid 
				   OR H.hdd_id = @productid 
				   OR S.ssd_id = @productid 
				   OR G.gpu_id = @productid 
				   OR M.motherboard_id = @productid 
				   OR CC.case_id = @productid 
				   OR AC.accessory_id = @productid 
				   OR A.adapter_id = @productid 
				   OR EC.card_id = @productid 
				   OR PB.backup_id = @productid 
				   OR p.psu_id = @productid 
				   OR MON.monitor_id = @productid 
				   OR CO.cooling_id = @productid;
END
GO

--Thêm sản phẩm vào giỏ hàng và lấy giá sản phẩm tử các table con 
CREATE OR ALTER PROC InsertCart
	@ProductID  int,
	@CustomerID int,
	@Quantity int
AS
BEGIN
		DECLARE @UnitPrice DECIMAL(18, 2);

		-- Lấy giá từ bảng Products
        SELECT @UnitPrice = COALESCE(C.price, R.price, H.price, S.price, G.price, M.price, 
		CC.price, A.price, EC.price, PB.price, CO.price, PSU.price, MON.price, AC.price)
		FROM Products P
		LEFT JOIN CPU C ON P.cpu_id = C.cpu_id
		LEFT JOIN RAM R ON P.ram_id = R.ram_id
		LEFT JOIN HDD H ON P.hdd_id = H.hdd_id
		LEFT JOIN SSD S ON P.ssd_id = S.ssd_id
		LEFT JOIN GPU G ON P.gpu_id = G.gpu_id
		LEFT JOIN Motherboard M ON P.motherboard_id = M.motherboard_id
		LEFT JOIN Cooling CO ON P.cooling_id = CO.cooling_id
		LEFT JOIN PSU PSU ON P.psu_id = PSU.psu_id
		LEFT JOIN Monitor MON ON P.monitor_id = MON.monitor_id
		LEFT JOIN Accessories AC ON P.accessory_id = AC.accessory_id
		LEFT JOIN Network_Adapter A ON P.adapter_id = A.adapter_id
		LEFT JOIN Expansion_Cards EC ON P.card_id = EC.card_id
		LEFT JOIN Power_Backup PB ON P.backup_id = PB.backup_id
		LEFT JOIN Computer_Case CC ON P.case_id = CC.case_id
		    WHERE P.product_id = @ProductID 

		-- Nếu không tìm thấy giá từ bảng Products, gán giá trị mặc định
		IF @UnitPrice IS NULL
		BEGIN
			SET @UnitPrice = 0; -- hoặc bất kỳ giá trị mặc định nào bạn chọn
		END

		INSERT INTO ShoppingCart (customer_id, product_id, quantity, unit_price, discount)
		VALUES (@CustomerID, @ProductID, @Quantity, @UnitPrice, 0);

END
GO

--lấy tên sản phẩm cho giỏ hàng
CREATE OR ALTER PROC GetProductNameForCart
	@CustomerID int
AS
BEGIN
		SELECT cart_item_id, COALESCE(C.cpu_name, R.ram_name, H.hdd_name, S.ssd_name, G.gpu_name, M.motherboard_name, CO.cooling_name, PSU.psu_name, MON.monitor_name, AC.accessory_name, A.adapter_name, EC.card_name, PB.backup_name, 
		CC.case_name) AS Name,
		SC.quantity, unit_price, p.discount 
		FROM ShoppingCart SC 
		LEFT JOIN Products P ON p.product_id = SC.product_id
		LEFT JOIN CPU C ON P.cpu_id = C.cpu_id
		LEFT JOIN RAM R ON P.ram_id = R.ram_id
		LEFT JOIN HDD H ON P.hdd_id = H.hdd_id
		LEFT JOIN SSD S ON P.ssd_id = S.ssd_id
		LEFT JOIN GPU G ON P.gpu_id = G.gpu_id
		LEFT JOIN Motherboard M ON P.motherboard_id = M.motherboard_id
		LEFT JOIN Cooling CO ON P.cooling_id = CO.cooling_id
		LEFT JOIN PSU PSU ON P.psu_id = PSU.psu_id
		LEFT JOIN Monitor MON ON P.monitor_id = MON.monitor_id
		LEFT JOIN Accessories AC ON P.accessory_id = AC.accessory_id
		LEFT JOIN Network_Adapter A ON P.adapter_id = A.adapter_id
		LEFT JOIN Expansion_Cards EC ON P.card_id = EC.card_id
		LEFT JOIN Power_Backup PB ON P.backup_id = PB.backup_id
		LEFT JOIN Computer_Case CC ON P.case_id = CC.case_id
		WHERE SC.customer_id = @CustomerID
END
GO

EXEC GetProductNameForCart 3


--Tạo bảng hóa đơn
CREATE OR ALTER PROCEDURE InsertOrder
    @CustomerID INT,
    @RecipientName NVARCHAR(255),
    @DeliveryAddress VARCHAR(255)
AS
BEGIN
	DECLARE @OrderID INT
    BEGIN TRY

        -- Tạo một đơn hàng mới
        INSERT INTO Orders (customer_id, order_date, Recipient_Name, delivery_address)
        VALUES (@CustomerID, GETDATE(), @RecipientName, @DeliveryAddress);

        -- Lấy ID của đơn hàng vừa tạo
        SET @OrderID = SCOPE_IDENTITY();

        -- Thêm các mặt hàng từ giỏ hàng vào chi tiết đơn hàng
        INSERT INTO Order_Details (order_id, product_id, quantity, unit_price, discount)
        SELECT @OrderID, product_id, quantity, unit_price, discount
        FROM ShoppingCart
        WHERE customer_id = @CustomerID;

        -- Tính tổng giá trị của đơn hàng
        UPDATE Orders
        SET total_price = (
            SELECT SUM(quantity * unit_price) 
            FROM Order_Details 
            WHERE order_id = @OrderID
        )
        WHERE order_id = @OrderID;
        
        -- Thêm hoá đơn
        INSERT INTO Invoices (order_id, customer_id, invoice_date, total_price, delivery_address, recipient_name)
        VALUES (@OrderID, @CustomerID, GETDATE(), 
            (SELECT total_price FROM Orders WHERE order_id = @OrderID), 
            @DeliveryAddress, @RecipientName);

        -- Xóa các mặt hàng đã được chuyển từ giỏ hàng
        DELETE FROM ShoppingCart WHERE customer_id = @CustomerID;
    END TRY
    BEGIN CATCH
        -- Nếu có lỗi xảy ra, hủy bỏ tất cả các thay đổi và không xóa dữ liệu từ giỏ hàng
        ROLLBACK TRANSACTION;
    END CATCH;
END;
GO

--lấy tên sản phẩm cho Chi tiết hóa đơn
CREATE OR ALTER PROC GetProductNameForDetail
	@OrderID int
AS
BEGIN
		SELECT order_detail_id, COALESCE(C.cpu_name, R.ram_name, H.hdd_name, S.ssd_name, G.gpu_name, M.motherboard_name, CO.cooling_name, PSU.psu_name, MON.monitor_name, AC.accessory_name, A.adapter_name, EC.card_name, PB.backup_name, 
		CC.case_name) AS Name,
		OD.quantity, unit_price, p.discount 
		FROM Order_Details OD
		LEFT JOIN Products P ON p.product_id = OD.product_id
		LEFT JOIN CPU C ON P.cpu_id = C.cpu_id
		LEFT JOIN RAM R ON P.ram_id = R.ram_id
		LEFT JOIN HDD H ON P.hdd_id = H.hdd_id
		LEFT JOIN SSD S ON P.ssd_id = S.ssd_id
		LEFT JOIN GPU G ON P.gpu_id = G.gpu_id
		LEFT JOIN Motherboard M ON P.motherboard_id = M.motherboard_id
		LEFT JOIN Cooling CO ON P.cooling_id = CO.cooling_id
		LEFT JOIN PSU PSU ON P.psu_id = PSU.psu_id
		LEFT JOIN Monitor MON ON P.monitor_id = MON.monitor_id
		LEFT JOIN Accessories AC ON P.accessory_id = AC.accessory_id
		LEFT JOIN Network_Adapter A ON P.adapter_id = A.adapter_id
		LEFT JOIN Expansion_Cards EC ON P.card_id = EC.card_id
		LEFT JOIN Power_Backup PB ON P.backup_id = PB.backup_id
		LEFT JOIN Computer_Case CC ON P.case_id = CC.case_id
		WHERE OD.order_id = @OrderID
END
GO