using DebitSuccess.AgeRanger.Api;
using System.Data.SQLite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;

namespace DebitSuccess.AgeRanger.Data.SQLite {
    public class DataProviderBase {

        private SQLiteConnection _connection;

        private void Connect() {

            var csb = new SQLiteConnectionStringBuilder {
                DataSource = GetDatabasePath()
            };

            _connection = new SQLiteConnection(csb.ToString());
            _connection.Open();

        }

        private void Disconnect() {
            if (_connection == null || _connection.State != ConnectionState.Open) return;
            _connection.Close();
        }

        protected DataTable ExecuteQuery(string sql) {

            SQLiteDataAdapter ad;
            DataTable dt = new DataTable();

            try {

                Connect();

                using (var cmd = _connection.CreateCommand()) {
                    cmd.CommandText = sql;  
                    ad = new SQLiteDataAdapter(cmd);
                    ad.Fill(dt); 
                }

            } finally {
                Disconnect();
            }

            return dt;
        }

        protected void ExecuteNonQuery(string sql, List<SQLiteParameter> parms) {

            try {

                Connect();

                using (var cmd = _connection.CreateCommand()) {
                    cmd.CommandText = sql;

                    if (parms != null) {
                        foreach (var parm in parms) {
                            cmd.Parameters.Add(parm);
                        }
                    }

                    cmd.ExecuteNonQuery();
                }

            } finally {
                Disconnect();
            }

        }

        private string GetDatabasePath() {
            return Path.Combine(GetApplicationBasePath(), @"AgeRanger.db");
        }

        // This isn't how I would normally store a connection...
        private string GetApplicationBasePath() {


            //if (ConfigurationManager.AppSettings.AllKeys.Any(key => key == "ApplicationBasePathOverride")) {
            //    return ConfigurationManager.AppSettings["ApplicationBasePathOverride"];
            //}

            string basePath = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(basePath);

            return Path.GetDirectoryName(Uri.UnescapeDataString(uri.Path));
        }

    }

}

