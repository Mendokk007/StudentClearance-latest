using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace CarDealership
{
    public partial class RegisterForm : Form
    {
        private readonly string _connectionString;
        private string focusedField = "";
        private bool isDarkMode = true;
        private bool isSliding = false;
        private string _reservedStudentID = null;

        private static readonly Font FontTitle = new Font("Segoe UI Semibold", 52F);
        private static readonly Font FontSubtitle = new Font("Segoe UI", 18F);
        private static readonly Font FontHeader = new Font("Segoe UI", 16, FontStyle.Bold);
        private SolidBrush brushPrimary;
        private SolidBrush brushSecondary;
        private Pen penActive;
        private Pen penInactive;

        private Color lunaDarkest = Color.FromArgb(1, 28, 64);
        private Color lunaTeal = Color.FromArgb(38, 101, 140);
        private Color lunaCyan = Color.FromArgb(84, 172, 191);
        private Color lunaLight = Color.FromArgb(167, 235, 242);

        [DllImport("user32.dll")] public static extern bool ReleaseCapture();
        [DllImport("user32.dll")] public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern Int32 SendMessage(IntPtr hWnd, int msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }

        public RegisterForm(string conn)
        {
            this.SuspendLayout();
            InitializeComponent();
            _connectionString = conn;

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

            txtRegUsername.ReadOnly = true;
            txtRegUsername.TabStop = false;
            LoadPreviewStudentID();

            this.ResumeLayout(false);
            this.Load += RegisterForm_Load;
            this.HandleCreated += (s, e) => SetPlaceholders();
            this.FormClosed += (s, e) => DisposeCachedResources();
        }

        private void LoadPreviewStudentID()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string query = "SELECT LastNumber + 1 FROM StudentIDCounter";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        int nextNumber = (int)cmd.ExecuteScalar();
                        _reservedStudentID = FormatStudentID(nextNumber);
                        txtRegUsername.Text = _reservedStudentID;
                    }
                }
            }
            catch (Exception ex)
            {
                txtRegUsername.Text = "Error loading ID";
                Console.WriteLine("Error previewing Student ID: " + ex.Message);
            }
        }

        private string FormatStudentID(int number)
        {
            if (number < 10) return "STUD00" + number;
            else if (number < 100) return "STUD0" + number;
            else return "STUD" + number;
        }

        private string GenerateStudentID(SqlConnection conn, SqlTransaction transaction)
        {
            using (SqlCommand cmd = new SqlCommand("sp_GenerateStudentID", conn, transaction))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                var outputParam = new SqlParameter("@NewStudentID", SqlDbType.NVarChar, 50);
                outputParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outputParam);
                cmd.ExecuteNonQuery();
                return outputParam.Value.ToString();
            }
        }

        private void SetupDefocusHandlers()
        {
            this.MouseClick += (s, e) => DefocusAll();
            pnlMain.MouseClick += (s, e) => DefocusAll();

            btnRegister.MouseClick += (s, e) => DefocusAll();
            lblBackToLogin.MouseClick += (s, e) => DefocusAll();
            btnDarkMode.MouseClick += (s, e) => DefocusAll();
            btnClose.MouseClick += (s, e) => DefocusAll();
            lblStudentIDNote.MouseClick += (s, e) => DefocusAll();

            txtRegPassword.LostFocus += (s, e) => { focusedField = ""; pnlMain.Invalidate(); };
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

        private void RegisterForm_Load(object sender, EventArgs e)
        {
            if (!isSliding) _ = SlidePanelIn();
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

        private void ApplyTheme()
        {
            brushPrimary.Color = isDarkMode ? Color.White : lunaDarkest;
            brushSecondary.Color = isDarkMode ? lunaCyan : lunaTeal;
            this.BackgroundImage = isDarkMode ? ImageCache.BgDark : ImageCache.BgLight;
            pnlMain.BackColor = isDarkMode ? Color.FromArgb(225, 1, 28, 64) : Color.FromArgb(225, 255, 255, 255);
            Color solidBg = isDarkMode ? Color.FromArgb(1, 28, 64) : Color.White;

            txtRegUsername.BackColor = isDarkMode ? Color.FromArgb(10, 35, 70) : Color.FromArgb(240, 240, 240);
            txtRegPassword.BackColor = solidBg;
            cboProgram.BackColor = solidBg;
            cboProgram.ForeColor = isDarkMode ? Color.White : Color.Black;
            txtRegUsername.ForeColor = txtRegPassword.ForeColor = isDarkMode ? Color.White : Color.Black;

            lblStudentIDNote.ForeColor = isDarkMode ? Color.FromArgb(185, 187, 190) : Color.FromArgb(100, 100, 100);

            lblBackToLogin.ForeColor = isDarkMode ? lunaCyan : lunaTeal;
            btnRegister.BackColor = lunaTeal;
            btnRegister.ForeColor = Color.White;
            btnDarkMode.ForeColor = isDarkMode ? lunaLight : lunaCyan;
            btnDarkMode.Text = isDarkMode ? "LIGHT MODE" : "DARK MODE";
            btnClose.ForeColor = isDarkMode ? Color.White : lunaDarkest;
            pnlMain.Invalidate();
        }

        private void SetupHoverEffects()
        {
            lblBackToLogin.MouseEnter += (s, e) => lblBackToLogin.ForeColor = isDarkMode ? lunaLight : lunaDarkest;
            lblBackToLogin.MouseLeave += (s, e) => lblBackToLogin.ForeColor = isDarkMode ? lunaCyan : lunaTeal;
            btnDarkMode.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btnDarkMode.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnDarkMode.MouseEnter += (s, e) => btnDarkMode.ForeColor = isDarkMode ? Color.White : Color.Black;
            btnDarkMode.MouseLeave += (s, e) => btnDarkMode.ForeColor = isDarkMode ? lunaLight : lunaCyan;
        }

        private void pnlMain_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            e.Graphics.DrawString("Create", FontTitle, brushPrimary, 40, 40);
            e.Graphics.DrawString("Student Account", FontSubtitle, brushSecondary, 50, 130);
            e.Graphics.DrawString("REGISTER", FontHeader, brushPrimary, 55, 200);

            e.Graphics.DrawLine(penInactive, 55, 275, 325, 275);
            e.Graphics.DrawLine(focusedField == "pass" ? penActive : penInactive, 55, 335, 325, 335);

            using (GraphicsPath btnPath = new GraphicsPath())
            {
                int r = btnRegister.Height;
                btnPath.AddArc(0, 0, r, r, 90, 180);
                btnPath.AddArc(btnRegister.Width - r, 0, r, r, 270, 180);
                btnPath.CloseFigure();
                btnRegister.Region = new Region(btnPath);
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string password = txtRegPassword.Text.Trim();
            string program = cboProgram.SelectedItem?.ToString() ?? "";

            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter a password.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtRegPassword.Focus();
                return;
            }
            if (string.IsNullOrEmpty(program) || program == "Select Program...")
            {
                MessageBox.Show("Please select your program.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboProgram.Focus();
                return;
            }
            if (password.Length < 6)
            {
                MessageBox.Show("Password must be at least 6 characters long.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtRegPassword.Focus();
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    using (SqlTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            string username = GenerateStudentID(conn, transaction);

                            string insertQuery = @"INSERT INTO Users (Username, Password, FullName, Program, Role) 
                                                   VALUES (@username, @password, @fullname, @program, 'Student')";
                            using (SqlCommand insertCmd = new SqlCommand(insertQuery, conn, transaction))
                            {
                                insertCmd.Parameters.AddWithValue("@username", username);
                                insertCmd.Parameters.AddWithValue("@password", password);
                                insertCmd.Parameters.AddWithValue("@fullname", username);
                                insertCmd.Parameters.AddWithValue("@program", program);
                                insertCmd.ExecuteNonQuery();
                            }

                            transaction.Commit();

                            MessageBox.Show($"Registration successful!\nYour Student ID is: {username}\n\nPlease save this ID for login.",
                                "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lblBackToLogin_Click(sender, e);
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Registration failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetPlaceholders()
        {
            SendMessage(txtRegPassword.Handle, 0x1501, 0, "Password");
        }

        private async void lblBackToLogin_Click(object sender, EventArgs e)
        {
            await SlidePanelOut();
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e) => lblBackToLogin_Click(sender, e);

        private void btnDarkMode_Click(object sender, EventArgs e)
        {
            isDarkMode = !isDarkMode;
            ApplyTheme();
            SetPlaceholders();
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
            if (sender == txtRegUsername) return;
            focusedField = (sender == txtRegPassword) ? "pass" : "";
            pnlMain.Invalidate();
        }

        private void Field_Leave(object sender, EventArgs e)
        {
            focusedField = "";
            pnlMain.Invalidate();
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