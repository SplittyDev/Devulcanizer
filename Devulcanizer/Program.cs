using System;
using System.ServiceProcess;

namespace Devulcanizer
{
	class MainClass
	{
		[STAThread]
		public static void Main (string[] args) {

			ServiceBase.Run (new DevulcanizerService ());
		}
	}
}
