namespace RealtimeChatClient
{
    partial class RegisterForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) { components.Dispose(); }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.txtUser = new System.Windows.Forms.TextBox();
            this.txtPass = new System.Windows.Forms.TextBox();
            this.txtFullName = new System.Windows.Forms.TextBox();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // label1
            this.label1.Text = "Tên đăng nhập:";
            this.label1.Location = new System.Drawing.Point(30, 30);
            this.label1.Size = new System.Drawing.Size(100, 20);
            // txtUser
            this.txtUser.Location = new System.Drawing.Point(140, 27);
            this.txtUser.Size = new System.Drawing.Size(180, 22);
            // label2
            this.label2.Text = "Mật khẩu:";
            this.label2.Location = new System.Drawing.Point(30, 70);
            this.label2.Size = new System.Drawing.Size(100, 20);
            // txtPass
            this.txtPass.Location = new System.Drawing.Point(140, 67);
            this.txtPass.PasswordChar = '*';
            this.txtPass.Size = new System.Drawing.Size(180, 22);
            // label3
            this.label3.Text = "Họ và tên:";
            this.label3.Location = new System.Drawing.Point(30, 110);
            this.label3.Size = new System.Drawing.Size(100, 20);
            // txtFullName
            this.txtFullName.Location = new System.Drawing.Point(140, 107);
            this.txtFullName.Size = new System.Drawing.Size(180, 22);
            // btnSubmit
            this.btnSubmit.Text = "Xác nhận Đăng ký";
            this.btnSubmit.Location = new System.Drawing.Point(140, 150);
            this.btnSubmit.Size = new System.Drawing.Size(130, 35);
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // RegisterForm
            this.ClientSize = new System.Drawing.Size(360, 210);
            this.Controls.AddRange(new System.Windows.Forms.Control[] { this.btnSubmit, this.txtFullName, this.label3, this.txtPass, this.label2, this.txtUser, this.label1 });
            this.Text = "Đăng Ký Thành Viên";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.TextBox txtPass;
        private System.Windows.Forms.TextBox txtFullName;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}