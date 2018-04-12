using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Joystick_code;

namespace WpfActiveDefenceSystem
{
    /// <summary>
    /// PageControlInput.xaml 的交互逻辑
    /// </summary>
    /// 
    public struct Axis
    {
        public double Pitch;
        public double Roll;
        public double Yaw;
        public double Throttle;
    };

    public partial class PageControlInput : Page
    {
        private MainWindow mainWD;
        private Joystick joystick_PR;
        private Joystick joystick_T;
        private Joystick joystick_Y;

        private Joystick joystick_PR2;
        private Joystick joystick_T2;
        private Joystick joystick_Y2;

        private JoystickButtons PreviousButtons = JoystickButtons.None;
        private Axis PreviousAxis;

        private JoystickButtons PreviousButtons2 = JoystickButtons.None;
        private Axis PreviousAxis2;

        private System.Threading.Timer CaptureTimer;
        private System.Threading.Timer CaptureTimer2;
        private delegate void ButtonsUpCallBack(JoystickButtons Buttons);
        private delegate void ButtonsDownCallBack(JoystickButtons Buttons);
        private delegate void AxisChangeCallBack(Axis Axis);

        public PageControlInput()
        {
            InitializeComponent();
            // 下面的定义方式适用于最新的 ThrustMaster A10c疣猪游戏手柄+脚舵
            // 如果接两套摇杆，第一套是0,1,2
            //joystick_Y = new Joystick(0);
            //joystick_T = new Joystick(1);
            //joystick_PR = new Joystick(2);
            joystick_Y = new Joystick(5);
            joystick_T = new Joystick(4);               // 油门正确
            joystick_PR = new Joystick(3);              // 

            joystick_Y2 = new Joystick(0);
            joystick_T2 = new Joystick(1);               // 油门正确
            joystick_PR2 = new Joystick(2);              // 
            // 下面的定义方式适用于一般的飞行手柄（三轴集成+油门）
            this.CaptureTimer = new System.Threading.Timer(this.OnTimerCallback, null, 0, 50);
            this.CaptureTimer2 = new System.Threading.Timer(this.OnTimerCallback2, null, 0, 50);
        }

        public void SetPage(MainWindow mainWindow)
        {
            this.mainWD = mainWindow;
        }

        private void buttonDown(JoystickButtons Buttons)
        {
            tbuttonFire.IsChecked = true;
            //this.label_Fire.BorderStyle = ((Buttons & JoystickButtons.B1) == JoystickButtons.B1) ? BorderStyle.Fixed3D : this.Fire.BorderStyle;
        }
        private void buttonUp(JoystickButtons Buttons)
        {
            tbuttonFire.IsChecked = false;
            //this.Fire.BorderStyle = ((Buttons & JoystickButtons.B1) == JoystickButtons.B1) ? BorderStyle.None : this.Fire.BorderStyle;
        }

        private void buttonDown2(JoystickButtons Buttons)
        {
            tbuttonFire2.IsChecked = true;
            //this.label_Fire.BorderStyle = ((Buttons & JoystickButtons.B1) == JoystickButtons.B1) ? BorderStyle.Fixed3D : this.Fire.BorderStyle;
        }
        private void buttonUp2(JoystickButtons Buttons)
        {
            tbuttonFire2.IsChecked = false;
            //this.Fire.BorderStyle = ((Buttons & JoystickButtons.B1) == JoystickButtons.B1) ? BorderStyle.None : this.Fire.BorderStyle;
        }

