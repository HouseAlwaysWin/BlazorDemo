using BlazorWinFormsGenericToolKit.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reflection.Metadata;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlazorWinFormsGenericToolKit.Services
{
	public class DataTransferService<T>
	{


		public event Action<object> ReceivedData;

		public async Task SendData(object data)
		{
			await Task.Run(() =>
			{
				ReceivedData?.Invoke(data);
			});
		}

		private readonly Subject<T> dataSubject = new Subject<T>();

		public void PublishData(T data)
		{
			dataSubject.OnNext(data);
		}

		public IObservable<T> SubscribeToData()
		{
			return dataSubject.AsObservable();
		}

		//public async Task SendData(object data)
		//{
		//	await Clients.All.SendAsync("ReceiveLog", data);
		//}

		//public async Task Receive(Action<object> action)
		//{
		//	Connection = new HubConnectionBuilder()
		//	   .WithUrl("/dataTransfer")
		//	   .Build();
		//	Connection.On<string>("ReceiveLog", (message) =>
		//	{
		//		action(message);
		//	});

		//	await Connection.StartAsync();
		//}

	}
}
