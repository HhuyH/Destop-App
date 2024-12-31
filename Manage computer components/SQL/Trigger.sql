-- Tạo trigger để kiểm tra tính duy nhất của tài khoản ngân hàng
CREATE TRIGGER CheckUniqueBankAccountNumber
ON PaymentMethods
AFTER INSERT, UPDATE
AS
BEGIN
    IF EXISTS (
        SELECT bank_account_number
        FROM inserted
        WHERE bank_account_number IS NOT NULL
        GROUP BY bank_account_number
        HAVING COUNT(*) > 1
    )
    BEGIN
        RAISERROR('Không thể chèn hoặc cập nhật dữ liệu vì tồn tại tài khoản ngân hàng trùng lặp.', 16, 1)
        ROLLBACK TRANSACTION
    END
END
GO

CREATE TRIGGER CheckUniqueCardNumber
ON PaymentMethods
AFTER INSERT, UPDATE
AS
BEGIN
    IF EXISTS (
        SELECT card_number
        FROM inserted
        WHERE card_number IS NOT NULL
        GROUP BY card_number
        HAVING COUNT(*) > 1
    )
    BEGIN
        RAISERROR('Không thể chèn hoặc cập nhật dữ liệu vì tồn tại số thẻ trùng lặp.', 16, 1)
        ROLLBACK TRANSACTION
    END
END
GO

-- Tạo trigger ID tự động cho CPU
CREATE TRIGGER Generate_CPU_ID
ON CPU
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @brand VARCHAR(255);
    DECLARE @model VARCHAR(255);
    DECLARE @count INT;

    SELECT @count = COUNT(*) FROM CPU;

    IF @count IS NULL
    BEGIN
        SET @count = 0;
    END

    DECLARE @new_cpu_id VARCHAR(100);

    -- Lặp qua các dòng được chèn mới
    DECLARE @inserted TABLE (
        cpu_id VARCHAR(100),
        cpu_name VARCHAR(255),
        brand VARCHAR(255),
        model VARCHAR(255),
        speed FLOAT,
        socket_type VARCHAR(50),
        price DECIMAL(10, 2),
        component_type_id INT,
        supplier_id INT,
        image VARCHAR(255)
    );
    
    INSERT INTO @inserted
    SELECT * FROM inserted;

    -- Tạo CPU ID cho mỗi dòng được chèn mới
    WHILE EXISTS (SELECT 1 FROM @inserted)
    BEGIN
        SELECT TOP 1 @brand = brand, @model = model FROM @inserted;

        -- Kiểm tra xem CPU đã tồn tại trong bảng hay không
        IF EXISTS (SELECT 1 FROM CPU WHERE brand = @brand AND model = @model)
        BEGIN
            -- Sử dụng CPU ID đã có nếu CPU đã tồn tại
            SELECT @new_cpu_id = cpu_id FROM CPU WHERE brand = @brand AND model = @model;
        END
        ELSE
        BEGIN
            -- Tạo CPU ID mới nếu CPU chưa tồn tại
            SET @new_cpu_id = CONCAT(@brand, '_C', @count + 1, '_', @model);
            SET @count = @count + 1;
        END

        -- Chèn dữ liệu vào bảng CPU với CPU ID mới hoặc đã có
        INSERT INTO CPU (cpu_id, cpu_name, brand, model, speed, socket_type, price, component_type_id, supplier_id, image)
        SELECT @new_cpu_id, cpu_name, brand, model, speed, socket_type, price, component_type_id, supplier_id, image
        FROM @inserted
        WHERE brand = @brand AND model = @model;

        -- Loại bỏ dòng đã được xử lý ra khỏi bảng tạm
        DELETE FROM @inserted WHERE brand = @brand AND model = @model;
    END
END;
GO

-- Tạo trigger ID tự động cho RAM
CREATE TRIGGER Generate_RAM_ID
ON RAM
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @brand VARCHAR(255);
    DECLARE @model VARCHAR(255);
    DECLARE @count INT;

    SELECT @count = COUNT(*) FROM RAM;

    IF @count IS NULL
    BEGIN
        SET @count = 0;
    END

    DECLARE @new_ram_id VARCHAR(100);

    -- Lặp qua các dòng được chèn mới
    DECLARE @inserted TABLE (
        ram_id VARCHAR(100),
        ram_name VARCHAR(255),
        brand VARCHAR(255),
        model VARCHAR(255),
        capacity INT,
        speed INT,
        type VARCHAR(50),
        price DECIMAL(10, 2),
        component_type_id INT,
        supplier_id INT,
        image VARCHAR(255)
    );
    
    INSERT INTO @inserted
    SELECT * FROM inserted;

    -- Tạo RAM ID cho mỗi dòng được chèn mới
    WHILE EXISTS (SELECT 1 FROM @inserted)
    BEGIN
        SELECT TOP 1 @brand = brand, @model = model FROM @inserted;

        -- Kiểm tra xem RAM đã tồn tại trong bảng hay không
        IF EXISTS (SELECT 1 FROM RAM WHERE brand = @brand AND model = @model)
        BEGIN
            -- Sử dụng RAM ID đã có nếu RAM đã tồn tại
            SELECT @new_ram_id = ram_id FROM RAM WHERE brand = @brand AND model = @model;
        END
        ELSE
        BEGIN
            -- Tạo RAM ID mới nếu RAM chưa tồn tại
            SET @new_ram_id = CONCAT(@brand, '_R', @count + 1, '_', @model);
            SET @count = @count + 1;
        END

        -- Chèn dữ liệu vào bảng RAM với RAM ID mới hoặc đã có
        INSERT INTO RAM (ram_id, ram_name, brand, model, capacity, speed, type, price, component_type_id, supplier_id, image)
        SELECT @new_ram_id, ram_name, brand, model, capacity, speed, type, price, component_type_id, supplier_id, image
        FROM @inserted
        WHERE brand = @brand AND model = @model;

        -- Loại bỏ dòng đã được xử lý ra khỏi bảng tạm
        DELETE FROM @inserted WHERE brand = @brand AND model = @model;
    END
