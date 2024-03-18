using BlazorWinFormsGenericToolKit.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorWinFormsGenericToolKit.Core.Extensions
{
	public static class DbSettingExtension
	{
		public static string GetConnectionString(this DbSetting db)
		{
			return GetConnectionString(db.Ip, db.Port, db.ServiceName, db.UserName, db.Password);
		}

		public static string GetConnectionString(string ip, string port, string serviceName, string userName, string password)
		{
			return $"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={ip})(PORT={port}))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME={serviceName})));password={password};user id={userName};Pooling=true;Max Pool Size=100;";
		}


	}
	public static class DbSettingHelper
	{
		public static Dictionary<string, DbSetting> DbMap = new Dictionary<string, DbSetting>
		{
			{ "TC" , new DbSetting { FactoryCode = "TC", Ip = "172.20.10.110", Port = "1521", ServiceName = "tcdbt", UserName = "dlpdba", Password = "tctdlpdba" } },
			{ "DG" , new DbSetting { FactoryCode = "DG", Ip = "172.29.1.3", Port = "1521", ServiceName = "dgdbt", UserName = "oadba", Password = "dgtoadba" } },
			{ "VN" , new DbSetting { FactoryCode = "VN", Ip = "172.32.1.199", Port = "1521", ServiceName = "vndbt", UserName = "oadba", Password = "vntoadba" } },
			{ "VG" , new DbSetting { FactoryCode = "VG", Ip = "172.32.1.199", Port = "1521", ServiceName = "vgdbt", UserName = "oadba", Password = "vgtoadba" } },
			{ "VS" , new DbSetting { FactoryCode = "VS", Ip = "172.32.1.199", Port = "1521", ServiceName = "vsdbt", UserName = "oadba", Password = "vstoadba" } },
		};

		public static Dictionary<string, DbSetting> DbPortalMap = new Dictionary<string, DbSetting>
		{
			{"TC_Portal" , new DbSetting { FactoryCode = "TC", Ip = "172.20.10.108", Port = "1521", ServiceName = "k8sdb", UserName = "eipdba", Password = "eipdba" } },
			{"DG_Portal" , new DbSetting { FactoryCode = "DG", Ip = "172.29.1.3", Port = "1521", ServiceName = "DGPORTAL", UserName = "eipdba", Password = "eipdba" } },
			{"VN_Portal" , new DbSetting { FactoryCode = "VN", Ip = "172.32.1.199", Port = "1521", ServiceName = "vnportal", UserName = "eipdba", Password = "eipdba" } },
			{"VG_Portal" , new DbSetting { FactoryCode = "VG", Ip = "172.32.1.199", Port = "1521", ServiceName = "vgportal", UserName = "eipdba", Password = "eipdba" } },
			{"VS_Portal" , new DbSetting { FactoryCode = "VS", Ip = "172.32.1.199", Port = "1521", ServiceName = "vsportal", UserName = "eipdba", Password = "eipdba" } },
		};

		public static IDbCommand CreateCommand(this IDbConnection connection, string sql)
		{
			var cmd = connection.CreateCommand();
			cmd.CommandText = sql;
			return cmd;
		}


	}


}
