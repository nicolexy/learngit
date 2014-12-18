using System;

namespace TENCENT.OSS.CFT.KF.Common
{
	/// <summary>
	/// 金额转换。
	/// </summary>
	public class MoneyTransfer
	{
		public MoneyTransfer()
		{
		}

		public static string FenToYuan(string strfen)
		{
			if (strfen == "")
			{
				strfen = "0";
			}

			double yuan = (double)(Int64.Parse(strfen))/100;
			yuan = Math.Round(yuan,2);

			string tmp = yuan.ToString();
			int iindex = tmp.IndexOf(".");
			if(iindex == -1) tmp += ".00";
			if(iindex == tmp.Length - 2) tmp += "0";

			return tmp;
		}

		public static double FenToYuan(double inum)
		{
			double yuan = (double)(inum)/100;
			yuan = Math.Round(yuan,2);

			return yuan;
		}

		public static string YuanToFen(string stryuan)
		{
			if (stryuan == "")
			{
				stryuan = "0";
			}

			double yuan = Double.Parse(stryuan) * 100;

			int fen = (int)Math.Round(yuan);

			return fen.ToString();
		}

		public static int YuanToFen(double doublenum)
		{
			double yuan = doublenum * 100;

			int fen = (int)Math.Round(yuan);

			return fen;
		}

	}


	/// <summary>
	/// 时间转换。
	/// </summary>
	public class TimeTransfer
	{
		public TimeTransfer()
		{
		}

		public static string GetTimeFromTick(string tick)
		{
			if(tick == null || tick.Trim() == "0")
			{
				return "";
			}

			long ltick = 0;
			try
			{
				ltick = long.Parse(tick);
			}
			catch
			{
				return "";
			}

			return DateTime.Parse("1970-01-01 08:00:00").AddTicks(ltick * 10000000).ToString("yyyy-MM-dd HH:mm:ss");
		}

		public static string GetTickFromTime(string time)
		{
			if(time == null || time.Trim() == "")
			{
				return "";
			}

			long ltick = 0;
			try
			{
				ltick = DateTime.Parse(time).Ticks;
			}
			catch
			{
				return "";
			}

			ltick = ltick - DateTime.Parse("1970-01-01 08:00:00").Ticks;
			ltick = ltick / 10000000;
			return ltick.ToString();
		}

	}



}
