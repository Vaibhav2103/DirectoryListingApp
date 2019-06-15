using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Text;

namespace DLWinApp
{
    class DirectoryTreeLoader : IDisposable
    {
        const string connectionString = "Server=10.11.8.88;Database=DbTest;Trusted_Connection=True;User ID=sa;password=1234";
        private SqlConnection Connection;
        private SqlCommand Command;
        private bool CommandPrepared;

        private SqlParameter ParentDirectoryId;
        private SqlParameter DirectoryName;

        public DirectoryTreeLoader()
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

        public void Load(DirectoryInfo root)
        {
            if (Connection.State == ConnectionState.Closed)
            {
                Connection.Open();
                Command.Prepare();
            }

            Visit(null, root);

            return;
        }

        private void Visit(int? parentId, DirectoryInfo dir)
        {
            // insert the current directory
            ParentDirectoryId.SqlValue = parentId.HasValue ? new SqlInt32(parentId.Value) : SqlInt32.Null;
            DirectoryName.SqlValue = new SqlString(dir.Name);

            object o = Command.ExecuteScalar();
            int id = (int)(decimal)o;

            // For storing file details on database
            string[] filePaths = Directory.GetFiles(dir.FullName, "*.*");
            foreach (string fName in filePaths){
                ParentDirectoryId.SqlValue = id;
                DirectoryName.SqlValue = new SqlString(Path.GetFileName(fName.ToString()));
                object i = Command.ExecuteScalar();
            }

            // visit each subdirectory in turn
            foreach (DirectoryInfo subdir in dir.EnumerateDirectories())
            {
                Visit(id, subdir);
            }

            return;
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
