using System;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace Devulcanizer
{
	public class DevulcanizerService : ServiceBase
	{
		const int		DELAY				= 1000 * 60 * 60;		// 1 hour
		const int		DELAY_INFECTED		= 1000 * 60;			// 1 minute
		const int		TRAY_TIMEOUT		= 1000 * 60;			// 1 minute
		const int		KILL_TIMEOUT		= 1000 * 60;			// 1 minute
		const string	TRAY_TITLE			= "Devulcanizer";
		const string	TRAY_MSG_INFECTED	= "You are infected with Vulcan!";
		const string	TRAY_MSG_KILL		= "Killing Vulcan process...";
		const string	TRAY_TRY_PURGE		= "Trying to purge Vulcan from your system...";
		const string	TRAY_MSG_PURGED		= "Vulcan was successfully removed from your system!";
		const string	TRAY_MSG_PURGE_FAIL	= "Couldn't remove Vulcan from your system!\nPlease remove " + VULCAN_PATH + " manually.";
		const string	VULCAN_DIR			= "Windows\\WinSrvInit";
		const string	VULCAN_PATH			= "Windows\\WinSrvInit\\wininit.exe";
		const string	VULCAN_PROCESS		= "wininit.exe";
		const string	EVENT_INFECTED		= "Found Vulcan infection.";
		const string	EVENT_PURGED		= "Successfully removed Vulcan.";
		const string	EVENT_PURGE_FAIL	= "Removal of Vulcan failed!";
		const string	EVENT_SCAN			= "Scanning for Vulcan infection.";

		bool infected;

		public DevulcanizerService () {
			ServiceName = "Devulcanizer";
			infected = false;
		}

		async Task Run () {

			if (infected) {
				await Task.Delay (DELAY_INFECTED);
				return;
			}

			EventLog.WriteEntry (EVENT_SCAN);

			var path = Path.Combine (Native.WindowsDriveRoot, VULCAN_PATH);
			var vulcaned = File.Exists (path);

			if (vulcaned) {
				
				infected = true;
				EventLog.WriteEntry (EVENT_INFECTED, EventLogEntryType.Warning);

				await TryPurge ();
			}

			await Task.Delay (DELAY);
		}

		async Task TryPurge () {
			
			await Task.Run (() => {

				var vulcan_dir = Path.Combine (Native.WindowsDriveRoot, VULCAN_DIR);
				var vulcan_path = Path.Combine (Native.WindowsDriveRoot, VULCAN_PATH);
				var processes = Process.GetProcessesByName (VULCAN_PROCESS);

				for (var i = 0; i < processes.Length; i++) {
					
					if (processes[i].StartInfo.WorkingDirectory == vulcan_dir
						|| processes[i].StartInfo.FileName == vulcan_path) {

						processes[i].Kill ();
						processes[i].WaitForExit (KILL_TIMEOUT);
					}
				}

				try {
					
					File.Delete (vulcan_path);
					Directory.Delete (vulcan_dir);
				} catch {
					
					EventLog.WriteEntry (EVENT_PURGE_FAIL, EventLogEntryType.Error);
					return;
				}

				infected = false;
				EventLog.WriteEntry (EVENT_PURGED);
			});
		}

		[STAThread]
		protected async override void OnStart (string[] args) {
			
			while (true) {
				await Run ();
			}
		}

		protected override void OnStop () {
			
		}
	}
}

