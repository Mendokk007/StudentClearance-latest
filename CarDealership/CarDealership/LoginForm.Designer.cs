using System;
using System.Drawing;
using System.Windows.Forms;

namespace CarDealership
{
    partial class LoginForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel pnlSidebar;
        private System.Windows.Forms.PictureBox picSidebarImage;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Label lblRegister;
        private System.Windows.Forms.Label lblDeleteAccount;
        private System.Windows.Forms.Button btnDarkMode;
        private System.Windows.Forms.Label lblHeaderLoginTab;

        private void InitializeComponent()
        {
            this.pnlSidebar = new System.Windows.Forms.Panel();
            this.picSidebarImage = new System.Windows.Forms.PictureBox();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.lblRegister = new System.Windows.Forms.Label();
            this.lblDeleteAccount = new System.Windows.Forms.Label();
            this.btnDarkMode = new System.Windows.Forms.Button();
            this.lblHeaderLoginTab = new System.Windows.Forms.Label();

            this.SuspendLayout();
            this.ClientSize = new System.Drawing.Size(900, 600);
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;

            // Sidebar
            this.pnlSidebar.Dock = DockStyle.Left;
            this.pnlSidebar.Width = 350;
            this.pnlSidebar.Controls.Add(this.picSidebarImage);
            this.picSidebarImage.Dock = DockStyle.Fill;
            this.picSidebarImage.MouseDown += new MouseEventHandler(this.pnlMain_MouseDown);

            // Main
            this.pnlMain.Dock = DockStyle.Fill;
            this.pnlMain.BackColor = Color.White;
            this.pnlMain.Paint += new PaintEventHandler(this.pnlMain_Paint);
            this.pnlMain.MouseDown += new MouseEventHandler(this.pnlMain_MouseDown);

            // Close (Clean look)
            this.btnClose.Location = new Point(510, 10);
            this.btnClose.Size = new Size(30, 30);
            this.btnClose.FlatStyle = FlatStyle.Flat;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.Text = "X";
            this.btnClose.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnClose.Click += new EventHandler(this.btnClose_Click);

            // Inputs
            this.txtUsername.Location = new Point(135, 260);
            this.txtUsername.Size = new Size(300, 25);
            this.txtUsername.BorderStyle = BorderStyle.None;
            this.txtUsername.Font = new Font("Segoe UI", 12F);
            this.txtUsername.Text = "Username";
            this.txtUsername.ForeColor = Color.DarkGray;
            this.txtUsername.Enter += new EventHandler(this.txtUsername_Enter);
            this.txtUsername.Leave += new EventHandler(this.txtUsername_Leave);

            this.txtPassword.Location = new Point(135, 330);
            this.txtPassword.Size = new Size(300, 25);
            this.txtPassword.BorderStyle = BorderStyle.None;
            this.txtPassword.Font = new Font("Segoe UI", 12F);
            this.txtPassword.Text = "Password";
            this.txtPassword.ForeColor = Color.DarkGray;
            this.txtPassword.Enter += new EventHandler(this.txtPassword_Enter);
            this.txtPassword.Leave += new EventHandler(this.txtPassword_Leave);

            // Login Button
            this.btnLogin.Location = new Point(185, 410);
            this.btnLogin.Size = new Size(180, 45);
            this.btnLogin.BackColor = Color.FromArgb(38, 101, 140);
            this.btnLogin.ForeColor = Color.White;
            this.btnLogin.FlatStyle = FlatStyle.Flat;
            this.btnLogin.FlatAppearance.BorderSize = 0;
            this.btnLogin.Text = "LOGIN";
            this.btnLogin.Font = new Font("Segoe UI", 11, FontStyle.Bold);

            // Register Label (Wired for Hover)
            this.lblRegister.Location = new Point(180, 480);
            this.lblRegister.AutoSize = true;
            this.lblRegister.Text = "Don't have an account? Register";
            this.lblRegister.ForeColor = Color.FromArgb(84, 172, 191);
            this.lblRegister.Cursor = Cursors.Hand;
            this.lblRegister.Click += new EventHandler(this.lblRegister_Click);
            this.lblRegister.MouseEnter += new EventHandler(this.lblRegister_MouseEnter);
            this.lblRegister.MouseLeave += new EventHandler(this.lblRegister_MouseLeave);

            // Delete Account (Bottom Left)
            this.lblDeleteAccount.Location = new Point(10, 575);
            this.lblDeleteAccount.Text = "Delete Account";
            this.lblDeleteAccount.ForeColor = Color.LightGray;

            // Dark Mode Switch (NEW POSITION: Bottom Right)
            this.btnDarkMode.Location = new Point(440, 570);
            this.btnDarkMode.Size = new Size(100, 25);
            this.btnDarkMode.FlatStyle = FlatStyle.Flat;
            this.btnDarkMode.FlatAppearance.BorderSize = 0; // FIXED: Clean buttons
            this.btnDarkMode.Font = new Font("Segoe UI", 7F, FontStyle.Bold);
            this.btnDarkMode.Click += new EventHandler(this.btnDarkMode_Click);

            this.lblHeaderLoginTab.Size = new Size(0, 0);

            // Assemble
            this.pnlMain.Controls.Add(this.btnClose);
            this.pnlMain.Controls.Add(this.txtUsername);
            this.pnlMain.Controls.Add(this.txtPassword);
            this.pnlMain.Controls.Add(this.btnLogin);
            this.pnlMain.Controls.Add(this.lblRegister);
            this.pnlMain.Controls.Add(this.lblDeleteAccount);
            this.pnlMain.Controls.Add(this.btnDarkMode);
            this.pnlMain.Controls.Add(this.lblHeaderLoginTab);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlSidebar);
            this.ResumeLayout(false);
        }
    }
}