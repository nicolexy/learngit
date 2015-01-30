using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace TENCENT.OSS.CFT.KF.KF_Web.classLibrary
{
	/// <summary>
	/// dateControl 的摘要说明。
	/// </summary>
	/// <summary>
	/// 用户自定义时间控件，取到年、月、日、时、分、秒
	/// </summary>
	[DefaultProperty("Text"),
	ToolboxData("<{0}:clsUserTime runat=server></{0}:clsUserTime>")]
	//INamingContainer为子控件提供了一个新的命名范围,保证子控件的ID唯一性
	public class clsUserTime : System.Web.UI.WebControls.WebControl, INamingContainer
	{
		private System.Web.UI.WebControls.DropDownList ddlYear;
		private DropDownList ddlMonth;
		private DropDownList ddlDay;
		private DropDownList ddlHour;
		private DropDownList ddlMinite;
		private DropDownList ddlSecond;

		private static int beginYear = 0; //起始年号，这里必须使用static以防二次调用后变量被清零
		private static bool IsInit = false; //是否已经初始化过，这里必须使用static以防二次调用后变量被清零

		public int Year //取控件时间：年
		{
			get
			{
				return ddlYear.SelectedIndex + beginYear;
			}
		}

		public int Month //取控件时间：月
		{
			get
			{
				return ddlMonth.SelectedIndex + 1;
			}
		}

		public int Day //取控件时间：日
		{
			get
			{
				return ddlDay.SelectedIndex + 1;
			}
		}

		public int Hour //取控件时间：时
		{
			get
			{
				return ddlHour.SelectedIndex;
			}
		}

		public int Minite //取控件时间：分
		{
			get
			{
				return ddlMinite.SelectedIndex;
			}
		}

		public int Second //取控件时间：秒
		{
			get
			{
				return ddlSecond.SelectedIndex;
			}
		}

		//用起始年号初始化控件，如果已经初始化过控件，直接返回
		public void InitControls(int startYear, int endYear)
		{
			if (IsInit)
			{
				return;
			}
			EnsureChildControls();
			if (startYear > endYear)
			{
				int tmp = startYear;
				startYear = endYear;
				endYear = startYear;
			}
			beginYear = startYear;
			for (int i = startYear; i <= endYear; ++i)
			{
				ddlYear.Items.Add(i.ToString());
			}
			for (int i = 1; i <=12; ++i)
			{
				ddlMonth.Items.Add(i.ToString());
			}
			for (int i = 1; i <= 31; ++i)
			{
				ddlDay.Items.Add(i.ToString());
			}
			for (int i = 0; i < 24; ++i)
			{
				ddlHour.Items.Add(i.ToString());
			}
			for (int i = 0; i < 60; ++i)
			{
				ddlMinite.Items.Add(i.ToString());
				ddlSecond.Items.Add(i.ToString());
			}
			IsInit = true;
		}
		//重载Controls属性，取得ControlCollection对象，该对象表示UI层次结构中的服务器控件的子控件
		public override ControlCollection Controls
		{
			get
			{
				//确定服务器是否包含子控件
				EnsureChildControls();
				return base.Controls;
			}
		}

		//动态创建子控件
		protected override void CreateChildControls()
		{
			//创建年、月、日、时、分、秒下拉列表框
			this.Controls.Clear();
			ddlYear = new DropDownList();
			ddlYear.ID = "ddlYear";
			ddlYear.Width = 54;

			ddlMonth = new DropDownList();
			ddlMonth.ID = "ddlMonth";
			ddlMonth.Width = 40;

			ddlDay = new DropDownList();
			ddlDay.ID = "ddlDay";
			ddlDay.Width = 40;

			ddlHour = new DropDownList();
			ddlHour.ID = "ddlHour";
			ddlHour.Width = 40;

			ddlMinite = new DropDownList();
			ddlMinite.ID = "ddlMinite";
			ddlMinite.Width = 40;

			ddlSecond = new DropDownList();
			ddlSecond.ID = "ddlSecond";
			ddlSecond.Width = 40;

			this.Controls.Add(ddlYear);
			this.Controls.Add(ddlMonth);
			this.Controls.Add(ddlDay);
			this.Controls.Add(ddlHour);
			this.Controls.Add(ddlMinite);
			this.Controls.Add(ddlSecond);

			base.CreateChildControls ();
		}

		/// <summary>
		/// 将此控件呈现给指定的输出参数。
		/// </summary>
		/// <param name="output"> 要写出到的 HTML 编写器 </param>
		protected override void Render(HtmlTextWriter output)
		{
			//将要呈现的HTML属性添加到指定的System.Web.HtmlTextWriter中
			AddAttributesToRender(output);
			output.AddAttribute(System.Web.UI.HtmlTextWriterAttribute.Cellpadding, "0", false);

			//制定一个表，把子控件排在一行上
			output.RenderBeginTag(HtmlTextWriterTag.Table); //表
			output.RenderBeginTag(HtmlTextWriterTag.Tr); //行

			//年下拉列表框
			output.RenderBeginTag(HtmlTextWriterTag.Td); //列
			ddlYear.RenderControl(output);
			output.RenderEndTag();

			output.RenderBeginTag(HtmlTextWriterTag.Td);
			output.Write("年");
			output.RenderEndTag();

			//月下拉列表框
			output.RenderBeginTag(HtmlTextWriterTag.Td);
			ddlMonth.RenderControl(output);
			output.RenderEndTag();

			output.RenderBeginTag(HtmlTextWriterTag.Td);
			output.Write("月");
			output.RenderEndTag();

			//日下拉列表框
			output.RenderBeginTag(HtmlTextWriterTag.Td);
			ddlDay.RenderControl(output);
			output.RenderEndTag();

			output.RenderBeginTag(HtmlTextWriterTag.Td);
			output.Write("日");
			output.RenderEndTag();

			//时下拉列表框
			output.RenderBeginTag(HtmlTextWriterTag.Td);
			ddlHour.RenderControl(output);
			output.RenderEndTag();

			output.RenderBeginTag(HtmlTextWriterTag.Td);
			output.Write("时");
			output.RenderEndTag();

			//分下拉列表框
			output.RenderBeginTag(HtmlTextWriterTag.Td);
			ddlMinite.RenderControl(output);
			output.RenderEndTag();

			output.RenderBeginTag(HtmlTextWriterTag.Td);
			output.Write("分");
			output.RenderEndTag();

			//秒下拉列表框
			output.RenderBeginTag(HtmlTextWriterTag.Td);
			ddlSecond.RenderControl(output);
			output.RenderEndTag();

			output.RenderBeginTag(HtmlTextWriterTag.Td);
			output.Write("秒");
			output.RenderEndTag();

			output.RenderEndTag();
			output.RenderEndTag();
		}

		public DateTime GetTime() //取得选定的时间
		{
			DateTime time = new DateTime(this.Year, this.Month, this.Day, this.Hour, this.Minite, this.Second, 0);
			return time;
		}

		//清除控件里所有下拉列表框的值
		public void ClearControls()
		{
			ddlYear.Items.Clear();
			ddlMonth.Items.Clear();
			ddlDay.Items.Clear();
			ddlHour.Items.Clear();
			ddlMinite.Items.Clear();
			ddlSecond.Items.Clear();
			IsInit = false;
		}

		//撤消对所有下拉列表框的选项
		public void ClearSelection()
		{
			ddlYear.ClearSelection();
			ddlMonth.ClearSelection();
			ddlDay.ClearSelection();
			ddlHour.ClearSelection();
			ddlMinite.ClearSelection();
			ddlSecond.ClearSelection();
		}
	}
}