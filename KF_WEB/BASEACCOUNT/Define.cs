using System;
using System.Web.UI.WebControls;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.TokenCoin
{
	public enum GwqState
	{
		已发行 = 1, 已激活授权 = 2, 已激活 = 3, 预使用 = 4 , 已赠送 = 5 , 已拒绝 = 6, 已使用确认 = 7, 未使用结算 = 8, 部分使用后结算 = 9
	};

	public enum GwqAdjust
	{
		正常 = 1, 调账 = 2
	};

	public enum CoinType
	{
		现金券 = 1 , 比例折扣券 = 2 , 固定金额折扣券 = 3
	};

	public enum PubType
	{
		发行加激活 = 1 , 发行不激活 = 2
	};

	public enum DonateType
	{
		允许 = 1 , 不允许 = 2
	};

	public enum State
	{
		录入维护 = 0, 待审批 = 1 , 可发行 = 2 , 拒绝发行 = 3 , 已发行 = 4 , 已过期 = 5
	};

	public enum ListState
	{
		正常 = 1, 作废 = 2, 过期 = 3
	};

	public enum RollRequestType
	{
		发行 = 31, 激活授权 = 32, 激活成功 = 33, 购物券使用 = 34, 预使用冲正 = 35, 购物券拒绝激活 = 36, 手工充值 = 37, 购物券发行并激活 = 38, 提现调账 = 39, 购物券使用成功 = 40, 使用确认冲正 = 41, 赠送 = 75, 购物券任意领用 = 76
	};

	public enum RollListState
	{
		正常 = 0, 被冲正 = 1, 冲正 = 2
	};

	public enum RollAdjust
	{
		正常 = 1, 调账 = 2
	};

	public enum InState
	{
		未处理 = 0, 匹配正常 = 1, 已发放 = 2, 已作废 = 9
	};

	public enum AcFlag
	{
		是 = 1 , 否 = 2
	};

	/// <summary>
	/// Define 的摘要说明。
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
			if ( cointype==((int)CoinType.比例折扣券).ToString() )
				return ( toedit ? "" : "万分之" ) + fee;
			else
				return SunLibrary.Sun.MoneyFormat(fee);
		}

		public static bool CanImport(string state)
		{
			return state==((int)State.可发行).ToString();
		}

		public static bool StateEnable(string state)
		{
			return state==((int)State.可发行).ToString();
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
