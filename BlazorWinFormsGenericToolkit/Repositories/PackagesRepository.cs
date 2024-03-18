using BlazorWinFormsGenericToolKit.Core.Extensions;
using BlazorWinFormsGenericToolKit.Models;
using Dapper;
using Polly;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorWinFormsGenericToolKit.Repositories
{
	public class PackagesRepository : RepositoryBase
	{
		private readonly string _connStr;
		private readonly DbSetting _dbSetting;
		#region Property
		private static readonly Dictionary<Type, string> TypeAliases = new Dictionary<Type, string> {
			//{ typeof(int), "int" },
			//{ typeof(short), "short" },
			//{ typeof(byte), "byte" },
			//{ typeof(byte[]), "byte[]" },
			//{ typeof(long), "long" },
			//{ typeof(double), "double" },
			//{ typeof(decimal), "decimal" },
			//{ typeof(float), "float" },
			//{ typeof(bool), "bool" },
			//{ typeof(string), "string" }

			{ typeof(int), "Number" },
			{ typeof(short), "Number" },
			{ typeof(byte), "Number" },
			{ typeof(byte[]), "Select" },
			{ typeof(long), "Number" },
			{ typeof(double), "Number" },
			{ typeof(decimal), "Number" },
			{ typeof(float), "Number" },
			{ typeof(bool), "Checkbox" },
			{ typeof(string), "Text" },
			{ typeof(DateTime), "Date" }
		};

		private static readonly Dictionary<string, string> QuerySqls = new Dictionary<string, string> {
			   {"sqlconnection", "SELECT  *  FROM [{0}] WHERE 1=2" },
			   {"sqlceserver", "SELECT  *  FROM [{0}] WHERE 1=2" },
			   {"sqliteconnection", "SELECT  *  FROM [{0}] WHERE 1=2" },
			   {"oracleconnection", "SELECT  *  FROM \"{0}\" WHERE 1=2" },
			   {"mysqlconnection", "SELECT  *  FROM `{0}` WHERE 1=2" },
			   {"npgsqlconnection", "SELECT  *  FROM \"{0}\" WHERE 1=2" }
	   };

		private static readonly Dictionary<string, string> TableSchemaSqls = new Dictionary<string, string> {
			   {"sqlconnection", "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'" },
			   {"sqlceserver", "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES  WHERE TABLE_TYPE = 'BASE TABLE'" },
			   {"sqliteconnection", "SELECT NAME FROM SQLITE_MASTER WHERE TYPE = 'TABLE'" },
			   {"oracleconnection", "SELECT TABLE_NAME FROM USER_TABLES WHERE TABLE_NAME NOT IN (SELECT VIEW_NAME FROM USER_VIEWS)" },
			   {"mysqlconnection", "SELECT TABLE_NAME FROM  INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'" },
			   {"npgsqlconnection", "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'" }
	   };


		private static readonly HashSet<Type> NullableTypes = new HashSet<Type> {
			   typeof(int),
			   typeof(short),
			   typeof(long),
			   typeof(double),
			   typeof(decimal),
			   typeof(float),
			   typeof(bool),
			   typeof(DateTime)
	   };
		#endregion
		public PackagesRepository(DbSetting dbSetting)
		{
			_dbSetting = dbSetting;
			_connStr = dbSetting.GetConnectionString();
		}

	
		public List<string> GetAllPackageNames()
		{
			var sqlCmd = @" 
              SELECT DISTINCT NAME FROM all_source              
              WHERE TYPE = 'PACKAGE' AND NAME LIKE 'PK_%' OR NAME LIKE 'PC_%'";

			using (var conn = GetConnection(_connStr))
			{
				return conn.Query<string>(sqlCmd).ToList();
			}
		}

		public List<string> GetPackageHead(string packageName)
		{
			var sqlCmd = @" 
                SELECT text 
                  FROM all_source 
                 WHERE name = :packageName 
                   AND type = 'PACKAGE' 
				   AND OWNER = :UserName
              ORDER BY line";

			using (var conn = GetConnection(_connStr))
			{
				return conn.Query<string>(sqlCmd, new { packageName, UserName = _dbSetting.UserName.ToUpper() }).ToList();
			}
		}

		public List<string> GetPackageBody(string packageName)
		{
			var sqlCmd = @" 
                SELECT text 
                  FROM all_source 
                 WHERE name = :packageName 
                   AND type = 'PACKAGE BODY' 
				   AND OWNER = :UserName
              ORDER BY line";

			using (var conn = GetConnection(_connStr))
			{
				return conn.Query<string>(sqlCmd, new { packageName, UserName = _dbSetting.UserName.ToUpper() }).ToList();
			}
		}

		public string UpdatePackage(string head, string body)
		{
			var errorMessage = "";
			using (var conn = GetConnection(_connStr))
			{
				try
				{
					conn.Execute(head);
					conn.Execute(body);
				}
				catch (Exception ex)
				{
					errorMessage = ex.Message;
				}
				return errorMessage;
			}
		}

		public List<DbaErrors> GetDbaErrors(string packageName)
		{
			var sqlCmd = @"
                SELECT*
                  FROM dba_errors
                 WHERE NAME = :packageName
            ";
			using (var conn = GetConnection(_connStr))
			{

				var dbaErrors = conn.Query<DbaErrors>(sqlCmd, new { packageName });
				return dbaErrors.ToList();
			}
		}
	}
}
