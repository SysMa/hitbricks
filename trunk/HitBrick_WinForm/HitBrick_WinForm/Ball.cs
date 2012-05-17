using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HitBrick_WinForm
{
    public partial class KinectForm : Form
    {
        //小球
        //坐标
        public int XPos { get; set; }
        public int YPos { get; set; }
        //速度和方向控制
        public int SpeedX { get; set; }
        public int SpeedY { get; set; }
        //对象载体
        public Rectangle ballRect { get; set; }

        private PictureBox pb1;

        private const int ball_R = 10;

        //小球运动
        public void RunBall()
        {
            XPos = XPos + SpeedX;
            YPos = YPos - SpeedY;
            // Console.WriteLine("Position-x:{0}, y:{1}", XPos, YPos);
            if (XPos <= 0)
                SpeedX = (new Random().Next(3, 8));
            if (XPos > this.splitContainer1.Panel1.Width - ball_R - SpeedX)
                SpeedX = -(new Random().Next(3, 8));
            if (YPos <= 0)
                SpeedY = -(new Random().Next(3, 8));

            pb1.Location = new Point(XPos, YPos);
            ballRect = new Rectangle(XPos, YPos, 20, 20);
        }
    }
}
