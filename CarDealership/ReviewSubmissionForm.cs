using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CarDealership
{
    public partial class ReviewSubmissionForm : Form
    {
        private readonly string _connectionString;
        private readonly int _submissionId;
        private readonly string _studentName;
        private readonly string _department;
        private readonly byte[] _imageData;
        private readonly string _reviewer;

        public event Action OnReviewComplete;

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        public ReviewSubmissionForm(string connectionString, int submissionId,
            string studentName, string department, byte[] imageData, string reviewer)
        {
            InitializeComponent();
            _connectionString = connectionString;
            _submissionId = submissionId;
            _studentName = studentName;
            _department = department;
            _imageData = imageData;
            _reviewer = reviewer;

            lblStudent.Text = $"Student: {studentName}";
            lblDepartment.Text = $"Department: {department}";

            MakeDraggable(pnlTopBar);
            MakeDraggable(lblTitle);

            LoadImage();
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

        private void LoadImage()
        {
            try
            {
                if (_imageData != null && _imageData.Length > 0)
                {
                    using (var ms = new MemoryStream(_imageData))
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
                            CreatePlaceholderImage("Error Loading Image");
                        }
                    }
                }
                else
                {
                    CreatePlaceholderImage("No Image Submitted");
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
                g.Clear(Color.FromArgb(47, 49, 54));
                using (var font = new Font("Segoe UI", 12))
                using (var brush = new SolidBrush(Color.FromArgb(185, 187, 190)))
                {
                    var sf = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };
                    g.DrawString(message, font, brush, new Rectangle(0, 0, 550, 250), sf);
                }

                using (var pen = new Pen(Color.FromArgb(80, 80, 85), 1))
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
                    using (var cmd = new SqlCommand("sp_ReviewClearance", conn))
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