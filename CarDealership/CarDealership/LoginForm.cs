using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;

namespace CarDealership
{
    public partial class LoginForm : Form
    {
        private readonly string _connectionString;
        private string focusedField = "";
        private bool isDarkMode = false; // REVERTED: Now starts in Light Mode

        // LUNA Palette
        Color lunaDarkest = Color.FromArgb(1, 28, 64);   // #011C40
        Color lunaDark = Color.FromArgb(2, 56, 89);      // #023859
        Color lunaTeal = Color.FromArgb(38, 101, 140);   // #26658C
        Color lunaCyan = Color.FromArgb(84, 172, 191);   // #54ACBF
        Color lunaLight = Color.FromArgb(167, 235, 242); // #A7EBF2

        public LoginForm()
        {
            InitializeComponent();
            _connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=StudentClearanceDB;Integrated Security=True;";
            this.DoubleBuffered = true;

            LoadSidebarImage();
            ApplyTheme();
        }

        private void LoadSidebarImage()
        {
            string imgName = isDarkMode ? "sidebar_bg2.jpg" : "sidebar_bg.jpg";
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, imgName);

            if (File.Exists(path))
            {
                if (picSidebarImage.Image != null) picSidebarImage.Image.Dispose();
                picSidebarImage.Image = Image.FromFile(path);
                picSidebarImage.SizeMode = PictureBoxSizeMode.CenterImage;
            }
            else
            {
                this.picSidebarImage.Paint += new PaintEventHandler(this.picSidebarImage_Paint);
            }
        }

        #region Theme & Interaction Logic
        private void btnDarkMode_Click(object sender, EventArgs e)
        {
            isDarkMode = !isDarkMode;
            LoadSidebarImage();
            ApplyTheme();
        }

        private void ApplyTheme()
        {
            if (isDarkMode)
            {
                pnlMain.BackColor = lunaDarkest;
                txtUsername.BackColor = lunaDarkest;
                txtPassword.BackColor = lunaDarkest;
                txtUsername.ForeColor = Color.White;
                txtPassword.ForeColor = Color.White;
                btnClose.ForeColor = Color.White;
                btnDarkMode.Text = "LIGHT MODE";
                btnDarkMode.ForeColor = lunaLight;
            }
            else
            {
                pnlMain.BackColor = Color.White;
                txtUsername.BackColor = Color.White;
                txtPassword.BackColor = Color.White;
                txtUsername.ForeColor = Color.Black;
                txtPassword.ForeColor = Color.Black;
                btnClose.ForeColor = Color.DarkGray;
                btnDarkMode.Text = "DARK MODE";
                btnDarkMode.ForeColor = lunaCyan;
            }
            pnlMain.Invalidate();
        }

        private void txtUsername_Enter(object sender, EventArgs e)
        {
            focusedField = "username";
            if (txtUsername.Text == "Username") { txtUsername.Text = ""; txtUsername.ForeColor = isDarkMode ? Color.White : Color.Black; }
            pnlMain.Invalidate();
        }

        private void txtUsername_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text)) { txtUsername.Text = "Username"; txtUsername.ForeColor = Color.DarkGray; }
        }

        private void txtPassword_Enter(object sender, EventArgs e)
        {
            focusedField = "password";
            if (txtPassword.Text == "Password") { txtPassword.Text = ""; txtPassword.ForeColor = isDarkMode ? Color.White : Color.Black; txtPassword.UseSystemPasswordChar = true; }
            pnlMain.Invalidate();
        }

        private void txtPassword_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPassword.Text)) { txtPassword.Text = "Password"; txtPassword.ForeColor = Color.DarkGray; txtPassword.UseSystemPasswordChar = false; }
        }

        private void pnlMain_MouseDown(object sender, MouseEventArgs e)
        {
            focusedField = "";
            lblHeaderLoginTab.Focus();
            pnlMain.Invalidate();
            if (e.Button == MouseButtons.Left) { ReleaseCapture(); SendMessage(this.Handle, 0xA1, 0x2, 0); }
        }

        // Hover Effect Logic
        private void lblRegister_MouseEnter(object sender, EventArgs e) => lblRegister.ForeColor = isDarkMode ? lunaLight : lunaDarkest;
        private void lblRegister_MouseLeave(object sender, EventArgs e) => lblRegister.ForeColor = lunaCyan;
        #endregion

        #region Custom Painting
        private void pnlMain_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Color headerColor = isDarkMode ? lunaLight : lunaDarkest;
            Color lineBase = isDarkMode ? Color.FromArgb(60, 255, 255, 255) : Color.FromArgb(220, 220, 220);

            using (Font headerFont = new Font("Segoe UI", 32, FontStyle.Bold))
            {
                string text = "LOGIN";
                Size s = TextRenderer.MeasureText(text, headerFont);
                g.DrawString(text, headerFont, new SolidBrush(headerColor), (pnlMain.Width / 2) - (s.Width / 2), 115);
            }

            using (Pen activePen = new Pen(lunaCyan, 2f))
            using (Pen grayPen = new Pen(lineBase, 1f))
            {
                g.DrawLine(focusedField == "username" ? activePen : grayPen, 105, 290, 435, 290);
                g.DrawLine(focusedField == "password" ? activePen : grayPen, 105, 360, 435, 360);
            }

            GraphicsPath btnPath = new GraphicsPath();
            int r = btnLogin.Height;
            btnPath.AddArc(0, 0, r, r, 90, 180);
            btnPath.AddArc(btnLogin.Width - r, 0, r, r, 270, 180);
            btnPath.CloseFigure();
            btnLogin.Region = new Region(btnPath);
        }

        private void picSidebarImage_Paint(object sender, PaintEventArgs e)
        {
            if (picSidebarImage.Image == null)
            {
                Graphics g = e.Graphics;
                g.FillPolygon(new SolidBrush(lunaDark), new Point[] { new Point(0, 0), new Point(350, 0), new Point(0, 600) });
                g.FillPolygon(new SolidBrush(lunaTeal), new Point[] { new Point(0, 400), new Point(350, 200), new Point(350, 600), new Point(0, 600) });
            }
        }
        #endregion

        [DllImport("user32.dll")] public static extern bool ReleaseCapture();
        [DllImport("user32.dll")] public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        private void btnClose_Click(object sender, EventArgs e) => Application.Exit();
        private void lblRegister_Click(object sender, EventArgs e) => new RegisterForm(_connectionString).ShowDialog();
    }
}