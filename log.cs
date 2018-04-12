// Copyright (c) 2015 IntervalZero, Inc.  All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using IntervalZero.RTX64.RTAPI.IO;

namespace WpfActiveDefenceSystem
{
	internal static partial class Command
	{
		// DWORD RtWaitForSingleObject(
		//            HANDLE hHandle,
		//            DWORD dwMilliseconds);
		//[DllImport("rtapi.dll", SetLastError = true)]
		//private static extern int RtWaitForSingleObject(IntPtr hHandle, int dwMilliseconds);
        
        static IntPtr processHandle;
        public static SharedParameter strSharedParameter = new SharedParameter();
        public static RTSharedMemory sharedMemory;

        public static bool OpenShareMemory()
		{
			
			const string sharedDataName = "rtxSHM";
			var memorySize = Marshal.SizeOf(typeof(SharedParameter));
            sharedMemory = new RTSharedMemory(memorySize, sharedDataName);

            string[] appParams = {"/h","/s","/f",String.Format("/m {0}", sharedDataName),"15"};

            return true;
		}

        public static bool RunRTSSProcess()
        {

            string[] appParams = { "/h", "/s", "/f", String.Format("/m {0}", "rtxSHM"), "15" };

            var launchStr = LaunchSclient.Launch(appParams, out processHandle);
            if (!String.IsNullOrEmpty(launchStr))
                return false;

            if (processHandle != IntPtr.Zero)
                ReadShareMemory();

            return true;
        }


        public static string ReadShareMemory()
        {
            // seek to start
            sharedMemory.Seek(0, SeekOrigin.Begin);

            // create binary reader
            var binRead = new BinaryReader(sharedMemory);

            // read each field in order
            strSharedParameter.test = binRead.ReadDouble();
            binRead.ReadBytes(2);   //iRTSS
            //strSharedParameter.iRTSS = binRead.ReadInt64();
            binRead.ReadBytes(Marshal.SizeOf(strSharedParameter.strStick));
            binRead.ReadBytes(Marshal.SizeOf(strSharedParameter.strStick2));
            return String.Empty;
        }

        public static string WriteShareMemory()
        {
            // seek to start
            sharedMemory.Seek(0, SeekOrigin.Begin);

            Command.sharedMemory.Read(System.BitConverter.GetBytes(strSharedParameter.test), 0, sizeof(double));    //不改变内存里的值
            Command.sharedMemory.Write(System.BitConverter.GetBytes(strSharedParameter.iRTSS), 0, 8);
            Command.sharedMemory.Write(StructToBytes(strSharedParameter.strStick), 0, Marshal.SizeOf(strSharedParameter.strStick));
            Command.sharedMemory.Write(StructToBytes(strSharedParameter.strStick2), 0, Marshal.SizeOf(strSharedParameter.strStick2));

            return String.Empty;
        }

        [StructLayout(LayoutKind.Sequential)]
		public struct SharedParameter
        {
            public double test;
            public Int64 iRTSS;   //0: Stop 1: Run 2: Pause
            public Stick strStick;
            public Stick strStick2;
        };

        public struct Stick
        {
            public double Roll;
            public double Pitch;
            public double Yaw;
            public double Throttle;
            public double bFire;
        };

        public static Byte[] StructToBytes(Object structure)
        {
            Int32 size = Marshal.SizeOf(structure);
            Console.WriteLine(size);
            IntPtr buffer = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.StructureToPtr(structure, buffer, false);
                Byte[] bytes = new Byte[size];
                Marshal.Copy(buffer, bytes, 0, size);
                return bytes;
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }


    }
}