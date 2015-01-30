using System;

namespace Tencent.CFT.LimitManage
{
	/// <summary>
	/// 封装权限相关函数,方便各系统使用及改写方便。
	/// </summary>
	public class LimitCommonLib
	{
		private string fconnstring = "";

		/// <summary>
		/// 给定本人权限串,校验是否有iRight的权限.
		/// ***OneRight.ValidRight改写成调用此函数***
		/// </summary>
		/// <param name="strRight">权限串</param>
		/// <param name="iRight">要校验的权限ID</param>
		/// <returns>是否有此权限</returns>
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

			//原版本严重有问题
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
