using Microsoft.AspNet.SignalR.Client;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CarDealership
{
    public partial class AdminForm : Form
    {
        private readonly string _connectionString;
        private readonly string _username;
        private readonly string _department;
        private IHubProxy _hubProxy;
        private HubConnection _hubConnection;
        private Timer _notificationTimer;
        private bool _signalRConnected = false;
        private AppContext _appContext;

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        public AdminForm(string connectionString, string username)
        {
            InitializeComponent();
            _connectionString = connectionString;
            _username = username;
            _department = GetAdminDepartment();

            this.Text = $"Admin Dashboard - {_department}";
            lblDepartment.Text = _department;
            lblAdminName.Text = $"Welcome, {_username}";

            MakeDraggable(pnlTopBar);
            MakeDraggable(pbLogo);
            MakeDraggable(lblDepartment);
            MakeDraggable(lblAdminName);

            SetupLogo();
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

        private string GetAdminDepartment()
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string query = "SELECT Department FROM Users WHERE Username = @Username";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", _username);
                        return cmd.ExecuteScalar()?.ToString() ?? "";
                    }
                }
            }
            catch
            {
                return "";
            }
        }

        private void SetupLogo()
        {
            var bmp = new Bitmap(100, 50);
            using (var g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Transparent);
                using (var font = new Font("Segoe UI", 10, FontStyle.Bold))
                using (var brush = new SolidBrush(Color.White))
                {
                    var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                    g.DrawString("ADMIN", font, brush, new Rectangle(0, 0, 100, 25), sf);
                    g.DrawString("PANEL", new Font("Segoe UI", 8, FontStyle.Bold), brush, new Rectangle(0, 25, 100, 25), sf);
                }
            }
            pbLogo.Image = bmp;
        }

        private async void InitializeSignalR()
        {
            try
            {
                _hubConnection = new HubConnection("http://localhost:8080/");
                _hubProxy = _hubConnection.CreateHubProxy("clearanceHub");

                _hubProxy.On<string, string>("newSubmission", (message, studentUsername) =>
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
                            Console.WriteLine("SignalR connection closed for admin");
                            _signalRConnected = false;
                            SetupNotificationTimer(); // Fallback to polling
                        }));
                    }
                };

                await _hubConnection.Start();
                _signalRConnected = true;
                await _hubProxy.Invoke("JoinAdminGroup", _department);
                Console.WriteLine($"Admin {_username} connected to SignalR for department {_department}");
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
                    using (var cmd = new SqlCommand("sp_GetPendingSubmissions", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@DepartmentName", _department);

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
                Console.WriteLine("Error loading submissions: " + ex.Message);
            }
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

                var reviewForm = new ReviewSubmissionForm(_connectionString, submissionId,
                    studentName, _department, imageData, _username);
                reviewForm.OnReviewComplete += () =>
                {
                    LoadPendingSubmissions();

                    if (_signalRConnected && _hubProxy != null && !string.IsNullOrEmpty(studentUsername))
                    {
                        try
                        {
                            _hubProxy.Invoke("NotifyStatusUpdate", studentUsername, _department, "Reviewed");
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
                // Clean up SignalR
                try
                {
                    if (_hubConnection != null && _hubConnection.State == Microsoft.AspNet.SignalR.Client.ConnectionState.Connected)
                    {
                        _hubProxy?.Invoke("LeaveAdminGroup", _department);
                        _hubConnection.Stop();
                        _hubConnection = null;
                    }
                }
                catch { }

                // Stop timer
                if (_notificationTimer != null)
                {
                    _notificationTimer.Stop();
                    _notificationTimer.Dispose();
                    _notificationTimer = null;
                }

                // Close the form
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