using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace HitBrick_WinForm
{
    public partial class KinectForm : Form
    {
        //小球
        //坐标
        private const int ori_SpeedX = -5;
        private const int ori_SpeedY = 1;

        private class Ball
        {
            public Ball(int xPos, int yPos)
            {
                this.xPos = xPos;
                this.yPos = yPos;

                speedX = ori_SpeedX;
                speedY = ori_SpeedY;

                pic = new PictureBox();
                pic.Location = new Point(xPos, yPos);
                pic.BackColor = Color.Transparent;
                pic.Image = global::HitBrick_WinForm.Properties.Resources.ball;
                pic.Size = new Size(2 * ball_R, 2 * ball_R);

                rect = new Rectangle(xPos, yPos, 2 * ball_R, 2 * ball_R);
            }
            public PictureBox pic;
            public int xPos;
            public int yPos;
            public int speedX;
            public int speedY;
            public Rectangle rect;
        }

        private List<Ball> balls = new List<Ball>();

        //public int XPos { get; set; }
        //public int YPos { get; set; }
        ////速度和方向控制
        //public int SpeedX { get; set; }
        //public int SpeedY { get; set; }
        ////对象载体
        //public Rectangle ballRect { get; set; }

        //private PictureBox pbBall;

        private const int ball_R = 8;

        //小球运动
        public void RunBall()
        {
            for (int i = 0; i < balls.Count; i++)
            {

                balls[i].xPos = balls[i].xPos + balls[i].speedX;
                balls[i].yPos = balls[i].yPos - balls[i].speedY;
                if (balls[i].xPos <= 0)
                {
                    balls[i].speedX = -balls[i].speedX;
                    //SpeedX = (new Random().Next(3, 8));
                }

                if (balls[i].xPos > this.splitContainer1.Panel1.Width - ball_R - balls[i].speedX)
                {
                    balls[i].speedX = -balls[i].speedX;
                    //SpeedX = -(new Random().Next(3, 8));
                }
                if (balls[i].yPos <= 0)
                {
                    balls[i].speedY = -1;
                    // SpeedY = (SpeedY > 3) ? (-(SpeedY - 1)) : (-SpeedY);
                    //SpeedY = -(new Random().Next(3, 8));
                }

                //
                if (balls[i].yPos >= this.splitContainer1.Panel1.Height - ball_R + balls[i].speedY)
                    balls[i].speedY = -balls[i].speedY;
                //

                balls[i].pic.Location = new Point(balls[i].xPos, balls[i].yPos);
                balls[i].rect = new Rectangle(balls[i].xPos, balls[i].yPos, 2 * ball_R, 2 * ball_R);
            }
        }

        public void BulletsRun()
        {
            for (int i = 0; i < bullets.Count;)
            {

                bullets[i].xPos = bullets[i].xPos + bullets[i].speedX;
                bullets[i].yPos = bullets[i].yPos - bullets[i].speedY;

                if (bullets[i].yPos < 0)
                {
                    bullets[i].pic.Visible = false;
                    bullets[i].pic.Dispose();
                    bullets.Remove(bullets[i]);
                    // numberOfBullets--;
                }
                else
                {
                    bullets[i].pic.Location = new Point(bullets[i].xPos, bullets[i].yPos);
                    bullets[i].rect = new Rectangle(bullets[i].xPos, bullets[i].yPos, 2 * ball_R, 2 * ball_R);
                    i++;
                }

            }
        }

    }
}
