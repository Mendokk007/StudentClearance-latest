namespace CarDealership
{
    partial class HomeForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel pnlTopBar;
        private System.Windows.Forms.PictureBox pbLogo;
        private System.Windows.Forms.Button btnLogout;

        // Clearance Panel
        private System.Windows.Forms.Panel pnlClearance;
        private System.Windows.Forms.Label lblClearanceTitle;
        private System.Windows.Forms.Label lblOverallStatus;
        private System.Windows.Forms.ProgressBar progressOverall;

        // Department Panels
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

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlTopBar = new System.Windows.Forms.Panel();
            this.pbLogo = new System.Windows.Forms.PictureBox();
            this.btnLogout = new System.Windows.Forms.Button();
            this.pnlClearance = new System.Windows.Forms.Panel();
            this.lblClearanceTitle = new System.Windows.Forms.Label();
            this.lblOverallStatus = new System.Windows.Forms.Label();
            this.progressOverall = new System.Windows.Forms.ProgressBar();

            // Library
            this.pnlLibrary = new System.Windows.Forms.Panel();
            this.lblLibrary = new System.Windows.Forms.Label();
            this.btnLibrarySubmit = new System.Windows.Forms.Button();
            this.lblLibraryStatus = new System.Windows.Forms.Label();

            // SAO
            this.pnlSAO = new System.Windows.Forms.Panel();
            this.lblSAO = new System.Windows.Forms.Label();
            this.btnSAOSubmit = new System.Windows.Forms.Button();
            this.lblSAOStatus = new System.Windows.Forms.Label();

            // Cashier
            this.pnlCashier = new System.Windows.Forms.Panel();
            this.lblCashier = new System.Windows.Forms.Label();
            this.btnCashierSubmit = new System.Windows.Forms.Button();
            this.lblCashierStatus = new System.Windows.Forms.Label();

            // Accounting
            this.pnlAccounting = new System.Windows.Forms.Panel();
            this.lblAccounting = new System.Windows.Forms.Label();
            this.btnAccountingSubmit = new System.Windows.Forms.Button();
            this.lblAccountingStatus = new System.Windows.Forms.Label();

            // Dean's Office
            this.pnlDean = new System.Windows.Forms.Panel();
            this.lblDean = new System.Windows.Forms.Label();
            this.btnDeanSubmit = new System.Windows.Forms.Button();
            this.lblDeanStatus = new System.Windows.Forms.Label();

            // Records
            this.pnlRecords = new System.Windows.Forms.Panel();
            this.lblRecords = new System.Windows.Forms.Label();
            this.btnRecordsSubmit = new System.Windows.Forms.Button();
            this.lblRecordsStatus = new System.Windows.Forms.Label();

            this.pnlTopBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
            this.pnlClearance.SuspendLayout();
            this.pnlLibrary.SuspendLayout();
            this.pnlSAO.SuspendLayout();
            this.pnlCashier.SuspendLayout();
            this.pnlAccounting.SuspendLayout();
            this.pnlDean.SuspendLayout();
            this.pnlRecords.SuspendLayout();
            this.SuspendLayout();

            // pnlTopBar
            this.pnlTopBar.BackColor = System.Drawing.Color.FromArgb(47, 49, 54);
            this.pnlTopBar.Controls.Add(this.pbLogo);
            this.pnlTopBar.Controls.Add(this.btnLogout);
            this.pnlTopBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTopBar.Location = new System.Drawing.Point(0, 0);
            this.pnlTopBar.Name = "pnlTopBar";
            this.pnlTopBar.Size = new System.Drawing.Size(900, 70);
            this.pnlTopBar.TabIndex = 0;

            // pbLogo
            this.pbLogo.BackColor = System.Drawing.Color.Transparent;
            this.pbLogo.Location = new System.Drawing.Point(400, 10);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.Size = new System.Drawing.Size(100, 50);
            this.pbLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbLogo.TabIndex = 1;
            this.pbLogo.TabStop = false;

            // btnLogout
            this.btnLogout.BackColor = System.Drawing.Color.FromArgb(217, 55, 55);
            this.btnLogout.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogout.FlatAppearance.BorderSize = 0;
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnLogout.ForeColor = System.Drawing.Color.White;
            this.btnLogout.Location = new System.Drawing.Point(790, 20);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(90, 30);
            this.btnLogout.TabIndex = 2;
            this.btnLogout.Text = "Logout";
            this.btnLogout.UseVisualStyleBackColor = false;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);

            // pnlClearance
            this.pnlClearance.AutoScroll = true;
            this.pnlClearance.BackColor = System.Drawing.Color.FromArgb(32, 34, 37);
            this.pnlClearance.Controls.Add(this.lblClearanceTitle);
            this.pnlClearance.Controls.Add(this.lblOverallStatus);
            this.pnlClearance.Controls.Add(this.progressOverall);
            this.pnlClearance.Controls.Add(this.pnlLibrary);
            this.pnlClearance.Controls.Add(this.pnlSAO);
            this.pnlClearance.Controls.Add(this.pnlCashier);
            this.pnlClearance.Controls.Add(this.pnlAccounting);
            this.pnlClearance.Controls.Add(this.pnlDean);
            this.pnlClearance.Controls.Add(this.pnlRecords);
            this.pnlClearance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlClearance.Location = new System.Drawing.Point(0, 70);
            this.pnlClearance.Name = "pnlClearance";
            this.pnlClearance.Padding = new System.Windows.Forms.Padding(20);
            this.pnlClearance.Size = new System.Drawing.Size(900, 630);
            this.pnlClearance.TabIndex = 1;

            // lblClearanceTitle
            this.lblClearanceTitle.AutoSize = true;
            this.lblClearanceTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblClearanceTitle.ForeColor = System.Drawing.Color.White;
            this.lblClearanceTitle.Location = new System.Drawing.Point(20, 15);
            this.lblClearanceTitle.Name = "lblClearanceTitle";
            this.lblClearanceTitle.Size = new System.Drawing.Size(205, 25);
            this.lblClearanceTitle.TabIndex = 0;
            this.lblClearanceTitle.Text = "Department Clearance";

            // lblOverallStatus
            this.lblOverallStatus.AutoSize = true;
            this.lblOverallStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblOverallStatus.ForeColor = System.Drawing.Color.FromArgb(185, 187, 190);
            this.lblOverallStatus.Location = new System.Drawing.Point(22, 55);
            this.lblOverallStatus.Name = "lblOverallStatus";
            this.lblOverallStatus.Size = new System.Drawing.Size(98, 15);
            this.lblOverallStatus.TabIndex = 1;
            this.lblOverallStatus.Text = "Overall Progress: 0/6";

            // progressOverall
            this.progressOverall.Location = new System.Drawing.Point(25, 75);
            this.progressOverall.Name = "progressOverall";
            this.progressOverall.Size = new System.Drawing.Size(850, 20);
            this.progressOverall.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressOverall.TabIndex = 2;

            // pnlLibrary
            this.pnlLibrary.BackColor = System.Drawing.Color.FromArgb(47, 49, 54);
            this.pnlLibrary.Controls.Add(this.lblLibrary);
            this.pnlLibrary.Controls.Add(this.btnLibrarySubmit);
            this.pnlLibrary.Controls.Add(this.lblLibraryStatus);
            this.pnlLibrary.Location = new System.Drawing.Point(25, 115);
            this.pnlLibrary.Name = "pnlLibrary";
            this.pnlLibrary.Size = new System.Drawing.Size(415, 100);
            this.pnlLibrary.TabIndex = 3;

            // lblLibrary
            this.lblLibrary.AutoSize = true;
            this.lblLibrary.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblLibrary.ForeColor = System.Drawing.Color.White;
            this.lblLibrary.Location = new System.Drawing.Point(20, 20);
            this.lblLibrary.Name = "lblLibrary";
            this.lblLibrary.Size = new System.Drawing.Size(63, 21);
            this.lblLibrary.TabIndex = 1;
            this.lblLibrary.Text = "Library";

            // lblLibraryStatus
            this.lblLibraryStatus.AutoSize = true;
            this.lblLibraryStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblLibraryStatus.ForeColor = System.Drawing.Color.FromArgb(255, 193, 7);
            this.lblLibraryStatus.Location = new System.Drawing.Point(20, 50);
            this.lblLibraryStatus.Name = "lblLibraryStatus";
            this.lblLibraryStatus.Size = new System.Drawing.Size(56, 15);
            this.lblLibraryStatus.TabIndex = 2;
            this.lblLibraryStatus.Text = "Pending";

            // btnLibrarySubmit
            this.btnLibrarySubmit.BackColor = System.Drawing.Color.FromArgb(217, 55, 55);
            this.btnLibrarySubmit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLibrarySubmit.FlatAppearance.BorderSize = 0;
            this.btnLibrarySubmit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLibrarySubmit.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnLibrarySubmit.ForeColor = System.Drawing.Color.White;
            this.btnLibrarySubmit.Location = new System.Drawing.Point(310, 35);
            this.btnLibrarySubmit.Name = "btnLibrarySubmit";
            this.btnLibrarySubmit.Size = new System.Drawing.Size(90, 30);
            this.btnLibrarySubmit.TabIndex = 3;
            this.btnLibrarySubmit.Text = "Submit";
            this.btnLibrarySubmit.UseVisualStyleBackColor = false;
            this.btnLibrarySubmit.Click += new System.EventHandler(this.btnLibrarySubmit_Click);

            // pnlSAO
            this.pnlSAO.BackColor = System.Drawing.Color.FromArgb(47, 49, 54);
            this.pnlSAO.Controls.Add(this.lblSAO);
            this.pnlSAO.Controls.Add(this.btnSAOSubmit);
            this.pnlSAO.Controls.Add(this.lblSAOStatus);
            this.pnlSAO.Location = new System.Drawing.Point(460, 115);
            this.pnlSAO.Name = "pnlSAO";
            this.pnlSAO.Size = new System.Drawing.Size(415, 100);
            this.pnlSAO.TabIndex = 4;

            // lblSAO
            this.lblSAO.AutoSize = true;
            this.lblSAO.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblSAO.ForeColor = System.Drawing.Color.White;
            this.lblSAO.Location = new System.Drawing.Point(20, 20);
            this.lblSAO.Name = "lblSAO";
            this.lblSAO.Size = new System.Drawing.Size(44, 21);
            this.lblSAO.TabIndex = 1;
            this.lblSAO.Text = "SAO";

            // lblSAOStatus
            this.lblSAOStatus.AutoSize = true;
            this.lblSAOStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblSAOStatus.ForeColor = System.Drawing.Color.FromArgb(255, 193, 7);
            this.lblSAOStatus.Location = new System.Drawing.Point(20, 50);
            this.lblSAOStatus.Name = "lblSAOStatus";
            this.lblSAOStatus.Size = new System.Drawing.Size(56, 15);
            this.lblSAOStatus.TabIndex = 2;
            this.lblSAOStatus.Text = "Pending";

            // btnSAOSubmit
            this.btnSAOSubmit.BackColor = System.Drawing.Color.FromArgb(217, 55, 55);
            this.btnSAOSubmit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSAOSubmit.FlatAppearance.BorderSize = 0;
            this.btnSAOSubmit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSAOSubmit.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnSAOSubmit.ForeColor = System.Drawing.Color.White;
            this.btnSAOSubmit.Location = new System.Drawing.Point(310, 35);
            this.btnSAOSubmit.Name = "btnSAOSubmit";
            this.btnSAOSubmit.Size = new System.Drawing.Size(90, 30);
            this.btnSAOSubmit.TabIndex = 3;
            this.btnSAOSubmit.Text = "Submit";
            this.btnSAOSubmit.UseVisualStyleBackColor = false;
            this.btnSAOSubmit.Click += new System.EventHandler(this.btnSAOSubmit_Click);

            // pnlCashier
            this.pnlCashier.BackColor = System.Drawing.Color.FromArgb(47, 49, 54);
            this.pnlCashier.Controls.Add(this.lblCashier);
            this.pnlCashier.Controls.Add(this.btnCashierSubmit);
            this.pnlCashier.Controls.Add(this.lblCashierStatus);
            this.pnlCashier.Location = new System.Drawing.Point(25, 235);
            this.pnlCashier.Name = "pnlCashier";
            this.pnlCashier.Size = new System.Drawing.Size(415, 100);
            this.pnlCashier.TabIndex = 5;

            // lblCashier
            this.lblCashier.AutoSize = true;
            this.lblCashier.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblCashier.ForeColor = System.Drawing.Color.White;
            this.lblCashier.Location = new System.Drawing.Point(20, 20);
            this.lblCashier.Name = "lblCashier";
            this.lblCashier.Size = new System.Drawing.Size(65, 21);
            this.lblCashier.TabIndex = 1;
            this.lblCashier.Text = "Cashier";

            // lblCashierStatus
            this.lblCashierStatus.AutoSize = true;
            this.lblCashierStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblCashierStatus.ForeColor = System.Drawing.Color.FromArgb(255, 193, 7);
            this.lblCashierStatus.Location = new System.Drawing.Point(20, 50);
            this.lblCashierStatus.Name = "lblCashierStatus";
            this.lblCashierStatus.Size = new System.Drawing.Size(56, 15);
            this.lblCashierStatus.TabIndex = 2;
            this.lblCashierStatus.Text = "Pending";

            // btnCashierSubmit
            this.btnCashierSubmit.BackColor = System.Drawing.Color.FromArgb(217, 55, 55);
            this.btnCashierSubmit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCashierSubmit.FlatAppearance.BorderSize = 0;
            this.btnCashierSubmit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCashierSubmit.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnCashierSubmit.ForeColor = System.Drawing.Color.White;
            this.btnCashierSubmit.Location = new System.Drawing.Point(310, 35);
            this.btnCashierSubmit.Name = "btnCashierSubmit";
            this.btnCashierSubmit.Size = new System.Drawing.Size(90, 30);
            this.btnCashierSubmit.TabIndex = 3;
            this.btnCashierSubmit.Text = "Submit";
            this.btnCashierSubmit.UseVisualStyleBackColor = false;
            this.btnCashierSubmit.Click += new System.EventHandler(this.btnCashierSubmit_Click);

            // pnlAccounting
            this.pnlAccounting.BackColor = System.Drawing.Color.FromArgb(47, 49, 54);
            this.pnlAccounting.Controls.Add(this.lblAccounting);
            this.pnlAccounting.Controls.Add(this.btnAccountingSubmit);
            this.pnlAccounting.Controls.Add(this.lblAccountingStatus);
            this.pnlAccounting.Location = new System.Drawing.Point(460, 235);
            this.pnlAccounting.Name = "pnlAccounting";
            this.pnlAccounting.Size = new System.Drawing.Size(415, 100);
            this.pnlAccounting.TabIndex = 6;

            // lblAccounting
            this.lblAccounting.AutoSize = true;
            this.lblAccounting.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblAccounting.ForeColor = System.Drawing.Color.White;
            this.lblAccounting.Location = new System.Drawing.Point(20, 20);
            this.lblAccounting.Name = "lblAccounting";
            this.lblAccounting.Size = new System.Drawing.Size(99, 21);
            this.lblAccounting.TabIndex = 1;
            this.lblAccounting.Text = "Accounting";

            // lblAccountingStatus
            this.lblAccountingStatus.AutoSize = true;
            this.lblAccountingStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblAccountingStatus.ForeColor = System.Drawing.Color.FromArgb(255, 193, 7);
            this.lblAccountingStatus.Location = new System.Drawing.Point(20, 50);
            this.lblAccountingStatus.Name = "lblAccountingStatus";
            this.lblAccountingStatus.Size = new System.Drawing.Size(56, 15);
            this.lblAccountingStatus.TabIndex = 2;
            this.lblAccountingStatus.Text = "Pending";

            // btnAccountingSubmit
            this.btnAccountingSubmit.BackColor = System.Drawing.Color.FromArgb(217, 55, 55);
            this.btnAccountingSubmit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAccountingSubmit.FlatAppearance.BorderSize = 0;
            this.btnAccountingSubmit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAccountingSubmit.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnAccountingSubmit.ForeColor = System.Drawing.Color.White;
            this.btnAccountingSubmit.Location = new System.Drawing.Point(310, 35);
            this.btnAccountingSubmit.Name = "btnAccountingSubmit";
            this.btnAccountingSubmit.Size = new System.Drawing.Size(90, 30);
            this.btnAccountingSubmit.TabIndex = 3;
            this.btnAccountingSubmit.Text = "Submit";
            this.btnAccountingSubmit.UseVisualStyleBackColor = false;
            this.btnAccountingSubmit.Click += new System.EventHandler(this.btnAccountingSubmit_Click);

            // pnlDean
            this.pnlDean.BackColor = System.Drawing.Color.FromArgb(47, 49, 54);
            this.pnlDean.Controls.Add(this.lblDean);
            this.pnlDean.Controls.Add(this.btnDeanSubmit);
            this.pnlDean.Controls.Add(this.lblDeanStatus);
            this.pnlDean.Location = new System.Drawing.Point(25, 355);
            this.pnlDean.Name = "pnlDean";
            this.pnlDean.Size = new System.Drawing.Size(415, 100);
            this.pnlDean.TabIndex = 7;

            // lblDean
            this.lblDean.AutoSize = true;
            this.lblDean.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblDean.ForeColor = System.Drawing.Color.White;
            this.lblDean.Location = new System.Drawing.Point(20, 20);
            this.lblDean.Name = "lblDean";
            this.lblDean.Size = new System.Drawing.Size(104, 21);
            this.lblDean.TabIndex = 1;
            this.lblDean.Text = "Dean's Office";

            // lblDeanStatus
            this.lblDeanStatus.AutoSize = true;
            this.lblDeanStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblDeanStatus.ForeColor = System.Drawing.Color.FromArgb(255, 193, 7);
            this.lblDeanStatus.Location = new System.Drawing.Point(20, 50);
            this.lblDeanStatus.Name = "lblDeanStatus";
            this.lblDeanStatus.Size = new System.Drawing.Size(56, 15);
            this.lblDeanStatus.TabIndex = 2;
            this.lblDeanStatus.Text = "Pending";

            // btnDeanSubmit
            this.btnDeanSubmit.BackColor = System.Drawing.Color.FromArgb(217, 55, 55);
            this.btnDeanSubmit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDeanSubmit.FlatAppearance.BorderSize = 0;
            this.btnDeanSubmit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeanSubmit.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnDeanSubmit.ForeColor = System.Drawing.Color.White;
            this.btnDeanSubmit.Location = new System.Drawing.Point(310, 35);
            this.btnDeanSubmit.Name = "btnDeanSubmit";
            this.btnDeanSubmit.Size = new System.Drawing.Size(90, 30);
            this.btnDeanSubmit.TabIndex = 3;
            this.btnDeanSubmit.Text = "Submit";
            this.btnDeanSubmit.UseVisualStyleBackColor = false;
            this.btnDeanSubmit.Click += new System.EventHandler(this.btnDeanSubmit_Click);

            // pnlRecords
            this.pnlRecords.BackColor = System.Drawing.Color.FromArgb(47, 49, 54);
            this.pnlRecords.Controls.Add(this.lblRecords);
            this.pnlRecords.Controls.Add(this.btnRecordsSubmit);
            this.pnlRecords.Controls.Add(this.lblRecordsStatus);
            this.pnlRecords.Location = new System.Drawing.Point(460, 355);
            this.pnlRecords.Name = "pnlRecords";
            this.pnlRecords.Size = new System.Drawing.Size(415, 100);
            this.pnlRecords.TabIndex = 8;

            // lblRecords
            this.lblRecords.AutoSize = true;
            this.lblRecords.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblRecords.ForeColor = System.Drawing.Color.White;
            this.lblRecords.Location = new System.Drawing.Point(20, 20);
            this.lblRecords.Name = "lblRecords";
            this.lblRecords.Size = new System.Drawing.Size(70, 21);
            this.lblRecords.TabIndex = 1;
            this.lblRecords.Text = "Records";

            // lblRecordsStatus
            this.lblRecordsStatus.AutoSize = true;
            this.lblRecordsStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblRecordsStatus.ForeColor = System.Drawing.Color.FromArgb(255, 193, 7);
            this.lblRecordsStatus.Location = new System.Drawing.Point(20, 50);
            this.lblRecordsStatus.Name = "lblRecordsStatus";
            this.lblRecordsStatus.Size = new System.Drawing.Size(56, 15);
            this.lblRecordsStatus.TabIndex = 2;
            this.lblRecordsStatus.Text = "Pending";

            // btnRecordsSubmit
            this.btnRecordsSubmit.BackColor = System.Drawing.Color.FromArgb(217, 55, 55);
            this.btnRecordsSubmit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRecordsSubmit.FlatAppearance.BorderSize = 0;
            this.btnRecordsSubmit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRecordsSubmit.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnRecordsSubmit.ForeColor = System.Drawing.Color.White;
            this.btnRecordsSubmit.Location = new System.Drawing.Point(310, 35);
            this.btnRecordsSubmit.Name = "btnRecordsSubmit";
            this.btnRecordsSubmit.Size = new System.Drawing.Size(90, 30);
            this.btnRecordsSubmit.TabIndex = 3;
            this.btnRecordsSubmit.Text = "Submit";
            this.btnRecordsSubmit.UseVisualStyleBackColor = false;
            this.btnRecordsSubmit.Click += new System.EventHandler(this.btnRecordsSubmit_Click);

            // HomeForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(32, 34, 37);
            this.ClientSize = new System.Drawing.Size(900, 700);
            this.Controls.Add(this.pnlClearance);
            this.Controls.Add(this.pnlTopBar);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.Name = "HomeForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "";
            this.pnlTopBar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).EndInit();
            this.pnlClearance.ResumeLayout(false);
            this.pnlClearance.PerformLayout();
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
            this.ResumeLayout(false);
        }
    }
}