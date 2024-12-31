--View thông tin tất cả sản phẩm
CREATE OR ALTER VIEW ProductInfo AS
SELECT 
	P.product_id AS STT,
    COALESCE(C.cpu_id, R.ram_id, H.hdd_id, S.ssd_id, G.gpu_id, M.motherboard_id, 
    CC.case_id, A.adapter_id, EC.card_id, PB.backup_id, CO.cooling_id, PSU.psu_id, MON.monitor_id, AC.accessory_id) AS product_id,

    COALESCE(C.cpu_name, R.ram_name, H.hdd_name, S.ssd_name, G.gpu_name, M.motherboard_name, 
    CC.case_name, A.adapter_name, EC.card_name, PB.backup_name, CO.cooling_name, 
    PSU.psu_name, MON.monitor_name, AC.accessory_name) AS product_name,

	COALESCE(C.price, R.price, H.price, S.price, G.price, M.price, CC.price, A.price, EC.price, PB.price, 
	CO.price, PSU.price, MON.price, AC.price) AS price,


    CT.component_type_name AS component_type,

    P.quantity

FROM Products P
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
LEFT JOIN Monitor MON ON P.monitor_id = MON.monitor_id
LEFT JOIN Accessories AC ON P.accessory_id = AC.accessory_id
LEFT JOIN Component_Types CT ON 
    CT.component_type_id = COALESCE(C.component_type_id, R.component_type_id, H.component_type_id, S.component_type_id, 
    G.component_type_id, M.component_type_id, CC.component_type_id, A.component_type_id, EC.component_type_id, 
    PB.component_type_id, CO.component_type_id, PSU.component_type_id, MON.component_type_id, AC.component_type_id)
GO

--Hiện ra tất cả sản phẩm
SELECT * FROM ProductInfo

CREATE OR ALTER VIEW ComputerInfo AS
SELECT 
    C.computer_id,
    CPU.cpu_name AS "CPU",
    RAM.ram_name AS "RAM",
    HDD.hdd_name AS "HDD",
    SSD.ssd_name AS "SSD",
    GPU.gpu_name AS "GPU",
    Motherboard.motherboard_name AS "Motherboard",
    Computer_Case.case_name AS "Case",
    Network_Adapter.adapter_name AS "Adapter",
    Expansion_Cards.card_name AS "Card",
    Power_Backup.backup_name AS "Backup",
    Cooling.cooling_name AS "Cooling",
    PSU.psu_name AS "PSU"
FROM Computer C
LEFT JOIN CPU ON C.cpu_id = CPU.cpu_id
LEFT JOIN RAM ON C.ram_id = RAM.ram_id
LEFT JOIN HDD ON C.hdd_id = HDD.hdd_id
LEFT JOIN SSD ON C.ssd_id = SSD.ssd_id
LEFT JOIN GPU ON C.gpu_id = GPU.gpu_id
LEFT JOIN Motherboard ON C.motherboard_id = Motherboard.motherboard_id
LEFT JOIN Computer_Case ON C.case_id = Computer_Case.case_id
LEFT JOIN Network_Adapter ON C.adapter_id = Network_Adapter.adapter_id
LEFT JOIN Expansion_Cards ON C.card_id = Expansion_Cards.card_id
LEFT JOIN Power_Backup ON C.backup_id = Power_Backup.backup_id
LEFT JOIN Cooling ON C.cooling_id = Cooling.cooling_id
LEFT JOIN PSU ON C.psu_id = PSU.psu_id;
GO

--CPU
--RAM 
--HDD
--SSD
--GPU
--Mainboard
--Cooling
--Power Supply Unit (PSU)
--Monitor
--Computer Case
--Accessories
--Network Adapter
--Expansion Cards
--Power Backup

select * from Price_History

SELECT * FROM CPU

