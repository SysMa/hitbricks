using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace HitBrick_WinForm
{
    public partial class KinectForm : Form
    {
        private int _width = 400; //砖块集宽
        private int _height = 300;//砖块集高

        //砖块
        public struct Brick_Type
        {
            public Rectangle r;
            public int i;
            public PictureBox pictureBox;
        }

        //砖块集
        public List<Brick_Type> Rects { get; set; }

        public void newBricks()
        {
            Rects = new List<Brick_Type>();
            switch(stage)
            {
                case 1:
                    draw_stage_one();
                    break;
                default:
                    draw_stage_one();
                    break;
            }

            foreach (Brick_Type b in Rects)
            {
                Image img;
                switch (b.i)
                {
                    case 0:
                        img = global::HitBrick_WinForm.Properties.Resources.yellow;
                        break;
                    case 1:
                        img = global::HitBrick_WinForm.Properties.Resources.taohong;
                        break;
                    case 2:
                    default:
                        img = global::HitBrick_WinForm.Properties.Resources.green;
                        break;
                }
                b.pictureBox.Location = new Point(b.r.X, b.r.Y);
                b.pictureBox.Image = img;
                b.pictureBox.Size = new Size(40, 18);
                this.splitContainer1.Panel1.Controls.Add(b.pictureBox);
            }
        }
    }
}
