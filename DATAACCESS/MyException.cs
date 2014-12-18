using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace TENCENT.OSS.CFT.KF.Common
{
	/// <summary>
	/// 业务逻辑异常类。
	/// </summary>
	[Serializable]
	public class LogicException : ApplicationException, ISerializable 
	{
		public LogicException()
			: base()
		{
		}

		public LogicException(string message)
			: base(message)
		{}

		public LogicException(string message,Exception innerException)
			: base(message, innerException)
		{}
	}


	/// <summary>
	/// 数据访问异常类。
	/// </summary>
	[Serializable]
	public class DataAccessException : ApplicationException, ISerializable 
	{
		private string connectionString;
		public string ConnectionString
		{
			get
			{
				return connectionString;
			}
		}

		private string sql;
		public string Sql
		{
			get
			{
				return sql;
			}
		}

		public DataAccessException()
			: base()
		{
		}

		public DataAccessException(string message)
			: base(message)
		{}

		public DataAccessException(string message,Exception innerException)
			: base(message, innerException)
		{}

		public DataAccessException(string message, string connectionstring, string strsql)
			: this(message)
		{
			sql = strsql;
			connectionString = connectionstring;
		}

		public DataAccessException(string message, string connectionstring, string strsql, Exception innerException)
			: this(message, innerException)
		{
			sql = strsql;
			connectionString = connectionstring;
		}

		private DataAccessException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			connectionString = info.GetString("ConnectionString");
			sql = info.GetString("Sql");
		}


		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("ConnectionString", connectionString);
			info.AddValue("Sql", sql);
			base.GetObjectData(info, context);
		}
	}
}
