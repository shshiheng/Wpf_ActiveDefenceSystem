// Copyright (c) 2015 IntervalZero, Inc.  All rights reserved.


using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace WpfActiveDefenceSystem
{
	/// <summary>
	/// Launch the Sclient Application
	/// </summary>
	public class LaunchSclient
	{

        // Process information
        public static ProcessInformation lpProcessInfo;

        public static String Launch(string[] appParams, out IntPtr processHandle)
		{
            const String rtssSClientAppName = "RTXSim.rtss"; 
             processHandle = IntPtr.Zero;

			var commandline = new StringBuilder();

			// get the app to run
			var lpAppName = rtssSClientAppName;

			try
			{
				//var rtx64Dir = Environment.GetEnvironmentVariable("RTX64Dir");
				//if (String.IsNullOrEmpty(rtx64Dir))
				//	rtx64Dir = Environment.CurrentDirectory;
				//rtx64Dir = Path.Combine(rtx64Dir, "bin\\");
				//lpAppName = Path.Combine(rtx64Dir, lpAppName);

				// See if the file exists
				var f = new FileInfo(lpAppName);
				if (!f.Exists)
				{
					return String.Format("File {0} not found", lpAppName);
				}

				// The specified RTSS file exists.

				lpAppName = String.Format("{0}{1}{0}", "\"", f.FullName);
				commandline.Append(lpAppName);

				// get the app parameters
				foreach (var s in appParams)
					commandline.AppendFormat(" {0}", s);

				// Trim it
				var lpCommandLine = commandline.ToString().Trim();
				if (lpCommandLine.Length == 0)
					lpCommandLine = null;

				// Create STARTUPINFOEX structure for launch
				var sInfo = new StartupInfoEx(WindowShowFlags.SwHide);

				Int64 procAttributeSize = 0;

				// ask RTAPI How many bytes we need to allocate
				RtInitializeProcThreadAttributeList(IntPtr.Zero, 0, 0, ref procAttributeSize);
				sInfo.lpAttributeList = Marshal.AllocHGlobal((int)procAttributeSize);
				// now initialize the list
				RtInitializeProcThreadAttributeList(sInfo.lpAttributeList, 0, 0,
																 ref procAttributeSize);



				// Create the process
				var result = RtCreateProcess(
					null,				// lpAppName
					lpCommandLine,		// lpCommandLine
					IntPtr.Zero,		// lpThreadAttributes
					IntPtr.Zero,		// lpProcessAttributes
					false,				// bInheritHandles
					(int)DwCreationFlag.ExtendedSTARTUPINFOPresent,
					IntPtr.Zero,		// lpEnvironment
					IntPtr.Zero,		// lpCurrentDirectory
					ref sInfo,			// sInfo
					out lpProcessInfo);

				processHandle = lpProcessInfo.hProcess;

				// check if it launched
				if (!result)
				{
					var error = Marshal.GetLastWin32Error();
					return String.Format("ERROR Launching {0} : {1}", lpAppName, error);
				}

				// We must close the thread handle before closing the process handle.
				if (lpProcessInfo.hThread == IntPtr.Zero) return "";

                // close the thread handle
                RtCloseHandle(lpProcessInfo.hThread);
                processHandle = lpProcessInfo.hProcess;
                return "";
			}
			catch (Exception e)
			{
				return e.Message;
			}
		}


        /// <summary>
        /// Allow startupInfo and startupinfoex
        /// </summary>
        /// <param name="lpApplicationName"></param>
        /// <param name="lpCommandLine"></param>
        /// <param name="lpProcessAttributes"></param>
        /// <param name="lpThreadAttributes"></param>
        /// <param name="bInheritHandles"></param>
        /// <param name="dwCreationFlags"></param>
        /// <param name="lpEnvironment"></param>
        /// <param name="lpCurrentDirectory"></param>
        /// <param name="lpStartupInfo"></param>
        /// <param name="lpProcessInformation"></param>
        /// <returns></returns>
        [DllImport("Rtapi.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool RtCreateProcess(string lpApplicationName,
			string lpCommandLine,
			IntPtr lpProcessAttributes,
			IntPtr lpThreadAttributes,
			[MarshalAs(UnmanagedType.Bool)] bool bInheritHandles,
			uint dwCreationFlags,
			IntPtr lpEnvironment,
			IntPtr lpCurrentDirectory,
			ref StartupInfoEx lpStartupInfo,
			out ProcessInformation
				lpProcessInformation);

		[DllImport("Rtapi.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool RtCloseHandle(IntPtr handle);

        //[DllImport("Rtapi.dll", SetLastError = true)]
        //public static extern void ExitProcess(uint uExitCode);

        /// <summary>
        /// Init the thread attribute list
        /// </summary>
        /// <param name="lpAttributeList"></param>
        /// <param name="dwAttributeCount"></param>
        /// <param name="dwFlags"></param>
        /// <param name="lpSize"></param>
        /// <returns></returns>
        [DllImport("Rtapi.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool RtInitializeProcThreadAttributeList(
			IntPtr lpAttributeList,
			int dwAttributeCount,
			int dwFlags,
			ref Int64 lpSize);
	}

	/// <summary>
	/// Extended startup info
	/// </summary>
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct StartupInfoEx
	{
		public Int32 cb;
		public string lpReserved;
		public string lpDesktop;
		public string lpTitle;
		public Int32 dwX;
		public Int32 dwY;
		public Int32 dwXSize;
		public Int32 dwYSize;
		public Int32 dwXCountChars;
		public Int32 dwYCountChars;
		public Int32 dwFillAttribute;
		public Int32 dwFlags;
		public Int16 wShowWindow;
		public Int16 cbReserved2;
		public IntPtr lpReserved2;
		public IntPtr hStdInput;
		public IntPtr hStdOutput;
		public IntPtr hStdError;
		public IntPtr lpAttributeList;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="show"></param>
		public StartupInfoEx(WindowShowFlags show)
		{
			cb = Marshal.SizeOf(typeof(StartupInfoEx));
			lpReserved = null;
			lpDesktop = null;
			lpTitle = null;
			dwX = 0;
			dwY = 0;
			dwXSize = 0;
			dwYSize = 0;
			dwXCountChars = 0;
			dwYCountChars = 0;
			dwFillAttribute = 0;
			dwFlags = 0;
			wShowWindow = Convert.ToInt16(show);
			cbReserved2 = 0;
			lpReserved2 = IntPtr.Zero;
			hStdInput = IntPtr.Zero;
			hStdOutput = IntPtr.Zero;
			hStdError = IntPtr.Zero;
			lpAttributeList = IntPtr.Zero;
		}
	}

	/// <summary>
	/// Process info structure
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct ProcessInformation
	{
		public IntPtr hProcess;
		public IntPtr hThread;
		public int dwProcessId;
		public int dwThreadId;
	}

	/// <summary>
	/// Window positioning flags
	/// </summary>
	public enum WindowShowFlags
	{
		SwForceminimize = 11,
		// Minimizes a window, even if the thread that owns the window is not responding. This flag should only be used when minimizing windows from a different thread.
		SwHide = 0, // Hides the window and activates another window.
		SwMaximize = 3, // Maximizes the specified window.
		SwMinimize = 6, // Minimizes the specified window and activates the next top-level window in the Z order.
		SwRestore = 9,
		// Activates and displays the window. If the window is minimized or maximized, the system restores it to its original procAttributeSize and position. An application should specify this flag when restoring a minimized window.
		SwShow = 5, // Activates the window and displays it in its current procAttributeSize and position.
		SwShowdefault = 10,
		// Sets the show state based on the SW_ value specified in the STARTUPINFO structure passed to the CreateProcess function by the program that started the application.
		SwShowmaximized = 3, // Activates the window and displays it as a maximized window.
		SwShowminimized = 2, // Activates the window and displays it as a minimized window.
		SwShowminnoactive = 7,
		// Displays the window as a minimized window. This value is similar to SW_SHOWMINIMIZED, except the window is not activated.
		SwShowna = 8,
		// Displays the window in its current procAttributeSize and position. This value is similar to SW_SHOW, except that the window is not activated.
		SwShownoactivate = 4,
		// Displays a window in its most recent procAttributeSize and position. This value is similar to SW_SHOWNORMAL, except that the window is not activated.
		SwShownormal = 1,
		// Activates and displays a window. If the window is minimized or maximized, the system restores it to its original procAttributeSize and position. An application should specify this flag when displaying the window for the first time.
	}

	/// <summary>
	/// Creation flags for StartupInfo
	/// </summary>
	[Flags]
	public enum DwCreationFlag : uint
	{
		DebugProcess = 0x00000001,
		DebugOnlyThisProcess = 0x00000002,
		CreateSuspended = 0x00000004,
		DetachedProcess = 0x00000008,

		CreateNewConsole = 0x00000010,
		NormalPriorityClass = 0x00000020,
		IdlePriorityClass = 0x00000040,
		HighPriorityClass = 0x00000080,

		RealtimePriorityClass = 0x00000100,
		CreateNewProcessGroup = 0x00000200,
		CreateUnicodeEnvironment = 0x00000400,
		CreateSeparateWowVdm = 0x00000800,

		CreateSharedWowVdm = 0x00001000,
		CreateForcedos = 0x00002000,
		BelowNormalPriorityClass = 0x00004000,
		AboveNormalPriorityClass = 0x00008000,

		InheritParentAffinity = 0x00010000,
		InheritCallerPriority = 0x00020000, // Deprecated
		CreateProtectedProcess = 0x00040000,
		ExtendedSTARTUPINFOPresent = 0x00080000,

		ProcessModeBackgroundBegin = 0x00100000,
		ProcessModeBackgroundEnd = 0x00200000,

		CreateBreakawayFromJob = 0x01000000,
		CreatePreserveCodeAuthzLevel = 0x02000000,
		CreateDefaultErrorMode = 0x04000000,
		CreateNoWindow = 0x08000000,

		ProfileUser = 0x10000000,
		ProfileKernel = 0x20000000,
		ProfileServer = 0x40000000,
		CreateIgnoreSystemDefault = 0x80000000,
	}
}