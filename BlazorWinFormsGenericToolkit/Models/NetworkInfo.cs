using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorWinFormsGenericToolKit.Models
{
	public class NetworkInfo
	{
		public string Protocol { get; set; }
		public string LocalAddress { get; set; }
		public int LocalPort { get; set; }
		public string ExternalAddress { get; set; }
		public int ExternalPort { get; set; }
		public string Status { get; set; }
		public int PID { get; set; }
	}

}
