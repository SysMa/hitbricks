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
            if (XPos <= 0)
            {
                SpeedX = -SpeedX;
                //SpeedX = (new Random().Next(3, 8));
            }

            if (XPos > this.splitContainer1.Panel1.Width - ball_R - SpeedX)
            {
                SpeedX = -SpeedX;
                //SpeedX = -(new Random().Next(3, 8));
            }
            if (YPos <= 0)
            {
                SpeedY = -1;
                // SpeedY = (SpeedY > 3) ? (-(SpeedY - 1)) : (-SpeedY);
                //SpeedY = -(new Random().Next(3, 8));
            }

            //
            //if (YPos >= this.splitContainer1.Panel1.Height - ball_R + SpeedY)
            //    SpeedY = -SpeedY;
            //

            pbBall.Location = new Point(XPos, YPos);
            ballRect = new Rectangle(XPos, YPos, 2*ball_R, 2*ball_R);
        }
    }
}