END;
GO

-- Tạo trigger ID tự động cho HDD
CREATE TRIGGER Generate_HDD_ID
ON HDD
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @brand VARCHAR(255);
    DECLARE @model VARCHAR(255);
    DECLARE @count INT;

    SELECT @count = COUNT(*) FROM HDD;

    IF @count IS NULL
    BEGIN
        SET @count = 0;
    END

    DECLARE @new_hdd_id VARCHAR(100);

    -- Lặp qua các dòng được chèn mới
    DECLARE @inserted TABLE (
        hdd_id VARCHAR(100),
        hdd_name VARCHAR(255),
        brand VARCHAR(255),
        model VARCHAR(255),
        capacity VARCHAR(50),
        interface_type VARCHAR(50),
        price DECIMAL(10, 2),
        component_type_id INT,
        supplier_id INT,
        image VARCHAR(255)
    );
    
    INSERT INTO @inserted
    SELECT * FROM inserted;

    -- Tạo HDD ID cho mỗi dòng được chèn mới
    WHILE EXISTS (SELECT 1 FROM @inserted)
    BEGIN
        SELECT TOP 1 @brand = brand, @model = model FROM @inserted;

        -- Kiểm tra xem HDD đã tồn tại trong bảng hay không
        IF EXISTS (SELECT 1 FROM HDD WHERE brand = @brand AND model = @model)
        BEGIN
            -- Sử dụng HDD ID đã có nếu HDD đã tồn tại
            SELECT @new_hdd_id = hdd_id FROM HDD WHERE brand = @brand AND model = @model;
        END
        ELSE
        BEGIN
            -- Tạo HDD ID mới nếu HDD chưa tồn tại
            SET @new_hdd_id = CONCAT(@brand, '_H', @count + 1, '_', @model);
            SET @count = @count + 1;
        END

        -- Chèn dữ liệu vào bảng HDD với HDD ID mới hoặc đã có
        INSERT INTO HDD (hdd_id, hdd_name, brand, model, capacity, interface_type, price, component_type_id, supplier_id, image)
        SELECT @new_hdd_id, hdd_name, brand, model, capacity, interface_type, price, component_type_id, supplier_id, image
        FROM @inserted
        WHERE brand = @brand AND model = @model;

        -- Loại bỏ dòng đã được xử lý ra khỏi bảng tạm
        DELETE FROM @inserted WHERE brand = @brand AND model = @model;
    END
END;
GO

-- Tạo trigger ID tự động cho SSD
CREATE TRIGGER Generate_SSD_ID
ON SSD
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @brand VARCHAR(255);
    DECLARE @model VARCHAR(255);
    DECLARE @count INT;

    SELECT @count = COUNT(*) FROM SSD;

    IF @count IS NULL
    BEGIN
        SET @count = 0;
    END

    DECLARE @new_ssd_id VARCHAR(100);

    -- Lặp qua các dòng được chèn mới
    DECLARE @inserted TABLE (
        ssd_id VARCHAR(100),
        ssd_name VARCHAR(255),
        brand VARCHAR(255),
        model VARCHAR(255),
        capacity VARCHAR(50),
        interface_type VARCHAR(50),
        price DECIMAL(10, 2),
        component_type_id INT,
        supplier_id INT,
        image VARCHAR(255)
    );
    
    INSERT INTO @inserted
    SELECT * FROM inserted;

    -- Tạo SSD ID cho mỗi dòng được chèn mới
    WHILE EXISTS (SELECT 1 FROM @inserted)
    BEGIN
        SELECT TOP 1 @brand = brand, @model = model FROM @inserted;

        -- Kiểm tra xem SSD đã tồn tại trong bảng hay không
        IF EXISTS (SELECT 1 FROM SSD WHERE brand = @brand AND model = @model)
        BEGIN
            -- Sử dụng SSD ID đã có nếu SSD đã tồn tại
            SELECT @new_ssd_id = ssd_id FROM SSD WHERE brand = @brand AND model = @model;
        END
        ELSE
        BEGIN
            -- Tạo SSD ID mới nếu SSD chưa tồn tại
            SET @new_ssd_id = CONCAT(@brand, '_S', @count + 1, '_', @model);
            SET @count = @count + 1;
        END

        -- Chèn dữ liệu vào bảng SSD với SSD ID mới hoặc đã có
        INSERT INTO SSD (ssd_id, ssd_name, brand, model, capacity, interface_type, price, component_type_id, supplier_id, image)
        SELECT @new_ssd_id, ssd_name, brand, model, capacity, interface_type, price, component_type_id, supplier_id, image
        FROM @inserted
        WHERE brand = @brand AND model = @model;

        -- Loại bỏ dòng đã được xử lý ra khỏi bảng tạm
        DELETE FROM @inserted WHERE brand = @brand AND model = @model;
    END
END;
GO

