using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Core.GeneralUtils.Container;
using Core.SeleniumUtils.Core;

namespace Core.SeleniumUtils.WindowsHandling
{
	/// <summary>
	///     The class provides methods to wait and close for windows by title.
	/// </summary>
	public static class WinAPIUtils
	{
		/// <summary>
		///     Close constant.
		/// </summary>
		private const uint WmClose = 0x0010;

		/// <summary>
		///     Enums the windows.
		/// </summary>
		/// <param name="lpEnumFunc">The lp enum func.</param>
		/// <param name="data">The data.</param>
		/// <returns>The flag.</returns>
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, SearchData data);

		/// <summary>
		///     Gets the window text.
		/// </summary>
		/// <param name="window">The window.</param>
		/// <param name="text">The text.</param>
		/// <param name="maxCount">The max count.</param>
		/// <returns>The flag.</returns>
		[DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		private static extern int GetWindowText(IntPtr window, StringBuilder text, int maxCount);

		/// <summary>
		///     Find window by Caption only. Note you must pass IntPtr.Zero as the first parameter.
		/// </summary>
		/// <param name="zeroOnly">The zero only.</param>
		/// <param name="lpWindowName">Name of the lp window.</param>
		/// <returns>The IntPtr.</returns>
		[DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "FindWindow", SetLastError = true)]
		private static extern IntPtr FindWindowByCaption(IntPtr zeroOnly, string lpWindowName);

		/// <summary>
		///     Send action to window.
		/// </summary>
		/// <param name="hWnd">The h WND.</param>
		/// <param name="msg">The MSG.</param>
		/// <param name="wParam">The w param.</param>
		/// <param name="lParam">The l param.</param>
		/// <returns>The IntPtr.</returns>
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		private static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

		/// <summary>
		///     Enumerate all existing windows.
		/// </summary>
		/// <param name="hWnd">The h WND.</param>
		/// <param name="data">The data.</param>
		/// <returns>The flag.</returns>
		private static bool EnumTheWindows(IntPtr hWnd, SearchData data)
		{
			// Check title 
			// This is different from FindWindow() in that the code below allows partial matches
			var sb = new StringBuilder(1024);
			GetWindowText(hWnd, sb, sb.Capacity);
			if (data.PossibleNames.Any(pn => sb.ToString().StartsWith(pn, StringComparison.OrdinalIgnoreCase)))
			{
				data.HWnd = hWnd;
				return false; // halt enumeration when found
			}

			return true;
		}

		/// <summary>
		///     Wait for window starts with name.
		/// </summary>
		/// <param name="possibleStartsWithNames">beginning of the window name.</param>
		/// <returns>The IntPtr.</returns>
		public static IntPtr WaitForWindowStartsWith(string[] possibleStartsWithNames)
		{
			var sd = new SearchData {PossibleNames = possibleStartsWithNames};
			var waitForWindow = new Func<bool>(() => !EnumWindows(EnumTheWindows, sd));
			Waiter.SpinWaitEnsureSatisfied(waitForWindow, TimeSpan.FromSeconds(3), TimeSpan.FromMilliseconds(300),
				"No opened windows found with titles start with [" + string.Join(", ", possibleStartsWithNames) + "]");
			return sd.HWnd;
		}

		/// <summary>
		///     Wait for window with exact name.
		/// </summary>
		/// <param name="possibleNames">possible window name.</param>
		/// <returns>The IntPtr.</returns>
		public static IntPtr WaitForWindow(string[] possibleNames)
		{
			var hWnd = IntPtr.Zero;
			var waitForWindow = new Func<bool>(() => possibleNames.Any(pn =>
			{
				hWnd = FindWindowByCaption(IntPtr.Zero, pn);
				return hWnd != IntPtr.Zero;
			}));

			Waiter.SpinWaitEnsureSatisfied(waitForWindow, TimeSpan.FromSeconds(3), TimeSpan.FromMilliseconds(300),
				"No opened windows found with titles [" + string.Join(", ", possibleNames) + "]");
			return hWnd;
		}

		/// <summary>
		///     Method is waiting for a dialog window. Method is decided dialog window is displayed or not.
		/// </summary>
		/// <param name="dialogWindow">Possible dialog window title.</param>
		/// <returns>The flag.</returns>
		public static bool WaitForDialogWindow(string[] dialogWindow)
		{
			var waitForDialogWindow =
				new Func<bool>(() => dialogWindow.Any(pn => { return FindWindowByCaption(IntPtr.Zero, pn) != IntPtr.Zero; }));

			return Waiter.SpinWait(waitForDialogWindow, TimeSpan.FromSeconds(3));
		}

		/// <summary>
		///     Wait for window with exact name and close.
		/// </summary>
		/// <param name="possibleNames">window name.</param>
		public static void CloseWindow(string[] possibleNames)
		{
			var windowPtr = WaitForWindow(possibleNames);
			SendMessage(windowPtr, WmClose, IntPtr.Zero, IntPtr.Zero);
		}

