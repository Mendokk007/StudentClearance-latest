# Student Clearance Database Setup

## 🚀 How to Install (for Users)

### ⚙️ Database Setup (Required First)
`Before running the application, you must set up the SQL database:`
1.  Ensure you have **SQL Server Express** or **LocalDB** installed.
2.  **Download the Setup Script:**
    <br>
    <a href="https://github.com/Mendokk007/StudentClearance-latest/releases/download/v1.0/database_setup.sql" download>
      <img src="https://img.shields.io/badge/Download-SQL_Script-blue?style=for-the-badge&logo=microsoftsqlserver" alt="Download SQL Script">
    </a>
3.  Open **SQL Server Management Studio (SSMS)** and connect to your local server.
4.  Go to **File > Open > File...** and select the downloaded `database_setup.sql`.
5.  Press **Execute (F5)** to create the tables and initial data.


### 💻 Application Installation
1.  Go to the [Releases](https://github.com/Mendokk007/StudentClearance-latest/releases) page.
2.  Download `StudentClearance_v1.0_Installer.rar`.
3.  **Extract** the RAR file to a folder on your computer.
4.  Run `setup.exe` to install the application.
5.  Once installed, you can open the app via the **StudentClearance** shortcut on your desktop.

> [!IMPORTANT]
> If Windows shows a "Protected your PC" warning during installation, click **More Info** then **Run Anyway**.
<a href="PASTE_YOUR_VIRUSTOTAL_URL_HERE" target="_blank">
  <img src="https://img.shields.io/badge/VirusTotal-Verified_Clean-0070ad?style=for-the-badge&logo=virustotal&logoColor=white" alt="VirusTotal Scan Result">
</a>
<br>

`Scan performed on the installer package to ensure it is free from malware and safe for use.`

---

## 🚀🚀Instructions (for Devs)

1. Open **SQL Server Management Studio (SSMS)**
2. Connect to your SQL Server instance
3. Copy the query below
4. Paste it into a new query window
5. Press **F5** or click **Execute** to run

## Database Query

```sql
-- =============================================
-- STUDENT CLEARANCE SYSTEM - DATABASE SETUP
-- =============================================
-- Run this entire script in SSMS to create the database
-- =============================================

USE master;
GO

-- Drop database if it exists
IF EXISTS (SELECT name FROM sys.databases WHERE name = 'StudentClearanceDB')
BEGIN
    ALTER DATABASE StudentClearanceDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE StudentClearanceDB;
END
GO

-- =============================================
-- CREATE DATABASE
-- =============================================
CREATE DATABASE StudentClearanceDB;
GO

USE StudentClearanceDB;
GO

-- =============================================
-- CREATE TABLES
-- =============================================

-- Student ID counter table (tracks next available ID number)
CREATE TABLE StudentIDCounter (
    LastNumber INT NOT NULL DEFAULT 0
);
GO

-- Users table (both students and admins)
CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) UNIQUE NOT NULL,
    Password NVARCHAR(100) NOT NULL,
    FullName NVARCHAR(100) NULL,
    Email NVARCHAR(100) NULL,
    Program NVARCHAR(50) NULL,        -- Student program (e.g., BSIT, BSCS)
    ProfileImage VARBINARY(MAX) NULL,
    Role NVARCHAR(20) NOT NULL DEFAULT 'Student',
    Department NVARCHAR(50) NULL,     -- For admins only (e.g., Library, SAO)
    CreatedAt DATETIME DEFAULT GETDATE()
);
GO

-- Departments table
CREATE TABLE Departments (
    DepartmentID INT IDENTITY(1,1) PRIMARY KEY,
    DepartmentName NVARCHAR(50) UNIQUE NOT NULL,
    Description NVARCHAR(200) NULL,
    DisplayOrder INT DEFAULT 0
);
GO

-- Clearance Submissions table
CREATE TABLE ClearanceSubmissions (
    SubmissionID INT IDENTITY(1,1) PRIMARY KEY,
    StudentUsername NVARCHAR(50) NOT NULL,
    DepartmentName NVARCHAR(50) NOT NULL,
    ImageData VARBINARY(MAX) NULL,
    ImageFileName NVARCHAR(255) NULL,
    Status NVARCHAR(20) DEFAULT 'Pending',
    RejectionReason NVARCHAR(500) NULL,
    SubmittedAt DATETIME DEFAULT GETDATE(),
    ReviewedAt DATETIME NULL,
    ReviewedBy NVARCHAR(50) NULL,
    FOREIGN KEY (StudentUsername) REFERENCES Users(Username) ON DELETE CASCADE,
    FOREIGN KEY (DepartmentName) REFERENCES Departments(DepartmentName)
);
GO

-- Notifications table
CREATE TABLE Notifications (
    NotificationID INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL,
    Message NVARCHAR(500) NOT NULL,
    Type NVARCHAR(50) NULL,
    IsRead BIT DEFAULT 0,
    CreatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (Username) REFERENCES Users(Username) ON DELETE CASCADE
);
GO

-- =============================================
-- INSERT INITIAL DATA
-- =============================================

-- Initialize Student ID counter (starts at 0, first student gets STUD001)
INSERT INTO StudentIDCounter (LastNumber) VALUES (0);
GO

-- Insert departments
INSERT INTO Departments (DepartmentName, Description, DisplayOrder) VALUES
('Library', 'Library clearance for borrowed books', 1),
('SAO', 'Student Affairs Office clearance', 2),
('Cashier', 'Financial clearance for tuition and fees', 3),
('Accounting', 'Accounting department clearance', 4),
('Dean''s Office', 'Dean''s Office clearance', 5),
('Records', 'Records and Registrar clearance', 6);
GO

-- Insert admin accounts (Password: admin123)
-- Admins keep their custom usernames, not auto-generated IDs
INSERT INTO Users (Username, Password, FullName, Role, Department, Program) VALUES
('admin_library', 'admin123', 'Library Administrator', 'Admin', 'Library', NULL),
('admin_sao', 'admin123', 'SAO Administrator', 'Admin', 'SAO', NULL),
('admin_cashier', 'admin123', 'Cashier Administrator', 'Admin', 'Cashier', NULL),
('admin_accounting', 'admin123', 'Accounting Administrator', 'Admin', 'Accounting', NULL),
('admin_dean', 'admin123', 'Dean''s Office Administrator', 'Admin', 'Dean''s Office', NULL),
('admin_records', 'admin123', 'Records Administrator', 'Admin', 'Records', NULL);
GO

-- Insert test student account (Password: student123)
-- This simulates a student who registered and got STUD001
INSERT INTO Users (Username, Password, FullName, Role, Program) VALUES
('STUD001', 'student123', 'John Doe', 'Student', 'BSIT');
GO

-- Update counter to 1 so next student gets STUD002
UPDATE StudentIDCounter SET LastNumber = 1;
GO

-- =============================================
-- CREATE STORED PROCEDURES
-- =============================================

-- Generate next Student ID (STUD001, STUD002, etc.)
CREATE PROCEDURE sp_GenerateStudentID
    @NewStudentID NVARCHAR(50) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @NextNumber INT;
    
    -- Atomically increment and get the next number
    UPDATE StudentIDCounter
    SET @NextNumber = LastNumber + 1,
        LastNumber = LastNumber + 1;
    
    -- Format as STUD001, STUD002, ..., STUD999, STUD1000, etc.
    IF @NextNumber < 10
        SET @NewStudentID = 'STUD00' + CAST(@NextNumber AS NVARCHAR);
    ELSE IF @NextNumber < 100
        SET @NewStudentID = 'STUD0' + CAST(@NextNumber AS NVARCHAR);
    ELSE IF @NextNumber < 1000
        SET @NewStudentID = 'STUD' + CAST(@NextNumber AS NVARCHAR);
    ELSE
        SET @NewStudentID = 'STUD' + CAST(@NextNumber AS NVARCHAR); -- 4+ digits
END
GO

-- Get student clearance status
CREATE PROCEDURE sp_GetStudentClearanceStatus
    @Username NVARCHAR(50)
AS
BEGIN
    SELECT 
        d.DepartmentName,
        ISNULL(cs.Status, 'Pending') AS Status,
        cs.SubmittedAt,
        cs.ReviewedAt,
        cs.RejectionReason
    FROM Departments d
    LEFT JOIN ClearanceSubmissions cs 
        ON d.DepartmentName = cs.DepartmentName 
        AND cs.StudentUsername = @Username
    ORDER BY d.DisplayOrder;
END
GO

-- Submit clearance
CREATE PROCEDURE sp_SubmitClearance
    @StudentUsername NVARCHAR(50),
    @DepartmentName NVARCHAR(50),
    @ImageData VARBINARY(MAX),
    @ImageFileName NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    
    IF EXISTS (SELECT 1 FROM ClearanceSubmissions 
               WHERE StudentUsername = @StudentUsername 
               AND DepartmentName = @DepartmentName)
    BEGIN
        UPDATE ClearanceSubmissions
        SET ImageData = @ImageData,
            ImageFileName = @ImageFileName,
            Status = 'Pending',
            SubmittedAt = GETDATE(),
            ReviewedAt = NULL,
            ReviewedBy = NULL,
            RejectionReason = NULL
        WHERE StudentUsername = @StudentUsername 
          AND DepartmentName = @DepartmentName;
    END
    ELSE
    BEGIN
        INSERT INTO ClearanceSubmissions (StudentUsername, DepartmentName, ImageData, ImageFileName, Status, SubmittedAt)
        VALUES (@StudentUsername, @DepartmentName, @ImageData, @ImageFileName, 'Pending', GETDATE());
    END

    DECLARE @AdminUsername NVARCHAR(50);
    SELECT @AdminUsername = Username FROM Users 
    WHERE Role = 'Admin' AND Department = @DepartmentName;
    
    IF @AdminUsername IS NOT NULL
    BEGIN
        INSERT INTO Notifications (Username, Message, Type, IsRead, CreatedAt)
        VALUES (@AdminUsername, 
                'New clearance submission from ' + @StudentUsername + ' for ' + @DepartmentName,
                'NewSubmission', 0, GETDATE());
    END
END
GO

-- Review clearance (approve/reject)
CREATE PROCEDURE sp_ReviewClearance
    @SubmissionID INT,
    @Status NVARCHAR(20),
    @RejectionReason NVARCHAR(500) = NULL,
    @ReviewedBy NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @StudentUsername NVARCHAR(50), @DepartmentName NVARCHAR(50);
    
    SELECT @StudentUsername = StudentUsername, 
           @DepartmentName = DepartmentName
    FROM ClearanceSubmissions 
    WHERE SubmissionID = @SubmissionID;
    
    UPDATE ClearanceSubmissions
    SET Status = @Status,
        RejectionReason = @RejectionReason,
        ReviewedAt = GETDATE(),
        ReviewedBy = @ReviewedBy
    WHERE SubmissionID = @SubmissionID;
    
    DECLARE @Message NVARCHAR(500);
    IF @Status = 'Approved'
        SET @Message = 'Your ' + @DepartmentName + ' clearance has been APPROVED!';
    ELSE
        SET @Message = 'Your ' + @DepartmentName + ' clearance was REJECTED. Reason: ' + ISNULL(@RejectionReason, 'No reason provided');
    
    INSERT INTO Notifications (Username, Message, Type, IsRead, CreatedAt)
    VALUES (@StudentUsername, @Message, 'ClearanceUpdate', 0, GETDATE());
    
    SELECT @StudentUsername AS StudentUsername, @DepartmentName AS DepartmentName, @Status AS Status;
END
GO

-- Get pending submissions for a department admin
CREATE PROCEDURE sp_GetPendingSubmissions
    @DepartmentName NVARCHAR(50)
AS
BEGIN
    SELECT 
        cs.SubmissionID,
        cs.StudentUsername,
        u.FullName AS StudentName,
        u.Program AS StudentProgram,
        cs.ImageData,
        cs.ImageFileName,
        cs.SubmittedAt,
        cs.Status
    FROM ClearanceSubmissions cs
    INNER JOIN Users u ON cs.StudentUsername = u.Username
    WHERE cs.DepartmentName = @DepartmentName
      AND cs.Status = 'Pending'
    ORDER BY cs.SubmittedAt DESC;
END
GO

-- Get unread notifications count
CREATE PROCEDURE sp_GetUnreadNotificationCount
    @Username NVARCHAR(50)
AS
BEGIN
    SELECT COUNT(*) AS UnreadCount
    FROM Notifications
    WHERE Username = @Username AND IsRead = 0;
END
GO

PRINT '========================================';
PRINT 'DATABASE SETUP COMPLETE!';
PRINT '========================================';
PRINT 'Test Accounts:';
PRINT '  STUDENT: Student ID = STUD001, password = student123, Program = BSIT';
PRINT '  ADMIN (Library): username = admin_library, password = admin123';
PRINT '';
PRINT 'Auto-Generated Student IDs:';
PRINT '  STUD001, STUD002, STUD003, ...';
PRINT '========================================';
GO


```


---

## 🚀 Getting Started

### Prerequisites

- **Visual Studio 2019+** (Community Edition works)
- **SQL Server 2019+** (Express Edition works)
- **.NET Framework 4.7.2 SDK**
- **Git** (optional, for cloning)

### Installation Steps

1. **Clone the repository**
   ```bash
   git clone https://github.com/Mendokk007/StudentClearance-latest.git
    ```

2. **Install NuGet Packages**

    - Microsoft.AspNet.SignalR
    - Microsoft.AspNet.SignalR.Client
    - Microsoft.Owin
    - Microsoft.Owin.Host.SystemWeb
    - Newtonsoft.Json (v13.0.3+)

3. **Update connection string**
    ```bash
    _connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=StudentClearanceDB;Integrated Security=True;";
     ```

---

### Test Accounts
```
Student	                student1	        student123
Library Admin	        admin_library	    admin123
SAO Admin	            admin_sao	        admin123
Cashier Admin	        admin_cashier	    admin123
Accounting Admin	    admin_accounting	admin123
Dean's Office Admin    	admin_dean	        admin123
Records Admin	        admin_records	    admin123
```
