namespace LNBT
{
    partial class FrmThongTinNguoiMua
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnKiemTra = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tbTenKh = new System.Windows.Forms.TextBox();
            this.tbSdt = new System.Windows.Forms.TextBox();
            this.tbEmail = new System.Windows.Forms.TextBox();
            this.dtNgaySinh = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.cbDiemTichLuy = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnKiemTra
            // 
            this.btnKiemTra.Location = new System.Drawing.Point(277, 184);
            this.btnKiemTra.Name = "btnKiemTra";
            this.btnKiemTra.Size = new System.Drawing.Size(75, 23);
            this.btnKiemTra.TabIndex = 2;
            this.btnKiemTra.Text = "Kiểm tra";
            this.btnKiemTra.UseVisualStyleBackColor = true;
            this.btnKiemTra.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(54, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Tên khách hàng: ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(54, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Số điện thoại: ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(54, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Email: ";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(54, 124);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Ngày sinh: ";
            // 
            // tbTenKh
            // 
            this.tbTenKh.Location = new System.Drawing.Point(152, 29);
            this.tbTenKh.Name = "tbTenKh";
            this.tbTenKh.Size = new System.Drawing.Size(200, 20);
            this.tbTenKh.TabIndex = 8;
            // 
            // tbSdt
            // 
            this.tbSdt.Location = new System.Drawing.Point(152, 58);
            this.tbSdt.Name = "tbSdt";
            this.tbSdt.Size = new System.Drawing.Size(200, 20);
            this.tbSdt.TabIndex = 9;
            // 
            // tbEmail
            // 
            this.tbEmail.Location = new System.Drawing.Point(152, 90);
            this.tbEmail.Name = "tbEmail";
            this.tbEmail.Size = new System.Drawing.Size(200, 20);
            this.tbEmail.TabIndex = 10;
            // 
            // dtNgaySinh
            // 
            this.dtNgaySinh.Location = new System.Drawing.Point(152, 118);
            this.dtNgaySinh.Name = "dtNgaySinh";
            this.dtNgaySinh.Size = new System.Drawing.Size(200, 20);
            this.dtNgaySinh.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(54, 158);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Dùng điểm tích lũy ";
            // 
            // cbDiemTichLuy
            // 
            this.cbDiemTichLuy.AutoSize = true;
            this.cbDiemTichLuy.Location = new System.Drawing.Point(152, 158);
            this.cbDiemTichLuy.Name = "cbDiemTichLuy";
            this.cbDiemTichLuy.Size = new System.Drawing.Size(15, 14);
            this.cbDiemTichLuy.TabIndex = 13;
            this.cbDiemTichLuy.UseVisualStyleBackColor = true;
            // 
            // FrmThongTinNguoiMua
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(416, 219);
            this.Controls.Add(this.cbDiemTichLuy);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dtNgaySinh);
            this.Controls.Add(this.tbEmail);
            this.Controls.Add(this.tbSdt);
            this.Controls.Add(this.tbTenKh);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnKiemTra);
            this.Name = "FrmThongTinNguoiMua";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FrmThongTinNguoiMua";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnKiemTra;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbTenKh;
        private System.Windows.Forms.TextBox tbSdt;
        private System.Windows.Forms.TextBox tbEmail;
        private System.Windows.Forms.DateTimePicker dtNgaySinh;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbDiemTichLuy;
    }
}