-- Tạo trigger ID tự động cho GPU --------------------------------------------
CREATE TRIGGER Generate_GPU_ID
ON GPU
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @brand VARCHAR(255);
    DECLARE @model VARCHAR(255);
    DECLARE @count INT;

    SELECT @count = COUNT(*) FROM GPU;

    IF @count IS NULL
    BEGIN
        SET @count = 0;
    END

    DECLARE @new_gpu_id VARCHAR(100);

    -- Lặp qua các dòng được chèn mới
    DECLARE @inserted TABLE (
        gpu_id VARCHAR(100),
        gpu_name VARCHAR(255),
        brand VARCHAR(255),
        model VARCHAR(255),
        vram INT,
        interface_type VARCHAR(50),
        price DECIMAL(18, 2),
        component_type_id INT,
        supplier_id INT,
        image VARCHAR(255)
    );
    
    INSERT INTO @inserted
    SELECT * FROM inserted;

    -- Tạo GPU ID cho mỗi dòng được chèn mới
    WHILE EXISTS (SELECT 1 FROM @inserted)
    BEGIN
        SELECT TOP 1 @brand = brand, @model = model FROM @inserted;

        -- Kiểm tra xem GPU đã tồn tại trong bảng hay không
        IF EXISTS (SELECT 1 FROM GPU WHERE brand = @brand AND model = @model)
        BEGIN
            -- Sử dụng GPU ID đã có nếu GPU đã tồn tại
            SELECT @new_gpu_id = gpu_id FROM GPU WHERE brand = @brand AND model = @model;
        END
        ELSE
        BEGIN
            -- Tạo GPU ID mới nếu GPU chưa tồn tại
            SET @new_gpu_id = CONCAT(@brand, '_G', @count + 1, '_', @model);
            SET @count = @count + 1;
        END

        -- Chèn dữ liệu vào bảng GPU với GPU ID mới hoặc đã có
        INSERT INTO GPU (gpu_id, gpu_name, brand, model, vram, interface_type, price, component_type_id, supplier_id, image)
        SELECT @new_gpu_id, gpu_name, brand, model, vram, interface_type, price, component_type_id, supplier_id, image
        FROM @inserted
        WHERE brand = @brand AND model = @model;

        -- Loại bỏ dòng đã được xử lý ra khỏi bảng tạm
        DELETE FROM @inserted WHERE brand = @brand AND model = @model;
    END
END;
GO

-- Tạo trigger ID tự động cho Mainboard (Motherboard)
CREATE TRIGGER Generate_Motherboard_ID
ON Motherboard
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @brand VARCHAR(255);
    DECLARE @model VARCHAR(255);
    DECLARE @count INT;

    SELECT @count = COUNT(*) FROM Motherboard;

    IF @count IS NULL
    BEGIN
        SET @count = 0;
    END

    DECLARE @new_motherboard_id VARCHAR(100);

    -- Lặp qua các dòng được chèn mới
    DECLARE @inserted TABLE (
        motherboard_id VARCHAR(100),
        motherboard_name VARCHAR(255),
        brand VARCHAR(255),
        model VARCHAR(255),
        socket_type VARCHAR(50),
        max_ram_capacity INT,
        price DECIMAL(18, 2),
        component_type_id INT,
        supplier_id INT,
        image VARCHAR(255)
    );
    
    INSERT INTO @inserted
    SELECT * FROM inserted;

    -- Tạo Motherboard ID cho mỗi dòng được chèn mới
    WHILE EXISTS (SELECT 1 FROM @inserted)
    BEGIN
        SELECT TOP 1 @brand = brand, @model = model FROM @inserted;

        -- Kiểm tra xem Mainboard đã tồn tại trong bảng hay không
        IF EXISTS (SELECT 1 FROM Motherboard WHERE brand = @brand AND model = @model)
        BEGIN
            -- Sử dụng Motherboard ID đã có nếu Mainboard đã tồn tại
            SELECT @new_motherboard_id = motherboard_id FROM Motherboard WHERE brand = @brand AND model = @model;
        END
        ELSE
        BEGIN
            -- Tạo Motherboard ID mới nếu Mainboard chưa tồn tại
            SET @new_motherboard_id = CONCAT(@brand, '_M', @count + 1, '_', @model);
            SET @count = @count + 1;
        END

        -- Chèn dữ liệu vào bảng Motherboard với Motherboard ID mới hoặc đã có
        INSERT INTO Motherboard (motherboard_id, motherboard_name, brand, model, socket_type, max_ram_capacity, price, component_type_id, supplier_id, image)
        SELECT @new_motherboard_id, motherboard_name, brand, model, socket_type, max_ram_capacity, price, component_type_id, supplier_id, image
        FROM @inserted
        WHERE brand = @brand AND model = @model;

        -- Loại bỏ dòng đã được xử lý ra khỏi bảng tạm
        DELETE FROM @inserted WHERE brand = @brand AND model = @model;
    END
END;
GO

-- Tạo trigger ID tự động cho Cooling
CREATE OR ALTER TRIGGER Generate_Cooling_ID
ON Cooling
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @brand VARCHAR(255);
    DECLARE @model VARCHAR(255);
    DECLARE @count INT;

    SELECT @count = ISNULL(MAX(CAST(SUBSTRING(cooling_id, CHARINDEX('_', cooling_id) + 2, CHARINDEX('_', cooling_id, CHARINDEX('_', cooling_id) + 1) - CHARINDEX('_', cooling_id) - 2) AS INT)), 0) FROM Cooling;

    DECLARE @new_cooling_id VARCHAR(100);

    -- Lặp qua các dòng được chèn mới
    DECLARE @inserted TABLE (
        cooling_id VARCHAR(100),
        cooling_name VARCHAR(100),
        brand VARCHAR(255),
        model VARCHAR(255),
        price DECIMAL(18, 2),
        component_type_id INT,
        supplier_id INT,
        image VARCHAR(255) -- Thêm cột image
    );

    INSERT INTO @inserted
    SELECT * FROM inserted;

    -- Tạo Cooling ID cho mỗi dòng được chèn mới
    WHILE EXISTS (SELECT 1 FROM @inserted)
    BEGIN
        SELECT TOP 1 @brand = brand, @model = model FROM @inserted;

        -- Tạo Cooling ID theo quy tắc: BRAND + 'CL' + (số thứ tự tăng dần) + MODEL
        SET @new_cooling_id = CONCAT(@brand, '_CL', @count + 1, '_', @model);

        -- Chèn dữ liệu vào bảng Cooling với Cooling ID mới
        INSERT INTO Cooling (cooling_id, cooling_name, brand, model, price, component_type_id, supplier_id, image)
        SELECT @new_cooling_id, cooling_name, brand, model, price, component_type_id, supplier_id, image
        FROM @inserted
        WHERE brand = @brand AND model = @model;

        -- Loại bỏ dòng đã được xử lý ra khỏi bảng tạm
        DELETE FROM @inserted WHERE brand = @brand AND model = @model;

        SET @count = @count + 1;
    END
