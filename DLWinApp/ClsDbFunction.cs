using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Text;

namespace DLWinApp
{
    class ClsDbFunction
    {
        const string strCon = "Provider=SQLOLEDB.1;Persist Security Info=False;User ID=sa;Initial Catalog=DbTest;Data Source=10.11.8.88";
        private SqlConnection Sqlcon;
        private SqlCommand SqlCommand;
        private SqlParameter DirParentId;
        private SqlParameter DirName;

        public ClsDbFunction()
        {
            Sqlcon = new SqlConnection(strCon);
            SqlCommand = Sqlcon.CreateCommand();

            DirParentId=new SqlParameter("@parent_id",SqlDbType.Int,4);
            DirName = new SqlParameter("@name", SqlDbType.VarChar, 500);

            DirParentId.IsNullable = true;
            DirName.IsNullable = false;

            SqlCommand.Parameters.Add(DirParentId);
            SqlCommand.Parameters.Add(DirName);
            SqlCommand.CommandType = CommandType.Text;
            SqlCommand.CommandText = @"Insert Into Dbo.Directory (parent_id,name) Values(@parent_id,@name); Select id=scope_identity();".Trim();

        }


    }
}
