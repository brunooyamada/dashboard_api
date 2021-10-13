using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace dashboard_api
{
    public class DbSession: IDisposable
    {

        public IDbConnection Connection { get; }
        public DbSession(IConfiguration configuration)
        {
            Connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));

            Connection.Open();
        }

        public void Dispose() => Connection?.Dispose();

    }
}
