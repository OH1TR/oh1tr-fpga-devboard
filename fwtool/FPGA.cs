using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace fwtool
{
	public class FPGA
	{
		Serial con;

		public FPGA (Serial p)
		{
			con = p;
		}

		public bool OnlineConfigure(string file)
		{
			byte[] data=File.ReadAllBytes (file);

			//con.sendString("\r");
			con.sendString("FPGA WIRECONFIG\r");
			if (con.readString () != "OK") 
			{
				Console.WriteLine ("Reset fails\r\n");
				return(false);
			}

			int len = data.Length;
			int pos = 0;
			while (len-pos > 0) 
			{
				int l = (len-pos) >= 54 ? 54 : (len-pos);
				StringBuilder sb = new StringBuilder();
				sb.Append("FPGA WCLOAD ");
				sb.Append (l.ToString ("X4"));

				for (int i = 0; i < l; i++) 
					sb.Append (data [pos + i].ToString ("X2"));

				sb.Append("\r");
				con.sendString (sb.ToString ());
				string ret=con.readString();
				if (ret.Contains ("Done")) 
				{
					Console.Write ("\r\n");
					return(true);
				}

				if (ret.Contains ("Fail")) 
				{
					Console.Write ("\r\n");
					return(false);
				}
				pos += l;

				Console.Write (pos.ToString () + " / " + data.Length.ToString () + "             \r");
			}
			Console.Write ("\r\n");
			return(false);
		}

		public void writePort(byte port,byte val)
		{
			con.sendString("FPGA W "+port.ToString("X2")+","+val.ToString("X2")+"\r");
			string ret=con.readString();
			if (!ret.Contains("OK"))
				Console.WriteLine ("writePort failed:" + ret);
		}

		public void writePort16b(byte port,UInt16 val)
		{
			con.sendString("FPGA W2 "+port.ToString("X2")+","+val.ToString("X4")+"\r");
			string ret=con.readString();
			if (!ret.Contains("OK"))
				Console.WriteLine ("writePort failed:" + ret);
		}

		public void writePort32b(byte port,UInt32 val)
		{
			con.sendString("FPGA W4 "+port.ToString("X2")+","+val.ToString("X8")+"\r");
			string ret=con.readString();
			if (!ret.Contains("OK"))
				Console.WriteLine ("writePort failed:" + ret);
		}

		public byte readPort(byte port)
		{
			string s;
			con.sendString("FPGA R "+port.ToString("X2")+"\r");
			while(true)
			{
				s=con.readString();

				if(s.StartsWith("Result:"))
					return(byte.Parse(s.Substring(7,2),NumberStyles.HexNumber));
				else
					Console.WriteLine ("read Port failed:" + s);
			}
		}

		public UInt16 readPort16b(byte port)
		{
			string s;
			con.sendString("FPGA R2 "+port.ToString("X2")+"\r");
			while(true)
			{
				s=con.readString();

				if(s.StartsWith("Result:"))
					return(UInt16.Parse(s.Substring(7,4),NumberStyles.HexNumber));
				else
					Console.WriteLine ("read Port failed:" + s);
			}
		}

		public UInt32 readPort32b(byte port)
		{
			string s;
			con.sendString("FPGA R4 "+port.ToString("X2")+"\r");
			while(true)
			{
				s=con.readString();

				if(s.StartsWith("Result:"))
					return(UInt32.Parse(s.Substring(7,8),NumberStyles.HexNumber));
				else
					Console.WriteLine ("read Port failed:" + s);
			}
		}

	}
}


