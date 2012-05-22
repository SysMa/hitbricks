using System;
using System.Drawing;
using System.Windows.Forms;

namespace HitBrick_WinForm
{
    public partial class KinectForm : Form
    {
        public void draw_stage_one()
        {
            int half_blank = (this.splitContainer1.Panel1.Width - _width) / 2;
            Random rd = new Random();
            int bricksNumberPerLine = 10;
            for (int i = 66, j = 0; i < _height - 18 * 3; i += 18, j += 20, bricksNumberPerLine--)
            {
                int bricksNumberCurrentLine = bricksNumberPerLine;
                for (int x = j; x < _width; x += 40)
                {
                    if (bricksNumberCurrentLine-- > 0)
                    {
                        Rectangle Rect = new Rectangle(half_blank + x, i, 40, 18);
                        Brick_Type temp_brick = new Brick_Type();
                        temp_brick.rectangle = Rect;
                        temp_brick.type = rd.Next() % 3;
                        temp_brick.pictureBox = new PictureBox();
                        Rects.Add(temp_brick);
                    }
                }
            }
        }

        public void draw_stage_two()
        {
            int half_blank = (this.splitContainer1.Panel1.Width - _width) / 2;
            Random rd = new Random();
            for (int i = 66; i < _height - 18 * 3; i += 18)   //_width: 400 、  _height: 300 、 每个砖块的高度18
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

        public void draw_stage_three()
        {
            int half_blank = (this.splitContainer1.Panel1.Width - _width) / 2;
            Random rd = new Random();
            for (int i = 66; i < _height - 18 * 3; i += 18)   //_width: 400 、  _height: 300 、 每个砖块的高度18
            {
                for (int j = 0; j < _width; j += 40)
                {
                    int x = j / 40;
                    int y = (i - 66) / 18;
                    if (x == 0 || x == 9 || y == 0 || y == 9 || x + y == 4 || x - y == 5 ||
                                   (1 < x && x < 8 && y == 5) || (x == 2 && y > 4) || (x == 7 && y > 4))
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
