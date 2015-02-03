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
	/// DKAdjust ��ժҪ˵����
	/// </summary>
	public partial class DKAdjust : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
			if(!IsPostBack)
			{
				//�����ǵ������������鿴���黹�ǰ��ļ����룬����ť����
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

					//RadioButtonList1.Enabled = true; //�ݲ��������Ϊ�ɹ�
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

				//��ʼ�����ݣ���������ѡ���ǵ����ݣ����л���չ�֣�
				if(querytype != "fileselect")
				{
					string batchid = Request.QueryString["batchid"];
					ViewState["batchid"] = batchid;

					BindBatchInfo();

					//����������鿴��չ�ֳ�ԭ�򣬸������������͡�
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
			//��ѯ����ԭ��͸���URL
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

		protected void btnFile_Click(object sender, System.EventArgs e)
        {
            string xlsPath = "";
            //�����ϴ��ļ�������
            labError.Text = "";
            try
            {
                if (uploadFile.PostedFile.FileName.Trim() == "")
                    return;

                //�ϴ��ļ���ʼ
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

                SunLibrary.LoggerFactory.Get("kf_web DKAdjust:").Info("after xlsPath��" + xlsPath);

                string requestUrl = System.Configuration.ConfigurationManager.AppSettings["GetImageFromKf2Url"].ToString();

                SunLibrary.LoggerFactory.Get("kf_web DKAdjust:").Info("after requestUrl��" + requestUrl);

                string url = requestUrl + "/"
                    + "uploadfile" + "/" + System.DateTime.Now.ToString("yyyyMMdd") + "/CSOMS/"
                    + "DKFile" + "/"
                    + filename;

                SunLibrary.LoggerFactory.Get("kf_web DKAdjust:").Info("after url��" + url);

                string targetPath = src + "uploadfile\\"
                    + System.DateTime.Now.ToString("yyyyMMdd") + "\\CSOMS\\DKFile";
                PublicRes.CreateDirectory(targetPath);

                SunLibrary.LoggerFactory.Get("kf_web DKAdjust:").Info("createFolder success targetPath��" + targetPath);

                uploadFile.PostedFile.SaveAs(xlsPath);

                SunLibrary.LoggerFactory.Get("kf_web DKAdjust:").Info("upFile success");
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "�ϴ��ļ��쳣��" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
                return;
            }

			//��web����������ļ��ɣ�Ȼ���ٰѽ��������������
			string batchid = "";
			StreamReader sr = new StreamReader(xlsPath,Encoding.GetEncoding("gb2312"));
			try
			{
				ArrayList al = new ArrayList();
				while(sr.Peek()>0)
				{
					//�����ļ��ṹ����֪�����ж˶����ṩʲô�ֶΣ���������ݲ����õģ����ȶ������Ҫ�ء����ֺŷָ���
					//���׵���;�����˺�;���;״̬������ֻ��ʧ�ܣ�;ʧ��ԭ��
					string strlen = sr.ReadLine().Trim();
					if(strlen == "" || strlen.StartsWith("���׵���"))
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
					//���ļ������γɹ�
					//����Ϣ�ŵ�ͨ�ú�����
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
			
			

			//�ɹ��Ļ�������ʾlabel�󶨲����� 
			ViewState["batchid"] = batchid;
			//RadioButtonList1.SelectedValue = "";//�ݲ�֧�ֳɹ���ֻ��ʹ�õ���ʧ��


			//�ɹ�����а�ť����
			labInputfile.Enabled =false;
			//uploadFile.Enabled = false;
			btnFile.Enabled = false;

			//�����ļ��м�¼״̬ѡ��������͡�
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
			//�ٽ��а�ť��ʼ����
			string querytype = ViewState["querytype"].ToString();
			if(querytype == "adjust")
			{
				labInputfile.Visible =false;
				uploadFile.Visible = false;
				btnFile.Visible = false;

				//RadioButtonList1.Enabled = true; //�ݲ�֧�ֳɹ���ֻ��ʹ�õ���ʧ��
				tbReason.Enabled = true;
				tbReason.Text = "";

				uploadImg.Visible = true;
				HyperLink1.Visible = false;

				bt_ok.Visible = true;
				btn_cancel.Visible = true;
			}
			else if(querytype == "fileselect")
			{
				//�Ƚ������������
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
              
                //���ѡ������ļ������ļ��ϴ���ȥ��

                if (uploadImg.PostedFile.FileName.Trim() != "")
                {
                    string src = System.Configuration.ConfigurationManager.AppSettings["KFWebSrc"].ToString();

                    //�ϴ��ļ���ʼ
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
                WebUtils.ShowMessage(this.Page, "�ϴ��ļ��쳣��" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
                return;
            }

            try
            {
                //����service������������������ID,����URL�ύ��service���ύ������
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
                    labError.Text = "���������ɹ�";
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "���������쳣��" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
                return;
            }
		}
	}
}
