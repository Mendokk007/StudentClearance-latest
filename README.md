# Student Clearance System

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
2.  Download `StudentClearance_v2.0_Complete.rar`.
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
-- STUDENT CLEARANCE SYSTEM v2.1
-- COMPLETE DATABASE SETUP SCRIPT
-- =============================================
-- Run this entire script in SQL Server Management Studio (SSMS)
-- This will DROP and RECREATE the database from scratch
-- 
-- INCLUDES:
--   - 4 user roles: SuperAdmin, Admin, Instructor, Student
--   - Department clearance (6 departments)
--   - Subject clearance (30 subjects across 5 programs)
--   - Teacher subject load (junction table)
--   - Activity logs with date range filtering
--   - File upload support (Image + PDF)
--   - 18 stored procedures
-- =============================================

USE master;
GO

-- Drop existing database if present (WARNING: deletes all data)
IF EXISTS (SELECT name FROM sys.databases WHERE name = 'StudentClearanceDB')
BEGIN
    ALTER DATABASE StudentClearanceDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE StudentClearanceDB;
END
GO

-- Create fresh database
CREATE DATABASE StudentClearanceDB;
GO

USE StudentClearanceDB;
GO

-- =============================================
-- TABLE: StudentIDCounter
-- Tracks the next available student ID number
-- Used by sp_GenerateStudentID to create STUD001, STUD002...
-- =============================================
CREATE TABLE StudentIDCounter (
    LastNumber INT NOT NULL DEFAULT 0
);
GO

-- =============================================
-- TABLE: Users
-- Stores ALL user accounts across all roles
-- Role determines which dashboard they see after login
-- Department is for Admins only, AssignedSubject for Instructors only
-- =============================================
CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) UNIQUE NOT NULL,
    Password NVARCHAR(100) NOT NULL,
    FullName NVARCHAR(100) NULL,
    Email NVARCHAR(100) NULL,
    Program NVARCHAR(50) NULL,              -- Student's program code (BSIT, BSCS, etc.)
    ProfileImage VARBINARY(MAX) NULL,       -- Profile picture as binary data
    Role NVARCHAR(20) NOT NULL DEFAULT 'Student',  -- SuperAdmin, Admin, Instructor, Student
    Department NVARCHAR(50) NULL,           -- For department admins only (Library, SAO, etc.)
    AssignedSubject NVARCHAR(80) NULL,      -- Legacy: single subject per instructor
    CreatedAt DATETIME DEFAULT GETDATE()
);
GO

-- =============================================
-- TABLE: Departments
-- The 6 clearance departments every student must clear
-- =============================================
CREATE TABLE Departments (
    DepartmentID INT IDENTITY(1,1) PRIMARY KEY,
    DepartmentName NVARCHAR(50) UNIQUE NOT NULL,
    Description NVARCHAR(200) NULL,
    DisplayOrder INT DEFAULT 0               -- Controls display order on student dashboard
);
GO

-- =============================================
-- TABLE: ClearanceSubmissions
-- Stores department clearance submissions from students
-- FileData can be image or PDF (determined by FileType)
-- =============================================
CREATE TABLE ClearanceSubmissions (
    SubmissionID INT IDENTITY(1,1) PRIMARY KEY,
    StudentUsername NVARCHAR(50) NOT NULL,
    DepartmentName NVARCHAR(50) NOT NULL,
    FileData VARBINARY(MAX) NULL,           -- The uploaded file (image or PDF)
    FileName NVARCHAR(255) NULL,            -- Original filename
    FileType NVARCHAR(10) NULL,             -- File extension without dot: 'pdf', 'jpg', 'png'
    Status NVARCHAR(20) DEFAULT 'Pending',  -- Pending, Approved, Rejected
    RejectionReason NVARCHAR(500) NULL,     -- Required if rejected
    SubmittedAt DATETIME DEFAULT GETDATE(),
    ReviewedAt DATETIME NULL,
    ReviewedBy NVARCHAR(50) NULL,           -- Username of admin who reviewed
    FOREIGN KEY (StudentUsername) REFERENCES Users(Username) ON DELETE CASCADE,
    FOREIGN KEY (DepartmentName) REFERENCES Departments(DepartmentName)
);
GO

-- =============================================
-- TABLE: Programs
-- Academic programs that students belong to
-- Each program has multiple subjects
-- =============================================
CREATE TABLE Programs (
    ProgramID INT IDENTITY(1,1) PRIMARY KEY,
    ProgramCode NVARCHAR(10) UNIQUE NOT NULL,   -- Short code: BSIT, BSCS, etc.
    ProgramName NVARCHAR(100) NOT NULL,         -- Full name: BS Information Technology
    CreatedAt DATETIME DEFAULT GETDATE()
);
GO

