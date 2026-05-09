# Student Clearance Database Setup

## 🚀 How to Install (for Users)

### ⚙️ Database Setup (Required First)
`Before running the application, you must set up the SQL database:`
1.  Ensure you have **SQL Server Express** or **LocalDB** installed.
2.  **Download the Setup Script:**
    <br>
    <a href="https://github.com/Mendokk007/StudentClearance-latest/releases/download/2.0/database_setup.sql" download>
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

<a href="https://www.virustotal.com/gui/file/acedddd4893913b69de3f89683dd7c728d8eda8d33235de659d98703369bc38c" target="_blank">
  <img src="https://img.shields.io/badge/VirusTotal-Verified_Clean-0070ad?style=for-the-badge&logo=virustotal&logoColor=white" alt="VirusTotal Scan Result">
</a> 
<a href="https://www.virustotal.com/gui/file/572be7a08e2b55ba3e2032f1ae610b1f8610a0716d373e8a92e8f2061c97f654" target="_blank">
  <img src="https://img.shields.io/badge/VirusTotal-Verified_Clean-0070ad?style=for-the-badge&logo=virustotal&logoColor=white" alt="VirusTotal Scan Result">
</a> 
<br>

`Scan performed on the installer package and the setup.exe to ensure it is free from malware and safe for use.`

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
-- STUDENT CLEARANCE SYSTEM - COMPLETE DATABASE SETUP
-- =============================================
-- Run this entire script in SSMS to create the database
-- Includes: Department Clearance + Subject Clearance + Activity Logs
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

-- Student ID counter table
CREATE TABLE StudentIDCounter (
    LastNumber INT NOT NULL DEFAULT 0
);
GO

-- Users table
CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) UNIQUE NOT NULL,
    Password NVARCHAR(100) NOT NULL,
    FullName NVARCHAR(100) NULL,
    Email NVARCHAR(100) NULL,
    Program NVARCHAR(50) NULL,
    ProfileImage VARBINARY(MAX) NULL,
    Role NVARCHAR(20) NOT NULL DEFAULT 'Student',
    Department NVARCHAR(50) NULL,
    AssignedSubject NVARCHAR(80) NULL,
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

-- Clearance Submissions table (department)
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

-- Programs table
CREATE TABLE Programs (
    ProgramID INT IDENTITY(1,1) PRIMARY KEY,
    ProgramCode NVARCHAR(10) UNIQUE NOT NULL,
    ProgramName NVARCHAR(100) NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE()
);
GO

-- Subjects table
CREATE TABLE Subjects (
    SubjectID INT IDENTITY(1,1) PRIMARY KEY,
    ProgramCode NVARCHAR(10) NOT NULL,
    SubjectName NVARCHAR(80) NOT NULL,
    DisplayOrder INT DEFAULT 0,
    FOREIGN KEY (ProgramCode) REFERENCES Programs(ProgramCode) ON DELETE CASCADE,
    CONSTRAINT UQ_SubjectPerProgram UNIQUE (ProgramCode, SubjectName)
);
GO

