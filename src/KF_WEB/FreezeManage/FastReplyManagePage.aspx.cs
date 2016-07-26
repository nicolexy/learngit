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
	/// FastReplyManagePage 的摘要说明。
	/// </summary>
	public partial class FastReplyManagePage : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面

			if(!ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");

			if(!IsPostBack)
			{
				this.lb_operatorID.Text = Session["uid"].ToString();
				BindData("0",false);
			}

			this.ddl_fastReplyBlock.SelectedIndexChanged += new EventHandler(ddl_fastReplyBlock_SelectedIndexChanged);
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
		/// 设计器支持所需的方法 - 不要使用代码编辑器修改
		/// 此方法的内容。
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
					throw new Exception("未知的类型");
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
						WebUtils.ShowMessage(this,"添加快捷回复成功");

						this.ddl_fastReplyContent.Items.Add(this.tbx_fastReplyContent.Text.Trim());
					}
					else
					{
						WebUtils.ShowMessage(this,"添加快捷回复失败");
					}
				}
				else
				{
					throw new Exception("未知的类型");
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
				WebUtils.ShowMessage(this,"请输入添加的内容");
				return;
			}

			AddFastReply(this.ddl_fastReplyBlock.SelectedValue);
		}

		protected void btn_modifyFastReply_Click(object sender, System.EventArgs e)
		{
			if(this.tbx_fastReplyContent.Text.Trim().Length == 0)
			{
				WebUtils.ShowMessage(this,"请输入修改的内容");
				return;
			}

			if(getData.UpdateFreezeFastReply(this,this.tbx_fastReplyContent.Text.Trim(),this.ddl_fastReplyContent.SelectedIndex))
			{
				WebUtils.ShowMessage(this,"修改回复成功");
			}
			else
			{
				WebUtils.ShowMessage(this,"修改回复失败");
			}

			BindData(this.ddl_fastReplyBlock.SelectedValue,true);
		}

		protected void btn_deleteFastReply_Click(object sender, System.EventArgs e)
		{
			if(this.DeleteFastReply(this.ddl_fastReplyBlock.SelectedValue,this.ddl_fastReplyContent.SelectedIndex))
			{
				WebUtils.ShowMessage(this,"删除回复成功");
			}
			else
			{
				WebUtils.ShowMessage(this,"删除回复失败");
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