-- =============================================
-- TABLE: Subjects
-- Subjects within each program
-- Students must clear all subjects in their program
-- =============================================
CREATE TABLE Subjects (
    SubjectID INT IDENTITY(1,1) PRIMARY KEY,
    ProgramCode NVARCHAR(10) NOT NULL,          -- Which program this subject belongs to
    SubjectName NVARCHAR(80) NOT NULL,
    DisplayOrder INT DEFAULT 0,                 -- Display order on student dashboard
    FOREIGN KEY (ProgramCode) REFERENCES Programs(ProgramCode) ON DELETE CASCADE,
    CONSTRAINT UQ_SubjectPerProgram UNIQUE (ProgramCode, SubjectName)  -- No duplicate subject names per program
);
GO

-- =============================================
-- TABLE: SubjectClearanceSubmissions
-- Stores subject clearance submissions (like department but per subject)
-- =============================================
CREATE TABLE SubjectClearanceSubmissions (
    SubmissionID INT IDENTITY(1,1) PRIMARY KEY,
    StudentUsername NVARCHAR(50) NOT NULL,
    SubjectName NVARCHAR(80) NOT NULL,
    FileData VARBINARY(MAX) NULL,
    FileName NVARCHAR(255) NULL,
    FileType NVARCHAR(10) NULL,
    Status NVARCHAR(20) DEFAULT 'Pending',
    RejectionReason NVARCHAR(500) NULL,
    SubmittedAt DATETIME DEFAULT GETDATE(),
    ReviewedAt DATETIME NULL,
    ReviewedBy NVARCHAR(50) NULL,
    FOREIGN KEY (StudentUsername) REFERENCES Users(Username) ON DELETE CASCADE
);
GO

-- =============================================
-- TABLE: Notifications
-- Real-time alerts for students and instructors
-- Populated by stored procedures when submissions are created/reviewed
-- =============================================
CREATE TABLE Notifications (
    NotificationID INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL,             -- Recipient of the notification
    Message NVARCHAR(500) NOT NULL,
    Type NVARCHAR(50) NULL,                     -- NewSubmission, ClearanceUpdate, SubjectClearanceUpdate
    IsRead BIT DEFAULT 0,                       -- 0 = unread, 1 = read
    CreatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (Username) REFERENCES Users(Username) ON DELETE CASCADE
);
GO

-- =============================================
-- TABLE: ActivityLogs
-- Persistent log of all admin/instructor actions
-- Used for the Activity Log panel and CSV download
-- LogType: Submission, Approval, Rejection, System
-- =============================================
CREATE TABLE ActivityLogs (
    LogID INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL,             -- Admin/Instructor who performed the action
    Message NVARCHAR(500) NOT NULL,             -- Human-readable description
    LogType NVARCHAR(30) NOT NULL,              -- Category of action
    CreatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (Username) REFERENCES Users(Username) ON DELETE CASCADE
);
GO

-- =============================================
-- TABLE: TeacherSubjects (JUNCTION TABLE)
-- NEW in v2.1: Allows teachers to cover MULTIPLE subjects
-- Replaces the old single AssignedSubject column in Users table
-- A teacher can have many subjects, a subject can have many teachers
-- =============================================
CREATE TABLE TeacherSubjects (
    TeacherSubjectID INT IDENTITY(1,1) PRIMARY KEY,
    TeacherUsername NVARCHAR(50) NOT NULL,      -- The instructor
    SubjectName NVARCHAR(80) NOT NULL,          -- The subject they cover
    ProgramCode NVARCHAR(10) NOT NULL,          -- Which program this subject is in
    FOREIGN KEY (TeacherUsername) REFERENCES Users(Username) ON DELETE CASCADE,
    CONSTRAINT UQ_TeacherSubject UNIQUE (TeacherUsername, SubjectName, ProgramCode)  -- No duplicate assignments
);
GO

-- =============================================
-- INSERT SEED DATA
-- =============================================

-- Initialize counter: first registered student gets STUD001
INSERT INTO StudentIDCounter (LastNumber) VALUES (0);
GO

-- Insert the 6 clearance departments
INSERT INTO Departments (DepartmentName, Description, DisplayOrder) VALUES
('Library', 'Library clearance for borrowed books', 1),
('SAO', 'Student Affairs Office clearance', 2),
('Cashier', 'Financial clearance for tuition and fees', 3),
('Accounting', 'Accounting department clearance', 4),
('Dean''s Office', 'Dean''s Office clearance', 5),
('Records', 'Records and Registrar clearance', 6);
GO

