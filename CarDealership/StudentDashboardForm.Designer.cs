namespace CarDealership
{
    partial class StudentDashboardForm
    {
        private System.ComponentModel.IContainer components = null;

        // Top bar
        private System.Windows.Forms.Panel pnlTopBar;
        private System.Windows.Forms.PictureBox pbLogo;
        private System.Windows.Forms.Button btnLogout;

        // Subject Panel (Step 1)
        private System.Windows.Forms.Panel pnlSubjects;
        private System.Windows.Forms.Label lblSubjectTitle;
        private System.Windows.Forms.Label lblSubjectOverallStatus;
        private LunaProgressBar progressSubjects;
        private System.Windows.Forms.Panel pnlSubject1;
        private System.Windows.Forms.Label lblSubject1;
        private System.Windows.Forms.Button btnSubject1Submit;
        private System.Windows.Forms.Label lblSubject1Status;
        private System.Windows.Forms.Panel pnlSubject2;
        private System.Windows.Forms.Label lblSubject2;
        private System.Windows.Forms.Button btnSubject2Submit;
        private System.Windows.Forms.Label lblSubject2Status;
        private System.Windows.Forms.Panel pnlSubject3;
        private System.Windows.Forms.Label lblSubject3;
        private System.Windows.Forms.Button btnSubject3Submit;
        private System.Windows.Forms.Label lblSubject3Status;
        private System.Windows.Forms.Panel pnlSubject4;
        private System.Windows.Forms.Label lblSubject4;
        private System.Windows.Forms.Button btnSubject4Submit;
        private System.Windows.Forms.Label lblSubject4Status;
        private System.Windows.Forms.Panel pnlSubject5;
        private System.Windows.Forms.Label lblSubject5;
        private System.Windows.Forms.Button btnSubject5Submit;
        private System.Windows.Forms.Label lblSubject5Status;
        private System.Windows.Forms.Panel pnlSubject6;
        private System.Windows.Forms.Label lblSubject6;
        private System.Windows.Forms.Button btnSubject6Submit;
        private System.Windows.Forms.Label lblSubject6Status;

        // Department Panel (Step 2)
        private System.Windows.Forms.Panel pnlDepartments;
        private System.Windows.Forms.Label lblDeptTitle;
        private System.Windows.Forms.Label lblDeptOverallStatus;
        private LunaProgressBar progressDepts;
        private System.Windows.Forms.Panel pnlLibrary;
        private System.Windows.Forms.Label lblLibrary;
        private System.Windows.Forms.Button btnLibrarySubmit;
        private System.Windows.Forms.Label lblLibraryStatus;
        private System.Windows.Forms.Panel pnlSAO;
        private System.Windows.Forms.Label lblSAO;
        private System.Windows.Forms.Button btnSAOSubmit;
        private System.Windows.Forms.Label lblSAOStatus;
        private System.Windows.Forms.Panel pnlCashier;
        private System.Windows.Forms.Label lblCashier;
        private System.Windows.Forms.Button btnCashierSubmit;
        private System.Windows.Forms.Label lblCashierStatus;
        private System.Windows.Forms.Panel pnlAccounting;
        private System.Windows.Forms.Label lblAccounting;
        private System.Windows.Forms.Button btnAccountingSubmit;
        private System.Windows.Forms.Label lblAccountingStatus;
        private System.Windows.Forms.Panel pnlDean;
        private System.Windows.Forms.Label lblDean;
        private System.Windows.Forms.Button btnDeanSubmit;
        private System.Windows.Forms.Label lblDeanStatus;
        private System.Windows.Forms.Panel pnlRecords;
        private System.Windows.Forms.Label lblRecords;
        private System.Windows.Forms.Button btnRecordsSubmit;
        private System.Windows.Forms.Label lblRecordsStatus;

        // Certificate Panel (Step 3) — SIMPLIFIED
        private System.Windows.Forms.Panel pnlCertificate;
        private System.Windows.Forms.Label lblCertTitle;
        private System.Windows.Forms.Label lblCertStudentID;
        private System.Windows.Forms.Label lblCertProgram;
        private System.Windows.Forms.Label lblCertDate;
        private System.Windows.Forms.Label lblCertSubjectsCheck;
        private System.Windows.Forms.Label lblCertDeptsCheck;
        private System.Windows.Forms.Button btnDownloadCert;

        // Step Navigation
        private System.Windows.Forms.Panel pnlStepNav;
        private System.Windows.Forms.Button btnStep1;
        private System.Windows.Forms.Button btnStep2;
        private System.Windows.Forms.Button btnStep3;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                _isCleaningUp = true;

                if (_hubConnection != null)
                {
                    _hubConnection.Closed -= OnConnectionClosed;
                }

                if (_subjectRefreshTimer != null)
                {
                    _subjectRefreshTimer.Stop();
                    _subjectRefreshTimer.Dispose();
                    _subjectRefreshTimer = null;
                }

                if (_deptRefreshTimer != null)
                {
                    _deptRefreshTimer.Stop();
                    _deptRefreshTimer.Dispose();
                    _deptRefreshTimer = null;
                }

                if (_notificationTimer != null)
                {
                    _notificationTimer.Stop();
                    _notificationTimer.Dispose();
                    _notificationTimer = null;
                }

                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlTopBar = new System.Windows.Forms.Panel();
            this.pbLogo = new System.Windows.Forms.PictureBox();
            this.btnLogout = new System.Windows.Forms.Button();

            // Subject Panel
            this.pnlSubjects = new System.Windows.Forms.Panel();
            this.lblSubjectTitle = new System.Windows.Forms.Label();
            this.lblSubjectOverallStatus = new System.Windows.Forms.Label();
            this.progressSubjects = new LunaProgressBar();
            this.pnlSubject1 = new System.Windows.Forms.Panel();
            this.lblSubject1 = new System.Windows.Forms.Label();
            this.btnSubject1Submit = new System.Windows.Forms.Button();
            this.lblSubject1Status = new System.Windows.Forms.Label();
            this.pnlSubject2 = new System.Windows.Forms.Panel();
            this.lblSubject2 = new System.Windows.Forms.Label();
            this.btnSubject2Submit = new System.Windows.Forms.Button();
            this.lblSubject2Status = new System.Windows.Forms.Label();
            this.pnlSubject3 = new System.Windows.Forms.Panel();
            this.lblSubject3 = new System.Windows.Forms.Label();
            this.btnSubject3Submit = new System.Windows.Forms.Button();
            this.lblSubject3Status = new System.Windows.Forms.Label();
            this.pnlSubject4 = new System.Windows.Forms.Panel();
            this.lblSubject4 = new System.Windows.Forms.Label();
            this.btnSubject4Submit = new System.Windows.Forms.Button();
            this.lblSubject4Status = new System.Windows.Forms.Label();
            this.pnlSubject5 = new System.Windows.Forms.Panel();
            this.lblSubject5 = new System.Windows.Forms.Label();
            this.btnSubject5Submit = new System.Windows.Forms.Button();
            this.lblSubject5Status = new System.Windows.Forms.Label();
            this.pnlSubject6 = new System.Windows.Forms.Panel();
            this.lblSubject6 = new System.Windows.Forms.Label();
            this.btnSubject6Submit = new System.Windows.Forms.Button();
            this.lblSubject6Status = new System.Windows.Forms.Label();

            // Department Panel
            this.pnlDepartments = new System.Windows.Forms.Panel();
            this.lblDeptTitle = new System.Windows.Forms.Label();
            this.lblDeptOverallStatus = new System.Windows.Forms.Label();
            this.progressDepts = new LunaProgressBar();
            this.pnlLibrary = new System.Windows.Forms.Panel();
            this.lblLibrary = new System.Windows.Forms.Label();
            this.btnLibrarySubmit = new System.Windows.Forms.Button();
            this.lblLibraryStatus = new System.Windows.Forms.Label();
            this.pnlSAO = new System.Windows.Forms.Panel();
            this.lblSAO = new System.Windows.Forms.Label();
            this.btnSAOSubmit = new System.Windows.Forms.Button();
            this.lblSAOStatus = new System.Windows.Forms.Label();
            this.pnlCashier = new System.Windows.Forms.Panel();
            this.lblCashier = new System.Windows.Forms.Label();
            this.btnCashierSubmit = new System.Windows.Forms.Button();
            this.lblCashierStatus = new System.Windows.Forms.Label();
            this.pnlAccounting = new System.Windows.Forms.Panel();
            this.lblAccounting = new System.Windows.Forms.Label();
            this.btnAccountingSubmit = new System.Windows.Forms.Button();
            this.lblAccountingStatus = new System.Windows.Forms.Label();
            this.pnlDean = new System.Windows.Forms.Panel();
            this.lblDean = new System.Windows.Forms.Label();
            this.btnDeanSubmit = new System.Windows.Forms.Button();
            this.lblDeanStatus = new System.Windows.Forms.Label();
            this.pnlRecords = new System.Windows.Forms.Panel();
            this.lblRecords = new System.Windows.Forms.Label();
            this.btnRecordsSubmit = new System.Windows.Forms.Button();
            this.lblRecordsStatus = new System.Windows.Forms.Label();

            // Certificate Panel
            this.pnlCertificate = new System.Windows.Forms.Panel();
            this.lblCertTitle = new System.Windows.Forms.Label();
            this.lblCertStudentID = new System.Windows.Forms.Label();
            this.lblCertProgram = new System.Windows.Forms.Label();
            this.lblCertDate = new System.Windows.Forms.Label();
            this.lblCertSubjectsCheck = new System.Windows.Forms.Label();
            this.lblCertDeptsCheck = new System.Windows.Forms.Label();
            this.btnDownloadCert = new System.Windows.Forms.Button();

            // Step Navigation
            this.pnlStepNav = new System.Windows.Forms.Panel();
            this.btnStep1 = new System.Windows.Forms.Button();
            this.btnStep2 = new System.Windows.Forms.Button();
            this.btnStep3 = new System.Windows.Forms.Button();

            this.pnlTopBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
            this.pnlSubjects.SuspendLayout();
            this.pnlSubject1.SuspendLayout();
            this.pnlSubject2.SuspendLayout();
            this.pnlSubject3.SuspendLayout();
            this.pnlSubject4.SuspendLayout();
            this.pnlSubject5.SuspendLayout();
            this.pnlSubject6.SuspendLayout();
            this.pnlDepartments.SuspendLayout();
            this.pnlLibrary.SuspendLayout();
            this.pnlSAO.SuspendLayout();
            this.pnlCashier.SuspendLayout();
            this.pnlAccounting.SuspendLayout();
            this.pnlDean.SuspendLayout();
            this.pnlRecords.SuspendLayout();
            this.pnlCertificate.SuspendLayout();
            this.pnlStepNav.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTopBar
            // 
            this.pnlTopBar.BackColor = System.Drawing.Color.FromArgb(47, 49, 54);
            this.pnlTopBar.Controls.Add(this.pbLogo);
            this.pnlTopBar.Controls.Add(this.btnLogout);
            this.pnlTopBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTopBar.Location = new System.Drawing.Point(0, 0);
            this.pnlTopBar.Name = "pnlTopBar";
            this.pnlTopBar.Size = new System.Drawing.Size(900, 70);
            this.pnlTopBar.TabIndex = 0;
            // 
            // pbLogo
            // 
            this.pbLogo.BackColor = System.Drawing.Color.Transparent;
            this.pbLogo.Location = new System.Drawing.Point(400, 10);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.Size = new System.Drawing.Size(100, 50);
            this.pbLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbLogo.TabIndex = 1;
            this.pbLogo.TabStop = false;
            // 
            // btnLogout
            // 
            this.btnLogout.BackColor = System.Drawing.Color.FromArgb(38, 101, 140);
            this.btnLogout.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogout.FlatAppearance.BorderSize = 0;
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnLogout.ForeColor = System.Drawing.Color.White;
            this.btnLogout.Location = new System.Drawing.Point(780, 17);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(100, 35);
            this.btnLogout.TabIndex = 2;
            this.btnLogout.Text = "Logout";
            this.btnLogout.UseVisualStyleBackColor = false;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);

            // ===================================================
            // SUBJECT PANEL
            // ===================================================
            this.pnlSubjects.BackColor = System.Drawing.Color.Transparent;
            this.pnlSubjects.Controls.Add(this.lblSubjectTitle);
            this.pnlSubjects.Controls.Add(this.lblSubjectOverallStatus);
            this.pnlSubjects.Controls.Add(this.progressSubjects);
            this.pnlSubjects.Controls.Add(this.pnlSubject1);
            this.pnlSubjects.Controls.Add(this.pnlSubject2);
            this.pnlSubjects.Controls.Add(this.pnlSubject3);
            this.pnlSubjects.Controls.Add(this.pnlSubject4);
            this.pnlSubjects.Controls.Add(this.pnlSubject5);
            this.pnlSubjects.Controls.Add(this.pnlSubject6);
            this.pnlSubjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSubjects.Location = new System.Drawing.Point(0, 70);
            this.pnlSubjects.Name = "pnlSubjects";
            this.pnlSubjects.Padding = new System.Windows.Forms.Padding(20, 20, 20, 10);
            this.pnlSubjects.Size = new System.Drawing.Size(900, 510);
            this.pnlSubjects.TabIndex = 1;
            // 
            // lblSubjectTitle
            // 
            this.lblSubjectTitle.AutoSize = true;
            this.lblSubjectTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblSubjectTitle.ForeColor = System.Drawing.Color.White;
            this.lblSubjectTitle.Location = new System.Drawing.Point(20, 15);
            this.lblSubjectTitle.Name = "lblSubjectTitle";
            this.lblSubjectTitle.Size = new System.Drawing.Size(220, 32);
            this.lblSubjectTitle.TabIndex = 0;
            this.lblSubjectTitle.Text = "Subject Clearance";
            // 
            // lblSubjectOverallStatus
            // 
            this.lblSubjectOverallStatus.AutoSize = true;
            this.lblSubjectOverallStatus.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblSubjectOverallStatus.ForeColor = System.Drawing.Color.FromArgb(84, 172, 191);
            this.lblSubjectOverallStatus.Location = new System.Drawing.Point(22, 55);
            this.lblSubjectOverallStatus.Name = "lblSubjectOverallStatus";
            this.lblSubjectOverallStatus.Size = new System.Drawing.Size(156, 20);
            this.lblSubjectOverallStatus.TabIndex = 1;
            this.lblSubjectOverallStatus.Text = "Overall Progress: 0/6";
            // 
            // progressSubjects
            // 
            this.progressSubjects.Location = new System.Drawing.Point(25, 80);
            this.progressSubjects.Name = "progressSubjects";
            this.progressSubjects.Size = new System.Drawing.Size(850, 25);
            this.progressSubjects.TabIndex = 2;

            // Subject cards 1-6 (unchanged)
            this.pnlSubject1.BackColor = System.Drawing.Color.FromArgb(38, 101, 140);
            this.pnlSubject1.Controls.Add(this.lblSubject1);
            this.pnlSubject1.Controls.Add(this.btnSubject1Submit);
            this.pnlSubject1.Controls.Add(this.lblSubject1Status);
            this.pnlSubject1.Location = new System.Drawing.Point(25, 120);
            this.pnlSubject1.Name = "pnlSubject1";
            this.pnlSubject1.Size = new System.Drawing.Size(415, 95);
            this.pnlSubject1.TabIndex = 3;
            this.lblSubject1.AutoSize = true;
            this.lblSubject1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblSubject1.ForeColor = System.Drawing.Color.White;
            this.lblSubject1.Location = new System.Drawing.Point(20, 15);
            this.lblSubject1.Name = "lblSubject1";
            this.lblSubject1.Size = new System.Drawing.Size(85, 21);
            this.lblSubject1.TabIndex = 1;
            this.lblSubject1.Text = "Subject 1";
            this.btnSubject1Submit.BackColor = System.Drawing.Color.FromArgb(38, 101, 140);
            this.btnSubject1Submit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSubject1Submit.FlatAppearance.BorderSize = 0;
            this.btnSubject1Submit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSubject1Submit.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSubject1Submit.ForeColor = System.Drawing.Color.White;
            this.btnSubject1Submit.Location = new System.Drawing.Point(290, 28);
            this.btnSubject1Submit.Name = "btnSubject1Submit";
            this.btnSubject1Submit.Size = new System.Drawing.Size(110, 40);
            this.btnSubject1Submit.TabIndex = 3;
            this.btnSubject1Submit.Text = "Submit";
            this.btnSubject1Submit.UseVisualStyleBackColor = false;
            this.lblSubject1Status.AutoSize = true;
            this.lblSubject1Status.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblSubject1Status.ForeColor = System.Drawing.Color.FromArgb(84, 172, 191);
            this.lblSubject1Status.Location = new System.Drawing.Point(20, 45);
            this.lblSubject1Status.Name = "lblSubject1Status";
            this.lblSubject1Status.Size = new System.Drawing.Size(64, 19);
            this.lblSubject1Status.TabIndex = 2;
            this.lblSubject1Status.Text = "Pending";

            this.pnlSubject2.BackColor = System.Drawing.Color.FromArgb(38, 101, 140);
            this.pnlSubject2.Controls.Add(this.lblSubject2);
            this.pnlSubject2.Controls.Add(this.btnSubject2Submit);
            this.pnlSubject2.Controls.Add(this.lblSubject2Status);
            this.pnlSubject2.Location = new System.Drawing.Point(460, 120);
            this.pnlSubject2.Name = "pnlSubject2";
            this.pnlSubject2.Size = new System.Drawing.Size(415, 95);
            this.pnlSubject2.TabIndex = 4;
            this.lblSubject2.AutoSize = true;
            this.lblSubject2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblSubject2.ForeColor = System.Drawing.Color.White;
            this.lblSubject2.Location = new System.Drawing.Point(20, 15);
            this.lblSubject2.Name = "lblSubject2";
            this.lblSubject2.Size = new System.Drawing.Size(85, 21);
            this.lblSubject2.TabIndex = 1;
            this.lblSubject2.Text = "Subject 2";
            this.btnSubject2Submit.BackColor = System.Drawing.Color.FromArgb(38, 101, 140);
            this.btnSubject2Submit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSubject2Submit.FlatAppearance.BorderSize = 0;
            this.btnSubject2Submit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSubject2Submit.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSubject2Submit.ForeColor = System.Drawing.Color.White;
            this.btnSubject2Submit.Location = new System.Drawing.Point(290, 28);
            this.btnSubject2Submit.Name = "btnSubject2Submit";
            this.btnSubject2Submit.Size = new System.Drawing.Size(110, 40);
            this.btnSubject2Submit.TabIndex = 3;
            this.btnSubject2Submit.Text = "Submit";
            this.btnSubject2Submit.UseVisualStyleBackColor = false;
            this.lblSubject2Status.AutoSize = true;
            this.lblSubject2Status.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblSubject2Status.ForeColor = System.Drawing.Color.FromArgb(84, 172, 191);
            this.lblSubject2Status.Location = new System.Drawing.Point(20, 45);
            this.lblSubject2Status.Name = "lblSubject2Status";
            this.lblSubject2Status.Size = new System.Drawing.Size(64, 19);
            this.lblSubject2Status.TabIndex = 2;
            this.lblSubject2Status.Text = "Pending";

            this.pnlSubject3.BackColor = System.Drawing.Color.FromArgb(38, 101, 140);
            this.pnlSubject3.Controls.Add(this.lblSubject3);
            this.pnlSubject3.Controls.Add(this.btnSubject3Submit);
            this.pnlSubject3.Controls.Add(this.lblSubject3Status);
            this.pnlSubject3.Location = new System.Drawing.Point(25, 235);
            this.pnlSubject3.Name = "pnlSubject3";
            this.pnlSubject3.Size = new System.Drawing.Size(415, 95);
            this.pnlSubject3.TabIndex = 5;
            this.lblSubject3.AutoSize = true;
            this.lblSubject3.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblSubject3.ForeColor = System.Drawing.Color.White;
            this.lblSubject3.Location = new System.Drawing.Point(20, 15);
            this.lblSubject3.Name = "lblSubject3";
            this.lblSubject3.Size = new System.Drawing.Size(85, 21);
            this.lblSubject3.TabIndex = 1;
            this.lblSubject3.Text = "Subject 3";
            this.btnSubject3Submit.BackColor = System.Drawing.Color.FromArgb(38, 101, 140);
            this.btnSubject3Submit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSubject3Submit.FlatAppearance.BorderSize = 0;
            this.btnSubject3Submit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSubject3Submit.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSubject3Submit.ForeColor = System.Drawing.Color.White;
            this.btnSubject3Submit.Location = new System.Drawing.Point(290, 28);
            this.btnSubject3Submit.Name = "btnSubject3Submit";
            this.btnSubject3Submit.Size = new System.Drawing.Size(110, 40);
            this.btnSubject3Submit.TabIndex = 3;
            this.btnSubject3Submit.Text = "Submit";
            this.btnSubject3Submit.UseVisualStyleBackColor = false;
            this.lblSubject3Status.AutoSize = true;
            this.lblSubject3Status.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblSubject3Status.ForeColor = System.Drawing.Color.FromArgb(84, 172, 191);
            this.lblSubject3Status.Location = new System.Drawing.Point(20, 45);
            this.lblSubject3Status.Name = "lblSubject3Status";
            this.lblSubject3Status.Size = new System.Drawing.Size(64, 19);
            this.lblSubject3Status.TabIndex = 2;
            this.lblSubject3Status.Text = "Pending";

            this.pnlSubject4.BackColor = System.Drawing.Color.FromArgb(38, 101, 140);
            this.pnlSubject4.Controls.Add(this.lblSubject4);
            this.pnlSubject4.Controls.Add(this.btnSubject4Submit);
            this.pnlSubject4.Controls.Add(this.lblSubject4Status);
            this.pnlSubject4.Location = new System.Drawing.Point(460, 235);
            this.pnlSubject4.Name = "pnlSubject4";
            this.pnlSubject4.Size = new System.Drawing.Size(415, 95);
            this.pnlSubject4.TabIndex = 6;
            this.lblSubject4.AutoSize = true;
            this.lblSubject4.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblSubject4.ForeColor = System.Drawing.Color.White;
            this.lblSubject4.Location = new System.Drawing.Point(20, 15);
            this.lblSubject4.Name = "lblSubject4";
            this.lblSubject4.Size = new System.Drawing.Size(85, 21);
            this.lblSubject4.TabIndex = 1;
            this.lblSubject4.Text = "Subject 4";
            this.btnSubject4Submit.BackColor = System.Drawing.Color.FromArgb(38, 101, 140);
            this.btnSubject4Submit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSubject4Submit.FlatAppearance.BorderSize = 0;
            this.btnSubject4Submit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSubject4Submit.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSubject4Submit.ForeColor = System.Drawing.Color.White;
            this.btnSubject4Submit.Location = new System.Drawing.Point(290, 28);
            this.btnSubject4Submit.Name = "btnSubject4Submit";
            this.btnSubject4Submit.Size = new System.Drawing.Size(110, 40);
            this.btnSubject4Submit.TabIndex = 3;
            this.btnSubject4Submit.Text = "Submit";
            this.btnSubject4Submit.UseVisualStyleBackColor = false;
            this.lblSubject4Status.AutoSize = true;
            this.lblSubject4Status.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblSubject4Status.ForeColor = System.Drawing.Color.FromArgb(84, 172, 191);
            this.lblSubject4Status.Location = new System.Drawing.Point(20, 45);
            this.lblSubject4Status.Name = "lblSubject4Status";
            this.lblSubject4Status.Size = new System.Drawing.Size(64, 19);
            this.lblSubject4Status.TabIndex = 2;
            this.lblSubject4Status.Text = "Pending";

            this.pnlSubject5.BackColor = System.Drawing.Color.FromArgb(38, 101, 140);
            this.pnlSubject5.Controls.Add(this.lblSubject5);
            this.pnlSubject5.Controls.Add(this.btnSubject5Submit);
            this.pnlSubject5.Controls.Add(this.lblSubject5Status);
            this.pnlSubject5.Location = new System.Drawing.Point(25, 350);
            this.pnlSubject5.Name = "pnlSubject5";
            this.pnlSubject5.Size = new System.Drawing.Size(415, 95);
            this.pnlSubject5.TabIndex = 7;
            this.lblSubject5.AutoSize = true;
            this.lblSubject5.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblSubject5.ForeColor = System.Drawing.Color.White;
            this.lblSubject5.Location = new System.Drawing.Point(20, 15);
            this.lblSubject5.Name = "lblSubject5";
            this.lblSubject5.Size = new System.Drawing.Size(85, 21);
            this.lblSubject5.TabIndex = 1;
            this.lblSubject5.Text = "Subject 5";
            this.btnSubject5Submit.BackColor = System.Drawing.Color.FromArgb(38, 101, 140);
            this.btnSubject5Submit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSubject5Submit.FlatAppearance.BorderSize = 0;
            this.btnSubject5Submit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSubject5Submit.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSubject5Submit.ForeColor = System.Drawing.Color.White;
            this.btnSubject5Submit.Location = new System.Drawing.Point(290, 28);
            this.btnSubject5Submit.Name = "btnSubject5Submit";
            this.btnSubject5Submit.Size = new System.Drawing.Size(110, 40);
            this.btnSubject5Submit.TabIndex = 3;
            this.btnSubject5Submit.Text = "Submit";
            this.btnSubject5Submit.UseVisualStyleBackColor = false;
            this.lblSubject5Status.AutoSize = true;
            this.lblSubject5Status.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblSubject5Status.ForeColor = System.Drawing.Color.FromArgb(84, 172, 191);
            this.lblSubject5Status.Location = new System.Drawing.Point(20, 45);
            this.lblSubject5Status.Name = "lblSubject5Status";
            this.lblSubject5Status.Size = new System.Drawing.Size(64, 19);
            this.lblSubject5Status.TabIndex = 2;
            this.lblSubject5Status.Text = "Pending";

            this.pnlSubject6.BackColor = System.Drawing.Color.FromArgb(38, 101, 140);
            this.pnlSubject6.Controls.Add(this.lblSubject6);
            this.pnlSubject6.Controls.Add(this.btnSubject6Submit);
            this.pnlSubject6.Controls.Add(this.lblSubject6Status);
            this.pnlSubject6.Location = new System.Drawing.Point(460, 350);
            this.pnlSubject6.Name = "pnlSubject6";
            this.pnlSubject6.Size = new System.Drawing.Size(415, 95);
            this.pnlSubject6.TabIndex = 8;
            this.lblSubject6.AutoSize = true;
            this.lblSubject6.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblSubject6.ForeColor = System.Drawing.Color.White;
            this.lblSubject6.Location = new System.Drawing.Point(20, 15);
            this.lblSubject6.Name = "lblSubject6";
            this.lblSubject6.Size = new System.Drawing.Size(85, 21);
            this.lblSubject6.TabIndex = 1;
            this.lblSubject6.Text = "Subject 6";
            this.btnSubject6Submit.BackColor = System.Drawing.Color.FromArgb(38, 101, 140);
            this.btnSubject6Submit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSubject6Submit.FlatAppearance.BorderSize = 0;
            this.btnSubject6Submit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSubject6Submit.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSubject6Submit.ForeColor = System.Drawing.Color.White;
            this.btnSubject6Submit.Location = new System.Drawing.Point(290, 28);
            this.btnSubject6Submit.Name = "btnSubject6Submit";
            this.btnSubject6Submit.Size = new System.Drawing.Size(110, 40);
            this.btnSubject6Submit.TabIndex = 3;
            this.btnSubject6Submit.Text = "Submit";
            this.btnSubject6Submit.UseVisualStyleBackColor = false;
            this.lblSubject6Status.AutoSize = true;
            this.lblSubject6Status.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblSubject6Status.ForeColor = System.Drawing.Color.FromArgb(84, 172, 191);
            this.lblSubject6Status.Location = new System.Drawing.Point(20, 45);
            this.lblSubject6Status.Name = "lblSubject6Status";
            this.lblSubject6Status.Size = new System.Drawing.Size(64, 19);
            this.lblSubject6Status.TabIndex = 2;
            this.lblSubject6Status.Text = "Pending";

            // ===================================================
            // DEPARTMENT PANEL
            // ===================================================
            this.pnlDepartments.BackColor = System.Drawing.Color.Transparent;
            this.pnlDepartments.Controls.Add(this.lblDeptTitle);
            this.pnlDepartments.Controls.Add(this.lblDeptOverallStatus);
            this.pnlDepartments.Controls.Add(this.progressDepts);
            this.pnlDepartments.Controls.Add(this.pnlLibrary);
            this.pnlDepartments.Controls.Add(this.pnlSAO);
            this.pnlDepartments.Controls.Add(this.pnlCashier);
            this.pnlDepartments.Controls.Add(this.pnlAccounting);
            this.pnlDepartments.Controls.Add(this.pnlDean);
            this.pnlDepartments.Controls.Add(this.pnlRecords);
            this.pnlDepartments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDepartments.Location = new System.Drawing.Point(0, 70);
            this.pnlDepartments.Name = "pnlDepartments";
            this.pnlDepartments.Padding = new System.Windows.Forms.Padding(20, 20, 20, 10);
            this.pnlDepartments.Size = new System.Drawing.Size(900, 510);
            this.pnlDepartments.TabIndex = 2;
            this.pnlDepartments.Visible = false;

            this.lblDeptTitle.AutoSize = true;
            this.lblDeptTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblDeptTitle.ForeColor = System.Drawing.Color.White;
            this.lblDeptTitle.Location = new System.Drawing.Point(20, 15);
            this.lblDeptTitle.Name = "lblDeptTitle";
            this.lblDeptTitle.Size = new System.Drawing.Size(270, 32);
            this.lblDeptTitle.TabIndex = 0;
            this.lblDeptTitle.Text = "Department Clearance";

            this.lblDeptOverallStatus.AutoSize = true;
            this.lblDeptOverallStatus.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblDeptOverallStatus.ForeColor = System.Drawing.Color.FromArgb(84, 172, 191);
            this.lblDeptOverallStatus.Location = new System.Drawing.Point(22, 55);
            this.lblDeptOverallStatus.Name = "lblDeptOverallStatus";
            this.lblDeptOverallStatus.Size = new System.Drawing.Size(156, 20);
            this.lblDeptOverallStatus.TabIndex = 1;
            this.lblDeptOverallStatus.Text = "Overall Progress: 0/6";

            this.progressDepts.Location = new System.Drawing.Point(25, 80);
            this.progressDepts.Name = "progressDepts";
            this.progressDepts.Size = new System.Drawing.Size(850, 25);
            this.progressDepts.TabIndex = 2;

            this.pnlLibrary.BackColor = System.Drawing.Color.FromArgb(38, 101, 140);
            this.pnlLibrary.Controls.Add(this.lblLibrary);
            this.pnlLibrary.Controls.Add(this.btnLibrarySubmit);
            this.pnlLibrary.Controls.Add(this.lblLibraryStatus);
            this.pnlLibrary.Location = new System.Drawing.Point(25, 120);
            this.pnlLibrary.Name = "pnlLibrary";
            this.pnlLibrary.Size = new System.Drawing.Size(415, 95);
            this.pnlLibrary.TabIndex = 3;
            this.lblLibrary.AutoSize = true;
            this.lblLibrary.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblLibrary.ForeColor = System.Drawing.Color.White;
            this.lblLibrary.Location = new System.Drawing.Point(20, 15);
            this.lblLibrary.Name = "lblLibrary";
            this.lblLibrary.Size = new System.Drawing.Size(64, 21);
            this.lblLibrary.TabIndex = 1;
            this.lblLibrary.Text = "Library";
            this.btnLibrarySubmit.BackColor = System.Drawing.Color.FromArgb(38, 101, 140);
            this.btnLibrarySubmit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLibrarySubmit.FlatAppearance.BorderSize = 0;
            this.btnLibrarySubmit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLibrarySubmit.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnLibrarySubmit.ForeColor = System.Drawing.Color.White;
            this.btnLibrarySubmit.Location = new System.Drawing.Point(290, 28);
            this.btnLibrarySubmit.Name = "btnLibrarySubmit";
            this.btnLibrarySubmit.Size = new System.Drawing.Size(110, 40);
            this.btnLibrarySubmit.TabIndex = 3;
            this.btnLibrarySubmit.Text = "Submit";
            this.btnLibrarySubmit.UseVisualStyleBackColor = false;
            this.btnLibrarySubmit.Click += new System.EventHandler(this.btnLibrarySubmit_Click);
            this.lblLibraryStatus.AutoSize = true;
            this.lblLibraryStatus.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblLibraryStatus.ForeColor = System.Drawing.Color.FromArgb(84, 172, 191);
            this.lblLibraryStatus.Location = new System.Drawing.Point(20, 45);
            this.lblLibraryStatus.Name = "lblLibraryStatus";
            this.lblLibraryStatus.Size = new System.Drawing.Size(64, 19);
            this.lblLibraryStatus.TabIndex = 2;
            this.lblLibraryStatus.Text = "Pending";

            this.pnlSAO.BackColor = System.Drawing.Color.FromArgb(38, 101, 140);
            this.pnlSAO.Controls.Add(this.lblSAO);
            this.pnlSAO.Controls.Add(this.btnSAOSubmit);
            this.pnlSAO.Controls.Add(this.lblSAOStatus);
            this.pnlSAO.Location = new System.Drawing.Point(460, 120);
            this.pnlSAO.Name = "pnlSAO";
            this.pnlSAO.Size = new System.Drawing.Size(415, 95);
            this.pnlSAO.TabIndex = 4;
            this.lblSAO.AutoSize = true;
            this.lblSAO.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblSAO.ForeColor = System.Drawing.Color.White;
            this.lblSAO.Location = new System.Drawing.Point(20, 15);
            this.lblSAO.Name = "lblSAO";
            this.lblSAO.Size = new System.Drawing.Size(42, 21);
            this.lblSAO.TabIndex = 1;
            this.lblSAO.Text = "SAO";
            this.btnSAOSubmit.BackColor = System.Drawing.Color.FromArgb(38, 101, 140);
            this.btnSAOSubmit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSAOSubmit.FlatAppearance.BorderSize = 0;
            this.btnSAOSubmit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSAOSubmit.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSAOSubmit.ForeColor = System.Drawing.Color.White;
            this.btnSAOSubmit.Location = new System.Drawing.Point(290, 28);
            this.btnSAOSubmit.Name = "btnSAOSubmit";
            this.btnSAOSubmit.Size = new System.Drawing.Size(110, 40);
            this.btnSAOSubmit.TabIndex = 3;
            this.btnSAOSubmit.Text = "Submit";
            this.btnSAOSubmit.UseVisualStyleBackColor = false;
            this.btnSAOSubmit.Click += new System.EventHandler(this.btnSAOSubmit_Click);
            this.lblSAOStatus.AutoSize = true;
            this.lblSAOStatus.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblSAOStatus.ForeColor = System.Drawing.Color.FromArgb(84, 172, 191);
            this.lblSAOStatus.Location = new System.Drawing.Point(20, 45);
            this.lblSAOStatus.Name = "lblSAOStatus";
            this.lblSAOStatus.Size = new System.Drawing.Size(64, 19);
            this.lblSAOStatus.TabIndex = 2;
            this.lblSAOStatus.Text = "Pending";

            this.pnlCashier.BackColor = System.Drawing.Color.FromArgb(38, 101, 140);
            this.pnlCashier.Controls.Add(this.lblCashier);
            this.pnlCashier.Controls.Add(this.btnCashierSubmit);
            this.pnlCashier.Controls.Add(this.lblCashierStatus);
            this.pnlCashier.Location = new System.Drawing.Point(25, 235);
            this.pnlCashier.Name = "pnlCashier";
            this.pnlCashier.Size = new System.Drawing.Size(415, 95);
            this.pnlCashier.TabIndex = 5;
            this.lblCashier.AutoSize = true;
            this.lblCashier.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblCashier.ForeColor = System.Drawing.Color.White;
            this.lblCashier.Location = new System.Drawing.Point(20, 15);
            this.lblCashier.Name = "lblCashier";
            this.lblCashier.Size = new System.Drawing.Size(66, 21);
            this.lblCashier.TabIndex = 1;
            this.lblCashier.Text = "Cashier";
            this.btnCashierSubmit.BackColor = System.Drawing.Color.FromArgb(38, 101, 140);
            this.btnCashierSubmit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCashierSubmit.FlatAppearance.BorderSize = 0;
            this.btnCashierSubmit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCashierSubmit.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCashierSubmit.ForeColor = System.Drawing.Color.White;
            this.btnCashierSubmit.Location = new System.Drawing.Point(290, 28);
            this.btnCashierSubmit.Name = "btnCashierSubmit";
            this.btnCashierSubmit.Size = new System.Drawing.Size(110, 40);
            this.btnCashierSubmit.TabIndex = 3;
            this.btnCashierSubmit.Text = "Submit";
            this.btnCashierSubmit.UseVisualStyleBackColor = false;
            this.btnCashierSubmit.Click += new System.EventHandler(this.btnCashierSubmit_Click);
            this.lblCashierStatus.AutoSize = true;
            this.lblCashierStatus.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblCashierStatus.ForeColor = System.Drawing.Color.FromArgb(84, 172, 191);
            this.lblCashierStatus.Location = new System.Drawing.Point(20, 45);
            this.lblCashierStatus.Name = "lblCashierStatus";
            this.lblCashierStatus.Size = new System.Drawing.Size(64, 19);
            this.lblCashierStatus.TabIndex = 2;
            this.lblCashierStatus.Text = "Pending";

            this.pnlAccounting.BackColor = System.Drawing.Color.FromArgb(38, 101, 140);
            this.pnlAccounting.Controls.Add(this.lblAccounting);
            this.pnlAccounting.Controls.Add(this.btnAccountingSubmit);
            this.pnlAccounting.Controls.Add(this.lblAccountingStatus);
            this.pnlAccounting.Location = new System.Drawing.Point(460, 235);
            this.pnlAccounting.Name = "pnlAccounting";
            this.pnlAccounting.Size = new System.Drawing.Size(415, 95);
            this.pnlAccounting.TabIndex = 6;
            this.lblAccounting.AutoSize = true;
            this.lblAccounting.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblAccounting.ForeColor = System.Drawing.Color.White;
            this.lblAccounting.Location = new System.Drawing.Point(20, 15);
            this.lblAccounting.Name = "lblAccounting";
            this.lblAccounting.Size = new System.Drawing.Size(98, 21);
            this.lblAccounting.TabIndex = 1;
            this.lblAccounting.Text = "Accounting";
            this.btnAccountingSubmit.BackColor = System.Drawing.Color.FromArgb(38, 101, 140);
            this.btnAccountingSubmit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAccountingSubmit.FlatAppearance.BorderSize = 0;
            this.btnAccountingSubmit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAccountingSubmit.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnAccountingSubmit.ForeColor = System.Drawing.Color.White;
            this.btnAccountingSubmit.Location = new System.Drawing.Point(290, 28);
            this.btnAccountingSubmit.Name = "btnAccountingSubmit";
            this.btnAccountingSubmit.Size = new System.Drawing.Size(110, 40);
            this.btnAccountingSubmit.TabIndex = 3;
            this.btnAccountingSubmit.Text = "Submit";
            this.btnAccountingSubmit.UseVisualStyleBackColor = false;
            this.btnAccountingSubmit.Click += new System.EventHandler(this.btnAccountingSubmit_Click);
            this.lblAccountingStatus.AutoSize = true;
            this.lblAccountingStatus.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblAccountingStatus.ForeColor = System.Drawing.Color.FromArgb(84, 172, 191);
            this.lblAccountingStatus.Location = new System.Drawing.Point(20, 45);
            this.lblAccountingStatus.Name = "lblAccountingStatus";
            this.lblAccountingStatus.Size = new System.Drawing.Size(64, 19);
            this.lblAccountingStatus.TabIndex = 2;
            this.lblAccountingStatus.Text = "Pending";

            this.pnlDean.BackColor = System.Drawing.Color.FromArgb(38, 101, 140);
            this.pnlDean.Controls.Add(this.lblDean);
            this.pnlDean.Controls.Add(this.btnDeanSubmit);
            this.pnlDean.Controls.Add(this.lblDeanStatus);
            this.pnlDean.Location = new System.Drawing.Point(25, 350);
            this.pnlDean.Name = "pnlDean";
            this.pnlDean.Size = new System.Drawing.Size(415, 95);
            this.pnlDean.TabIndex = 7;
            this.lblDean.AutoSize = true;
            this.lblDean.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblDean.ForeColor = System.Drawing.Color.White;
            this.lblDean.Location = new System.Drawing.Point(20, 15);
            this.lblDean.Name = "lblDean";
            this.lblDean.Size = new System.Drawing.Size(110, 21);
            this.lblDean.TabIndex = 1;
            this.lblDean.Text = "Dean's Office";
            this.btnDeanSubmit.BackColor = System.Drawing.Color.FromArgb(38, 101, 140);
            this.btnDeanSubmit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDeanSubmit.FlatAppearance.BorderSize = 0;
            this.btnDeanSubmit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeanSubmit.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnDeanSubmit.ForeColor = System.Drawing.Color.White;
            this.btnDeanSubmit.Location = new System.Drawing.Point(290, 28);
            this.btnDeanSubmit.Name = "btnDeanSubmit";
            this.btnDeanSubmit.Size = new System.Drawing.Size(110, 40);
            this.btnDeanSubmit.TabIndex = 3;
            this.btnDeanSubmit.Text = "Submit";
            this.btnDeanSubmit.UseVisualStyleBackColor = false;
            this.btnDeanSubmit.Click += new System.EventHandler(this.btnDeanSubmit_Click);
            this.lblDeanStatus.AutoSize = true;
            this.lblDeanStatus.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblDeanStatus.ForeColor = System.Drawing.Color.FromArgb(84, 172, 191);
            this.lblDeanStatus.Location = new System.Drawing.Point(20, 45);
            this.lblDeanStatus.Name = "lblDeanStatus";
            this.lblDeanStatus.Size = new System.Drawing.Size(64, 19);
            this.lblDeanStatus.TabIndex = 2;
            this.lblDeanStatus.Text = "Pending";

            this.pnlRecords.BackColor = System.Drawing.Color.FromArgb(38, 101, 140);
            this.pnlRecords.Controls.Add(this.lblRecords);
            this.pnlRecords.Controls.Add(this.btnRecordsSubmit);
            this.pnlRecords.Controls.Add(this.lblRecordsStatus);
            this.pnlRecords.Location = new System.Drawing.Point(460, 350);
            this.pnlRecords.Name = "pnlRecords";
            this.pnlRecords.Size = new System.Drawing.Size(415, 95);
            this.pnlRecords.TabIndex = 8;
            this.lblRecords.AutoSize = true;
            this.lblRecords.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblRecords.ForeColor = System.Drawing.Color.White;
            this.lblRecords.Location = new System.Drawing.Point(20, 15);
            this.lblRecords.Name = "lblRecords";
            this.lblRecords.Size = new System.Drawing.Size(70, 21);
            this.lblRecords.TabIndex = 1;
            this.lblRecords.Text = "Records";
            this.btnRecordsSubmit.BackColor = System.Drawing.Color.FromArgb(38, 101, 140);
            this.btnRecordsSubmit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRecordsSubmit.FlatAppearance.BorderSize = 0;
            this.btnRecordsSubmit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRecordsSubmit.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnRecordsSubmit.ForeColor = System.Drawing.Color.White;
            this.btnRecordsSubmit.Location = new System.Drawing.Point(290, 28);
            this.btnRecordsSubmit.Name = "btnRecordsSubmit";
            this.btnRecordsSubmit.Size = new System.Drawing.Size(110, 40);
            this.btnRecordsSubmit.TabIndex = 3;
            this.btnRecordsSubmit.Text = "Submit";
            this.btnRecordsSubmit.UseVisualStyleBackColor = false;
            this.btnRecordsSubmit.Click += new System.EventHandler(this.btnRecordsSubmit_Click);
            this.lblRecordsStatus.AutoSize = true;
            this.lblRecordsStatus.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblRecordsStatus.ForeColor = System.Drawing.Color.FromArgb(84, 172, 191);
            this.lblRecordsStatus.Location = new System.Drawing.Point(20, 45);
            this.lblRecordsStatus.Name = "lblRecordsStatus";
            this.lblRecordsStatus.Size = new System.Drawing.Size(64, 19);
            this.lblRecordsStatus.TabIndex = 2;
            this.lblRecordsStatus.Text = "Pending";

            // ===================================================
            // CERTIFICATE PANEL (SIMPLIFIED)
            // ===================================================
            this.pnlCertificate.BackColor = System.Drawing.Color.Transparent;
            this.pnlCertificate.Controls.Add(this.lblCertTitle);
            this.pnlCertificate.Controls.Add(this.lblCertStudentID);
            this.pnlCertificate.Controls.Add(this.lblCertProgram);
            this.pnlCertificate.Controls.Add(this.lblCertDate);
            this.pnlCertificate.Controls.Add(this.lblCertSubjectsCheck);
            this.pnlCertificate.Controls.Add(this.lblCertDeptsCheck);
            this.pnlCertificate.Controls.Add(this.btnDownloadCert);
            this.pnlCertificate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCertificate.Location = new System.Drawing.Point(0, 70);
            this.pnlCertificate.Name = "pnlCertificate";
            this.pnlCertificate.Padding = new System.Windows.Forms.Padding(40, 30, 40, 20);
            this.pnlCertificate.Size = new System.Drawing.Size(900, 510);
            this.pnlCertificate.TabIndex = 3;
            this.pnlCertificate.Visible = false;

            // lblCertTitle
            this.lblCertTitle.AutoSize = false;
            this.lblCertTitle.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            this.lblCertTitle.ForeColor = System.Drawing.Color.White;
            this.lblCertTitle.Location = new System.Drawing.Point(40, 50);
            this.lblCertTitle.Name = "lblCertTitle";
            this.lblCertTitle.Size = new System.Drawing.Size(820, 40);
            this.lblCertTitle.Text = "CLEARANCE COMPLETION CERTIFICATE";
            this.lblCertTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // lblCertStudentID
            this.lblCertStudentID.AutoSize = false;
            this.lblCertStudentID.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.lblCertStudentID.ForeColor = System.Drawing.Color.White;
            this.lblCertStudentID.Location = new System.Drawing.Point(40, 110);
            this.lblCertStudentID.Name = "lblCertStudentID";
            this.lblCertStudentID.Size = new System.Drawing.Size(820, 30);
            this.lblCertStudentID.Text = "Student ID: STUD001";
            this.lblCertStudentID.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // lblCertProgram
            this.lblCertProgram.AutoSize = false;
            this.lblCertProgram.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.lblCertProgram.ForeColor = System.Drawing.Color.White;
            this.lblCertProgram.Location = new System.Drawing.Point(40, 150);
            this.lblCertProgram.Name = "lblCertProgram";
            this.lblCertProgram.Size = new System.Drawing.Size(820, 30);
            this.lblCertProgram.Text = "Program: BS Information Technology";
            this.lblCertProgram.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // lblCertDate
            this.lblCertDate.AutoSize = false;
            this.lblCertDate.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblCertDate.ForeColor = System.Drawing.Color.FromArgb(185, 187, 190);
            this.lblCertDate.Location = new System.Drawing.Point(40, 190);
            this.lblCertDate.Name = "lblCertDate";
            this.lblCertDate.Size = new System.Drawing.Size(820, 25);
            this.lblCertDate.Text = "Completed on: January 1, 2025";
            this.lblCertDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // lblCertSubjectsCheck
            this.lblCertSubjectsCheck.AutoSize = false;
            this.lblCertSubjectsCheck.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblCertSubjectsCheck.ForeColor = System.Drawing.Color.FromArgb(84, 172, 191);
            this.lblCertSubjectsCheck.Location = new System.Drawing.Point(40, 250);
            this.lblCertSubjectsCheck.Name = "lblCertSubjectsCheck";
            this.lblCertSubjectsCheck.Size = new System.Drawing.Size(820, 35);
            this.lblCertSubjectsCheck.Text = "✓ Subjects Cleared";
            this.lblCertSubjectsCheck.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // lblCertDeptsCheck
            this.lblCertDeptsCheck.AutoSize = false;
            this.lblCertDeptsCheck.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblCertDeptsCheck.ForeColor = System.Drawing.Color.FromArgb(84, 172, 191);
            this.lblCertDeptsCheck.Location = new System.Drawing.Point(40, 300);
            this.lblCertDeptsCheck.Name = "lblCertDeptsCheck";
            this.lblCertDeptsCheck.Size = new System.Drawing.Size(820, 35);
            this.lblCertDeptsCheck.Text = "✓ Departments Cleared";
            this.lblCertDeptsCheck.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // btnDownloadCert
            this.btnDownloadCert.BackColor = System.Drawing.Color.FromArgb(38, 101, 140);
            this.btnDownloadCert.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDownloadCert.FlatAppearance.BorderSize = 0;
            this.btnDownloadCert.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDownloadCert.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnDownloadCert.ForeColor = System.Drawing.Color.White;
            this.btnDownloadCert.Location = new System.Drawing.Point(280, 390);
            this.btnDownloadCert.Name = "btnDownloadCert";
            this.btnDownloadCert.Size = new System.Drawing.Size(340, 50);
            this.btnDownloadCert.TabIndex = 0;
            this.btnDownloadCert.Text = "Download Certificate (PNG)";
            this.btnDownloadCert.UseVisualStyleBackColor = false;
            this.btnDownloadCert.Click += new System.EventHandler(this.btnDownloadCert_Click);

            // ===================================================
            // STEP NAVIGATION
            // ===================================================
            this.pnlStepNav.BackColor = System.Drawing.Color.FromArgb(47, 49, 54);
            this.pnlStepNav.Controls.Add(this.btnStep1);
            this.pnlStepNav.Controls.Add(this.btnStep2);
            this.pnlStepNav.Controls.Add(this.btnStep3);
            this.pnlStepNav.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlStepNav.Location = new System.Drawing.Point(0, 530);
            this.pnlStepNav.Name = "pnlStepNav";
            this.pnlStepNav.Size = new System.Drawing.Size(900, 50);
            this.pnlStepNav.TabIndex = 3;
            // 
            // btnStep1
            // 
            this.btnStep1.BackColor = System.Drawing.Color.FromArgb(84, 172, 191);
            this.btnStep1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStep1.FlatAppearance.BorderSize = 0;
            this.btnStep1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStep1.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.btnStep1.ForeColor = System.Drawing.Color.White;
            this.btnStep1.Location = new System.Drawing.Point(380, 8);
            this.btnStep1.Name = "btnStep1";
            this.btnStep1.Size = new System.Drawing.Size(35, 35);
            this.btnStep1.TabIndex = 0;
            this.btnStep1.Text = "";
            this.btnStep1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnStep1.UseVisualStyleBackColor = false;
            this.btnStep1.Click += new System.EventHandler(this.btnStep1_Click);
            // 
            // btnStep2
            // 
            this.btnStep2.BackColor = System.Drawing.Color.FromArgb(100, 100, 100);
            this.btnStep2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStep2.Enabled = false;
            this.btnStep2.FlatAppearance.BorderSize = 0;
            this.btnStep2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStep2.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnStep2.ForeColor = System.Drawing.Color.FromArgb(150, 150, 150);
            this.btnStep2.Location = new System.Drawing.Point(425, 8);
            this.btnStep2.Name = "btnStep2";
            this.btnStep2.Size = new System.Drawing.Size(35, 35);
            this.btnStep2.TabIndex = 1;
            this.btnStep2.Text = "";
            this.btnStep2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnStep2.UseVisualStyleBackColor = false;
            this.btnStep2.Click += new System.EventHandler(this.btnStep2_Click);
            // 
            // btnStep3
            // 
            this.btnStep3.BackColor = System.Drawing.Color.FromArgb(30, 60, 90);
            this.btnStep3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStep3.Enabled = false;
            this.btnStep3.FlatAppearance.BorderSize = 0;
            this.btnStep3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStep3.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnStep3.ForeColor = System.Drawing.Color.FromArgb(150, 150, 160);
            this.btnStep3.Location = new System.Drawing.Point(470, 8);
            this.btnStep3.Name = "btnStep3";
            this.btnStep3.Size = new System.Drawing.Size(35, 35);
            this.btnStep3.TabIndex = 2;
            this.btnStep3.Text = "";
            this.btnStep3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnStep3.UseVisualStyleBackColor = false;
            this.btnStep3.Click += new System.EventHandler(this.btnStep3_Click);

            // ===================================================
            // StudentDashboardForm
            // ===================================================
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(32, 34, 37);
            this.ClientSize = new System.Drawing.Size(900, 580);
            this.Controls.Add(this.pnlSubjects);
            this.Controls.Add(this.pnlDepartments);
            this.Controls.Add(this.pnlCertificate);
            this.Controls.Add(this.pnlStepNav);
            this.Controls.Add(this.pnlTopBar);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.Name = "StudentDashboardForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.pnlTopBar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).EndInit();
            this.pnlSubjects.ResumeLayout(false);
            this.pnlSubjects.PerformLayout();
            this.pnlSubject1.ResumeLayout(false);
            this.pnlSubject1.PerformLayout();
            this.pnlSubject2.ResumeLayout(false);
            this.pnlSubject2.PerformLayout();
            this.pnlSubject3.ResumeLayout(false);
            this.pnlSubject3.PerformLayout();
            this.pnlSubject4.ResumeLayout(false);
            this.pnlSubject4.PerformLayout();
            this.pnlSubject5.ResumeLayout(false);
            this.pnlSubject5.PerformLayout();
            this.pnlSubject6.ResumeLayout(false);
            this.pnlSubject6.PerformLayout();
            this.pnlDepartments.ResumeLayout(false);
            this.pnlDepartments.PerformLayout();
            this.pnlLibrary.ResumeLayout(false);
            this.pnlLibrary.PerformLayout();
            this.pnlSAO.ResumeLayout(false);
            this.pnlSAO.PerformLayout();
            this.pnlCashier.ResumeLayout(false);
            this.pnlCashier.PerformLayout();
            this.pnlAccounting.ResumeLayout(false);
            this.pnlAccounting.PerformLayout();
            this.pnlDean.ResumeLayout(false);
            this.pnlDean.PerformLayout();
            this.pnlRecords.ResumeLayout(false);
            this.pnlRecords.PerformLayout();
            this.pnlCertificate.ResumeLayout(false);
            this.pnlStepNav.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}