namespace CarDealership
{
    partial class ReviewSubmissionForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel pnlTopBar;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblStudent;
        private System.Windows.Forms.Label lblDepartment;
        private System.Windows.Forms.PictureBox pbSubmission;
        private System.Windows.Forms.TextBox txtRejectionReason;
        private System.Windows.Forms.Label lblRejectionReason;
        private System.Windows.Forms.Button btnApprove;
        private System.Windows.Forms.Button btnReject;
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
            this.pnlTopBar = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblStudent = new System.Windows.Forms.Label();
            this.lblDepartment = new System.Windows.Forms.Label();
            this.pbSubmission = new System.Windows.Forms.PictureBox();
            this.txtRejectionReason = new System.Windows.Forms.TextBox();
            this.lblRejectionReason = new System.Windows.Forms.Label();
            this.btnApprove = new System.Windows.Forms.Button();
            this.btnReject = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.pnlTopBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbSubmission)).BeginInit();
            this.SuspendLayout();

            // pnlTopBar
            this.pnlTopBar.BackColor = System.Drawing.Color.FromArgb(47, 49, 54);
            this.pnlTopBar.Controls.Add(this.lblTitle);
            this.pnlTopBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTopBar.Location = new System.Drawing.Point(0, 0);
            this.pnlTopBar.Name = "pnlTopBar";
            this.pnlTopBar.Size = new System.Drawing.Size(600, 50);
            this.pnlTopBar.TabIndex = 0;

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(15, 12);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(173, 25);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Review Submission";

            // lblStudent
            this.lblStudent.AutoSize = true;
            this.lblStudent.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblStudent.ForeColor = System.Drawing.Color.White;
            this.lblStudent.Location = new System.Drawing.Point(20, 65);
            this.lblStudent.Name = "lblStudent";
            this.lblStudent.Size = new System.Drawing.Size(113, 20);
            this.lblStudent.TabIndex = 1;
            this.lblStudent.Text = "Student: Name";

            // lblDepartment
            this.lblDepartment.AutoSize = true;
            this.lblDepartment.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblDepartment.ForeColor = System.Drawing.Color.FromArgb(185, 187, 190);
            this.lblDepartment.Location = new System.Drawing.Point(20, 90);
            this.lblDepartment.Name = "lblDepartment";
            this.lblDepartment.Size = new System.Drawing.Size(126, 19);
            this.lblDepartment.TabIndex = 2;
            this.lblDepartment.Text = "Department: Name";

            // pbSubmission
            this.pbSubmission.BackColor = System.Drawing.Color.FromArgb(47, 49, 54);
            this.pbSubmission.Location = new System.Drawing.Point(25, 120);
            this.pbSubmission.Name = "pbSubmission";
            this.pbSubmission.Size = new System.Drawing.Size(550, 250);
            this.pbSubmission.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbSubmission.TabIndex = 3;
            this.pbSubmission.TabStop = false;

            // txtRejectionReason
            this.txtRejectionReason.BackColor = System.Drawing.Color.FromArgb(64, 68, 75);
            this.txtRejectionReason.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRejectionReason.ForeColor = System.Drawing.Color.White;
            this.txtRejectionReason.Location = new System.Drawing.Point(25, 405);
            this.txtRejectionReason.Multiline = true;
            this.txtRejectionReason.Name = "txtRejectionReason";
            this.txtRejectionReason.Size = new System.Drawing.Size(550, 60);
            this.txtRejectionReason.TabIndex = 4;

            // lblRejectionReason
            this.lblRejectionReason.AutoSize = true;
            this.lblRejectionReason.ForeColor = System.Drawing.Color.FromArgb(185, 187, 190);
            this.lblRejectionReason.Location = new System.Drawing.Point(25, 387);
            this.lblRejectionReason.Name = "lblRejectionReason";
            this.lblRejectionReason.Size = new System.Drawing.Size(151, 15);
            this.lblRejectionReason.TabIndex = 5;
            this.lblRejectionReason.Text = "Rejection Reason (if any):";

            // btnApprove
            this.btnApprove.BackColor = System.Drawing.Color.FromArgb(40, 167, 69);
            this.btnApprove.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnApprove.FlatAppearance.BorderSize = 0;
            this.btnApprove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApprove.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnApprove.ForeColor = System.Drawing.Color.White;
            this.btnApprove.Location = new System.Drawing.Point(320, 480);
            this.btnApprove.Name = "btnApprove";
            this.btnApprove.Size = new System.Drawing.Size(120, 35);
            this.btnApprove.TabIndex = 6;
            this.btnApprove.Text = "✓ Approve";
            this.btnApprove.UseVisualStyleBackColor = false;
            this.btnApprove.Click += new System.EventHandler(this.btnApprove_Click);

            // btnReject
            this.btnReject.BackColor = System.Drawing.Color.FromArgb(217, 55, 55);
            this.btnReject.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReject.FlatAppearance.BorderSize = 0;
            this.btnReject.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReject.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnReject.ForeColor = System.Drawing.Color.White;
            this.btnReject.Location = new System.Drawing.Point(450, 480);
            this.btnReject.Name = "btnReject";
            this.btnReject.Size = new System.Drawing.Size(120, 35);
            this.btnReject.TabIndex = 7;
            this.btnReject.Text = "✗ Reject";
            this.btnReject.UseVisualStyleBackColor = false;
            this.btnReject.Click += new System.EventHandler(this.btnReject_Click);

            // btnCancel
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(64, 68, 75);
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(25, 480);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 35);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);

            // ReviewSubmissionForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(32, 34, 37);
            this.ClientSize = new System.Drawing.Size(600, 530);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnReject);
            this.Controls.Add(this.btnApprove);
            this.Controls.Add(this.lblRejectionReason);
            this.Controls.Add(this.txtRejectionReason);
            this.Controls.Add(this.pbSubmission);
            this.Controls.Add(this.lblDepartment);
            this.Controls.Add(this.lblStudent);
            this.Controls.Add(this.pnlTopBar);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ReviewSubmissionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Review Submission";
            this.pnlTopBar.ResumeLayout(false);
            this.pnlTopBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbSubmission)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}