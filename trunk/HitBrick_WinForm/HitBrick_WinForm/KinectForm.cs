using System;
using System.Drawing;
using System.Media;
using System.Windows.Forms;
using System.Collections.Generic;

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

        // 生命值
        private PictureBox[] lifePics = new PictureBox[3];
        private int remainLife = 3;
        private const int maxLife = 3;
        private const int rightestX = 134;
        private const int rightestY = 197;
        private const int dis = 39;
        
        //奖励
        private const int bonus_speed = 3;
        // COUNT is always the last
        public enum Bonus_Type { INCREASE = 0, DECREASE, ADD_LIFE, BOMB, COUNT };
        private const int bonus_bricks = 3;

        public class Bonus
        {
            public PictureBox pic { get; set; }
            public Rectangle rect { get; set; }
            public int xPos { get; set; }
            public int yPos { get; set; }
            public Bonus_Type type { get; set; }
        }
        // 奖励集
        public List<Bonus> bonuses { get; set; }
        private Random random = new Random();

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
            pbBall.BackColor = Color.Transparent;
            pbBall.Image = global::HitBrick_WinForm.Properties.Resources.xiaoqiu;
            pbBall.Size = new Size(2 * ball_R, 2 * ball_R);
            this.splitContainer1.Panel1.Controls.Add(pbBall);
            pbBall.BringToFront();

            ballRect = new Rectangle(XPos, YPos, 2 * ball_R, 2 * ball_R);

            bonuses = new List<Bonus>();

            for (int i = 0; i < maxLife; i++)
            {
                lifePics[i] = new PictureBox();
                lifePics[i].Size = new Size(32, 32);
                lifePics[i].Image = global::HitBrick_WinForm.Properties.Resources.life;
                lifePics[i].Location = new Point(rightestX - dis * i, rightestY);
                this.splitContainer1.Panel2.Controls.Add(lifePics[i]);
            }

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

                // this.splitContainer1.Panel1.Refresh();
                // this.splitContainer1.Panel1.Invalidate();
                txtScore.Text = score.ToString();
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
            txtTime.Text = hour.ToString("00") + ":" 
                + minute.ToString("00") + ":" + second.ToString("00");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Tag == "Start")
            {
                timer.Start();
                timer_time.Start();
                bgmPlayer.PlayLooping();
                button1.Tag = "Pause";
                button1.Image = global::HitBrick_WinForm.Properties.Resources.pauseButton;
            }
            else
            {
                timer.Stop();
                timer_time.Stop();
                bgmPlayer.Stop();
                button1.Tag = "Start";
                button1.Image = global::HitBrick_WinForm.Properties.Resources.startButton;
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
                    if(i % 5 == 0)
                    {
                        Bonus bonus = new Bonus();
                        bonus.xPos = Rects[i].rectangle.X;
                        bonus.yPos = Rects[i].rectangle.Y;
                        bonus.rect = new Rectangle(bonus.xPos, bonus.yPos,
                            2 * ball_R, 2 * ball_R);

                        bonus.pic = new PictureBox();
                        bonus.pic.BackColor = Color.Transparent;
                        bonus.pic.Location = new Point(bonus.xPos, bonus.yPos);
                        bonus.pic.Size = new Size(2 * ball_R, 2 * ball_R);
                        this.splitContainer1.Panel1.Controls.Add(bonus.pic);
                        bonus.pic.BringToFront();

                        switch (random.Next(0, (int)(Bonus_Type.COUNT - 1)))
                        {
                            case (int)Bonus_Type.INCREASE:
                                bonus.type = Bonus_Type.INCREASE;
                                bonus.pic.Image = global::HitBrick_WinForm.Properties.Resources.increaseLength;
                                break;
                            case (int)Bonus_Type.DECREASE:
                                bonus.type = Bonus_Type.DECREASE;
                                bonus.pic.Image = global::HitBrick_WinForm.Properties.Resources.decreaseLength;
                                break;
                            case (int)Bonus_Type.ADD_LIFE:
                                bonus.type = Bonus_Type.ADD_LIFE;
                                bonus.pic.Image = global::HitBrick_WinForm.Properties.Resources.life_small;
                                break;
                            case (int)Bonus_Type.BOMB:
                            default:
                                bonus.type = Bonus_Type.BOMB;
                                bonus.pic.Image = global::HitBrick_WinForm.Properties.Resources.bomb;
                                break;
                        }

                        bonuses.Add(bonus);
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
            for (int i = 0; i < bonuses.Count;)
            {
                Bonus bonus = bonuses[i];
                if (bonus.pic.RectangleToScreen(bonus.pic.DisplayRectangle)
                    .IntersectsWith(render.manImage.RectangleToScreen(render.manImage.DisplayRectangle)))
                {
                    //奖励效果
                    score += 1;
                    switch (bonus.type)
                    {
                        case Bonus_Type.INCREASE:
                            render.type = Render.BarWidth.DOUBLE;
                            break;
                        case Bonus_Type.DECREASE:
                            render.type = Render.BarWidth.HALF;
                            break;
                        case Bonus_Type.ADD_LIFE:
                            if (remainLife < 3)
                            {
                                lifePics[remainLife].Visible = true;
                                remainLife++;
                            }
                            break;
                        case Bonus_Type.BOMB:
                        default:
                            for (int num_bonus = 0; num_bonus < bonus_bricks && num_bonus < Rects.Count; num_bonus++)
                            {
                                int pick = random.Next(0, Rects.Count - 1);
                                // delete the brick
                                Rects[pick].pictureBox.Visible = false;
                                Rects[pick].pictureBox.Dispose();
                                Rects.Remove(Rects[pick]);
                            }
                            break;
                    }

                    bonus.pic.Dispose();
                    bonuses.Remove(bonus);
                }
                //奖励消失
                else if (bonus.yPos > this.Height)
                {
                    bonus.pic.Dispose();
                    bonuses.Remove(bonus);
                }
                else
                {
                    bonus.yPos = bonus.yPos + bonus_speed;
                    bonus.pic.Location = new Point(bonus.xPos, bonus.yPos);
                    bonus.rect = new Rectangle(bonus.xPos, bonus.yPos, 2 * ball_R, 2 * ball_R);
                    i++;
                }
            }

            //Point center = render.GetBarLocation();

            //// 小球与挡板碰撞
            //if (pbBall.RectangleToScreen(pbBall.DisplayRectangle).IntersectsWith(render.barImage.RectangleToScreen(render.GetBarRect())) 
            //    &&
            //    pbBall.RectangleToScreen(pbBall.DisplayRectangle).IntersectsWith(render.manImage.RectangleToScreen(render.manImage.DisplayRectangle)))
            //{
            //    int dis_x = center.X - lastCenter.X;
            //    int dis_y = center.Y - lastCenter.Y;
            //    double angle = render.Angle;

            //    if (angle < 0)
            //    {
            //        angle = angle + Math.PI;
            //    }

            //    if (angle > Math.PI / 2)
            //    {
            //        angle = Math.PI - angle;
            //    }

            //    // so now the v is this 
            //    // along the board: x*cosα-y*sinα
            //    // and x*sinα+y*cosα
            //    // rules: along the board, no changes, the other, direction and speedup.
            //    if ((dis_x > 0 && SpeedX > 0 && SpeedX > dis_x) || (dis_x < 0 && SpeedX < 0 && dis_x > SpeedX))
            //    {
            //        SpeedX = dis_x * 1;
            //        SpeedY = dis_y * 1;
            //    }
            //    else
            //    {
            //        SpeedX = (int)((SpeedX * Math.Cos(2 * angle) - SpeedY * Math.Sin(2 * angle))
            //            + dis_x * speedup_x);
            //        SpeedY = (int)((SpeedX * Math.Sin(2 * angle) + SpeedY * Math.Cos(2 * angle))
            //            + dis_y * speedup_y);

            //        if(SpeedY == 0)
            //            SpeedY = 5;
            //    }

            //    // old version of math calculate.
            //    //if (dis_x < 0)
            //    //{
            //    //    SpeedX = -Math.Abs(SpeedX);
            //    //    SpeedY = Math.Abs(SpeedY) * dis_y * 1;
            //    //}
            //    //else if (dis_x > 0)
            //    //{
            //    //    SpeedX = -Math.Abs(SpeedX);
            //    //    SpeedY = Math.Abs(SpeedY) * dis_y * 1;
            //    //}
            //    //else
            //    //{
            //    //}
            //}

            //lastCenter = center;
        }

        //游戏结束
        public bool IsGameOver()
        {
            if (ballRect.Y >= this.splitContainer1.Panel1.Height - ball_R + SpeedY)
            {
                lifePics[remainLife - 1].Visible = false;
                remainLife -= 1;
                if (remainLife <= 0)
                {
                    isGameOver = true;
                }
                else
                {
                    this.XPos = ori_XPos;
                    this.YPos = ori_YPos;
                    pbBall.Location = new Point(XPos, YPos);
                    ballRect = new Rectangle(XPos, YPos, 2 * ball_R, 2 * ball_R);
                }
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
            txtScore.Text = "0";

            this.button1.Tag = "Pause";
            this.button1.Image = global::HitBrick_WinForm.Properties.Resources.startButton;
            this.button1.Visible = true;
            this.isGameOver = false;

            this.remainLife = 3;
            for (int i = 0; i < maxLife; i++)
            {
                lifePics[i].Visible = true;
            }

            timer.Start();
        }
    }
}