END;
GO


-- Tạo trigger ID tự động cho PSU
CREATE OR ALTER TRIGGER Generate_PSU_ID
ON PSU
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @brand VARCHAR(255);
    DECLARE @model VARCHAR(255);
    DECLARE @count INT;

    SELECT @count = COUNT(*) FROM PSU;

    IF @count IS NULL
    BEGIN
        SET @count = 0;
    END

    DECLARE @new_psu_id VARCHAR(100);

    -- Lặp qua các dòng được chèn mới
    DECLARE @inserted TABLE (
        psu_id VARCHAR(100),
        psu_name VARCHAR(255),
        brand VARCHAR(255),
        model VARCHAR(255),
        wattage INT,
        efficiency_rating VARCHAR(50),
        price DECIMAL(18, 2),
        component_type_id INT,
        supplier_id INT,
        image VARCHAR(255)
    );
    
    INSERT INTO @inserted
    SELECT * FROM inserted;

    -- Tạo PSU ID cho mỗi dòng được chèn mới
    WHILE EXISTS (SELECT 1 FROM @inserted)
    BEGIN
        SELECT TOP 1 @brand = brand, @model = model FROM @inserted;

        -- Kiểm tra xem PSU đã tồn tại trong bảng hay không
        IF EXISTS (SELECT 1 FROM PSU WHERE brand = @brand AND model = @model)
        BEGIN
            -- Sử dụng PSU ID đã có nếu PSU đã tồn tại
            SELECT @new_psu_id = psu_id FROM PSU WHERE brand = @brand AND model = @model;
        END
        ELSE
        BEGIN
            -- Tạo PSU ID mới nếu PSU chưa tồn tại
            SET @new_psu_id = CONCAT(@brand, '_PS', @count + 1, '_', @model);
            SET @count = @count + 1;
        END

        -- Chèn dữ liệu vào bảng PSU với PSU ID mới hoặc đã có
        INSERT INTO PSU (psu_id, psu_name, brand, model, wattage, efficiency_rating, price, component_type_id, supplier_id, image)
        SELECT @new_psu_id, psu_name, brand, model, wattage, efficiency_rating, price, component_type_id, supplier_id, image
        FROM @inserted
        WHERE brand = @brand AND model = @model;

        -- Loại bỏ dòng đã được xử lý ra khỏi bảng tạm
        DELETE FROM @inserted WHERE brand = @brand AND model = @model;
    END
END;
GO

-- Tạo trigger ID tự động cho Monitor
CREATE OR ALTER TRIGGER Generate_Monitor_ID
ON Monitor
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @brand VARCHAR(255);
    DECLARE @model VARCHAR(255);
    DECLARE @count INT;

    SELECT @count = COUNT(*) FROM Monitor;

    IF @count IS NULL
    BEGIN
        SET @count = 0;
    END

    DECLARE @new_monitor_id VARCHAR(100);

    -- Lặp qua các dòng được chèn mới
    DECLARE @inserted TABLE (
        monitor_id VARCHAR(100),
        monitor_name VARCHAR(255),
        brand VARCHAR(255),
        model VARCHAR(255),
        size_inch FLOAT,
        resolution VARCHAR(20),
        price DECIMAL(18, 2),
        component_type_id INT,
        supplier_id INT,
        image VARCHAR(255)
    );
    
    INSERT INTO @inserted
    SELECT * FROM inserted;

    -- Tạo Monitor ID cho mỗi dòng được chèn mới
    WHILE EXISTS (SELECT 1 FROM @inserted)
    BEGIN
        SELECT TOP 1 @brand = brand, @model = model FROM @inserted;

        -- Kiểm tra xem Monitor đã tồn tại trong bảng hay không
        IF EXISTS (SELECT 1 FROM Monitor WHERE brand = @brand AND model = @model)
        BEGIN
            -- Sử dụng Monitor ID đã có nếu Monitor đã tồn tại
            SELECT @new_monitor_id = monitor_id FROM Monitor WHERE brand = @brand AND model = @model;
        END
        ELSE
        BEGIN
            -- Tạo Monitor ID mới nếu Monitor chưa tồn tại
            SET @new_monitor_id = CONCAT(@brand, '_MT', @count + 1, '_', @model);
            SET @count = @count + 1;
        END

        -- Chèn dữ liệu vào bảng Monitor với Monitor ID mới hoặc đã có
        INSERT INTO Monitor (monitor_id, monitor_name, brand, model, size_inch, resolution, price, component_type_id, supplier_id, image)
        SELECT @new_monitor_id, monitor_name, brand, model, size_inch, resolution, price, component_type_id, supplier_id, image
        FROM @inserted
        WHERE brand = @brand AND model = @model;

        -- Loại bỏ dòng đã được xử lý ra khỏi bảng tạm
        DELETE FROM @inserted WHERE brand = @brand AND model = @model;
    END
END;
GO

