using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace CarDealership
{
    public partial class LoginForm : Form
    {
        private readonly string _connectionString;

        public LoginForm()
        {
            InitializeComponent();
            _connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=StudentClearanceDB;Integrated Security=True;";

            // Prevent the button from showing focus rectangle
            this.btnLogin.TabStop = false;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter username and password.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string query = @"SELECT Role, Department FROM Users 
                                   WHERE Username = @Username AND Password = @Password";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@Password", password);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string role = reader["Role"].ToString();

                                if (role == "Admin")
                                {
                                    AdminForm adminForm = new AdminForm(_connectionString, username);
                                    adminForm.Show();
                                }
                                else
                                {
                                    HomeForm home = new HomeForm(_connectionString, username);
                                    home.Show();
                                }
                                this.Hide();
                            }
                            else
                            {
                                MessageBox.Show("Invalid username or password.", "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Login Error: " + ex.Message);
            }
        }

        private void lblRegister_Click(object sender, EventArgs e)
        {
            RegisterForm regForm = new RegisterForm(_connectionString);
            regForm.ShowDialog();
        }

        private void lblDeleteAccount_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Enter username to delete in the username field.", "Input Required",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult confirm = MessageBox.Show($"Delete account '{username}' permanently?",
                "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(_connectionString))
                    {
                        conn.Open();
                        string deleteQuery = "DELETE FROM Users WHERE Username = @Username";
                        using (SqlCommand cmd = new SqlCommand(deleteQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@Username", username);
                            int rows = cmd.ExecuteNonQuery();
                            if (rows > 0)
                            {
                                MessageBox.Show("Account deleted.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                txtUsername.Clear();
                                txtPassword.Clear();
                            }
                            else
                            {
                                MessageBox.Show("Account not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        // Red theme hover effects - only color changes, no size changes
        private void btnClose_MouseEnter(object sender, EventArgs e)
        {
            btnClose.BackColor = Color.FromArgb(217, 55, 55); // Darker red
        }

        private void btnClose_MouseLeave(object sender, EventArgs e)
        {
            btnClose.BackColor = Color.FromArgb(240, 71, 71); // Normal red
        }

        private void btnLogin_MouseEnter(object sender, EventArgs e)
        {
            btnLogin.BackColor = Color.FromArgb(217, 55, 55); // Darker red only, no size change
        }

        private void btnLogin_MouseLeave(object sender, EventArgs e)
        {
            btnLogin.BackColor = Color.FromArgb(240, 71, 71); // Normal red only, no size change
        }

        private void lblRegister_MouseEnter(object sender, EventArgs e)
        {
            lblRegister.ForeColor = Color.FromArgb(242, 101, 101); // Lighter red
        }

        private void lblRegister_MouseLeave(object sender, EventArgs e)
        {
            lblRegister.ForeColor = Color.FromArgb(240, 71, 71);
        }

        private void lblDeleteAccount_MouseEnter(object sender, EventArgs e)
        {
            lblDeleteAccount.ForeColor = Color.Salmon;
        }

        private void lblDeleteAccount_MouseLeave(object sender, EventArgs e)
        {
            lblDeleteAccount.ForeColor = Color.IndianRed;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private void titleBar_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0xA1, 0x2, 0);
        }
    }
}