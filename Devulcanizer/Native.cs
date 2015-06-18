using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Devulcanizer
{
	public static class Native
	{
		[DllImport ("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		public static extern uint GetWindowsDirectory (StringBuilder lpBuf, uint uSz);

		public static string WindowsDirectory {
			get {
				var sz = 0u;
				sz = GetWindowsDirectory (null, sz);
				var accum = new StringBuilder ((int)sz);
				GetWindowsDirectory (accum, sz);
				return accum.ToString ();
			}
		}

		public static string WindowsDriveRoot {
			get { return Path.GetPathRoot (WindowsDirectory); }
		}
	}
}

