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
        int hour = 0, minute = 0, second = 0;

        // elements
        private bool isGameOver = false;
        public int score = 0;

        //音乐部分
        SoundPlayer bgmPlayer;

        //关卡
        private uint stage = 1;

        public KinectForm()
        {
            // this.DoubleBuffered = true;
            InitializeComponent();
            this.oversign.Visible = false;

            newBricks();

            timer = new System.Windows.Forms.Timer();
            timer_time = new System.Windows.Forms.Timer();

            this.XPos = ori_XPos;
            this.YPos = ori_YPos;
            this.SpeedX = ori_SpeedX;
            this.SpeedY = ori_SpeedY;

            timer.Interval = 10;
            timer.Tick += new EventHandler(timer_Tick);
            timer_time.Interval = 1000;
            timer_time.Tick += new EventHandler(timer_time_Tick);

            bgmPlayer = new System.Media.SoundPlayer(global::HitBrick_WinForm.Properties.Resources.bgm);

            pbBall = new PictureBox();
            pbBall.Location = new Point(XPos, YPos);
            pbBall.Image = global::HitBrick_WinForm.Properties.Resources.xiaoqiu;
            pbBall.Size = new Size(16, 16);
            this.splitContainer1.Panel1.Controls.Add(pbBall);
            pbBall.BringToFront();

            ballRect = new Rectangle(XPos, YPos, 16, 16);
            
            // 带参数的，必须是object型
            // Thread parameterThread = new Thread(new ParameterizedThreadStart());
            // parameterThread.Start(this.));  

            // Thread ballThread = new Thread(RunBall);
            // ballThread.Start();
        }

        public void timer_Tick(object sender, EventArgs e)
        {
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

                    foreach (Brick_Type brick in Rects)
                    {
                        brick.pictureBox.Dispose();
                    }
                    Rects.Clear();
                    newBricks();
                }
            }
            else
            {
                timer_time.Stop();
                timer.Stop();
                bgmPlayer.Stop();
                foreach (Brick_Type brick in Rects)
                {
                    brick.pictureBox.Dispose();
                }
                Rects.Clear();
                this.button1.Visible = false;
                this.oversign.Visible = true;
            }
        }

        //游戏时间
        public void timer_time_Tick(object sender, EventArgs e)
        {
            second++;
            
            if (second >= 59)
            {
                minute += 1;
                second = 0;
            }
            if (minute >= 59)
            {
                hour += 1;
                minute = 0;
            }
            txtTime.Text = "Time : " + hour.ToString("00") + ":" 
                + minute.ToString("00") + ":" + second.ToString("00");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Start")
            {
                timer.Start();
                bgmPlayer.PlayLooping();
                button1.Text = "Pause";
            }
            else
            {
                timer.Stop();
                bgmPlayer.Stop();
                button1.Text = "Start";
            }
        }

        //碰撞检测
        public void Hit()
        {
            //砖块与小球碰撞
            for (int i = 0; i < Rects.Count; i++)
            {
                // 下面8个变量用来记录相交与否
                // 如果某一条边和小球的矩形相交，则为true
                // 否则为false
                bool[] flags = new bool[8];
                bool hit = false;

                // 相交与否
                flags[0] = Rects[i].rectangle.Contains(new Point(ballRect.X, ballRect.Y));
                flags[1] = Rects[i].rectangle.Contains(new Point(ballRect.X + ball_R, ballRect.Y));
                flags[2] = Rects[i].rectangle.Contains(new Point(ballRect.X + 2 * ball_R, ballRect.Y));
                flags[3] = Rects[i].rectangle.Contains(new Point(ballRect.X, ballRect.Y + ball_R));
                flags[4] = Rects[i].rectangle.Contains(new Point(ballRect.X + 2 * ball_R, ballRect.Y + ball_R));
                flags[5] = Rects[i].rectangle.Contains(new Point(ballRect.X, ballRect.Y + ball_R * 2));
                flags[6] = Rects[i].rectangle.Contains(new Point(ballRect.X + ball_R, ballRect.Y + ball_R * 2));
                flags[7] = Rects[i].rectangle.Contains(new Point(ballRect.X + ball_R * 2, ballRect.Y + ball_R * 2));
                                    
                //if ((flags[0] && (!flags[2]) && !(flags[1] && !flags[3])) ||
                //    (flags[7] && (!flags[5]) && !(flags[6] && !flags[4])) ||
                //    !(flags[5] && !flags[3] && !(!flags[0] && !flags[6])) ||
                //    !(flags[2] && !flags[4] && !(!flags[7] && !flags[1]))
                //    )
                //{
                //    SpeedX = -SpeedX;
                //}

                //if (!(flags[0] && !flags[1] && !(!flags[2] && !flags[3])) ||
                //    !(flags[7] && !flags[6] && !(!flags[5] && !flags[4])) ||
                //    (flags[5] && (!flags[0]) && !(flags[3] && !flags[6])) ||
                //    (flags[2] && (!flags[7]) && !(flags[4] && !flags[1]))
                //    )
                //{
                //    SpeedY = -SpeedY;
                //}

                if (flags[0] && flags[2])
                {
                    SpeedY = -SpeedY;
                    hit = true;
                }
                else if (flags[0] && !flags[2] &&!flags[5])
                {
                    hit = true;
                    if (SpeedX > 0 && SpeedY > 0)
                    {
                        SpeedY = -SpeedY;
                    }
                    else if (SpeedX < 0)
                    {
                        SpeedX = -SpeedX;
                    }
                    else if(SpeedX > 0 && SpeedY < 0)
                    {
                        // impossible
                        return;
                    }
                }
                else if (flags[0] && flags[5])
                {
                    hit = true; 
                    SpeedX = -SpeedX;
                }
                else if(flags[5] && !flags[0] && !flags[7])
                {
                    hit = true;
                    if (SpeedX > 0 && SpeedY < 0)
                    {
                        SpeedY = -SpeedY;
                    }
                    else if (SpeedX < 0)
                    {
                        SpeedX = -SpeedX;
                    }
                    else if (SpeedX > 0 && SpeedY > 0)
                    {
                        // impossible
                        return;
                    }
                }
                else if (flags[5] && flags[7])
                {
                    hit = true;
                    SpeedY = -SpeedY;
                }
                else if (flags[7] && !flags[2] && !flags[5])
                {
                    hit = true;
                    if (SpeedX < 0 && SpeedY < 0)
                    {
                        SpeedY = -SpeedY;
                    }
                    else if (SpeedX > 0)
                    {
                        SpeedX = -SpeedX;
                    }
                    else if (SpeedX < 0 && SpeedY > 0)
                    {
                        // impossible
                        return;
                    }
                }
                else if (flags[2] && flags[7])
                {
                    hit = true;
                    SpeedX = -SpeedX;
                }
                else if (flags[2] && !flags[0] && !flags[7])
                {
                    hit = true;
                    if (SpeedX < 0 && SpeedY > 0)
                    {
                        SpeedY = -SpeedY;
                    }
                    else if (SpeedX > 0)
                    {
                        SpeedX = -SpeedX;
                    }
                    else if (SpeedX < 0 && SpeedY < 0)
                    {
                        // impossible
                        return;
                    }
                }
                if (hit)
                {
                    //得分
                    score += (Rects[i].type + 1) * 10;

                    //删除砖块
                    Rects[i].pictureBox.Visible = false;
                    Rects[i].pictureBox.Dispose();
                    Rects.Remove(Rects[i]);

                    Mp3 mp3 = new Mp3();
                    mp3.FileName = @"..\..\Resources\hitBricks.wav";
                    mp3.play();
                    break;
                }
            }
            
            //小球与挡板碰撞
            if (pbBall.RectangleToScreen(pbBall.DisplayRectangle).IntersectsWith(render.barImage.RectangleToScreen(render.GetBarRect())) &&
                pbBall.RectangleToScreen(pbBall.DisplayRectangle).IntersectsWith(render.manImage.RectangleToScreen(render.manImage.DisplayRectangle)))
            {
                SpeedX = -SpeedX;
                SpeedY = -SpeedY;
            }
        }

        //游戏结束
        public bool IsGameOver()
        {
            if (ballRect.Y >= this.splitContainer1.Panel1.Height - ball_R + SpeedY)
            {
                isGameOver = true;
            }
            return isGameOver;
            
            // return false;
        }

        //游戏通关
        public bool IsSuccess()
        {
            if (Rects.Count == 0)
            {
                stage++;
                // MessageBox.Show("Congratulations! Next Stage!");
                return true;
            }
            return false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.oversign.Visible = false;

            this.stage = 1;
            foreach (Brick_Type brick in Rects)
            {
                brick.pictureBox.Dispose();
            }
            Rects.Clear();
            newBricks();

            pbBall.Dispose();
            this.XPos = ori_XPos;
            this.YPos = ori_YPos;
            this.SpeedX = ori_SpeedX;
            this.SpeedY = ori_SpeedY;
            pbBall = new PictureBox();
            pbBall.Location = new Point(XPos, YPos);
            pbBall.Image = global::HitBrick_WinForm.Properties.Resources.xiaoqiu;
            pbBall.Size = new Size(16, 16);
            this.splitContainer1.Panel1.Controls.Add(pbBall);
            pbBall.BringToFront();

            ballRect = new Rectangle(XPos, YPos, 16, 16);

            score = 0;
            txtScore.Text = "Score: 0";

            this.button1.Text = "Pause";
            this.button1.Visible = true;
            this.isGameOver = false;
            timer.Start();
        }
    }
}
