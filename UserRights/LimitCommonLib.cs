using System;

namespace Tencent.CFT.LimitManage
{
	/// <summary>
	/// ��װȨ����غ���,�����ϵͳʹ�ü���д���㡣
	/// </summary>
	public class LimitCommonLib
	{
		private string fconnstring = "";

		/// <summary>
		/// ��������Ȩ�޴�,У���Ƿ���iRight��Ȩ��.
		/// ***OneRight.ValidRight��д�ɵ��ô˺���***
		/// </summary>
		/// <param name="strRight">Ȩ�޴�</param>
		/// <param name="iRight">ҪУ���Ȩ��ID</param>
		/// <returns>�Ƿ��д�Ȩ��</returns>
		public static bool ValidRight(string strRight,int iRight)
		{
			//return true;


			if(strRight == null)
				return false;

			string[] rights = strRight.Split('|');
			int index = (iRight-1) / 63;

			if(rights.Length <= index)
				return false;

			long lrights = long.Parse(rights[index]);
			
			string str = Convert.ToString(lrights,2);

			//ԭ�汾����������
			//index = iRight % 63;
			//return str[index] == '1';

			index = (iRight-1) % 63;

			char[] arr = str.ToCharArray();
			Array.Reverse(arr);
			string str_rev = new string(arr);

			return str_rev[index] == '1';
		}
	}
}
