using System;

namespace TENCENT.OSS.CFT.KF.KF_Web.UnitTest
{

	public delegate void EveryCheckFinHandler(object _param1,object _param2);

	/// <summary>
	/// ICheck ��ժҪ˵����
	/// </summary>
	public interface ICheck
	{
		bool DoAllCheck();

		bool DoCheck(int index);
	}
}
