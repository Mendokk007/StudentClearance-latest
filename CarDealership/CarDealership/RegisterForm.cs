using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace CarDealership
{
    public partial class RegisterForm : Form
    {
        private readonly string _connectionString;

        public RegisterForm(string connectionString)
        {
            InitializeComponent();
            _connectionString = connectionString;
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string user = txtRegUsername.Text.Trim();
            string pass = txtRegPassword.Text;
            string confirm = txtConfirmPassword.Text;

            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
            {
                MessageBox.Show("Please fill in all fields.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (pass != confirm)
            {
                MessageBox.Show("Passwords do not match!", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Optional: Add password strength validation
            if (pass.Length < 6)
            {
                MessageBox.Show("Password must be at least 6 characters long.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    // Check if username already exists
                    string checkQuery = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@Username", user);
                        int existingCount = (int)checkCmd.ExecuteScalar();

                        if (existingCount > 0)
                        {
                            MessageBox.Show("Username already taken.", "Duplicate User",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    // Insert new student account with Role = 'Student'
                    string insertQuery = @"INSERT INTO Users (Username, Password, Role, CreatedAt) 
                                         VALUES (@Username, @Password, 'Student', GETDATE())";
                    using (SqlCommand insertCmd = new SqlCommand(insertQuery, conn))
                    {
                        insertCmd.Parameters.AddWithValue("@Username", user);
                        insertCmd.Parameters.AddWithValue("@Password", pass);
                        insertCmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Registration Successful!\n\nYou can now login with your credentials.",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Database Error: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Red theme hover effects
        private void btnRegister_MouseEnter(object sender, EventArgs e)
        {
            btnRegister.BackColor = Color.FromArgb(217, 55, 55);
        }

        private void btnRegister_MouseLeave(object sender, EventArgs e)
        {
            btnRegister.BackColor = Color.FromArgb(240, 71, 71);
        }

        private void btnCancel_MouseEnter(object sender, EventArgs e)
        {
            btnCancel.BackColor = Color.FromArgb(94, 99, 107);
        }

        private void btnCancel_MouseLeave(object sender, EventArgs e)
        {
            btnCancel.BackColor = Color.FromArgb(74, 79, 87);
        }

        // Allow Enter key to trigger Register
        private void RegisterForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnRegister_Click(sender, e);
            }
            else if (e.KeyCode == Keys.Escape)
            {
                btnCancel_Click(sender, e);
            }
        }
    }
}