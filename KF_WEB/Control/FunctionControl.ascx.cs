namespace TENCENT.OSS.CFT.KF.KF_Web.Control
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		FunctionControl ��ժҪ˵����
	/// </summary>
	public partial class FunctionControl : System.Web.UI.UserControl
	{

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
		}

		#region Web ������������ɵĴ���
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: �õ����� ASP.NET Web ���������������ġ�
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///		�����֧������ķ��� - ��Ҫʹ�ô���༭��
		///		�޸Ĵ˷��������ݡ�
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
					
					if(check=="1")//��ʾ��ѡ��
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