        private void axisChange(Axis axis)
        {
            double temp;
            this.ProgressBar_Pitch.Value = Convert.ToInt32(axis.Pitch);
            temp = (axis.Pitch - 50) * 9 / 5;
            this.label_PData.Content = temp.ToString("f1") + "°";
            this.ProgressBar_Roll.Value = Convert.ToInt32(axis.Roll);
            temp = (axis.Roll - 50) * 9 / 5;
            this.label_RData.Content = temp.ToString("f1") + "°";
            this.ProgressBar_Yaw.Value = Convert.ToInt32(axis.Yaw);
            temp = (axis.Yaw - 50) * 9 / 5;
            this.label_YData.Content = temp.ToString("f1") + "°";
            this.ProgressBar_Throttle.Value = Convert.ToInt32(axis.Throttle);
            this.label_TData.Content = axis.Throttle.ToString("f1") + "%";
            //if (this.tbuttonFire.IsEnabled)
            //{
            //    this.label_TData.Content = axis.Throttle.ToString("1") + "%";
            //}
            
        }
        private void axisChange2(Axis axis)
        {
            double temp;
            this.ProgressBar_Pitch2.Value = Convert.ToInt32(axis.Pitch);
            temp = (axis.Pitch - 50) * 9 / 5;
            this.label_PData2.Content = temp.ToString("f1") + "°";
            this.ProgressBar_Roll2.Value = Convert.ToInt32(axis.Roll);
            temp = (axis.Roll - 50) * 9 / 5;
            this.label_RData2.Content = temp.ToString("f1") + "°";
            this.ProgressBar_Yaw2.Value = Convert.ToInt32(axis.Yaw);
            temp = (axis.Yaw - 50) * 9 / 5;
            this.label_YData2.Content = temp.ToString("f1") + "°";
            this.ProgressBar_Throttle2.Value = Convert.ToInt32(axis.Throttle);
            this.label_TData2.Content = axis.Throttle.ToString("f1") + "%";
            //if (this.tbuttonFire.IsEnabled)
            //{
            //    this.label_TData.Content = axis.Throttle.ToString("1") + "%";
            //}

        }
        private void OnTimerCallback(object state)
        {
            JoystickAPI.JOYINFOEX infoEx_PR = new JoystickAPI.JOYINFOEX();
            JoystickAPI.JOYINFOEX infoEx_Y = new JoystickAPI.JOYINFOEX();
            JoystickAPI.JOYINFOEX infoEx_T = new JoystickAPI.JOYINFOEX();
            Axis axis_now = PreviousAxis;
            if (joystick_PR.getinfo(ref infoEx_PR) == JoystickAPI.JOYERR_NOERROR)
            {
                JoystickButtons buttons = joystick_PR.GetButtons(infoEx_PR.dwButtons);
                JoystickAxis axis = joystick_PR.GetAxis(ref infoEx_PR);

                if (PreviousButtons != buttons)
                {
                    //按钮状态有更改,则发出ButtonUp事件

                    //取得前一次按下的按钮与本次按下的按钮的差(即不再处于按下状态的按钮)
                    JoystickButtons b = (JoystickButtons)(PreviousButtons - (PreviousButtons & buttons));
                    JoystickButtons n = (JoystickButtons)(buttons - (PreviousButtons & buttons));//新按下的按钮;
                    if (b != JoystickButtons.None)
                    {
                        this.Dispatcher.Invoke(new ButtonsUpCallBack(buttonUp), b);
                    }
                    if (n != JoystickButtons.None)
                    {
                        this.Dispatcher.Invoke(new ButtonsDownCallBack(buttonDown), n);
                    }
                    PreviousButtons = buttons;
                }
                axis_now.Pitch = Convert.ToDouble(axis.Ypos - joystick_PR.JoystickCAPS.wYmin) * 100 / (joystick_PR.JoystickCAPS.wYmax - joystick_PR.JoystickCAPS.wYmin);
                axis_now.Roll = Convert.ToDouble(axis.Xpos - joystick_PR.JoystickCAPS.wXmin) * 100 / (joystick_PR.JoystickCAPS.wXmax - joystick_PR.JoystickCAPS.wXmin);
            }
            if (joystick_Y.getinfo(ref infoEx_Y) == JoystickAPI.JOYERR_NOERROR)
            {
                JoystickAxis axis = joystick_Y.GetAxis(ref infoEx_Y);
                axis_now.Yaw = Convert.ToDouble(axis.Zpos - joystick_Y.JoystickCAPS.wRmin) * 100 / (joystick_Y.JoystickCAPS.wRmax - joystick_Y.JoystickCAPS.wRmin);
            }
            if (joystick_T.getinfo(ref infoEx_T) == JoystickAPI.JOYERR_NOERROR)
            {
                JoystickAxis axis = joystick_T.GetAxis(ref infoEx_T);
                //axis_now.Throttle = 100 - Convert.ToDouble(axis.Rpos - joystick_T.JoystickCAPS.wRmin) * 100 / (joystick_T.JoystickCAPS.wRmax - joystick_T.JoystickCAPS.wRmin);
                axis_now.Throttle = 100 - Convert.ToDouble(axis.Zpos - joystick_T.JoystickCAPS.wZmin) * 100 / (joystick_T.JoystickCAPS.wZmax - joystick_T.JoystickCAPS.wZmin);
            }
            if (axis_now.Equals(PreviousAxis))
            {

            }
            else
            {
                this.Dispatcher.Invoke(new AxisChangeCallBack(axisChange), axis_now);
                PreviousAxis = axis_now;
            }
        }

