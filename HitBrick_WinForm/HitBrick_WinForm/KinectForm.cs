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
        public enum Bonus_Type { INCREASE = 0, DECREASE, ADD_LIFE, BOMB, THREE_BALL, COUNT };
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

            timer.Interval = timer_inter;
            timer.Tick += new EventHandler(timer_Tick);
            timer_time.Interval = ms_to_second;
            timer_time.Tick += new EventHandler(timer_time_Tick);

            bgmPlayer = new System.Media.SoundPlayer(global::HitBrick_WinForm.Properties.Resources.bgm);

            Ball ball = new Ball(ori_XPos, ori_YPos);
            this.splitContainer1.Panel1.Controls.Add(ball.pic);
            ball.pic.BringToFront();
            balls.Add(ball);

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
            if (button1.Tag.ToString() == "Start")
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
            for (int j = 0; j < balls.Count; j++)
            {
                //砖块与小球碰撞
                for (int i = 0; i < Rects.Count; i++)
                {
                    if (balls[j].speedY < 0)
                    {
                        balls[j].speedY = -3;
                    }
                    // 下面4个变量用来记录相交与否
                    // 如果某一条边和小球的矩形相交，则为true
                    // 否则为false
                    bool[] flags = new bool[4];
                    bool hit = false;

                    // 相交与否
                    flags[0] = Rects[i].rectangle.Contains(
                        new Point(balls[j].rect.X, balls[j].rect.Y));
                    flags[1] = Rects[i].rectangle.Contains(
                        new Point(balls[j].rect.X + 2 * ball_R, balls[j].rect.Y));
                    flags[2] = Rects[i].rectangle.Contains(
                        new Point(balls[j].rect.X, balls[j].rect.Y + ball_R * 2));
                    flags[3] = Rects[i].rectangle.Contains(
                        new Point(balls[j].rect.X + ball_R * 2, balls[j].rect.Y + ball_R * 2));

                    if (flags[0] && flags[1])
                    {
                        balls[j].speedY = -balls[j].speedY;
                        hit = true;
                    }
                    else if (flags[0] && !flags[1] && !flags[2])
                    {
                        hit = true;
                        if (balls[j].speedX > 0 && balls[j].speedY > 0)
                        {
                            balls[j].speedY = -balls[j].speedY;
                        }
                        else if (balls[j].speedX < 0)
                        {
                            balls[j].speedX = -balls[j].speedX;
                        }
                        else if (balls[j].speedX > 0 && balls[j].speedY < 0)
                        {
                            // impossible
                            return;
                        }
                    }
                    else if (flags[0] && flags[2])
                    {
                        hit = true;
                        balls[j].speedX = -balls[j].speedX;
                    }
                    else if (flags[2] && !flags[0] && !flags[3])
                    {
                        hit = true;
                        if (balls[j].speedX > 0 && balls[j].speedY < 0)
                        {
                            balls[j].speedY = -balls[j].speedY;
                        }
                        else if (balls[j].speedX < 0)
                        {
                            balls[j].speedX = -balls[j].speedX;
                        }
                        else if (balls[j].speedX > 0 && balls[j].speedY > 0)
                        {
                            // impossible
                            return;
                        }
                    }
                    else if (flags[2] && flags[3])
                    {
                        hit = true;
                        balls[j].speedY = -balls[j].speedY;
                    }
                    else if (flags[3] && !flags[1] && !flags[2])
                    {
                        hit = true;
                        if (balls[j].speedX < 0 && balls[j].speedY < 0)
                        {
                            balls[j].speedY = -balls[j].speedY;
                        }
                        else if (balls[j].speedX > 0)
                        {
                            balls[j].speedX = -balls[j].speedX;
                        }
                        else if (balls[j].speedX < 0 && balls[j].speedY > 0)
                        {
                            // impossible
                            return;
                        }
                    }
                    else if (flags[1] && flags[3])
                    {
                        hit = true;
                        balls[j].speedX = -balls[j].speedX;
                    }
                    else if (flags[1] && !flags[0] && !flags[3])
                    {
                        hit = true;
                        if (balls[j].speedX < 0 && balls[j].speedY > 0)
                        {
                            balls[j].speedY = -balls[j].speedY;
                        }
                        else if (balls[j].speedX > 0)
                        {
                            balls[j].speedX = -balls[j].speedX;
                        }
                        else if (balls[j].speedX < 0 && balls[j].speedY < 0)
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
                        if (i % 5 == 0)
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
                                    bonus.type = Bonus_Type.BOMB;
                                    bonus.pic.Image = global::HitBrick_WinForm.Properties.Resources.bomb;
                                    break;
                                case (int)Bonus_Type.THREE_BALL:
                                default:
                                    bonus.type = Bonus_Type.THREE_BALL;
                                    bonus.pic.Image = global::HitBrick_WinForm.Properties.Resources.xiaoqiu;
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
            }

            //获得奖励
            for (int i = 0; i < bonuses.Count;)
            {
                Bonus bonus = bonuses[i];
                if (bonus.pic.RectangleToScreen(bonus.pic.DisplayRectangle)
                    .IntersectsWith(render.manImage.RectangleToScreen(render.manImage.DisplayRectangle)))
                {
                    //奖励效果
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
                            for (int num_bonus = 0; num_bonus < bonus_bricks && num_bonus < Rects.Count; num_bonus++)
                            {
                                int pick = random.Next(0, Rects.Count - 1);
                                // delete the brick
                                Rects[pick].pictureBox.Visible = false;
                                Rects[pick].pictureBox.Dispose();
                                Rects.Remove(Rects[pick]);
                            }
                            break;
                        case Bonus_Type.THREE_BALL:
                        default:
                            {
                                Ball ball1 = new Ball(10, 50);
                                this.splitContainer1.Panel1.Controls.Add(ball1.pic);
                                ball1.pic.BringToFront();
                                balls.Add(ball1);
                                Ball ball2 = new Ball(this.splitContainer1.Panel1.Width - 30, 50);
                                this.splitContainer1.Panel1.Controls.Add(ball2.pic);
                                ball2.pic.BringToFront();
                                balls.Add(ball2);
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

            Point center = render.GetBarLocation();

            for (int i = 0; i < balls.Count; i++)
            {
                //小球与挡板碰撞
                if (balls[i].pic.RectangleToScreen(balls[i].pic.DisplayRectangle).IntersectsWith(
                    render.barImage.RectangleToScreen(render.GetBarRect()))
                    &&
                    balls[i].pic.RectangleToScreen(balls[i].pic.DisplayRectangle).IntersectsWith(
                    render.manImage.RectangleToScreen(render.manImage.DisplayRectangle)))
                {
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
                    if ((dis_x > 0 && balls[i].speedX > 0 && balls[i].speedX > dis_x) ||
                        (dis_x < 0 && balls[i].speedX < 0 && dis_x > balls[i].speedX))
                    {
                        balls[i].speedX = dis_x * 1;
                        balls[i].speedX = dis_y * 1;
                    }
                    else
                    {
                        balls[i].speedX = (int)((balls[i].speedX * Math.Cos(2 * angle)
                            - balls[i].speedY * Math.Sin(2 * angle)) + dis_x * speedup_x);
                        balls[i].speedY = (int)((balls[i].speedX * Math.Sin(2 * angle)
                            + balls[i].speedY * Math.Cos(2 * angle)) + dis_y * speedup_y);

                        if (balls[i].speedY == 0)
                            balls[i].speedY = 5;
                    }

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
            }

            lastCenter = center;
        }

        //游戏结束
        public bool IsGameOver()
        {
            for (int i = 0; i < balls.Count; )
            {
                if (balls[i].rect.Y >= this.splitContainer1.Panel1.Height - ball_R + balls[i].speedY)
                {
                    balls[i].pic.Dispose();
                    balls.Remove(balls[i]);
                }
                else
                {
                    i++;
                }
            }

            if (balls.Count == 0)
            {
                lifePics[remainLife - 1].Visible = false;
                remainLife -= 1;
                if (remainLife <= 0)
                {
                    isGameOver = true;
                }
                else
                {
                    Ball ball = new Ball(ori_XPos, ori_YPos);
                    this.splitContainer1.Panel1.Controls.Add(ball.pic);
                    ball.pic.BringToFront();
                    balls.Add(ball);
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

            balls.Clear();
            Ball ball = new Ball(ori_XPos, ori_YPos);
            this.splitContainer1.Panel1.Controls.Add(ball.pic);
            ball.pic.BringToFront();
            balls.Add(ball);

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
