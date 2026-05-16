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
        // =============================================
        // FIELDS
        // =============================================
        private readonly string _connectionString;
        private readonly string _username;
        private readonly string _assignedSubject;
        private IHubProxy _hubProxy;
        private HubConnection _hubConnection;
        private Timer _notificationTimer;
        private bool _signalRConnected = false;
        private bool _isCleaningUp = false;
        private AppContext _appContext;

        // Luna Theme
        private bool isDarkMode = true;
        Color lunaDarkest = Color.FromArgb(1, 28, 64);
        Color lunaTeal = Color.FromArgb(38, 101, 140);
        Color lunaCyan = Color.FromArgb(84, 172, 191);
        Color lunaLight = Color.FromArgb(167, 235, 242);
        private Button btnThemeToggle;

        // Activity Log Panel
        private Panel pnlActivityLog;
        private ListBox lstActivityLogs;
        private Label lblActivityLogTitle;
        private Button btnCloseActivityLog;
        private Button btnViewActivityLog;
        private DateTimePicker dtpLogStart;
        private DateTimePicker dtpLogEnd;
        private Button btnDownloadLogs;

        // Win32 API
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

        // =============================================
        // CONSTRUCTOR
        // =============================================
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

        // =============================================
        // HELPER METHODS
        // =============================================
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
            btnThemeToggle = new Button
            {
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(720, 18),
                Size = new Size(50, 35),
                Text = "🌙",
                UseVisualStyleBackColor = false
            };
            btnThemeToggle.FlatAppearance.BorderSize = 0;
            btnThemeToggle.Click += (s, e) =>
            {
                isDarkMode = !isDarkMode;
                ApplyTheme();
                btnThemeToggle.Text = isDarkMode ? "🌙" : "☀️";
            };
            pnlTopBar.Controls.Add(btnThemeToggle);
        }

        // =============================================
        // LOGO
        // =============================================
        private void SetupLogo()
        {
            string logoPath = isDarkMode
                ? Path.Combine(Application.StartupPath, "logo2.png")
                : Path.Combine(Application.StartupPath, "logo.png");

            if (!File.Exists(logoPath))
            {
                string logosFolder = Path.Combine(Application.StartupPath, "Logos");
                logoPath = isDarkMode
                    ? Path.Combine(logosFolder, "logo2.png")
                    : Path.Combine(logosFolder, "logo.png");
            }

            if (File.Exists(logoPath))
            {
                try
                {
                    using (var fs = new FileStream(logoPath, FileMode.Open, FileAccess.Read))
                    using (var original = Image.FromStream(fs))
                    {
                        var resized = new Bitmap(100, 50);
                        using (var g = Graphics.FromImage(resized))
                        {
                            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            g.DrawImage(original, 0, 0, 100, 50);
                        }
                        pbLogo.Image = resized;
                    }
                }
                catch
                {
                    CreateDefaultLogo();
                }
            }
            else
            {
                CreateDefaultLogo();
            }
        }

        private void CreateDefaultLogo()
        {
            var bmp = new Bitmap(100, 50);
            using (var g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Transparent);
                g.SmoothingMode = SmoothingMode.AntiAlias;

                using (var font = new Font("Segoe UI", 10, FontStyle.Bold))
                using (var brush = new SolidBrush(isDarkMode ? lunaLight : lunaDarkest))
                {
                    var sf = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };
                    g.DrawString("INSTRUCTOR", font, brush, new Rectangle(0, 0, 100, 25), sf);
                    g.DrawString("PANEL", new Font("Segoe UI", 8, FontStyle.Bold),
                        brush, new Rectangle(0, 25, 100, 25), sf);
                }
            }
            pbLogo.Image = bmp;
        }

        // =============================================
        // ACTIVITY LOG PANEL
        // =============================================
        private void SetupActivityLogPanel()
        {
            // Main panel
            pnlActivityLog = new Panel
            {
                BackColor = isDarkMode ? Color.FromArgb(1, 28, 64) : Color.FromArgb(240, 248, 255),
                Size = new Size(320, 420),
                Location = new Point(this.Width - 340, 80),
                Visible = false,
                BorderStyle = BorderStyle.None
            };
            ApplyRoundedCorners(pnlActivityLog, 15);

            // Title
            lblActivityLogTitle = new Label
            {
                Text = "Activity Log",
                Font = new Font("Segoe UI Semibold", 14, FontStyle.Bold),
                ForeColor = isDarkMode ? lunaLight : lunaTeal,
                Location = new Point(15, 15),
                Size = new Size(200, 30)
            };
            pnlActivityLog.Controls.Add(lblActivityLogTitle);

            // Close button
            btnCloseActivityLog = new Button
            {
                Text = "✕",
                BackColor = Color.Transparent,
                ForeColor = isDarkMode ? lunaLight : lunaTeal,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(35, 30),
                Location = new Point(270, 15),
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 12F)
            };
            btnCloseActivityLog.FlatAppearance.BorderSize = 0;
            btnCloseActivityLog.Click += (s, e) => pnlActivityLog.Visible = false;
            pnlActivityLog.Controls.Add(btnCloseActivityLog);

            // ListBox
            lstActivityLogs = new ListBox
            {
                BackColor = isDarkMode ? Color.FromArgb(1, 28, 64) : Color.FromArgb(255, 255, 255),
                ForeColor = isDarkMode ? lunaLight : lunaDarkest,
                BorderStyle = BorderStyle.None,
                Location = new Point(15, 55),
                Size = new Size(290, 265),
                DrawMode = DrawMode.OwnerDrawVariable
            };
            lstActivityLogs.MeasureItem += (s, e) => { e.ItemHeight = 45; };
            lstActivityLogs.DrawItem += (s, e) =>
            {
                if (e.Index < 0) return;
                e.DrawBackground();
                var log = lstActivityLogs.Items[e.Index] as dynamic;
                if (log == null) return;
                using (var brush = new SolidBrush(e.ForeColor))
                    e.Graphics.DrawString(
                        log.Message + "\n" + ((DateTime)log.CreatedAt).ToString("MMM dd, HH:mm"),
                        e.Font, brush, e.Bounds.X + 5, e.Bounds.Y + 5);
            };
            pnlActivityLog.Controls.Add(lstActivityLogs);

            // Date pickers — smaller & centered
            var lblStart = new Label
            {
                Text = "From:",
                Location = new Point(15, 335),
                Size = new Size(40, 20),
                ForeColor = isDarkMode ? lunaLight : lunaTeal,
                Font = new Font("Segoe UI", 8F)
            };
            pnlActivityLog.Controls.Add(lblStart);

            dtpLogStart = new DateTimePicker
            {
                Location = new Point(55, 333),
                Size = new Size(120, 25),
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Today
            };
            pnlActivityLog.Controls.Add(dtpLogStart);

            var lblEnd = new Label
            {
                Text = "To:",
                Location = new Point(180, 335),
                Size = new Size(25, 20),
                ForeColor = isDarkMode ? lunaLight : lunaTeal,
                Font = new Font("Segoe UI", 8F)
            };
            pnlActivityLog.Controls.Add(lblEnd);

            dtpLogEnd = new DateTimePicker
            {
                Location = new Point(205, 333),
                Size = new Size(120, 25),
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Today
            };
            pnlActivityLog.Controls.Add(dtpLogEnd);

            // Download button
            btnDownloadLogs = new Button
            {
                Text = "Download CSV",
                Location = new Point(15, 368),
                Size = new Size(290, 30),
                BackColor = lunaTeal,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };
            btnDownloadLogs.FlatAppearance.BorderSize = 0;
            btnDownloadLogs.Click += (s, e) => DownloadLogs();
            pnlActivityLog.Controls.Add(btnDownloadLogs);
            ApplyRoundedCornersToButton(btnDownloadLogs, 10);

            // 📋 button in top bar
            btnViewActivityLog = new Button
            {
                Text = "📋",
                Font = new Font("Segoe UI", 14),
                BackColor = Color.Transparent,
                ForeColor = isDarkMode ? Color.White : lunaDarkest,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(45, 40),
                Location = new Point(665, 15),
                Cursor = Cursors.Hand
            };
            btnViewActivityLog.FlatAppearance.BorderSize = 0;
            btnViewActivityLog.FlatAppearance.MouseOverBackColor = lunaDarkest;
            btnViewActivityLog.Click += (s, e) =>
            {
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
            catch (Exception ex)
            {
                Console.WriteLine("Error loading activity logs: " + ex.Message);
            }
        }

        private void DownloadLogs()
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("sp_GetLogsByDateRange", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Username", _username);
                        cmd.Parameters.AddWithValue("@StartDate", dtpLogStart.Value.Date);
                        cmd.Parameters.AddWithValue("@EndDate", dtpLogEnd.Value.Date);

                        var dt = new DataTable();
                        using (var adapter = new SqlDataAdapter(cmd))
                            adapter.Fill(dt);

                        if (dt.Rows.Count == 0)
                        {
                            MessageBox.Show("No logs found for this date range.",
                                "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        using (var sfd = new SaveFileDialog())
                        {
                            sfd.Filter = "CSV File|*.csv";

                            string baseName = $"ActivityLogs_{dtpLogStart.Value:yyyyMMdd}_{dtpLogEnd.Value:yyyyMMdd}";
                            string fileName = baseName + ".csv";
                            int counter = 1;

                            // Increment filename if it already exists
                            while (File.Exists(fileName))
                            {
                                fileName = $"{baseName}_{counter}.csv";
                                counter++;
                            }

                            sfd.FileName = fileName;

                            if (sfd.ShowDialog() == DialogResult.OK)
                            {
                                using (var writer = new StreamWriter(sfd.FileName))
                                {
                                    writer.WriteLine("Date,Type,Message");
                                    foreach (DataRow row in dt.Rows)
                                    {
                                        writer.WriteLine(
                                            $"\"{row["CreatedAt"]}\"," +
                                            $"\"{row["LogType"]}\"," +
                                            $"\"{row["Message"]}\"");
                                    }
                                }
                                MessageBox.Show("Logs downloaded successfully!",
                                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        // =============================================
        // GRID
        // =============================================
        private void MakeGridTransparent()
        {
            dgvSubmissions.BackgroundColor = isDarkMode
                ? Color.FromArgb(1, 28, 64)
                : Color.FromArgb(240, 240, 245);

            dgvSubmissions.GridColor = isDarkMode
                ? Color.FromArgb(30, 60, 100)
                : Color.FromArgb(200, 210, 220);

            typeof(DataGridView).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.SetProperty |
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.NonPublic,
                null, dgvSubmissions, new object[] { true });
        }

        // =============================================
        // THEME
        // =============================================
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
                        this.BackgroundImage = CreateSolidBackground(
                            isDarkMode ? lunaDarkest : Color.FromArgb(240, 248, 255));

                    this.BackgroundImageLayout = ImageLayout.Stretch;
                }
                catch
                {
                    this.BackgroundImage = CreateSolidBackground(
                        isDarkMode ? lunaDarkest : Color.FromArgb(240, 248, 255));
                    this.BackgroundImageLayout = ImageLayout.Stretch;
                }

                // Panels
                pnlTopBar.BackColor = isDarkMode
                    ? Color.FromArgb(200, 1, 28, 64)
                    : Color.FromArgb(200, 255, 255, 255);

                pnlContent.BackColor = Color.Transparent;

                Color textColor = isDarkMode ? Color.White : lunaDarkest;
                Color accentColor = lunaCyan;

                // Labels
                lblPendingTitle.ForeColor = textColor;
                lblPendingTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
                lblPendingCount.ForeColor = accentColor;
                lblSubject.ForeColor = accentColor;
                lblSubject.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
                lblInstructorName.ForeColor = isDarkMode
                    ? Color.FromArgb(185, 187, 190)
                    : Color.FromArgb(80, 80, 80);

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

                if (btnThemeToggle != null)
                {
                    btnThemeToggle.ForeColor = textColor;
                    ApplyRoundedCornersToButton(btnThemeToggle, 15);
                }

                SetupLogo();

                // Activity log panel
                if (pnlActivityLog != null)
                {
                    pnlActivityLog.BackColor = isDarkMode
                        ? Color.FromArgb(1, 28, 64)
                        : Color.FromArgb(240, 248, 255);

                    lblActivityLogTitle.ForeColor = isDarkMode ? lunaLight : lunaTeal;
                    btnCloseActivityLog.ForeColor = isDarkMode ? lunaLight : lunaTeal;

                    lstActivityLogs.BackColor = isDarkMode
                        ? Color.FromArgb(1, 28, 64)
                        : Color.FromArgb(255, 255, 255);

                    lstActivityLogs.ForeColor = isDarkMode ? lunaLight : lunaDarkest;

                    if (btnViewActivityLog != null)
                        btnViewActivityLog.ForeColor = textColor;
                }

                // DataGridView
                dgvSubmissions.BackgroundColor = isDarkMode
                    ? Color.FromArgb(1, 28, 64)
                    : Color.FromArgb(240, 240, 245);

                dgvSubmissions.GridColor = isDarkMode
                    ? Color.FromArgb(30, 60, 100)
                    : Color.FromArgb(200, 210, 220);

                dgvSubmissions.ColumnHeadersDefaultCellStyle.BackColor = lunaTeal;
                dgvSubmissions.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

                dgvSubmissions.DefaultCellStyle.BackColor = isDarkMode
                    ? Color.FromArgb(1, 28, 64) : Color.White;

                dgvSubmissions.DefaultCellStyle.ForeColor = textColor;
                dgvSubmissions.DefaultCellStyle.SelectionBackColor = lunaCyan;
                dgvSubmissions.DefaultCellStyle.SelectionForeColor = Color.White;

                dgvSubmissions.RowTemplate.DefaultCellStyle.BackColor = isDarkMode
                    ? Color.FromArgb(1, 28, 64) : Color.White;

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
            var bmp = new Bitmap(900, 600);
            using (var g = Graphics.FromImage(bmp))
                g.Clear(color);
            return bmp;
        }

        // =============================================
        // ROUNDED CORNERS
        // =============================================
        private void ApplyRoundedCorners(Control ctrl, int radius)
        {
            using (var path = new GraphicsPath())
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
            using (var path = new GraphicsPath())
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

        // =============================================
        // SIGNALR
        // =============================================
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
                    if (this.IsDisposed || _isCleaningUp) return;
                    try
                    {
                        this.Invoke((Action)(() =>
                        {
                            if (!this.IsDisposed)
                            {
                                Console.WriteLine("SignalR connection closed for instructor");
                                _signalRConnected = false;
                                SetupNotificationTimer();
                            }
                        }));
                    }
                    catch (ObjectDisposedException) { }
                };

                await _hubConnection.Start();
                _signalRConnected = true;

                // Join SignalR groups for ALL subjects this teacher covers
                try
                {
                    using (var conn = new SqlConnection(_connectionString))
                    {
                        conn.Open();
                        using (var cmd = new SqlCommand(
                            "SELECT SubjectName FROM TeacherSubjects WHERE TeacherUsername = @Teacher", conn))
                        {
                            cmd.Parameters.AddWithValue("@Teacher", _username);
                            using (var reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    string subject = reader["SubjectName"].ToString();
                                    await _hubProxy.Invoke("JoinInstructorGroup", subject);
                                    Console.WriteLine($"Instructor {_username} joined group for {subject}");
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Fallback: join at least the primary assigned subject
                    await _hubProxy.Invoke("JoinInstructorGroup", _assignedSubject);
                    Console.WriteLine("SignalR group join fallback: " + ex.Message);
                }
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
                _notificationTimer = new Timer { Interval = 10000 };
                _notificationTimer.Tick += (s, e) => LoadPendingSubmissions();
            }
            _notificationTimer.Start();
        }

        // =============================================
        // DATA LOADING
        // =============================================
        private void LoadPendingSubmissions()
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    // Use TeacherUsername for teacher subject load
                    using (var cmd = new SqlCommand("sp_GetPendingSubjectSubmissions", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@TeacherUsername", _username);

                        var dt = new DataTable();
                        using (var adapter = new SqlDataAdapter(cmd))
                            adapter.Fill(dt);

                        if (dgvSubmissions.InvokeRequired)
                            dgvSubmissions.Invoke((Action)(() => UpdateDataGridView(dt)));
                        else
                            UpdateDataGridView(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading submissions: " + ex.Message);
            }
        }

        private void UpdateDataGridView(DataTable dt)
        {
            dgvSubmissions.Columns.Clear();
            dgvSubmissions.DataSource = dt;

            if (dgvSubmissions.Columns.Contains("FileData"))
                dgvSubmissions.Columns["FileData"].Visible = false;
            if (dgvSubmissions.Columns.Contains("SubmissionID"))
                dgvSubmissions.Columns["SubmissionID"].Visible = false;
            if (dgvSubmissions.Columns.Contains("FileType"))
                dgvSubmissions.Columns["FileType"].Visible = false;
            if (dgvSubmissions.Columns.Contains("SubjectName"))
                dgvSubmissions.Columns["SubjectName"].HeaderText = "Subject";

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
                string subjectName = row.Cells["SubjectName"].Value?.ToString() ?? _assignedSubject;

                byte[] fileData = null;
                var fileCellValue = row.Cells["FileData"].Value;
                string fileName = row.Cells["FileName"].Value?.ToString() ?? "";
                string fileType = row.Cells["FileType"]?.Value?.ToString() ?? "";

                if (fileCellValue != DBNull.Value && fileCellValue != null)
                {
                    try { fileData = (byte[])fileCellValue; }
                    catch { fileData = null; }
                }

                if (fileType == "pdf" && fileData != null)
                {
                    string tempPath = Path.Combine(Path.GetTempPath(), fileName);
                    File.WriteAllBytes(tempPath, fileData);
                    try
                    {
                        System.Diagnostics.Process.Start(tempPath);
                    }
                    catch
                    {
                        MessageBox.Show("File saved to:\n" + tempPath, "PDF Saved");
                    }
                }
                else
                {
                    var reviewForm = new ReviewSubjectForm(_connectionString, submissionId,
                        studentName, subjectName, fileData, _username);
                    reviewForm.OnReviewComplete += () =>
                    {
                        LoadPendingSubmissions();
                        if (_signalRConnected && _hubProxy != null && !string.IsNullOrEmpty(studentUsername))
                        {
                            try
                            {
                                _hubProxy.Invoke("NotifySubjectStatusUpdate",
                                    studentUsername, subjectName, "Reviewed");
                            }
                            catch { }
                        }
                    };
                    reviewForm.ShowDialog();
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadPendingSubmissions();
        }

        // =============================================
        // LOGOUT
        // =============================================
        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to logout?", "Confirm Logout",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _isCleaningUp = true;

                try
                {
                    if (_hubConnection != null &&
                        _hubConnection.State == Microsoft.AspNet.SignalR.Client.ConnectionState.Connected)
                    {
                        // Leave ALL subject groups this teacher covers
                        try
                        {
                            using (var conn = new SqlConnection(_connectionString))
                            {
                                conn.Open();
                                using (var cmd = new SqlCommand(
                                    "SELECT SubjectName FROM TeacherSubjects WHERE TeacherUsername = @Teacher", conn))
                                {
                                    cmd.Parameters.AddWithValue("@Teacher", _username);
                                    using (var reader = cmd.ExecuteReader())
                                    {
                                        while (reader.Read())
                                        {
                                            _hubProxy?.Invoke("LeaveInstructorGroup", reader["SubjectName"].ToString());
                                        }
                                    }
                                }
                            }
                        }
                        catch
                        {
                            // Fallback: leave at least the primary subject group
                            _hubProxy?.Invoke("LeaveInstructorGroup", _assignedSubject);
                        }

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
            _isCleaningUp = true;

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