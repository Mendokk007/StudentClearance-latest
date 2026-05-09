using Microsoft.AspNet.SignalR.Client;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CarDealership
{
    public partial class AdminFormSubjects : Form
    {
        private readonly string _connectionString;
        private readonly string _username;
        private readonly string _assignedSubject;
        private IHubProxy _hubProxy;
        private HubConnection _hubConnection;
        private Timer _notificationTimer;
        private bool _signalRConnected = false;
        private AppContext _appContext;
        // Activity Log Panel
        private Panel pnlActivityLog;
        private ListBox lstActivityLogs;
        private Label lblActivityLogTitle;
        private Button btnCloseActivityLog;
        private Button btnViewActivityLog;

        // Luna Theme
        private bool isDarkMode = true;
        Color lunaDarkest = Color.FromArgb(1, 28, 64);
        Color lunaTeal = Color.FromArgb(38, 101, 140);
        Color lunaCyan = Color.FromArgb(84, 172, 191);
        Color lunaLight = Color.FromArgb(167, 235, 242);
        private Button btnThemeToggle;

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }

        public AdminFormSubjects(string connectionString, string username, string assignedSubject)
        {
            this.DoubleBuffered = true;
            InitializeComponent();
            _connectionString = connectionString;
            _username = username;
            _assignedSubject = assignedSubject;

            this.Text = $"Instructor Dashboard - {_assignedSubject}";
            lblSubject.Text = _assignedSubject;
            lblInstructorName.Text = $"Welcome, {_username}";

            ApplyRoundedCorners(this, 30);
            AddThemeToggleButton();

            MakeDraggable(pnlTopBar);
            MakeDraggable(pbLogo);
            MakeDraggable(lblSubject);
            MakeDraggable(lblInstructorName);

            SetupLogo();
            SetupActivityLogPanel();
            ApplyTheme();
            MakeGridTransparent();
            InitializeSignalR();
            LoadPendingSubmissions();
            SetupNotificationTimer();
        }

        public void SetAppContext(AppContext ctx)
        {
            _appContext = ctx;
        }

        private void MakeDraggable(Control control)
        {
            if (control == null) return;
            control.MouseDown += (sender, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    ReleaseCapture();
                    SendMessage(this.Handle, 0xA1, 0x2, 0);
                }
            };
        }

        private void AddThemeToggleButton()
        {
            btnThemeToggle = new Button();
            btnThemeToggle.BackColor = Color.Transparent;
            btnThemeToggle.Cursor = Cursors.Hand;
            btnThemeToggle.FlatAppearance.BorderSize = 0;
            btnThemeToggle.FlatStyle = FlatStyle.Flat;
            btnThemeToggle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnThemeToggle.ForeColor = Color.White;
            btnThemeToggle.Location = new Point(720, 18);
            btnThemeToggle.Size = new Size(50, 35);
            btnThemeToggle.Text = "🌙";
            btnThemeToggle.UseVisualStyleBackColor = false;
            btnThemeToggle.Click += (s, e) => {
                isDarkMode = !isDarkMode;
                ApplyTheme();
                btnThemeToggle.Text = isDarkMode ? "🌙" : "☀️";
            };
            pnlTopBar.Controls.Add(btnThemeToggle);
        }

        private void ApplyTheme()
        {
            this.SuspendLayout();
            pnlTopBar.SuspendLayout();
            pnlContent.SuspendLayout();

            try
            {
                // Background
                try
                {
                    string bgPath = isDarkMode
                        ? Path.Combine(Application.StartupPath, "home_bg2.jpg")
                        : Path.Combine(Application.StartupPath, "home_bg.jpg");

                    if (File.Exists(bgPath))
                        this.BackgroundImage = Image.FromFile(bgPath);
                    else
                        this.BackgroundImage = CreateSolidBackground(isDarkMode ? lunaDarkest : Color.FromArgb(240, 248, 255));
                    this.BackgroundImageLayout = ImageLayout.Stretch;
                }
                catch
                {
                    this.BackgroundImage = CreateSolidBackground(isDarkMode ? lunaDarkest : Color.FromArgb(240, 248, 255));
                    this.BackgroundImageLayout = ImageLayout.Stretch;
                }

                // Top bar
                pnlTopBar.BackColor = isDarkMode ? Color.FromArgb(200, 1, 28, 64) : Color.FromArgb(200, 255, 255, 255);

                // Content panel
                pnlContent.BackColor = Color.Transparent;

                // Colors
                Color textColor = isDarkMode ? Color.White : lunaDarkest;
                Color accentColor = lunaCyan;

                // Labels
                lblPendingTitle.ForeColor = textColor;
                lblPendingTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
                lblPendingCount.ForeColor = accentColor;
                lblSubject.ForeColor = accentColor;
                lblInstructorName.ForeColor = isDarkMode ? Color.FromArgb(185, 187, 190) : Color.FromArgb(80, 80, 80);

                // Buttons
                btnRefresh.BackColor = lunaTeal;
                btnRefresh.ForeColor = Color.White;
                btnRefresh.FlatStyle = FlatStyle.Flat;
                btnRefresh.FlatAppearance.BorderSize = 0;
                ApplyRoundedCornersToButton(btnRefresh, 15);

                btnLogout.BackColor = lunaTeal;
                btnLogout.ForeColor = Color.White;
                btnLogout.FlatStyle = FlatStyle.Flat;
                btnLogout.FlatAppearance.BorderSize = 0;
                ApplyRoundedCornersToButton(btnLogout, 15);

                // Theme toggle
                if (btnThemeToggle != null)
                {
                    btnThemeToggle.ForeColor = textColor;
                    ApplyRoundedCornersToButton(btnThemeToggle, 15);
                }

                // Logo
                SetupLogo();

                // Activity log panel theme
                if (pnlActivityLog != null)
                {
                    pnlActivityLog.BackColor = isDarkMode ? Color.FromArgb(1, 28, 64) : Color.FromArgb(240, 248, 255);
                    lblActivityLogTitle.ForeColor = isDarkMode ? lunaLight : lunaTeal;
                    btnCloseActivityLog.ForeColor = isDarkMode ? lunaLight : lunaTeal;
                    lstActivityLogs.BackColor = isDarkMode ? Color.FromArgb(1, 28, 64) : Color.FromArgb(255, 255, 255);
                    lstActivityLogs.ForeColor = isDarkMode ? lunaLight : lunaDarkest;
                    if (btnViewActivityLog != null)
                        btnViewActivityLog.ForeColor = textColor;
                }

                // DataGridView — transparent with Luna styling
                dgvSubmissions.BackgroundColor = isDarkMode ? Color.FromArgb(1, 28, 64) : Color.FromArgb(240, 240, 245);
                dgvSubmissions.GridColor = isDarkMode ? Color.FromArgb(30, 60, 100) : Color.FromArgb(200, 210, 220);
                dgvSubmissions.ColumnHeadersDefaultCellStyle.BackColor = lunaTeal;
                dgvSubmissions.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgvSubmissions.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
                dgvSubmissions.ColumnHeadersDefaultCellStyle.SelectionBackColor = lunaTeal;
                dgvSubmissions.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;

                // Row styling — fixes the gray selection issue
                dgvSubmissions.DefaultCellStyle.BackColor = isDarkMode ? Color.FromArgb(1, 28, 64) : Color.White;
                dgvSubmissions.DefaultCellStyle.ForeColor = textColor;
                dgvSubmissions.DefaultCellStyle.SelectionBackColor = lunaCyan;
                dgvSubmissions.DefaultCellStyle.SelectionForeColor = Color.White;
                dgvSubmissions.DefaultCellStyle.Font = new Font("Segoe UI", 10F);

                // Also set RowTemplate for any dynamically added rows
                dgvSubmissions.RowTemplate.DefaultCellStyle.BackColor = isDarkMode ? Color.FromArgb(1, 28, 64) : Color.White;
                dgvSubmissions.RowTemplate.DefaultCellStyle.ForeColor = textColor;
                dgvSubmissions.RowTemplate.DefaultCellStyle.SelectionBackColor = lunaCyan;
                dgvSubmissions.RowTemplate.DefaultCellStyle.SelectionForeColor = Color.White;

                dgvSubmissions.EnableHeadersVisualStyles = false;
                dgvSubmissions.BorderStyle = BorderStyle.None;
                dgvSubmissions.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            }
            finally
            {
                pnlContent.ResumeLayout(false);
                pnlContent.PerformLayout();
                pnlTopBar.ResumeLayout(false);
                pnlTopBar.PerformLayout();
                this.ResumeLayout(false);
            }
        }

        private Image CreateSolidBackground(Color color)
        {
            Bitmap bmp = new Bitmap(900, 600);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(color);
            }
            return bmp;
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

        private void ApplyRoundedCornersToButton(Button btn, int radius)
        {
            using (GraphicsPath path = new GraphicsPath())
            {
                int r = radius;
                path.AddArc(0, 0, r, r, 180, 90);
                path.AddArc(btn.Width - r, 0, r, r, 270, 90);
                path.AddArc(btn.Width - r, btn.Height - r, r, r, 0, 90);
                path.AddArc(0, btn.Height - r, r, r, 90, 90);
                path.CloseFigure();
                btn.Region = new Region(path);
            }
        }

        private void MakeGridTransparent()
        {
            // Use solid color instead of transparent
            dgvSubmissions.BackgroundColor = isDarkMode ? Color.FromArgb(1, 28, 64) : Color.FromArgb(240, 240, 245);
            dgvSubmissions.GridColor = isDarkMode ? Color.FromArgb(30, 60, 100) : Color.FromArgb(200, 210, 220);

            typeof(DataGridView).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.SetProperty |
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.NonPublic,
                null, dgvSubmissions, new object[] { true });
        }

        private void SetupLogo()
        {
            var bmp = new Bitmap(100, 50);
            using (var g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Transparent);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (var font = new Font("Segoe UI", 10, FontStyle.Bold))
                using (var brush = new SolidBrush(isDarkMode ? lunaLight : lunaDarkest))
                {
                    var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                    g.DrawString("INSTRUCTOR", font, brush, new Rectangle(0, 0, 100, 25), sf);
                    g.DrawString("PANEL", new Font("Segoe UI", 8, FontStyle.Bold), brush, new Rectangle(0, 25, 100, 25), sf);
                }
            }
            pbLogo.Image = bmp;
        }

        private void SetupActivityLogPanel()
        {
            pnlActivityLog = new Panel();
            pnlActivityLog.BackColor = isDarkMode ? Color.FromArgb(1, 28, 64) : Color.FromArgb(240, 248, 255);
            pnlActivityLog.Size = new Size(320, 420);
            pnlActivityLog.Location = new Point(this.Width - 340, 80);
            pnlActivityLog.Visible = false;
            pnlActivityLog.BorderStyle = BorderStyle.None;
            ApplyRoundedCorners(pnlActivityLog, 15);

            lblActivityLogTitle = new Label();
            lblActivityLogTitle.Text = "Activity Log";
            lblActivityLogTitle.Font = new Font("Segoe UI Semibold", 14, FontStyle.Bold);
            lblActivityLogTitle.ForeColor = isDarkMode ? lunaLight : lunaTeal;
            lblActivityLogTitle.Location = new Point(15, 15);
            lblActivityLogTitle.Size = new Size(200, 30);
            pnlActivityLog.Controls.Add(lblActivityLogTitle);

            btnCloseActivityLog = new Button();
            btnCloseActivityLog.Text = "✕";
            btnCloseActivityLog.BackColor = Color.Transparent;
            btnCloseActivityLog.ForeColor = isDarkMode ? lunaLight : lunaTeal;
            btnCloseActivityLog.FlatStyle = FlatStyle.Flat;
            btnCloseActivityLog.FlatAppearance.BorderSize = 0;
            btnCloseActivityLog.Size = new Size(35, 30);
            btnCloseActivityLog.Location = new Point(270, 15);
            btnCloseActivityLog.Cursor = Cursors.Hand;
            btnCloseActivityLog.Font = new Font("Segoe UI", 12F);
            btnCloseActivityLog.Click += (s, e) => pnlActivityLog.Visible = false;
            pnlActivityLog.Controls.Add(btnCloseActivityLog);

            lstActivityLogs = new ListBox();
            lstActivityLogs.BackColor = isDarkMode ? Color.FromArgb(1, 28, 64) : Color.FromArgb(255, 255, 255);
            lstActivityLogs.ForeColor = isDarkMode ? lunaLight : lunaDarkest;
            lstActivityLogs.BorderStyle = BorderStyle.None;
            lstActivityLogs.Location = new Point(15, 55);
            lstActivityLogs.Size = new Size(290, 350);
            lstActivityLogs.DrawMode = DrawMode.OwnerDrawVariable;
            lstActivityLogs.MeasureItem += LstActivityLogs_MeasureItem;
            lstActivityLogs.DrawItem += LstActivityLogs_DrawItem;
            pnlActivityLog.Controls.Add(lstActivityLogs);

            btnViewActivityLog = new Button();
            btnViewActivityLog.Text = "📋";
            btnViewActivityLog.Font = new Font("Segoe UI", 14);
            btnViewActivityLog.BackColor = Color.Transparent;
            btnViewActivityLog.ForeColor = isDarkMode ? Color.White : lunaDarkest;
            btnViewActivityLog.FlatStyle = FlatStyle.Flat;
            btnViewActivityLog.FlatAppearance.BorderSize = 0;
            btnViewActivityLog.FlatAppearance.MouseOverBackColor = Color.FromArgb(64, 68, 75);
            btnViewActivityLog.Size = new Size(45, 40);
            btnViewActivityLog.Location = new Point(665, 15);
            btnViewActivityLog.Cursor = Cursors.Hand;
            btnViewActivityLog.Click += (s, e) => {
                pnlActivityLog.Visible = !pnlActivityLog.Visible;
                if (pnlActivityLog.Visible)
                {
                    pnlActivityLog.BringToFront();
                    LoadActivityLogs();
                }
            };
            pnlTopBar.Controls.Add(btnViewActivityLog);

            this.Controls.Add(pnlActivityLog);
            pnlActivityLog.BringToFront();
        }

        private void LstActivityLogs_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            if (e.Index < 0 || lstActivityLogs.Items.Count == 0) return;
            var log = lstActivityLogs.Items[e.Index] as dynamic;
            if (log == null) { e.ItemHeight = 50; return; }
            string message = log.Message?.ToString() ?? "";
            using (var g = lstActivityLogs.CreateGraphics())
            {
                SizeF size = g.MeasureString(message, lstActivityLogs.Font, lstActivityLogs.Width - 15);
                e.ItemHeight = Math.Max(50, (int)size.Height + 28);
            }
        }

        private void LstActivityLogs_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;
            e.DrawBackground();
            var log = lstActivityLogs.Items[e.Index] as dynamic;
            if (log == null) return;
            string message = log.Message?.ToString() ?? "";
            string date = ((DateTime)log.CreatedAt).ToString("MMM dd, HH:mm");
            using (var brush = new SolidBrush(e.ForeColor))
            {
                RectangleF rect = new RectangleF(e.Bounds.X + 5, e.Bounds.Y + 5, e.Bounds.Width - 10, e.Bounds.Height - 28);
                e.Graphics.DrawString(message, e.Font, brush, rect);
            }
            using (var smallFont = new Font("Segoe UI", 8))
            using (var grayBrush = new SolidBrush(Color.FromArgb(185, 187, 190)))
            {
                e.Graphics.DrawString(date, smallFont, grayBrush, e.Bounds.X + 5, e.Bounds.Bottom - 20);
            }
            e.DrawFocusRectangle();
        }

        private async void InitializeSignalR()
        {
            try
            {
                _hubConnection = new HubConnection("http://localhost:8080/");
                _hubProxy = _hubConnection.CreateHubProxy("clearanceHub");

                _hubProxy.On<string, string>("newSubjectSubmission", (message, studentUsername) =>
                {
                    if (!this.IsDisposed)
                    {
                        this.Invoke((Action)(() =>
                        {
                            LoadPendingSubmissions();
                            notifyIcon1.ShowBalloonTip(3000, "Clearance System", message, ToolTipIcon.Info);
                        }));
                    }
                });

                _hubConnection.Closed += () =>
                {
                    if (!this.IsDisposed)
                    {
                        this.Invoke((Action)(() =>
                        {
                            Console.WriteLine("SignalR connection closed for instructor");
                            _signalRConnected = false;
                            SetupNotificationTimer();
                        }));
                    }
                };

                await _hubConnection.Start();
                _signalRConnected = true;
                await _hubProxy.Invoke("JoinInstructorGroup", _assignedSubject);
                Console.WriteLine($"Instructor {_username} connected to SignalR for subject {_assignedSubject}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("SignalR Error: " + ex.Message);
                _signalRConnected = false;
                SetupNotificationTimer();
            }
        }

        private void SetupNotificationTimer()
        {
            if (_notificationTimer == null)
            {
                _notificationTimer = new Timer();
                _notificationTimer.Interval = 10000;
                _notificationTimer.Tick += (s, e) => LoadPendingSubmissions();
            }
            _notificationTimer.Start();
        }

        private void LoadPendingSubmissions()
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("sp_GetPendingSubjectSubmissions", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@SubjectName", _assignedSubject);

                        var dt = new DataTable();
                        using (var adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }

                        if (dgvSubmissions.InvokeRequired)
                        {
                            dgvSubmissions.Invoke((Action)(() => UpdateDataGridView(dt)));
                        }
                        else
                        {
                            UpdateDataGridView(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading subject submissions: " + ex.Message);
            }
        }

        private void LoadActivityLogs()
        {
            try
            {
                lstActivityLogs.Items.Clear();
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("sp_GetActivityLogs", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Username", _username);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                lstActivityLogs.Items.Add(new
                                {
                                    Message = reader["Message"].ToString(),
                                    LogType = reader["LogType"].ToString(),
                                    CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine("Error loading activity logs: " + ex.Message); }
        }

        private void UpdateDataGridView(DataTable dt)
        {
            dgvSubmissions.Columns.Clear();
            dgvSubmissions.DataSource = dt;

            if (dgvSubmissions.Columns.Contains("ImageData"))
                dgvSubmissions.Columns["ImageData"].Visible = false;
            if (dgvSubmissions.Columns.Contains("SubmissionID"))
                dgvSubmissions.Columns["SubmissionID"].Visible = false;

            if (dgvSubmissions.Columns.Contains("StudentUsername"))
                dgvSubmissions.Columns["StudentUsername"].HeaderText = "Username";
            if (dgvSubmissions.Columns.Contains("StudentName"))
                dgvSubmissions.Columns["StudentName"].HeaderText = "Full Name";
            if (dgvSubmissions.Columns.Contains("StudentProgram"))
                dgvSubmissions.Columns["StudentProgram"].HeaderText = "Program";
            if (dgvSubmissions.Columns.Contains("ImageFileName"))
                dgvSubmissions.Columns["ImageFileName"].HeaderText = "File";
            if (dgvSubmissions.Columns.Contains("SubmittedAt"))
                dgvSubmissions.Columns["SubmittedAt"].HeaderText = "Submitted";
            if (dgvSubmissions.Columns.Contains("Status"))
                dgvSubmissions.Columns["Status"].HeaderText = "Status";

            lblPendingCount.Text = $"({dt.Rows.Count})";
        }

        private void dgvSubmissions_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dgvSubmissions.Rows[e.RowIndex];
                int submissionId = Convert.ToInt32(row.Cells["SubmissionID"].Value);
                string studentName = row.Cells["StudentName"].Value?.ToString() ?? "Unknown";
                string studentUsername = row.Cells["StudentUsername"].Value?.ToString();

                byte[] imageData = null;
                var imageCellValue = row.Cells["ImageData"].Value;

                if (imageCellValue != DBNull.Value && imageCellValue != null)
                {
                    try
                    {
                        imageData = (byte[])imageCellValue;
                        using (var ms = new MemoryStream(imageData))
                        {
                            Image.FromStream(ms);
                        }
                    }
                    catch
                    {
                        imageData = null;
                    }
                }

                var reviewForm = new ReviewSubjectForm(_connectionString, submissionId,
                    studentName, _assignedSubject, imageData, _username);
                reviewForm.OnReviewComplete += () =>
                {
                    LoadPendingSubmissions();

                    if (_signalRConnected && _hubProxy != null && !string.IsNullOrEmpty(studentUsername))
                    {
                        try
                        {
                            _hubProxy.Invoke("NotifySubjectStatusUpdate", studentUsername, _assignedSubject, "Reviewed");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("SignalR notification failed: " + ex.Message);
                        }
                    }
                };
                reviewForm.ShowDialog();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadPendingSubmissions();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to logout?", "Confirm Logout",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    if (_hubConnection != null && _hubConnection.State == Microsoft.AspNet.SignalR.Client.ConnectionState.Connected)
                    {
                        _hubProxy?.Invoke("LeaveInstructorGroup", _assignedSubject);
                        _hubConnection.Stop();
                        _hubConnection = null;
                    }
                }
                catch { }

                if (_notificationTimer != null)
                {
                    _notificationTimer.Stop();
                    _notificationTimer.Dispose();
                    _notificationTimer = null;
                }

                this.Close();
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            try
            {
                if (_hubConnection != null)
                {
                    _hubConnection.Stop();
                    _hubConnection = null;
                }
            }
            catch { }

            if (_notificationTimer != null)
            {
                _notificationTimer.Stop();
                _notificationTimer.Dispose();
                _notificationTimer = null;
            }

            base.OnFormClosing(e);
        }
    }
}