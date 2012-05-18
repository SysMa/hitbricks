using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Kinect;

namespace HitBrick_WinForm
{
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

        const int DEFAULTWIDTH = 60;
        enum BarWidth {NORMAL,HALF,DOUBLE,AUTO };
        BarWidth type;
        private float angle;

        Bitmap depthImageBitmap;
        Rectangle depthImageBitmapRect;

        Bitmap barImageBitmap;

        private Point rightHand;
        private Point leftHand;
        private Point head;

        Graphics barImageGraphic;
        Rectangle barRect;
        SolidBrush barBrush;

        //注册图形
        PictureBox manImage;
        PictureBox barImage;
        SplitterPanel manPanel;
        public void BindComponent(ref PictureBox manShape, SplitterPanel manPanel)
        {
            this.manImage = manShape;
            this.manPanel = manPanel;

            //set manImage size
            this.manImage.Width = (int)(3 / 4.0 * this.manImage.Height);
            manImageHeight = this.manImage.Height;
            manImageWidth = this.manImage.Width;
            this.manImage.SizeMode = PictureBoxSizeMode.StretchImage;

            //copy the manImage to barImage
            this.barImage = new PictureBox();
            
            this.barImage.Parent = this.manImage;
            this.barImage.Location = new Point(0,0);
            this.barImage.Size = this.manImage.Size;
            this.barImage.BackColor = Color.Transparent;            
            this.barImage.SizeMode = PictureBoxSizeMode.StretchImage;
           
            barBrush = new SolidBrush(Color.Red);

            barImageBitmap = new Bitmap(barImage.Width, barImage.Height);
            barImage.Image = barImageBitmap;
            barImageGraphic = Graphics.FromImage(barImage.Image);

            type = BarWidth.NORMAL;
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
            //manImageGraphic.FillRectangle(barBrush, barRect);
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
            //manImageGraphic.FillRectangle(barBrush, barRect);
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

            rightHand = getDisplayPosition(skeleton.Joints[JointType.HandRight]);
            leftHand = getDisplayPosition(skeleton.Joints[JointType.HandLeft]);
            head = getDisplayPosition(skeleton.Joints[JointType.Head]);

            SetBarPosition(skeleton.Joints[JointType.HandLeft], skeleton.Joints[JointType.HandRight], barImage);
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
                manImage.Image = depthImageBitmap;//depthImageBitmap;              

                //map to panel
                avgPosition = (avgPosition) * (manImageWidth / depthFrame.Width);// - manImage.Height / depthFrame.Height);
                avgPosition = avgPosition * ((double)manPanel.Width / manImage.Width) - manImage.Width / 2.0;
                manImage.Location = new Point((int)(avgPosition + manPanel.Location.X), manPanel.Height - manImage.Height);
            }
        }

        private void SetBarPosition(Joint leftHand, Joint rightHand, PictureBox imgBar = null, Panel panel = null)
        {
            Point leftP = getDisplayPosition(leftHand);
            Point rightP = getDisplayPosition(rightHand);

            int w;
            switch(type){
                case BarWidth.NORMAL:
                    w = DEFAULTWIDTH;
                    break;
                case BarWidth.HALF:
                    w = DEFAULTWIDTH/2;
                    break;
                case BarWidth.DOUBLE:
                    w = DEFAULTWIDTH*2;
                    break;
                case BarWidth.AUTO:
                    w = (int)Math.Sqrt((rightP.Y - leftP.Y) * (rightP.Y - leftP.Y) + (rightP.X - leftP.X) * (rightP.X - leftP.X));
                    break;
                default:
                    w = DEFAULTWIDTH;
                    break;
            }

            if (rightP.X != leftP.X)
            {
                angle = (float)Math.Atan(((double)(rightP.Y - leftP.Y)) / (rightP.X - leftP.X));
                angle = (float)(angle * 180 / Math.PI);

                barRect = new Rectangle(new Point(0, 0), new Size(w, 10));
                barImageGraphic.Clear(Color.Transparent);
                barImageGraphic.ResetTransform();
                if (leftP.X > rightP.X)
                {
                    barImageGraphic.TranslateTransform(rightP.X, rightP.Y);
                }
                else
                {
                    barImageGraphic.TranslateTransform(leftP.X, leftP.Y);
                }
                
                barImageGraphic.RotateTransform(angle, MatrixOrder.Prepend);                
                barImageGraphic.FillRectangle(barBrush, barRect);
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

        public Point RightHand
        {
            get { return rightHand; }
            set { rightHand = value; }
        }
        public Point LeftHand
        {
            get { return leftHand; }
            set { leftHand = value; }
        }
        public Point Head
        {
            get { return head; }
            set { head = value; }
        }
        public SolidBrush BarBrush
        {
            get { return barBrush; }
            set { barBrush = value; }
        }
        public float Angle
        {
            get { return angle; }
            set { angle = value; }
        }
    }
}
