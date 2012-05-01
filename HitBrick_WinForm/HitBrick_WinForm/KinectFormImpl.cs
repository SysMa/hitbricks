using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using System.Windows.Forms;
using Coding4Fun.Kinect.WinForm;
using System.Windows;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Drawing.Drawing2D;

namespace HitBrick_WinForm
{
    enum ERRORTYPE { CONSOLE_PRINT, MESSAGE_BOX };
    public partial class KinectForm : Form
    {

        Render render;
        Ball ball;

        private void KinectForm_Disposed(object sender, EventArgs e)
        {
            render.CloseSensor();
        }

        private void KinectForm_Load(object sender, EventArgs e)
        {
            KinectSensor sensor;
            render = new Render();
            ball = new Ball();
            //存在kinect体感设备，取第一个,否则退出
            if (KinectSensor.KinectSensors.Count != 0)
            {
                sensor = KinectSensor.KinectSensors[0];
            }
            else
            {
                MessageBox.Show("Kinect is not ready!");
                Error("Window_Loaded", "Kinect is not ready", ERRORTYPE.CONSOLE_PRINT);
                return;
            }

            if (sensor != null)
            {
                render.SetSensor(sensor);
                //初始化设备流
                render.InitStream();
                //开启体感设备
                render.StartSensor();
            }

            initPosition();

            render.BindComponent(ref manImage, ref manPanel);

            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true); 

            render.RunRender();

            //ball.BindComponent(ref ellBall);
            //ball.Run();
        }

        private void initPosition()
        {
            //Image img0 = new Image();
            //img0.Source = imgBar.Source;

            //Canvas.SetLeft(ellLeft, 100);
            //Canvas.SetTop(ellLeft, 400);

            //Canvas.SetLeft(ellRight, 200);
            //Canvas.SetTop(ellRight, 400);

            //Canvas.SetLeft(imgBar, 100);
            //Canvas.SetTop(imgBar, 300);

            //Canvas.SetLeft(border, 0);
        }