-- Subject Clearance Submissions table
CREATE TABLE SubjectClearanceSubmissions (
    SubmissionID INT IDENTITY(1,1) PRIMARY KEY,
    StudentUsername NVARCHAR(50) NOT NULL,
    SubjectName NVARCHAR(80) NOT NULL,
    ImageData VARBINARY(MAX) NULL,
    ImageFileName NVARCHAR(255) NULL,
    Status NVARCHAR(20) DEFAULT 'Pending',
    RejectionReason NVARCHAR(500) NULL,
    SubmittedAt DATETIME DEFAULT GETDATE(),
    ReviewedAt DATETIME NULL,
    ReviewedBy NVARCHAR(50) NULL,
    FOREIGN KEY (StudentUsername) REFERENCES Users(Username) ON DELETE CASCADE
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
-- NEW: ACTIVITY LOGS TABLE
-- =============================================
CREATE TABLE ActivityLogs (
    LogID INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL,
    Message NVARCHAR(500) NOT NULL,
    LogType NVARCHAR(30) NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (Username) REFERENCES Users(Username) ON DELETE CASCADE
);
GO

-- =============================================
-- INSERT INITIAL DATA
-- =============================================

INSERT INTO StudentIDCounter (LastNumber) VALUES (0);
GO

INSERT INTO Departments (DepartmentName, Description, DisplayOrder) VALUES
('Library', 'Library clearance for borrowed books', 1),
('SAO', 'Student Affairs Office clearance', 2),
('Cashier', 'Financial clearance for tuition and fees', 3),
('Accounting', 'Accounting department clearance', 4),
('Dean''s Office', 'Dean''s Office clearance', 5),
('Records', 'Records and Registrar clearance', 6);
GO

INSERT INTO Programs (ProgramCode, ProgramName) VALUES
('BSIT', 'BS Information Technology'),
('BSCS', 'BS Computer Science'),
('BSBA', 'BS Business Administration'),
('BSED', 'BS Education'),
('BSHM', 'BS Hospitality Management');
GO

-- Subjects
INSERT INTO Subjects (ProgramCode, SubjectName, DisplayOrder) VALUES
('BSIT', 'Programming 1', 1),
('BSIT', 'Database Systems', 2),
('BSIT', 'Web Dev', 3),
('BSIT', 'Networking 1', 4),
('BSIT', 'OS Concepts', 5),
('BSIT', 'System Analysis', 6);

INSERT INTO Subjects (ProgramCode, SubjectName, DisplayOrder) VALUES
('BSCS', 'Data Structures', 1),
('BSCS', 'Algorithms', 2),
('BSCS', 'Discrete Math', 3),
('BSCS', 'Software Eng', 4),
('BSCS', 'AI Basics', 5),
('BSCS', 'Computer Org', 6);

INSERT INTO Subjects (ProgramCode, SubjectName, DisplayOrder) VALUES
('BSBA', 'Financial Mgmt', 1),
('BSBA', 'Marketing 101', 2),
('BSBA', 'Business Ethics', 3),
('BSBA', 'HR Mgmt', 4),
('BSBA', 'Operations Mgmt', 5),
('BSBA', 'Business Law', 6);

INSERT INTO Subjects (ProgramCode, SubjectName, DisplayOrder) VALUES
('BSED', 'Found. of Educ', 1),
('BSED', 'Ed. Psychology', 2),
('BSED', 'Teaching Methods', 3),
('BSED', 'Curriculum Dev', 4),
('BSED', 'Assessment 101', 5),
('BSED', 'Child Dev', 6);

INSERT INTO Subjects (ProgramCode, SubjectName, DisplayOrder) VALUES
('BSHM', 'Food & Beverage', 1),
('BSHM', 'Housekeeping', 2),
('BSHM', 'Front Office', 3),
('BSHM', 'Culinary Arts 1', 4),
('BSHM', 'Tourism Mgmt', 5),
('BSHM', 'Bartending', 6);
GO

-- Admin accounts
INSERT INTO Users (Username, Password, FullName, Role, Department, Program) VALUES
('admin_library', 'admin123', 'Library Administrator', 'Admin', 'Library', NULL),
('admin_sao', 'admin123', 'SAO Administrator', 'Admin', 'SAO', NULL),
('admin_cashier', 'admin123', 'Cashier Administrator', 'Admin', 'Cashier', NULL),
('admin_accounting', 'admin123', 'Accounting Administrator', 'Admin', 'Accounting', NULL),
('admin_dean', 'admin123', 'Dean''s Office Administrator', 'Admin', 'Dean''s Office', NULL),
('admin_records', 'admin123', 'Records Administrator', 'Admin', 'Records', NULL);
GO

-- Instructor accounts
INSERT INTO Users (Username, Password, FullName, Role, AssignedSubject) VALUES
('inst_prog1', 'inst123', 'Instructor - Programming 1', 'Instructor', 'Programming 1'),
('inst_db', 'inst123', 'Instructor - Database Systems', 'Instructor', 'Database Systems'),
('inst_webdev', 'inst123', 'Instructor - Web Dev', 'Instructor', 'Web Dev'),
('inst_net1', 'inst123', 'Instructor - Networking 1', 'Instructor', 'Networking 1'),
('inst_os', 'inst123', 'Instructor - OS Concepts', 'Instructor', 'OS Concepts'),
('inst_sysanal', 'inst123', 'Instructor - System Analysis', 'Instructor', 'System Analysis'),
('inst_ds', 'inst123', 'Instructor - Data Structures', 'Instructor', 'Data Structures'),
('inst_algo', 'inst123', 'Instructor - Algorithms', 'Instructor', 'Algorithms'),
('inst_dmath', 'inst123', 'Instructor - Discrete Math', 'Instructor', 'Discrete Math'),
('inst_se', 'inst123', 'Instructor - Software Eng', 'Instructor', 'Software Eng'),
('inst_ai', 'inst123', 'Instructor - AI Basics', 'Instructor', 'AI Basics'),
('inst_corg', 'inst123', 'Instructor - Computer Org', 'Instructor', 'Computer Org'),
('inst_fin', 'inst123', 'Instructor - Financial Mgmt', 'Instructor', 'Financial Mgmt'),
('inst_mkt', 'inst123', 'Instructor - Marketing 101', 'Instructor', 'Marketing 101'),
('inst_eth', 'inst123', 'Instructor - Business Ethics', 'Instructor', 'Business Ethics'),
('inst_hr', 'inst123', 'Instructor - HR Mgmt', 'Instructor', 'HR Mgmt'),
('inst_ops', 'inst123', 'Instructor - Operations Mgmt', 'Instructor', 'Operations Mgmt'),
('inst_blaw', 'inst123', 'Instructor - Business Law', 'Instructor', 'Business Law'),
('inst_fed', 'inst123', 'Instructor - Found. of Educ', 'Instructor', 'Found. of Educ'),
('inst_edpsy', 'inst123', 'Instructor - Ed. Psychology', 'Instructor', 'Ed. Psychology'),
('inst_tm', 'inst123', 'Instructor - Teaching Methods', 'Instructor', 'Teaching Methods'),
('inst_cd', 'inst123', 'Instructor - Curriculum Dev', 'Instructor', 'Curriculum Dev'),
('inst_as101', 'inst123', 'Instructor - Assessment 101', 'Instructor', 'Assessment 101'),
('inst_cdev', 'inst123', 'Instructor - Child Dev', 'Instructor', 'Child Dev'),
('inst_fb', 'inst123', 'Instructor - Food & Beverage', 'Instructor', 'Food & Beverage'),
('inst_hk', 'inst123', 'Instructor - Housekeeping', 'Instructor', 'Housekeeping'),
('inst_fo', 'inst123', 'Instructor - Front Office', 'Instructor', 'Front Office'),
('inst_ca1', 'inst123', 'Instructor - Culinary Arts 1', 'Instructor', 'Culinary Arts 1'),
('inst_tmgt', 'inst123', 'Instructor - Tourism Mgmt', 'Instructor', 'Tourism Mgmt'),
('inst_bt', 'inst123', 'Instructor - Bartending', 'Instructor', 'Bartending');
GO

-- Test student
INSERT INTO Users (Username, Password, FullName, Role, Program) VALUES
('STUD001', 'student123', 'John Doe', 'Student', 'BSIT');
GO

UPDATE StudentIDCounter SET LastNumber = 1;
GO

-- =============================================
-- STORED PROCEDURES
-- =============================================

-- Generate next Student ID
CREATE PROCEDURE sp_GenerateStudentID
    @NewStudentID NVARCHAR(50) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @NextNumber INT;
    UPDATE StudentIDCounter
    SET @NextNumber = LastNumber + 1, LastNumber = LastNumber + 1;
    IF @NextNumber < 10 SET @NewStudentID = 'STUD00' + CAST(@NextNumber AS NVARCHAR);
    ELSE IF @NextNumber < 100 SET @NewStudentID = 'STUD0' + CAST(@NextNumber AS NVARCHAR);
    ELSE SET @NewStudentID = 'STUD' + CAST(@NextNumber AS NVARCHAR);
END
GO

-- Department clearance status
CREATE PROCEDURE sp_GetStudentClearanceStatus
    @Username NVARCHAR(50)
AS
BEGIN
    SELECT d.DepartmentName, ISNULL(cs.Status, 'Pending') AS Status, cs.SubmittedAt, cs.ReviewedAt, cs.RejectionReason
    FROM Departments d
    LEFT JOIN ClearanceSubmissions cs ON d.DepartmentName = cs.DepartmentName AND cs.StudentUsername = @Username
    ORDER BY d.DisplayOrder;
END
GO

-- Submit department clearance
CREATE PROCEDURE sp_SubmitClearance
    @StudentUsername NVARCHAR(50),
    @DepartmentName NVARCHAR(50),
    @ImageData VARBINARY(MAX),
    @ImageFileName NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM ClearanceSubmissions WHERE StudentUsername = @StudentUsername AND DepartmentName = @DepartmentName)
    BEGIN
        UPDATE ClearanceSubmissions SET ImageData = @ImageData, ImageFileName = @ImageFileName, Status = 'Pending',
            SubmittedAt = GETDATE(), ReviewedAt = NULL, ReviewedBy = NULL, RejectionReason = NULL
        WHERE StudentUsername = @StudentUsername AND DepartmentName = @DepartmentName;
    END
    ELSE
    BEGIN
        INSERT INTO ClearanceSubmissions (StudentUsername, DepartmentName, ImageData, ImageFileName, Status, SubmittedAt)
        VALUES (@StudentUsername, @DepartmentName, @ImageData, @ImageFileName, 'Pending', GETDATE());
    END
    DECLARE @AdminUsername NVARCHAR(50);
    SELECT @AdminUsername = Username FROM Users WHERE Role = 'Admin' AND Department = @DepartmentName;
    IF @AdminUsername IS NOT NULL
    BEGIN
        INSERT INTO Notifications (Username, Message, Type, IsRead, CreatedAt)
        VALUES (@AdminUsername, 'New clearance submission from ' + @StudentUsername + ' for ' + @DepartmentName, 'NewSubmission', 0, GETDATE());
        -- NEW: Activity log
        INSERT INTO ActivityLogs (Username, Message, LogType, CreatedAt)
        VALUES (@AdminUsername, 'Received submission from ' + @StudentUsername, 'Submission', GETDATE());
    END
