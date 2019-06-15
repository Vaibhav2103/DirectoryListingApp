using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Text;

namespace DLWebApp
{
    class DirectoryTree : IDisposable
    {
        const string connectionString = "Server=10.11.8.88;Database=DbTest;Trusted_Connection=True;User ID=sa;password=1234";
        private SqlConnection Connection;
        private SqlCommand Command;
        private bool CommandPrepared;

        private SqlParameter ParentDirectoryId;
        private SqlParameter DirectoryName;

        public DirectoryTree()
        {
            Connection = new SqlConnection(connectionString);
            Command = Connection.CreateCommand();
            ParentDirectoryId = new SqlParameter("@parent_id", SqlDbType.Int, 4);
            DirectoryName = new SqlParameter("@name", SqlDbType.VarChar, 256);

            ParentDirectoryId.IsNullable = true;

            DirectoryName.IsNullable = false;

            Command.Parameters.Add(ParentDirectoryId);
            Command.Parameters.Add(DirectoryName);
            Command.CommandType = CommandType.Text;
            Command.CommandText = @"insert dbo.directory ( parent_id , name ) values ( @parent_id , @name ) ; select id = scope_identity() ; ".Trim();

            return;
        }

        public DataTable GetDataTableFromQuery(string strQry)
        {
            DataTable dataTable = new DataTable();
            SqlCommand selectCommand = new SqlCommand();
            bool flag = false;
            try
            {

                dataTable = new DataTable("ResultSet");
                selectCommand.CommandType = CommandType.Text;
                selectCommand.Connection = Connection;
                selectCommand.CommandText = strQry;
                using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    sqlDataAdapter.Fill(dataTable);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dataTable;
        }

        public void Load()
        {
            if (Connection.State == ConnectionState.Closed)
            {
                Connection.Open();
                Command.Prepare();
            }
        }

        public void Dispose()
        {
            if (Command != null)
            {
                Command.Cancel();
                Command.Dispose();
                Command = null;
            }
            if (Connection != null)
            {
                Connection.Dispose();
                Connection = null;
            }
            return;
        }
    }
}
