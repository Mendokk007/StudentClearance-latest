namespace CarDealership
{
    partial class SuperAdminForm
    {
        private System.ComponentModel.IContainer components = null;

        // Top bar
        private System.Windows.Forms.Panel pnlTopBar;
        private System.Windows.Forms.PictureBox pbLogo;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Button btnThemeToggle;

        // Tab navigation
        private System.Windows.Forms.Panel pnlTabNav;
        private System.Windows.Forms.Button btnProgramsTab;
        private System.Windows.Forms.Button btnSubjectsTab;
        private System.Windows.Forms.Button btnTeachersTab;
        private System.Windows.Forms.Button btnStudentsTab;

        // Tab panels
        private System.Windows.Forms.Panel pnlProgramsTab;
        private System.Windows.Forms.Panel pnlSubjectsTab;
        private System.Windows.Forms.Panel pnlTeachersTab;
        private System.Windows.Forms.Panel pnlStudentsTab;

        // Programs Tab
        private System.Windows.Forms.DataGridView dgvPrograms;
        private System.Windows.Forms.Button btnAddProgram;
        private System.Windows.Forms.Button btnEditProgram;
        private System.Windows.Forms.Button btnDeleteProgram;
        private System.Windows.Forms.Label lblProgramsTitle;

        // Subjects Tab
        private System.Windows.Forms.DataGridView dgvAllSubjects;
        private System.Windows.Forms.Button btnAddSubject;
        private System.Windows.Forms.Button btnEditSubject;
        private System.Windows.Forms.Button btnDeleteSubject;
        private System.Windows.Forms.Button btnManageTeachers;
        private System.Windows.Forms.Label lblSubjectsTitle;

        // Teachers Tab
        private System.Windows.Forms.DataGridView dgvTeachers;
        private System.Windows.Forms.Button btnCreateTeacher;
        private System.Windows.Forms.Button btnEditTeacher;
        private System.Windows.Forms.Button btnResetTeacherPassword;
        private System.Windows.Forms.Button btnDeleteTeacher;
        private System.Windows.Forms.Button btnEditCoverage;
        private System.Windows.Forms.Label lblTeachersTitle;

        // Students Tab
        private System.Windows.Forms.DataGridView dgvStudents;
        private System.Windows.Forms.Button btnCreateStudent;
        private System.Windows.Forms.Button btnEditStudent;
        private System.Windows.Forms.Button btnResetStudentPassword;
        private System.Windows.Forms.Button btnDeleteStudent;
        private System.Windows.Forms.Label lblStudentsTitle;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            // =============================================
            // TOP BAR
            // =============================================
            this.pnlTopBar = new System.Windows.Forms.Panel();
            this.pbLogo = new System.Windows.Forms.PictureBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.btnLogout = new System.Windows.Forms.Button();
            this.btnThemeToggle = new System.Windows.Forms.Button();

            // =============================================
            // TAB NAVIGATION
            // =============================================
            this.pnlTabNav = new System.Windows.Forms.Panel();
            this.btnProgramsTab = new System.Windows.Forms.Button();
            this.btnSubjectsTab = new System.Windows.Forms.Button();
            this.btnTeachersTab = new System.Windows.Forms.Button();
            this.btnStudentsTab = new System.Windows.Forms.Button();

            // =============================================
            // PROGRAMS TAB
            // =============================================
            this.pnlProgramsTab = new System.Windows.Forms.Panel();
            this.lblProgramsTitle = new System.Windows.Forms.Label();
            this.dgvPrograms = new System.Windows.Forms.DataGridView();
            this.btnAddProgram = new System.Windows.Forms.Button();
            this.btnEditProgram = new System.Windows.Forms.Button();
            this.btnDeleteProgram = new System.Windows.Forms.Button();

            // =============================================
            // SUBJECTS TAB
            // =============================================
            this.pnlSubjectsTab = new System.Windows.Forms.Panel();
            this.lblSubjectsTitle = new System.Windows.Forms.Label();
            this.dgvAllSubjects = new System.Windows.Forms.DataGridView();
            this.btnAddSubject = new System.Windows.Forms.Button();
            this.btnEditSubject = new System.Windows.Forms.Button();
            this.btnDeleteSubject = new System.Windows.Forms.Button();
            this.btnManageTeachers = new System.Windows.Forms.Button();

            // =============================================
            // TEACHERS TAB
            // =============================================
            this.pnlTeachersTab = new System.Windows.Forms.Panel();
            this.lblTeachersTitle = new System.Windows.Forms.Label();
            this.dgvTeachers = new System.Windows.Forms.DataGridView();
            this.btnCreateTeacher = new System.Windows.Forms.Button();
            this.btnEditTeacher = new System.Windows.Forms.Button();
            this.btnResetTeacherPassword = new System.Windows.Forms.Button();
            this.btnDeleteTeacher = new System.Windows.Forms.Button();
            this.btnEditCoverage = new System.Windows.Forms.Button();

            // =============================================
            // STUDENTS TAB
            // =============================================
            this.pnlStudentsTab = new System.Windows.Forms.Panel();
            this.lblStudentsTitle = new System.Windows.Forms.Label();
            this.dgvStudents = new System.Windows.Forms.DataGridView();
            this.btnCreateStudent = new System.Windows.Forms.Button();
            this.btnEditStudent = new System.Windows.Forms.Button();
            this.btnResetStudentPassword = new System.Windows.Forms.Button();
            this.btnDeleteStudent = new System.Windows.Forms.Button();

            // =============================================
            // SUSPEND LAYOUT
            // =============================================
            this.pnlTopBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
            this.pnlTabNav.SuspendLayout();
            this.pnlProgramsTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPrograms)).BeginInit();
            this.pnlSubjectsTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAllSubjects)).BeginInit();
            this.pnlTeachersTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTeachers)).BeginInit();
            this.pnlStudentsTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStudents)).BeginInit();
            this.SuspendLayout();

            // =============================================
            // pnlTopBar
            // =============================================
            this.pnlTopBar.BackColor = System.Drawing.Color.FromArgb(47, 49, 54);
            this.pnlTopBar.Controls.Add(this.pbLogo);
            this.pnlTopBar.Controls.Add(this.lblTitle);
            this.pnlTopBar.Controls.Add(this.lblSubtitle);
            this.pnlTopBar.Controls.Add(this.btnLogout);
            this.pnlTopBar.Controls.Add(this.btnThemeToggle);
            this.pnlTopBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTopBar.Location = new System.Drawing.Point(0, 0);
            this.pnlTopBar.Name = "pnlTopBar";
            this.pnlTopBar.Size = new System.Drawing.Size(1000, 70);
            this.pnlTopBar.TabIndex = 0;

            // pbLogo
            this.pbLogo.BackColor = System.Drawing.Color.Transparent;
            this.pbLogo.Location = new System.Drawing.Point(450, 10);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.Size = new System.Drawing.Size(100, 50);
            this.pbLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbLogo.TabStop = false;

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(84, 172, 191);
            this.lblTitle.Location = new System.Drawing.Point(15, 12);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(180, 25);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Super Admin Panel";

            // lblSubtitle
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(185, 187, 190);
            this.lblSubtitle.Location = new System.Drawing.Point(17, 38);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(126, 15);
            this.lblSubtitle.TabIndex = 1;
            this.lblSubtitle.Text = "System Administrator";

            // btnLogout
            this.btnLogout.BackColor = System.Drawing.Color.FromArgb(38, 101, 140);
            this.btnLogout.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogout.FlatAppearance.BorderSize = 0;
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnLogout.ForeColor = System.Drawing.Color.White;
            this.btnLogout.Location = new System.Drawing.Point(890, 20);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(90, 30);
            this.btnLogout.TabIndex = 2;
            this.btnLogout.Text = "Logout";
            this.btnLogout.UseVisualStyleBackColor = false;

            // btnThemeToggle
            this.btnThemeToggle.BackColor = System.Drawing.Color.Transparent;
            this.btnThemeToggle.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnThemeToggle.FlatAppearance.BorderSize = 0;
            this.btnThemeToggle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThemeToggle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnThemeToggle.ForeColor = System.Drawing.Color.White;
            this.btnThemeToggle.Location = new System.Drawing.Point(840, 18);
            this.btnThemeToggle.Name = "btnThemeToggle";
            this.btnThemeToggle.Size = new System.Drawing.Size(45, 35);
            this.btnThemeToggle.Text = "🌙";
            this.btnThemeToggle.UseVisualStyleBackColor = false;

            // =============================================
            // pnlTabNav
            // =============================================
            this.pnlTabNav.BackColor = System.Drawing.Color.FromArgb(1, 20, 50);
            this.pnlTabNav.Controls.Add(this.btnProgramsTab);
            this.pnlTabNav.Controls.Add(this.btnSubjectsTab);
            this.pnlTabNav.Controls.Add(this.btnTeachersTab);
            this.pnlTabNav.Controls.Add(this.btnStudentsTab);
            this.pnlTabNav.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTabNav.Location = new System.Drawing.Point(0, 70);
            this.pnlTabNav.Name = "pnlTabNav";
            this.pnlTabNav.Size = new System.Drawing.Size(1000, 40);
            this.pnlTabNav.TabIndex = 1;

            // btnProgramsTab
            this.btnProgramsTab.BackColor = System.Drawing.Color.FromArgb(84, 172, 191);
            this.btnProgramsTab.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnProgramsTab.FlatAppearance.BorderSize = 0;
            this.btnProgramsTab.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProgramsTab.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnProgramsTab.ForeColor = System.Drawing.Color.White;
            this.btnProgramsTab.Location = new System.Drawing.Point(0, 0);
            this.btnProgramsTab.Name = "btnProgramsTab";
            this.btnProgramsTab.Size = new System.Drawing.Size(250, 40);
            this.btnProgramsTab.TabIndex = 0;
            this.btnProgramsTab.Text = "Programs";

            // btnSubjectsTab
            this.btnSubjectsTab.BackColor = System.Drawing.Color.FromArgb(38, 101, 140);
            this.btnSubjectsTab.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSubjectsTab.FlatAppearance.BorderSize = 0;
            this.btnSubjectsTab.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSubjectsTab.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSubjectsTab.ForeColor = System.Drawing.Color.White;
            this.btnSubjectsTab.Location = new System.Drawing.Point(250, 0);
            this.btnSubjectsTab.Name = "btnSubjectsTab";
            this.btnSubjectsTab.Size = new System.Drawing.Size(250, 40);
            this.btnSubjectsTab.TabIndex = 1;
            this.btnSubjectsTab.Text = "Subjects";

            // btnTeachersTab
            this.btnTeachersTab.BackColor = System.Drawing.Color.FromArgb(38, 101, 140);
            this.btnTeachersTab.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTeachersTab.FlatAppearance.BorderSize = 0;
            this.btnTeachersTab.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTeachersTab.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnTeachersTab.ForeColor = System.Drawing.Color.White;
            this.btnTeachersTab.Location = new System.Drawing.Point(500, 0);
            this.btnTeachersTab.Name = "btnTeachersTab";
            this.btnTeachersTab.Size = new System.Drawing.Size(250, 40);
            this.btnTeachersTab.TabIndex = 2;
            this.btnTeachersTab.Text = "Teachers";

            // btnStudentsTab
            this.btnStudentsTab.BackColor = System.Drawing.Color.FromArgb(38, 101, 140);
            this.btnStudentsTab.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStudentsTab.FlatAppearance.BorderSize = 0;
            this.btnStudentsTab.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStudentsTab.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnStudentsTab.ForeColor = System.Drawing.Color.White;
            this.btnStudentsTab.Location = new System.Drawing.Point(750, 0);
            this.btnStudentsTab.Name = "btnStudentsTab";
            this.btnStudentsTab.Size = new System.Drawing.Size(250, 40);
            this.btnStudentsTab.TabIndex = 3;
            this.btnStudentsTab.Text = "Students";

            // =============================================
            // PROGRAMS TAB
            // =============================================
            this.pnlProgramsTab.BackColor = System.Drawing.Color.Transparent;
            this.pnlProgramsTab.Controls.Add(this.lblProgramsTitle);
            this.pnlProgramsTab.Controls.Add(this.dgvPrograms);
            this.pnlProgramsTab.Controls.Add(this.btnAddProgram);
            this.pnlProgramsTab.Controls.Add(this.btnEditProgram);
            this.pnlProgramsTab.Controls.Add(this.btnDeleteProgram);
            this.pnlProgramsTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlProgramsTab.Location = new System.Drawing.Point(0, 110);
            this.pnlProgramsTab.Name = "pnlProgramsTab";
            this.pnlProgramsTab.Size = new System.Drawing.Size(1000, 490);
            this.pnlProgramsTab.TabIndex = 2;

            // lblProgramsTitle
            this.lblProgramsTitle.AutoSize = true;
            this.lblProgramsTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblProgramsTitle.ForeColor = System.Drawing.Color.White;
            this.lblProgramsTitle.Location = new System.Drawing.Point(20, 15);
            this.lblProgramsTitle.Name = "lblProgramsTitle";
            this.lblProgramsTitle.Size = new System.Drawing.Size(186, 30);
            this.lblProgramsTitle.Text = "Manage Programs";

            // dgvPrograms
            this.dgvPrograms.BackgroundColor = System.Drawing.Color.FromArgb(1, 28, 64);
            this.dgvPrograms.Location = new System.Drawing.Point(25, 55);
            this.dgvPrograms.Name = "dgvPrograms";
            this.dgvPrograms.Size = new System.Drawing.Size(600, 400);

            // btnAddProgram
            this.btnAddProgram.Location = new System.Drawing.Point(650, 55);
            this.btnAddProgram.Name = "btnAddProgram";
            this.btnAddProgram.Size = new System.Drawing.Size(150, 40);
            this.btnAddProgram.TabIndex = 1;
            this.btnAddProgram.Text = "Add Program";
            this.btnAddProgram.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddProgram.FlatAppearance.BorderSize = 0;
            this.btnAddProgram.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddProgram.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnAddProgram.UseVisualStyleBackColor = false;

            // btnEditProgram
            this.btnEditProgram.Location = new System.Drawing.Point(650, 105);
            this.btnEditProgram.Name = "btnEditProgram";
            this.btnEditProgram.Size = new System.Drawing.Size(150, 40);
            this.btnEditProgram.TabIndex = 2;
            this.btnEditProgram.Text = "Edit Subjects";
            this.btnEditProgram.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditProgram.FlatAppearance.BorderSize = 0;
            this.btnEditProgram.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEditProgram.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnEditProgram.UseVisualStyleBackColor = false;

            // btnDeleteProgram
            this.btnDeleteProgram.Location = new System.Drawing.Point(650, 155);
            this.btnDeleteProgram.Name = "btnDeleteProgram";
            this.btnDeleteProgram.Size = new System.Drawing.Size(150, 40);
            this.btnDeleteProgram.TabIndex = 3;
            this.btnDeleteProgram.Text = "Delete Program";
            this.btnDeleteProgram.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteProgram.FlatAppearance.BorderSize = 0;
            this.btnDeleteProgram.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDeleteProgram.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnDeleteProgram.UseVisualStyleBackColor = false;

            // =============================================
            // SUBJECTS TAB
            // =============================================
            this.pnlSubjectsTab.BackColor = System.Drawing.Color.Transparent;
            this.pnlSubjectsTab.Controls.Add(this.lblSubjectsTitle);
            this.pnlSubjectsTab.Controls.Add(this.dgvAllSubjects);
            this.pnlSubjectsTab.Controls.Add(this.btnAddSubject);
            this.pnlSubjectsTab.Controls.Add(this.btnEditSubject);
            this.pnlSubjectsTab.Controls.Add(this.btnDeleteSubject);
            this.pnlSubjectsTab.Controls.Add(this.btnManageTeachers);
            this.pnlSubjectsTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSubjectsTab.Location = new System.Drawing.Point(0, 110);
            this.pnlSubjectsTab.Name = "pnlSubjectsTab";
            this.pnlSubjectsTab.Size = new System.Drawing.Size(1000, 490);
            this.pnlSubjectsTab.Visible = false;

            // lblSubjectsTitle
            this.lblSubjectsTitle.AutoSize = true;
            this.lblSubjectsTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblSubjectsTitle.ForeColor = System.Drawing.Color.White;
            this.lblSubjectsTitle.Location = new System.Drawing.Point(20, 15);
            this.lblSubjectsTitle.Name = "lblSubjectsTitle";
            this.lblSubjectsTitle.Size = new System.Drawing.Size(175, 30);
            this.lblSubjectsTitle.Text = "Manage Subjects";

            // dgvAllSubjects
            this.dgvAllSubjects.BackgroundColor = System.Drawing.Color.FromArgb(1, 28, 64);
            this.dgvAllSubjects.Location = new System.Drawing.Point(25, 55);
            this.dgvAllSubjects.Name = "dgvAllSubjects";
            this.dgvAllSubjects.Size = new System.Drawing.Size(600, 400);

            // btnAddSubject
            this.btnAddSubject.Location = new System.Drawing.Point(650, 55);
            this.btnAddSubject.Name = "btnAddSubject";
            this.btnAddSubject.Size = new System.Drawing.Size(150, 40);
            this.btnAddSubject.TabIndex = 1;
            this.btnAddSubject.Text = "Add Subject";
            this.btnAddSubject.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddSubject.FlatAppearance.BorderSize = 0;
            this.btnAddSubject.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddSubject.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnAddSubject.UseVisualStyleBackColor = false;

            // btnEditSubject
            this.btnEditSubject.Location = new System.Drawing.Point(650, 105);
            this.btnEditSubject.Name = "btnEditSubject";
            this.btnEditSubject.Size = new System.Drawing.Size(150, 40);
            this.btnEditSubject.TabIndex = 2;
            this.btnEditSubject.Text = "Edit Subject";
            this.btnEditSubject.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditSubject.FlatAppearance.BorderSize = 0;
            this.btnEditSubject.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEditSubject.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnEditSubject.UseVisualStyleBackColor = false;

            // btnDeleteSubject
            this.btnDeleteSubject.Location = new System.Drawing.Point(650, 155);
            this.btnDeleteSubject.Name = "btnDeleteSubject";
            this.btnDeleteSubject.Size = new System.Drawing.Size(150, 40);
            this.btnDeleteSubject.TabIndex = 3;
            this.btnDeleteSubject.Text = "Delete Subject";
            this.btnDeleteSubject.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteSubject.FlatAppearance.BorderSize = 0;
            this.btnDeleteSubject.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDeleteSubject.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnDeleteSubject.UseVisualStyleBackColor = false;

            // btnManageTeachers (Teacher List)
            this.btnManageTeachers.Location = new System.Drawing.Point(650, 205);
            this.btnManageTeachers.Name = "btnManageTeachers";
            this.btnManageTeachers.Size = new System.Drawing.Size(150, 40);
            this.btnManageTeachers.TabIndex = 4;
            this.btnManageTeachers.Text = "Teacher List";
            this.btnManageTeachers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnManageTeachers.FlatAppearance.BorderSize = 0;
            this.btnManageTeachers.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnManageTeachers.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnManageTeachers.UseVisualStyleBackColor = false;

            // =============================================
            // TEACHERS TAB
            // =============================================
            this.pnlTeachersTab.BackColor = System.Drawing.Color.Transparent;
            this.pnlTeachersTab.Controls.Add(this.lblTeachersTitle);
            this.pnlTeachersTab.Controls.Add(this.dgvTeachers);
            this.pnlTeachersTab.Controls.Add(this.btnCreateTeacher);
            this.pnlTeachersTab.Controls.Add(this.btnEditTeacher);
            this.pnlTeachersTab.Controls.Add(this.btnResetTeacherPassword);
            this.pnlTeachersTab.Controls.Add(this.btnDeleteTeacher);
            this.pnlTeachersTab.Controls.Add(this.btnEditCoverage);
            this.pnlTeachersTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTeachersTab.Location = new System.Drawing.Point(0, 110);
            this.pnlTeachersTab.Name = "pnlTeachersTab";
            this.pnlTeachersTab.Size = new System.Drawing.Size(1000, 490);
            this.pnlTeachersTab.Visible = false;

            // lblTeachersTitle
            this.lblTeachersTitle.AutoSize = true;
            this.lblTeachersTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTeachersTitle.ForeColor = System.Drawing.Color.White;
            this.lblTeachersTitle.Location = new System.Drawing.Point(20, 15);
            this.lblTeachersTitle.Name = "lblTeachersTitle";
            this.lblTeachersTitle.Size = new System.Drawing.Size(173, 30);
            this.lblTeachersTitle.Text = "Manage Teachers";

            // dgvTeachers
            this.dgvTeachers.BackgroundColor = System.Drawing.Color.FromArgb(1, 28, 64);
            this.dgvTeachers.Location = new System.Drawing.Point(25, 55);
            this.dgvTeachers.Name = "dgvTeachers";
            this.dgvTeachers.Size = new System.Drawing.Size(600, 400);

            // btnCreateTeacher
            this.btnCreateTeacher.Location = new System.Drawing.Point(650, 55);
            this.btnCreateTeacher.Name = "btnCreateTeacher";
            this.btnCreateTeacher.Size = new System.Drawing.Size(160, 40);
            this.btnCreateTeacher.TabIndex = 1;
            this.btnCreateTeacher.Text = "Create Teacher";
            this.btnCreateTeacher.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCreateTeacher.FlatAppearance.BorderSize = 0;
            this.btnCreateTeacher.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCreateTeacher.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCreateTeacher.UseVisualStyleBackColor = false;

            // btnEditTeacher
            this.btnEditTeacher.Location = new System.Drawing.Point(650, 105);
            this.btnEditTeacher.Name = "btnEditTeacher";
            this.btnEditTeacher.Size = new System.Drawing.Size(160, 40);
            this.btnEditTeacher.TabIndex = 2;
            this.btnEditTeacher.Text = "Edit Teacher";
            this.btnEditTeacher.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditTeacher.FlatAppearance.BorderSize = 0;
            this.btnEditTeacher.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEditTeacher.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnEditTeacher.UseVisualStyleBackColor = false;

            // btnResetTeacherPassword
            this.btnResetTeacherPassword.Location = new System.Drawing.Point(650, 155);
            this.btnResetTeacherPassword.Name = "btnResetTeacherPassword";
            this.btnResetTeacherPassword.Size = new System.Drawing.Size(160, 40);
            this.btnResetTeacherPassword.TabIndex = 3;
            this.btnResetTeacherPassword.Text = "Reset Password";
            this.btnResetTeacherPassword.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnResetTeacherPassword.FlatAppearance.BorderSize = 0;
            this.btnResetTeacherPassword.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnResetTeacherPassword.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnResetTeacherPassword.UseVisualStyleBackColor = false;

            // btnDeleteTeacher
            this.btnDeleteTeacher.Location = new System.Drawing.Point(650, 205);
            this.btnDeleteTeacher.Name = "btnDeleteTeacher";
            this.btnDeleteTeacher.Size = new System.Drawing.Size(160, 40);
            this.btnDeleteTeacher.TabIndex = 4;
            this.btnDeleteTeacher.Text = "Delete Teacher";
            this.btnDeleteTeacher.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteTeacher.FlatAppearance.BorderSize = 0;
            this.btnDeleteTeacher.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDeleteTeacher.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnDeleteTeacher.UseVisualStyleBackColor = false;

            // btnEditCoverage
            this.btnEditCoverage.Location = new System.Drawing.Point(650, 255);
            this.btnEditCoverage.Name = "btnEditCoverage";
            this.btnEditCoverage.Size = new System.Drawing.Size(160, 40);
            this.btnEditCoverage.TabIndex = 5;
            this.btnEditCoverage.Text = "Edit Coverage";
            this.btnEditCoverage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditCoverage.FlatAppearance.BorderSize = 0;
            this.btnEditCoverage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEditCoverage.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnEditCoverage.UseVisualStyleBackColor = false;

            // =============================================
            // STUDENTS TAB
            // =============================================
            this.pnlStudentsTab.BackColor = System.Drawing.Color.Transparent;
            this.pnlStudentsTab.Controls.Add(this.lblStudentsTitle);
            this.pnlStudentsTab.Controls.Add(this.dgvStudents);
            this.pnlStudentsTab.Controls.Add(this.btnCreateStudent);
            this.pnlStudentsTab.Controls.Add(this.btnEditStudent);
            this.pnlStudentsTab.Controls.Add(this.btnResetStudentPassword);
            this.pnlStudentsTab.Controls.Add(this.btnDeleteStudent);
            this.pnlStudentsTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlStudentsTab.Location = new System.Drawing.Point(0, 110);
            this.pnlStudentsTab.Name = "pnlStudentsTab";
            this.pnlStudentsTab.Size = new System.Drawing.Size(1000, 490);
            this.pnlStudentsTab.Visible = false;

            // lblStudentsTitle
            this.lblStudentsTitle.AutoSize = true;
            this.lblStudentsTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblStudentsTitle.ForeColor = System.Drawing.Color.White;
            this.lblStudentsTitle.Location = new System.Drawing.Point(20, 15);
            this.lblStudentsTitle.Name = "lblStudentsTitle";
            this.lblStudentsTitle.Size = new System.Drawing.Size(172, 30);
            this.lblStudentsTitle.Text = "Manage Students";

            // dgvStudents
            this.dgvStudents.BackgroundColor = System.Drawing.Color.FromArgb(1, 28, 64);
            this.dgvStudents.Location = new System.Drawing.Point(25, 55);
            this.dgvStudents.Name = "dgvStudents";
            this.dgvStudents.Size = new System.Drawing.Size(600, 400);

            // btnCreateStudent
            this.btnCreateStudent.Location = new System.Drawing.Point(650, 55);
            this.btnCreateStudent.Name = "btnCreateStudent";
            this.btnCreateStudent.Size = new System.Drawing.Size(160, 40);
            this.btnCreateStudent.TabIndex = 1;
            this.btnCreateStudent.Text = "Create Student";
            this.btnCreateStudent.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCreateStudent.FlatAppearance.BorderSize = 0;
            this.btnCreateStudent.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCreateStudent.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCreateStudent.UseVisualStyleBackColor = false;

            // btnEditStudent
            this.btnEditStudent.Location = new System.Drawing.Point(650, 105);
            this.btnEditStudent.Name = "btnEditStudent";
            this.btnEditStudent.Size = new System.Drawing.Size(160, 40);
            this.btnEditStudent.TabIndex = 2;
            this.btnEditStudent.Text = "Edit Student";
            this.btnEditStudent.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditStudent.FlatAppearance.BorderSize = 0;
            this.btnEditStudent.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEditStudent.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnEditStudent.UseVisualStyleBackColor = false;

            // btnResetStudentPassword
            this.btnResetStudentPassword.Location = new System.Drawing.Point(650, 155);
            this.btnResetStudentPassword.Name = "btnResetStudentPassword";
            this.btnResetStudentPassword.Size = new System.Drawing.Size(160, 40);
            this.btnResetStudentPassword.TabIndex = 3;
            this.btnResetStudentPassword.Text = "Reset Password";
            this.btnResetStudentPassword.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnResetStudentPassword.FlatAppearance.BorderSize = 0;
            this.btnResetStudentPassword.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnResetStudentPassword.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnResetStudentPassword.UseVisualStyleBackColor = false;

            // btnDeleteStudent
            this.btnDeleteStudent.Location = new System.Drawing.Point(650, 205);
            this.btnDeleteStudent.Name = "btnDeleteStudent";
            this.btnDeleteStudent.Size = new System.Drawing.Size(160, 40);
            this.btnDeleteStudent.TabIndex = 4;
            this.btnDeleteStudent.Text = "Delete Student";
            this.btnDeleteStudent.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteStudent.FlatAppearance.BorderSize = 0;
            this.btnDeleteStudent.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDeleteStudent.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnDeleteStudent.UseVisualStyleBackColor = false;

            // =============================================
            // SuperAdminForm
            // =============================================
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(32, 34, 37);
            this.ClientSize = new System.Drawing.Size(1000, 600);
            this.Controls.Add(this.pnlProgramsTab);
            this.Controls.Add(this.pnlSubjectsTab);
            this.Controls.Add(this.pnlTeachersTab);
            this.Controls.Add(this.pnlStudentsTab);
            this.Controls.Add(this.pnlTabNav);
            this.Controls.Add(this.pnlTopBar);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SuperAdminForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Super Admin Panel";

            // =============================================
            // RESUME LAYOUT
            // =============================================
            this.pnlTopBar.ResumeLayout(false);
            this.pnlTopBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).EndInit();
            this.pnlTabNav.ResumeLayout(false);
            this.pnlProgramsTab.ResumeLayout(false);
            this.pnlProgramsTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPrograms)).EndInit();
            this.pnlSubjectsTab.ResumeLayout(false);
            this.pnlSubjectsTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAllSubjects)).EndInit();
            this.pnlTeachersTab.ResumeLayout(false);
            this.pnlTeachersTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTeachers)).EndInit();
            this.pnlStudentsTab.ResumeLayout(false);
            this.pnlStudentsTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStudents)).EndInit();
            this.ResumeLayout(false);
        }
    }
}