using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace CarDealership
{
    public partial class LoginForm : Form
    {
        private readonly string _connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=StudentClearanceDB;Integrated Security=True;";
        private string focusedField = "";
        private bool isDarkMode = true;
        private bool isSliding = false;
        private AppContext _appContext;
        public bool NavigatedAway { get; private set; } = false;

        private static readonly Font FontTitle = new Font("Segoe UI Semibold", 52F);
        private static readonly Font FontSubtitle = new Font("Segoe UI", 18F);
        private static readonly Font FontHeader = new Font("Segoe UI", 16, FontStyle.Bold);
        private SolidBrush brushPrimary;
        private SolidBrush brushSecondary;
        private Pen penActive;
        private Pen penInactive;

        Color lunaDarkest = Color.FromArgb(1, 28, 64);
        Color lunaTeal = Color.FromArgb(38, 101, 140);
        Color lunaCyan = Color.FromArgb(84, 172, 191);
        Color lunaLight = Color.FromArgb(167, 235, 242);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern Int32 SendMessage(IntPtr hWnd, int msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);
        [DllImport("user32.dll")] public static extern bool ReleaseCapture();
        [DllImport("user32.dll")] public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }

        public LoginForm()
        {
            InitializeComponent();

            this.Opacity = 1;
            this.DoubleBuffered = true;
            typeof(Panel).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.SetProperty |
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.NonPublic,
                null, pnlMain, new object[] { true });

            InitCachedResources();
            ApplyTheme();
            ApplyRoundedCorners(this, 30);
            SetupHoverEffects();
            SetupDefocusHandlers();

            this.Load += LoginForm_Load;
            this.HandleCreated += (s, e) => SetPlaceholders();
            this.VisibleChanged += LoginForm_VisibleChanged;
            this.FormClosed += (s, e) => DisposeCachedResources();
        }

        public void SetAppContext(AppContext ctx) => _appContext = ctx;

        // ✅ FIXED: Only attach defocus to non-textbox controls
        private void SetupDefocusHandlers()
        {
            // Form and panel background clicks
            this.MouseClick += (s, e) => DefocusAll();
            pnlMain.MouseClick += (s, e) => DefocusAll();

            // Only attach to NON-textbox controls
            btnLogin.MouseClick += (s, e) => DefocusAll();
            lblRegister.MouseClick += (s, e) => DefocusAll();
            btnDarkMode.MouseClick += (s, e) => DefocusAll();
            btnClose.MouseClick += (s, e) => DefocusAll();
            lblHeaderSink.MouseClick += (s, e) => DefocusAll();

            // Textbox lost focus
            txtUsername.LostFocus += (s, e) => { focusedField = ""; pnlMain.Invalidate(); };
            txtPassword.LostFocus += (s, e) => { focusedField = ""; pnlMain.Invalidate(); };
        }

        private void DefocusAll()
        {
            if (this.ActiveControl is TextBox)
            {
                this.ActiveControl = null;
                focusedField = "";
                pnlMain.Invalidate();
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            if (!isSliding) _ = SlidePanelIn();
        }

        private async void LoginForm_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible && !isSliding) await SlidePanelIn();
        }

        private async Task SlidePanelIn()
        {
            if (isSliding) return;
            isSliding = true;
            int targetLeft = this.ClientSize.Width - pnlMain.Width;
            pnlMain.Dock = DockStyle.None;
            pnlMain.Height = this.ClientSize.Height;
            pnlMain.Left = this.ClientSize.Width;
            for (int i = 1; i <= 3; i++)
            {
                pnlMain.Left = this.ClientSize.Width - (pnlMain.Width * i / 3);
                await Task.Delay(5);
            }
            pnlMain.Left = targetLeft;
            pnlMain.Dock = DockStyle.Right;
            isSliding = false;
        }

        private async Task SlidePanelOut()
        {
            if (isSliding) return;
            isSliding = true;
            pnlMain.Dock = DockStyle.None;
            pnlMain.Height = this.ClientSize.Height;
            int startLeft = pnlMain.Left;
            for (int i = 1; i <= 3; i++)
            {
                pnlMain.Left = startLeft + (pnlMain.Width * i / 3);
                await Task.Delay(5);
            }
            isSliding = false;
        }

        private void InitCachedResources()
        {
            brushPrimary = new SolidBrush(isDarkMode ? Color.White : lunaDarkest);
            brushSecondary = new SolidBrush(isDarkMode ? lunaCyan : lunaTeal);
            penActive = new Pen(lunaCyan, 2.5f);
            penInactive = new Pen(Color.FromArgb(80, lunaCyan), 1.5f);
        }

        private void DisposeCachedResources()
        {
            brushPrimary?.Dispose();
            brushSecondary?.Dispose();
            penActive?.Dispose();
            penInactive?.Dispose();
        }

        private void SetPlaceholders()
        {
            SendMessage(txtUsername.Handle, 0x1501, 0, "Student ID");
            SendMessage(txtPassword.Handle, 0x1501, 0, "Password");
        }

        private void ApplyTheme()
        {
            brushPrimary.Color = isDarkMode ? Color.White : lunaDarkest;
            brushSecondary.Color = isDarkMode ? lunaCyan : lunaTeal;
            this.BackgroundImage = isDarkMode ? ImageCache.BgDark : ImageCache.BgLight;
            pnlMain.BackColor = isDarkMode ? Color.FromArgb(230, 1, 28, 64) : Color.FromArgb(230, 255, 255, 255);
            Color solidBg = isDarkMode ? Color.FromArgb(1, 28, 64) : Color.White;
            txtUsername.BackColor = txtPassword.BackColor = solidBg;
            txtUsername.ForeColor = txtPassword.ForeColor = isDarkMode ? Color.White : Color.Black;
            lblRegister.ForeColor = isDarkMode ? lunaCyan : lunaTeal;
            btnDarkMode.ForeColor = isDarkMode ? lunaLight : lunaCyan;
            btnDarkMode.Text = isDarkMode ? "LIGHT MODE" : "DARK MODE";
            btnLogin.BackColor = lunaTeal;
            btnLogin.ForeColor = Color.White;
            pnlMain.Invalidate();
        }

        private void SetupHoverEffects()
        {
            lblRegister.MouseEnter += (s, e) => lblRegister.ForeColor = isDarkMode ? lunaLight : lunaDarkest;
            lblRegister.MouseLeave += (s, e) => lblRegister.ForeColor = isDarkMode ? lunaCyan : lunaTeal;
            btnDarkMode.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btnDarkMode.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnDarkMode.MouseEnter += (s, e) => btnDarkMode.ForeColor = isDarkMode ? Color.White : Color.Black;
            btnDarkMode.MouseLeave += (s, e) => btnDarkMode.ForeColor = isDarkMode ? lunaLight : lunaCyan;
        }

        private void pnlMain_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            g.DrawString("Hello!", FontTitle, brushPrimary, 40, 40);
            g.DrawString("Welcome Student", FontSubtitle, brushSecondary, 50, 130);
            g.DrawString("LOGIN", FontHeader, brushPrimary, 55, 220);

            g.DrawLine(focusedField == "user" ? penActive : penInactive, 55, 295, 325, 295);
            g.DrawLine(focusedField == "pass" ? penActive : penInactive, 55, 365, 325, 365);

            using (GraphicsPath btnPath = new GraphicsPath())
            {
                int r = btnLogin.Height;
                btnPath.AddArc(0, 0, r, r, 90, 180);
                btnPath.AddArc(btnLogin.Width - r, 0, r, r, 270, 180);
                btnPath.CloseFigure();
                btnLogin.Region = new Region(btnPath);
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Please enter your Student ID.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                return;
            }
            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter your password.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string query = "SELECT Role FROM Users WHERE Username = @username AND Password = @password";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password);
                        object result = cmd.ExecuteScalar();

                        if (result != null)
                        {
                            string role = result.ToString();
                            if (role == "Student")
                            {
                                NavigatedAway = true;
                                this.Close();
                                _appContext?.OpenHomeForm(_connectionString, username);
                            }
                            else if (role == "Admin")
                            {
                                MessageBox.Show("Admin login successful! Admin panel coming soon.", "Welcome Admin", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Invalid Student ID or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtPassword.Clear();
                            txtPassword.Focus();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Login error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void lblRegister_Click(object sender, EventArgs e)
        {
            await SlidePanelOut();
            this.Hide();
            RegisterForm reg = new RegisterForm(_connectionString);
            reg.StartPosition = FormStartPosition.Manual;
            reg.Location = this.Location;
            reg.FormClosed += (s, args) => { this.Location = reg.Location; this.Show(); };
            reg.Show();
        }

        private void HandleDragging(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, 0xA1, 0x2, 0);
            }
        }

        private void Field_Enter(object sender, EventArgs e)
        {
            focusedField = (sender == txtUsername) ? "user" : "pass";
            pnlMain.Invalidate();
        }

        private void Field_Leave(object sender, EventArgs e)
        {
            focusedField = "";
            pnlMain.Invalidate();
        }

        private void btnClose_Click(object sender, EventArgs e) => Application.Exit();

        private void btnDarkMode_Click(object sender, EventArgs e)
        {
            isDarkMode = !isDarkMode;
            ApplyTheme();
            SetPlaceholders();
        }

        private void ApplyRoundedCorners(Control ctrl, int radius)
        {
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddArc(0, 0, radius, radius, 180, 90);
                path.AddArc(ctrl.Width - radius, 0, radius, radius, 270, 90);
                path.AddArc(ctrl.Width - radius, ctrl.Height - radius, radius, radius, 0, 90);
                path.AddArc(0, ctrl.Height - radius, radius, radius, 90, 90);
                path.CloseFigure();
                ctrl.Region = new Region(path);
            }
        }
    }
}