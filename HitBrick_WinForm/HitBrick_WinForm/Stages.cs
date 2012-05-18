using System;
using System.Drawing;
using System.Windows.Forms;

namespace HitBrick_WinForm
{
    public partial class KinectForm : Form
    {
        public void draw_stage_one()
        {
            int temp = 0;
            Random rd = new Random();
            for (int i = 100; i < _height; i += 18)
            {
                temp += 18;
                for (int j = temp - 18; j < _width - temp; j += 40)
                {
                    Rectangle Rect = new Rectangle(j, i, 40, 18);
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
