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
            this.label1 = new System.Windows.Forms.Label();
            // this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.manPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.manImage)).BeginInit();
            this.SuspendLayout();
            // 
            // manPanel needed
            // 
            this.manPanel.BackColor = System.Drawing.Color.Goldenrod;
            this.manPanel.Controls.Add(this.label1);
            this.manPanel.Controls.Add(this.manImage);
            this.manPanel.Location = new System.Drawing.Point(2, 358);
            this.manPanel.Name = "manPanel";
            this.manPanel.Size = new System.Drawing.Size(906, 163);
            this.manPanel.TabIndex = 0;
            // 
            // manImage needed
            // 
            this.manImage.Location = new System.Drawing.Point(37, 2);
            this.manImage.Name = "manImage";
            this.manImage.Size = new System.Drawing.Size(178, 161);
            this.manImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.manImage.TabIndex = 0;
            this.manImage.TabStop = false;
            // 
            // KinectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(911, 523);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.manPanel);
            this.Name = "KinectForm";
            this.Text = "HitBricks";
            this.Load += new System.EventHandler(this.KinectForm_Load);
            this.Disposed += new System.EventHandler(this.KinectForm_Disposed);
            this.manPanel.ResumeLayout(false);
            this.manPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.manImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel manPanel;
        private System.Windows.Forms.PictureBox manImage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton radioButton1;

        // Buttons including init deleted by msq
        // time and score added by msq
        private System.Windows.Forms.TextBox txtTime;
        private System.Windows.Forms.TextBox txtSorce;
    }
}

