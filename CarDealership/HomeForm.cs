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

namespace CarDealership
{
    public partial class HomeForm : Form
    {
        private readonly string _connectionString;
        private readonly string _username;
        private readonly Dictionary<string, bool> _clearanceStatus;
        private readonly OpenFileDialog _openFileDialog;
        private IHubProxy _hubProxy;
        private HubConnection _hubConnection;
        private Timer _refreshTimer;

        // Theme variables (matching LoginForm exactly)
        private bool isDarkMode = true;

        Color lunaDarkest = Color.FromArgb(1, 28, 64);
        Color lunaTeal = Color.FromArgb(38, 101, 140);
        Color lunaCyan = Color.FromArgb(84, 172, 191);
        Color lunaLight = Color.FromArgb(167, 235, 242);

        // AppContext integration
        private AppContext _appContext;
        private bool _loggedOut = false;
        public bool LoggedOut => _loggedOut;

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

        // Theme button
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

        public HomeForm(string connectionString, string username)
        {
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer | ControlStyles.ResizeRedraw, true);
            this.UpdateStyles();

            InitializeComponent();

            AddThemeToggleButton();
            ApplyRoundedCorners(this, 30);

            _connectionString = connectionString;
            _username = username;

            _clearanceStatus = new Dictionary<string, bool>
            {
                { "Library", false },
                { "SAO", false },
                { "Cashier", false },
                { "Accounting", false },
                { "Dean's Office", false },
                { "Records", false }
            };

            _openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp|All Files|*.*",
                Title = "Select an image for submission"
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
            SetupClearanceSystem();

            // Set form size
            this.ClientSize = new System.Drawing.Size(900, 580);
            pnlClearance.Height = 510;
            pnlClearance.Padding = new Padding(20, 20, 20, 10);

            ApplyTheme();

            LoadClearanceStatusFromDB();
            InitializeSignalR();
            SetupNotificationPolling();

