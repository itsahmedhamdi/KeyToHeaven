-- Ensure the database exists and use it
CREATE DATABASE IF NOT EXISTS keytoheaven;
USE keytoheaven;

-- Donor Table (Updated Structure)
CREATE TABLE IF NOT EXISTS Donor (
    Donor_Id INT AUTO_INCREMENT PRIMARY KEY,
    FirstName VARCHAR(100) NOT NULL,
    LastName VARCHAR(100) NOT NULL,
    Email VARCHAR(255) UNIQUE NOT NULL,
    Password VARCHAR(255) NOT NULL,
    Phone VARCHAR(15) NOT NULL
);

-- Association Table (Updated Structure)
CREATE TABLE IF NOT EXISTS Association (
    Association_Id INT AUTO_INCREMENT PRIMARY KEY,
    FirstName VARCHAR(100) NOT NULL,
    LastName VARCHAR(100) NOT NULL,
    Email VARCHAR(255) UNIQUE NOT NULL,
    Password VARCHAR(255) NOT NULL,
    Phone VARCHAR(15) NOT NULL,
    IDMinistry VARCHAR(100) NOT NULL
);
ALTER TABLE Association ADD COLUMN Description TEXT NOT NULL;

-- Donation_Transaction Table (Updated Structure)
CREATE TABLE IF NOT EXISTS Donation_Transaction (
    Transaction_Id INT AUTO_INCREMENT PRIMARY KEY,
    Donor_Id INT,
    Association_Id INT,
    Amount DECIMAL(10,2) NOT NULL,
    Payment_Method ENUM('credit_card', 'D17') NOT NULL,
    Transaction_Status ENUM('pending', 'completed', 'failed') DEFAULT 'pending',
    Transaction_Date DATETIME DEFAULT NOW(),
    FOREIGN KEY (Donor_Id) REFERENCES Donor(Donor_Id) ON DELETE CASCADE,
    FOREIGN KEY (Association_Id) REFERENCES Association(Association_Id) ON DELETE CASCADE
);

-- Beneficiary Table (Updated Structure)
CREATE TABLE IF NOT EXISTS Beneficiary (
    Beneficiary_Id INT AUTO_INCREMENT PRIMARY KEY,  
    Donor_Id INT, 
    Association_Id INT, 
    Beneficiary_First_Name VARCHAR(100) NOT NULL, 
    Beneficiary_Last_Name VARCHAR(100) NOT NULL,  
    Beneficiary_Email VARCHAR(255) UNIQUE NOT NULL,  
    Beneficiary_Phone_Number VARCHAR(20), 
    Image TEXT, 
    Needs TEXT,  
    Description TEXT, 
    Birth_Date DATETIME,  
    Goal_Amount DECIMAL(10,2), 
    Raised_Amount DECIMAL(10,2) DEFAULT 0.00, 
    Gender ENUM('male','female'),  
    Income DECIMAL(10,5), 
    Family_Size INT,  
    Beneficiary_Address VARCHAR(255),  
    Beneficiary_Status ENUM('pending','ongoing','received','not_eligible','urgent','confirmed','rejected'),  

    Registration_Date DATETIME DEFAULT NOW(), 

    -- Foreign Key Constraints
    FOREIGN KEY (Donor_Id) REFERENCES Donor(Donor_Id) ON DELETE CASCADE,
    FOREIGN KEY (Association_Id) REFERENCES Association(Association_Id) ON DELETE CASCADE
);



-- Insert Data into Donor Table
INSERT INTO Donor (Donor_Id,FirstName, LastName, Email, Password, Phone) 
VALUES 
(1,'John', 'Doe', 'john.doe@example.com', SHA2('fazefz', 512), '1234567890'),
(2,'Jane', 'Smith', 'jane.smith@example.com', SHA2('fazefz', 512), '0987654321'),
(3,'Mike', 'Johnson', 'mike.johnson@example.com', SHA2('fazefz', 512), '1122334455'),
(4,'Alice', 'Brown', 'alice.brown@example.com', SHA2('fazefz', 512), '5566778899'),
(5,'Bob', 'Martin', 'bob.martin@example.com', SHA2('fazefz', 512), '6677889900');

-- Insert Data into Association Table
INSERT INTO Association (Association_Id,FirstName, LastName, Email, Password, Phone, IDMinistry, Description) 
VALUES 
(1,'Jeunesactifs', 'Sactifs', 'jeunesactifs@example.com', SHA2('azerty', 512), '1231231234', 'MIN12345', 'Providing essential aid and support to underprivileged communities.'),
(2,'Darna', '', 'darna@example.com', SHA2('azerty', 512), '4564564567', 'MIN67890', 'Dedicated to offering food, shelter, and education to those in need.'),
(3,'Shanti', '', 'shanti@example.com', SHA2('azerty', 512), '7897897890', 'MIN54321', 'A charity focused on empowering individuals through skill development and financial aid.'),
(4,'Amal', '', 'amal@example.com', SHA2('azerty', 512), '1112223334', 'MIN98765', 'Supporting orphaned children with education, healthcare, and emotional care.'),
(5,'Tamss', '', 'tamss@example.com', SHA2('azerty', 512), '4445556667', 'MIN54312', 'Committed to providing medical assistance and disaster relief worldwide.');

