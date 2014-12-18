using System;

namespace TENCENT.OSS.CFT.KF.Common
{
	/// <summary>
	/// T0Transfer 的摘要说明。
	/// </summary>
	/// <summary>
	/// T+0付款中,场次号和时间等的转化
	/// </summary>
	public class T0Transfer
	{
		const int BASETICK = 10000;
		const int BASEORDER = 48;

		/// <summary>
		/// ASC码转化为场次.
		/// </summary>
		/// <param name="ascstr">ASC码</param>
		/// <returns>场次,如果有错,返回-1</returns>
		public static int Asc2Order(string ascstr)
		{
			if(ascstr == null || ascstr.Length > 1)
				return -1;

			int itmp = Convert.ToInt32(Convert.ToChar(ascstr));
			return itmp - BASEORDER;
		}

		/// <summary>
		/// 场次转化为ASC码
		/// </summary>
		/// <param name="order">场次</param>
		/// <returns>ASC码</returns>
		public static string Order2Asc(int order)
		{
			order += BASEORDER;

			if(order < 49)
				order = 49;

			if(order > 122)
				return null;

			//避开 ? ' - \ ; 字符
			if(order == 63 || order == 96)
				order ++;

			if(order == 95)
				order += 2;

			if(order == 59)
				order ++;

			if(order == 92)
				order++;

			string stmp = Convert.ToChar(order).ToString();
			return stmp;
		}

		/// <summary>
		/// 场次转化为时间
		/// </summary>
		/// <param name="order">场次</param>
		/// <returns>时间</returns>
		public static string Order2Time(int order)
		{
			int nowtick = BASETICK + BASEORDER + order;
			return TimeTransfer.GetTimeFromTick(nowtick.ToString());
		}

		public static string GetSafeString4File(string instr)
		{
			if(instr == null || instr.Trim() == "")
				return "";

			return instr.Replace("|","").Replace(",","").Replace(";","")
				.Replace("'","").Replace("\r","").Replace("\n","")
				.Replace("\\","").Replace("\t","").Replace("\"","");
		}
	}

}
