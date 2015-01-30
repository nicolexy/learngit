namespace TENCENT.OSS.CFT.KF.KF_Web.Control
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		FunctionControl 的摘要说明。
	/// </summary>
	public partial class FunctionControl : System.Web.UI.UserControl
	{

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面
		}

		#region Web 窗体设计器生成的代码
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: 该调用是 ASP.NET Web 窗体设计器所必需的。
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///		设计器支持所需的方法 - 不要使用代码编辑器
		///		修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{

		}
		#endregion

		public void SetFunCheck(string funstr)
		{
			if(funstr!=null&&funstr!="")
			{
				int count=FunPanel.Controls.Count;
				for(int i=0;i<funstr.Length;i++)
				{
					string check=funstr[i].ToString();
					
					if(check=="1")//表示被选中
					{
						for(int j=0;j<count;j++)
						{
							if(FunPanel.Controls[j].GetType()==typeof(CheckBox))
							{
								string name=((CheckBox)FunPanel.Controls[j]).ID;
								string key=name.Substring(3);

								if(i.ToString()==key)
								{
									
									((CheckBox)FunPanel.Controls[j]).Checked=true;
									break;
									
								}
							
							}
						}
					}

				}
			}

			this.Fun0.Enabled = false;
			this.Fun1.Enabled = false;
			this.Fun2.Enabled = false;
			this.Fun3.Enabled = false;
			this.Fun4.Enabled = false;
			this.Fun5.Enabled = false;
			this.Fun6.Enabled = false;
			this.Fun7.Enabled = false;
			this.Fun8.Enabled = false;
			this.Fun9.Enabled = false;
			this.Fun10.Enabled = false;
			this.Fun11.Enabled = false;
			this.Fun12.Enabled = false;
			this.Fun13.Enabled = false;
			this.Fun14.Enabled = false;
			this.Fun15.Enabled = false;
			this.Fun16.Enabled = false;
			this.Fun17.Enabled = false;
			this.Fun18.Enabled = false;
			this.Fun19.Enabled = false;
		
		}


		public void InitializeCheck()
		{
			int count=FunPanel.Controls.Count;
			for(int i=0;i<count;i++)
			{
				if(FunPanel.Controls[i].GetType()==typeof(CheckBox))
				{
					((CheckBox)FunPanel.Controls[i]).Checked=false;
				}

			}

		}

		public void SetEnable(bool enable)
		{
			int count=FunPanel.Controls.Count;
			for(int i=0;i<count;i++)
			{
				if(FunPanel.Controls[i].GetType()==typeof(CheckBox))
				{
					((CheckBox)FunPanel.Controls[i]).Enabled=enable;
				}

			}

		}
	}
}
