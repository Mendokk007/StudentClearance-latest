namespace CarDealership
{
    partial class RegisterForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TextBox txtRegUsername;
        private System.Windows.Forms.TextBox txtRegPassword;
        private System.Windows.Forms.ComboBox cboProgram;
        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.Label lblBackToLogin;
        private System.Windows.Forms.Button btnDarkMode;
        private System.Windows.Forms.Label lblHeaderSink;
        private System.Windows.Forms.Label lblStudentIDNote;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlMain = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.txtRegUsername = new System.Windows.Forms.TextBox();
            this.lblStudentIDNote = new System.Windows.Forms.Label();
            this.txtRegPassword = new System.Windows.Forms.TextBox();
            this.cboProgram = new System.Windows.Forms.ComboBox();
            this.btnRegister = new System.Windows.Forms.Button();
            this.lblBackToLogin = new System.Windows.Forms.Label();
            this.btnDarkMode = new System.Windows.Forms.Button();
            this.lblHeaderSink = new System.Windows.Forms.Label();
            this.SuspendLayout();

            this.ClientSize = new System.Drawing.Size(900, 620);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleDragging);

            this.pnlMain.Width = 380;
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlMain.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlMain_Paint);
            this.pnlMain.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleDragging);

            // Student ID textbox
            this.txtRegUsername.Location = new System.Drawing.Point(60, 245);
            this.txtRegUsername.Size = new System.Drawing.Size(260, 30);
            this.txtRegUsername.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtRegUsername.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.txtRegUsername.ReadOnly = true;
            this.txtRegUsername.TabStop = false;
            this.txtRegUsername.Enter += new System.EventHandler(this.Field_Enter);
            this.txtRegUsername.Leave += new System.EventHandler(this.Field_Leave);

            // "Student ID (Auto-assigned)" label — BELOW the textbox
            this.lblStudentIDNote.AutoSize = true;
            this.lblStudentIDNote.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblStudentIDNote.ForeColor = System.Drawing.Color.FromArgb(185, 187, 190);
            this.lblStudentIDNote.Location = new System.Drawing.Point(62, 278);
            this.lblStudentIDNote.Text = "Student ID (Auto-assigned)";
            this.lblStudentIDNote.BackColor = System.Drawing.Color.Transparent;

            // Password textbox
            this.txtRegPassword.Location = new System.Drawing.Point(60, 305);
            this.txtRegPassword.Size = new System.Drawing.Size(260, 30);
            this.txtRegPassword.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtRegPassword.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.txtRegPassword.UseSystemPasswordChar = true;
            this.txtRegPassword.Enter += new System.EventHandler(this.Field_Enter);
            this.txtRegPassword.Leave += new System.EventHandler(this.Field_Leave);

            // Program combobox
            this.cboProgram.Location = new System.Drawing.Point(60, 365);
            this.cboProgram.Size = new System.Drawing.Size(260, 30);
            this.cboProgram.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.cboProgram.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProgram.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboProgram.Items.AddRange(new object[] {
    "Select Program",
    "BSIT",
    "BSCS",
    "BSBA",
    "BSED",
    "BSHM"
});
            this.cboProgram.SelectedIndex = 0;

            this.btnRegister.Location = new System.Drawing.Point(60, 430);
            this.btnRegister.Size = new System.Drawing.Size(260, 50);
            this.btnRegister.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRegister.FlatAppearance.BorderSize = 0;
            this.btnRegister.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnRegister.Text = "REGISTER";
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);

            this.lblBackToLogin.Text = "Already have an account? Login";
            this.lblBackToLogin.Location = new System.Drawing.Point(60, 495);
            this.lblBackToLogin.AutoSize = true;
            this.lblBackToLogin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblBackToLogin.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblBackToLogin.Click += new System.EventHandler(this.lblBackToLogin_Click);

            this.btnDarkMode.Location = new System.Drawing.Point(265, 580);
            this.btnDarkMode.Size = new System.Drawing.Size(100, 25);
            this.btnDarkMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDarkMode.FlatAppearance.BorderSize = 0;
            this.btnDarkMode.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.btnDarkMode.Click += new System.EventHandler(this.btnDarkMode_Click);

            this.btnClose.Location = new System.Drawing.Point(340, 5);
            this.btnClose.Size = new System.Drawing.Size(35, 35);
            this.btnClose.Text = "X";
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);

            this.pnlMain.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.btnClose,
                this.txtRegUsername,
                this.lblStudentIDNote,
                this.txtRegPassword,
                this.cboProgram,
                this.btnRegister,
                this.lblBackToLogin,
                this.btnDarkMode
            });

            this.Controls.Add(this.pnlMain);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}