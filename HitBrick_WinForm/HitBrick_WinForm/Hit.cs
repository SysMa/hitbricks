using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HitBrick_WinForm
{
    public partial class KinectForm : Form
    {
        //碰撞检测
        public void Hit()
        {
            // 这部分和下面一部分最好不要合并。
            // 意外是：子弹和小球同时击中。
            bool bullet_hit = false;
            for (int i = 0; i < Rects.Count;)
            {
                if (bullets.Count == 0)
                {
                    break;
                }
                for (int k = 0; k < bullets.Count;)
                {
                    if (bullets[k].rect.IntersectsWith(Rects[i].rectangle))
                    {
                        Rects[i].pictureBox.Visible = false;
                        Rects[i].pictureBox.Dispose();
                        Rects.Remove(Rects[i]);
                        bullet_hit = true;

                        bullets[k].pic.Visible = false;
                        bullets[k].pic.Dispose();
                        bullets.Remove(bullets[k]);

                        Mp3 mp3 = new Mp3();
                        mp3.FileName = @"..\..\Resources\hitBricks.wav";
                        mp3.play();
                        break;
                    }
                    else
                    {
                        k++;
                    }
                }
                if (bullet_hit)
                {
                    bullet_hit = false;
                }
                else
                {
                    i++;
                }
            }

            for (int j = 0; j < balls.Count; j++)
            {
                //砖块与小球碰撞
                for (int i = 0; i < Rects.Count; i++)
                {                    
                    if (balls[j].speedY < 0)
                    {
                        balls[j].speedY = -3;
                    }
                    // 下面4个变量用来记录相交与否
                    // 如果某一条边和小球的矩形相交，则为true
                    // 否则为false
                    bool[] flags = new bool[4];
                    bool hit = false;

                    // 相交与否
                    flags[0] = Rects[i].rectangle.Contains(
                        new Point(balls[j].rect.X, balls[j].rect.Y));
                    flags[1] = Rects[i].rectangle.Contains(
                        new Point(balls[j].rect.X + 2 * ball_R, balls[j].rect.Y));
                    flags[2] = Rects[i].rectangle.Contains(
                        new Point(balls[j].rect.X, balls[j].rect.Y + ball_R * 2));
                    flags[3] = Rects[i].rectangle.Contains(
                        new Point(balls[j].rect.X + ball_R * 2, balls[j].rect.Y + ball_R * 2));

                    if (flags[0] && flags[1])
                    {
                        if (!isHeavy)
                        {
                            balls[j].speedY = -balls[j].speedY;
                        }
                        hit = true;
                    }
                    else if (flags[0] && !flags[1] && !flags[2])
                    {
                        hit = true;
                        if (!isHeavy)
                        {
                            if (balls[j].speedX > 0 && balls[j].speedY > 0)
                            {
                                balls[j].speedY = -balls[j].speedY;
                            }
                            else if (balls[j].speedX < 0)
                            {
                                balls[j].speedX = -balls[j].speedX;
                            }
                            else if (balls[j].speedX > 0 && balls[j].speedY < 0)
                            {
                                // impossible
                                return;
                            }
                        }
                    }
                    else if (flags[0] && flags[2])
                    {
                        hit = true;
                        if (!isHeavy)
                        {
                            balls[j].speedX = -balls[j].speedX;
                        }
                    }
                    else if (flags[2] && !flags[0] && !flags[3])
                    {
                        hit = true;
                        if (!isHeavy)
                        {
                            if (balls[j].speedX > 0 && balls[j].speedY < 0)
                            {
                                balls[j].speedY = -balls[j].speedY;
                            }
                            else if (balls[j].speedX < 0)
                            {
                                balls[j].speedX = -balls[j].speedX;
                            }
                            else if (balls[j].speedX > 0 && balls[j].speedY > 0)
                            {
                                // impossible
                                return;
                            }
                        }
                    }
                    else if (flags[2] && flags[3])
                    {
                        hit = true;
                        if (!isHeavy)
                        {
                            balls[j].speedY = -balls[j].speedY;
                        }
                    }
                    else if (flags[3] && !flags[1] && !flags[2])
                    {
                        hit = true;
                        if (!isHeavy)
                        {
                            if (balls[j].speedX < 0 && balls[j].speedY < 0)
                            {
                                balls[j].speedY = -balls[j].speedY;
                            }
                            else if (balls[j].speedX > 0)
                            {
                                balls[j].speedX = -balls[j].speedX;
                            }
                            else if (balls[j].speedX < 0 && balls[j].speedY > 0)
                            {
                                // impossible
                                return;
                            }
                        }
                    }
                    else if (flags[1] && flags[3])
                    {
                        hit = true;
                        if (!isHeavy)
                        {
                            balls[j].speedX = -balls[j].speedX;
                        }
                    }
                    else if (flags[1] && !flags[0] && !flags[3])
                    {
                        hit = true;
                        if (!isHeavy)
                        {
                            if (balls[j].speedX < 0 && balls[j].speedY > 0)
                            {
                                balls[j].speedY = -balls[j].speedY;
                            }
                            else if (balls[j].speedX > 0)
                            {
                                balls[j].speedX = -balls[j].speedX;
                            }
                            else if (balls[j].speedX < 0 && balls[j].speedY < 0)
                            {
                                // impossible
                                return;
                            }
                        }
                    }

                    if (hit)
                    {
                        //得分
                        score += (Rects[i].type + 1) * 10;
                        txtScore.Text = score.ToString();

                        if (i % 5 == 0)
                        {
                            Bonus bonus = new Bonus();
                            bonus.xPos = Rects[i].rectangle.X;
                            bonus.yPos = Rects[i].rectangle.Y;
                            bonus.rect = new Rectangle(bonus.xPos, bonus.yPos,
                                2 * ball_R, 2 * ball_R);

                            bonus.pic = new PictureBox();
                            bonus.pic.BackColor = Color.Transparent;
                            bonus.pic.Location = new Point(bonus.xPos, bonus.yPos);
                            bonus.pic.Size = new Size(2 * ball_R, 2 * ball_R);
                            this.splitContainer1.Panel1.Controls.Add(bonus.pic);
                            bonus.pic.BringToFront();

                            switch (random.Next(0, (int)(Bonus_Type.COUNT)))
                            {
                                case (int)Bonus_Type.INCREASE:
                                    bonus.type = Bonus_Type.INCREASE;
                                    bonus.pic.Image = global::HitBrick_WinForm.Properties.Resources.increaseLength;
                                    break;
                                case (int)Bonus_Type.DECREASE:
                                    bonus.type = Bonus_Type.DECREASE;
                                    bonus.pic.Image = global::HitBrick_WinForm.Properties.Resources.decreaseLength;
                                    break;
                                case (int)Bonus_Type.ADD_LIFE:
                                    bonus.type = Bonus_Type.ADD_LIFE;
                                    bonus.pic.Image = global::HitBrick_WinForm.Properties.Resources.life_small;
                                    break;
                                case (int)Bonus_Type.BOMB:
                                    bonus.type = Bonus_Type.BOMB;
                                    bonus.pic.Image = global::HitBrick_WinForm.Properties.Resources.bomb;
                                    break;
                                case (int)Bonus_Type.THREE_BALLS:
                                    bonus.type = Bonus_Type.THREE_BALLS;
                                    bonus.pic.Image = global::HitBrick_WinForm.Properties.Resources.ball;
                                    break;
                                case (int)Bonus_Type.HEAVY_BALL:
                                    bonus.type = Bonus_Type.HEAVY_BALL;
                                    bonus.pic.Image = global::HitBrick_WinForm.Properties.Resources.heavyBall;
                                    break;
                                case (int)Bonus_Type.BULLET:
                                default:
                                    bonus.type = Bonus_Type.BULLET;
                                    bonus.pic.Image = global::HitBrick_WinForm.Properties.Resources.lighting;
                                    break;
                            }

                            bonuses.Add(bonus);
                        }

                        //删除砖块
                        Rects[i].pictureBox.Visible = false;
                        Rects[i].pictureBox.Dispose();
                        Rects.Remove(Rects[i]);
                        Mp3 mp3 = new Mp3();
                        mp3.FileName = @"..\..\Resources\hitBricks.wav";
                        mp3.play();
                        break;
                    }
                }
            }

            //获得奖励
            for (int i = 0; i < bonuses.Count; )
            {
                Bonus bonus = bonuses[i];
                if (bonus.pic.RectangleToScreen(bonus.pic.DisplayRectangle)
                    .IntersectsWith(render.manImage.RectangleToScreen(render.manImage.DisplayRectangle)))
                {
                    //奖励效果
                    switch (bonus.type)
                    {
                        case Bonus_Type.INCREASE:
                            render.type = Render.BarWidth.DOUBLE;
                            break;
                        case Bonus_Type.DECREASE:
                            render.type = Render.BarWidth.HALF;
                            break;
                        case Bonus_Type.ADD_LIFE:
                            if (remainLife < 3)
                            {
                                lifePics[remainLife].Visible = true;
                                remainLife++;
                            }
                            break;
                        case Bonus_Type.BOMB:
                            for (int num_bonus = 0; num_bonus < bonus_bricks && num_bonus < Rects.Count; num_bonus++)
                            {
                                int pick = random.Next(0, Rects.Count - 1);
                                // delete the brick
                                Rects[pick].pictureBox.Visible = false;
                                Rects[pick].pictureBox.Dispose();
                                Rects.Remove(Rects[pick]);
                            }
                            break;
                        case Bonus_Type.THREE_BALLS:
                            {
                                Ball ball1 = new Ball(10, 50);
                                this.splitContainer1.Panel1.Controls.Add(ball1.pic);
                                ball1.pic.BringToFront();
                                balls.Add(ball1);
                                Ball ball2 = new Ball(this.splitContainer1.Panel1.Width - 30, 50);
                                this.splitContainer1.Panel1.Controls.Add(ball2.pic);
                                ball2.pic.BringToFront();
                                balls.Add(ball2);
                            }
                            break;
                        case Bonus_Type.HEAVY_BALL:
                            {
                                isHeavy = true;
                                for (int j = 0; j < balls.Count; j++)
                                {
                                    balls[j].pic.Image = global::HitBrick_WinForm.Properties.Resources.heavyBall;
                                }
                            }
                            break;
                        case Bonus_Type.BULLET:
                        default:
                            {
                                //render.setBegin(0);
                                numberOfBullets = 10;
                                break;
                            }
                    }

                    bonus.pic.Dispose();
                    bonuses.Remove(bonus);
                }
                //奖励消失
                else if (bonus.yPos > this.Height)
                {
                    bonus.pic.Dispose();
                    bonuses.Remove(bonus);
                }
                else
                {
                    bonus.yPos = bonus.yPos + bonus_speed;
                    bonus.pic.Location = new Point(bonus.xPos, bonus.yPos);
                    bonus.rect = new Rectangle(bonus.xPos, bonus.yPos, 2 * ball_R, 2 * ball_R);
                    i++;
                }
            }

            Point center = render.GetBarLocation();

            for (int i = 0; i < balls.Count; i++)
            {
                //小球与挡板碰撞
                if (balls[i].pic.RectangleToScreen(balls[i].pic.DisplayRectangle).IntersectsWith(
                    render.barImage.RectangleToScreen(render.GetBarRect()))
                    &&
                    balls[i].pic.RectangleToScreen(balls[i].pic.DisplayRectangle).IntersectsWith(
                    render.manImage.RectangleToScreen(render.manImage.DisplayRectangle)))
                {
                    int dis_x = center.X - lastCenter.X;
                    int dis_y = center.Y - lastCenter.Y;
                    double angle = render.Angle;

                    if (angle < 0)
                    {
                        angle = angle + Math.PI;
                    }

                    if (angle > Math.PI / 2)
                    {
                        angle = Math.PI - angle;
                    }

                    // so now the v is this 
                    // along the board: x*cosα-y*sinα
                    // and x*sinα+y*cosα
                    // rules: along the board, no changes, the other, direction and speedup.
                    if ((dis_x > 0 && balls[i].speedX > 0 && balls[i].speedX > dis_x) ||
                        (dis_x < 0 && balls[i].speedX < 0 && dis_x > balls[i].speedX))
                    {
                        balls[i].speedX = dis_x * 1;
                        balls[i].speedX = dis_y * 1;
                    }
                    else
                    {
                        balls[i].speedX = (int)((balls[i].speedX * Math.Cos(2 * angle)
                            - balls[i].speedY * Math.Sin(2 * angle)) + dis_x * speedup_x);
                        balls[i].speedY = (int)((balls[i].speedX * Math.Sin(2 * angle)
                            + balls[i].speedY * Math.Cos(2 * angle)) + dis_y * speedup_y);

                        if (balls[i].speedY == 0)
                            balls[i].speedY = 5;
                    }
                }
            }

            lastCenter = center;
        }
    }
}