            this.Shown += (s, e) => {
                UpdateNotificationBadge();
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

        public void SetAppContext(AppContext ctx)
        {
            _appContext = ctx;
        }

        private void ApplyTheme()
        {
            // Load background images - Dark mode: home_bg2.jpg, Light mode: home_bg.jpg
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

            // Reload logo based on theme
            LoadLogo();

            // Top bar with gradient effect (semi-transparent)
            pnlTopBar.BackColor = isDarkMode ? Color.FromArgb(200, 1, 28, 64) : Color.FromArgb(200, 255, 255, 255);

            // Clearance panel transparent to show background
            pnlClearance.BackColor = Color.Transparent;

            // Department panels - DARKER opacity for better readability
            // Dark mode: much darker semi-transparent background
            // Light mode: solid white with slight transparency
            Color panelBackColor = isDarkMode ? Color.FromArgb(220, 1, 28, 64) : Color.FromArgb(245, 255, 255, 255);
            Color textColor = isDarkMode ? Color.White : lunaDarkest;
            // Pending font color - NOT BOLD, just regular
            Color pendingColor = lunaCyan;
            Color approvedColor = Color.FromArgb(40, 167, 69);
            Color rejectedColor = Color.FromArgb(240, 71, 71);

            foreach (Control ctrl in pnlClearance.Controls)
            {
                if (ctrl is Panel pnl && (pnl.Name.StartsWith("pnl") && pnl.Name != "pnlClearance" && pnl.Name != "pnlTopBar"))
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

                                // NOT BOLD - regular font weight
                                lbl.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
                            }
                            else
                                lbl.ForeColor = textColor;
                        }
                        else if (inner is Button btn && btn.Name.Contains("Submit"))
                        {
                            // Larger buttons for better aesthetics
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

                            // Hover effects
                            btn.MouseEnter += (s, ev) => {
                                if (btn.Enabled && btn.Text != "Approved" && btn.Text != "Submitted!")
                                    btn.BackColor = lunaCyan;
                            };
                            btn.MouseLeave += (s, ev) => {
                                if (btn.Enabled && btn.Text != "Approved" && btn.Text != "Submitted!")
                                    btn.BackColor = lunaTeal;
                            };
                        }
                    }
                }
            }

            // Apply larger size to all submit buttons
            ApplyLargerButtons();

            // Title and status labels
            lblClearanceTitle.ForeColor = textColor;
            lblClearanceTitle.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold);
            lblOverallStatus.ForeColor = pendingColor;
            lblOverallStatus.Font = new Font("Segoe UI", 11F, FontStyle.Regular);

            // Progress bar theme
            progressOverall.BackColor = isDarkMode ? Color.FromArgb(30, 35, 45) : Color.FromArgb(220, 230, 240);
            progressOverall.ForeColor = lunaTeal;
            progressOverall.Height = 25;

            // Theme toggle button
            if (btnThemeToggle != null)
            {
                btnThemeToggle.ForeColor = textColor;
                ApplyRoundedCornersToButton(btnThemeToggle, 15);
            }

            // Logout button - larger and rounded
            btnLogout.Size = new Size(100, 35);
            btnLogout.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnLogout.BackColor = lunaTeal;
            btnLogout.ForeColor = Color.White;
            btnLogout.FlatStyle = FlatStyle.Flat;
            btnLogout.FlatAppearance.BorderSize = 0;
            ApplyRoundedCornersToButton(btnLogout, 15);

            btnLogout.MouseEnter += (s, e) => { btnLogout.BackColor = lunaCyan; };
            btnLogout.MouseLeave += (s, e) => { btnLogout.BackColor = lunaTeal; };

            // Notification button theme
            if (btnViewNotifications != null)
                btnViewNotifications.ForeColor = textColor;

            // Profile section colors - ensure visibility in light mode
            if (lblUserGreetingText != null)
                lblUserGreetingText.ForeColor = textColor;

            if (lnkEditProfile != null)
            {
                lnkEditProfile.LinkColor = lunaCyan;
                lnkEditProfile.ForeColor = lunaCyan;
            }

            pnlClearance.Invalidate();
        }

        private void ApplyLargerButtons()
        {
            // Make all submit buttons larger
            btnLibrarySubmit.Size = new Size(110, 40);
            btnLibrarySubmit.Location = new Point(btnLibrarySubmit.Location.X, 28);
            btnLibrarySubmit.Font = new Font("Segoe UI", 10F, FontStyle.Bold);

            btnSAOSubmit.Size = new Size(110, 40);
            btnSAOSubmit.Location = new Point(btnSAOSubmit.Location.X, 28);
            btnSAOSubmit.Font = new Font("Segoe UI", 10F, FontStyle.Bold);

            btnCashierSubmit.Size = new Size(110, 40);
            btnCashierSubmit.Location = new Point(btnCashierSubmit.Location.X, 28);
            btnCashierSubmit.Font = new Font("Segoe UI", 10F, FontStyle.Bold);

            btnAccountingSubmit.Size = new Size(110, 40);
            btnAccountingSubmit.Location = new Point(btnAccountingSubmit.Location.X, 28);
            btnAccountingSubmit.Font = new Font("Segoe UI", 10F, FontStyle.Bold);

            btnDeanSubmit.Size = new Size(110, 40);
            btnDeanSubmit.Location = new Point(btnDeanSubmit.Location.X, 28);
            btnDeanSubmit.Font = new Font("Segoe UI", 10F, FontStyle.Bold);

            btnRecordsSubmit.Size = new Size(110, 40);
            btnRecordsSubmit.Location = new Point(btnRecordsSubmit.Location.X, 28);
            btnRecordsSubmit.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
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

        private void SetupNotificationPolling()
        {
            var notificationTimer = new Timer();
            notificationTimer.Interval = 10000;
            notificationTimer.Tick += (s, e) => {
                if (this.Visible && !this.IsDisposed)
                {
                    UpdateNotificationBadge();
                }
            };
            notificationTimer.Start();
        }

        private void UpdateNotificationBadge()
        {
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

                        if (this.InvokeRequired)
                            this.Invoke((Action)(() => ApplyNotificationBadgeState()));
                        else
                            ApplyNotificationBadgeState();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating notification badge: " + ex.Message);
            }
        }

        private void ApplyNotificationBadgeState()
        {
            if (lblNotificationBadge == null || btnViewNotifications == null) return;

            if (_unreadNotificationCount > 0)
            {
                lblNotificationBadge.Text = _unreadNotificationCount > 99 ? "99+" : _unreadNotificationCount.ToString();
                lblNotificationBadge.Visible = true;
                btnViewNotifications.ForeColor = Color.FromArgb(240, 71, 71);
            }
            else
            {
                lblNotificationBadge.Visible = false;
                btnViewNotifications.ForeColor = isDarkMode ? Color.White : lunaDarkest;
            }
        }

        private void SetupNotificationsPanel()
        {
            pnlNotifications = new Panel();
            pnlNotifications.BackColor = isDarkMode ? Color.FromArgb(35, 40, 48) : Color.FromArgb(255, 255, 255);
            pnlNotifications.Size = new Size(320, 420);
            pnlNotifications.Location = new Point(this.Width - 340, 80);
            pnlNotifications.Visible = false;
            pnlNotifications.BorderStyle = BorderStyle.None;
            ApplyRoundedCorners(pnlNotifications, 15);

            lblNotificationTitle = new Label();
            lblNotificationTitle.Text = "Notifications";
            lblNotificationTitle.Font = new Font("Segoe UI Semibold", 14, FontStyle.Bold);
            lblNotificationTitle.ForeColor = isDarkMode ? Color.White : lunaDarkest;
            lblNotificationTitle.Location = new Point(15, 15);
            lblNotificationTitle.Size = new Size(200, 30);
            pnlNotifications.Controls.Add(lblNotificationTitle);

            btnCloseNotifications = new Button();
            btnCloseNotifications.Text = "✕";
            btnCloseNotifications.BackColor = Color.Transparent;
            btnCloseNotifications.ForeColor = isDarkMode ? Color.White : lunaDarkest;
            btnCloseNotifications.FlatStyle = FlatStyle.Flat;
            btnCloseNotifications.FlatAppearance.BorderSize = 0;
            btnCloseNotifications.Size = new Size(35, 30);
            btnCloseNotifications.Location = new Point(270, 15);
            btnCloseNotifications.Cursor = Cursors.Hand;
            btnCloseNotifications.Font = new Font("Segoe UI", 12F);
            btnCloseNotifications.Click += (s, e) => pnlNotifications.Visible = false;
            pnlNotifications.Controls.Add(btnCloseNotifications);

            lstNotifications = new ListBox();
            lstNotifications.BackColor = isDarkMode ? Color.FromArgb(45, 50, 58) : Color.FromArgb(248, 250, 252);
            lstNotifications.ForeColor = isDarkMode ? Color.White : lunaDarkest;
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
            btnMarkRead.Click += BtnMarkRead_Click;
            ApplyRoundedCornersToButton(btnMarkRead, 15);
            pnlNotifications.Controls.Add(btnMarkRead);

            btnViewNotifications = new Button();
            btnViewNotifications.Text = "🔔";
            btnViewNotifications.Font = new Font("Segoe UI", 14);
            btnViewNotifications.BackColor = Color.Transparent;
            btnViewNotifications.ForeColor = isDarkMode ? Color.White : lunaDarkest;
            btnViewNotifications.FlatStyle = FlatStyle.Flat;
            btnViewNotifications.FlatAppearance.BorderSize = 0;
            btnViewNotifications.FlatAppearance.MouseOverBackColor = Color.FromArgb(64, 68, 75);
            btnViewNotifications.Size = new Size(45, 40);
            btnViewNotifications.Location = new Point(735, 15);
            btnViewNotifications.Cursor = Cursors.Hand;
            btnViewNotifications.Click += (s, e) => {
                pnlNotifications.Visible = !pnlNotifications.Visible;
                if (pnlNotifications.Visible) LoadNotifications();
            };
            pnlTopBar.Controls.Add(btnViewNotifications);

            lblNotificationBadge = new Label();
            lblNotificationBadge.Text = "";
            lblNotificationBadge.Font = new Font("Segoe UI", 8, FontStyle.Bold);
            lblNotificationBadge.ForeColor = Color.White;
            lblNotificationBadge.BackColor = Color.FromArgb(240, 71, 71);
            lblNotificationBadge.AutoSize = false;
            lblNotificationBadge.Size = new Size(18, 18);
            lblNotificationBadge.Location = new Point(762, 12);
            lblNotificationBadge.TextAlign = ContentAlignment.MiddleCenter;
            lblNotificationBadge.Visible = false;
            lblNotificationBadge.Cursor = Cursors.Hand;
            lblNotificationBadge.Click += (s, e) => {
                pnlNotifications.Visible = !pnlNotifications.Visible;
                if (pnlNotifications.Visible) LoadNotifications();
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

        private void LstNotifications_MeasureItem(object sender, MeasureItemEventArgs e) => e.ItemHeight = 50;

        private void LstNotifications_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;
            e.DrawBackground();
            var notification = lstNotifications.Items[e.Index] as dynamic;
            if (notification != null)
            {
                using (var brush = new SolidBrush(e.ForeColor))
                    e.Graphics.DrawString(notification.Message, e.Font, brush, e.Bounds.X + 5, e.Bounds.Y + 5);
                using (var smallFont = new Font("Segoe UI", 8))
                using (var grayBrush = new SolidBrush(Color.FromArgb(185, 187, 190)))
                    e.Graphics.DrawString(notification.CreatedAt.ToString("MMM dd, HH:mm"), smallFont, grayBrush, e.Bounds.X + 5, e.Bounds.Y + 28);
            }
            e.DrawFocusRectangle();
        }

        private void BtnMarkRead_Click(object sender, EventArgs e)
        {
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
            catch (Exception ex) { Console.WriteLine("Error marking notifications read: " + ex.Message); }
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

        private void AddProfileSection()
        {
            pnlProfileContainer = new Panel { Size = new Size(280, 70), Location = new Point(0, 0), BackColor = Color.Transparent };
            MakeDraggable(pnlProfileContainer);

            pbProfilePicture = new PictureBox { Size = new Size(50, 50), Location = new Point(15, 10), SizeMode = PictureBoxSizeMode.Zoom, BackColor = Color.Transparent, Cursor = Cursors.Hand };
            pbProfilePicture.Click += PbProfilePicture_Click;
            MakeDraggable(pbProfilePicture);

            var lblWelcome = new Label { Text = "Welcome,", Location = new Point(75, 12), Size = new Size(60, 15), Font = new Font("Segoe UI", 9F), ForeColor = isDarkMode ? lunaLight : lunaTeal, BackColor = Color.Transparent };
            MakeDraggable(lblWelcome);

            lblUserGreetingText = new Label { Location = new Point(75, 30), Size = new Size(190, 20), Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold), ForeColor = isDarkMode ? Color.White : lunaDarkest, BackColor = Color.Transparent, Text = _username };
            MakeDraggable(lblUserGreetingText);

            lnkEditProfile = new LinkLabel { Text = "Edit Profile", Location = new Point(75, 50), Size = new Size(70, 15), Font = new Font("Segoe UI", 8F), LinkColor = lunaCyan, ActiveLinkColor = lunaLight, VisitedLinkColor = lunaCyan, Cursor = Cursors.Hand, BackColor = Color.Transparent };
            lnkEditProfile.LinkClicked += LnkEditProfile_LinkClicked;

            pnlProfileContainer.Controls.Add(pbProfilePicture);
            pnlProfileContainer.Controls.Add(lblWelcome);
            pnlProfileContainer.Controls.Add(lblUserGreetingText);
            pnlProfileContainer.Controls.Add(lnkEditProfile);
            pnlTopBar.Controls.Add(pnlProfileContainer);
        }

        private void LoadLogo()
        {
            string logoPath;

            // Choose logo based on theme: logo.png for light mode, logo2.png for dark mode
            if (isDarkMode)
                logoPath = Path.Combine(Application.StartupPath, "logo2.png");
            else
                logoPath = Path.Combine(Application.StartupPath, "logo.png");

            // Check in Logos folder if not found in root
            if (!File.Exists(logoPath))
            {
                string logosFolder = Path.Combine(Application.StartupPath, "Logos");
                if (isDarkMode)
                    logoPath = Path.Combine(logosFolder, "logo2.png");
                else
                    logoPath = Path.Combine(logosFolder, "logo.png");
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

        private void PbProfilePicture_Click(object sender, EventArgs e) => OpenEditProfileForm();
        private void LnkEditProfile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => OpenEditProfileForm();

        private void OpenEditProfileForm()
        {
            var editForm = new EditProfileForm(_connectionString, _username);
            if (editForm.ShowDialog() == DialogResult.OK) LoadUserGreeting();
        }

        private void SetupClearanceSystem()
        {
            lblClearanceTitle.Text = "Department Clearance";
            lblClearanceTitle.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold);
            lblClearanceTitle.ForeColor = isDarkMode ? Color.White : lunaDarkest;
            lblOverallStatus.Text = "Overall Progress: 0/6";
            lblOverallStatus.Font = new Font("Segoe UI", 11F, FontStyle.Regular);
            lblOverallStatus.ForeColor = lunaCyan;
            progressOverall.Style = ProgressBarStyle.Continuous;
            progressOverall.ForeColor = lunaTeal;
            progressOverall.BackColor = isDarkMode ? Color.FromArgb(30, 35, 45) : Color.FromArgb(220, 230, 240);
            progressOverall.Height = 25;
        }

        private async void InitializeSignalR()
        {
            try
            {
                _hubConnection = new HubConnection("http://localhost:8080/");
                _hubProxy = _hubConnection.CreateHubProxy("clearanceHub");
                _hubProxy.On<string, string>("statusUpdated", (department, status) =>
                {
                    this.Invoke((Action)(() =>
                    {
                        UpdateDepartmentStatus(department, status);
                        notifyIcon1.ShowBalloonTip(3000, "Clearance Update", $"{department} clearance has been {status}!", ToolTipIcon.Info);
                        UpdateOverallProgress();
                        UpdateNotificationBadge();
                    }));
                });
                await _hubConnection.Start();
                await _hubProxy.Invoke("JoinStudentGroup", _username);
            }
            catch (Exception ex)
            {
                Console.WriteLine("SignalR Error: " + ex.Message);
                SetupPollingTimer();
            }
        }

        private void SetupPollingTimer()
        {
            _refreshTimer = new Timer { Interval = 5000 };
            _refreshTimer.Tick += (s, e) => LoadClearanceStatusFromDB();
            _refreshTimer.Start();
        }

        private void LoadClearanceStatusFromDB()
        {
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
                                string dept = reader["DepartmentName"].ToString();
                                string status = reader["Status"].ToString();
                                if (status != "Pending")
                                    UpdateDepartmentStatus(dept, status);
                                else
                                {
                                    var controls = GetDepartmentControls(dept);
                                    if (controls.submitButton != null && reader["SubmittedAt"] != DBNull.Value)
                                    {
                                        controls.submitButton.Text = "Submitted!";
                                        controls.submitButton.BackColor = lunaCyan;
                                        controls.submitButton.Enabled = false;
                                        controls.statusLabel.Text = "Pending Review";
                                        controls.statusLabel.ForeColor = lunaCyan;
                                        ApplyRoundedCornersToButton(controls.submitButton, 20);
                                    }
                                }
                            }
                        }
                    }
                }
                UpdateOverallProgress();
            }
            catch (Exception ex) { Console.WriteLine("Error loading status: " + ex.Message); }
        }

        private void UpdateDepartmentStatus(string department, string status)
        {
            var controls = GetDepartmentControls(department);
            if (controls.submitButton != null)
            {
                _clearanceStatus[department] = (status == "Approved");
                if (status == "Approved")
                {
                    controls.submitButton.Text = "Approved";
                    controls.submitButton.BackColor = Color.FromArgb(40, 167, 69);
                    controls.submitButton.Enabled = false;
                    controls.statusLabel.Text = "Approved";
                    controls.statusLabel.ForeColor = Color.FromArgb(40, 167, 69);
                }
                else if (status == "Rejected")
                {
                    controls.submitButton.Text = "Rejected";
                    controls.submitButton.BackColor = Color.FromArgb(240, 71, 71);
                    controls.submitButton.Enabled = true;
                    controls.statusLabel.Text = "Rejected";
                    controls.statusLabel.ForeColor = Color.FromArgb(240, 71, 71);
                    _clearanceStatus[department] = false;
                }
                controls.statusLabel.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
                ApplyRoundedCornersToButton(controls.submitButton, 20);
            }
        }

        private (Button submitButton, Label statusLabel) GetDepartmentControls(string department)
        {
            switch (department)
            {
                case "Library": return (btnLibrarySubmit, lblLibraryStatus);
                case "SAO": return (btnSAOSubmit, lblSAOStatus);
                case "Cashier": return (btnCashierSubmit, lblCashierStatus);
                case "Accounting": return (btnAccountingSubmit, lblAccountingStatus);
                case "Dean's Office": return (btnDeanSubmit, lblDeanStatus);
                case "Records": return (btnRecordsSubmit, lblRecordsStatus);
                default: return (null, null);
            }
        }

        private void UpdateOverallProgress()
        {
            int submittedCount = 0;
            foreach (var dept in _clearanceStatus.Keys)
            {
                var controls = GetDepartmentControls(dept);
                if (controls.statusLabel != null)
                    if (controls.statusLabel.Text == "Approved" || controls.statusLabel.Text == "Pending Review")
                        submittedCount++;
            }
            progressOverall.Maximum = 6;
            progressOverall.Value = submittedCount;
            lblOverallStatus.Text = $"Overall Progress: {submittedCount}/6";
        }

        private void HandleSubmission(string department, Button submitButton, Label statusLabel)
        {
            if (statusLabel.Text == "Approved")
            {
                MessageBox.Show($"{department} clearance has already been approved.", "Already Approved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            var result = MessageBox.Show($"Do you want to submit an image for {department} clearance?", "Confirm Submission", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes && _openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    byte[] imageData;
                    using (var fs = new FileStream(_openFileDialog.FileName, FileMode.Open, FileAccess.Read))
                    {
                        using (var testImage = Image.FromStream(fs))
                        {
                            fs.Position = 0;
                            imageData = new byte[fs.Length];
                            fs.Read(imageData, 0, imageData.Length);
                        }
                    }
                    string fileName = Path.GetFileName(_openFileDialog.FileName);
                    using (var conn = new SqlConnection(_connectionString))
                    {
                        conn.Open();
                        using (var cmd = new SqlCommand("sp_SubmitClearance", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@StudentUsername", _username);
                            cmd.Parameters.AddWithValue("@DepartmentName", department);
                            cmd.Parameters.AddWithValue("@ImageData", imageData);
                            cmd.Parameters.AddWithValue("@ImageFileName", fileName);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    submitButton.Text = "Submitted!";
                    submitButton.BackColor = lunaCyan;
                    submitButton.Enabled = false;
                    statusLabel.Text = "Pending Review";
                    statusLabel.ForeColor = lunaCyan;
                    statusLabel.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
                    UpdateOverallProgress();
                    ApplyRoundedCornersToButton(submitButton, 20);
                    MessageBox.Show($"{department} clearance submitted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (ArgumentException) { MessageBox.Show("The selected file is not a valid image.", "Invalid Image", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                catch (Exception ex) { MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
        }

        private void btnLibrarySubmit_Click(object sender, EventArgs e) => HandleSubmission("Library", btnLibrarySubmit, lblLibraryStatus);
        private void btnSAOSubmit_Click(object sender, EventArgs e) => HandleSubmission("SAO", btnSAOSubmit, lblSAOStatus);
        private void btnCashierSubmit_Click(object sender, EventArgs e) => HandleSubmission("Cashier", btnCashierSubmit, lblCashierStatus);
        private void btnAccountingSubmit_Click(object sender, EventArgs e) => HandleSubmission("Accounting", btnAccountingSubmit, lblAccountingStatus);
        private void btnDeanSubmit_Click(object sender, EventArgs e) => HandleSubmission("Dean's Office", btnDeanSubmit, lblDeanStatus);
        private void btnRecordsSubmit_Click(object sender, EventArgs e) => HandleSubmission("Records", btnRecordsSubmit, lblRecordsStatus);

        private void btnLogout_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to logout?", "Confirm Logout",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                _loggedOut = true;
                _hubConnection?.Stop();
                _refreshTimer?.Stop();
                _appContext?.OpenLoginForm();
                this.Close();
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _hubConnection?.Stop();
            _refreshTimer?.Stop();
            base.OnFormClosing(e);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (pnlNotifications != null)
                pnlNotifications.Location = new Point(this.Width - 340, 80);
        }

        private void pnlClearance_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}