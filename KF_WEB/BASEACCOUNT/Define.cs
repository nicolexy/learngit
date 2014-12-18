using System;
using System.Web.UI.WebControls;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.TokenCoin
{
	public enum GwqState
	{
		�ѷ��� = 1, �Ѽ�����Ȩ = 2, �Ѽ��� = 3, Ԥʹ�� = 4 , ������ = 5 , �Ѿܾ� = 6, ��ʹ��ȷ�� = 7, δʹ�ý��� = 8, ����ʹ�ú���� = 9
	};

	public enum GwqAdjust
	{
		���� = 1, ���� = 2
	};

	public enum CoinType
	{
		�ֽ�ȯ = 1 , �����ۿ�ȯ = 2 , �̶�����ۿ�ȯ = 3
	};

	public enum PubType
	{
		���мӼ��� = 1 , ���в����� = 2
	};

	public enum DonateType
	{
		���� = 1 , ������ = 2
	};

	public enum State
	{
		¼��ά�� = 0, ������ = 1 , �ɷ��� = 2 , �ܾ����� = 3 , �ѷ��� = 4 , �ѹ��� = 5
	};

	public enum ListState
	{
		���� = 1, ���� = 2, ���� = 3
	};

	public enum RollRequestType
	{
		���� = 31, ������Ȩ = 32, ����ɹ� = 33, ����ȯʹ�� = 34, Ԥʹ�ó��� = 35, ����ȯ�ܾ����� = 36, �ֹ���ֵ = 37, ����ȯ���в����� = 38, ���ֵ��� = 39, ����ȯʹ�óɹ� = 40, ʹ��ȷ�ϳ��� = 41, ���� = 75, ����ȯ�������� = 76
	};

	public enum RollListState
	{
		���� = 0, ������ = 1, ���� = 2
	};

	public enum RollAdjust
	{
		���� = 1, ���� = 2
	};

	public enum InState
	{
		δ���� = 0, ƥ������ = 1, �ѷ��� = 2, ������ = 9
	};

	public enum AcFlag
	{
		�� = 1 , �� = 2
	};

	/// <summary>
	/// Define ��ժҪ˵����
	/// </summary>
	public class Define
	{
		public Define()
		{
		}

		public static string FeeFormat(string cointype,string fee)
		{
			return FeeFormat(cointype,fee,false);
		}

		public static string FeeFormat(string cointype,string fee,bool toedit)
		{
			if ( cointype==((int)CoinType.�����ۿ�ȯ).ToString() )
				return ( toedit ? "" : "���֮" ) + fee;
			else
				return SunLibrary.Sun.MoneyFormat(fee);
		}

		public static bool CanImport(string state)
		{
			return state==((int)State.�ɷ���).ToString();
		}

		public static bool StateEnable(string state)
		{
			return state==((int)State.�ɷ���).ToString();
		}

		public static void InitDateTime(DropDownList ListHour,DropDownList ListMin,DropDownList ListSec)
		{
			ListHour.Items.Clear();
			ListMin.Items.Clear();
			ListSec.Items.Clear();
			string s;
			for (int i=0;i<24;i++)
			{
				s = (i<10?"0":"") + i.ToString();
				ListHour.Items.Add(new ListItem(s,s));
			}
			for (int i=0;i<60;i++)
			{
				s = (i<10?"0":"") + i.ToString();
				ListMin.Items.Add(new ListItem(s,s));
			}
			for (int i=0;i<60;i++)
			{
				s = (i<10?"0":"") + i.ToString();
				ListSec.Items.Add(new ListItem(s,s));
			}
			ListHour.SelectedIndex = 0;
			ListMin.SelectedIndex = 0;
			ListSec.SelectedIndex = 0;
		}

		public static void SetDateTime(string datetime,TextBox TextDate,DropDownList ListHour,DropDownList ListMin,DropDownList ListSec)
		{
			System.DateTime dt = System.DateTime.Now;
			try
			{
				dt = Convert.ToDateTime(datetime);
			}
			catch
			{
			}
			TextDate.Text = dt.ToString("yyyy-MM-dd");
			ListHour.SelectedValue = dt.ToString("HH");
			ListMin.SelectedValue = dt.ToString("mm");
			ListSec.SelectedValue = dt.ToString("ss");
		}

		public static string GetDateTime(TextBox TextDate,DropDownList ListHour,DropDownList ListMin,DropDownList ListSec)
		{
			return TextDate.Text+" "+ListHour.SelectedValue+":"+ListMin.SelectedValue+":"+ListSec.SelectedValue;
		}

	}
}
