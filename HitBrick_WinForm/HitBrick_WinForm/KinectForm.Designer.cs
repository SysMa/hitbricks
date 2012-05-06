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
            this.txtSorce = new System.Windows.Forms.TextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.manPanel = new System.Windows.Forms.Panel();
            this.manImage = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.manPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.manImage)).BeginInit();
            this.SuspendLayout();
            // 
            // txtTime
            // 
            this.txtTime.BackColor = System.Drawing.Color.Black;
            this.txtTime.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtTime.Enabled = false;
            this.txtTime.Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTime.ForeColor = System.Drawing.Color.White;
            this.txtTime.Location = new System.Drawing.Point(37, 12);
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
            this.txtSorce.Location = new System.Drawing.Point(37, 41);
            this.txtSorce.Name = "txtSorce";
            this.txtSorce.ReadOnly = true;
            this.txtSorce.Size = new System.Drawing.Size(101, 23);
            this.txtSorce.TabIndex = 3;
            this.txtSorce.Text = "0";
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.txtSorce);
            this.splitContainer1.Panel2.Controls.Add(this.txtTime);
            this.splitContainer1.Size = new System.Drawing.Size(1018, 523);
            this.splitContainer1.SplitterDistance = 795;
            this.splitContainer1.TabIndex = 5;
            this.splitContainer1.SplitterMoving += new System.Windows.Forms.SplitterCancelEventHandler(this.splitContainer1_SplitterMoving);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.SabBoy_Paint);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.manPanel);
            this.splitContainer2.Size = new System.Drawing.Size(791, 519);
            this.splitContainer2.SplitterDistance = 351;
            this.splitContainer2.TabIndex = 2;
            this.splitContainer2.SplitterMoving += new System.Windows.Forms.SplitterCancelEventHandler(this.splitContainer2_SplitterMoving);
            // 
            // manPanel
            // 
            this.manPanel.BackColor = System.Drawing.Color.Goldenrod;
            this.manPanel.Controls.Add(this.manImage);
            this.manPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.manPanel.Location = new System.Drawing.Point(0, 0);
            this.manPanel.Name = "manPanel";
            this.manPanel.Size = new System.Drawing.Size(791, 164);
            this.manPanel.TabIndex = 2;
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
            // KinectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1018, 523);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "KinectForm";
            this.Text = "HitBricks";
            this.Disposed += new System.EventHandler(this.KinectForm_Disposed);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.manPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.manImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtTime;
        private System.Windows.Forms.TextBox txtSorce;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Panel manPanel;
        private System.Windows.Forms.PictureBox manImage;
    }
}