-- Tạo trigger ID tự động cho Case
CREATE OR ALTER TRIGGER Generate_Case_ID
ON Computer_Case
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @brand VARCHAR(255);
    DECLARE @model VARCHAR(255);
    DECLARE @count INT;

    SELECT @count = COUNT(*) FROM Computer_Case;

    IF @count IS NULL
    BEGIN
        SET @count = 0;
    END

    DECLARE @new_case_id VARCHAR(100);

    -- Lặp qua các dòng được chèn mới
    DECLARE @inserted TABLE (
        case_id VARCHAR(100),
        case_name VARCHAR(255),
        brand VARCHAR(255),
        model VARCHAR(255),
        size VARCHAR(20),
        color VARCHAR(50),
        price DECIMAL(18, 2),
        component_type_id INT,
        supplier_id INT,
        image VARCHAR(255)
    );
    
    INSERT INTO @inserted
    SELECT * FROM inserted;

    -- Tạo Case ID cho mỗi dòng được chèn mới
    WHILE EXISTS (SELECT 1 FROM @inserted)
    BEGIN
        SELECT TOP 1 @brand = brand, @model = model FROM @inserted;

        -- Kiểm tra xem Case đã tồn tại trong bảng hay không
        IF EXISTS (SELECT 1 FROM Computer_Case WHERE brand = @brand AND model = @model)
        BEGIN
            -- Sử dụng Case ID đã có nếu Case đã tồn tại
            SELECT @new_case_id = case_id FROM Computer_Case WHERE brand = @brand AND model = @model;
        END
        ELSE
        BEGIN
            -- Tạo Case ID mới nếu Case chưa tồn tại
            SET @new_case_id = CONCAT(@brand, '_CS', @count + 1, '_', @model);
            SET @count = @count + 1;
        END

        -- Chèn dữ liệu vào bảng Computer_Case với Case ID mới hoặc đã có
        INSERT INTO Computer_Case (case_id, case_name, brand, model, size, color, price, component_type_id, supplier_id, image)
        SELECT @new_case_id, case_name, brand, model, size, color, price, component_type_id, supplier_id, image
        FROM @inserted
        WHERE brand = @brand AND model = @model;

        -- Loại bỏ dòng đã được xử lý ra khỏi bảng tạm
        DELETE FROM @inserted WHERE brand = @brand AND model = @model;
    END
END;
GO

-- Tạo trigger ID tự động cho Accessories
CREATE TRIGGER Generate_Accessory_ID
ON Accessories
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @brand VARCHAR(255);
    DECLARE @model VARCHAR(255);
    DECLARE @count INT;

    SELECT @count = COUNT(*) FROM Accessories;

    IF @count IS NULL
    BEGIN
        SET @count = 0;
    END

    DECLARE @new_accessory_id VARCHAR(100);

    -- Lặp qua các dòng được chèn mới
    DECLARE @inserted TABLE (
        accessory_id VARCHAR(100),
        accessory_name VARCHAR(255),
        brand VARCHAR(255),
        model VARCHAR(255),
        price DECIMAL(18, 2),
        supplier_id INT,
        component_type_id INT,
        image VARCHAR(255)
    );
    
    INSERT INTO @inserted
    SELECT * FROM inserted;

    -- Tạo Accessory ID cho mỗi dòng được chèn mới
    WHILE EXISTS (SELECT 1 FROM @inserted)
    BEGIN
        SELECT TOP 1 @brand = brand, @model = model FROM @inserted;

        -- Kiểm tra xem Accessory đã tồn tại trong bảng hay không
        IF EXISTS (SELECT 1 FROM Accessories WHERE brand = @brand AND model = @model)
        BEGIN
            -- Sử dụng Accessory ID đã có nếu Accessory đã tồn tại
            SELECT @new_accessory_id = accessory_id FROM Accessories WHERE brand = @brand AND model = @model;
        END
        ELSE
        BEGIN
            -- Tạo Accessory ID mới nếu Accessory chưa tồn tại
            SET @new_accessory_id = CONCAT(@brand, '_A', @count + 1, '_', @model);
            SET @count = @count + 1;
        END

        -- Chèn dữ liệu vào bảng Accessories với Accessory ID mới hoặc đã có
        INSERT INTO Accessories (accessory_id, accessory_name, brand, model, price, supplier_id, component_type_id, image)
        SELECT @new_accessory_id, accessory_name, brand, model, price, supplier_id, component_type_id, image
        FROM @inserted
        WHERE brand = @brand AND model = @model;

        -- Loại bỏ dòng đã được xử lý ra khỏi bảng tạm
        DELETE FROM @inserted WHERE brand = @brand AND model = @model;
    END
END;
GO

-- Tạo trigger ID tự động cho Network_Adapter
CREATE OR ALTER TRIGGER Generate_Network_Adapter_ID
ON Network_Adapter
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @count INT;

    SELECT @count = COUNT(*) FROM Network_Adapter;

    IF @count IS NULL
    BEGIN
        SET @count = 0;
    END

    DECLARE @new_adapter_id VARCHAR(100);

    -- Lặp qua các dòng được chèn mới
    DECLARE @inserted TABLE (
        adapter_id VARCHAR(100),
        adapter_name VARCHAR(255),
        brand VARCHAR(255),
        model VARCHAR(255),
        component_type_id INT,
        price DECIMAL(18, 2),
        supplier_id INT,
        image VARCHAR(255)
    );
    
    INSERT INTO @inserted
    SELECT * FROM inserted;

    -- Tạo Adapter ID cho mỗi dòng được chèn mới
    WHILE EXISTS (SELECT 1 FROM @inserted)
    BEGIN
        DECLARE @brand VARCHAR(255);
        DECLARE @model VARCHAR(255);

        SELECT TOP 1 @brand = brand, @model = model FROM @inserted;

        -- Kiểm tra xem Network Adapter đã tồn tại trong bảng hay không
        IF EXISTS (SELECT 1 FROM Network_Adapter WHERE brand = @brand AND model = @model)
        BEGIN
            -- Sử dụng Adapter ID đã có nếu Adapter đã tồn tại
            SELECT @new_adapter_id = adapter_id FROM Network_Adapter WHERE brand = @brand AND model = @model;
        END
        ELSE
        BEGIN
            -- Tạo Adapter ID mới nếu Adapter chưa tồn tại
            SET @new_adapter_id = CONCAT(@brand, '_NA', @count + 1, '_', @model);
            SET @count = @count + 1;
        END

        -- Chèn dữ liệu vào bảng Network_Adapter với Adapter ID mới
        INSERT INTO Network_Adapter (adapter_id, adapter_name, brand, model, component_type_id, price, supplier_id, image)
        SELECT @new_adapter_id, adapter_name, brand, model, component_type_id, price, supplier_id, image
        FROM @inserted
        WHERE brand = @brand AND model = @model;

        -- Loại bỏ dòng đã được xử lý ra khỏi bảng tạm
        DELETE FROM @inserted WHERE brand = @brand AND model = @model;
    END
