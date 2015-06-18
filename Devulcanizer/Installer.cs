using System;
using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace Devulcanizer
{
	[RunInstaller (true)]
	public class DevulcanizerInstaller : Installer
	{
		public DevulcanizerInstaller () {

			var proc = new ServiceProcessInstaller ();
			proc.Account = ServiceAccount.LocalSystem;

			var installer = new ServiceInstaller ();
			installer.StartType = ServiceStartMode.Automatic;
			installer.ServiceName = "Devulcanizer";
			installer.Description = "Always watches. No eyes.";

			Installers.Add (proc);
			Installers.Add (installer);
		}
	}
}