-- Insert the 5 academic programs
INSERT INTO Programs (ProgramCode, ProgramName) VALUES
('BSIT', 'BS Information Technology'),
('BSCS', 'BS Computer Science'),
('BSBA', 'BS Business Administration'),
('BSED', 'BS Education'),
('BSHM', 'BS Hospitality Management');
GO

-- =============================================
-- INSERT SUBJECTS (6 per program = 30 total)
-- Each subject has a DisplayOrder for sorting on the student dashboard
-- =============================================

-- BSIT Subjects
INSERT INTO Subjects (ProgramCode, SubjectName, DisplayOrder) VALUES
('BSIT', 'Programming 1', 1),
('BSIT', 'Database Systems', 2),
('BSIT', 'Web Dev', 3),
('BSIT', 'Networking 1', 4),
('BSIT', 'OS Concepts', 5),
('BSIT', 'System Analysis', 6);

-- BSCS Subjects
INSERT INTO Subjects (ProgramCode, SubjectName, DisplayOrder) VALUES
('BSCS', 'Data Structures', 1),
('BSCS', 'Algorithms', 2),
('BSCS', 'Discrete Math', 3),
('BSCS', 'Software Eng', 4),
('BSCS', 'AI Basics', 5),
('BSCS', 'Computer Org', 6);

-- BSBA Subjects
INSERT INTO Subjects (ProgramCode, SubjectName, DisplayOrder) VALUES
('BSBA', 'Financial Mgmt', 1),
('BSBA', 'Marketing 101', 2),
('BSBA', 'Business Ethics', 3),
('BSBA', 'HR Mgmt', 4),
('BSBA', 'Operations Mgmt', 5),
('BSBA', 'Business Law', 6);

-- BSED Subjects
INSERT INTO Subjects (ProgramCode, SubjectName, DisplayOrder) VALUES
('BSED', 'Found. of Educ', 1),
('BSED', 'Ed. Psychology', 2),
('BSED', 'Teaching Methods', 3),
('BSED', 'Curriculum Dev', 4),
('BSED', 'Assessment 101', 5),
('BSED', 'Child Dev', 6);

-- BSHM Subjects
INSERT INTO Subjects (ProgramCode, SubjectName, DisplayOrder) VALUES
('BSHM', 'Food & Beverage', 1),
('BSHM', 'Housekeeping', 2),
('BSHM', 'Front Office', 3),
('BSHM', 'Culinary Arts 1', 4),
('BSHM', 'Tourism Mgmt', 5),
('BSHM', 'Bartending', 6);
GO

-- =============================================
-- INSERT USER ACCOUNTS
-- =============================================

-- Super Admin: manages the entire system (programs, subjects, teachers, students)
INSERT INTO Users (Username, Password, FullName, Role) VALUES
('super_admin', 'admin123', 'System Administrator', 'SuperAdmin');
GO

-- Department Admins: each reviews submissions for their assigned department
INSERT INTO Users (Username, Password, FullName, Role, Department) VALUES
('admin_library', 'admin123', 'Library Administrator', 'Admin', 'Library'),
('admin_sao', 'admin123', 'SAO Administrator', 'Admin', 'SAO'),
('admin_cashier', 'admin123', 'Cashier Administrator', 'Admin', 'Cashier'),
('admin_accounting', 'admin123', 'Accounting Administrator', 'Admin', 'Accounting'),
('admin_dean', 'admin123', 'Dean''s Office Administrator', 'Admin', 'Dean''s Office'),
('admin_records', 'admin123', 'Records Administrator', 'Admin', 'Records');
GO

-- Instructors: each reviews submissions for their assigned subject(s)
-- Note: AssignedSubject is the PRIMARY subject; TeacherSubjects junction table handles additional subjects
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

-- Seed TeacherSubjects: each instructor initially covers their AssignedSubject
-- This mirrors the legacy single-subject setup so existing functionality continues to work
INSERT INTO TeacherSubjects (TeacherUsername, SubjectName, ProgramCode)
SELECT Username, AssignedSubject,
    (SELECT TOP 1 ProgramCode FROM Subjects WHERE SubjectName = Users.AssignedSubject)
FROM Users WHERE Role = 'Instructor' AND AssignedSubject IS NOT NULL;
GO

-- Test student account for development
INSERT INTO Users (Username, Password, FullName, Role, Program) VALUES
('STUD001', 'student123', 'John Doe', 'Student', 'BSIT');
GO

