using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

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

        // new bricks
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;

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
                this.button2.Visible = false;
                this.button3.Visible = false;
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
            this.button2 = new Button();
            this.button3 = new Button();
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(54, 36);
            this.button2.TabIndex = 1;
            this.button2.BackColor = Color.Yellow;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(286, 113);

            this.splitContainer1.Panel1.Controls.Add(this.button3);
            this.splitContainer1.Panel1.Controls.Add(this.button2);

            // gets the rectangle of the button 
            // Rectangle rec_bt2 = button2.ClientRectangle;
        }
    }
}
