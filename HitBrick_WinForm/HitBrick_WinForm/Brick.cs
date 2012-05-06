﻿using System.Collections.Generic;
using System.Drawing;

namespace HitBrick_WinForm
{
    public class Brick : BaseEntity
    {
        private int _width = 400; //砖块集宽
        private int _height = 300;//砖块集高

        //砖块
        public struct Brick_Type
        {
            public Rectangle r;
            public int i;
        }

        //砖块集
        public List<Brick_Type> Rects { get; set; }
        //构造砖墙形状
        public List<Brick_Type> BrickWall()
        {
            int temp = 0;
            Rects = new List<Brick_Type>();
            for (int i = 100; i < _height; i += 20)
            {
                temp += 20;
                for (int j = temp - 20; j < _width - temp; j += 30)
                {
                    Rect = new Rectangle(j, i, 28, 18);
                    Brick_Type temp_brick = new Brick_Type();
                    temp_brick.r = Rect;
                    temp_brick.i = 0;
                    Rects.Add(temp_brick);
                }
            }
            return Rects;
        }

        //画墙
        public override void Draw(Graphics g)
        {
            foreach (Brick_Type b in Rects)
            {
                Image img;
                switch (b.i)
                {
                    case 0:
                    default:
                        img = Image.FromFile(@"C:\Users\MSQ\Pictures\62ddd102jw1dqa0cs909lj.jpg");
                        break;
                }
                g.DrawImage(img, b.r);
            }
            g.Dispose();
        }
    }
}
