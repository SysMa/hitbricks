using System;
using System.Windows.Forms;

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
            this.txtTime = new System.Windows.Forms.TextBox();
            this.txtScore = new System.Windows.Forms.TextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.manImage = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.manImage)).BeginInit();
            this.SuspendLayout();
            // 
            // txtTime
            // 
            this.txtTime.BackColor = System.Drawing.Color.Silver;
            this.txtTime.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtTime.Enabled = false;
            this.txtTime.Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTime.ForeColor = System.Drawing.Color.White;
            this.txtTime.Location = new System.Drawing.Point(10, 14);
            this.txtTime.Name = "txtTime";
            this.txtTime.ReadOnly = true;
            this.txtTime.Size = new System.Drawing.Size(146, 23);
            this.txtTime.TabIndex = 4;
            this.txtTime.Text = "Time : 00:00:00";
            // 
            // txtScore
            // 
            this.txtScore.BackColor = System.Drawing.Color.Silver;
            this.txtScore.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtScore.Enabled = false;
            this.txtScore.Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtScore.ForeColor = System.Drawing.Color.White;
            this.txtScore.Location = new System.Drawing.Point(10, 48);
            this.txtScore.Name = "txtScore";
            this.txtScore.ReadOnly = true;
            this.txtScore.Size = new System.Drawing.Size(146, 23);
            this.txtScore.TabIndex = 3;
            this.txtScore.Text = "0";
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackgroundImage = global::HitBrick_WinForm.Properties.Resources.bg;
            this.splitContainer1.Panel1.Controls.Add(this.manImage);
            this.splitContainer1.Panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.SabBoy_Paint);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.Color.Silver;
            this.splitContainer1.Panel2.Controls.Add(this.txtScore);
            this.splitContainer1.Panel2.Controls.Add(this.txtTime);
            this.splitContainer1.Size = new System.Drawing.Size(793, 523);
            this.splitContainer1.SplitterDistance = 600;
            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer1.TabIndex = 5;
            // 
            // manImage
            // 
            this.manImage.Location = new System.Drawing.Point(-2, 355);
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
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(793, 523);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "KinectForm";
            this.Text = "HitBricks";
            this.Load += new System.EventHandler(this.KinectForm_Load);
            this.Disposed += new System.EventHandler(this.KinectForm_Disposed);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.manImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtTime;
        private System.Windows.Forms.TextBox txtScore;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.PictureBox manImage;
    }
}

