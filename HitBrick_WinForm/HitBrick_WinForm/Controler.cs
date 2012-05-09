using System.Drawing;

namespace HitBrick_WinForm
{
    class Controler
    {
        private System.Drawing.Bitmap bitmap;
        private Brick brick;
        private Ball ball;
        //游戏画面尺寸
        private int width = 0;
        private int height = 0;
        private bool isGameOver = false;
        public int score = 0;

        //构造函数，初始化对象
        public Controler(int w, int h)
        {
            this.width = w;
            this.height = h;
            bitmap = new Bitmap(width, height);
            brick = new Brick();
            ball = new Ball(/*width / 2 - 45, height - 300*/378, 78, 20, 30);
            brick.BrickWall();
        }

        //初始化画面
        public void InitGame(object g)
        {
            Graphics gra = (Graphics)g;
            //使用双缓冲，减少画面闪烁
            bitmap = new Bitmap(width, height);
            brick.Draw(Graphics.FromImage(bitmap)); //画砖墙
            ball.Draw(Graphics.FromImage(bitmap)); //画小球
            gra.DrawImage(bitmap, 0, 0);
            gra.Dispose();
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
        /*
        public Rectangle Board
        {
            get { return board.Rect; }
            set { board.Rect = value; }
        }

        public BoardDirection Direction
        {
            get { return board.Direction; }
            set { board.Direction = value; }
        }
        */
    }
}
