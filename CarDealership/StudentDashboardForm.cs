using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.AspNet.SignalR.Client;
using System.Threading.Tasks;

namespace CarDealership
{
    public partial class StudentDashboardForm : Form
    {
        private readonly string _connectionString;
        private readonly string _username;

        // Subject data
        private readonly List<string> _subjects;
        private readonly Dictionary<string, bool> _subjectStatus;
        private Dictionary<string, Button> _subjectButtons;
        private Dictionary<string, Label> _subjectStatusLabels;
        private List<Panel> _subjectPanels;
        private List<Label> _subjectNameLabels;

        // Department data (from existing HomeForm)
        private readonly Dictionary<string, bool> _deptStatus;
        private readonly Dictionary<string, Button> _deptButtons;
        private readonly Dictionary<string, Label> _deptStatusLabels;

        private readonly OpenFileDialog _openFileDialog;
        private IHubProxy _hubProxy;
        private HubConnection _hubConnection;
        private Timer _subjectRefreshTimer;
        private Timer _deptRefreshTimer;
        private Timer _notificationTimer;
        private bool _isCleaningUp = false;
        private bool _isSliding = false;

        // Theme
        private bool isDarkMode = true;
        Color lunaDarkest = Color.FromArgb(1, 28, 64);
        Color lunaTeal = Color.FromArgb(38, 101, 140);
        Color lunaCyan = Color.FromArgb(84, 172, 191);
        Color lunaLight = Color.FromArgb(167, 235, 242);

        // AppContext
        private AppContext _appContext;
        private bool _loggedOut = false;
        public bool LoggedOut => _loggedOut;

        // Profile/Notifications (shared)
        private PictureBox pbProfilePicture;
        private LinkLabel lnkEditProfile;
        private Label lblUserGreetingText;
        private Panel pnlProfileContainer;
        private NotifyIcon notifyIcon1;
        private Panel pnlNotifications;
        private ListBox lstNotifications;
        private Label lblNotificationTitle;
        private Button btnCloseNotifications;
        private Button btnViewNotifications;
        private Label lblNotificationBadge;
        private int _unreadNotificationCount = 0;
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

        public StudentDashboardForm(string connectionString, string username)
        {
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer | ControlStyles.ResizeRedraw, true);
            this.UpdateStyles();

            InitializeComponent();

