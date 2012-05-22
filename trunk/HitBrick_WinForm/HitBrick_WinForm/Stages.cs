﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace HitBrick_WinForm
{
    public partial class KinectForm : Form
    {
        public void draw_stage_one()
        {
            int temp = 0;
            int half_blank = (this.splitContainer1.Panel1.Width - _width) / 2;
            Random rd = new Random();
            for (int i = begin; i < _height - 18 * 2; i += 18)
            {
                temp += 18;
                for (int j = temp - 18; j < _width - temp; j += 40)
                {
                    Rectangle Rect = new Rectangle(half_blank + j, i, 40, 18);
                    Brick_Type temp_brick = new Brick_Type();
                    temp_brick.rectangle = Rect;
                    temp_brick.type = rd.Next() % 3;
                    temp_brick.pictureBox = new PictureBox();
                    Rects.Add(temp_brick);
                }
            }
        }

        public void draw_stage_two()
        {
            int half_blank = (this.splitContainer1.Panel1.Width - _width) / 2;
            Random rd = new Random();
            for (int i = begin; i < _height - 18 * 3; i += 18)   //_width: 400 、  _height: 300 、 每个砖块的高度18
            {
                for (int j = 0; j < _width; j += 40)
                {
                    int x = j / 40;
                    int y = (i - 66) / 18;
                    if (x + y == 9 || x == y || y == 5 || y == 0 || y == 9 || x == 0 || x == 9)
                    {
                        Rectangle Rect = new Rectangle(half_blank + j, i, 40, 18);
                        Brick_Type temp_brick = new Brick_Type();
                        temp_brick.rectangle = Rect;
                        temp_brick.type = rd.Next() % 3;
                        temp_brick.pictureBox = new PictureBox();
                        Rects.Add(temp_brick);
                    }
                }
            }
        }
    }
}