-- Set counter so next registered student gets STUD002
UPDATE StudentIDCounter SET LastNumber = 1;
GO

-- =============================================
-- STORED PROCEDURES
-- =============================================

-- =============================================
-- sp_GenerateStudentID
-- Atomically generates the next student ID (STUD001, STUD002...)
-- Uses OUTPUT parameter to return the new ID
-- Thread-safe: uses UPDATE with variable assignment in a single statement
-- =============================================
CREATE PROCEDURE sp_GenerateStudentID
    @NewStudentID NVARCHAR(50) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @NextNumber INT;
    
    -- Atomically increment and retrieve the next number
    UPDATE StudentIDCounter 
    SET @NextNumber = LastNumber + 1, 
        LastNumber = LastNumber + 1;
    
    -- Format with leading zeros: STUD001, STUD010, STUD100
    IF @NextNumber < 10 
        SET @NewStudentID = 'STUD00' + CAST(@NextNumber AS NVARCHAR);
    ELSE IF @NextNumber < 100 
        SET @NewStudentID = 'STUD0' + CAST(@NextNumber AS NVARCHAR);
    ELSE 
        SET @NewStudentID = 'STUD' + CAST(@NextNumber AS NVARCHAR);
END
GO

-- =============================================
-- sp_GetStudentClearanceStatus
-- Returns clearance status for all 6 departments for a given student
-- Shows 'Pending' if no submission exists yet
-- =============================================
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

-- =============================================
-- sp_SubmitClearance
-- Student submits a file for department clearance
-- If a submission already exists, updates it (re-submission)
-- Also creates a notification for the department admin
-- Also logs the activity
-- =============================================
CREATE PROCEDURE sp_SubmitClearance
    @StudentUsername NVARCHAR(50),
    @DepartmentName NVARCHAR(50),
    @FileData VARBINARY(MAX),
    @FileName NVARCHAR(255),
    @FileType NVARCHAR(10)          -- 'pdf', 'jpg', 'png', etc.
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Check if student already submitted for this department
    IF EXISTS (SELECT 1 FROM ClearanceSubmissions 
               WHERE StudentUsername = @StudentUsername 
               AND DepartmentName = @DepartmentName)
    BEGIN
        -- Update existing submission (re-submission)
        UPDATE ClearanceSubmissions 
        SET FileData = @FileData, 
            FileName = @FileName, 
            FileType = @FileType, 
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
        -- Insert new submission
        INSERT INTO ClearanceSubmissions 
            (StudentUsername, DepartmentName, FileData, FileName, FileType, Status, SubmittedAt)
        VALUES 
            (@StudentUsername, @DepartmentName, @FileData, @FileName, @FileType, 'Pending', GETDATE());
    END

    -- Find the admin assigned to this department
    DECLARE @AdminUsername NVARCHAR(50);
    SELECT @AdminUsername = Username FROM Users 
    WHERE Role = 'Admin' AND Department = @DepartmentName;
    
    -- Notify admin + log activity
    IF @AdminUsername IS NOT NULL
    BEGIN
        INSERT INTO Notifications (Username, Message, Type, IsRead, CreatedAt)
        VALUES (@AdminUsername, 
                'New clearance submission from ' + @StudentUsername + ' for ' + @DepartmentName,
                'NewSubmission', 0, GETDATE());
        
        INSERT INTO ActivityLogs (Username, Message, LogType, CreatedAt)
        VALUES (@AdminUsername, 
                'Received submission from ' + @StudentUsername, 
                'Submission', GETDATE());
    END
END
GO