            typeof(Panel).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.SetProperty |
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.NonPublic,
                null, pnlTopBar, new object[] { true });
            typeof(Panel).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.SetProperty |
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.NonPublic,
                null, pnlSubjects, new object[] { true });
            typeof(Panel).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.SetProperty |
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.NonPublic,
                null, pnlDepartments, new object[] { true });
            typeof(Panel).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.SetProperty |
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.NonPublic,
                null, pnlCertificate, new object[] { true });

            AddThemeToggleButton();
            ApplyRoundedCorners(this, 30);

            pnlSubjects.AutoScroll = true;

            _connectionString = connectionString;
            _username = username;

            // Initialize subject data
            _subjects = new List<string>();
            _subjectStatus = new Dictionary<string, bool>();
            _subjectButtons = new Dictionary<string, Button>();
            _subjectStatusLabels = new Dictionary<string, Label>();
            _subjectPanels = new List<Panel>();
            _subjectNameLabels = new List<Label>();

            // Initialize department data
            _deptStatus = new Dictionary<string, bool>
    {
        { "Library", false },
        { "SAO", false },
        { "Cashier", false },
        { "Accounting", false },
        { "Dean's Office", false },
        { "Records", false }
    };
            _deptButtons = new Dictionary<string, Button>();
            _deptStatusLabels = new Dictionary<string, Label>();

            _openFileDialog = new OpenFileDialog
            {
                Filter = "All Supported Files|*.jpg;*.jpeg;*.png;*.bmp;*.pdf|Image Files|*.jpg;*.jpeg;*.png;*.bmp|PDF Files|*.pdf|All Files|*.*",
                Title = "Select a file for submission"
            };

            notifyIcon1 = new NotifyIcon
            {
                Icon = SystemIcons.Information,
                Visible = true
            };

            MakeDraggable(pnlTopBar);
            MakeDraggable(pbLogo);
            AddProfileSection();
            SetupNotificationsPanel();
            LoadUserGreeting();
            LoadLogo();

            // Load subjects based on student's program
            LoadSubjectsFromDB();
            MapSubjectControls();
            MapDeptControls();

            ApplyTheme();
            InitializeSignalR();
            SetupNotificationPolling();

            this.Shown += (s, e) => {
                LoadSubjectStatusFromDB();
                LoadDeptStatusFromDB();
                UpdateNotificationBadge();
                UpdateStep2Button();
                UpdateStep3Button();
                SetDefaultPanel();
            };

            btnStep1.Paint += BtnStep1_Paint;
            btnStep2.Paint += BtnStep2_Paint;
            btnStep3.Paint += BtnStep3_Paint;
        }

        // =============================================
        // SUBJECT LOADING
        // =============================================
        private void LoadSubjectsFromDB()
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT s.SubjectName, s.DisplayOrder 
                        FROM Users u
                        INNER JOIN Subjects s ON u.Program = s.ProgramCode
                        WHERE u.Username = @Username
                        ORDER BY s.DisplayOrder";

                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", _username);
                        using (var reader = cmd.ExecuteReader())
                        {
                            int index = 0;
                            while (reader.Read() && index < 6)
                            {
                                string subjectName = reader["SubjectName"].ToString();
                                _subjects.Add(subjectName);
                                _subjectStatus[subjectName] = false;
                                index++;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading subjects: " + ex.Message);
                if (_subjects.Count == 0)
                {
                    string[] fallback = { "Subject 1", "Subject 2", "Subject 3", "Subject 4", "Subject 5", "Subject 6" };
                    _subjects.AddRange(fallback);
                    foreach (var s in _subjects)
                        _subjectStatus[s] = false;
                }
            }
        }

        private void MapSubjectControls()
        {
            // Clear old dynamic panels
            foreach (var panel in _subjectPanels)
            {
                pnlSubjects.Controls.Remove(panel);
                panel.Dispose();
            }
            _subjectPanels.Clear();
            _subjectNameLabels.Clear();
            _subjectButtons.Clear();
            _subjectStatusLabels.Clear();

            // Hide the 6 static panels
            pnlSubject1.Visible = false;
            pnlSubject2.Visible = false;
            pnlSubject3.Visible = false;
            pnlSubject4.Visible = false;
            pnlSubject5.Visible = false;
            pnlSubject6.Visible = false;

            // Create dynamic panels for each subject
            int y = 120;
            for (int i = 0; i < _subjects.Count; i++)
            {
                string subject = _subjects[i];
                int column = i % 2;
                int x = (column == 0) ? 25 : 460;

                if (column == 0 && i > 0) y += 115;

                // Create panel
                var panel = new Panel
                {
                    BackColor = Color.FromArgb(38, 101, 140),
                    Location = new Point(x, y),
                    Size = new Size(415, 95),
                    Name = "pnlDynamicSubject" + i
                };

                // Subject name label
                var lblName = new Label
                {
                    Text = subject,
                    AutoSize = true,
                    Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                    ForeColor = Color.White,
                    Location = new Point(20, 15)
                };

                // Submit button
                var btnSubmit = new Button
                {
                    Text = "Submit",
                    BackColor = Color.FromArgb(38, 101, 140),
                    Cursor = Cursors.Hand,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    ForeColor = Color.White,
                    Location = new Point(290, 28),
                    Size = new Size(110, 40),
                    Name = "btnDynamicSubmit" + i
                };
                btnSubmit.FlatAppearance.BorderSize = 0;

                // Status label
                var lblStatus = new Label
                {
                    Text = "Pending",
                    AutoSize = true,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(84, 172, 191),
                    Location = new Point(20, 45)
                };

                // Wire up click event
                string capturedSubject = subject;
                btnSubmit.Click += (s, e) => HandleSubjectSubmission(capturedSubject, btnSubmit, lblStatus);

                // Add controls to panel
                panel.Controls.Add(lblName);
                panel.Controls.Add(btnSubmit);
                panel.Controls.Add(lblStatus);

                // Add panel to subjects panel
                pnlSubjects.Controls.Add(panel);

                // Store references
                _subjectPanels.Add(panel);
                _subjectNameLabels.Add(lblName);
                _subjectButtons[subject] = btnSubmit;
                _subjectStatusLabels[subject] = lblStatus;
            }
        }

        private void MapDeptControls()
        {
            _deptButtons["Library"] = btnLibrarySubmit;
            _deptButtons["SAO"] = btnSAOSubmit;
            _deptButtons["Cashier"] = btnCashierSubmit;
            _deptButtons["Accounting"] = btnAccountingSubmit;
            _deptButtons["Dean's Office"] = btnDeanSubmit;
            _deptButtons["Records"] = btnRecordsSubmit;

            _deptStatusLabels["Library"] = lblLibraryStatus;
            _deptStatusLabels["SAO"] = lblSAOStatus;
            _deptStatusLabels["Cashier"] = lblCashierStatus;
            _deptStatusLabels["Accounting"] = lblAccountingStatus;
            _deptStatusLabels["Dean's Office"] = lblDeanStatus;
            _deptStatusLabels["Records"] = lblRecordsStatus;
        }

        private (Button submitButton, Label statusLabel) GetSubjectControls(string subject)
        {
            if (_subjectButtons.ContainsKey(subject) && _subjectStatusLabels.ContainsKey(subject))
                return (_subjectButtons[subject], _subjectStatusLabels[subject]);
            return (null, null);
        }

        private (Button submitButton, Label statusLabel) GetDeptControls(string department)
        {
            if (_deptButtons.ContainsKey(department) && _deptStatusLabels.ContainsKey(department))
                return (_deptButtons[department], _deptStatusLabels[department]);
            return (null, null);
        }

        // =============================================
        // THEME
        // =============================================
        private void AddThemeToggleButton()
        {
            btnThemeToggle = new Button();
            btnThemeToggle.BackColor = Color.Transparent;
            btnThemeToggle.Cursor = Cursors.Hand;
            btnThemeToggle.FlatAppearance.BorderSize = 0;
            btnThemeToggle.FlatStyle = FlatStyle.Flat;
            btnThemeToggle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnThemeToggle.ForeColor = Color.White;
            btnThemeToggle.Location = new Point(680, 18);
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

        private void ApplyCircleToButton(Button btn)
        {
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddEllipse(0, 0, btn.Width - 1, btn.Height - 1);
                btn.Region = new Region(path);
            }
        }

        private void BtnStep1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            string textToDraw;
            if (pnlSubjects.Visible)
            {
                textToDraw = "1";
            }
            else
            {
                textToDraw = "1";
            }

            using (var font = new Font("Segoe UI", 14F, FontStyle.Bold))
            using (var brush = new SolidBrush(btnStep1.ForeColor))
            {
                var sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                e.Graphics.DrawString(textToDraw, font, brush, new Rectangle(0, 0, btnStep1.Width, btnStep1.Height), sf);
            }
        }

        private void BtnStep2_Paint(object sender, PaintEventArgs e)
        {
            Button btn = sender as Button;
            bool wasEnabled = btn.Enabled;
            btn.Enabled = true;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            using (var font = new Font("Segoe UI", 14F, FontStyle.Bold))
            using (var brush = new SolidBrush(btn.ForeColor))
            {
                var sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                e.Graphics.DrawString("2", font, brush, new Rectangle(0, 0, btn.Width, btn.Height), sf);
            }

            btn.Enabled = wasEnabled;
        }

        private void BtnStep3_Paint(object sender, PaintEventArgs e)
        {
            Button btn = sender as Button;
            bool wasEnabled = btn.Enabled;
            btn.Enabled = true;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            using (var font = new Font("Segoe UI", 14F, FontStyle.Bold))
            using (var brush = new SolidBrush(btn.ForeColor))
            {
                var sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                e.Graphics.DrawString("3", font, brush, new Rectangle(0, 0, btn.Width, btn.Height), sf);
            }

            btn.Enabled = wasEnabled;
        }

        public void SetAppContext(AppContext ctx)
        {
            _appContext = ctx;
        }

        private void ApplyTheme()
        {
            this.SuspendLayout();
            pnlSubjects.SuspendLayout();
            pnlDepartments.SuspendLayout();
            pnlCertificate.SuspendLayout();
            pnlStepNav.SuspendLayout();

            try
            {
                try
                {
                    string bgPath;
                    if (isDarkMode)
                        bgPath = Path.Combine(Application.StartupPath, "home_bg2.jpg");
                    else
                        bgPath = Path.Combine(Application.StartupPath, "home_bg.jpg");

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

                LoadLogo();
                pnlTopBar.BackColor = isDarkMode ? Color.FromArgb(200, 1, 28, 64) : Color.FromArgb(200, 255, 255, 255);
                pnlSubjects.BackColor = Color.Transparent;
                pnlDepartments.BackColor = Color.Transparent;
                pnlCertificate.BackColor = Color.Transparent;

                Color panelBackColor = isDarkMode ? Color.FromArgb(220, 1, 28, 64) : Color.FromArgb(245, 255, 255, 255);
                Color textColor = isDarkMode ? Color.White : lunaDarkest;
                Color pendingColor = lunaCyan;
                Color approvedColor = lunaTeal;
                Color rejectedColor = Color.FromArgb(180, 60, 80);

                // Theme all panels
                ThemePanel(pnlSubjects, panelBackColor, textColor, pendingColor, approvedColor, rejectedColor);
                ThemePanel(pnlDepartments, panelBackColor, textColor, pendingColor, approvedColor, rejectedColor);

                // Subject title
                lblSubjectTitle.ForeColor = textColor;
                lblSubjectTitle.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold);
                lblSubjectOverallStatus.ForeColor = pendingColor;
                lblSubjectOverallStatus.Font = new Font("Segoe UI", 11F, FontStyle.Regular);
                progressSubjects.BackColor = isDarkMode ? Color.FromArgb(30, 35, 45) : Color.FromArgb(220, 230, 240);
                progressSubjects.ForeColor = lunaTeal;
                progressSubjects.Height = 25;

                // Dept title
                lblDeptTitle.ForeColor = textColor;
                lblDeptTitle.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold);
                lblDeptOverallStatus.ForeColor = pendingColor;
                lblDeptOverallStatus.Font = new Font("Segoe UI", 11F, FontStyle.Regular);
                progressDepts.BackColor = isDarkMode ? Color.FromArgb(30, 35, 45) : Color.FromArgb(220, 230, 240);
                progressDepts.ForeColor = lunaTeal;
                progressDepts.Height = 25;

                // Certificate panel theme (SIMPLIFIED)
                lblCertTitle.ForeColor = textColor;
                lblCertStudentID.ForeColor = textColor;
                lblCertProgram.ForeColor = textColor;
                lblCertDate.ForeColor = isDarkMode ? Color.FromArgb(185, 187, 190) : Color.FromArgb(100, 100, 100);
                lblCertSubjectsCheck.ForeColor = lunaCyan;
                lblCertDeptsCheck.ForeColor = lunaCyan;
                btnDownloadCert.BackColor = lunaTeal;
                btnDownloadCert.ForeColor = Color.White;
                btnDownloadCert.FlatStyle = FlatStyle.Flat;
                btnDownloadCert.FlatAppearance.BorderSize = 0;
                ApplyRoundedCornersToButton(btnDownloadCert, 20);

                if (btnThemeToggle != null)
                {
                    btnThemeToggle.ForeColor = textColor;
                    ApplyRoundedCornersToButton(btnThemeToggle, 15);
                }

                btnLogout.Size = new Size(100, 35);
                btnLogout.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
                btnLogout.BackColor = lunaTeal;
                btnLogout.ForeColor = Color.White;
                btnLogout.FlatStyle = FlatStyle.Flat;
                btnLogout.FlatAppearance.BorderSize = 0;
                ApplyRoundedCornersToButton(btnLogout, 15);

                // =============================================
                // STEP BUTTONS — PERFECT CIRCLES WITH PAINT TEXT
                // =============================================
                btnStep1.FlatStyle = FlatStyle.Flat;
                btnStep1.FlatAppearance.BorderSize = 0;
                btnStep1.Size = new Size(35, 35);
                btnStep1.Text = "";
                ApplyCircleToButton(btnStep1);

                btnStep2.FlatStyle = FlatStyle.Flat;
                btnStep2.FlatAppearance.BorderSize = 0;
                btnStep2.Size = new Size(35, 35);
                btnStep2.Text = "";
                ApplyCircleToButton(btnStep2);

                btnStep3.FlatStyle = FlatStyle.Flat;
                btnStep3.FlatAppearance.BorderSize = 0;
                btnStep3.Size = new Size(35, 35);
                btnStep3.Text = "";
                ApplyCircleToButton(btnStep3);

                // Step nav panel — matches top bar
                pnlStepNav.BackColor = isDarkMode ? Color.FromArgb(200, 1, 28, 64) : Color.FromArgb(200, 255, 255, 255);

                // Determine active step
                bool step1Active = pnlSubjects.Visible;
                bool step2Active = pnlDepartments.Visible;
                bool step3Active = pnlCertificate.Visible;

                // Button 1 — cyan if active, teal if not
                if (step1Active)
                {
                    btnStep1.BackColor = lunaCyan;
                    btnStep1.ForeColor = Color.White;
                }
                else
                {
                    btnStep1.BackColor = lunaTeal;
                    btnStep1.ForeColor = Color.White;
                }

                // Button 2 — cyan if active, teal if unlocked, muted if locked
                if (!btnStep2.Enabled)
                {
                    btnStep2.BackColor = Color.FromArgb(30, 60, 90);
                    btnStep2.ForeColor = Color.FromArgb(150, 150, 160);
                }
                else if (step2Active)
                {
                    btnStep2.BackColor = lunaCyan;
                    btnStep2.ForeColor = Color.White;
                }
                else
                {
                    btnStep2.BackColor = lunaTeal;
                    btnStep2.ForeColor = Color.White;
                }

                // Button 3 — cyan if active, teal if unlocked, muted if locked
                if (!btnStep3.Enabled)
                {
                    btnStep3.BackColor = Color.FromArgb(30, 60, 90);
                    btnStep3.ForeColor = Color.FromArgb(150, 150, 160);
                }
                else if (step3Active)
                {
                    btnStep3.BackColor = lunaCyan;
                    btnStep3.ForeColor = Color.White;
                }
                else
                {
                    btnStep3.BackColor = lunaTeal;
                    btnStep3.ForeColor = Color.White;
                }

                // Redraw Paint for perfect centered text
                btnStep1.Invalidate();
                btnStep2.Invalidate();
                btnStep3.Invalidate();

                // Ensure notifications stay on top
                if (pnlNotifications != null)
                    pnlNotifications.BringToFront();

                if (btnViewNotifications != null)
                    btnViewNotifications.ForeColor = textColor;

                if (lblUserGreetingText != null)
                    lblUserGreetingText.ForeColor = textColor;

                if (lnkEditProfile != null)
                {
                    lnkEditProfile.LinkColor = lunaCyan;
                    lnkEditProfile.ForeColor = lunaCyan;
                }

                UpdateNotificationBadgeTheme();
            }
            finally
            {
                pnlStepNav.ResumeLayout(false);
                pnlCertificate.ResumeLayout(false);
                pnlDepartments.ResumeLayout(false);
                pnlSubjects.ResumeLayout(false);
                this.ResumeLayout(false);
                pnlSubjects.Invalidate();
                pnlDepartments.Invalidate();
                pnlCertificate.Invalidate();
            }
        }

        private void ThemePanel(Panel panel, Color panelBackColor, Color textColor, Color pendingColor, Color approvedColor, Color rejectedColor)
        {
            panel.SuspendLayout();
            try
            {
                foreach (Control ctrl in panel.Controls)
                {
                    if (ctrl is Panel pnl && (pnl.Name.StartsWith("pnl") && pnl.Name != "pnlSubjects" && pnl.Name != "pnlDepartments" && pnl.Name != "pnlTopBar"))
                    {
                        pnl.BackColor = panelBackColor;
                        pnl.BorderStyle = BorderStyle.None;
                        ApplyRoundedCorners(pnl, 15);

                        foreach (Control inner in pnl.Controls)
                        {
                            if (inner is Label lbl)
                            {
                                if (lbl.Name.Contains("Status"))
                                {
                                    if (lbl.Text == "Approved")
                                        lbl.ForeColor = approvedColor;
                                    else if (lbl.Text == "Rejected" || lbl.Text.Contains("Rejected"))
                                        lbl.ForeColor = rejectedColor;
                                    else if (lbl.Text == "Pending Review" || lbl.Text == "Pending")
                                        lbl.ForeColor = pendingColor;
                                    else
                                        lbl.ForeColor = lunaCyan;
                                    lbl.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
                                }
                                else
                                    lbl.ForeColor = textColor;
                            }
                            else if (inner is Button btn && btn.Name.Contains("Submit"))
                            {
                                btn.Size = new Size(110, 40);
                                btn.Location = new Point(btn.Location.X, 28);
                                btn.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
                                btn.FlatStyle = FlatStyle.Flat;
                                btn.FlatAppearance.BorderSize = 0;
                                btn.Cursor = Cursors.Hand;

                                if (btn.Text == "Approved")
                                {
                                    btn.BackColor = approvedColor;
                                    btn.ForeColor = Color.White;
                                }
                                else if (btn.Text == "Submitted!")
                                {
                                    btn.BackColor = pendingColor;
                                    btn.ForeColor = Color.White;
                                }
                                else if (btn.Text == "Rejected")
                                {
                                    btn.BackColor = rejectedColor;
                                    btn.ForeColor = Color.White;
                                }
                                else
                                {
                                    btn.BackColor = lunaTeal;
                                    btn.ForeColor = Color.White;
                                }
                                ApplyRoundedCornersToButton(btn, 20);
                            }
                        }
                    }
                }
            }
            finally
            {
                panel.ResumeLayout(false);
            }
        }

        private Image CreateSolidBackground(Color color)
        {
            Bitmap bmp = new Bitmap(900, 580);
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

        // =============================================
        // NOTIFICATIONS
        // =============================================
        private void SetupNotificationPolling()
        {
            _notificationTimer = new Timer();
            _notificationTimer.Interval = 10000;
            _notificationTimer.Tick += (s, e) =>
            {
                if (!this.IsDisposed && this.Visible)
                    UpdateNotificationBadge();
                else if (this.IsDisposed)
                {
                    _notificationTimer.Stop();
                    _notificationTimer.Dispose();
                    _notificationTimer = null;
                }
            };
            _notificationTimer.Start();
        }

        private void UpdateNotificationBadge()
        {
            if (this.IsDisposed || _isCleaningUp) return;
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("sp_GetUnreadNotificationCount", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Username", _username);
                        _unreadNotificationCount = (int)cmd.ExecuteScalar();
                        if (this.IsDisposed || _isCleaningUp) return;
                        if (this.InvokeRequired)
                            this.Invoke((Action)(() => ApplyNotificationBadgeState()));
                        else
                            ApplyNotificationBadgeState();
                    }
                }
            }
            catch (ObjectDisposedException) { }
            catch (Exception ex) { Console.WriteLine("Error updating notification badge: " + ex.Message); }
        }

        private void ApplyNotificationBadgeState()
        {
            if (this.IsDisposed || lblNotificationBadge == null || btnViewNotifications == null) return;
            if (this.InvokeRequired) { this.Invoke((Action)(() => ApplyNotificationBadgeState())); return; }
            try
            {
                if (lblNotificationBadge.IsDisposed || btnViewNotifications.IsDisposed) return;
                if (_unreadNotificationCount > 0)
                {
                    lblNotificationBadge.Text = _unreadNotificationCount > 99 ? "99+" : _unreadNotificationCount.ToString();
                    lblNotificationBadge.Visible = true;
                    btnViewNotifications.ForeColor = lunaTeal;
                    lblNotificationBadge.BackColor = lunaCyan;
                    lblNotificationBadge.ForeColor = isDarkMode ? lunaDarkest : Color.White;
                }
                else
                {
                    lblNotificationBadge.Visible = false;
                    btnViewNotifications.ForeColor = isDarkMode ? Color.White : lunaDarkest;
                }
            }
            catch (ObjectDisposedException) { }
        }

        private void SetupNotificationsPanel()
        {
            pnlNotifications = new Panel();
            pnlNotifications.BackColor = isDarkMode ? Color.FromArgb(1, 28, 64) : Color.FromArgb(240, 248, 255);
            pnlNotifications.Size = new Size(320, 420);
            pnlNotifications.Location = new Point(this.Width - 340, 80);
            pnlNotifications.Visible = false;
            pnlNotifications.BorderStyle = BorderStyle.None;
            ApplyRoundedCorners(pnlNotifications, 15);

            lblNotificationTitle = new Label();
            lblNotificationTitle.Text = "Notifications";
            lblNotificationTitle.Font = new Font("Segoe UI Semibold", 14, FontStyle.Bold);
            lblNotificationTitle.ForeColor = isDarkMode ? lunaLight : lunaTeal;
            lblNotificationTitle.Location = new Point(15, 15);
            lblNotificationTitle.Size = new Size(200, 30);
            pnlNotifications.Controls.Add(lblNotificationTitle);

            btnCloseNotifications = new Button();
            btnCloseNotifications.Text = "✕";
            btnCloseNotifications.BackColor = Color.Transparent;
            btnCloseNotifications.ForeColor = isDarkMode ? lunaLight : lunaTeal;
            btnCloseNotifications.FlatStyle = FlatStyle.Flat;
            btnCloseNotifications.FlatAppearance.BorderSize = 0;
            btnCloseNotifications.Size = new Size(35, 30);
            btnCloseNotifications.Location = new Point(270, 15);
            btnCloseNotifications.Cursor = Cursors.Hand;
            btnCloseNotifications.Font = new Font("Segoe UI", 12F);
            btnCloseNotifications.Click += (s, e) => pnlNotifications.Visible = false;
            pnlNotifications.Controls.Add(btnCloseNotifications);

            lstNotifications = new ListBox();
            lstNotifications.BackColor = isDarkMode ? Color.FromArgb(1, 28, 64) : Color.FromArgb(255, 255, 255);
            lstNotifications.ForeColor = isDarkMode ? lunaLight : lunaDarkest;
            lstNotifications.BorderStyle = BorderStyle.None;
            lstNotifications.Location = new Point(15, 55);
            lstNotifications.Size = new Size(290, 310);
            lstNotifications.DrawMode = DrawMode.OwnerDrawVariable;
            lstNotifications.MeasureItem += LstNotifications_MeasureItem;
            lstNotifications.DrawItem += LstNotifications_DrawItem;
            pnlNotifications.Controls.Add(lstNotifications);

            var btnMarkRead = new Button();
            btnMarkRead.Text = "Mark All as Read";
            btnMarkRead.BackColor = lunaTeal;
            btnMarkRead.ForeColor = Color.White;
            btnMarkRead.FlatStyle = FlatStyle.Flat;
            btnMarkRead.FlatAppearance.BorderSize = 0;
            btnMarkRead.Size = new Size(130, 35);
            btnMarkRead.Location = new Point(15, 372);
            btnMarkRead.Cursor = Cursors.Hand;
            btnMarkRead.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnMarkRead.Click += (s, e) => {
                try
                {
                    using (var conn = new SqlConnection(_connectionString))
                    {
                        conn.Open();
                        string query = "UPDATE Notifications SET IsRead = 1 WHERE Username = @Username";
                        using (var cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@Username", _username);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    lstNotifications.Items.Clear();
                    pnlNotifications.Visible = false;
                    _unreadNotificationCount = 0;
                    ApplyNotificationBadgeState();
                }
                catch (Exception ex) { Console.WriteLine("Error: " + ex.Message); }
            };
            ApplyRoundedCornersToButton(btnMarkRead, 15);
            pnlNotifications.Controls.Add(btnMarkRead);

            btnViewNotifications = new Button();
            btnViewNotifications.Text = "🔔";
            btnViewNotifications.Font = new Font("Segoe UI", 14);
            btnViewNotifications.BackColor = Color.Transparent;
            btnViewNotifications.ForeColor = isDarkMode ? Color.White : lunaDarkest;
            btnViewNotifications.FlatStyle = FlatStyle.Flat;
            btnViewNotifications.FlatAppearance.BorderSize = 0;
            btnViewNotifications.FlatAppearance.MouseOverBackColor = lunaDarkest;
            btnViewNotifications.Size = new Size(45, 40);
            btnViewNotifications.Location = new Point(735, 15);
            btnViewNotifications.Cursor = Cursors.Hand;
            btnViewNotifications.Click += (s, e) => {
                pnlNotifications.Visible = !pnlNotifications.Visible;
                if (pnlNotifications.Visible)
                {
                    pnlNotifications.BringToFront();  // ← FIX: Ensure panel is on top
                    LoadNotifications();
                }
            };
            pnlTopBar.Controls.Add(btnViewNotifications);

            lblNotificationBadge = new Label();
            lblNotificationBadge.Text = "";
            lblNotificationBadge.Font = new Font("Segoe UI", 8, FontStyle.Bold);
            lblNotificationBadge.ForeColor = isDarkMode ? lunaDarkest : Color.White;
            lblNotificationBadge.BackColor = lunaCyan;
            lblNotificationBadge.AutoSize = false;
            lblNotificationBadge.Size = new Size(18, 18);
            lblNotificationBadge.Location = new Point(762, 12);
            lblNotificationBadge.TextAlign = ContentAlignment.MiddleCenter;
            lblNotificationBadge.Visible = false;
            lblNotificationBadge.Cursor = Cursors.Hand;
            lblNotificationBadge.Click += (s, e) => {
                pnlNotifications.Visible = !pnlNotifications.Visible;
                if (pnlNotifications.Visible)
                {
                    pnlNotifications.BringToFront();  // ← FIX: Ensure panel is on top
                    LoadNotifications();
                }
            };
            lblNotificationBadge.Paint += (s, e) => {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var path = new GraphicsPath())
                {
                    path.AddEllipse(0, 0, lblNotificationBadge.Width - 1, lblNotificationBadge.Height - 1);
                    lblNotificationBadge.Region = new Region(path);
                }
            };
            pnlTopBar.Controls.Add(lblNotificationBadge);
            lblNotificationBadge.BringToFront();
            this.Controls.Add(pnlNotifications);
            pnlNotifications.BringToFront();
        }

        private void LstNotifications_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            if (e.Index < 0 || lstNotifications.Items.Count == 0) return;

            var notification = lstNotifications.Items[e.Index] as dynamic;
            if (notification == null) { e.ItemHeight = 50; return; }

            string message = notification.Message?.ToString() ?? "";

            // Measure how tall the text needs to be
            using (var g = lstNotifications.CreateGraphics())
            {
                SizeF size = g.MeasureString(message, lstNotifications.Font, lstNotifications.Width - 15);
                // Height = text height + date line + padding
                e.ItemHeight = Math.Max(50, (int)size.Height + 28);
            }
        }

        private void LstNotifications_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            e.DrawBackground();

            var notification = lstNotifications.Items[e.Index] as dynamic;
            if (notification == null) return;

            string message = notification.Message?.ToString() ?? "";
            string date = ((DateTime)notification.CreatedAt).ToString("MMM dd, HH:mm");

            // Draw message — allow word wrap
            using (var brush = new SolidBrush(e.ForeColor))
            {
                RectangleF messageRect = new RectangleF(e.Bounds.X + 5, e.Bounds.Y + 5, e.Bounds.Width - 10, e.Bounds.Height - 28);
                e.Graphics.DrawString(message, e.Font, brush, messageRect);
            }

            // Draw date at bottom
            using (var smallFont = new Font("Segoe UI", 8))
            using (var grayBrush = new SolidBrush(Color.FromArgb(185, 187, 190)))
            {
                e.Graphics.DrawString(date, smallFont, grayBrush, e.Bounds.X + 5, e.Bounds.Bottom - 20);
            }

            e.DrawFocusRectangle();
        }

        private void LoadNotifications()
        {
            try
            {
                lstNotifications.Items.Clear();
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string query = "SELECT Message, Type, CreatedAt FROM Notifications WHERE Username = @Username AND IsRead = 0 ORDER BY CreatedAt DESC";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", _username);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                lstNotifications.Items.Add(new
                                {
                                    Message = reader["Message"].ToString(),
                                    Type = reader["Type"].ToString(),
                                    CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
                                });
                            }
                        }
                    }
                }
                UpdateNotificationBadge();
            }
            catch (Exception ex) { Console.WriteLine("Error loading notifications: " + ex.Message); }
        }

        // =============================================
        // PROFILE
        // =============================================
        private void AddProfileSection()
        {
            pnlProfileContainer = new Panel { Size = new Size(280, 70), Location = new Point(0, 0), BackColor = Color.Transparent };

            // Double-buffer to prevent flickering
            typeof(Panel).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.SetProperty |
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.NonPublic,
                null, pnlProfileContainer, new object[] { true });

            MakeDraggable(pnlProfileContainer);

            pbProfilePicture = new PictureBox { Size = new Size(50, 50), Location = new Point(15, 10), SizeMode = PictureBoxSizeMode.Zoom, BackColor = Color.Transparent, Cursor = Cursors.Hand };
            pbProfilePicture.Click += (s, e) => OpenEditProfileForm();
            MakeDraggable(pbProfilePicture);

            var lblWelcome = new Label { Text = "Welcome,", Location = new Point(75, 12), Size = new Size(60, 15), Font = new Font("Segoe UI", 9F), ForeColor = isDarkMode ? lunaLight : lunaTeal, BackColor = Color.Transparent };
            MakeDraggable(lblWelcome);

            lblUserGreetingText = new Label { Location = new Point(75, 30), Size = new Size(190, 20), Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold), ForeColor = isDarkMode ? Color.White : lunaDarkest, BackColor = Color.Transparent, Text = _username };
            MakeDraggable(lblUserGreetingText);

            lnkEditProfile = new LinkLabel { Text = "Edit Profile", Location = new Point(75, 50), Size = new Size(70, 15), Font = new Font("Segoe UI", 8F), LinkColor = lunaCyan, ActiveLinkColor = lunaLight, VisitedLinkColor = lunaCyan, Cursor = Cursors.Hand, BackColor = Color.Transparent };
            lnkEditProfile.LinkClicked += (s, e) => OpenEditProfileForm();

            pnlProfileContainer.Controls.Add(pbProfilePicture);
            pnlProfileContainer.Controls.Add(lblWelcome);
            pnlProfileContainer.Controls.Add(lblUserGreetingText);
            pnlProfileContainer.Controls.Add(lnkEditProfile);
            pnlTopBar.Controls.Add(pnlProfileContainer);
        }

        private void LoadLogo()
        {
            string logoPath;
            if (isDarkMode) logoPath = Path.Combine(Application.StartupPath, "logo2.png");
            else logoPath = Path.Combine(Application.StartupPath, "logo.png");
            if (!File.Exists(logoPath))
            {
                string logosFolder = Path.Combine(Application.StartupPath, "Logos");
                if (isDarkMode) logoPath = Path.Combine(logosFolder, "logo2.png");
                else logoPath = Path.Combine(logosFolder, "logo.png");
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
                catch { CreateDefaultLogo(); }
            }
            else { CreateDefaultLogo(); }
        }

        private void CreateDefaultLogo()
        {
            var bmp = new Bitmap(100, 50);
            using (var g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Transparent);
                using (var font = new Font("Segoe UI", 10, FontStyle.Bold))
                using (var brush = new SolidBrush(isDarkMode ? lunaLight : lunaDarkest))
                {
                    var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                    g.DrawString("CLEARANCE", font, brush, new Rectangle(0, 0, 100, 25), sf);
                    g.DrawString("SYSTEM", new Font("Segoe UI", 8, FontStyle.Bold), brush, new Rectangle(0, 25, 100, 25), sf);
                }
            }
            pbLogo.Image = bmp;
        }

        private void LoadUserGreeting()
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string query = "SELECT FullName, ProfileImage FROM Users WHERE Username = @Username";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", _username);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string fullName = reader["FullName"] != DBNull.Value ? reader["FullName"].ToString() : null;
                                lblUserGreetingText.Text = string.IsNullOrEmpty(fullName) ? _username : fullName;
                                if (reader["ProfileImage"] != DBNull.Value)
                                    DisplayProfileImage((byte[])reader["ProfileImage"]);
                                else CreateDefaultProfileImage();
                            }
                            else { CreateDefaultProfileImage(); }
                        }
                    }
                }
            }
            catch { CreateDefaultProfileImage(); }
        }

        private void DisplayProfileImage(byte[] imageData)
        {
            if (imageData != null && imageData.Length > 0)
            {
                try
                {
                    using (var ms = new MemoryStream(imageData))
                    using (var img = Image.FromStream(ms))
                        pbProfilePicture.Image = MakeCircularImage(img, 50, 50);
                }
                catch { CreateDefaultProfileImage(); }
            }
            else { CreateDefaultProfileImage(); }
        }

        private void CreateDefaultProfileImage()
        {
            var bmp = new Bitmap(50, 50);
            using (var g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Transparent);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (var brush = new SolidBrush(lunaTeal))
                    g.FillEllipse(brush, 0, 0, 49, 49);
                using (var font = new Font("Segoe UI", 20, FontStyle.Bold))
                using (var brush = new SolidBrush(Color.White))
                {
                    var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                    string initial = _username.Length > 0 ? _username[0].ToString().ToUpper() : "?";
                    g.DrawString(initial, font, brush, new Rectangle(0, 0, 50, 50), sf);
                }
            }
            pbProfilePicture.Image = bmp;
        }

        private Image MakeCircularImage(Image img, int width, int height)
        {
            var bmp = new Bitmap(width, height);
            using (var g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Transparent);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                var path = new GraphicsPath();
                path.AddEllipse(0, 0, width - 1, height - 1);
                g.SetClip(path);
                g.DrawImage(img, 0, 0, width, height);
            }
            return bmp;
        }

        private void OpenEditProfileForm()
        {
            var editForm = new EditProfileForm(_connectionString, _username);
            if (editForm.ShowDialog() == DialogResult.OK) LoadUserGreeting();
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

                // Listen for subject status updates
                _hubProxy.On<string, string>("subjectStatusUpdated", (subject, status) =>
                {
                    if (!this.IsDisposed && !_isCleaningUp && this.Visible)
                    {
                        this.Invoke((Action)(() =>
                        {
                            if (!this.IsDisposed && !_isCleaningUp)
                            {
                                UpdateSubjectStatus(subject, status);
                                UpdateSubjectProgress();
                                UpdateStep2Button();
                                UpdateStep3Button();
                                UpdateNotificationBadge();
                            }
                        }));
                    }
                });

                // Listen for department status updates
                _hubProxy.On<string, string>("statusUpdated", (department, status) =>
                {
                    if (!this.IsDisposed && !_isCleaningUp && this.Visible)
                    {
                        this.Invoke((Action)(() =>
                        {
                            if (!this.IsDisposed && !_isCleaningUp)
                            {
                                UpdateDeptStatus(department, status);
                                UpdateDeptProgress();
                                UpdateNotificationBadge();
                            }
                        }));
                    }
                });

                _hubConnection.Closed += OnConnectionClosed;

                await _hubConnection.Start();
                await _hubProxy.Invoke("JoinStudentGroup", _username);

                // Immediately refresh both panels after connection
                LoadSubjectStatusFromDB();
                LoadDeptStatusFromDB();
            }
            catch (Exception ex)
            {
                Console.WriteLine("SignalR Error: " + ex.Message);
                SetupSubjectPollingTimer();
                SetupDeptPollingTimer();
                // Load immediately since SignalR failed
                LoadSubjectStatusFromDB();
                LoadDeptStatusFromDB();
            }
        }

        private void OnConnectionClosed()
        {
            if (_isCleaningUp || this.IsDisposed) return;
            try
            {
                if (!this.IsDisposed && this.Visible)
                {
                    this.Invoke((Action)(() =>
                    {
                        if (!this.IsDisposed && !_isCleaningUp)
                        {
                            Console.WriteLine("SignalR connection closed. Using polling fallback...");
                            SetupSubjectPollingTimer();
                            SetupDeptPollingTimer();
                        }
                    }));
                }
            }
            catch (ObjectDisposedException) { }
        }

        private void SetupSubjectPollingTimer()
        {
            if (_isCleaningUp || this.IsDisposed) return;
            if (_subjectRefreshTimer == null)
            {
                _subjectRefreshTimer = new Timer { Interval = 5000 };
                _subjectRefreshTimer.Tick += (s, e) =>
                {
                    if (!this.IsDisposed && !_isCleaningUp && this.Visible)
                        LoadSubjectStatusFromDB();
                };
                _subjectRefreshTimer.Start();
            }
        }

        private void SetupDeptPollingTimer()
        {
            if (_isCleaningUp || this.IsDisposed) return;
            if (_deptRefreshTimer == null)
            {
                _deptRefreshTimer = new Timer { Interval = 5000 };
                _deptRefreshTimer.Tick += (s, e) =>
                {
                    if (!this.IsDisposed && !_isCleaningUp && this.Visible)
                        LoadDeptStatusFromDB();
                };
                _deptRefreshTimer.Start();
            }
        }

        private void LoadCertificateData()
        {
            lblCertStudentID.Text = "Student ID: " + _username;
            lblCertDate.Text = "Completed on: " + DateTime.Now.ToString("MMMM dd, yyyy");

            string program = "";
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string query = "SELECT p.ProgramName FROM Users u INNER JOIN Programs p ON u.Program = p.ProgramCode WHERE u.Username = @Username";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", _username);
                        program = cmd.ExecuteScalar()?.ToString() ?? "N/A";
                    }
                }
            }
            catch { program = "N/A"; }

            lblCertProgram.Text = "Program: " + program;
            lblCertSubjectsCheck.Text = "✓ Subjects Cleared";
            lblCertDeptsCheck.Text = "✓ Departments Cleared";

            ApplyTheme();
        }

        // =============================================
        // SUBJECT STATUS LOADING
        // =============================================
        private void LoadSubjectStatusFromDB()
        {
            if (this.IsDisposed) return;
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("sp_GetStudentSubjectStatus", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Username", _username);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (this.IsDisposed) return;
                                string subject = reader["SubjectName"].ToString();
                                string status = reader["Status"].ToString();
                                if (status != "Pending")
                                    UpdateSubjectStatus(subject, status);
                                else if (reader["SubmittedAt"] != DBNull.Value)
                                {
                                    var controls = GetSubjectControls(subject);
                                    if (controls.submitButton != null && !controls.submitButton.IsDisposed)
                                    {
                                        controls.submitButton.Invoke((Action)(() =>
                                        {
                                            controls.submitButton.Text = "Submitted!";
                                            controls.submitButton.BackColor = lunaCyan;
                                            controls.submitButton.Enabled = false;
                                            controls.statusLabel.Text = "Pending Review";
                                            controls.statusLabel.ForeColor = lunaCyan;
                                            ApplyRoundedCornersToButton(controls.submitButton, 20);
                                        }));
                                    }
                                }
                            }
                        }
                    }
                }
                if (!this.IsDisposed)
                {
                    UpdateSubjectProgress();
                    UpdateStep2Button();
                }
            }
            catch (ObjectDisposedException) { }
            catch (Exception ex) { Console.WriteLine("Error loading subject status: " + ex.Message); }
        }

        private void UpdateSubjectStatus(string subject, string status)
        {
            var controls = GetSubjectControls(subject);
            if (controls.submitButton != null && !controls.submitButton.IsDisposed)
            {
                _subjectStatus[subject] = (status == "Approved");
                try
                {
                    if (status == "Approved")
                    {
                        controls.submitButton.Text = "Approved";
                        controls.submitButton.BackColor = lunaTeal;
                        controls.submitButton.Enabled = false;
                        controls.statusLabel.Text = "Approved";
                        controls.statusLabel.ForeColor = lunaTeal;
                    }
                    else if (status == "Rejected")
                    {
                        controls.submitButton.Text = "Rejected";
                        controls.submitButton.BackColor = Color.FromArgb(180, 60, 80);
                        controls.submitButton.Enabled = true;
                        controls.statusLabel.Text = "Rejected";
                        controls.statusLabel.ForeColor = Color.FromArgb(180, 60, 80);
                        _subjectStatus[subject] = false;
                    }
                    controls.statusLabel.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
                    ApplyRoundedCornersToButton(controls.submitButton, 20);
                }
                catch (ObjectDisposedException) { }
            }
        }

        private void UpdateSubjectProgress()
        {
            if (this.IsDisposed) return;
            if (this.InvokeRequired) { this.Invoke((Action)(() => UpdateSubjectProgress())); return; }

            int approvedCount = 0;
            foreach (var subject in _subjects)
            {
                var controls = GetSubjectControls(subject);
                if (controls.statusLabel != null && !controls.statusLabel.IsDisposed && controls.statusLabel.Text == "Approved")
                    approvedCount++;
            }

            if (progressSubjects != null && !progressSubjects.IsDisposed)
            {
                progressSubjects.Maximum = _subjects.Count;
                progressSubjects.Value = approvedCount;
                lblSubjectOverallStatus.Text = $"Overall Progress: {approvedCount}/{_subjects.Count}";
            }

            UpdateStep2Button();
            UpdateStep3Button();
        }

        // =============================================
        // DEPARTMENT STATUS LOADING
        // =============================================
        private void LoadDeptStatusFromDB()
        {
            if (this.IsDisposed) return;
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("sp_GetStudentClearanceStatus", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Username", _username);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (this.IsDisposed) return;
                                string dept = reader["DepartmentName"].ToString();
                                string status = reader["Status"].ToString();
                                if (status != "Pending")
                                    UpdateDeptStatus(dept, status);
                                else if (reader["SubmittedAt"] != DBNull.Value)
                                {
                                    var controls = GetDeptControls(dept);
                                    if (controls.submitButton != null && !controls.submitButton.IsDisposed)
                                    {
                                        controls.submitButton.Invoke((Action)(() =>
                                        {
                                            controls.submitButton.Text = "Submitted!";
                                            controls.submitButton.BackColor = lunaCyan;
                                            controls.submitButton.Enabled = false;
                                            controls.statusLabel.Text = "Pending Review";
                                            controls.statusLabel.ForeColor = lunaCyan;
                                            ApplyRoundedCornersToButton(controls.submitButton, 20);
                                        }));
                                    }
                                }
                            }
                        }
                    }
                }
                if (!this.IsDisposed) UpdateDeptProgress();
            }
            catch (ObjectDisposedException) { }
            catch (Exception ex) { Console.WriteLine("Error loading dept status: " + ex.Message); }
        }

        private void UpdateDeptStatus(string department, string status)
        {
            var controls = GetDeptControls(department);
            if (controls.submitButton != null && !controls.submitButton.IsDisposed)
            {
                _deptStatus[department] = (status == "Approved");
                try
                {
                    if (status == "Approved")
                    {
                        controls.submitButton.Text = "Approved";
                        controls.submitButton.BackColor = lunaTeal;
                        controls.submitButton.Enabled = false;
                        controls.statusLabel.Text = "Approved";
                        controls.statusLabel.ForeColor = lunaTeal;
                    }
                    else if (status == "Rejected")
                    {
                        controls.submitButton.Text = "Rejected";
                        controls.submitButton.BackColor = Color.FromArgb(180, 60, 80);
                        controls.submitButton.Enabled = true;
                        controls.statusLabel.Text = "Rejected";
                        controls.statusLabel.ForeColor = Color.FromArgb(180, 60, 80);
                        _deptStatus[department] = false;
                    }
                    controls.statusLabel.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
                    ApplyRoundedCornersToButton(controls.submitButton, 20);
                }
                catch (ObjectDisposedException) { }
            }
        }

        private void UpdateDeptProgress()
        {
            if (this.IsDisposed) return;
            if (this.InvokeRequired) { this.Invoke((Action)(() => UpdateDeptProgress())); return; }

            int approvedCount = 0;
            foreach (var dept in _deptStatus.Keys)
            {
                var controls = GetDeptControls(dept);
                if (controls.statusLabel != null && !controls.statusLabel.IsDisposed && controls.statusLabel.Text == "Approved")
                    approvedCount++;
            }

            if (progressDepts != null && !progressDepts.IsDisposed)
            {
                progressDepts.Maximum = 6;
                progressDepts.Value = approvedCount;
                lblDeptOverallStatus.Text = $"Overall Progress: {approvedCount}/6";
            }

            UpdateStep3Button();
        }

        // =============================================
        // STEP NAVIGATION
        // =============================================
        private void UpdateStep2Button()
        {
            if (this.IsDisposed) return;
            if (this.InvokeRequired) { this.Invoke((Action)(() => UpdateStep2Button())); return; }

            if (btnStep2 == null || btnStep2.IsDisposed) return;

            btnStep2.Size = new Size(35, 35);
            btnStep2.Text = "";
            btnStep2.FlatStyle = FlatStyle.Flat;
            btnStep2.FlatAppearance.BorderSize = 0;
            ApplyCircleToButton(btnStep2);

            int approvedCount = 0;
            foreach (var subject in _subjects)
            {
                var controls = GetSubjectControls(subject);
                if (controls.statusLabel != null && !controls.statusLabel.IsDisposed && controls.statusLabel.Text == "Approved")
                    approvedCount++;
            }

            if (approvedCount >= _subjects.Count && _subjects.Count > 0)
            {
                btnStep2.Enabled = true;
                btnStep2.Cursor = Cursors.Hand;
            }
            else
            {
                btnStep2.Enabled = false;
                btnStep2.BackColor = Color.FromArgb(30, 60, 90);
                btnStep2.ForeColor = Color.FromArgb(150, 150, 160);
            }

            btnStep2.Invalidate();
            ApplyTheme(); 
        }

        private void UpdateStep3Button()
        {
            if (this.IsDisposed) return;
            if (this.InvokeRequired) { this.Invoke((Action)(() => UpdateStep3Button())); return; }

            if (btnStep3 == null || btnStep3.IsDisposed) return;

            btnStep3.Size = new Size(35, 35);
            btnStep3.Text = "";
            btnStep3.FlatStyle = FlatStyle.Flat;
            btnStep3.FlatAppearance.BorderSize = 0;
            ApplyCircleToButton(btnStep3);

            // Check if ALL subjects AND ALL departments are approved
            int approvedSubjects = 0;
            foreach (var subject in _subjects)
            {
                var controls = GetSubjectControls(subject);
                if (controls.statusLabel != null && !controls.statusLabel.IsDisposed && controls.statusLabel.Text == "Approved")
                    approvedSubjects++;
            }

            int approvedDepts = 0;
            foreach (var dept in _deptStatus.Keys)
            {
                var controls = GetDeptControls(dept);
                if (controls.statusLabel != null && !controls.statusLabel.IsDisposed && controls.statusLabel.Text == "Approved")
                    approvedDepts++;
            }

            if (approvedSubjects >= _subjects.Count && approvedDepts >= 6 && _subjects.Count > 0)
            {
                btnStep3.Enabled = true;
                btnStep3.Cursor = Cursors.Hand;
            }
            else
            {
                btnStep3.Enabled = false;
                btnStep3.BackColor = Color.FromArgb(30, 60, 90);
                btnStep3.ForeColor = Color.FromArgb(150, 150, 160);
            }

            btnStep3.Invalidate();
        }

        private void SetDefaultPanel()
        {
            if (this.IsDisposed) return;

            // Check if all subjects AND all departments are approved → go to Certificate
            int approvedSubjects = 0;
            foreach (var subject in _subjects)
            {
                var controls = GetSubjectControls(subject);
                if (controls.statusLabel != null && !controls.statusLabel.IsDisposed && controls.statusLabel.Text == "Approved")
                    approvedSubjects++;
            }

            int approvedDepts = 0;
            foreach (var dept in _deptStatus.Keys)
            {
                var controls = GetDeptControls(dept);
                if (controls.statusLabel != null && !controls.statusLabel.IsDisposed && controls.statusLabel.Text == "Approved")
                    approvedDepts++;
            }

            if (approvedSubjects >= _subjects.Count && approvedDepts >= 6 && _subjects.Count > 0)
            {
                // All done — show Certificate
                pnlSubjects.Visible = false;
                pnlDepartments.Visible = false;
                pnlCertificate.Visible = true;
                pnlCertificate.BringToFront();
                LoadCertificateData();
            }
            else if (approvedSubjects >= _subjects.Count && _subjects.Count > 0)
            {
                // Subjects done — show Departments
                pnlSubjects.Visible = false;
                pnlDepartments.Visible = true;
                pnlCertificate.Visible = false;
                pnlDepartments.BringToFront();
            }
            else
            {
                // Default — show Subjects
                pnlSubjects.Visible = true;
                pnlDepartments.Visible = false;
                pnlCertificate.Visible = false;
                pnlSubjects.BringToFront();
            }

            ApplyTheme();
        }

        private async Task FadeSwitchPanels(Panel showPanel, Panel hidePanel)
        {
            if (_isSliding) return;
            _isSliding = true;

            // Quick 2-step crossfade (~40ms total)
            showPanel.Visible = true;
            showPanel.BringToFront();

            if (hidePanel != null && !hidePanel.IsDisposed)
            {
                hidePanel.BackColor = isDarkMode ? lunaDarkest : Color.FromArgb(240, 248, 255);
                await Task.Delay(20);
                hidePanel.Visible = false;
                hidePanel.BackColor = Color.Transparent;
            }

            if (showPanel != null && !showPanel.IsDisposed)
            {
                showPanel.BackColor = isDarkMode ? lunaDarkest : Color.FromArgb(240, 248, 255);
                await Task.Delay(20);
                showPanel.BackColor = Color.Transparent;
            }

            if (pnlNotifications != null)
                pnlNotifications.BringToFront();

            _isSliding = false;
            ApplyTheme();
        }

        private async void btnStep1_Click(object sender, EventArgs e)
        {
            if (_isSliding) return;
            if (pnlSubjects.Visible) return;

            await FadeSwitchPanels(pnlSubjects, pnlDepartments);
        }

        private async void btnStep2_Click(object sender, EventArgs e)
        {
            if (_isSliding) return;
            if (!btnStep2.Enabled) return;
            if (pnlDepartments.Visible) return;

            LoadDeptStatusFromDB();
            await FadeSwitchPanels(pnlDepartments, pnlSubjects);
        }

        private async void btnStep3_Click(object sender, EventArgs e)
        {
            if (_isSliding) return;
            if (!btnStep3.Enabled) return;
            if (pnlCertificate.Visible) return;

            LoadCertificateData();
            await FadeSwitchPanels(pnlCertificate, pnlDepartments.Visible ? pnlDepartments : pnlSubjects);
        }

        private void btnDownloadCert_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "PNG Image|*.png";
                sfd.Title = "Save Certificate";
                sfd.FileName = "Clearance_Certificate_" + _username + ".png";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    // Capture the certificate panel as an image
                    Bitmap bmp = new Bitmap(pnlCertificate.Width, pnlCertificate.Height);
                    pnlCertificate.DrawToBitmap(bmp, new Rectangle(0, 0, pnlCertificate.Width, pnlCertificate.Height));
                    bmp.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Png);
                    bmp.Dispose();

                    MessageBox.Show("Certificate saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        // =============================================
        // SUBJECT SUBMISSION HANDLERS
        // =============================================
        private void HandleSubjectSubmission(string subject, Button submitButton, Label statusLabel)
        {
            if (statusLabel.Text == "Approved")
            {
                MessageBox.Show($"{subject} clearance has already been approved.", "Already Approved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            var result = MessageBox.Show($"Do you want to submit a file for {subject} clearance?", "Confirm Submission", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes && _openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    byte[] fileData;
                    string fileName = Path.GetFileName(_openFileDialog.FileName);
                    string fileType = Path.GetExtension(_openFileDialog.FileName).ToLower().Replace(".", "");

                    // Read file as bytes (works for both images and PDFs)
                    fileData = File.ReadAllBytes(_openFileDialog.FileName);

                    // Validate image if it's an image file
                    if (fileType != "pdf")
                    {
                        try
                        {
                            using (var ms = new MemoryStream(fileData))
                            {
                                Image.FromStream(ms);
                            }
                        }
                        catch
                        {
                            MessageBox.Show("The selected file is not a valid image.", "Invalid File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    using (var conn = new SqlConnection(_connectionString))
                    {
                        conn.Open();
                        using (var cmd = new SqlCommand("sp_SubmitSubjectClearance", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@StudentUsername", _username);
                            cmd.Parameters.AddWithValue("@SubjectName", subject);
                            cmd.Parameters.AddWithValue("@FileData", fileData);
                            cmd.Parameters.AddWithValue("@FileName", fileName);
                            cmd.Parameters.AddWithValue("@FileType", fileType);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    submitButton.Text = "Submitted!";
                    submitButton.BackColor = lunaCyan;
                    submitButton.Enabled = false;
                    statusLabel.Text = "Pending Review";
                    statusLabel.ForeColor = lunaCyan;
                    statusLabel.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
                    UpdateSubjectProgress();
                    UpdateStep2Button();
                    ApplyRoundedCornersToButton(submitButton, 20);

                    try
                    {
                        if (_hubConnection?.State == Microsoft.AspNet.SignalR.Client.ConnectionState.Connected)
                            _hubProxy?.Invoke("NotifyNewSubjectSubmission", subject, lblUserGreetingText.Text, _username);
                    }
                    catch { }

                    MessageBox.Show($"{subject} clearance submitted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex) { MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
        }

        // =============================================
        // DEPARTMENT SUBMISSION HANDLERS
        // =============================================
        private void HandleDeptSubmission(string department, Button submitButton, Label statusLabel)
        {
            if (statusLabel.Text == "Approved")
            {
                MessageBox.Show($"{department} clearance has already been approved.", "Already Approved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            var result = MessageBox.Show($"Do you want to submit a file for {department} clearance?", "Confirm Submission", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes && _openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    byte[] fileData;
                    string fileName = Path.GetFileName(_openFileDialog.FileName);
                    string fileType = Path.GetExtension(_openFileDialog.FileName).ToLower().Replace(".", "");

                    // Read file as bytes (works for both images and PDFs)
                    fileData = File.ReadAllBytes(_openFileDialog.FileName);

                    // Validate image if it's an image file
                    if (fileType != "pdf")
                    {
                        try
                        {
                            using (var ms = new MemoryStream(fileData))
                            {
                                Image.FromStream(ms);
                            }
                        }
                        catch
                        {
                            MessageBox.Show("The selected file is not a valid image.", "Invalid File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    using (var conn = new SqlConnection(_connectionString))
                    {
                        conn.Open();
                        using (var cmd = new SqlCommand("sp_SubmitClearance", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@StudentUsername", _username);
                            cmd.Parameters.AddWithValue("@DepartmentName", department);
                            cmd.Parameters.AddWithValue("@FileData", fileData);
                            cmd.Parameters.AddWithValue("@FileName", fileName);
                            cmd.Parameters.AddWithValue("@FileType", fileType);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    submitButton.Text = "Submitted!";
                    submitButton.BackColor = lunaCyan;
                    submitButton.Enabled = false;
                    statusLabel.Text = "Pending Review";
                    statusLabel.ForeColor = lunaCyan;
                    UpdateDeptProgress();
                    ApplyRoundedCornersToButton(submitButton, 20);
                    MessageBox.Show($"{department} clearance submitted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex) { MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
        }

        private void btnLibrarySubmit_Click(object sender, EventArgs e) => HandleDeptSubmission("Library", btnLibrarySubmit, lblLibraryStatus);
        private void btnSAOSubmit_Click(object sender, EventArgs e) => HandleDeptSubmission("SAO", btnSAOSubmit, lblSAOStatus);
        private void btnCashierSubmit_Click(object sender, EventArgs e) => HandleDeptSubmission("Cashier", btnCashierSubmit, lblCashierStatus);
        private void btnAccountingSubmit_Click(object sender, EventArgs e) => HandleDeptSubmission("Accounting", btnAccountingSubmit, lblAccountingStatus);
        private void btnDeanSubmit_Click(object sender, EventArgs e) => HandleDeptSubmission("Dean's Office", btnDeanSubmit, lblDeanStatus);
        private void btnRecordsSubmit_Click(object sender, EventArgs e) => HandleDeptSubmission("Records", btnRecordsSubmit, lblRecordsStatus);

        // =============================================
        // LOGOUT
        // =============================================
        private void btnLogout_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to logout?", "Confirm Logout",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                _loggedOut = true;
                _isCleaningUp = true;

                if (_subjectRefreshTimer != null) { _subjectRefreshTimer.Stop(); _subjectRefreshTimer.Dispose(); _subjectRefreshTimer = null; }
                if (_deptRefreshTimer != null) { _deptRefreshTimer.Stop(); _deptRefreshTimer.Dispose(); _deptRefreshTimer = null; }
                if (_notificationTimer != null) { _notificationTimer.Stop(); _notificationTimer.Dispose(); _notificationTimer = null; }

                try
                {
                    if (_hubConnection != null)
                    {
                        _hubConnection.Closed -= OnConnectionClosed;
                        if (_hubConnection.State == Microsoft.AspNet.SignalR.Client.ConnectionState.Connected)
                            _hubProxy?.Invoke("LeaveStudentGroup", _username);
                        _hubConnection.Stop();
                        _hubConnection.Dispose();
                        _hubConnection = null;
                    }
                }
                catch { }

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
                    _hubConnection.Closed -= OnConnectionClosed;
                    _hubConnection.Stop();
                    _hubConnection = null;
                }
                if (_subjectRefreshTimer != null) { _subjectRefreshTimer.Stop(); _subjectRefreshTimer.Dispose(); _subjectRefreshTimer = null; }
                if (_deptRefreshTimer != null) { _deptRefreshTimer.Stop(); _deptRefreshTimer.Dispose(); _deptRefreshTimer = null; }
                if (_notificationTimer != null) { _notificationTimer.Stop(); _notificationTimer.Dispose(); _notificationTimer = null; }
            }
            catch { }
            base.OnFormClosing(e);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (pnlNotifications != null)
                pnlNotifications.Location = new Point(this.Width - 340, 80);
        }

        private void UpdateNotificationBadgeTheme()
        {
            if (this.IsDisposed || lblNotificationBadge == null || btnViewNotifications == null) return;
            if (this.InvokeRequired) { this.Invoke((Action)(() => UpdateNotificationBadgeTheme())); return; }
            try
            {
                if (_unreadNotificationCount > 0)
                {
                    if (!lblNotificationBadge.IsDisposed && !btnViewNotifications.IsDisposed)
                    {
                        btnViewNotifications.ForeColor = lunaTeal;
                        lblNotificationBadge.BackColor = lunaCyan;
                        lblNotificationBadge.ForeColor = isDarkMode ? lunaDarkest : Color.White;
                    }
                }
                else
                {
                    if (btnViewNotifications != null && !btnViewNotifications.IsDisposed)
                        btnViewNotifications.ForeColor = isDarkMode ? Color.White : lunaDarkest;
                }
            }
            catch (ObjectDisposedException) { }
        }
    }
}