END
GO

-- Review department clearance
CREATE PROCEDURE sp_ReviewClearance
    @SubmissionID INT,
    @Status NVARCHAR(20),
    @RejectionReason NVARCHAR(500) = NULL,
    @ReviewedBy NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @StudentUsername NVARCHAR(50), @DepartmentName NVARCHAR(50);
    SELECT @StudentUsername = StudentUsername, @DepartmentName = DepartmentName FROM ClearanceSubmissions WHERE SubmissionID = @SubmissionID;
    UPDATE ClearanceSubmissions SET Status = @Status, RejectionReason = @RejectionReason, ReviewedAt = GETDATE(), ReviewedBy = @ReviewedBy WHERE SubmissionID = @SubmissionID;
    DECLARE @Message NVARCHAR(500);
    IF @Status = 'Approved'
    BEGIN
        SET @Message = 'Your ' + @DepartmentName + ' clearance has been APPROVED!';
        INSERT INTO ActivityLogs (Username, Message, LogType, CreatedAt) VALUES (@ReviewedBy, 'Approved submission from ' + @StudentUsername, 'Approval', GETDATE());
    END
    ELSE
    BEGIN
        SET @Message = 'Your ' + @DepartmentName + ' clearance was REJECTED. Reason: ' + ISNULL(@RejectionReason, 'No reason provided');
        INSERT INTO ActivityLogs (Username, Message, LogType, CreatedAt) VALUES (@ReviewedBy, 'Rejected submission from ' + @StudentUsername + ' - Reason: ' + ISNULL(@RejectionReason, 'No reason'), 'Rejection', GETDATE());
    END
    INSERT INTO Notifications (Username, Message, Type, IsRead, CreatedAt) VALUES (@StudentUsername, @Message, 'ClearanceUpdate', 0, GETDATE());
    SELECT @StudentUsername AS StudentUsername, @DepartmentName AS DepartmentName, @Status AS Status;
