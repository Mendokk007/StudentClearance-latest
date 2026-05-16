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
    public partial class ReviewSubjectForm : Form
    {
        private readonly string _connectionString;
        private readonly int _submissionId;
        private readonly string _studentName;
        private readonly string _subject;
        private readonly byte[] _fileData;
        private readonly string _reviewer;

        // Luna Theme
        private bool isDarkMode = true;
        Color lunaDarkest = Color.FromArgb(1, 28, 64);
        Color lunaTeal = Color.FromArgb(38, 101, 140);
        Color lunaCyan = Color.FromArgb(84, 172, 191);
        Color lunaLight = Color.FromArgb(167, 235, 242);

        public event Action OnReviewComplete;

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

        public ReviewSubjectForm(string connectionString, int submissionId,
            string studentName, string subject, byte[] fileData, string reviewer)
        {
            this.DoubleBuffered = true;
            InitializeComponent();
            _connectionString = connectionString;
            _submissionId = submissionId;
            _studentName = studentName;
            _subject = subject;
            _fileData = fileData;
            _reviewer = reviewer;

            lblStudent.Text = $"Student: {studentName}";
            lblSubject.Text = $"Subject: {subject}";

            ApplyRoundedCorners(this, 20);
            ApplyTheme();

            MakeDraggable(pnlTopBar);
            MakeDraggable(lblTitle);

            LoadFile();
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

        private void ApplyTheme()
        {
            this.SuspendLayout();
            pnlTopBar.SuspendLayout();

            try
            {
                this.BackColor = isDarkMode ? lunaDarkest : Color.FromArgb(240, 248, 255);
                pnlTopBar.BackColor = isDarkMode ? Color.FromArgb(1, 20, 50) : Color.FromArgb(220, 235, 250);

                Color textColor = isDarkMode ? Color.White : lunaDarkest;
                Color accentColor = lunaCyan;
                Color inputBg = isDarkMode ? Color.FromArgb(1, 20, 50) : Color.White;

                lblTitle.ForeColor = textColor;
                lblStudent.ForeColor = textColor;
                lblSubject.ForeColor = accentColor;
                lblReason.ForeColor = isDarkMode ? Color.FromArgb(185, 187, 190) : Color.FromArgb(80, 80, 80);

                pbSubmission.BackColor = isDarkMode ? Color.FromArgb(1, 20, 50) : Color.FromArgb(230, 235, 240);

                txtRejectionReason.BackColor = inputBg;
                txtRejectionReason.ForeColor = textColor;
                txtRejectionReason.BorderStyle = BorderStyle.FixedSingle;

                btnApprove.BackColor = lunaTeal;
                btnApprove.ForeColor = Color.White;
                btnApprove.FlatStyle = FlatStyle.Flat;
                btnApprove.FlatAppearance.BorderSize = 0;
                ApplyRoundedCornersToButton(btnApprove, 20);

                btnReject.BackColor = Color.FromArgb(180, 60, 80);
                btnReject.ForeColor = Color.White;
                btnReject.FlatStyle = FlatStyle.Flat;
                btnReject.FlatAppearance.BorderSize = 0;
                ApplyRoundedCornersToButton(btnReject, 20);

                btnCancel.BackColor = isDarkMode ? Color.FromArgb(80, 80, 90) : Color.FromArgb(180, 185, 195);
                btnCancel.ForeColor = Color.White;
                btnCancel.FlatStyle = FlatStyle.Flat;
                btnCancel.FlatAppearance.BorderSize = 0;
                ApplyRoundedCornersToButton(btnCancel, 20);
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

        private void LoadFile()
        {
            try
            {
                if (_fileData != null && _fileData.Length > 0)
                {
                    using (var ms = new MemoryStream(_fileData))
                    {
                        try
                        {
                            var img = Image.FromStream(ms);
                            pbSubmission.Image = new Bitmap(img);
                        }
                        catch (ArgumentException)
                        {
                            CreatePlaceholderImage("Invalid Image Format");
                        }
                        catch (Exception)
                        {
                            CreatePlaceholderImage("Error Loading File");
                        }
                    }
                }
                else
                {
                    CreatePlaceholderImage("No File Submitted");
                }
            }
            catch (Exception ex)
            {
                CreatePlaceholderImage($"Error: {ex.Message}");
            }
        }

        private void CreatePlaceholderImage(string message)
        {
            var bmp = new Bitmap(550, 250);
            using (var g = Graphics.FromImage(bmp))
            {
                g.Clear(isDarkMode ? Color.FromArgb(1, 20, 50) : Color.FromArgb(230, 235, 240));
                using (var font = new Font("Segoe UI", 12))
                using (var brush = new SolidBrush(isDarkMode ? Color.FromArgb(185, 187, 190) : Color.FromArgb(80, 80, 80)))
                {
                    var sf = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };
                    g.DrawString(message, font, brush, new Rectangle(0, 0, 550, 250), sf);
                }

                using (var pen = new Pen(lunaTeal, 1))
                {
                    g.DrawRectangle(pen, 0, 0, 549, 249);
                }
            }
            pbSubmission.Image = bmp;
        }

        private void btnApprove_Click(object sender, EventArgs e)
        {
            ProcessReview("Approved", null);
        }

        private void btnReject_Click(object sender, EventArgs e)
        {
            string reason = txtRejectionReason.Text.Trim();
            if (string.IsNullOrEmpty(reason))
            {
                MessageBox.Show("Please provide a reason for rejection.",
                    "Reason Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            ProcessReview("Rejected", reason);
        }

        private void ProcessReview(string status, string reason)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("sp_ReviewSubjectClearance", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@SubmissionID", _submissionId);
                        cmd.Parameters.AddWithValue("@Status", status);
                        cmd.Parameters.AddWithValue("@RejectionReason", (object)reason ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@ReviewedBy", _reviewer);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Successfully processed
                            }
                        }
                    }
                }

                MessageBox.Show($"Submission {status.ToLower()} successfully!",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                OnReviewComplete?.Invoke();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error processing review: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}