END;
GO

-- Tạo trigger ID tự động cho Expansion_Cards
CREATE OR ALTER TRIGGER Generate_Expansion_Cards_ID
ON Expansion_Cards
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @count INT;

    SELECT @count = COUNT(*) FROM Expansion_Cards;

    IF @count IS NULL
    BEGIN
        SET @count = 0;
    END

    DECLARE @new_card_id VARCHAR(100);

    -- Lặp qua các dòng được chèn mới
    DECLARE @inserted TABLE (
        card_id VARCHAR(100),
        card_name VARCHAR(255),
        brand VARCHAR(255),
        model VARCHAR(255),
        component_type_id INT,
        price DECIMAL(18, 2),
        supplier_id INT,
        image VARCHAR(255)
    );
    
    INSERT INTO @inserted
    SELECT * FROM inserted;

    -- Tạo Card ID cho mỗi dòng được chèn mới
    WHILE EXISTS (SELECT 1 FROM @inserted)
    BEGIN
        DECLARE @brand VARCHAR(255);
        DECLARE @model VARCHAR(255);

        SELECT TOP 1 @brand = brand, @model = model FROM @inserted;

        -- Kiểm tra xem Expansion Card đã tồn tại trong bảng hay không
        IF EXISTS (SELECT 1 FROM Expansion_Cards WHERE brand = @brand AND model = @model)
        BEGIN
            -- Sử dụng Card ID đã có nếu Card đã tồn tại
            SELECT @new_card_id = card_id FROM Expansion_Cards WHERE brand = @brand AND model = @model;
        END
        ELSE
        BEGIN
            -- Tạo Card ID mới nếu Card chưa tồn tại
            SET @new_card_id = CONCAT(@brand, '_EC', @count + 1, '_', @model);
            SET @count = @count + 1;
        END

        -- Chèn dữ liệu vào bảng Expansion_Cards với Card ID mới
        INSERT INTO Expansion_Cards (card_id, card_name, brand, model, component_type_id, price, supplier_id, image)
        SELECT @new_card_id, card_name, brand, model, component_type_id, price, supplier_id, image
        FROM @inserted
        WHERE brand = @brand AND model = @model;

        -- Loại bỏ dòng đã được xử lý ra khỏi bảng tạm
        DELETE FROM @inserted WHERE brand = @brand AND model = @model;
    END
END;
GO

-- Tạo trigger ID tự động cho Power_Backup
CREATE OR ALTER TRIGGER Generate_Power_Backup_ID
ON Power_Backup
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @count INT;

    SELECT @count = COUNT(*) FROM Power_Backup;

    IF @count IS NULL
    BEGIN
        SET @count = 0;
    END

    DECLARE @new_backup_id VARCHAR(100);

    -- Lặp qua các dòng được chèn mới
    DECLARE @inserted TABLE (
        backup_id VARCHAR(100),
        backup_name VARCHAR(255),
        brand VARCHAR(255),
        model VARCHAR(255),
        component_type_id INT,
        price DECIMAL(18, 2),
        supplier_id INT,
        image VARCHAR(255)
    );
    
    INSERT INTO @inserted
    SELECT * FROM inserted;

    -- Tạo Backup ID cho mỗi dòng được chèn mới
    WHILE EXISTS (SELECT 1 FROM @inserted)
    BEGIN
        DECLARE @brand VARCHAR(255);
        DECLARE @model VARCHAR(255);

        SELECT TOP 1 @brand = brand, @model = model FROM @inserted;

        -- Kiểm tra xem Power Backup đã tồn tại trong bảng hay không
        IF EXISTS (SELECT 1 FROM Power_Backup WHERE brand = @brand AND model = @model)
        BEGIN
            -- Sử dụng Backup ID đã có nếu Backup đã tồn tại
            SELECT @new_backup_id = backup_id FROM Power_Backup WHERE brand = @brand AND model = @model;
        END
        ELSE
        BEGIN
            -- Tạo Backup ID mới nếu Backup chưa tồn tại
            SET @new_backup_id = CONCAT(@brand, '_PB', @count + 1, '_', @model);
            SET @count = @count + 1;
        END

        -- Chèn dữ liệu vào bảng Power_Backup với Backup ID mới
        INSERT INTO Power_Backup (backup_id, backup_name, brand, model, component_type_id, price, supplier_id, image)
        SELECT @new_backup_id, backup_name, brand, model, component_type_id, price, supplier_id, image
        FROM @inserted
        WHERE brand = @brand AND model = @model;

        -- Loại bỏ dòng đã được xử lý ra khỏi bảng tạm
        DELETE FROM @inserted WHERE brand = @brand AND model = @model;
    END
END;
GO

-----------------Các Trigger tự động cập nhập bảng product
--CPU
CREATE TRIGGER Insert_Products_ID_CPU
ON CPU
AFTER INSERT
AS
BEGIN
    INSERT INTO Products (cpu_id)
    SELECT cpu_id
    FROM inserted;
