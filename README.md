# Car Dealership Database Setup

## Instructions

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

-- Drop database if it exists (optional - remove if you want to keep existing data)
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

-- Users table (both students and admins)
CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) UNIQUE NOT NULL,
    Password NVARCHAR(100) NOT NULL,
    FullName NVARCHAR(100) NULL,
    Email NVARCHAR(100) NULL,
    ProfileImage VARBINARY(MAX) NULL,
    Role NVARCHAR(20) NOT NULL DEFAULT 'Student',
    Department NVARCHAR(50) NULL,
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
INSERT INTO Users (Username, Password, FullName, Role, Department) VALUES
('admin_library', 'admin123', 'Library Administrator', 'Admin', 'Library'),
('admin_sao', 'admin123', 'SAO Administrator', 'Admin', 'SAO'),
('admin_cashier', 'admin123', 'Cashier Administrator', 'Admin', 'Cashier'),
('admin_accounting', 'admin123', 'Accounting Administrator', 'Admin', 'Accounting'),
('admin_dean', 'admin123', 'Dean''s Office Administrator', 'Admin', 'Dean''s Office'),
('admin_records', 'admin123', 'Records Administrator', 'Admin', 'Records');
GO

-- Insert test student account (Password: student123)
INSERT INTO Users (Username, Password, FullName, Role) VALUES
('student1', 'student123', 'John Doe', 'Student');
GO

-- =============================================
-- CREATE STORED PROCEDURES
-- =============================================

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
PRINT '  STUDENT: username = student1, password = student123';
PRINT '  ADMIN (Library): username = admin_library, password = admin123';
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
