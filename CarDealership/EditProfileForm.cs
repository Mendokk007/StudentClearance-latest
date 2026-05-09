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
    public partial class EditProfileForm : Form
    {
        private readonly string _connectionString;
        private readonly string _username;
        private byte[] _profileImageData;
        private bool _imageChanged = false;
        private Image _currentProfileImage = null;

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

        public EditProfileForm(string connectionString, string username)
        {
            this.DoubleBuffered = true;
            InitializeComponent();
            _connectionString = connectionString;
            _username = username;

            ApplyRoundedCorners(this, 20);
            AddThemeToggleButton();

            MakeDraggable(pnlTopBar);
            MakeDraggable(lblTitle);

            SetupProfilePanel();
            ApplyTheme();
            LoadUserData();
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
            btnThemeToggle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnThemeToggle.ForeColor = Color.White;
            btnThemeToggle.Location = new Point(255, 8);
            btnThemeToggle.Size = new Size(25, 25);
            btnThemeToggle.Text = "🌙";
            btnThemeToggle.UseVisualStyleBackColor = false;
            btnThemeToggle.Click += (s, e) => {
                isDarkMode = !isDarkMode;
                ApplyTheme();
                btnThemeToggle.Text = isDarkMode ? "🌙" : "☀️";
            };
            pnlTopBar.Controls.Add(btnThemeToggle);
        }

        private void SetupProfilePanel()
        {
            pnlProfileImage = new Panel();
            pnlProfileImage.Size = new Size(100, 100);
            pnlProfileImage.Location = new Point(10, 10);
            pnlProfileImage.Cursor = Cursors.Hand;
            pnlProfileImage.Click += ProfilePanel_Click;
            pnlProfileImage.Paint += PnlProfileImage_Paint;

            pnlProfileBg.Controls.Add(pnlProfileImage);
        }

        private void PnlProfileImage_Paint(object sender, PaintEventArgs e)
        {
            if (_currentProfileImage != null)
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

                using (var path = new GraphicsPath())
                {
                    path.AddEllipse(0, 0, pnlProfileImage.Width - 1, pnlProfileImage.Height - 1);
                    e.Graphics.SetClip(path);
                    e.Graphics.DrawImage(_currentProfileImage, 0, 0, pnlProfileImage.Width, pnlProfileImage.Height);
                }

                using (var pen = new Pen(lunaTeal, 2))
                {
                    e.Graphics.DrawEllipse(pen, 0, 0, pnlProfileImage.Width - 1, pnlProfileImage.Height - 1);
                }
            }
        }

        private void ProfilePanel_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp|All Files|*.*";
                ofd.Title = "Select Profile Picture";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        _profileImageData = File.ReadAllBytes(ofd.FileName);
                        using (var ms = new MemoryStream(_profileImageData))
                        {
                            _currentProfileImage = Image.FromStream(ms);
                        }
                        _imageChanged = true;
                        pnlProfileImage.Invalidate();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error loading image: " + ex.Message, "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void ApplyTheme()
        {
            this.SuspendLayout();
            pnlTopBar.SuspendLayout();

            try
            {
                // Form background
                this.BackColor = isDarkMode ? lunaDarkest : Color.FromArgb(240, 248, 255);

                // Top bar
                pnlTopBar.BackColor = isDarkMode ? Color.FromArgb(1, 20, 50) : Color.FromArgb(220, 235, 250);

                // Colors
                Color textColor = isDarkMode ? Color.White : lunaDarkest;
                Color inputBg = isDarkMode ? Color.FromArgb(1, 20, 50) : Color.White;
                Color panelBg = isDarkMode ? Color.FromArgb(1, 20, 50) : Color.FromArgb(220, 235, 250);

                // Labels
                lblTitle.ForeColor = textColor;
                lblTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
                lblFullName.ForeColor = textColor;
                lblEmail.ForeColor = textColor;

                // TextBoxes
                txtFullName.BackColor = inputBg;
                txtFullName.ForeColor = textColor;
                txtFullName.BorderStyle = BorderStyle.FixedSingle;

                txtEmail.BackColor = inputBg;
                txtEmail.ForeColor = textColor;
                txtEmail.BorderStyle = BorderStyle.FixedSingle;

                // Profile background panel
                pnlProfileBg.BackColor = panelBg;
                ApplyRoundedCorners(pnlProfileBg, 15);

                // Buttons
                btnSave.BackColor = lunaTeal;
                btnSave.ForeColor = Color.White;
                btnSave.FlatStyle = FlatStyle.Flat;
                btnSave.FlatAppearance.BorderSize = 0;
                ApplyRoundedCornersToButton(btnSave, 15);

                btnCancel.BackColor = isDarkMode ? Color.FromArgb(80, 80, 90) : Color.FromArgb(180, 185, 195);
                btnCancel.ForeColor = Color.White;
                btnCancel.FlatStyle = FlatStyle.Flat;
                btnCancel.FlatAppearance.BorderSize = 0;
                ApplyRoundedCornersToButton(btnCancel, 15);

                // Close button
                btnClose.BackColor = Color.Transparent;
                btnClose.ForeColor = textColor;
                btnClose.FlatStyle = FlatStyle.Flat;
                btnClose.FlatAppearance.BorderSize = 0;
                btnClose.FlatAppearance.MouseOverBackColor = lunaTeal;
                btnClose.FlatAppearance.MouseDownBackColor = lunaCyan;

                // Theme toggle
                if (btnThemeToggle != null)
                {
                    btnThemeToggle.ForeColor = textColor;
                    btnThemeToggle.FlatAppearance.MouseOverBackColor = Color.Transparent;
                }

                // Redraw profile circle border
                pnlProfileImage.Invalidate();
            }
            finally
            {
                pnlTopBar.ResumeLayout(false);
                pnlTopBar.PerformLayout();
                this.ResumeLayout(false);
                this.PerformLayout();
            }
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

        private void LoadUserData()
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string query = "SELECT FullName, Email, ProfileImage FROM Users WHERE Username = @Username";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", _username);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtFullName.Text = reader["FullName"]?.ToString() ?? "";
                                txtEmail.Text = reader["Email"]?.ToString() ?? "";

                                if (reader["ProfileImage"] != DBNull.Value)
                                {
                                    _profileImageData = (byte[])reader["ProfileImage"];
                                    using (var ms = new MemoryStream(_profileImageData))
                                    {
                                        _currentProfileImage = Image.FromStream(ms);
                                    }
                                }
                                else
                                {
                                    _currentProfileImage = CreateDefaultProfileImage();
                                }
                                pnlProfileImage.Invalidate();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading profile: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Image CreateDefaultProfileImage()
        {
            var bmp = new Bitmap(100, 100);
            using (var g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;

                // LunaTeal background
                using (var brush = new SolidBrush(lunaTeal))
                {
                    g.FillEllipse(brush, 0, 0, 99, 99);
                }

                using (var font = new Font("Segoe UI", 32, FontStyle.Bold))
                using (var brush = new SolidBrush(Color.White))
                {
                    var sf = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };

                    string initial = _username.Length > 0 ? _username[0].ToString().ToUpper() : "?";
                    g.DrawString(initial, font, brush, new Rectangle(0, 0, 100, 100), sf);
                }
            }
            return bmp;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    string query;
                    SqlCommand cmd;

                    if (_imageChanged && _profileImageData != null)
                    {
                        query = @"UPDATE Users 
                                 SET FullName = @FullName, 
                                     Email = @Email, 
                                     ProfileImage = @ProfileImage 
                                 WHERE Username = @Username";
                        cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@ProfileImage", _profileImageData);
                    }
                    else
                    {
                        query = @"UPDATE Users 
                                 SET FullName = @FullName, 
                                     Email = @Email 
                                 WHERE Username = @Username";
                        cmd = new SqlCommand(query, conn);
                    }

                    cmd.Parameters.AddWithValue("@FullName", string.IsNullOrEmpty(txtFullName.Text.Trim()) ? (object)DBNull.Value : txtFullName.Text.Trim());
                    cmd.Parameters.AddWithValue("@Email", string.IsNullOrEmpty(txtEmail.Text.Trim()) ? (object)DBNull.Value : txtEmail.Text.Trim());
                    cmd.Parameters.AddWithValue("@Username", _username);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Profile updated successfully!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("No changes were made.", "Information",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving profile: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}