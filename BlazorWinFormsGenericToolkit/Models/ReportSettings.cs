using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorWinFormsGenericToolKit.Models
{
    public class ReportSetting
    {
        public string Url { get; set; }
        public Dictionary<string, string> DefaultParams { get; set; }
    }
}
