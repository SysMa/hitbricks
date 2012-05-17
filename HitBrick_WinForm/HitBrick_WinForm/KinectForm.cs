using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Generic;

namespace HitBrick_WinForm
{
    public partial class KinectForm : Form
    {
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Timer timer_time;

        //游戏时间
        int h = 0, m = 0, s = 0;

        // elements
        private System.Drawing.Bitmap bitmap;
        private Ball ball;
        private bool isGameOver = false;
        public int score = 0;

        private int _width = 400; //砖块集宽
        private int _height = 300;//砖块集高

        //砖块
        public struct Brick_Type
        {
            public Rectangle r;
            public int i;
            public PictureBox pictureBox;
        }

        //砖块集
        public List<Brick_Type> Rects { get; set; }

        public KinectForm()
        {
            InitializeComponent();
            newBricks();

            this.DoubleBuffered = true;
            timer = new System.Windows.Forms.Timer();
            timer_time = new System.Windows.Forms.Timer();

            ball = new Ball(378, 78, 20, 30);

            timer.Interval = 10;
            timer.Tick += new EventHandler(timer_Tick);
            timer_time.Interval = 1000;
            timer_time.Tick += new EventHandler(timer_time_Tick);

            //Thread parameterThread = new Thread(new ParameterizedThreadStart(controler.InitGame));
            //parameterThread.Start(this.CreateGraphics());  
        }

        //初始化游戏界面
        private void SabBoy_Paint(object sender, PaintEventArgs e)
        {
            InitGame(e.Graphics);
        }

        // 游戏驱动
        // 我怀疑这个函数在这里是非是合适的。
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

            // those lines should not be here.
            // it is just an example of disappear.
            if (s == 6)
            {
                Rects[0].pictureBox.Visible = false;
            }

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
            ball.Draw(Graphics.FromImage(bitmap)); //画小球
            gra.DrawImage(bitmap, 0, 0);
            gra.Dispose();
            bitmap.Dispose();
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

        //小球运动
        public void RunBall()
        {
            ball.Run();
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

        //游戏通关
        public bool IsSuccess()
        {
            bool isSucess = false;
            return isSucess;
        }

        public void newBricks()
        {
            int temp = 0;
            Rects = new List<Brick_Type>();
            Random rd = new Random();
            for (int i = 100; i < _height; i += 18)
            {
                temp += 18;
                for (int j = temp - 18; j < _width - temp; j += 40)
                {
                    Rectangle Rect = new Rectangle(j, i, 40, 18);
                    Brick_Type temp_brick = new Brick_Type();
                    temp_brick.r = Rect;
                    temp_brick.i = rd.Next() % 3;
                    temp_brick.pictureBox = new PictureBox();
                    Rects.Add(temp_brick);
                }
            }

            foreach (Brick_Type b in Rects)
            {
                Image img;
                switch (b.i)
                {
                    case 0:
                        img = global::HitBrick_WinForm.Properties.Resources.yellow;
                        break;
                    case 1:
                        img = global::HitBrick_WinForm.Properties.Resources.taohong;
                        break;
                    case 2:
                    default:
                        img = global::HitBrick_WinForm.Properties.Resources.green;
                        break;
                }
                b.pictureBox.Location = new Point(b.r.X, b.r.Y);
                b.pictureBox.Image = img;
                b.pictureBox.Size = new Size(40, 18);
                this.splitContainer1.Panel1.Controls.Add(b.pictureBox);
            }
        }
    }
}
