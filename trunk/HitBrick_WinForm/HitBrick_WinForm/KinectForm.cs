using System;
using System.Drawing;
using System.Media;
using System.Windows.Forms;

namespace HitBrick_WinForm
{
    public partial class KinectForm : Form
    {
        //计时器
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Timer timer_time;

        //游戏时间
        int h = 0, m = 0, s = 0;

        // elements
        private bool isGameOver = false;
        public int score = 0;

        //音乐部分
        SoundPlayer bgmPlayer;

        public KinectForm()
        {
            InitializeComponent();
            newBricks();

            this.DoubleBuffered = true;
            timer = new System.Windows.Forms.Timer();
            timer_time = new System.Windows.Forms.Timer();

            //ball = new Ball(378, 78, 20, 30);
            this.XPos = 378;
            this.YPos = 78;
            this.SpeedX = 5;
            this.SpeedY = 1;

            timer.Interval = 10;
            timer.Tick += new EventHandler(timer_Tick);
            timer_time.Interval = 1000;
            timer_time.Tick += new EventHandler(timer_time_Tick);

            bgmPlayer = new System.Media.SoundPlayer(global::HitBrick_WinForm.Properties.Resources.bgm);

            pb1 = new PictureBox();
            pb1.Location = new Point(XPos, YPos);
            pb1.Image = global::HitBrick_WinForm.Properties.Resources.xiaoqiu;
            pb1.Size = new Size(20, 20);
            this.splitContainer1.Panel1.Controls.Add(pb1);

            ballRect = new Rectangle(XPos, YPos, 20, 20);
            //Thread parameterThread = new Thread(new ParameterizedThreadStart(controler.InitGame));
            //parameterThread.Start(this.CreateGraphics());  
        }

        public void timer_Tick(object sender, EventArgs e)
        {
            // if (!controler.IsGameOver())
            if( !IsGameOver())
            {
                if(!timer_time.Enabled)
                    timer_time.Start();

                RunBall();
                Hit();

                // this.splitContainer1.Panel1.Refresh();
                // this.splitContainer1.Panel1.Invalidate();
                txtScore.Text = "Score: " + score.ToString();
                // this.Invalidate();
                
                if( IsSuccess())
                {
                    this.CreateGraphics().DrawString("You Win", new Font("Comic Sans MS", 25), new SolidBrush(Color.Red), this.Width / 2 - 100, this.Height / 2 - 50);
                    timer.Stop();
                    timer_time.Stop();
                }
            }
            else
            {
                bgmPlayer.Stop();
                this.CreateGraphics().DrawString("Game Over", new Font("Comic Sans MS", 25), new SolidBrush(Color.Snow), this.Width / 2 - 100, this.Height / 2 - 50);
                timer_time.Stop();
            }
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

        private void button1_Click(object sender, EventArgs e)
        {
            timer.Start();
            bgmPlayer.PlayLooping();
        }

        //碰撞检测
        public void Hit()
        {
            
            //砖块与小球碰撞
            for (int i = 0; i < Rects.Count; i++)
            {
                if (ballRect.IntersectsWith(Rects[i].r))
                {
                    SpeedX = -SpeedX;
                    SpeedY = -SpeedY;
                    //删除砖块
                    Rects[i].pictureBox.Visible = false;
                    Rects.Remove(Rects[i]);
                    //得分
                    score += new Random().Next(50, 80);

                    Mp3 mp3 = new Mp3();
                    mp3.FileName = @"..\..\Resources\hitBricks.wav";
                    mp3.play();
                }
            }
            /*
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
            */
        }


        //游戏结束
        public bool IsGameOver()
        {
            if (ballRect.Y >= this.splitContainer1.Panel1.Height - ball_R + SpeedY)
            {
                isGameOver = true;
            }
            return isGameOver;
        }

        //游戏通关
        public bool IsSuccess()
        {
            if (Rects.Count == 0)
            {
                return true;
            }
            return false;
        }
    }
}
