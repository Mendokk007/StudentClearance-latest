namespace CarDealership
{
    partial class AdminFormSubjects
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel pnlTopBar;
        private System.Windows.Forms.PictureBox pbLogo;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Label lblSubject;
        private System.Windows.Forms.Label lblInstructorName;
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.Label lblPendingTitle;
        private System.Windows.Forms.Label lblPendingCount;
        private System.Windows.Forms.DataGridView dgvSubmissions;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.NotifyIcon notifyIcon1;

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
            this.components = new System.ComponentModel.Container();
            this.pnlTopBar = new System.Windows.Forms.Panel();
            this.pbLogo = new System.Windows.Forms.PictureBox();
            this.lblSubject = new System.Windows.Forms.Label();
            this.lblInstructorName = new System.Windows.Forms.Label();
            this.btnLogout = new System.Windows.Forms.Button();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.lblPendingTitle = new System.Windows.Forms.Label();
            this.lblPendingCount = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.dgvSubmissions = new System.Windows.Forms.DataGridView();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.pnlTopBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
            this.pnlContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSubmissions)).BeginInit();
            this.SuspendLayout();

            // pnlTopBar
            this.pnlTopBar.BackColor = System.Drawing.Color.FromArgb(47, 49, 54);
            this.pnlTopBar.Controls.Add(this.pbLogo);
            this.pnlTopBar.Controls.Add(this.lblSubject);
            this.pnlTopBar.Controls.Add(this.lblInstructorName);
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

            // lblSubject
            this.lblSubject.AutoSize = true;
            this.lblSubject.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblSubject.ForeColor = System.Drawing.Color.FromArgb(84, 172, 191);
            this.lblSubject.Location = new System.Drawing.Point(15, 15);
            this.lblSubject.Name = "lblSubject";
            this.lblSubject.Size = new System.Drawing.Size(80, 25);
            this.lblSubject.TabIndex = 2;
            this.lblSubject.Text = "Subject";

            // lblInstructorName
            this.lblInstructorName.AutoSize = true;
            this.lblInstructorName.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblInstructorName.ForeColor = System.Drawing.Color.FromArgb(185, 187, 190);
            this.lblInstructorName.Location = new System.Drawing.Point(17, 40);
            this.lblInstructorName.Name = "lblInstructorName";
            this.lblInstructorName.Size = new System.Drawing.Size(126, 15);
            this.lblInstructorName.TabIndex = 3;
            this.lblInstructorName.Text = "Welcome, Instructor";

            // btnLogout
            this.btnLogout.BackColor = System.Drawing.Color.FromArgb(38, 101, 140);
            this.btnLogout.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogout.FlatAppearance.BorderSize = 0;
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnLogout.ForeColor = System.Drawing.Color.White;
            this.btnLogout.Location = new System.Drawing.Point(790, 20);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(90, 30);
            this.btnLogout.TabIndex = 4;
            this.btnLogout.Text = "Logout";
            this.btnLogout.UseVisualStyleBackColor = false;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);

            // pnlContent
            this.pnlContent.BackColor = System.Drawing.Color.FromArgb(32, 34, 37);
            this.pnlContent.Controls.Add(this.lblPendingTitle);
            this.pnlContent.Controls.Add(this.lblPendingCount);
            this.pnlContent.Controls.Add(this.btnRefresh);
            this.pnlContent.Controls.Add(this.dgvSubmissions);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(0, 70);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Padding = new System.Windows.Forms.Padding(20);
            this.pnlContent.Size = new System.Drawing.Size(900, 530);
            this.pnlContent.TabIndex = 1;

            // lblPendingTitle
            this.lblPendingTitle.AutoSize = true;
            this.lblPendingTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblPendingTitle.ForeColor = System.Drawing.Color.White;
            this.lblPendingTitle.Location = new System.Drawing.Point(20, 15);
            this.lblPendingTitle.Name = "lblPendingTitle";
            this.lblPendingTitle.Size = new System.Drawing.Size(240, 25);
            this.lblPendingTitle.TabIndex = 0;
            this.lblPendingTitle.Text = "Pending Submissions";

            // lblPendingCount
            this.lblPendingCount.AutoSize = true;
            this.lblPendingCount.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblPendingCount.ForeColor = System.Drawing.Color.FromArgb(84, 172, 191);
            this.lblPendingCount.Location = new System.Drawing.Point(270, 17);
            this.lblPendingCount.Name = "lblPendingCount";
            this.lblPendingCount.Size = new System.Drawing.Size(37, 21);
            this.lblPendingCount.TabIndex = 1;
            this.lblPendingCount.Text = "(0)";

            // btnRefresh
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(38, 101, 140);
            this.btnRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(780, 15);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(90, 30);
            this.btnRefresh.TabIndex = 2;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);

            // dgvSubmissions
            this.dgvSubmissions.AllowUserToAddRows = false;
            this.dgvSubmissions.AllowUserToDeleteRows = false;
            this.dgvSubmissions.AllowUserToResizeRows = false;
            this.dgvSubmissions.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSubmissions.BackgroundColor = System.Drawing.Color.FromArgb(47, 49, 54);
            this.dgvSubmissions.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvSubmissions.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvSubmissions.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvSubmissions.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(64, 68, 75);
            this.dgvSubmissions.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.dgvSubmissions.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            this.dgvSubmissions.ColumnHeadersDefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(64, 68, 75);
            this.dgvSubmissions.ColumnHeadersDefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            this.dgvSubmissions.ColumnHeadersHeight = 40;
            this.dgvSubmissions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvSubmissions.Cursor = System.Windows.Forms.Cursors.Hand;
            this.dgvSubmissions.EnableHeadersVisualStyles = false;
            this.dgvSubmissions.GridColor = System.Drawing.Color.FromArgb(64, 68, 75);
            this.dgvSubmissions.Location = new System.Drawing.Point(25, 60);
            this.dgvSubmissions.MultiSelect = false;
            this.dgvSubmissions.Name = "dgvSubmissions";
            this.dgvSubmissions.ReadOnly = true;
            this.dgvSubmissions.RowHeadersVisible = false;
            this.dgvSubmissions.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(47, 49, 54);
            this.dgvSubmissions.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dgvSubmissions.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.White;
            this.dgvSubmissions.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(38, 101, 140);
            this.dgvSubmissions.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            this.dgvSubmissions.RowTemplate.Height = 50;
            this.dgvSubmissions.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSubmissions.Size = new System.Drawing.Size(850, 450);
            this.dgvSubmissions.TabIndex = 3;
            this.dgvSubmissions.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSubmissions_CellClick);

            // notifyIcon1
            this.notifyIcon1.Icon = System.Drawing.SystemIcons.Information;
            this.notifyIcon1.Visible = true;

            // AdminFormSubjects
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(32, 34, 37);
            this.ClientSize = new System.Drawing.Size(900, 600);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlTopBar);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AdminFormSubjects";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Instructor - Subject Review";
            this.pnlTopBar.ResumeLayout(false);
            this.pnlTopBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).EndInit();
            this.pnlContent.ResumeLayout(false);
            this.pnlContent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSubmissions)).EndInit();
            this.ResumeLayout(false);
        }
    }
}