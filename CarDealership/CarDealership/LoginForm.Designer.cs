namespace CarDealership
{
    partial class LoginForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Label lblRegister;
        private System.Windows.Forms.Button btnDarkMode;
        private System.Windows.Forms.Label lblHeaderSink;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlMain = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.lblRegister = new System.Windows.Forms.Label();
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

            this.txtUsername.Location = new System.Drawing.Point(60, 265);
            this.txtUsername.Size = new System.Drawing.Size(260, 30);
            this.txtUsername.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtUsername.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.txtUsername.Enter += new System.EventHandler(this.Field_Enter);
            this.txtUsername.Leave += new System.EventHandler(this.Field_Leave);

            this.txtPassword.Location = new System.Drawing.Point(60, 335);
            this.txtPassword.Size = new System.Drawing.Size(260, 30);
            this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPassword.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.txtPassword.UseSystemPasswordChar = true;
            this.txtPassword.Enter += new System.EventHandler(this.Field_Enter);
            this.txtPassword.Leave += new System.EventHandler(this.Field_Leave);

            this.btnLogin.Location = new System.Drawing.Point(60, 410);
            this.btnLogin.Size = new System.Drawing.Size(260, 50);
            this.btnLogin.BackColor = System.Drawing.Color.FromArgb(38, 101, 140);
            this.btnLogin.ForeColor = System.Drawing.Color.White;
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.FlatAppearance.BorderSize = 0;
            this.btnLogin.Text = "LOGIN";
            this.btnLogin.Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold);
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);

            this.lblRegister.Text = "Don't have an account? Register here";
            this.lblRegister.Location = new System.Drawing.Point(60, 480);
            this.lblRegister.AutoSize = true;
            this.lblRegister.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblRegister.Click += new System.EventHandler(this.lblRegister_Click);

            this.btnDarkMode.Location = new System.Drawing.Point(265, 580);
            this.btnDarkMode.Size = new System.Drawing.Size(100, 25);
            this.btnDarkMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDarkMode.FlatAppearance.BorderSize = 0;
            this.btnDarkMode.BackColor = System.Drawing.Color.Transparent;
            this.btnDarkMode.Click += new System.EventHandler(this.btnDarkMode_Click);

            this.btnClose.Location = new System.Drawing.Point(340, 5);
            this.btnClose.Size = new System.Drawing.Size(35, 35);
            this.btnClose.Text = "X";
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);

            this.lblHeaderSink.Size = new System.Drawing.Size(0, 0);
            this.lblHeaderSink.TabStop = true;

            this.pnlMain.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.btnClose, this.txtUsername, this.txtPassword,
                this.btnLogin, this.lblRegister, this.btnDarkMode, this.lblHeaderSink
            });

            this.Controls.Add(this.pnlMain);
            this.pnlMain.BringToFront();
            this.ResumeLayout(false);
        }
    }
}