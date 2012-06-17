
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.oversign = new System.Windows.Forms.Label();
            this.manImage = new System.Windows.Forms.PictureBox();
            this.txtTime = new System.Windows.Forms.Label();
            this.txtScore = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.manImage)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer1.Panel1.Controls.Add(this.oversign);
            this.splitContainer1.Panel1.Controls.Add(this.manImage);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer1.Panel2.BackgroundImage = global::HitBrick_WinForm.Properties.Resources.side;
            this.splitContainer1.Panel2.Controls.Add(this.txtTime);
            this.splitContainer1.Panel2.Controls.Add(this.txtScore);
            this.splitContainer1.Size = new System.Drawing.Size(790, 768);
            this.splitContainer1.SplitterDistance = 608;
            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer1.TabIndex = 5;
            // 
            // oversign
            // 
            this.oversign.AutoSize = true;
            this.oversign.Font = new System.Drawing.Font("宋体", 72F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.oversign.ForeColor = System.Drawing.Color.Yellow;
            this.oversign.Location = new System.Drawing.Point(78, 112);
            this.oversign.Name = "oversign";
            this.oversign.Size = new System.Drawing.Size(434, 194);
            this.oversign.TabIndex = 3;
            this.oversign.Text = "G A M E \r\nO V E R";
            // 
            // manImage
            // 
            this.manImage.BackColor = System.Drawing.Color.Transparent;
            this.manImage.Location = new System.Drawing.Point(-2, 355);
            this.manImage.Name = "manImage";
            this.manImage.Size = new System.Drawing.Size(178, 161);
            this.manImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.manImage.TabIndex = 2;
            this.manImage.TabStop = false;
            // 
            // txtTime
            // 
            this.txtTime.BackColor = System.Drawing.Color.Transparent;
            this.txtTime.Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTime.ForeColor = System.Drawing.Color.Lime;
            this.txtTime.Location = new System.Drawing.Point(64, 64);
            this.txtTime.Margin = new System.Windows.Forms.Padding(3);
            this.txtTime.Name = "txtTime";
            this.txtTime.Size = new System.Drawing.Size(102, 23);
            this.txtTime.TabIndex = 8;
            this.txtTime.Text = "00:00:00";
            this.txtTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtScore
            // 
            this.txtScore.BackColor = System.Drawing.Color.Transparent;
            this.txtScore.Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtScore.ForeColor = System.Drawing.Color.Lime;
            this.txtScore.Location = new System.Drawing.Point(64, 130);
            this.txtScore.Margin = new System.Windows.Forms.Padding(3);
            this.txtScore.Name = "txtScore";
            this.txtScore.Size = new System.Drawing.Size(102, 23);
            this.txtScore.TabIndex = 7;
            this.txtScore.Text = "0";
            this.txtScore.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // KinectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(790, 768);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "KinectForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HitBricks";
            this.Load += new System.EventHandler(this.KinectForm_Load);
            this.Disposed += new System.EventHandler(this.KinectForm_Disposed);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.manImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.PictureBox manImage;
        private System.Windows.Forms.Label oversign;
        private System.Windows.Forms.Label txtScore;
        private System.Windows.Forms.Label txtTime;
    }
}

