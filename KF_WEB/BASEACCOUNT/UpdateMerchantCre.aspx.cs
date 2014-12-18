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

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
	/// <summary>
	/// UpdateMerchantCre 的摘要说明。
	/// </summary>
	public partial class UpdateMerchantCre : System.Web.UI.Page
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
		/// 设计器支持所需的方法 - 不要使用代码编辑器修改
		/// 此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion


		private string GetTime()
		{
			return DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
		}


		private const int MAX_IAMGE_SIZE = 5 * 1024 * 1024;


		protected void Button_Update_Click(object sender, System.EventArgs e)
		{
			string spid = this.TX_SPID.Text.Trim();

			if(spid == "")
			{
				WebUtils.ShowMessage(this,"请输入商户号");
				return;
			}

			if(this.txbNewCreid.Text.Trim() == "" || this.txtOldCreid.Text.Trim() == "" || this.txReason.Text.Trim() == "")
			{
				WebUtils.ShowMessage(this,"请输入必填信息");
				return;
			}

			string file1 = this.File1.Value;
			string file2 = this.File2.Value;
			string typeName1 = file1.Substring(file1.Length-4,4);
			string typeName2 = file2.Substring(file2.Length-4,4);

			try
			{
				if(typeName1 != ".jpg" && typeName1 != ".bmp" && typeName1 != ".gif")
				{
					WebUtils.ShowMessage(this,"商户证件图片类型只能为jpg,bmp或gif");
					return;
				}

				if(typeName2 != ".jpg" && typeName2 != ".bmp" && typeName2 != ".gif")
				{
					WebUtils.ShowMessage(this,"商户营业执照图片类型只能为jpg,bmp或gif");
					return;
				}

				if(File1.PostedFile.ContentLength > MAX_IAMGE_SIZE)
				{
					WebUtils.ShowMessage(this,"上传的证件图片大小不能大于5M");
					return;
				}

				if(File2.PostedFile.ContentLength > MAX_IAMGE_SIZE)
				{
					WebUtils.ShowMessage(this,"上传的营业执照图片大小不能大于5M");
					return;
				}

				if(File1.Value != "" && File2.Value != "")
				{
					string md5value = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(spid + GetTime(),"md5").ToLower();
					string fileSaveName1 = spid + "_KF_ID_ID_" + md5value + typeName1;
					string fileSaveName2 = spid + "_KF_ID_ZZ_" + md5value + typeName2;

					//string path = Server.MapPath(Request.ApplicationPath) + "\\uploadfile\\" + DateTime.Now.ToString("yyyyMMdd");
					string path = System.Configuration.ConfigurationManager.AppSettings["AppealUrlPath"].Trim();

					if(!path.EndsWith("\\"))
						path += "\\";

					this.File1.PostedFile.SaveAs((path + fileSaveName1));
					this.File2.PostedFile.SaveAs((path + fileSaveName2));
				}
			}
			catch (System.Exception ex)
			{
				string errMsg = "上传文件失败！" + ex.Message.Replace("'","’");
				WebUtils.ShowMessage(this.Page,errMsg);	
				return;
			}

			
			// Do Save To DB

		}
	}
}