-- =============================================
-- sp_ReviewClearance
-- Admin approves or rejects a department clearance submission
-- Updates the submission status and notifies the student
-- Also logs the activity (approval or rejection)
-- =============================================
CREATE PROCEDURE sp_ReviewClearance
    @SubmissionID INT,
    @Status NVARCHAR(20),               -- 'Approved' or 'Rejected'
    @RejectionReason NVARCHAR(500) = NULL,
    @ReviewedBy NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Get the student and department from the submission
    DECLARE @StudentUsername NVARCHAR(50), @DepartmentName NVARCHAR(50);
    SELECT @StudentUsername = StudentUsername, 
           @DepartmentName = DepartmentName 
    FROM ClearanceSubmissions 
    WHERE SubmissionID = @SubmissionID;
    
    -- Update the submission
    UPDATE ClearanceSubmissions
    SET Status = @Status,
        RejectionReason = @RejectionReason,
        ReviewedAt = GETDATE(),
        ReviewedBy = @ReviewedBy
    WHERE SubmissionID = @SubmissionID;
    
    -- Build notification message and log the action
    DECLARE @Message NVARCHAR(500);
    IF @Status = 'Approved'
    BEGIN
        SET @Message = 'Your ' + @DepartmentName + ' clearance has been APPROVED!';
        INSERT INTO ActivityLogs (Username, Message, LogType, CreatedAt) 
        VALUES (@ReviewedBy, 'Approved submission from ' + @StudentUsername, 'Approval', GETDATE());
    END
    ELSE
    BEGIN
        SET @Message = 'Your ' + @DepartmentName + ' clearance was REJECTED. Reason: ' 
                     + ISNULL(@RejectionReason, 'No reason provided');
        INSERT INTO ActivityLogs (Username, Message, LogType, CreatedAt) 
        VALUES (@ReviewedBy, 
                'Rejected submission from ' + @StudentUsername + ' - Reason: ' 
                + ISNULL(@RejectionReason, 'No reason'), 
                'Rejection', GETDATE());
    END
    
    -- Notify the student
    INSERT INTO Notifications (Username, Message, Type, IsRead, CreatedAt)
    VALUES (@StudentUsername, @Message, 'ClearanceUpdate', 0, GETDATE());
    
    -- Return confirmation
    SELECT @StudentUsername AS StudentUsername, @DepartmentName AS DepartmentName, @Status AS Status;
END
GO

-- =============================================
-- sp_GetPendingSubmissions
-- Returns all pending department submissions for an admin
-- =============================================
CREATE PROCEDURE sp_GetPendingSubmissions
    @DepartmentName NVARCHAR(50)
AS
BEGIN
    SELECT 
        cs.SubmissionID,
        cs.StudentUsername,
        u.FullName AS StudentName,
        u.Program AS StudentProgram,
        cs.FileData,
        cs.FileName,
        cs.FileType,
        cs.SubmittedAt,
        cs.Status
    FROM ClearanceSubmissions cs
    INNER JOIN Users u ON cs.StudentUsername = u.Username
    WHERE cs.DepartmentName = @DepartmentName 
      AND cs.Status = 'Pending'
    ORDER BY cs.SubmittedAt DESC;
END
GO

-- =============================================
-- sp_GetStudentSubjectStatus
-- Returns subject clearance status for a student's program
-- Only shows subjects that belong to the student's program
-- =============================================
CREATE PROCEDURE sp_GetStudentSubjectStatus
    @Username NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        s.SubjectName,
        s.DisplayOrder,
        ISNULL(scs.Status, 'Pending') AS Status,
        scs.SubmittedAt,
        scs.ReviewedAt,
        scs.RejectionReason
    FROM Users u
    INNER JOIN Subjects s ON u.Program = s.ProgramCode
    LEFT JOIN SubjectClearanceSubmissions scs 
        ON s.SubjectName = scs.SubjectName 
        AND scs.StudentUsername = @Username
    WHERE u.Username = @Username
    ORDER BY s.DisplayOrder;
END
GO

-- =============================================
-- sp_SubmitSubjectClearance
-- Student submits a file for subject clearance
-- NEW in v2.1: Notifies ALL teachers assigned to this subject
-- Uses a CURSOR to loop through TeacherSubjects junction table
-- This supports the multi-teacher subject load feature
-- =============================================
CREATE PROCEDURE sp_SubmitSubjectClearance
    @StudentUsername NVARCHAR(50),
    @SubjectName NVARCHAR(80),
    @FileData VARBINARY(MAX),
    @FileName NVARCHAR(255),
    @FileType NVARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Upsert: update if exists, insert if new
    IF EXISTS (SELECT 1 FROM SubjectClearanceSubmissions 
               WHERE StudentUsername = @StudentUsername 
               AND SubjectName = @SubjectName)
    BEGIN
        UPDATE SubjectClearanceSubmissions 
        SET FileData = @FileData, 
            FileName = @FileName, 
            FileType = @FileType, 
            Status = 'Pending',
            RejectionReason = NULL, 
            SubmittedAt = GETDATE(), 
            ReviewedAt = NULL, 
            ReviewedBy = NULL
        WHERE StudentUsername = @StudentUsername 
          AND SubjectName = @SubjectName;
    END
    ELSE
    BEGIN
        INSERT INTO SubjectClearanceSubmissions 
            (StudentUsername, SubjectName, FileData, FileName, FileType, Status, SubmittedAt)
        VALUES 
            (@StudentUsername, @SubjectName, @FileData, @FileName, @FileType, 'Pending', GETDATE());
    END

    -- NOTIFY ALL TEACHERS assigned to this subject via the junction table
    -- Cursor loops through each teacher and creates a notification + activity log
    DECLARE @TeacherUsername NVARCHAR(50);
    DECLARE teacher_cursor CURSOR FOR
        SELECT TeacherUsername FROM TeacherSubjects WHERE SubjectName = @SubjectName;
    
    OPEN teacher_cursor;
    FETCH NEXT FROM teacher_cursor INTO @TeacherUsername;
    
    WHILE @@FETCH_STATUS = 0
    BEGIN
        -- Create notification for this teacher
        INSERT INTO Notifications (Username, Message, Type, IsRead, CreatedAt)
        VALUES (@TeacherUsername, 
                'New subject clearance from ' + @StudentUsername + ' for ' + @SubjectName,
                'NewSubjectSubmission', 0, GETDATE());
        
        -- Log the submission
        INSERT INTO ActivityLogs (Username, Message, LogType, CreatedAt)
        VALUES (@TeacherUsername, 
                'Received submission from ' + @StudentUsername, 
                'Submission', GETDATE());
        
        FETCH NEXT FROM teacher_cursor INTO @TeacherUsername;
    END;
    
    CLOSE teacher_cursor;
    DEALLOCATE teacher_cursor;
