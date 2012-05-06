using System.Collections.Generic;
using System.Drawing;

namespace HitBrick_WinForm
{
    public class Brick : BaseEntity
    {
        private int _width = 400; //砖块集宽
        private int _height = 300;//砖块集高
        //砖块集
        public List<Rectangle> Rects { get; set; }
        //构造砖墙形状
        public List<Rectangle> BrickWall()
        {
            int temp = 0;
            Rects = new List<Rectangle>();
            for (int i = 100; i < _height; i += 20)
            {
                temp += 20;
                for (int j = temp - 20; j < _width - temp; j += 30)
                {
                    Rect = new Rectangle(j, i, 28, 18);
                    Rects.Add(Rect);
                }
            }
            return Rects;
        }
        //画墙
        public override void Draw(Graphics g)
        {
            using (SolidBrush sbrush = new SolidBrush(Color.Orange))
            {
                Pen p = new Pen(Color.SaddleBrown, 3);
                foreach (Rectangle r in Rects)
                {
                    g.DrawRectangle(p, r);
                    g.FillRectangle(sbrush, r);
                }
            }
            g.Dispose();
        }
    }
}
