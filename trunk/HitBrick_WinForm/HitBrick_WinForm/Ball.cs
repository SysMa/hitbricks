﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace HitBrick_WinForm
{
    public partial class KinectForm : Form
    {
        //小球
        //坐标
        private const int ori_XPos = 378;
        private const int ori_YPos = 78;
        private const int ori_SpeedX = -5;
        private const int ori_SpeedY = 1;

        public int XPos { get; set; }
        public int YPos { get; set; }
        //速度和方向控制
        public int SpeedX { get; set; }
        public int SpeedY { get; set; }
        //对象载体
        public Rectangle ballRect { get; set; }

        private PictureBox pbBall;

        private const int ball_R = 8;

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

            //
            if (YPos >= this.splitContainer1.Panel1.Height - ball_R + SpeedY)
                SpeedY = -SpeedY;
            //

            pbBall.Location = new Point(XPos, YPos);
            ballRect = new Rectangle(XPos, YPos, 16, 16);
        }
    }
}
