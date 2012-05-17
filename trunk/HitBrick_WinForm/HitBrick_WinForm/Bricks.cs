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
            int temp = 0;
            Rects = new List<Brick_Type>();
            Random rd = new Random();
            for (int i = 100; i < _height; i += 18)
            {
                temp += 18;
                for (int j = temp - 18; j < _width - temp; j += 40)
                {
                    Rectangle Rect = new Rectangle(j, i, 40, 18);
                    Brick_Type temp_brick = new Brick_Type();
                    temp_brick.r = Rect;
                    temp_brick.i = rd.Next() % 3;
                    temp_brick.pictureBox = new PictureBox();
                    Rects.Add(temp_brick);
                }
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
