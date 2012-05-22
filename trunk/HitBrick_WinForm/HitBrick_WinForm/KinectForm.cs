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

        //板子
        Point lastCenter = new Point(0, 0);

        // time related
        private const int timer_inter = 10;
        private const int ms_to_second = 1000;
        
        //奖励
        PictureBox bonus;
        Rectangle bonusRect;
        bool have_one_bonus;
        private const int bonus_speed = 5;
        public int bonus_XPos { get; set; }
        public int bonus_YPos { get; set; }

        private const int speedup_x = 0;
        private const int speedup_y = 0;

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

            timer.Interval = timer_inter;
            timer.Tick += new EventHandler(timer_Tick);
            timer_time.Interval = ms_to_second;
            timer_time.Tick += new EventHandler(timer_time_Tick);

            bgmPlayer = new System.Media.SoundPlayer(global::HitBrick_WinForm.Properties.Resources.bgm);

            pbBall = new PictureBox();
            pbBall.Location = new Point(XPos, YPos);
            pbBall.Image = global::HitBrick_WinForm.Properties.Resources.xiaoqiu;
            pbBall.Size = new Size(2 * ball_R, 2 * ball_R);
            this.splitContainer1.Panel1.Controls.Add(pbBall);
            pbBall.BringToFront();

            ballRect = new Rectangle(XPos, YPos, 2 * ball_R, 2 * ball_R);


            bonus = new PictureBox();
            have_one_bonus = false;
            // 带参数的，必须是object型
            // Thread parameterThread = new Thread(new ParameterizedThreadStart());
            // parameterThread.Start(this.));  

            // Thread ballThread = new Thread(RunBall);
            // ballThread.Start();
        }

        public void timer_Tick(object sender, EventArgs e)
        {
            if(!IsGameOver())
            {
                if(!timer_time.Enabled)
                    timer_time.Start();

                RunBall();
                Hit();

                if (have_one_bonus)
                {
                    bonus_YPos = bonus_YPos + bonus_speed;
                    bonus.Location = new Point(bonus_XPos, bonus_YPos);
                    bonusRect = new Rectangle(bonus_XPos, bonus_YPos, 2 * ball_R, 2 * ball_R);
                }

                // this.splitContainer1.Panel1.Refresh();
                // this.splitContainer1.Panel1.Invalidate();
                txtScore.Text = "Score: " + score.ToString();
                // this.Invalidate();
                
                if( IsSuccess())
                {
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
                if (SpeedY < 0)
                {
                    SpeedY = -3;
                }
                // 下面4个变量用来记录相交与否
                // 如果某一条边和小球的矩形相交，则为true
                // 否则为false
                bool[] flags = new bool[4];
                bool hit = false;

                // 相交与否
                flags[0] = Rects[i].rectangle.Contains(new Point(ballRect.X, ballRect.Y));
                flags[1] = Rects[i].rectangle.Contains(new Point(ballRect.X + 2 * ball_R, ballRect.Y));
                flags[2] = Rects[i].rectangle.Contains(new Point(ballRect.X, ballRect.Y + ball_R * 2));
                flags[3] = Rects[i].rectangle.Contains(new Point(ballRect.X + ball_R * 2, ballRect.Y + ball_R * 2));
                                    
                if (flags[0] && flags[1])
                {
                    SpeedY = -SpeedY;
                    hit = true;
                }
                else if (flags[0] && !flags[1] &&!flags[2])
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
                else if (flags[0] && flags[2])
                {
                    hit = true; 
                    SpeedX = -SpeedX;
                }
                else if(flags[2] && !flags[0] && !flags[3])
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
                else if (flags[2] && flags[3])
                {
                    hit = true;
                    SpeedY = -SpeedY;
                }
                else if (flags[3] && !flags[1] && !flags[2])
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
                else if (flags[1] && flags[3])
                {
                    hit = true;
                    SpeedX = -SpeedX;
                }
                else if (flags[1] && !flags[0] && !flags[3])
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

                    // Bounds
                    // 保证一个屏幕上只有一个奖励出现
                    // if ((new Random().Next(3, 8)) % 13 == 0)
                    if(!have_one_bonus && i % 2 == 0)
                    {
                        have_one_bonus = true;

                        bonus_XPos = Rects[i].rectangle.X;
                        bonus_YPos = Rects[i].rectangle.Y;
                        bonus.Location = new Point(bonus_XPos, bonus_YPos);
                        ballRect = new Rectangle(bonus_XPos, bonus_YPos, 2 * ball_R, 2 * ball_R);

                        bonus.Image = global::HitBrick_WinForm.Properties.Resources.xiaoqiu;
                        bonus.Size = new Size(2 * ball_R, 2 * ball_R);
                        this.splitContainer1.Panel1.Controls.Add(bonus);
                        bonus.BringToFront();
                    }

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

            //获得奖励
            if (bonus.RectangleToScreen(bonus.DisplayRectangle).IntersectsWith(render.manImage.RectangleToScreen(render.manImage.DisplayRectangle)))
            {
                //奖励效果
                score += 1;

                have_one_bonus = false;
                bonus.Dispose();
                bonus = new PictureBox();
            }

            //奖励消失
            if (bonus_YPos > this.Height)
            {
                have_one_bonus = false;
            }

            Point center = render.GetBarLocation();

            //小球与挡板碰撞
            if (pbBall.RectangleToScreen(pbBall.DisplayRectangle).IntersectsWith(render.barImage.RectangleToScreen(render.GetBarRect())) 
                &&
                pbBall.RectangleToScreen(pbBall.DisplayRectangle).IntersectsWith(render.manImage.RectangleToScreen(render.manImage.DisplayRectangle)))
            {
                // calculate if hit the board.
                // line， assume a = 0
                // x1 + by1 + c = 0;
                // x2 + by2 + c = 0;
                //double x1 = this.render.LeftHand.X;
                //double y1 = this.render.LeftHand.Y;
                //double x2 = this.render.RightHand.X;
                //double y2 = this.render.RightHand.Y;

                //double b = (x1 - x2) / (y2 - y1);
                //double c = (x2 * y1 - x1 * y2) / (y2 - y1);

                //int count = 0;
                //int[] px = new int[4];
                //int[] py = new int[4];
                //px[0] = ballRect.X;
                //py[0] = ballRect.Y;

                //px[1] = ballRect.X + 2 * ball_R;
                //py[1] = ballRect.Y;
                
                //px[2] = ballRect.X;
                //py[2] = ballRect.Y + 2 * ball_R;
                
                //px[3] = ballRect.X + 2 * ball_R;
                //py[3] = ballRect.Y + 2 * ball_R;
                
                //for(int i = 0; i < 4; i++)
                //{
                //    if (px[i] + b * py[i] + c <= 0)
                //        count++;
                //}
                //if (count >= 1 && count <= 3)
                //{
                int dis_x = center.X - lastCenter.X;
                int dis_y = center.Y - lastCenter.Y;
                double angle = render.Angle;

                if (angle < 0)
                {
                    angle = angle + Math.PI;
                }

                if (angle > Math.PI / 2)
                {
                    angle = Math.PI - angle;
                }

                // so now the v is this 
                // along the board: x*cosα-y*sinα
                // and x*sinα+y*cosα
                // rules: along the board, no changes, the other, direction and speedup.
                
                SpeedX = (int)((SpeedX * Math.Cos(2 * angle) - SpeedY * Math.Sin(2 * angle))
                    + dis_x * speedup_x);
                SpeedY = (int)((SpeedX * Math.Sin(2 * angle) + SpeedY * Math.Cos(2 * angle))
                        + dis_y * speedup_y);
                //}

                // old version of math calculate.
                //if (dis_x < 0)
                //{
                //    SpeedX = -Math.Abs(SpeedX);
                //    SpeedY = Math.Abs(SpeedY) * dis_y * 1;
                //}
                //else if (dis_x > 0)
                //{
                //    SpeedX = -Math.Abs(SpeedX);
                //    SpeedY = Math.Abs(SpeedY) * dis_y * 1;
                //}
                //else
                //{
                //}
            }

            lastCenter = center;
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
                stage++;
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
            pbBall.Size = new Size(2 * ball_R, 2 * ball_R);
            this.splitContainer1.Panel1.Controls.Add(pbBall);
            pbBall.BringToFront();

            ballRect = new Rectangle(XPos, YPos, 2*ball_R, 2*ball_R);

            score = 0;
            txtScore.Text = "Score: 0";

            this.button1.Text = "Pause";
            this.button1.Visible = true;
            this.isGameOver = false;
            timer.Start();
        }
    }
}