        private void OnTimerCallback2(object state)
        {
            JoystickAPI.JOYINFOEX infoEx_PR = new JoystickAPI.JOYINFOEX();
            JoystickAPI.JOYINFOEX infoEx_Y = new JoystickAPI.JOYINFOEX();
            JoystickAPI.JOYINFOEX infoEx_T = new JoystickAPI.JOYINFOEX();
            Axis axis_now = PreviousAxis2;
            if (joystick_PR2.getinfo(ref infoEx_PR) == JoystickAPI.JOYERR_NOERROR)
            {
                JoystickButtons buttons = joystick_PR2.GetButtons(infoEx_PR.dwButtons);
                JoystickAxis axis = joystick_PR2.GetAxis(ref infoEx_PR);

                if (PreviousButtons2 != buttons)
                {
                    //按钮状态有更改,则发出ButtonUp事件

                    //取得前一次按下的按钮与本次按下的按钮的差(即不再处于按下状态的按钮)
                    JoystickButtons b = (JoystickButtons)(PreviousButtons2 - (PreviousButtons2 & buttons));
                    JoystickButtons n = (JoystickButtons)(buttons - (PreviousButtons2 & buttons));//新按下的按钮;
                    if (b != JoystickButtons.None)
                    {
                        this.Dispatcher.Invoke(new ButtonsUpCallBack(buttonUp2), b);
                    }
                    if (n != JoystickButtons.None)
                    {
                        this.Dispatcher.Invoke(new ButtonsDownCallBack(buttonDown2), n);
                    }
                    PreviousButtons2 = buttons;
                }
                axis_now.Pitch = Convert.ToDouble(axis.Ypos - joystick_PR2.JoystickCAPS.wYmin) * 100 / (joystick_PR2.JoystickCAPS.wYmax - joystick_PR2.JoystickCAPS.wYmin);
                axis_now.Roll = Convert.ToDouble(axis.Xpos - joystick_PR2.JoystickCAPS.wXmin) * 100 / (joystick_PR2.JoystickCAPS.wXmax - joystick_PR2.JoystickCAPS.wXmin);
            }
            if (joystick_Y2.getinfo(ref infoEx_Y) == JoystickAPI.JOYERR_NOERROR)
            {
                JoystickAxis axis = joystick_Y2.GetAxis(ref infoEx_Y);
                axis_now.Yaw = Convert.ToDouble(axis.Zpos - joystick_Y2.JoystickCAPS.wRmin) * 100 / (joystick_Y2.JoystickCAPS.wRmax - joystick_Y2.JoystickCAPS.wRmin);
            }
            if (joystick_T2.getinfo(ref infoEx_T) == JoystickAPI.JOYERR_NOERROR)
            {
                JoystickAxis axis = joystick_T2.GetAxis(ref infoEx_T);
                //axis_now.Throttle = 100 - Convert.ToDouble(axis.Rpos - joystick_T.JoystickCAPS.wRmin) * 100 / (joystick_T.JoystickCAPS.wRmax - joystick_T.JoystickCAPS.wRmin);
                axis_now.Throttle = 100 - Convert.ToDouble(axis.Zpos - joystick_T2.JoystickCAPS.wZmin) * 100 / (joystick_T2.JoystickCAPS.wZmax - joystick_T2.JoystickCAPS.wZmin);
            }
            if (axis_now.Equals(PreviousAxis2))
            {

            }
            else
            {
                this.Dispatcher.Invoke(new AxisChangeCallBack(axisChange2), axis_now);
                PreviousAxis2 = axis_now;
            }
        }

        private void tbuttonFire_Click(object sender, RoutedEventArgs e)
        {
            this.label_Throttle.Content = "1";
        }
        private void tbuttonFire_Click2(object sender, RoutedEventArgs e)
        {
            this.label_Throttle2.Content = "1";
        }
    }
}
