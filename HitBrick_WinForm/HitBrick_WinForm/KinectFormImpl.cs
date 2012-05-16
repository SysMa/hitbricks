using System;
using System.Windows.Forms;
using Microsoft.Kinect;

namespace HitBrick_WinForm
{
    enum ERRORTYPE { CONSOLE_PRINT, MESSAGE_BOX };
    public partial class KinectForm : Form
    {
        Render render;

        private void KinectForm_Disposed(object sender, EventArgs e)
        {
            render.CloseSensor();
        }

        private void KinectForm_Load(object sender, EventArgs e)
        {
            //KinectSensor sensor;
            //render = new Render();
            //////存在kinect体感设备，取第一个,否则退出
            //if (KinectSensor.KinectSensors.Count != 0)
            //{
            //    sensor = KinectSensor.KinectSensors[0];
            //}
            //else
            //{
            //    MessageBox.Show("Kinect is not ready!");
            //    Error("Window_Loaded", "Kinect is not ready", ERRORTYPE.CONSOLE_PRINT);
            //    return;
            //}

            //if (sensor != null)
            //{
            //    render.SetSensor(sensor);
            //    //初始化设备流
            //    render.InitStream();
            //    //开启体感设备
            //    render.StartSensor();
            //}
            initPosition();

            //render.BindComponent(ref manImage, splitContainer1.Panel1);

            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            // this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();
            

            //render.RunRender();
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
}
