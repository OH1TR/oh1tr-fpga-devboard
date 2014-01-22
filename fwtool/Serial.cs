using System;
using System.IO.Ports;
using System.Globalization;

namespace fwtool
{
	public class Serial
	{
		SerialPort sp;

		public Serial (string port,int speed)
		{
			sp=new SerialPort(port,speed);
			sp.Open ();
		}

		public void sendString(string s)
		{
			byte[] buf=System.Text.Encoding.ASCII.GetBytes(s);
			sp.Write(buf,0,buf.Length);
		}

		public string readString()
		{
			byte[] bs=new byte[1000];
			int i=0;
			byte[] buf=new byte[2];
			string s="";
			do
			{
				sp.Read(buf,0,1);
				//Console.WriteLine(System.Text.Encoding.ASCII.GetString(buf,0,1));
				if(buf[0]==10)
					continue;

				bs[i]=buf[0];
				i++;
			}
			while(buf[0]!=13);
			if(i>2)
				s=System.Text.Encoding.ASCII.GetString(bs,0,i-1);
			else	
				s="";

			return(s);
		}	
	}
}