        private void Error(String OperationName, String ErrorInfo, ERRORTYPE ErrorType = ERRORTYPE.CONSOLE_PRINT)
        {
            switch (ErrorType)
            {
                case ERRORTYPE.CONSOLE_PRINT:
                    Console.WriteLine("Error:{1} existes in operation:{0}", OperationName, ErrorInfo);
                    break;
                case ERRORTYPE.MESSAGE_BOX:
                    MessageBox.Show("Error:" + ErrorInfo + " existes in operation " + OperationName);
                    break;
                default:
                    break;
            }
        }
  
    }

    /// <summary>
    /// 检测人物骨骼点的检测和呈现人物抠图
    /// </summary>
    class Render
    {
        KinectSensor sensor;
        DepthImageStream depthImageStream;
        private short[] depthPixelData;
        private byte[] colorPixelData;
        int depthImageStride;
        double manImageWidth = 0;
        double manImageHeight = 0;

        Bitmap depthImageBitmap;
        Rectangle depthImageBitmapRect;

        Graphics manImageGraphic;
        Rectangle barRect;
        SolidBrush barBrush;
        //注册图形
        PictureBox manImage;
        PictureBox imgBar;
        Panel manPanel;
        public void BindComponent(ref PictureBox manShape, ref Panel manPanel)
        {
            this.manImage = manShape;
            this.manPanel = manPanel;
            //this.imgBar = imgBar;
           
            this.manImage.Width = (int)(3 / 4.0 * this.manImage.Height);
            manImageHeight = this.manImage.Height;
            manImageWidth = this.manImage.Width;
            manImageGraphic = manShape.CreateGraphics();
            barBrush = new SolidBrush(Color.Red);
            manImageGraphic.FillRectangle(barBrush, barRect);
        }

        public void SetSensor(KinectSensor sensor)
        {
            this.sensor = sensor;
        }

        public void InitStream()
        {
            //开启colorstream
            ColorImageStream colorStream = sensor.ColorStream;
            if (colorStream == null)
            {
                MessageBox.Show("Kinect is not ready!!");
                Error("InitStream", "Kinect is not ready", ERRORTYPE.CONSOLE_PRINT);
                return;
            }
            //开启colorstream
            sensor.ColorStream.Enable(ColorImageFormat.RgbResolution1280x960Fps12);

            //开启skeletonstream
            //设置参数
            var parameters = new TransformSmoothParameters
            {
                Smoothing = 0.75f,
                Correction = 0.0f,
                Prediction = 0.0f,
                JitterRadius = 0.05f,
                MaxDeviationRadius = 0.04f
            };
            sensor.SkeletonStream.Enable(parameters);

            //开启depthstream
            depthImageStream = sensor.DepthStream;
            sensor.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);

            depthImageBitmap = new Bitmap(depthImageStream.FrameWidth, depthImageStream.FrameHeight);
            depthImageBitmapRect = new Rectangle(0, 0, (int)depthImageStream.FrameWidth, (int)depthImageStream.FrameHeight);
            depthImageStride = depthImageStream.FrameWidth * 4;           
            depthPixelData = new short[sensor.DepthStream.FramePixelDataLength];
            colorPixelData = new byte[sensor.ColorStream.FramePixelDataLength];            
        }

        public void StartSensor()
        {
            sensor.Start();
        }

        public void CloseSensor()
        {
            sensor.Stop();
            sensor.Dispose();
        }

        public void RunRender()
        {
            //添加骨骼事件处理
            sensor.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(sensor_SkeletonFrameReady);
            //添加深度事件处理
            sensor.DepthFrameReady += new EventHandler<DepthImageFrameReadyEventArgs>(sensor_DepthFrameReady);
        }

        private void sensor_DepthFrameReady(object sender, DepthImageFrameReadyEventArgs e)
        {
            manImageGraphic.FillRectangle(barBrush, barRect);
            using (DepthImageFrame dframe = e.OpenDepthImageFrame())
            {
                using (ColorImageFrame cframe = sensor.ColorStream.OpenNextFrame(2))
                {
                    if (dframe != null && cframe != null)
                    {
                        RenderScreen(dframe, cframe);
                        
                    }
                }
            }
        }

        private void sensor_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            manImageGraphic.FillRectangle(barBrush, barRect);
            SkeletonFrame skeletonFrame = e.OpenSkeletonFrame();

            if (skeletonFrame == null)
                return;
            Skeleton[] skeletonData = new Skeleton[skeletonFrame.SkeletonArrayLength];
            skeletonFrame.CopySkeletonDataTo(skeletonData);

            //get the first tracked skeleton
            Skeleton skeleton = (from s in skeletonData

                                 where s.TrackingState == SkeletonTrackingState.Tracked

                                 select s).FirstOrDefault();
            if (skeleton == null)
                return;

            SetBarPosition(skeleton.Joints[JointType.HandLeft], skeleton.Joints[JointType.HandRight], imgBar);
        }

        private void RenderScreen(DepthImageFrame depthFrame, ColorImageFrame colorFrame)
        {
            if (sensor != null && colorFrame != null && depthFrame != null)
            {
                int count = 0;          //所有捕获的点的个数
                double avgPosition = 0;  //深度平均中心（决定人物方框的位置）
                int depthPixelIndex;
                int playerIndex;
                int colorPixelIndex;
                int playerImageIndex = 0;
                int colorStride = colorFrame.BytesPerPixel * colorFrame.Width;//5120
                int bytesPerPixel = 4;
                byte[] playerImage = new byte[sensor.DepthStream.FrameHeight * depthImageStride];//1 228 800

                ColorImagePoint colorPoint;

                depthFrame.CopyPixelDataTo(depthPixelData);//307200
                colorFrame.CopyPixelDataTo(colorPixelData);//4 915 200

                for (int depthY = 0; depthY < depthFrame.Height; depthY++)
                {
                    for (int depthX = 0; depthX < depthFrame.Width; depthX++, playerImageIndex += bytesPerPixel)
                    {
                        depthPixelIndex = depthX + (depthY * depthFrame.Width);
                        playerIndex = depthPixelData[depthPixelIndex] & DepthImageFrame.PlayerIndexBitmask;
                        if (playerIndex != 0)
                        {
                            colorPoint = sensor.MapDepthToColorImagePoint(depthFrame.Format, depthX, depthY, depthPixelData[depthPixelIndex], colorFrame.Format);
                            colorPixelIndex = (colorPoint.X * colorFrame.BytesPerPixel) + (colorPoint.Y * colorStride);

                            if (colorPixelIndex < sensor.ColorStream.FramePixelDataLength)
                            {
                                avgPosition = avgPosition * count + depthX;
                                count++;
                                avgPosition /= count;

                                playerImage[playerImageIndex] = colorPixelData[colorPixelIndex];             //Blue    
                                playerImage[playerImageIndex + 1] = colorPixelData[colorPixelIndex + 1];     //Green
                                playerImage[playerImageIndex + 2] = colorPixelData[colorPixelIndex + 2];     //Red
                                playerImage[playerImageIndex + 3] = 0xFF;    //Alpha
                            }
                        }
                    }
                }
                BitmapData bmapdata = depthImageBitmap.LockBits(depthImageBitmapRect, ImageLockMode.WriteOnly, depthImageBitmap.PixelFormat);

                IntPtr ptr = bmapdata.Scan0;
                System.Runtime.InteropServices.Marshal.Copy(playerImage, 0, ptr, playerImage.Length);

                depthImageBitmap.UnlockBits(bmapdata);
                manImage.SizeMode = PictureBoxSizeMode.StretchImage;
                manImage.Image = depthImageBitmap;//depthImageBitmap;              

                avgPosition = (avgPosition) * (manImageWidth / depthFrame.Width - manImage.Height / depthFrame.Height);
                                
                manImage.Location = new Point((int)(avgPosition + manPanel.Location.X), 0);                
            }
        }

        private void SetBarPosition(Joint leftHand, Joint rightHand, PictureBox imgBar = null, Panel panel = null)
        {
            Point leftP = getDisplayPosition(leftHand);
            Point rightP = getDisplayPosition(rightHand);
            
            int w = (int)Math.Sqrt((rightP.Y - leftP.Y) * (rightP.Y - leftP.Y) + (rightP.X - leftP.X) * (rightP.X - leftP.X));

            if (rightP.X != leftP.X)
            {
                float angle = (float)Math.Atan(((double)(rightP.Y - leftP.Y)) / (rightP.X - leftP.X));
                angle = (float)(angle * 180 / Math.PI);
                barRect = new Rectangle(new Point(0,0),new Size(w, 10));
                manImageGraphic.ResetTransform();
                manImageGraphic.TranslateTransform(leftP.X, leftP.Y);
                manImageGraphic.RotateTransform(angle, MatrixOrder.Prepend);

                manImageGraphic.FillRectangle(barBrush, barRect);
            }
        }

        private Point getDisplayPosition(Joint joint)
        {
            ColorImagePoint colorPoint = sensor.MapSkeletonPointToColor(joint.Position,
                                        ColorImageFormat.RgbResolution640x480Fps30);
            // map back to skeleton.Width & skeleton.Height
            return new Point((int)((manImageWidth) * colorPoint.X / 640.0),
                (int)((manImageHeight) * colorPoint.Y / 480.0));
        }

        private void Error(String OperationName, String ErrorInfo, ERRORTYPE ErrorType = ERRORTYPE.CONSOLE_PRINT)
        {
            switch (ErrorType)
            {
                case ERRORTYPE.CONSOLE_PRINT:
                    Console.WriteLine("Error:{1} existes in operation:{0}", OperationName, ErrorInfo);
                    break;
                case ERRORTYPE.MESSAGE_BOX:
                    MessageBox.Show("Error:" + ErrorInfo + " exists in operation " + OperationName);
                    break;
                default:
                    break;
            }
        }

        public double GetBarWidth()
        {
            return barRect.Width;
        }

        public Point GetBarLocation()
        {
            return barRect.Location;
        }

        public Rectangle GetBarRect()
        {
            return barRect;
        }
    };
 
    /// <summary>
    /// A Class that contains some static utility functions. 
    /// </summary>
    class Utilities
    {
        /// <summary>
        /// Creates a new Image containing the same image only rotated
        /// </summary>
        /// <param name=""image"">The <see cref=""System.Drawing.Image"/"> to rotate
        /// <param name=""offset"">The position to rotate from.
        /// <param name=""angle"">The amount to rotate the image, clockwise, in degrees
        /// <returns>A new <see cref=""System.Drawing.Bitmap"/"> of the same size rotated.</see>
        /// <exception cref=""System.ArgumentNullException"">Thrown if <see cref=""image"/"> 
        /// is null.</see>
        public static Bitmap RotateImage(Image image, PointF offset, float angle)
        {
            if (image == null)
                throw new ArgumentNullException("image");

            //create a new empty bitmap to hold rotated image
            Bitmap rotatedBmp = new Bitmap(image.Width, image.Height);
            rotatedBmp.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            //make a graphics object from the empty bitmap
            Graphics g = Graphics.FromImage(rotatedBmp);

            //Put the rotation point in the center of the image
            g.TranslateTransform(offset.X, offset.Y);

            //rotate the image
            g.RotateTransform(angle);

            //move the image back
            g.TranslateTransform(-offset.X, -offset.Y);

            //draw passed in image onto graphics object
            g.DrawImage(image, new PointF(0, 0));

            return rotatedBmp;
        }
    };

    /// <summary>
    /// 弹球的结构
    /// </summary>
    class Ball
    {
        public bool upDown;
        public bool leftRight;
        public bool run;
        public int speed;
        public double toLeft;
        public double toTop;
        //....
        //bind component
        //Ellipse ellBall;
        //public void BindComponent(ref Ellipse ellBall)
        public void BindComponent()
        {
           // this.ellBall = ellBall;
        }

        public Ball()
        {
            upDown = true;
            leftRight = true;
            run = true;
            speed = 1;
            toLeft = 0;
            toTop = 0;
        }

        public void Run()
        {
            //while (true)
            //{
            //    toLeft += 1;
            //    toTop += 1;
            //    if (toLeft > 640)
            //        break;
            //    Display();
            //}
        }
        public void Display()
        {
            //Canvas.SetLeft(ellBall, toLeft);
            //Canvas.SetTop(ellBall, toTop);
        }


    };
}
