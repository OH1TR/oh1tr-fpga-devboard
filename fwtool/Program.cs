using System;

namespace fwtool
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			try
			{
				if(args[0]=="test")
				{
					Serial s = new Serial (args[1], 9600);
					FPGA f = new FPGA (s);
					f.writePort16b(0,32010);
					Console.WriteLine(f.readPort16b(0));
					return;
				}

				if(args.Length==3 && args[0]=="wc")
				{
					Serial s = new Serial (args[1], 9600);
					FPGA f = new FPGA (s);
					f.OnlineConfigure (args [2]);
					Console.WriteLine ("Done\r\n");
					return;
				}
				Console.WriteLine("Usage:\r\n fwtool.exe wc /dev/ttyACM0 bs.bit   Configure FPGA over USB line.\r\n");
			}
			catch(Exception e) 
			{
				Console.WriteLine (e.Message + e.StackTrace);
			}
		}
	}
}
