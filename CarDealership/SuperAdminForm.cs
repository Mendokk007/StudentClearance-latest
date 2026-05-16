using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CarDealership
{
    public partial class SuperAdminForm : Form
    {
        // =============================================
        // FIELDS
        // =============================================
        private readonly string _connectionString;
        private readonly string _username;
        private AppContext _appContext;

        // Luna Theme
        private bool isDarkMode = true;
        Color lunaDarkest = Color.FromArgb(1, 28, 64);
        Color lunaTeal = Color.FromArgb(38, 101, 140);
        Color lunaCyan = Color.FromArgb(84, 172, 191);
        Color lunaLight = Color.FromArgb(167, 235, 242);

        // Activity Log
        private Panel pnlActivityLog;
        private ListBox lstActivityLogs;
        private Label lblActivityLogTitle;
        private Button btnCloseActivityLog;
        private Button btnViewActivityLog;
        private DateTimePicker dtpLogStart;
        private DateTimePicker dtpLogEnd;
        private Button btnDownloadLogs;

        // Win32 API for dragging
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
        public SuperAdminForm(string connectionString, string username)
        {
            this.DoubleBuffered = true;
            InitializeComponent();
            _connectionString = connectionString;
            _username = username;

            ApplyRoundedCorners(this, 30);
            MakeDraggable(pnlTopBar);
            MakeDraggable(pbLogo);

            SetupLogo();
            SetupActivityLogPanel();
            ApplyTheme();
            WireUpEvents();
            LoadAllPrograms();
            LogActivity("Logged in to Super Admin Panel");
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
            control.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    ReleaseCapture();
                    SendMessage(this.Handle, 0xA1, 0x2, 0);
                }
            };
        }

        private void LogActivity(string message)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO ActivityLogs (Username, Message, LogType) VALUES (@User, @Msg, @Type)";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@User", _username);
                        cmd.Parameters.AddWithValue("@Msg", message);
                        cmd.Parameters.AddWithValue("@Type", "System");
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch { }
        }

        private void ExecuteNonQuery(string spName, params (string, object)[] parameters)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand(spName, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        foreach (var p in parameters)
                            cmd.Parameters.AddWithValue(p.Item1, p.Item2 ?? DBNull.Value);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private string PromptDialog(string message, string title)
        {
            using (var form = new Form
            {
                Text = title,
                Size = new Size(350, 160),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            })
            {
                var label = new Label
                {
                    Text = message,
                    Location = new Point(15, 15),
                    Size = new Size(300, 20)
                };
                var textBox = new TextBox
                {
                    Location = new Point(15, 40),
                    Size = new Size(300, 25)
                };
                var btnOk = new Button
                {
                    Text = "OK",
                    Location = new Point(140, 80),
                    Size = new Size(80, 30),
                    DialogResult = DialogResult.OK
                };

                form.Controls.Add(label);
                form.Controls.Add(textBox);
                form.Controls.Add(btnOk);
                form.AcceptButton = btnOk;

                return form.ShowDialog() == DialogResult.OK ? textBox.Text : null;
            }
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
                    g.DrawString("SUPER", font, brush, new Rectangle(0, 0, 100, 25), sf);
                    g.DrawString("ADMIN", new Font("Segoe UI", 8, FontStyle.Bold), brush, new Rectangle(0, 25, 100, 25), sf);
                }
            }
            pbLogo.Image = bmp;
        }

        // =============================================
        // ACTIVITY LOG PANEL
        // =============================================
        private void SetupActivityLogPanel()
        {
            pnlActivityLog = new Panel
            {
                BackColor = isDarkMode ? Color.FromArgb(1, 28, 64) : Color.FromArgb(240, 248, 255),
                Size = new Size(320, 450),
                Location = new Point(this.Width - 340, 80),
                Visible = false,
                BorderStyle = BorderStyle.None
            };
            ApplyRoundedCorners(pnlActivityLog, 15);

            lblActivityLogTitle = new Label
            {
                Text = "Activity Log",
                Font = new Font("Segoe UI Semibold", 14, FontStyle.Bold),
                ForeColor = isDarkMode ? lunaLight : lunaTeal,
                Location = new Point(15, 15),
                Size = new Size(200, 30)
            };
            pnlActivityLog.Controls.Add(lblActivityLogTitle);

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

            lstActivityLogs = new ListBox
            {
                BackColor = isDarkMode ? Color.FromArgb(1, 28, 64) : Color.FromArgb(255, 255, 255),
                ForeColor = isDarkMode ? lunaLight : lunaDarkest,
                BorderStyle = BorderStyle.None,
                Location = new Point(15, 55),
                Size = new Size(290, 280),
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

            // Date pickers
            var lblStart = new Label
            {
                Text = "From:",
                Location = new Point(15, 345),
                Size = new Size(40, 20),
                ForeColor = isDarkMode ? lunaLight : lunaTeal,
                Font = new Font("Segoe UI", 8F)
            };
            pnlActivityLog.Controls.Add(lblStart);

            dtpLogStart = new DateTimePicker
            {
                Location = new Point(55, 342),
                Size = new Size(120, 25),
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Today
            };
            pnlActivityLog.Controls.Add(dtpLogStart);

            var lblEnd = new Label
            {
                Text = "To:",
                Location = new Point(180, 345),
                Size = new Size(25, 20),
                ForeColor = isDarkMode ? lunaLight : lunaTeal,
                Font = new Font("Segoe UI", 8F)
            };
            pnlActivityLog.Controls.Add(lblEnd);

            dtpLogEnd = new DateTimePicker
            {
                Location = new Point(205, 342),
                Size = new Size(120, 25),
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Today
            };
            pnlActivityLog.Controls.Add(dtpLogEnd);

            btnDownloadLogs = new Button
            {
                Text = "Download CSV",
                Location = new Point(15, 375),
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

            btnViewActivityLog = new Button
            {
                Text = "📋",
                Font = new Font("Segoe UI", 14),
                BackColor = Color.Transparent,
                ForeColor = isDarkMode ? Color.White : lunaDarkest,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(45, 40),
                Location = new Point(785, 15),
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
                Console.WriteLine("Error: " + ex.Message);
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
                            MessageBox.Show("No logs found for this date range.");
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
                                MessageBox.Show("Logs downloaded successfully!");
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
        // EVENT WIRING
        // =============================================
        private void WireUpEvents()
        {
            btnThemeToggle.Click += (s, e) =>
            {
                isDarkMode = !isDarkMode;
                ApplyTheme();
                btnThemeToggle.Text = isDarkMode ? "🌙" : "☀️";
            };

            btnLogout.Click += (s, e) =>
            {
                LogActivity("Logged out");
                this.Close();
            };

            // Tab navigation
            btnProgramsTab.Click += (s, e) => ShowTab("programs");
            btnSubjectsTab.Click += (s, e) => ShowTab("subjects");
            btnTeachersTab.Click += (s, e) => ShowTab("teachers");
            btnStudentsTab.Click += (s, e) => ShowTab("students");

            // Programs tab
            btnAddProgram.Click += (s, e) => { AddProgram(); LogActivity("Added a new program"); };
            btnEditProgram.Click += (s, e) => EditProgramSubjects();
            btnDeleteProgram.Click += (s, e) => { DeleteProgram(); LogActivity("Deleted a program"); };

            // Subjects tab
            btnAddSubject.Click += (s, e) => { AddSubject(); LogActivity("Added a new subject"); };
            btnEditSubject.Click += (s, e) => { EditSubject(); LogActivity("Edited a subject"); };
            btnDeleteSubject.Click += (s, e) => { DeleteSubject(); LogActivity("Deleted a subject"); };
            btnManageTeachers.Click += (s, e) => ShowTeacherList();

            // Teachers tab
            btnCreateTeacher.Click += (s, e) => { CreateTeacher(); LogActivity("Created a teacher account"); };
            btnEditTeacher.Click += (s, e) => { EditTeacher(); LogActivity("Edited a teacher"); };
            btnEditCoverage.Click += (s, e) => EditCoverage();
            btnResetTeacherPassword.Click += (s, e) => { ResetTeacherPassword(); LogActivity("Reset teacher password"); };
            btnDeleteTeacher.Click += (s, e) => { DeleteTeacher(); LogActivity("Deleted a teacher account"); };

            // Students tab
            btnCreateStudent.Click += (s, e) => { CreateStudent(); LogActivity("Created a student account"); };
            btnEditStudent.Click += (s, e) => { EditStudent(); LogActivity("Edited a student"); };
            btnResetStudentPassword.Click += (s, e) => { ResetStudentPassword(); LogActivity("Reset student password"); };
            btnDeleteStudent.Click += (s, e) => { DeleteStudent(); LogActivity("Deleted a student account"); };
        }

        // =============================================
        // TAB NAVIGATION
        // =============================================
        private void ShowTab(string tab)
        {
            pnlProgramsTab.Visible = (tab == "programs");
            pnlSubjectsTab.Visible = (tab == "subjects");
            pnlTeachersTab.Visible = (tab == "teachers");
            pnlStudentsTab.Visible = (tab == "students");

            btnProgramsTab.BackColor = (tab == "programs") ? lunaCyan : lunaTeal;
            btnSubjectsTab.BackColor = (tab == "subjects") ? lunaCyan : lunaTeal;
            btnTeachersTab.BackColor = (tab == "teachers") ? lunaCyan : lunaTeal;
            btnStudentsTab.BackColor = (tab == "students") ? lunaCyan : lunaTeal;

            if (tab == "programs") LoadAllPrograms();
            else if (tab == "subjects") LoadAllSubjects();
            else if (tab == "teachers") LoadAllTeachers();
            else if (tab == "students") LoadAllStudents();
        }

        // =============================================
        // DATA LOADING
        // =============================================
        private void LoadAllPrograms()
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("sp_GetAllPrograms", conn))
                    using (var adapter = new SqlDataAdapter(cmd))
                    {
                        var dt = new DataTable();
                        adapter.Fill(dt);
                        dgvPrograms.DataSource = dt;
                    }
                }
                StyleGrid(dgvPrograms);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading programs: " + ex.Message);
            }
        }

        private void LoadAllSubjects()
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string query = @"SELECT SubjectID, ProgramCode, SubjectName, DisplayOrder 
                                     FROM Subjects ORDER BY ProgramCode, DisplayOrder";
                    using (var cmd = new SqlCommand(query, conn))
                    using (var adapter = new SqlDataAdapter(cmd))
                    {
                        var dt = new DataTable();
                        adapter.Fill(dt);
                        dgvAllSubjects.DataSource = dt;
                    }
                }
                StyleGrid(dgvAllSubjects);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading subjects: " + ex.Message);
            }
        }

        private void LoadAllTeachers()
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("sp_GetAllTeachers", conn))
                    using (var adapter = new SqlDataAdapter(cmd))
                    {
                        var dt = new DataTable();
                        adapter.Fill(dt);
                        dgvTeachers.DataSource = dt;
                    }
                }
                StyleGrid(dgvTeachers);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading teachers: " + ex.Message);
            }
        }

        private void LoadAllStudents()
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("sp_GetAllStudents", conn))
                    using (var adapter = new SqlDataAdapter(cmd))
                    {
                        var dt = new DataTable();
                        adapter.Fill(dt);
                        dgvStudents.DataSource = dt;
                    }
                }
                StyleGrid(dgvStudents);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading students: " + ex.Message);
            }
        }

        private void StyleGrid(DataGridView dgv)
        {
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.ReadOnly = true;
            dgv.RowHeadersVisible = false;
            dgv.BackgroundColor = isDarkMode ? Color.FromArgb(1, 28, 64) : Color.FromArgb(240, 240, 245);
            dgv.ColumnHeadersDefaultCellStyle.BackColor = lunaTeal;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.DefaultCellStyle.BackColor = isDarkMode ? Color.FromArgb(1, 28, 64) : Color.White;
            dgv.DefaultCellStyle.ForeColor = isDarkMode ? Color.White : lunaDarkest;
            dgv.DefaultCellStyle.SelectionBackColor = lunaCyan;
            dgv.EnableHeadersVisualStyles = false;
            dgv.BorderStyle = BorderStyle.None;
        }

        // =============================================
        // PROGRAMS TAB
        // =============================================
        private void AddProgram()
        {
            string code = PromptDialog("Program Code:", "Add Program");
            if (string.IsNullOrEmpty(code)) return;
            code = code.ToUpper();

            string name = PromptDialog("Program Name:", "Add Program");
            if (string.IsNullOrEmpty(name)) return;

            ExecuteNonQuery("sp_AddProgram", ("@ProgramCode", code), ("@ProgramName", name));
            LoadAllPrograms();
        }

        private void EditProgramSubjects()
        {
            if (dgvPrograms.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select a program first.");
                return;
            }
            ShowTab("subjects");
        }

        private void DeleteProgram()
        {
            if (dgvPrograms.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select a program first.");
                return;
            }

            string code = dgvPrograms.SelectedRows[0].Cells["ProgramCode"].Value.ToString();

            // Check if any students are enrolled in this program
            int studentCount = 0;
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand(
                        "SELECT COUNT(*) FROM Users WHERE Program = @Program AND Role = 'Student'", conn))
                    {
                        cmd.Parameters.AddWithValue("@Program", code);
                        studentCount = (int)cmd.ExecuteScalar();
                    }
                }
            }
            catch { }

            if (studentCount > 0)
            {
                MessageBox.Show(
                    $"Cannot delete '{code}' because {studentCount} student(s) are currently enrolled in this program.\n\n" +
                    "Please reassign or remove those students first.",
                    "Cannot Delete",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"Delete program '{code}' and all its subjects?",
                "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    ExecuteNonQuery("sp_DeleteProgram", ("@ProgramCode", code));
                    LoadAllPrograms();
                    LogActivity($"Deleted program {code}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Cannot delete: " + ex.Message);
                }
            }
        }

        // =============================================
        // SUBJECTS TAB
        // =============================================
        private void AddSubject()
        {
            string code = PromptDialog("Program Code:", "Add Subject");
            if (string.IsNullOrEmpty(code)) return;
            code = code.ToUpper();

            string name = PromptDialog("Subject Name:", "Add Subject");
            if (string.IsNullOrEmpty(name)) return;

            string order = PromptDialog("Display Order:", "Add Subject");
            int.TryParse(order, out int displayOrder);

            ExecuteNonQuery("sp_AddSubject",
                ("@ProgramCode", code),
                ("@SubjectName", name),
                ("@DisplayOrder", displayOrder));
            LoadAllSubjects();
        }

        private void EditSubject()
        {
            if (dgvAllSubjects.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select a subject first.");
                return;
            }

            int id = Convert.ToInt32(dgvAllSubjects.SelectedRows[0].Cells["SubjectID"].Value);

            string name = PromptDialog("New Subject Name:", "Edit Subject");
            if (string.IsNullOrEmpty(name)) return;

            string order = PromptDialog("New Display Order:", "Edit Subject");
            int.TryParse(order, out int displayOrder);

            ExecuteNonQuery("sp_EditSubject",
                ("@SubjectID", id),
                ("@SubjectName", name),
                ("@DisplayOrder", displayOrder));
            LoadAllSubjects();
        }

        private void DeleteSubject()
        {
            if (dgvAllSubjects.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select a subject first.");
                return;
            }

            int id = Convert.ToInt32(dgvAllSubjects.SelectedRows[0].Cells["SubjectID"].Value);
            string subjectName = dgvAllSubjects.SelectedRows[0].Cells["SubjectName"].Value.ToString();

            if (MessageBox.Show($"Delete subject '{subjectName}'?",
                "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    ExecuteNonQuery("sp_DeleteSubject", ("@SubjectID", id));
                    LoadAllSubjects();
                    LogActivity($"Deleted subject {subjectName}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Cannot delete: " + ex.Message);
                }
            }
        }

        private void ShowTeacherList()
        {
            if (dgvAllSubjects.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select a subject first.");
                return;
            }

            string subject = dgvAllSubjects.SelectedRows[0].Cells["SubjectName"].Value.ToString();
            string code = dgvAllSubjects.SelectedRows[0].Cells["ProgramCode"].Value.ToString();

            var form = new Form
            {
                Text = $"Teachers - {subject}",
                Size = new Size(350, 300),
                StartPosition = FormStartPosition.CenterParent
            };
            form.BackColor = isDarkMode ? lunaDarkest : Color.FromArgb(240, 248, 255);

            var listBox = new ListBox
            {
                Location = new Point(10, 10),
                Size = new Size(310, 230)
            };
            listBox.BackColor = isDarkMode ? Color.FromArgb(1, 20, 50) : Color.White;
            listBox.ForeColor = isDarkMode ? Color.White : lunaDarkest;

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string query = @"SELECT ts.TeacherUsername 
                                     FROM TeacherSubjects ts 
                                     WHERE ts.SubjectName = @Subject 
                                       AND ts.ProgramCode = @Program 
                                     ORDER BY ts.TeacherUsername";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Subject", subject);
                        cmd.Parameters.AddWithValue("@Program", code);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                                listBox.Items.Add(reader["TeacherUsername"].ToString());
                        }
                    }
                }
            }
            catch { }

            form.Controls.Add(listBox);
            form.ShowDialog();
        }

        // =============================================
        // TEACHERS TAB
        // =============================================
        private void CreateTeacher()
        {
            string username = PromptDialog("Username:", "Create Teacher");
            if (string.IsNullOrEmpty(username)) return;

            string password = PromptDialog("Password:", "Create Teacher");
            if (string.IsNullOrEmpty(password)) return;

            string name = PromptDialog("Full Name:", "Create Teacher");

            ExecuteNonQuery("sp_CreateUser",
                ("@Username", username),
                ("@Password", password),
                ("@FullName", name ?? username),
                ("@Role", "Instructor"));
            LoadAllTeachers();
        }

        private void EditTeacher()
        {
            if (dgvTeachers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select a teacher first.");
                return;
            }

            string username = dgvTeachers.SelectedRows[0].Cells["Username"].Value.ToString();

            using (var form = new Form
            {
                Text = "Edit Teacher",
                Size = new Size(350, 220),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog
            })
            {
                form.BackColor = isDarkMode ? lunaDarkest : Color.FromArgb(240, 248, 255);

                var lblName = new Label
                {
                    Text = "Full Name:",
                    Location = new Point(15, 15),
                    Size = new Size(100, 20),
                    ForeColor = isDarkMode ? Color.White : lunaDarkest
                };
                var txtName = new TextBox
                {
                    Location = new Point(120, 12),
                    Size = new Size(200, 25)
                };

                var lblEmail = new Label
                {
                    Text = "Email:",
                    Location = new Point(15, 50),
                    Size = new Size(100, 20),
                    ForeColor = isDarkMode ? Color.White : lunaDarkest
                };
                var txtEmail = new TextBox
                {
                    Location = new Point(120, 47),
                    Size = new Size(200, 25)
                };

                var btnSave = new Button
                {
                    Text = "Save",
                    Location = new Point(120, 90),
                    Size = new Size(100, 35),
                    BackColor = lunaTeal,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat
                };
                btnSave.FlatAppearance.BorderSize = 0;
                btnSave.Click += (s, ev) =>
                {
                    try
                    {
                        using (var conn = new SqlConnection(_connectionString))
                        {
                            conn.Open();

                            if (!string.IsNullOrEmpty(txtName.Text))
                            {
                                using (var cmd = new SqlCommand(
                                    "UPDATE Users SET FullName = @Name WHERE Username = @User", conn))
                                {
                                    cmd.Parameters.AddWithValue("@Name", txtName.Text);
                                    cmd.Parameters.AddWithValue("@User", username);
                                    cmd.ExecuteNonQuery();
                                }
                            }

                            if (!string.IsNullOrEmpty(txtEmail.Text))
                            {
                                using (var cmd = new SqlCommand(
                                    "UPDATE Users SET Email = @Email WHERE Username = @User", conn))
                                {
                                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                                    cmd.Parameters.AddWithValue("@User", username);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }
                        form.DialogResult = DialogResult.OK;
                        form.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                };

                form.Controls.AddRange(new Control[] { lblName, txtName, lblEmail, txtEmail, btnSave });

                if (form.ShowDialog() == DialogResult.OK)
                {
                    MessageBox.Show("Teacher updated successfully.");
                    LoadAllTeachers();
                }
            }
        }

        private void EditCoverage()
        {
            if (dgvTeachers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select a teacher first.");
                return;
            }

            string teacher = dgvTeachers.SelectedRows[0].Cells["Username"].Value.ToString();

            var form = new Form
            {
                Text = $"Edit Coverage - {teacher}",
                Size = new Size(500, 450),
                StartPosition = FormStartPosition.CenterParent
            };
            form.BackColor = isDarkMode ? lunaDarkest : Color.FromArgb(240, 248, 255);

            var clb = new CheckedListBox
            {
                Location = new Point(10, 10),
                Size = new Size(460, 320)
            };
            clb.BackColor = isDarkMode ? Color.FromArgb(1, 20, 50) : Color.White;
            clb.ForeColor = isDarkMode ? Color.White : lunaDarkest;

            // Load all subjects and check assigned ones
            var allSubjects = new DataTable();
            var assignedSubjects = new HashSet<string>();

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    using (var cmd = new SqlCommand(
                        "SELECT ProgramCode, SubjectName FROM Subjects ORDER BY ProgramCode, DisplayOrder", conn))
                    using (var adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(allSubjects);
                    }

                    using (var cmd = new SqlCommand(
                        "SELECT SubjectName FROM TeacherSubjects WHERE TeacherUsername = @Teacher", conn))
                    {
                        cmd.Parameters.AddWithValue("@Teacher", teacher);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                                assignedSubjects.Add(reader["SubjectName"].ToString());
                        }
                    }
                }
            }
            catch { }

            foreach (DataRow row in allSubjects.Rows)
            {
                string item = $"{row["ProgramCode"]} - {row["SubjectName"]}";
                clb.Items.Add(item, assignedSubjects.Contains(row["SubjectName"].ToString()));
            }

            var btnSave = new Button
            {
                Text = "Save Coverage",
                Location = new Point(150, 350),
                Size = new Size(200, 35),
                BackColor = lunaTeal,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += (s, ev) =>
            {
                try
                {
                    using (var conn = new SqlConnection(_connectionString))
                    {
                        conn.Open();

                        // Remove all existing assignments
                        using (var cmd = new SqlCommand(
                            "DELETE FROM TeacherSubjects WHERE TeacherUsername = @Teacher", conn))
                        {
                            cmd.Parameters.AddWithValue("@Teacher", teacher);
                            cmd.ExecuteNonQuery();
                        }

                        // Add checked ones
                        foreach (var item in clb.CheckedItems)
                        {
                            string[] parts = item.ToString().Split(new[] { " - " }, StringSplitOptions.None);
                            using (var cmd = new SqlCommand(
                                "INSERT INTO TeacherSubjects (TeacherUsername, SubjectName, ProgramCode) VALUES (@T, @S, @P)", conn))
                            {
                                cmd.Parameters.AddWithValue("@T", teacher);
                                cmd.Parameters.AddWithValue("@S", parts[1]);
                                cmd.Parameters.AddWithValue("@P", parts[0]);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                    form.DialogResult = DialogResult.OK;
                    form.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            };

            form.Controls.Add(clb);
            form.Controls.Add(btnSave);

            if (form.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("Coverage updated successfully.");
                LogActivity($"Edited coverage for teacher {teacher}");
            }
        }

        private void ResetTeacherPassword()
        {
            if (dgvTeachers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select a teacher first.");
                return;
            }

            string username = dgvTeachers.SelectedRows[0].Cells["Username"].Value.ToString();
            string newPass = PromptDialog("New Password:", "Reset Password");

            if (!string.IsNullOrEmpty(newPass))
            {
                try
                {
                    using (var conn = new SqlConnection(_connectionString))
                    {
                        conn.Open();
                        using (var cmd = new SqlCommand(
                            "UPDATE Users SET Password = @Password WHERE Username = @Username", conn))
                        {
                            cmd.Parameters.AddWithValue("@Password", newPass);
                            cmd.Parameters.AddWithValue("@Username", username);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    MessageBox.Show("Password updated successfully.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void DeleteTeacher()
        {
            if (dgvTeachers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select a teacher first.");
                return;
            }

            string username = dgvTeachers.SelectedRows[0].Cells["Username"].Value.ToString();

            if (MessageBox.Show($"Delete teacher '{username}'?", "Confirm",
                MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    ExecuteNonQuery("sp_DeleteUser", ("@Username", username));
                    LoadAllTeachers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Cannot delete: " + ex.Message);
                }
            }
        }

        // =============================================
        // STUDENTS TAB
        // =============================================
        private void CreateStudent()
        {
            string username = PromptDialog("Username:", "Create Student");
            if (string.IsNullOrEmpty(username)) return;

            string password = PromptDialog("Password:", "Create Student");
            if (string.IsNullOrEmpty(password)) return;

            string name = PromptDialog("Full Name:", "Create Student");
            string program = PromptDialog("Program Code:", "Create Student");

            ExecuteNonQuery("sp_CreateUser",
                ("@Username", username),
                ("@Password", password),
                ("@FullName", name ?? username),
                ("@Role", "Student"),
                ("@Program", program));
            LoadAllStudents();
        }

        private void EditStudent()
        {
            if (dgvStudents.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select a student first.");
                return;
            }

            string username = dgvStudents.SelectedRows[0].Cells["Username"].Value.ToString();

            string name = PromptDialog("New Full Name:", "Edit Student");
            if (string.IsNullOrEmpty(name)) return;

            string program = PromptDialog("New Program Code (or leave blank):", "Edit Student");

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    using (var cmd = new SqlCommand(
                        "UPDATE Users SET FullName = @Name WHERE Username = @User", conn))
                    {
                        cmd.Parameters.AddWithValue("@Name", name);
                        cmd.Parameters.AddWithValue("@User", username);
                        cmd.ExecuteNonQuery();
                    }

                    if (!string.IsNullOrEmpty(program))
                    {
                        using (var cmd = new SqlCommand(
                            "UPDATE Users SET Program = @Program WHERE Username = @User", conn))
                        {
                            cmd.Parameters.AddWithValue("@Program", program);
                            cmd.Parameters.AddWithValue("@User", username);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                MessageBox.Show("Student updated successfully.");
                LoadAllStudents();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void ResetStudentPassword()
        {
            if (dgvStudents.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select a student first.");
                return;
            }

            string username = dgvStudents.SelectedRows[0].Cells["Username"].Value.ToString();
            string newPass = PromptDialog("New Password:", "Reset Password");

            if (!string.IsNullOrEmpty(newPass))
            {
                try
                {
                    using (var conn = new SqlConnection(_connectionString))
                    {
                        conn.Open();
                        using (var cmd = new SqlCommand(
                            "UPDATE Users SET Password = @Password WHERE Username = @Username", conn))
                        {
                            cmd.Parameters.AddWithValue("@Password", newPass);
                            cmd.Parameters.AddWithValue("@Username", username);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    MessageBox.Show("Password updated successfully.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void DeleteStudent()
        {
            if (dgvStudents.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select a student first.");
                return;
            }

            string username = dgvStudents.SelectedRows[0].Cells["Username"].Value.ToString();

            if (MessageBox.Show($"Delete student '{username}'?", "Confirm",
                MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    ExecuteNonQuery("sp_DeleteUser", ("@Username", username));
                    LoadAllStudents();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Cannot delete: " + ex.Message);
                }
            }
        }

        // =============================================
        // THEME
        // =============================================
        private void ApplyTheme()
        {
            this.SuspendLayout();
            pnlTopBar.SuspendLayout();

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
                        this.BackColor = isDarkMode ? lunaDarkest : Color.FromArgb(240, 248, 255);

                    this.BackgroundImageLayout = ImageLayout.Stretch;
                }
                catch
                {
                    this.BackColor = isDarkMode ? lunaDarkest : Color.FromArgb(240, 248, 255);
                }

                // Panels
                pnlTopBar.BackColor = isDarkMode
                    ? Color.FromArgb(200, 1, 28, 64)
                    : Color.FromArgb(200, 255, 255, 255);

                pnlTabNav.BackColor = isDarkMode
                    ? Color.FromArgb(1, 20, 50)
                    : Color.FromArgb(220, 235, 250);

                Color textColor = isDarkMode ? Color.White : lunaDarkest;

                // Labels
                lblTitle.ForeColor = lunaCyan;
                lblSubtitle.ForeColor = isDarkMode
                    ? Color.FromArgb(185, 187, 190)
                    : Color.FromArgb(80, 80, 80);

                // Theme toggle
                btnThemeToggle.ForeColor = textColor;
                btnThemeToggle.Text = isDarkMode ? "🌙" : "☀️";

                // Logout button
                btnLogout.BackColor = lunaTeal;
                btnLogout.ForeColor = Color.White;
                ApplyRoundedCornersToButton(btnLogout, 15);

                // Logo
                SetupLogo();

                // DataGridViews
                StyleGrid(dgvPrograms);
                StyleGrid(dgvAllSubjects);
                StyleGrid(dgvTeachers);
                StyleGrid(dgvStudents);

                // All action buttons
                Button[] allButtons =
                {
                    btnAddProgram, btnEditProgram, btnDeleteProgram,
                    btnAddSubject, btnEditSubject, btnDeleteSubject, btnManageTeachers,
                    btnCreateTeacher, btnEditTeacher, btnResetTeacherPassword, btnDeleteTeacher, btnEditCoverage,
                    btnCreateStudent, btnEditStudent, btnResetStudentPassword, btnDeleteStudent
                };

                foreach (Button btn in allButtons)
                {
                    btn.BackColor = lunaTeal;
                    btn.ForeColor = Color.White;
                    ApplyRoundedCornersToButton(btn, 15);
                }

                // Title labels
                Label[] titleLabels =
                {
                    lblProgramsTitle, lblSubjectsTitle,
                    lblTeachersTitle, lblStudentsTitle
                };

                foreach (Label lbl in titleLabels)
                    lbl.ForeColor = textColor;

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
                    btnViewActivityLog.ForeColor = textColor;
                }
            }
            finally
            {
                pnlTopBar.ResumeLayout(false);
                pnlTopBar.PerformLayout();
                this.ResumeLayout(false);
            }
        }

        // =============================================
        // ROUNDED CORNERS HELPERS
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
    }
}