END
GO

-- Get pending submissions for department admin
CREATE PROCEDURE sp_GetPendingSubmissions
    @DepartmentName NVARCHAR(50)
AS
BEGIN
    SELECT cs.SubmissionID, cs.StudentUsername, u.FullName AS StudentName, u.Program AS StudentProgram,
           cs.ImageData, cs.ImageFileName, cs.SubmittedAt, cs.Status
    FROM ClearanceSubmissions cs
    INNER JOIN Users u ON cs.StudentUsername = u.Username
    WHERE cs.DepartmentName = @DepartmentName AND cs.Status = 'Pending'
    ORDER BY cs.SubmittedAt DESC;
END
GO

-- Get student subject status
CREATE PROCEDURE sp_GetStudentSubjectStatus
    @Username NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT s.SubjectName, s.DisplayOrder, ISNULL(scs.Status, 'Pending') AS Status, scs.SubmittedAt, scs.ReviewedAt, scs.RejectionReason
    FROM Users u
    INNER JOIN Subjects s ON u.Program = s.ProgramCode
    LEFT JOIN SubjectClearanceSubmissions scs ON s.SubjectName = scs.SubjectName AND scs.StudentUsername = @Username
    WHERE u.Username = @Username
    ORDER BY s.DisplayOrder;
END
GO

