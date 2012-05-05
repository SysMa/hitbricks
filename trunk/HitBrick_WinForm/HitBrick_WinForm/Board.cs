using System.Drawing;

namespace HitBrick_WinForm
{
    /// <summary>
    /// 档板类
    /// </summary>

    //挡板方向
    public enum BoardDirection
    {
        Left, Right
    }

    public class Board : BaseEntity, ISabBoy
    {
        public BoardDirection Direction { get; set; }

        public Board(int x, int y, int speed)
        {
            this.XPos = x;
            this.YPos = y;
            this.SpeedX = speed;
        }

        public override void Draw(Graphics g)
        {
            using (SolidBrush sbrush = new SolidBrush(Color.LightBlue))
            {
                Pen p = new Pen(Color.SaddleBrown);
                Rect = new Rectangle(XPos, YPos, 70, 15);
                g.DrawRectangle(p, Rect);
                g.FillRectangle(sbrush, Rect);
            }
            g.Dispose();
        }

        #region ISabBoy 成员
        public void Run()
        {
            switch (Direction)
            {
                case BoardDirection.Left:
                    {
                        XPos -= SpeedX;
                    }
                    break;
                case BoardDirection.Right:
                    {
                        XPos += SpeedX;
                    }
                    break;
                default:
                    break;
            }
        }
        #endregion
    }
}
