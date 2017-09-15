using System;
using System.Data;
using System.Data.OracleClient;

namespace BookStore
{
    public class ControlAccess
    {
        private OracleConnection conn;
        //构造函数，构造时自动打开连接
        public ControlAccess()
        {
            conn = new OracleConnection("data source=dbproject;User Id=C#scott;Password=tiger;");
            Open();
        }

        public ControlAccess(string DataSource, string UserId, string Password)
        {
            conn = new OracleConnection("data source=" + DataSource + "User Id=" + UserId + "Password=" + Password);
            Open();
        }

        public bool Open()
        {
            try
            {
                conn.Open();
                Console.WriteLine("Database connect success!");
                return true;
            }
            catch (SystemException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Database connect fail!");
                return false;
            }
        }

        public void Close()
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
                conn.Dispose();
                conn = null;
                Console.WriteLine("Close connection success!");
            }
        }

        private OracleCommand CreateCommand(string sqlstr)
        {
            OracleCommand cmd = new OracleCommand(sqlstr, conn);
            return cmd;
        }

        //执行SQL命令，并返回受影响的行数
        public int ExecNonQuery(string sqlstr)
        {
            return CreateCommand(sqlstr).ExecuteNonQuery();
        }

        //执行SQL命令，并返回结果集的第一行第一列
        public object ExecuteScalar(string sqlstr)
        {
            return CreateCommand(sqlstr).ExecuteScalar();
        }
        //执行SQL命令，并返回reader对象
        public OracleDataReader ExecuteReader(string sqlstr)
        {
            return CreateCommand(sqlstr).ExecuteReader();
        }

        //执行SQL命令，并返回dataset对象
        public DataSet GetDataSet(string sqlstr)
        {
            OracleDataAdapter oda = new OracleDataAdapter(sqlstr, conn);
            DataSet ds = new DataSet();
            oda.Fill(ds);
            return ds;
        }

    }
}
