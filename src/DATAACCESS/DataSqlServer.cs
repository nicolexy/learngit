using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Data.SqlClient;

namespace TENCENT.OSS.CFT.KF.DataAccess
{
	/// <summary>
	/// DataSqlServer 的摘要说明。
	/// </summary>
	public class DataSqlServer
	{
		public string _DataSource;
		public string _DataBase;
		public string _uid;
		public string _password;
		public string _sql;
		public string _sqlStr;

		public DataSqlServer(string DataSource,string DataBase,string uid,string password,string sql)
		{
			this._DataSource = DataSource;
			this._DataBase = DataBase;
			this._uid = uid;
			this._password = password;
			this._sql = sql;
			this._sqlStr = "uid="+_uid+";password="+_password+";Database="+_DataBase+";Data Source="+_DataSource+";Connect Timeout=30";
		}

		#region 无返回值

		public void ModifyValue()
		{
			SqlConnection conn = new SqlConnection(_sqlStr);
			conn.Open();
			SqlCommand cmd = new SqlCommand(_sql,conn);
			cmd.ExecuteNonQuery();
			conn.Close();
		}

		#endregion

		#region 返回DataSet

		public DataSet GetDataSet()
		{
			SqlConnection cnDB = new SqlConnection(_sqlStr);
			SqlCommand cmdDB = new SqlCommand(_sql,cnDB);
			SqlDataAdapter daDB = new SqlDataAdapter();
			daDB.SelectCommand = cmdDB;
			DataSet dsDB = new DataSet();
			daDB.Fill(dsDB);
			return dsDB;  
		}

		#endregion

		#region 属性
		public string DataSource
		{
			get {return _DataSource;}
			set {_DataSource = value;}
		}
		public string DataBase
		{
			get {return _DataBase;}
			set {_DataBase = value;}
		}
		public string uid
		{
			get {return _uid;}
			set {_uid = value;}
		}
		public string password
		{
			get {return _password;}
			set {_password = value;}
		}
		public string sql
		{
			get {return _sql;}
			set {_sql = value;}
		}
		#endregion

	}


}
