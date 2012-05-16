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
        // private bool isKeyDown = false;
        //游戏时间
        int h = 0, m = 0, s = 0;

        // private Controler controler;
        private System.Drawing.Bitmap bitmap;
        private Brick brick;
        private Ball ball;
        //游戏画面尺寸
        private int width = 0;
        private int height = 0;
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

            // this should not be the size of the controller
            // we should minius the width because the right zone used
            // we should minius the height because the manPanel used.
            // controler = new Controler(this.Width, this.Height);
            this.width = this.Width;
            this.height = this.Height;
            bitmap = new Bitmap(width, height);
            brick = new Brick();
            ball = new Ball(/*width / 2 - 45, height - 300*/378, 78, 20, 30);
            brick.BrickWall();

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
            // controler.InitGame(e.Graphics);
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
                /*
                if (isKeyDown)
                {
                    controler.MoveBoard();
                }
                */
                // controler.RunBall();        
                // controler.Hit();
                RunBall();
                Hit();

                // this.splitContainer1.Panel1.Refresh();
                // this.splitContainer1.Panel1.Invalidate();
                // controler.InitGame(this.CreateGraphics());
                InitGame(this.CreateGraphics());
                txtScore.Text = "Score: " + /*controler.score.ToString();*/ score.ToString();
                this.Invalidate();
                
                // if (controler.IsSuccess())
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

        /// <summary>
        /// Used be control the board, No need for us.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /*
        private void SabBoy_KeyDown(object sender, KeyEventArgs e)
        {
            isKeyDown = true;
            switch (e.KeyCode)
            {
                case Keys.Left:
                    {
                        // Here is the board direction control code segment
                        // controler.Direction = BoardDirection.Left;
                        timer.Start();
                    }
                    break;
                case Keys.Right:
                    {
                        // Here is the board direction control code segment
                        // controler.Direction = BoardDirection.Right;
                        timer.Start();
                    }
                    break;
                default:
                    break;
            }

        }

        private void SabBoy_KeyUp(object sender, KeyEventArgs e)
        {
            isKeyDown = false;
        }
        */ 
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

            // those lines should not be here.
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
            bitmap = new Bitmap(width, height);
            // brick.Draw(Graphics.FromImage(bitmap)); //画砖墙
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

        //移动挡板
        public void MoveBoard()
        {
            // board.Run();
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
            //没有砖块
            if (brick.Rects.Count == 0)
                isSucess = true;
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
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(286, 113);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 2;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;

            this.splitContainer1.Panel1.Controls.Add(this.button3);
            this.splitContainer1.Panel1.Controls.Add(this.button2);
        }
    }
}