END
GO

-- =============================================
-- sp_ReviewSubjectClearance
-- Instructor approves or rejects a subject clearance submission
-- Works the same as department review but for subjects
-- =============================================
CREATE PROCEDURE sp_ReviewSubjectClearance
    @SubmissionID INT,
    @Status NVARCHAR(20),
    @RejectionReason NVARCHAR(500) = NULL,
    @ReviewedBy NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @StudentUsername NVARCHAR(50), @SubjectName NVARCHAR(80);
    SELECT @StudentUsername = StudentUsername, 
           @SubjectName = SubjectName 
    FROM SubjectClearanceSubmissions 
    WHERE SubmissionID = @SubmissionID;
    
    UPDATE SubjectClearanceSubmissions
    SET Status = @Status,
        RejectionReason = @RejectionReason,
        ReviewedAt = GETDATE(),
        ReviewedBy = @ReviewedBy
    WHERE SubmissionID = @SubmissionID;
    
    DECLARE @Message NVARCHAR(500);
    IF @Status = 'Approved'
    BEGIN
        SET @Message = 'Your ' + @SubjectName + ' clearance has been APPROVED!';
        INSERT INTO ActivityLogs (Username, Message, LogType, CreatedAt) 
        VALUES (@ReviewedBy, 'Approved submission from ' + @StudentUsername, 'Approval', GETDATE());
    END
    ELSE
    BEGIN
        SET @Message = 'Your ' + @SubjectName + ' clearance was REJECTED. Reason: ' 
                     + ISNULL(@RejectionReason, 'No reason provided');
        INSERT INTO ActivityLogs (Username, Message, LogType, CreatedAt) 
        VALUES (@ReviewedBy, 
                'Rejected submission from ' + @StudentUsername + ' - Reason: ' 
                + ISNULL(@RejectionReason, 'No reason'), 
                'Rejection', GETDATE());
    END
    
    INSERT INTO Notifications (Username, Message, Type, IsRead, CreatedAt)
    VALUES (@StudentUsername, @Message, 'SubjectClearanceUpdate', 0, GETDATE());
    
    SELECT @StudentUsername AS StudentUsername, @SubjectName AS SubjectName, @Status AS Status;
END
GO

-- =============================================
-- sp_GetPendingSubjectSubmissions
-- UPDATED in v2.1: Now accepts @TeacherUsername instead of @SubjectName
-- Joins with TeacherSubjects to get ALL subjects the teacher covers
-- This enables the multi-subject teacher load feature
-- =============================================
CREATE PROCEDURE sp_GetPendingSubjectSubmissions
    @TeacherUsername NVARCHAR(50)
AS
BEGIN
    SELECT 
        scs.SubmissionID,
        scs.StudentUsername,
        u.FullName AS StudentName,
        u.Program AS StudentProgram,
        scs.FileData,
        scs.FileName,
        scs.FileType,
        scs.SubmittedAt,
        scs.Status,
        scs.SubjectName                    -- Include subject name so instructor knows which subject
    FROM SubjectClearanceSubmissions scs
    INNER JOIN Users u ON scs.StudentUsername = u.Username
    INNER JOIN TeacherSubjects ts ON scs.SubjectName = ts.SubjectName  -- Junction table join
    WHERE ts.TeacherUsername = @TeacherUsername 
      AND scs.Status = 'Pending'
    ORDER BY scs.SubmittedAt DESC;