END;
GO

--RAM
CREATE TRIGGER Insert_Products_ID_RAM
ON RAM
AFTER INSERT
AS
BEGIN
    INSERT INTO Products (ram_id)
    SELECT ram_id
    FROM inserted;
END;
GO

--HDD
CREATE TRIGGER Insert_Products_ID_HDD
ON HDD
AFTER INSERT
AS
BEGIN
    INSERT INTO Products (hdd_id)
    SELECT hdd_id
    FROM inserted;
END;
GO

--SSD
CREATE TRIGGER Insert_Products_ID_SSD
ON SSD
AFTER INSERT
AS
BEGIN
    INSERT INTO Products (ssd_id)
    SELECT ssd_id
    FROM inserted;
END;
GO

--GPU
CREATE TRIGGER Insert_Products_ID_GPU
ON GPU
AFTER INSERT
AS
BEGIN
    INSERT INTO Products (gpu_id)
    SELECT gpu_id
    FROM inserted;
END;
GO

--MainBoard
CREATE TRIGGER Insert_Products_ID_Mainboard
ON Motherboard
AFTER INSERT
AS
BEGIN
    INSERT INTO Products (motherboard_id)
    SELECT motherboard_id
    FROM inserted;
END;
GO

--Cooling
CREATE TRIGGER Insert_Products_ID_Cooling
ON Cooling
AFTER INSERT
AS
BEGIN
    INSERT INTO Products (cooling_id)
    SELECT cooling_id
    FROM inserted;
END;
GO

--Power Supply Unit (PSU)
CREATE TRIGGER Insert_Products_ID_PSU
ON PSU
AFTER INSERT
AS
BEGIN
    INSERT INTO Products (psu_id)
    SELECT psu_id
    FROM inserted;
END;
GO

--Monitor
CREATE TRIGGER Insert_Products_ID_Monitor
ON Monitor
AFTER INSERT
AS
BEGIN
    INSERT INTO Products (monitor_id)
    SELECT monitor_id
    FROM inserted;
END;
GO

--Computer Case
CREATE TRIGGER Insert_Products_ID_ComputerCase
ON Computer_Case
AFTER INSERT
AS
BEGIN
    INSERT INTO Products (case_id)
    SELECT case_id
    FROM inserted;
END;
GO

--Accessories
CREATE TRIGGER Insert_Products_ID_Accessories
ON Accessories
AFTER INSERT
AS
BEGIN
    INSERT INTO Products (accessory_id)
    SELECT accessory_id
    FROM inserted;
END;
GO

--Network Adapter
CREATE TRIGGER Insert_Products_ID_NetworkAdapter
ON Network_Adapter
AFTER INSERT
AS
BEGIN
    INSERT INTO Products (adapter_id)
    SELECT adapter_id
    FROM inserted;
END;
GO

--Expansion Cards
CREATE TRIGGER Insert_Products_ID_ExpansionCards
ON Expansion_Cards
AFTER INSERT
AS
BEGIN
    INSERT INTO Products (card_id)
    SELECT card_id
    FROM inserted;
END;
GO

--Power Backup
CREATE TRIGGER Insert_Products_ID_PowerBackup
ON Power_Backup
AFTER INSERT
AS
BEGIN
    INSERT INTO Products (backup_id)
    SELECT backup_id
    FROM inserted;
END;
GO

-- Cập nhập số lượng cho Product vào việc build Computer
CREATE OR ALTER TRIGGER UpdateProductQuantityForComputer
ON Computer
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Cập nhật số lượng cho sản phẩm được thêm vào hoặc cập nhật
    UPDATE Products
    SET quantity = quantity - 1
    FROM Products p 
	LEFT JOIN CPU C ON P.cpu_id = C.cpu_id
	LEFT JOIN RAM R ON P.ram_id = R.ram_id
	LEFT JOIN HDD H ON P.hdd_id = H.hdd_id
	LEFT JOIN SSD S ON P.ssd_id = S.ssd_id
	LEFT JOIN GPU G ON P.gpu_id = G.gpu_id
	LEFT JOIN Motherboard M ON P.motherboard_id = M.motherboard_id
	LEFT JOIN Computer_Case CC ON P.case_id = CC.case_id
	LEFT JOIN Network_Adapter A ON P.adapter_id = A.adapter_id
	LEFT JOIN Expansion_Cards EC ON P.card_id = EC.card_id
	LEFT JOIN Power_Backup PB ON P.backup_id = PB.backup_id
	LEFT JOIN Cooling CO ON P.cooling_id = CO.cooling_id
	LEFT JOIN PSU PSU ON P.psu_id = PSU.psu_id
    INNER JOIN inserted i ON
        i.cpu_id = C.cpu_id OR
        i.ram_id = R.ram_id OR
        i.hdd_id = H.hdd_id OR
        i.ssd_id = S.ssd_id OR
        i.gpu_id = G.gpu_id OR
        i.motherboard_id = M.motherboard_id OR
        i.case_id = CC.case_id OR
        i.adapter_id = A.adapter_id OR
        i.card_id = EC.card_id OR
        i.backup_id = PB.backup_id OR
        i.cooling_id = CO.cooling_id OR
        i.psu_id = PSU.psu_id;

    -- Cập nhật số lượng cho sản phẩm bị cập nhật
    UPDATE Products
    SET quantity = quantity + 1
    FROM Products p 
	LEFT JOIN CPU C ON P.cpu_id = C.cpu_id
	LEFT JOIN RAM R ON P.ram_id = R.ram_id
	LEFT JOIN HDD H ON P.hdd_id = H.hdd_id
	LEFT JOIN SSD S ON P.ssd_id = S.ssd_id
	LEFT JOIN GPU G ON P.gpu_id = G.gpu_id
	LEFT JOIN Motherboard M ON P.motherboard_id = M.motherboard_id
	LEFT JOIN Computer_Case CC ON P.case_id = CC.case_id
	LEFT JOIN Network_Adapter A ON P.adapter_id = A.adapter_id
	LEFT JOIN Expansion_Cards EC ON P.card_id = EC.card_id
	LEFT JOIN Power_Backup PB ON P.backup_id = PB.backup_id
	LEFT JOIN Cooling CO ON P.cooling_id = CO.cooling_id
	LEFT JOIN PSU PSU ON P.psu_id = PSU.psu_id
    INNER JOIN inserted i ON
        i.cpu_id = C.cpu_id OR
        i.ram_id = R.ram_id OR
        i.hdd_id = H.hdd_id OR
        i.ssd_id = S.ssd_id OR
        i.gpu_id = G.gpu_id OR
        i.motherboard_id = M.motherboard_id OR
        i.case_id = CC.case_id OR
        i.adapter_id = A.adapter_id OR
        i.card_id = EC.card_id OR
        i.backup_id = PB.backup_id OR
        i.cooling_id = CO.cooling_id OR
        i.psu_id = PSU.psu_id;
