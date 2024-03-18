using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorWinFormsGenericToolKit.Repositories
{
	public abstract class RepositoryBase
	{
		public IDbConnection GetConnection(string connStr)
		{
			return new OracleConnection(connStr);
		}
	}
}
