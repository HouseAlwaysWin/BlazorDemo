using BlazorWinFormsGenericToolKit.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlazorWinFormsGenericToolKit.Models
{
	public class CommandItem
	{
		public string Id { get; set; } = Guid.NewGuid().ToString();
		public string Name { get; set; }
		public string Command { get; set; }
		public string Arguments { get; set; }
		public int? Port { get; set; }
		public int? MainPID { get; set; } = null;
		public int? SubPID { get; set; }
		public string Encoding { get; set; }

		private bool _isRunning;
		[JsonIgnore]
		public bool IsRunning
		{
			set { _isRunning = value; }
			get
			{
				if (!_isRunning)
				{
					return ProcessTaskHelper.IsProcessExisted(MainPID, out Process process);
				}
				return _isRunning;
			}
		}
		[JsonIgnore]
		public List<JsonObject> OutputLogs { get; set; } = new List<JsonObject>();

	}
}
