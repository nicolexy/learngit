using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using Tencent.DotNet.Common.UI;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;

namespace TENCENT.OSS.CFT.KF.KF_Web.FreezeManage
{
	/// <summary>
	/// FastReplyManagePage ��ժҪ˵����
	/// </summary>
	public partial class FastReplyManagePage : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��

			if(!ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");

			if(!IsPostBack)
			{
				this.lb_operatorID.Text = Session["uid"].ToString();
				BindData("0",false);
			}

			this.ddl_fastReplyBlock.SelectedIndexChanged += new EventHandler(ddl_fastReplyBlock_SelectedIndexChanged);
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
		/// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
		/// �˷��������ݡ�
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion

		private void BindData(string type,bool needUpdate)
		{
			try
			{
				string[] strReply;
				if(type == "0")
				{
					strReply = getData.GetFreezeFastReplay(this,needUpdate);
					if(strReply != null)
					{
						this.ddl_fastReplyContent.Items.Clear();
						foreach(string str in strReply)
						{
							if(str != null && str.Trim().Length > 0)
							{
								this.ddl_fastReplyContent.Items.Add(str);
							}
						}
					}
				}
				else
				{
					throw new Exception("δ֪������");
				}
			}
			catch(Exception ex)
			{
				WebUtils.ShowMessage(this,ex.Message);
			}
		}



		private void AddFastReply(string type)
		{
			try
			{
				if(type == "0")
				{
					if(getData.AddFreezeFastReplay(this,this.tbx_fastReplyContent.Text.Trim()))
					{
						WebUtils.ShowMessage(this,"��ӿ�ݻظ��ɹ�");

						this.ddl_fastReplyContent.Items.Add(this.tbx_fastReplyContent.Text.Trim());
					}
					else
					{
						WebUtils.ShowMessage(this,"��ӿ�ݻظ�ʧ��");
					}
				}
				else
				{
					throw new Exception("δ֪������");
				}
			}
			catch(Exception ex)
			{
				WebUtils.ShowMessage(this,ex.Message);
			}
		}



		private bool DeleteFastReply(string type,int index)
		{
			try
			{
				if(type == "0")
				{
					return getData.DeleteFreezeFastReply(this,index);
				}
			}
			catch(Exception ex)
			{
				WebUtils.ShowMessage(this,ex.Message);
			}

			return false;
		}


		protected void btn_addFastReply_Click(object sender, System.EventArgs e)
		{
			if(this.tbx_fastReplyContent.Text.Trim().Length == 0)
			{
				WebUtils.ShowMessage(this,"��������ӵ�����");
				return;
			}

			AddFastReply(this.ddl_fastReplyBlock.SelectedValue);
		}

		protected void btn_modifyFastReply_Click(object sender, System.EventArgs e)
		{
			if(this.tbx_fastReplyContent.Text.Trim().Length == 0)
			{
				WebUtils.ShowMessage(this,"�������޸ĵ�����");
				return;
			}

			if(getData.UpdateFreezeFastReply(this,this.tbx_fastReplyContent.Text.Trim(),this.ddl_fastReplyContent.SelectedIndex))
			{
				WebUtils.ShowMessage(this,"�޸Ļظ��ɹ�");
			}
			else
			{
				WebUtils.ShowMessage(this,"�޸Ļظ�ʧ��");
			}

			BindData(this.ddl_fastReplyBlock.SelectedValue,true);
		}

		protected void btn_deleteFastReply_Click(object sender, System.EventArgs e)
		{
			if(this.DeleteFastReply(this.ddl_fastReplyBlock.SelectedValue,this.ddl_fastReplyContent.SelectedIndex))
			{
				WebUtils.ShowMessage(this,"ɾ���ظ��ɹ�");
			}
			else
			{
				WebUtils.ShowMessage(this,"ɾ���ظ�ʧ��");
			}

			BindData(this.ddl_fastReplyBlock.SelectedValue,true);
		}

		private void ddl_fastReplyBlock_SelectedIndexChanged(object sender, EventArgs e)
		{
			BindData(this.ddl_fastReplyBlock.SelectedValue,true);
		}

		protected void btn_updateFastReply_Click(object sender, System.EventArgs e)
		{
			BindData(this.ddl_fastReplyBlock.SelectedValue,true);
		}
	}
}