END
GO

-- =============================================
-- sp_GetUnreadNotificationCount
-- Returns count of unread notifications for badge display
-- =============================================
CREATE PROCEDURE sp_GetUnreadNotificationCount
    @Username NVARCHAR(50)
AS
BEGIN
    SELECT COUNT(*) AS UnreadCount 
    FROM Notifications 
    WHERE Username = @Username AND IsRead = 0;
END
GO

-- =============================================
-- sp_GetActivityLogs
-- Returns all activity logs for an admin/instructor
-- Used in the Activity Log panel
-- =============================================
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
-- SUPER ADMIN PROCEDURES (v2.1)
-- These procedures power the SuperAdminForm operator panel
-- =============================================

-- =============================================
-- sp_GetLogsByDateRange
-- Returns activity logs filtered by date range
-- Used by the Download CSV feature
-- @EndDate is inclusive (adds 1 day in WHERE clause)
-- =============================================
CREATE PROCEDURE sp_GetLogsByDateRange
    @Username NVARCHAR(50),
    @StartDate DATETIME,
    @EndDate DATETIME
AS
BEGIN
    SET NOCOUNT ON;
    SELECT LogID, Message, LogType, CreatedAt
    FROM ActivityLogs
    WHERE Username = @Username
      AND CreatedAt >= @StartDate
      AND CreatedAt < DATEADD(DAY, 1, @EndDate)  -- Include the entire end date
    ORDER BY CreatedAt DESC;
END
GO

-- Get all instructor accounts
CREATE PROCEDURE sp_GetAllTeachers
AS
BEGIN
    SELECT Username, FullName, Email, CreatedAt 
    FROM Users 
    WHERE Role = 'Instructor' 
    ORDER BY Username;
END
GO

-- Get all student accounts
CREATE PROCEDURE sp_GetAllStudents
AS
BEGIN
    SELECT Username, FullName, Email, Program, CreatedAt 
    FROM Users 
    WHERE Role = 'Student' 
    ORDER BY Username;
END
GO

-- Get all programs
CREATE PROCEDURE sp_GetAllPrograms
AS
BEGIN
    SELECT ProgramID, ProgramCode, ProgramName, CreatedAt 
    FROM Programs 
    ORDER BY ProgramCode;
END
GO

-- Get subjects for a specific program
CREATE PROCEDURE sp_GetProgramSubjects
    @ProgramCode NVARCHAR(10)
AS
BEGIN
    SELECT SubjectID, SubjectName, DisplayOrder 
    FROM Subjects 
    WHERE ProgramCode = @ProgramCode 
    ORDER BY DisplayOrder;
END
GO

-- Get all subjects a teacher covers
CREATE PROCEDURE sp_GetTeacherSubjects
    @TeacherUsername NVARCHAR(50)
AS
BEGIN
    SELECT ts.SubjectName, ts.ProgramCode 
    FROM TeacherSubjects ts 
    WHERE ts.TeacherUsername = @TeacherUsername;
END
GO

-- Assign a teacher to a subject (adds to junction table)
CREATE PROCEDURE sp_AssignTeacherSubject
    @TeacherUsername NVARCHAR(50),
    @SubjectName NVARCHAR(80),
    @ProgramCode NVARCHAR(10)
AS
BEGIN
    -- Only insert if not already assigned (prevents duplicates)
    IF NOT EXISTS (SELECT 1 FROM TeacherSubjects 
                   WHERE TeacherUsername = @TeacherUsername 
                   AND SubjectName = @SubjectName 
                   AND ProgramCode = @ProgramCode)
    BEGIN
        INSERT INTO TeacherSubjects (TeacherUsername, SubjectName, ProgramCode) 
        VALUES (@TeacherUsername, @SubjectName, @ProgramCode);
    END
END
GO

-- Remove a teacher from a subject
CREATE PROCEDURE sp_RemoveTeacherSubject
    @TeacherUsername NVARCHAR(50),
    @SubjectName NVARCHAR(80),
    @ProgramCode NVARCHAR(10)
AS
BEGIN
    DELETE FROM TeacherSubjects 
    WHERE TeacherUsername = @TeacherUsername 
      AND SubjectName = @SubjectName 
      AND ProgramCode = @ProgramCode;
END
GO

-- Add a new program
CREATE PROCEDURE sp_AddProgram
    @ProgramCode NVARCHAR(10),
    @ProgramName NVARCHAR(100)
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Programs WHERE ProgramCode = @ProgramCode)
    BEGIN
        INSERT INTO Programs (ProgramCode, ProgramName) VALUES (@ProgramCode, @ProgramName);
    END