-- Insert Data into Donation_Transaction Table
INSERT INTO Donation_Transaction (Donor_Id, Association_Id, Amount, Payment_Method, Transaction_Status, Transaction_Date)
VALUES
(1, 1, 150.00, 'credit_card', 'completed', '2024-03-01 10:15:00'),
(2, 2, 250.50, 'D17', 'completed', '2024-03-05 14:30:00'),
(3, 3, 75.00, 'credit_card', 'pending', '2024-03-10 09:45:00'),
(4, 4, 500.00, 'D17', 'failed', '2024-03-12 16:20:00'),
(5, 5, 320.75, 'credit_card', 'completed', '2024-03-15 18:10:00'),
(1, 2, 200.00, 'D17', 'completed', '2024-03-18 11:00:00'),
(3, 1, 120.00, 'credit_card', 'pending', '2024-03-21 13:25:00'),
(4, 3, 450.00, 'D17', 'completed', '2024-03-25 15:40:00'),
(2, 4, 600.00, 'credit_card', 'failed', '2024-03-28 08:30:00'),
(5, 1, 275.00, 'D17', 'completed', '2024-03-30 20:00:00');

-- Insert Data into Beneficiary Table
INSERT INTO Beneficiary (Association_Id, Donor_Id, Beneficiary_First_Name, Beneficiary_Last_Name, Beneficiary_Email, Beneficiary_Phone_Number, Image, Needs, Description, Birth_Date, Goal_Amount, Raised_Amount, Gender, Income, Family_Size, Beneficiary_Address, Beneficiary_Status, Registration_Date)
VALUES
(1, 1, 'Mohamed', 'Ben Ali', 'mohamed.benali@example.com', '+21698765432', 'food.jpg', 'Food', 'Mohamed is a father of five struggling to provide daily meals for his family. He lost his job due to economic hardships and relies on donations to feed his children.', '1985-06-15', 500.00, 100.00, 'male', 250.50000, 5, 'Tunis, Tunisia', 'ongoing', NOW()),

(2, 2, 'Amina', 'Trabelsi', 'amina.trabelsi@example.com', '+21696543210', 'sante.jpg', 'Medical Assistance', 'Amina is a single mother diagnosed with a chronic illness. She needs urgent medical care, including surgery, which is beyond her financial means.', '1990-03-22', 7000.00, 3000.00, 'female', 100.75000, 3, 'Sousse, Tunisia', 'urgent', NOW()),

(3, 3, 'Houssem', 'Jebali', 'houssem.jebali@example.com', '+21695432178', 'education.jpg', 'Education Support', 'Houssem is a bright university student who excels in engineering but struggles to afford tuition and materials. He dreams of becoming an innovator.', '1998-11-05', 6000.00, 4500.00, 'male', 300.00000, 4, 'Sfax, Tunisia', 'pending', NOW()),

(4, 4, 'Fatma', 'Mansouri', 'fatma.mansouri@example.com', '+21691234567', 'building.jpg', 'Housing Assistance', 'Fatma is a widow living in poor conditions with her children. Her home is severely damaged, and she needs financial support to rebuild it.', '1982-07-09', 9000.00, 1200.00, 'female', 120.25000, 6, 'Gabes, Tunisia', 'received', NOW()),

(5, 5, 'Sami', 'Khelifa', 'sami.khelifa@example.com', '+21693456789', 'jobsupport.jpg', 'Job Support', 'Sami is a young professional who lost his job and is struggling to support his family. He needs assistance in securing stable employment.', '1995-02-18', 3000.00, 1000.00, 'male', 280.00000, 2, 'Bizerte, Tunisia', 'confirmed', NOW());

ALTER TABLE Association ADD COLUMN Image VARCHAR(255);
UPDATE Association 
SET Image = 'jeunesactifs.jpg' WHERE Association_Id = 1;
UPDATE Association 
SET Image = 'darna.jpg' WHERE Association_Id = 2;
UPDATE Association 
SET Image = 'shanti.jpg' WHERE Association_Id = 3;
UPDATE Association 
SET Image = 'amal.jpg' WHERE Association_Id = 4;
UPDATE Association 
SET Image = 'tamss.jpg' WHERE Association_Id = 5;
INSERT INTO Beneficiary (Association_Id, Donor_Id, Beneficiary_First_Name, Beneficiary_Last_Name, Beneficiary_Email, Beneficiary_Phone_Number, Image, Needs, Birth_Date, Goal_Amount, Raised_Amount, Gender, Income, Family_Size, Beneficiary_Address, Beneficiary_Status, Registration_Date)
VALUES
(1, 3, 'Rania', 'Bouazizi', 'rania.bouazizi@example.com', '+21699887766', 'clothing.jpg', 'Clothing Assistance', '1993-09-12', 1500.00, 500.00, 'female', 200.00000, 4, 'Nabeul, Tunisia', 'ongoing', NOW());


select * from Association;

