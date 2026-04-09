namespace CarDealership
{
    partial class RegisterForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblRegTitle;
        private System.Windows.Forms.Label lblRegUsername;
        private System.Windows.Forms.TextBox txtRegUsername;
        private System.Windows.Forms.Label lblRegPassword;
        private System.Windows.Forms.TextBox txtRegPassword;
        private System.Windows.Forms.Label lblConfirmPassword;
        private System.Windows.Forms.TextBox txtConfirmPassword;
        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.Button btnCancel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblRegTitle = new System.Windows.Forms.Label();
            this.lblRegUsername = new System.Windows.Forms.Label();
            this.txtRegUsername = new System.Windows.Forms.TextBox();
            this.lblRegPassword = new System.Windows.Forms.Label();
            this.txtRegPassword = new System.Windows.Forms.TextBox();
            this.lblConfirmPassword = new System.Windows.Forms.Label();
            this.txtConfirmPassword = new System.Windows.Forms.TextBox();
            this.btnRegister = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // lblRegTitle
            this.lblRegTitle.AutoSize = true;
            this.lblRegTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblRegTitle.ForeColor = System.Drawing.Color.White;
            this.lblRegTitle.Location = new System.Drawing.Point(20, 20);
            this.lblRegTitle.Name = "lblRegTitle";
            this.lblRegTitle.Size = new System.Drawing.Size(200, 25);
            this.lblRegTitle.TabIndex = 0;
            this.lblRegTitle.Text = "Create Student Account";

            // lblRegUsername
            this.lblRegUsername.AutoSize = true;
            this.lblRegUsername.ForeColor = System.Drawing.Color.White;
            this.lblRegUsername.Location = new System.Drawing.Point(30, 70);
            this.lblRegUsername.Name = "lblRegUsername";
            this.lblRegUsername.Size = new System.Drawing.Size(63, 15);
            this.lblRegUsername.TabIndex = 1;
            this.lblRegUsername.Text = "Username:";

            // txtRegUsername
            this.txtRegUsername.BackColor = System.Drawing.Color.FromArgb(64, 68, 75);
            this.txtRegUsername.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRegUsername.ForeColor = System.Drawing.Color.White;
            this.txtRegUsername.Location = new System.Drawing.Point(130, 67);
            this.txtRegUsername.Name = "txtRegUsername";
            this.txtRegUsername.Size = new System.Drawing.Size(180, 23);
            this.txtRegUsername.TabIndex = 2;

            // lblRegPassword
            this.lblRegPassword.AutoSize = true;
            this.lblRegPassword.ForeColor = System.Drawing.Color.White;
            this.lblRegPassword.Location = new System.Drawing.Point(30, 110);
            this.lblRegPassword.Name = "lblRegPassword";
            this.lblRegPassword.Size = new System.Drawing.Size(60, 15);
            this.lblRegPassword.TabIndex = 3;
            this.lblRegPassword.Text = "Password:";

            // txtRegPassword
            this.txtRegPassword.BackColor = System.Drawing.Color.FromArgb(64, 68, 75);
            this.txtRegPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRegPassword.ForeColor = System.Drawing.Color.White;
            this.txtRegPassword.Location = new System.Drawing.Point(130, 107);
            this.txtRegPassword.Name = "txtRegPassword";
            this.txtRegPassword.Size = new System.Drawing.Size(180, 23);
            this.txtRegPassword.TabIndex = 4;
            this.txtRegPassword.UseSystemPasswordChar = true;

            // lblConfirmPassword
            this.lblConfirmPassword.AutoSize = true;
            this.lblConfirmPassword.ForeColor = System.Drawing.Color.White;
            this.lblConfirmPassword.Location = new System.Drawing.Point(30, 150);
            this.lblConfirmPassword.Name = "lblConfirmPassword";
            this.lblConfirmPassword.Size = new System.Drawing.Size(107, 15);
            this.lblConfirmPassword.TabIndex = 5;
            this.lblConfirmPassword.Text = "Confirm Password:";

            // txtConfirmPassword
            this.txtConfirmPassword.BackColor = System.Drawing.Color.FromArgb(64, 68, 75);
            this.txtConfirmPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtConfirmPassword.ForeColor = System.Drawing.Color.White;
            this.txtConfirmPassword.Location = new System.Drawing.Point(130, 147);
            this.txtConfirmPassword.Name = "txtConfirmPassword";
            this.txtConfirmPassword.Size = new System.Drawing.Size(180, 23);
            this.txtConfirmPassword.TabIndex = 6;
            this.txtConfirmPassword.UseSystemPasswordChar = true;

            // btnRegister
            this.btnRegister.BackColor = System.Drawing.Color.FromArgb(240, 71, 71);
            this.btnRegister.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRegister.FlatAppearance.BorderSize = 0;
            this.btnRegister.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRegister.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnRegister.ForeColor = System.Drawing.Color.White;
            this.btnRegister.Location = new System.Drawing.Point(130, 190);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(100, 35);
            this.btnRegister.TabIndex = 7;
            this.btnRegister.Text = "Register";
            this.btnRegister.UseVisualStyleBackColor = false;
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
            this.btnRegister.MouseEnter += new System.EventHandler(this.btnRegister_MouseEnter);
            this.btnRegister.MouseLeave += new System.EventHandler(this.btnRegister_MouseLeave);

            // btnCancel
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(74, 79, 87);
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(235, 192);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 30);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            this.btnCancel.MouseEnter += new System.EventHandler(this.btnCancel_MouseEnter);
            this.btnCancel.MouseLeave += new System.EventHandler(this.btnCancel_MouseLeave);

            // RegisterForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(54, 57, 63);
            this.ClientSize = new System.Drawing.Size(350, 260);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnRegister);
            this.Controls.Add(this.txtConfirmPassword);
            this.Controls.Add(this.lblConfirmPassword);
            this.Controls.Add(this.txtRegPassword);
            this.Controls.Add(this.lblRegPassword);
            this.Controls.Add(this.txtRegUsername);
            this.Controls.Add(this.lblRegUsername);
            this.Controls.Add(this.lblRegTitle);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RegisterForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Register - Student Clearance System";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.RegisterForm_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}