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
        private System.Drawing.Bitmap bitmap;
        //private Ball ball;
        private bool isGameOver = false;
        public int score = 0;

        //音乐部分
        SoundPlayer bgmPlayer;

        //小球
        //坐标
        public int XPos { get; set; }
        public int YPos { get; set; }
        //速度和方向控制
        public int SpeedX { get; set; }
        public int SpeedY { get; set; }
        //对象载体
        public Rectangle ballRect { get; set; }

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
            this.SpeedX = 20;
            this.SpeedY = 30;

            timer.Interval = 10;
            timer.Tick += new EventHandler(timer_Tick);
            timer_time.Interval = 1000;
            timer_time.Tick += new EventHandler(timer_time_Tick);

            bgmPlayer = new System.Media.SoundPlayer(global::HitBrick_WinForm.Properties.Resources.bgm);

            //Thread parameterThread = new Thread(new ParameterizedThreadStart(controler.InitGame));
            //parameterThread.Start(this.CreateGraphics());  
        }

        //初始化游戏界面
        private void SabBoy_Paint(object sender, PaintEventArgs e)
        {
            InitGame(e.Graphics);
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
                InitGame(this.CreateGraphics());
                txtScore.Text = "Score: " + score.ToString();
                this.Invalidate();
                
                if( IsSuccess())
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

        /*
         * FUNCTIONS used for Controller.
         */
        //初始化画面
        public void InitGame(object g)
        {
            Graphics gra = (Graphics)g;
            //使用双缓冲，减少画面闪烁
            bitmap = new Bitmap(this.Width, this.Height);
            ballDraw(Graphics.FromImage(bitmap)); //画小球
            gra.DrawImage(bitmap, 0, 0);
            gra.Dispose();
            bitmap.Dispose();
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
             * */
        }

        //小球运动
        public void RunBall()
        {
            XPos = XPos + SpeedX;
            YPos = YPos - SpeedY;
            // Console.WriteLine("Position-x:{0}, y:{1}", XPos, YPos);
            if (XPos <= 0)
                SpeedX = (new Random().Next(3, 5));
            if (XPos > 378)
                SpeedX = -(new Random().Next(3, 5));
            if (YPos <= 100)
                SpeedY = -(new Random().Next(3, 8));
        }

        //游戏结束
        public bool IsGameOver()
        {
            /*
            if (ball.Rect.Y >= height - 22)
            {
                isGameOver = true;
            }
             */
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

        public void ballDraw(Graphics g)
        {
            using (SolidBrush sbrush = new SolidBrush(Color.Snow))
            {
                ballRect = new Rectangle(XPos, YPos, 20, 20);
                g.DrawEllipse(new Pen(Color.Gray), ballRect);
                g.FillEllipse(sbrush, ballRect);
            }
            g.Dispose();
        }
    }
}
