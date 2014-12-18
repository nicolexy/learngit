using System;

namespace TENCENT.OSS.CFT.KF.Common
{
	/// <summary>
	/// T0Transfer ��ժҪ˵����
	/// </summary>
	/// <summary>
	/// T+0������,���κź�ʱ��ȵ�ת��
	/// </summary>
	public class T0Transfer
	{
		const int BASETICK = 10000;
		const int BASEORDER = 48;

		/// <summary>
		/// ASC��ת��Ϊ����.
		/// </summary>
		/// <param name="ascstr">ASC��</param>
		/// <returns>����,����д�,����-1</returns>
		public static int Asc2Order(string ascstr)
		{
			if(ascstr == null || ascstr.Length > 1)
				return -1;

			int itmp = Convert.ToInt32(Convert.ToChar(ascstr));
			return itmp - BASEORDER;
		}

		/// <summary>
		/// ����ת��ΪASC��
		/// </summary>
		/// <param name="order">����</param>
		/// <returns>ASC��</returns>
		public static string Order2Asc(int order)
		{
			order += BASEORDER;

			if(order < 49)
				order = 49;

			if(order > 122)
				return null;

			//�ܿ� ? ' - \ ; �ַ�
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
		/// ����ת��Ϊʱ��
		/// </summary>
		/// <param name="order">����</param>
		/// <returns>ʱ��</returns>
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
