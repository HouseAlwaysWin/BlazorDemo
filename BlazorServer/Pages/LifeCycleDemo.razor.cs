using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;
using BlazorServer;
using BlazorServer.Shared;
using BlazorServer.Models;

namespace BlazorServer.Pages
{
    public partial class LifeCycleDemo
    {
        public string Title = "�Ѽ�(�q[Parameter]���o)";
        public void ChangedParamSet()
        {
            if (Title == "�Ѽ�(�q[Parameter]���o)")
            {
                Title = "�����O����";
            }
            else
            {
                Title = "�Ѽ�(�q[Parameter]���o)";
            }
        }
    }
}