END;
GO

--Cập nhập tổng giá tiền sản phẩm
CREATE OR ALTER TRIGGER UpdateTotalPrice
ON Order_Details
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    -- Khai báo biến để lưu trữ order_id
    DECLARE @AffectedOrderIDs TABLE (order_id INT);

    -- Thêm các order_id từ bảng inserted vào bảng tạm thời
    INSERT INTO @AffectedOrderIDs (order_id)
    SELECT DISTINCT order_id FROM inserted;

    -- Thêm các order_id từ bảng deleted vào bảng tạm thời
    INSERT INTO @AffectedOrderIDs (order_id)
    SELECT DISTINCT order_id FROM deleted;

    -- Cập nhật total_price cho mỗi order_id đã thay đổi
    UPDATE Orders
    SET total_price = (
        SELECT SUM(unit_price * quantity * (1 - discount))
        FROM Order_Details
        WHERE Order_Details.order_id = Orders.order_id
    )
    FROM Orders
    WHERE Orders.order_id IN (SELECT order_id FROM @AffectedOrderIDs);
END;
GO

--Tự động tạo table invoices
CREATE OR ALTER TRIGGER trgAfterInsertOrder
ON Orders
AFTER INSERT, UPDATE
AS
BEGIN
    INSERT INTO Invoices (order_id, customer_id, invoice_date, total_price, delivery_address, recipient_name)
    SELECT 
        inserted.order_id,
        inserted.customer_id,
        GETDATE(), -- Ngày hiện tại
        inserted.total_price,
        inserted.delivery_address,
        c.full_name -- Lấy full_name từ bảng Customers
    FROM
        inserted
    INNER JOIN
        Customers c ON inserted.customer_id = c.customer_id
    WHERE
        inserted.total_price IS NOT NULL; -- Chỉ tạo hóa đơn khi total_price được nhập
END;
GO

--Tự động cập nhập lịch sữ thay đổi giá sản phẩm
CREATE OR ALTER TRIGGER trg_CPU_PriceHistory
ON CPU
AFTER UPDATE
AS
BEGIN
    IF UPDATE(price)
    BEGIN
        DECLARE @CPU_ID VARCHAR(100), @OldPrice DECIMAL(18, 2), @NewPrice DECIMAL(18, 2);
        
        SELECT @CPU_ID = cpu_id, @OldPrice = price FROM deleted;
        SELECT @NewPrice = price FROM inserted;
        
        INSERT INTO Price_History (cpu_id, initial_price, price, changed_by, change_date)
        VALUES (@CPU_ID, @OldPrice, @NewPrice, SUSER_SNAME(), GETDATE());
    END
END;

GO

--Tạo customer mới khi có user mới được tạo
CREATE OR ALTER TRIGGER trgCreateCustomerForNewUser
ON Users
AFTER INSERT
AS
BEGIN
    -- Kiểm tra xem có bất kỳ bản ghi nào được chèn vào bảng Users không
    IF EXISTS (SELECT 1 FROM inserted WHERE role = 'Customer')
    BEGIN
        -- Chèn dữ liệu từ bảng Users vào bảng Customers
        INSERT INTO Customers (user_id, full_name, date_of_birth, address, email, phone_number, gender)
        SELECT 
            inserted.user_id, 
            NULL AS full_name, -- Thêm các giá trị mặc định cho các cột
            NULL AS date_of_birth, 
            NULL AS address, 
            NULL AS email, 
            NULL AS phone_number, 
            NULL AS gender
        FROM inserted
        WHERE role = 'Customer';
    END
END;
GO

-- Tạo nhân viên mới khi có người dùng mới được tạo
CREATE OR ALTER TRIGGER trgCreateEmployeeForNewUser
ON Users
AFTER INSERT
AS
BEGIN
    -- Kiểm tra xem có bất kỳ bản ghi nào được chèn vào bảng Users không
    IF EXISTS (SELECT 1 FROM inserted WHERE role IN ('Employee', 'Admin', 'Manager'))
    BEGIN
        -- Chèn dữ liệu từ bảng Users vào bảng Employees
        INSERT INTO Employees (user_id, full_name, date_of_birth, gender, email, phone_number, address, hire_date, salary, contract_type, profile_image)
        SELECT 
            inserted.user_id, 
            NULL AS full_name, -- Thêm các giá trị mặc định cho các cột
            NULL AS date_of_birth, 
            NULL AS gender,
            NULL AS email,
            NULL AS phone_number,
            NULL AS address,
            NULL AS hire_date,
            NULL AS salary,
            NULL AS contract_type,
            NULL AS profile_image
        FROM inserted
        WHERE role IN ('Employee', 'Admin', 'Manager');
    END
END;
GO