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
        private System.Windows.Forms.Timer start_timer;

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
        public enum Bonus_Type { INCREASE = 0, DECREASE, ADD_LIFE, BOMB, THREE_BALLS, HEAVY_BALL, BULLET, COUNT };
        private const int bonus_bricks = 3;
        private bool isHeavy = false;
        private int heavyDuration = 0;

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

        // 子弹
        public class Bullet
        {
            public PictureBox pic { get; set; }
            public Rectangle rect { get; set; }
            public int xPos { get; set; }
            public int yPos { get; set; }
            public int speedX { get; set; }
            public int speedY { get; set; }
        }
        public List<Bullet> bullets { get; set; }
        private int numberOfBullets = 0;

        public KinectForm()
        {
            // this.DoubleBuffered = true;
            InitializeComponent();
            this.oversign.Visible = false;

            newBricks();

            timer = new System.Windows.Forms.Timer();
            timer_time = new System.Windows.Forms.Timer();
            start_timer = new System.Windows.Forms.Timer();

            timer.Interval = timer_inter;
            timer.Tick += new EventHandler(timer_Tick);
            timer_time.Interval = ms_to_second;
            timer_time.Tick += new EventHandler(timer_time_Tick);
            start_timer.Interval = ms_to_second;
            start_timer.Tick += new EventHandler(judge_begin);

            bgmPlayer = new System.Media.SoundPlayer(global::HitBrick_WinForm.Properties.Resources.bgm);

            Ball ball = new Ball(this.splitContainer1.Panel1.Width / 2, begin + _height - 3 * 18);
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
            start_timer.Start();
        }

        public void timer_Tick(object sender, EventArgs e)
        {
            if(!IsGameOver())
            {
                if(!timer_time.Enabled)
                    timer_time.Start();

                BulletsRun();
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

            if (isHeavy)
            {
                heavyDuration++;
                if (heavyDuration > 5)
                {
                    isHeavy = false;
                    heavyDuration = 0;
                    for (int i = 0; i < balls.Count; i++)
                    {
                        balls[i].pic.Image = global::HitBrick_WinForm.Properties.Resources.ball;
                    }
                }
            }

            if ( (numberOfBullets > 0) && (render.getBegin() == 1))
            {
                Bullet one_bullet = new Bullet();
                one_bullet.xPos = manImage.DisplayRectangle.X + manImage.DisplayRectangle.Width / 2;
                one_bullet.yPos = manImage.DisplayRectangle.Y - 2 * ball_R;
                one_bullet.rect = new Rectangle(one_bullet.xPos, one_bullet.yPos, 2 * ball_R, 2 * ball_R);

                one_bullet.pic = new PictureBox();
                one_bullet.pic.BackColor = Color.Transparent;
                one_bullet.pic.Location = new Point(one_bullet.xPos, one_bullet.yPos);
                one_bullet.pic.Size = new Size(2 * ball_R, 2 * ball_R);
                this.splitContainer1.Panel1.Controls.Add(one_bullet.pic);
                one_bullet.pic.BringToFront();
                one_bullet.pic.Image = global::HitBrick_WinForm.Properties.Resources.ball;

                one_bullet.speedX = 0;
                one_bullet.speedY = 3;

                bullets.Add(one_bullet);
            }

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

        public void judge_begin(object sender, EventArgs e)
        {
            if (this.render.getBegin() == 1)
            {
                start_timer.Stop();
                start_timer.Dispose();
                timer.Start();
                bgmPlayer.PlayLooping();
            }
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
                    Ball ball = new Ball(this.splitContainer1.Panel1.Width / 2, begin + _height - 3 * 18);
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

            for (int i = 0; i < balls.Count; i++)
            {
                balls[i].pic.Dispose();
            }
            balls.Clear();
            Ball ball = new Ball(this.splitContainer1.Panel1.Width / 2, begin + _height - 3 * 18);
            this.splitContainer1.Panel1.Controls.Add(ball.pic);
            ball.pic.BringToFront();
            balls.Add(ball);

            render.type = Render.BarWidth.NORMAL;

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