-- Submit subject clearance
CREATE PROCEDURE sp_SubmitSubjectClearance
    @StudentUsername NVARCHAR(50),
    @SubjectName NVARCHAR(80),
    @ImageData VARBINARY(MAX),
    @ImageFileName NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM SubjectClearanceSubmissions WHERE StudentUsername = @StudentUsername AND SubjectName = @SubjectName)
    BEGIN
        UPDATE SubjectClearanceSubmissions SET ImageData = @ImageData, ImageFileName = @ImageFileName, Status = 'Pending',
            RejectionReason = NULL, SubmittedAt = GETDATE(), ReviewedAt = NULL, ReviewedBy = NULL
        WHERE StudentUsername = @StudentUsername AND SubjectName = @SubjectName;
    END
    ELSE
    BEGIN
        INSERT INTO SubjectClearanceSubmissions (StudentUsername, SubjectName, ImageData, ImageFileName, Status, SubmittedAt)
        VALUES (@StudentUsername, @SubjectName, @ImageData, @ImageFileName, 'Pending', GETDATE());
    END
    DECLARE @InstructorUsername NVARCHAR(50);
    SELECT @InstructorUsername = Username FROM Users WHERE Role = 'Instructor' AND AssignedSubject = @SubjectName;
    IF @InstructorUsername IS NOT NULL
    BEGIN
        INSERT INTO Notifications (Username, Message, Type, IsRead, CreatedAt)
        VALUES (@InstructorUsername, 'New subject clearance from ' + @StudentUsername + ' for ' + @SubjectName, 'NewSubjectSubmission', 0, GETDATE());
        -- NEW: Activity log
        INSERT INTO ActivityLogs (Username, Message, LogType, CreatedAt)
        VALUES (@InstructorUsername, 'Received submission from ' + @StudentUsername, 'Submission', GETDATE());
    END
END
GO

-- Review subject clearance
CREATE PROCEDURE sp_ReviewSubjectClearance
    @SubmissionID INT,
    @Status NVARCHAR(20),
    @RejectionReason NVARCHAR(500) = NULL,
    @ReviewedBy NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @StudentUsername NVARCHAR(50), @SubjectName NVARCHAR(80);
    SELECT @StudentUsername = StudentUsername, @SubjectName = SubjectName FROM SubjectClearanceSubmissions WHERE SubmissionID = @SubmissionID;
    UPDATE SubjectClearanceSubmissions SET Status = @Status, RejectionReason = @RejectionReason, ReviewedAt = GETDATE(), ReviewedBy = @ReviewedBy WHERE SubmissionID = @SubmissionID;
    DECLARE @Message NVARCHAR(500);
    IF @Status = 'Approved'
    BEGIN
        SET @Message = 'Your ' + @SubjectName + ' clearance has been APPROVED!';
        INSERT INTO ActivityLogs (Username, Message, LogType, CreatedAt) VALUES (@ReviewedBy, 'Approved submission from ' + @StudentUsername, 'Approval', GETDATE());
    END
    ELSE
    BEGIN
        SET @Message = 'Your ' + @SubjectName + ' clearance was REJECTED. Reason: ' + ISNULL(@RejectionReason, 'No reason provided');
        INSERT INTO ActivityLogs (Username, Message, LogType, CreatedAt) VALUES (@ReviewedBy, 'Rejected submission from ' + @StudentUsername + ' - Reason: ' + ISNULL(@RejectionReason, 'No reason'), 'Rejection', GETDATE());
    END
    INSERT INTO Notifications (Username, Message, Type, IsRead, CreatedAt) VALUES (@StudentUsername, @Message, 'SubjectClearanceUpdate', 0, GETDATE());
    SELECT @StudentUsername AS StudentUsername, @SubjectName AS SubjectName, @Status AS Status;
END
GO

-- Get pending subject submissions
CREATE PROCEDURE sp_GetPendingSubjectSubmissions
    @SubjectName NVARCHAR(80)