		/// <summary>
		///     The close window.
		/// </summary>
		/// <param name="windowHandler">
		///     The window ptr.
		/// </param>
		public static void CloseWindow(IntPtr windowHandler)
		{
			SendMessage(windowHandler, WmClose, IntPtr.Zero, IntPtr.Zero);
		}

		/// <summary>
		///     Wait for window starts with name and close.
		/// </summary>
		/// <param name="possibleStartsWithNames">beginning of the window name.</param>
		public static void CloseWindowStartsWith(string[] possibleStartsWithNames)
		{
			var windowPtr = WaitForWindowStartsWith(possibleStartsWithNames);
			SendMessage(windowPtr, WmClose, IntPtr.Zero, IntPtr.Zero);
		}

		/// <summary>
		///     Attaches the thread input.
		/// </summary>
		/// <param name="attachProcessId">The attach process identifier.</param>
		/// <param name="attachToProcessId">The attach to process identifier.</param>
		/// <param name="attach">if set to <c>true</c> [attach].</param>
		/// <returns>Thread attached.</returns>
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool AttachThreadInput(uint attachProcessId, uint attachToProcessId,
			[MarshalAs(UnmanagedType.Bool)] bool attach);

		/// <summary>
		///     Brings the window to top.
		/// </summary>
		/// <param name="mainWindowHandle">The main window handle.</param>
		/// <returns>If window set to foreground.</returns>
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool BringWindowToTop(IntPtr mainWindowHandle);

		/// <summary>
		///     Shows the window.
		/// </summary>
		/// <param name="mainWindowHandle">The main window handle.</param>
		/// <param name="showMode">The show mode.</param>
		/// <returns>Window shown.</returns>
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool ShowWindow(IntPtr mainWindowHandle, uint showMode);

		/// <summary>
		///     Gets the foreground window.
		/// </summary>
		/// <returns>Foreground window handle.</returns>
		[DllImport("user32.dll")]
		private static extern IntPtr GetForegroundWindow();

		/// <summary>
		///     Gets the window thread process identifier.
		/// </summary>
		/// <param name="mainWindowHandle">The main window handle.</param>
		/// <param name="processId">The process identifier.</param>
		/// <returns>Process Id.</returns>
		[DllImport("user32.dll")]
		private static extern uint GetWindowThreadProcessId(IntPtr mainWindowHandle, IntPtr processId);

		/// <summary>
		///     Gets the current thread identifier.
		/// </summary>
		/// <returns>Thread Id.</returns>
		[DllImport("kernel32.dll")]
		private static extern uint GetCurrentThreadId();

		/// <summary>
		///     http://www.pinvoke.net/default.aspx/user32.SetForegroundWindow
		///     Forces the foreground window.
		/// </summary>
		/// <param name="mainWindowHandle">The main window handle.</param>
		public static void ForceForegroundWindow(IntPtr mainWindowHandle)
		{
			try
			{
				var foreThread = GetWindowThreadProcessId(GetForegroundWindow(), IntPtr.Zero);
				var appThread = GetCurrentThreadId();
				const uint SW_SHOW = 5;

				if (foreThread != appThread)
				{
					Logger.WriteLine("AttachThreadInput()");
					AttachThreadInput(foreThread, appThread, true);
					Logger.WriteLine("BringWindowToTop()");
					BringWindowToTop(mainWindowHandle);
					Logger.WriteLine("ShowWindow()");
					ShowWindow(mainWindowHandle, SW_SHOW);
					Logger.WriteLine("AttachThreadInput()");
					AttachThreadInput(foreThread, appThread, false);
				}
				else
				{
					Logger.WriteLine("BringWindowToTop()");
					BringWindowToTop(mainWindowHandle);
					Logger.WriteLine("ShowWindow()");
					ShowWindow(mainWindowHandle, SW_SHOW);
				}

				Logger.WriteLine("Done all...");
			}
			catch (Exception e)
			{
				Logger.WriteLine("Exception while forcing foreground window: " + e.Message);
				Logger.WriteLine("Stack trace: " + e.StackTrace);
			}
		}

		/// <summary>
		///     Class to store window title and pointer.
		/// </summary>
		private class SearchData
		{
			/// <summary>
			///     Gets or sets the possible names.
			/// </summary>
			/// <value>The possible names.</value>
			public string[] PossibleNames { get; set; }

			/// <summary>
			///     Gets or sets the H WND.
			/// </summary>
			/// <value>The H WND.</value>
			public IntPtr HWnd { get; set; }
		}

		/// <summary>
		///     Enums WindowsProc.
		/// </summary>
		/// <param name="hWnd">The hWnd.</param>
		/// <param name="data">The data.</param>
		/// <returns>The flag.</returns>
		private delegate bool EnumWindowsProc(IntPtr hWnd, SearchData data);
	}
}