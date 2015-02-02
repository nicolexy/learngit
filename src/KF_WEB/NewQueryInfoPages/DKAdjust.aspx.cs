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
using System.IO;
using System.Text;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using Tencent.DotNet.Common.UI;

namespace TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages
{
	/// <summary>
	/// DKAdjust 的摘要说明。
	/// </summary>
	public partial class DKAdjust : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面
			if(!IsPostBack)
			{
				//根据是调整还是审批查看详情还是按文件导入，做按钮调整
				string querytype = Request.QueryString["querytype"];
				ViewState["querytype"] = querytype;

				if(querytype == "check")
				{
					labInputfile.Visible =false;
					uploadFile.Visible = false;
					btnFile.Visible = false;

					RadioButtonList1.Enabled = false;
					tbReason.Enabled = false;

					uploadImg.Visible = false;
					HyperLink1.Visible = true;

					bt_ok.Visible = false;
					btn_cancel.Visible = false;
				}
				else if(querytype == "adjust")
				{
					labInputfile.Visible =false;
					uploadFile.Visible = false;
					btnFile.Visible = false;

					//RadioButtonList1.Enabled = true; //暂不允许调整为成功
					tbReason.Enabled = true;

					uploadImg.Visible = true;
					HyperLink1.Visible = false;

					bt_ok.Visible = true;
					btn_cancel.Visible = true;

					if(!ClassLib.ValidateRight("DKAdjust",this)) Response.Redirect("../login.aspx?wh=1");
				}
				else if(querytype == "fileselect")
				{
					labInputfile.Visible =true;
					uploadFile.Visible = true;
					btnFile.Visible = true;

					RadioButtonList1.Enabled = false;
					tbReason.Enabled = false;

					uploadImg.Visible = false;
					HyperLink1.Visible = false;

					bt_ok.Visible = false;
					btn_cancel.Visible = false;

					if(!ClassLib.ValidateRight("DKAdjust",this)) Response.Redirect("../login.aspx?wh=1");
				}

				//开始绑定数据，根据批次选择标记的数据，进行汇总展现，
				if(querytype != "fileselect")
				{
					string batchid = Request.QueryString["batchid"];
					ViewState["batchid"] = batchid;

					BindBatchInfo();

					//如果是审批查看，展现出原因，附件，调整类型。
					if(querytype == "check")
					{
						BindCheckInfo();
					}
				}				
			}
		}

		private void BindBatchInfo()
		{
			string batchid = ViewState["batchid"].ToString();
			Query_Service.Query_Service fs = new Query_Service.Query_Service();

			DataSet ds = fs.DK_QueryCheckBatchAllDetail(batchid);

			if(ds != null && ds.Tables.Count>0 && ds.Tables[0] != null)
			{
				DataGrid_QueryResult.DataSource = ds.Tables[0].DefaultView;
				DataGrid_QueryResult.DataBind();

				lab21.Text = batchid;
				lab22.Text = ds.Tables[0].Rows.Count.ToString();
				lab23.Text = fs.DK_QueryCheckBatchSumAmount(batchid).ToString();
			}
		}

		private void BindCheckInfo()
		{
			string batchid = ViewState["batchid"].ToString();
			//查询审批原因和附件URL
			Query_Service.Query_Service fs = new Query_Service.Query_Service();

			DataSet ds = fs.DK_QueryCheckInfo(batchid);

			if(ds != null && ds.Tables.Count>0 && ds.Tables[0] != null)
			{
				foreach(DataRow dr in ds.Tables[0].Rows)
				{
					if(dr["FKey"].ToString()=="imgurl")
					{
						HyperLink1.NavigateUrl =dr["FValue"].ToString().Trim();
					}	
					else if(dr["FKey"].ToString()=="checkmemo")
					{
						tbReason.Text =dr["FValue"].ToString().Trim();
					}	
				}
			}
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

		protected void btnFile_Click(object sender, System.EventArgs e)
        {
            string xlsPath = "";
            //进行上传文件操作，
            labError.Text = "";
            try
            {
                if (uploadFile.PostedFile.FileName.Trim() == "")
                    return;

                //上传文件开始
                string tmpfilename = uploadFile.PostedFile.FileName.Trim();

                string src = System.Configuration.ConfigurationManager.AppSettings["KFWebSrc"].ToString();

                DateTime NowTime = DateTime.Now;
                string[] fileext = tmpfilename.Trim().Split('.');
                string filename = NowTime.ToString("yyyyMMddHHmmss") + "_" + Session["uid"].ToString() + "." + fileext[fileext.Length - 1];

                SunLibrary.LoggerFactory.Get("kf_web DKAdjust:").Info("before xlsPath");

                xlsPath = src
                    + "uploadfile" + "\\" + System.DateTime.Now.ToString("yyyyMMdd") + "\\CSOMS\\"
                    + "DKFile" + "\\"
                    + filename;

                SunLibrary.LoggerFactory.Get("kf_web DKAdjust:").Info("after xlsPath：" + xlsPath);

                string requestUrl = System.Configuration.ConfigurationManager.AppSettings["GetImageFromKf2Url"].ToString();

                SunLibrary.LoggerFactory.Get("kf_web DKAdjust:").Info("after requestUrl：" + requestUrl);

                string url = requestUrl + "/"
                    + "uploadfile" + "/" + System.DateTime.Now.ToString("yyyyMMdd") + "/CSOMS/"
                    + "DKFile" + "/"
                    + filename;

                SunLibrary.LoggerFactory.Get("kf_web DKAdjust:").Info("after url：" + url);

                string targetPath = src + "uploadfile\\"
                    + System.DateTime.Now.ToString("yyyyMMdd") + "\\CSOMS\\DKFile";
                PublicRes.CreateDirectory(targetPath);

                SunLibrary.LoggerFactory.Get("kf_web DKAdjust:").Info("createFolder success targetPath：" + targetPath);

                uploadFile.PostedFile.SaveAs(xlsPath);

                SunLibrary.LoggerFactory.Get("kf_web DKAdjust:").Info("upFile success");
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "上传文件异常！" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
                return;
            }

			//在web层初步解析文件吧，然后再把结果传给服务器。
			string batchid = "";
			StreamReader sr = new StreamReader(xlsPath,Encoding.GetEncoding("gb2312"));
			try
			{
				ArrayList al = new ArrayList();
				while(sr.Peek()>0)
				{
					//定义文件结构。不知道银行端都能提供什么字段，这个功能暂不启用的，就先定义基本要素。（分号分隔）
					//交易单号;银行账号;金额;状态（现在只认失败）;失败原因
					string strlen = sr.ReadLine().Trim();
					if(strlen == "" || strlen.StartsWith("交易单号"))
						continue;

					al.Add(strlen);
				}

				object[] newal = new object[al.Count];
				al.CopyTo(newal,0);

				string msg = "";

				Query_Service.Query_Service fs = new Query_Service.Query_Service();
				Query_Service.Finance_Header Ffh = classLibrary.setConfig.setFH(this);
				fs.Finance_HeaderValue = Ffh;

				if(fs.DK_FileSelect(newal,Session["uid"].ToString(), out batchid, out msg))
				{
					//用文件打批次成功
					//绑定信息放到通用函数里
					ViewState["batchid"] = batchid;
					BindBatchInfo();
				}
				else
				{
					labError.Text = msg;
					return;
				}
			}
			catch(Exception err)
			{
				labError.Text = err.Message;
				return;
			}
			finally
			{
				sr.Close();
			}
			
			

			//成功的话进行显示label绑定操作。 
			ViewState["batchid"] = batchid;
			//RadioButtonList1.SelectedValue = "";//暂不支持成功，只能使用调整失败


			//成功后进行按钮设置
			labInputfile.Enabled =false;
			//uploadFile.Enabled = false;
			btnFile.Enabled = false;

			//根据文件中记录状态选择调整类型。
			RadioButtonList1.Enabled = false;
			
			tbReason.Enabled = true;

			uploadImg.Visible = true;
			HyperLink1.Visible = false;

			bt_ok.Visible = true;
			btn_cancel.Visible = true;
		}

		protected void btn_cancel_Click(object sender, System.EventArgs e)
		{
			labError.Text = "";
			//再进行按钮初始化。
			string querytype = ViewState["querytype"].ToString();
			if(querytype == "adjust")
			{
				labInputfile.Visible =false;
				uploadFile.Visible = false;
				btnFile.Visible = false;

				//RadioButtonList1.Enabled = true; //暂不支持成功，只能使用调整失败
				tbReason.Enabled = true;
				tbReason.Text = "";

				uploadImg.Visible = true;
				HyperLink1.Visible = false;

				bt_ok.Visible = true;
				btn_cancel.Visible = true;
			}
			else if(querytype == "fileselect")
			{
				//先进行清除操作。
				lab21.Text = "";
				lab22.Text = "";
				lab23.Text = "";
				tbReason.Text = "";
				DataGrid_QueryResult.Visible = false;

				labInputfile.Visible =true;
				uploadFile.Visible = true;
				btnFile.Visible = true;

				RadioButtonList1.Enabled = false;
				tbReason.Enabled = false;

				uploadImg.Visible = false;
				HyperLink1.Visible = false;

				bt_ok.Visible = false;
				btn_cancel.Visible = false;
			}
		}

		protected void bt_ok_Click(object sender, System.EventArgs e)
		{
            string batchid = ViewState["batchid"].ToString();
            string url = "#";
            try
            {
                labError.Text = "";
              
                //如果选择的有文件，把文件上传上去。

                if (uploadImg.PostedFile.FileName.Trim() != "")
                {
                    string src = System.Configuration.ConfigurationManager.AppSettings["KFWebSrc"].ToString();

                    //上传文件开始
                    string tmpfilename = uploadImg.PostedFile.FileName.Trim();

                    DateTime NowTime = DateTime.Now;
                    string[] fileext = tmpfilename.Trim().Split('.');
                    string filename = Session["uid"].ToString() + "_" + batchid + "." + fileext[fileext.Length - 1];

                    SunLibrary.LoggerFactory.Get("kf_web DKAdjust:").Info("before xlsPath");

                    string xlsPath = src
                        + "uploadfile" + "\\" + System.DateTime.Now.ToString("yyyyMMdd") + "\\CSOMS" + "\\"
                        + "DKFile" + "\\"
                        + filename;

                    SunLibrary.LoggerFactory.Get("kf_web DKAdjust:").Info("afer xlsPath:"+xlsPath);

                    string requestUrl = System.Configuration.ConfigurationManager.AppSettings["GetImageFromKf2Url"].ToString();

                    SunLibrary.LoggerFactory.Get("kf_web DKAdjust:").Info("afer requestUrl" + requestUrl);

                    url = requestUrl + "/"
                        + "uploadfile" + "/" + System.DateTime.Now.ToString("yyyyMMdd") + "/CSOMS/"
                        + "DKFile" + "/"
                        + filename;

                    SunLibrary.LoggerFactory.Get("kf_web DKAdjust:").Info("after url:" + url);

                    string targetPath = src + "uploadfile\\"
                                     + System.DateTime.Now.ToString("yyyyMMdd") + "\\CSOMS\\DKFile";
                    PublicRes.CreateDirectory(targetPath);

                    SunLibrary.LoggerFactory.Get("kf_web DKAdjust:").Info("createFolder success targetPath:" + targetPath);

                    uploadImg.PostedFile.SaveAs(xlsPath);

                    SunLibrary.LoggerFactory.Get("kf_web DKAdjust:").Info("upFile success");
                }
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "上传文件异常！" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
                return;
            }

            try
            {
                //调用service发起审批。参数批次ID,附件URL提交到service再提交审批。
                Query_Service.Query_Service fs = new Query_Service.Query_Service();
                Query_Service.Finance_Header Ffh = classLibrary.setConfig.setFH(this);
                fs.Finance_HeaderValue = Ffh;


                string checkurl = Request.Url.AbsoluteUri.Replace(Request.Url.Query, "") + "?querytype=check&batchid=" + batchid;
                string checkmemo = tbReason.Text.Trim();

                string msg = "";
                if (!fs.DK_StartCheck(batchid, url, checkurl, checkmemo, out msg)) //DKAdjustFail
                {
                    labError.Text = msg;
                }
                else
                    labError.Text = "发起审批成功";
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "发起审批异常！" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
                return;
            }
		}
	}
}
