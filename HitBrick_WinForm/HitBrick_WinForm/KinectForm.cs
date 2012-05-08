using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace HitBrick_WinForm
{
    public partial class KinectForm : Form
    {
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Timer timer_time;
        // private bool isKeyDown = false;
        //游戏时间
        int h = 0, m = 0, s = 0;

        // used to be in the controller, replaced by msq
        private System.Drawing.Bitmap bitmap;
        private Brick brick;
        private Ball ball;
        //游戏画面尺寸
        private int width = 0;
        private int height = 0;
        private bool isGameOver = false;
        public int score = 100;

        public KinectForm()
        {
            InitializeComponent();
            timer = new System.Windows.Forms.Timer();
            timer_time = new System.Windows.Forms.Timer();

            this.width = this.splitContainer1.Panel1.Width;
            this.height = this.splitContainer1.Panel1.Height;
            bitmap = new Bitmap(width, height);
            brick = new Brick();
            ball = new Ball(width / 2 - 20, height - 300, 2, 3);
            brick.BrickWall();

            timer.Interval = 10;
            timer.Tick += new EventHandler(timer_Tick);
            timer_time.Interval = 1000;
            timer_time.Tick += new EventHandler(timer_time_Tick);
        }

        //初始化游戏界面
        private void SabBoy_Paint(object sender, PaintEventArgs e)
        {
            //使用双缓冲，减少画面闪烁
            brick.Draw(Graphics.FromImage(bitmap)); //画砖墙
            ball.Draw(Graphics.FromImage(bitmap)); //画小球
            Graphics g = e.Graphics;
            g.DrawImage(bitmap, 0, 0);
            g.Dispose();
        }

        //游戏结束
        public bool IsGameOver()
        {
            /*
            if (ball.Rect.Y >= height - 22)
            {
                isGameOver = true;
            }
            return isGameOver;
             */
            return isGameOver;
        }

        //小球运动
        public void RunBall()
        {
            // ball.Run();
        }

        //碰撞检测
        public void Hit()
        {
            /*
            //砖块与小球碰撞
            for (int i = 0; i < brick.Rects.Count; i++)
            {
                if (ball.Rect.IntersectsWith(brick.Rects[i]))
                {
                    //删除砖块
                    brick.Rects.Remove(brick.Rects[i]);
                    ball.SpeedX = -ball.SpeedX;
                    ball.SpeedY = -ball.SpeedY;
                    //得分
                    sorce += new Random().Next(50, 80) + 100;
                }
            }
            //小球与挡板碰撞
            if (ball.XPos + ball.Rect.Width - 5 >= board.XPos && ball.XPos <= board.XPos + board.Rect.Width - 5)
            {
                if (ball.YPos >= board.YPos - ball.Rect.Height - 2)
                {
                    switch (Direction)
                    {
                        case BoardDirection.Left:
                            {
                                ball.SpeedX = -(new Random().Next(2, 5));
                            }
                            break;
                        case BoardDirection.Right:
                            {
                                ball.SpeedX = (new Random().Next(2, 5));
                            }
                            break;
                        default:
                            break;
                    }
                    ball.SpeedY = (new Random().Next(2, 5));
                }
            }
             * */
        }

        //游戏通关
        public bool IsSuccess()
        {
            bool isSucess = false;
            //没有砖块
            if (brick.Rects.Count == 0)
                isSucess = true;
            return isSucess;
        }

        //游戏驱动
        public void timer_Tick(object sender, EventArgs e)
        {
            if (IsGameOver())
            {
                timer_time.Start();
          
                RunBall();
                Hit();

                //使用双缓冲，减少画面闪烁
                brick.Draw(Graphics.FromImage(bitmap)); //画砖墙
                ball.Draw(Graphics.FromImage(bitmap)); //画小球
                Graphics g = this.CreateGraphics();
                g.DrawImage(bitmap, 0, 0);
                g.Dispose();

                txtScore.Text = "Score: " + score.ToString(); //controler.score.ToString();

                if(IsSuccess())
                {
                    this.CreateGraphics().DrawString("You Win", new Font("Comic Sans MS", 25), new SolidBrush(Color.Red), this.Width / 2 - 100, this.Height / 2 - 50);
                    timer.Stop();
                    timer_time.Stop();
                }
            }
            else
            {
                this.CreateGraphics().DrawString("Game Over", new Font("Comic Sans MS", 25), new SolidBrush(Color.Snow), this.Width / 2 - 100, this.Height / 2 - 50);
                timer_time.Stop();
            }


        }

        /// <summary>
        /// Exit button, need ?
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //游戏时间
        public void timer_time_Tick(object sender, EventArgs e)
        {
            s++;
            if (s >= 59)
            {
                m += 1;
                s = 0;
            }
            if (m >= 59)
            {
                h += 1;
                m = 0;
            }
            txtTime.Text = "Time : " + h.ToString("00") + ":" 
                + m.ToString("00") + ":" + s.ToString("00");
        }
    }
}
