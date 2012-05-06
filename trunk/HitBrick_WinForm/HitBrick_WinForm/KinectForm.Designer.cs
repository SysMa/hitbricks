namespace HitBrick_WinForm
{
    partial class KinectForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.manPanel = new System.Windows.Forms.Panel();
            this.manImage = new System.Windows.Forms.PictureBox();
            this.txtTime = new System.Windows.Forms.TextBox();
            this.txtSorce = new System.Windows.Forms.TextBox();
            this.manPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.manImage)).BeginInit();
            this.SuspendLayout();
            // 
            // manPanel
            // 
            this.manPanel.BackColor = System.Drawing.Color.Goldenrod;
            this.manPanel.Controls.Add(this.manImage);
            this.manPanel.Location = new System.Drawing.Point(2, 358);
            this.manPanel.Name = "manPanel";
            this.manPanel.Size = new System.Drawing.Size(906, 163);
            this.manPanel.TabIndex = 0;
            // 
            // manImage
            // 
            this.manImage.Location = new System.Drawing.Point(37, 2);
            this.manImage.Name = "manImage";
            this.manImage.Size = new System.Drawing.Size(178, 161);
            this.manImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.manImage.TabIndex = 0;
            this.manImage.TabStop = false;
            // 
            // txtTime
            // 
            this.txtTime.BackColor = System.Drawing.Color.Black;
            this.txtTime.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtTime.Enabled = false;
            this.txtTime.Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTime.ForeColor = System.Drawing.Color.White;
            this.txtTime.Location = new System.Drawing.Point(783, 25);
            this.txtTime.Name = "txtTime";
            this.txtTime.ReadOnly = true;
            this.txtTime.Size = new System.Drawing.Size(101, 23);
            this.txtTime.TabIndex = 4;
            this.txtTime.Text = "00:00:00";
            // 
            // txtSorce
            // 
            this.txtSorce.BackColor = System.Drawing.Color.Black;
            this.txtSorce.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSorce.Enabled = false;
            this.txtSorce.Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSorce.ForeColor = System.Drawing.Color.White;
            this.txtSorce.Location = new System.Drawing.Point(783, 68);
            this.txtSorce.Name = "txtSorce";
            this.txtSorce.ReadOnly = true;
            this.txtSorce.Size = new System.Drawing.Size(101, 23);
            this.txtSorce.TabIndex = 3;
            this.txtSorce.Text = "0";
            // 
            // KinectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(911, 523);
            this.Controls.Add(this.txtSorce);
            this.Controls.Add(this.txtTime);
            this.Controls.Add(this.manPanel);
            this.Name = "KinectForm";
            this.Text = "HitBricks";
            this.Load += new System.EventHandler(this.KinectForm_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.SabBoy_Paint);
            this.Disposed += new System.EventHandler(this.KinectForm_Disposed);
            this.manPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.manImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel manPanel;
        private System.Windows.Forms.PictureBox manImage;
        private System.Windows.Forms.TextBox txtTime;
        private System.Windows.Forms.TextBox txtSorce;
    }
}