END
GO

-- Delete a program and all its subjects (CASCADE)
CREATE PROCEDURE sp_DeleteProgram
    @ProgramCode NVARCHAR(10)
AS
BEGIN
    DELETE FROM Programs WHERE ProgramCode = @ProgramCode;
END
GO

-- Add a subject to a program
CREATE PROCEDURE sp_AddSubject
    @ProgramCode NVARCHAR(10),
    @SubjectName NVARCHAR(80),
    @DisplayOrder INT
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Subjects WHERE ProgramCode = @ProgramCode AND SubjectName = @SubjectName)
    BEGIN
        INSERT INTO Subjects (ProgramCode, SubjectName, DisplayOrder) 
        VALUES (@ProgramCode, @SubjectName, @DisplayOrder);
    END
END
GO

-- Edit an existing subject's name or display order
CREATE PROCEDURE sp_EditSubject
    @SubjectID INT,
    @SubjectName NVARCHAR(80),
    @DisplayOrder INT
AS
BEGIN
    UPDATE Subjects 
    SET SubjectName = @SubjectName, DisplayOrder = @DisplayOrder 
    WHERE SubjectID = @SubjectID;
END
GO

-- Delete a subject
CREATE PROCEDURE sp_DeleteSubject
    @SubjectID INT
AS
BEGIN
    DELETE FROM Subjects WHERE SubjectID = @SubjectID;
END
GO

-- Change a student's program (for program shifting)
CREATE PROCEDURE sp_UpdateStudentProgram
    @Username NVARCHAR(50),
    @Program NVARCHAR(50)
AS
BEGIN
    UPDATE Users SET Program = @Program WHERE Username = @Username;
END
GO

-- Update user details (name, email, password)
-- Parameters are optional: only non-null values are updated
CREATE PROCEDURE sp_UpdateUser
    @Username NVARCHAR(50),
    @Password NVARCHAR(100) = NULL,
    @FullName NVARCHAR(100) = NULL,
    @Email NVARCHAR(100) = NULL
AS
BEGIN
    IF @Password IS NOT NULL
        UPDATE Users SET Password = @Password WHERE Username = @Username;
    IF @FullName IS NOT NULL
        UPDATE Users SET FullName = @FullName WHERE Username = @Username;
    IF @Email IS NOT NULL
        UPDATE Users SET Email = @Email WHERE Username = @Username;
END
GO

-- Create a new user account (used by Super Admin)
CREATE PROCEDURE sp_CreateUser
    @Username NVARCHAR(50),
    @Password NVARCHAR(100),
    @FullName NVARCHAR(100),
    @Role NVARCHAR(20),
    @Program NVARCHAR(50) = NULL,
    @Department NVARCHAR(50) = NULL
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Users WHERE Username = @Username)
    BEGIN
        INSERT INTO Users (Username, Password, FullName, Role, Program, Department)
        VALUES (@Username, @Password, @FullName, @Role, @Program, @Department);
    END
END
GO

-- Delete a user (cannot delete SuperAdmin)
CREATE PROCEDURE sp_DeleteUser
    @Username NVARCHAR(50)
AS
BEGIN
    DELETE FROM Users WHERE Username = @Username AND Role != 'SuperAdmin';
END
GO

-- =============================================
-- SETUP COMPLETE
-- =============================================
PRINT '========================================';
PRINT 'DATABASE SETUP COMPLETE - v2.1';
PRINT '========================================';
PRINT '';
PRINT '--- SUPER ADMIN ---';
PRINT '  super_admin / admin123';
PRINT '';
PRINT '--- DEPARTMENT ADMINS ---';
PRINT 'Password: admin123';
PRINT '  admin_library, admin_sao, admin_cashier';
PRINT '  admin_accounting, admin_dean, admin_records';
PRINT '';
PRINT '--- INSTRUCTORS ---';
PRINT 'Password: inst123';
PRINT '  30 instructors - one per subject';
PRINT '';
PRINT '--- STUDENTS ---';
PRINT '  STUD001 / student123 (BSIT)';
PRINT '';
PRINT '--- NEW v2.1 FEATURES ---';
PRINT '  TeacherSubjects junction table (multi-subject teachers)';
PRINT '  SuperAdmin role + operator panel procedures';
PRINT '  Activity log date range filtering';
PRINT '  FileType support (PDF/Image uploads)';
PRINT '  Dynamic programs & subjects CRUD';
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
Accounting Admin	admin_accounting	admin123
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
