/* ***********************************************
 * Author		:  kingthy
 * Email		:  kingthy@gmail.com
 * DateTime		:  2009-3-27 22:43:36
 * Description	:  游戏手柄的封装类
 *
 * ***********************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;

namespace Joystick_code
{
    /// <summary>
    /// 游戏手柄的按钮定义
    /// </summary>
    [Flags]
    public enum JoystickButtons
    {
        //没有任何按钮
        None = 0x0,
        B1 = 0x1,
        B2 = 0x2,
        B3 = 0x4,
        B4 = 0x8,
        B5 = 0x10,
        B6 = 0x20,
        B7 = 0x40,
        B8 = 0x80,
        B9 = 0x100,
        B10 = 0x200
    };
    public struct JoystickAxis
    {
        public int Xpos;
        public int Ypos;
        public int Zpos;
        public int Rpos;
        public int Upos;
        public int Vpos;
    };

    /// <summary>
    /// 游戏手柄类
    /// </summary>
    public class Joystick 
    {
        /// <summary>
        /// 返回当前游戏手柄的Id
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// 返回当前游戏手柄的名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 返回当前游戏手柄是否已连接
        /// </summary>
        public bool IsConnected { get; private set; }

        /// <summary>
        /// 游戏手柄的参数信息
        /// </summary>
        public JoystickAPI.JOYCAPS JoystickCAPS;

        /// <summary>
        /// 前一次的处于按下状态的按钮
        /// </summary>

        //直接初始化
        //遍历手柄ID寻找
        public Joystick()
        {
            int IDmax = 2, i = 0;
            int joystickId;
            this.JoystickCAPS = new JoystickAPI.JOYCAPS();
            for (; i < IDmax; i++)
            {
                joystickId = i;
                if (JoystickAPI.joyGetDevCaps(joystickId, ref this.JoystickCAPS, Marshal.SizeOf(typeof(JoystickAPI.JOYCAPS)))
                == JoystickAPI.JOYERR_NOERROR)
                {
                    this.IsConnected = true;
                    this.Name = this.JoystickCAPS.szPname;
                    i = IDmax + 1;
                }
                else
                {
                    this.IsConnected = false;
                }
            }
            if (i == IDmax)
            {
                System.Windows.MessageBox.Show("未插入摇杆", "ERROR");
                Environment.Exit(0);
            }
                
            //取得游戏手柄的参数信息

        }
        /// <summary>
        /// 根据游戏手柄的Id实例化
        /// </summary>
        /// <param name="joystickId"></param>
        public Joystick(int joystickId)
        {
            this.Id = joystickId;
            this.JoystickCAPS = new JoystickAPI.JOYCAPS();


            int test = JoystickAPI.joyGetDevCaps(joystickId, ref this.JoystickCAPS, Marshal.SizeOf(typeof(JoystickAPI.JOYCAPS)));

            //取得游戏手柄的参数信息
            if (test
                == JoystickAPI.JOYERR_NOERROR)
            {
                this.IsConnected = true;
                System.Console.WriteLine(JoystickAPI.joyGetNumDevs());
                this.Name = this.JoystickCAPS.szPname;
            }
            else
            {
                this.IsConnected = false;
                //System.Windows.MessageBox.Show("ID为" + joystickId + "的摇杆未插入", "ERROR");
                //Environment.Exit(0);
            }

        }

        
        public int getinfo(ref JoystickAPI.JOYINFOEX infoEx)
        {
            infoEx.dwSize = Marshal.SizeOf(typeof(JoystickAPI.JOYINFOEX));
            infoEx.dwFlags = (int)JoystickAPI.JOY_RETURNBUTTONS;

            int result = JoystickAPI.joyGetPosEx(this.Id, ref infoEx);
            return result;
        }
        /// <summary>
        /// 根据按钮码获取当前按下的按钮
        /// </summary>
        /// <param name="dwButtons"></param>
        /// <returns></returns>
        public JoystickButtons GetButtons(int dwButtons)
        {
            JoystickButtons buttons = JoystickButtons.None;
            if ((dwButtons & JoystickAPI.JOY_BUTTON1) == JoystickAPI.JOY_BUTTON1)
            {
                buttons |= JoystickButtons.B1;
            }
            if ((dwButtons & JoystickAPI.JOY_BUTTON2) == JoystickAPI.JOY_BUTTON2)
            {
                buttons |= JoystickButtons.B2;
            }
            if ((dwButtons & JoystickAPI.JOY_BUTTON3) == JoystickAPI.JOY_BUTTON3)
            {
                buttons |= JoystickButtons.B3;
            }
            if ((dwButtons & JoystickAPI.JOY_BUTTON4) == JoystickAPI.JOY_BUTTON4)
            {
                buttons |= JoystickButtons.B4;
            }
            if ((dwButtons & JoystickAPI.JOY_BUTTON5) == JoystickAPI.JOY_BUTTON5)
            {
                buttons |= JoystickButtons.B5;
            }
            if ((dwButtons & JoystickAPI.JOY_BUTTON6) == JoystickAPI.JOY_BUTTON6)
            {
                buttons |= JoystickButtons.B6;
            }
            if ((dwButtons & JoystickAPI.JOY_BUTTON7) == JoystickAPI.JOY_BUTTON7)
            {
                buttons |= JoystickButtons.B7;
            }
            if ((dwButtons & JoystickAPI.JOY_BUTTON8) == JoystickAPI.JOY_BUTTON8)
            {
                buttons |= JoystickButtons.B8;
            }
            if ((dwButtons & JoystickAPI.JOY_BUTTON9) == JoystickAPI.JOY_BUTTON9)
            {
                buttons |= JoystickButtons.B9;
            }
            if ((dwButtons & JoystickAPI.JOY_BUTTON10) == JoystickAPI.JOY_BUTTON10)
            {
                buttons |= JoystickButtons.B10;
            }

            return buttons;
        }

        /// <summary>
        /// 获取轴的状态
        /// </summary>
        /// <param name="infoEx"></param>
        public  JoystickAxis GetAxis(ref JoystickAPI.JOYINFOEX infoEx)
        {
            //处理X,Y轴
            JoystickAxis axis;
            axis.Xpos = infoEx.dwXpos;
            axis.Ypos = infoEx.dwYpos;
            axis.Zpos = infoEx.dwZpos;
            axis.Rpos = infoEx.dwRpos;
            axis.Upos = infoEx.dwUpos;
            axis.Vpos = infoEx.dwVpos;
            return axis;
        }
        #region IDisposable 成员
        /// <summary>
        /// 释放资源
        #endregion
    }
}
