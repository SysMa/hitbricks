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
        private Controler controler;
        // private bool isKeyDown = false;
        //游戏时间
        int h = 0, m = 0, s = 0;

        public KinectForm()
        {
            InitializeComponent();
            timer = new System.Windows.Forms.Timer();
            timer_time = new System.Windows.Forms.Timer();

            // this should not be the size of the controller
            // we should minius the width because the right zone used
            // we should minius the height because the manPanel used.
            controler = new Controler(this.Width, this.Height);

            timer.Interval = 1000;
            timer.Tick += new EventHandler(timer_Tick);
            timer_time.Interval = 1000;
            timer_time.Tick += new EventHandler(timer_time_Tick);

            // Thread parameterThread = new Thread(new ParameterizedThreadStart(controler.InitGame));
            // parameterThread.Start(this.CreateGraphics());  
        }

        //初始化游戏界面
        private void SabBoy_Paint(object sender, PaintEventArgs e)
        {
            controler.InitGame(e.Graphics);
        }

        // 游戏驱动
        // 我怀疑这个函数在这里是非是合适的。
        public void timer_Tick(object sender, EventArgs e)
        {
            if (!controler.IsGameOver())
            {
                if(!timer_time.Enabled)
                    timer_time.Start();
                /*
                if (isKeyDown)
                {
                    controler.MoveBoard();
                }
                */
                controler.RunBall();        
                controler.Hit();

                controler.InitGame(this.CreateGraphics());
                txtScore.Text = "Score: " + controler.score.ToString();

                if (controler.IsSuccess())
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
    }
}