AS
BEGIN
    SELECT scs.SubmissionID, scs.StudentUsername, u.FullName AS StudentName, u.Program AS StudentProgram,
           scs.ImageData, scs.ImageFileName, scs.SubmittedAt, scs.Status
    FROM SubjectClearanceSubmissions scs
    INNER JOIN Users u ON scs.StudentUsername = u.Username
    WHERE scs.SubjectName = @SubjectName AND scs.Status = 'Pending'
    ORDER BY scs.SubmittedAt DESC;
END
GO

-- Get unread notification count
CREATE PROCEDURE sp_GetUnreadNotificationCount
    @Username NVARCHAR(50)
AS
BEGIN
    SELECT COUNT(*) AS UnreadCount FROM Notifications WHERE Username = @Username AND IsRead = 0;
END
GO

-- =============================================
-- NEW: ACTIVITY LOGS PROCEDURES
-- =============================================

-- Get activity logs for a specific admin/instructor
CREATE PROCEDURE sp_GetActivityLogs
    @Username NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT LogID, Message, LogType, CreatedAt
    FROM ActivityLogs
    WHERE Username = @Username
    ORDER BY CreatedAt DESC;
END
GO

-- =============================================
-- COMPLETE!
-- =============================================
PRINT '========================================';
PRINT 'DATABASE SETUP COMPLETE!';
PRINT '========================================';
PRINT '';
PRINT '--- DEPARTMENT ADMIN ACCOUNTS ---';
PRINT 'Password: admin123';
PRINT '  admin_library, admin_sao, admin_cashier';
PRINT '  admin_accounting, admin_dean, admin_records';
PRINT '';
PRINT '--- INSTRUCTOR ACCOUNTS ---';
PRINT 'Password: inst123';
PRINT '  30 instructors - one per subject';
PRINT '';
PRINT '--- STUDENT ACCOUNTS ---';
PRINT '  STUD001 / student123 (BSIT)';
PRINT '';
PRINT '--- NEW: ACTIVITY LOGS ---';
PRINT '  ActivityLogs table + sp_GetActivityLogs';
PRINT '  Logs all submissions, approvals, rejections';
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

2. **Update connection string**
    ```bash
    _connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=StudentClearanceDB;Integrated Security=True;";
     ```

---

### Test Accounts
```
🔑 TEST ACCOUNTS
─────────────────────────────────────────────────────
Student	            STUD001	            student123
Library Admin	    admin_library	    admin123
SAO Admin	        admin_sao	        admin123
Cashier Admin	    admin_cashier	    admin123
Accounting Admin	admin_accounting	    admin123
Dean's Office Admin	admin_dean	        admin123
Records Admin	    admin_records	    admin123
─────────────────────────────────────────────────────
🔑 INSTRUCTORS (All passwords: inst123)
─────────────────────────────────────────────────────
Programming 1	    inst_prog1	        inst123
Database Systems	inst_db	            inst123
Web Dev	            inst_webdev	        inst123
Networking 1	    inst_net1	        inst123
OS Concepts	        inst_os	            inst123
System Analysis	    inst_sysanal	    inst123
Data Structures	    inst_ds	            inst123
Algorithms	        inst_algo	        inst123
Discrete Math	    inst_dmath	        inst123
Software Eng	    inst_se	            inst123
AI Basics	        inst_ai	            inst123
Computer Org	    inst_corg	        inst123
Financial Mgmt	    inst_fin	        inst123
Marketing 101	    inst_mkt	        inst123
Business Ethics	    inst_eth	        inst123
HR Mgmt	            inst_hr	            inst123
Operations Mgmt	    inst_ops	        inst123
Business Law	    inst_blaw	        inst123
Found. of Educ	    inst_fed	        inst123
Ed. Psychology	    inst_edpsy	        inst123
Teaching Methods	inst_tm	            inst123
Curriculum Dev	    inst_cd	            inst123
Assessment 101	    inst_as101	        inst123
Child Dev	        inst_cdev	        inst123
Food & Beverage	    inst_fb	            inst123
Housekeeping	    inst_hk	            inst123
Front Office	    inst_fo	            inst123
Culinary Arts 1	    inst_ca1	        inst123
Tourism Mgmt	    inst_tmgt	        inst123
Bartending	        inst_bt	            inst123
─────────────────────────────────────────────────────
📝 Register to create more students with auto-generated IDs
```
