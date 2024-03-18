using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorWinFormsGenericToolKit.Models
{
	public class DbaErrors
	{
		public string OWNER { get; set; }
		public string NAME { get; set; }
		public string TYPE { get; set; }
		public int SEQUENCE { get; set; }
		public int LINE { get; set; }
		public int POSITION { get; set; }
		public string TEXT { get; set; }
		public string ATTRIBUTE { get; set; }
		public int MESSAGE_NUMBER { get; set; }
	}
}
