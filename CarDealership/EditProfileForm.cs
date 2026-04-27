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

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        public EditProfileForm(string connectionString, string username)
        {
            InitializeComponent();
            _connectionString = connectionString;
            _username = username;

            MakeDraggable(pnlTopBar);
            MakeDraggable(lblTitle);

            SetupProfilePanel();
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

                using (var pen = new Pen(Color.FromArgb(80, 80, 85), 1))
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

                using (var brush = new SolidBrush(Color.FromArgb(217, 55, 55)))
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

        private void btnClose_MouseEnter(object sender, EventArgs e)
        {
            btnClose.BackColor = Color.FromArgb(217, 55, 55);
        }

        private void btnClose_MouseLeave(object sender, EventArgs e)
        {
            btnClose.BackColor = Color.FromArgb(240, 71, 71);
        }
    }
}