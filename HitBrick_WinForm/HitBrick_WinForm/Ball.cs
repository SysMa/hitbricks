using System.Drawing;

namespace HitBrick_WinForm
{
    public class Ball : BaseEntity, ISabBoy
    {
        /// <summary>
        /// 初始化小球位置和偏移值
        /// </summary>
        public Ball(int x, int y, int speedX, int speedY)
        {
            this.XPos = x;
            this.YPos = y;
            this.SpeedX = speedX;
            this.SpeedY = speedY;
        }

        public void Draw(Graphics g)
        {
            using (TextureBrush sbrush = new TextureBrush(global::HitBrick_WinForm.Properties.Resources.xiaoqiu))
            {
                Rect = new Rectangle(XPos, YPos, 20, 20);
                g.DrawEllipse(new Pen(Color.Gray), Rect);
                g.FillEllipse(sbrush, Rect);
            }
            g.Dispose();
        }


        #region ISabBoy 成员
        //临界检测碰撞，利用随机数，提高可玩性
        public void Run()
        {
            XPos = XPos + SpeedX;
            YPos = YPos - SpeedY;
            /*
            if (XPos <= 0)
                SpeedX = (new Random().Next(3, 5));
            if (XPos > 378)
                SpeedX = -(new Random().Next(3, 5));
            if (YPos <= 100)
                SpeedY = -(new Random().Next(3, 8));
             */
        }
        #endregion
    }
}
