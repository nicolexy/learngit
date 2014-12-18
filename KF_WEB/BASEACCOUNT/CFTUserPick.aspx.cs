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
using TENCENT.OSS.CFT.KF.DataAccess;
using System.Web.Services.Protocols;

using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using TENCENT.OSS.CFT.KF.Common;
using System.IO;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
	/// <summary>
	/// CFTUserPick ��ժҪ˵����
	/// </summary>
	public partial class CFTUserPick : System.Web.UI.Page
	{








































	
		private string type
		{
			get
			{
				if(ViewState["type"] != null)
					return ViewState["type"].ToString();
				else
					return "old";
			}

			set
			{
				ViewState["type"] = value.Trim();
			}
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				Label1.Text = Session["uid"].ToString();

				string szkey = Session["SzKey"].ToString();
//				int operid = Int32.Parse(Session["OperID"].ToString());

				//if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"CFTUserPick")) Response.Redirect("../login.aspx?wh=1");				if(!classLibrary.ClassLib.ValidateRight("CFTUserPick",this)) Response.Redirect("../login.aspx?wh=1");
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}

			if(!IsPostBack)
			{
				tbSumDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

				Image1.Attributes.Add("onclick","ShowImg(test,imgid,this,movediv)");
				Image1.Attributes.Add("onmouseover","ShowMoveImg(movediv,moveimgid,this,test)");
				Image1.Attributes.Add("onmouseout","HiddenImg(movediv)");

				Image2.Attributes.Add("onclick","ShowImg(test,imgid,this,movediv)");
				Image2.Attributes.Add("onmouseover","ShowMoveImg(movediv,moveimgid,this,test)");
				Image2.Attributes.Add("onmouseout","HiddenImg(movediv)");

				Image3.Attributes.Add("onclick","ShowImg(test,imgid,this,movediv)");
				Image3.Attributes.Add("onmouseover","ShowMoveImg(movediv,moveimgid,this,test)");
				Image3.Attributes.Add("onmouseout","HiddenImg(movediv)");

				Image4.Attributes.Add("onclick","ShowImg(test,imgid,this,movediv)");
				Image4.Attributes.Add("onmouseover","ShowMoveImg(movediv,moveimgid,this,test)");
				Image4.Attributes.Add("onmouseout","HiddenImg(movediv)");

				if(Request.QueryString["type"] != null && Request.QueryString["type"].Trim() == "new")
				{
					//��ȡ�µ���
					type = "new";
					PickData(1);
				}

				if(type == "new")
				{
					btnPick.Visible = false;
					hlNewORder.Target = "_self";
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

		protected void btnPick_Click(object sender, System.EventArgs e)
		{
			//�쵥,��ȡ���ſ��õ���.
			PickData(0);
		}

		private void InitPage()
		{
			labcardtype4.Text = "";
			labid4.Text = "";
			Image4.ImageUrl = "#";
			tbidcard4.Text = "";
			rblist4.SelectedIndex = 0;

			labcardtype3.Text = "";
			labid3.Text = "";
			Image3.ImageUrl = "#";
			tbidcard3.Text = "";
			rblist3.SelectedIndex = 0;

			labcardtype2.Text = "";
			labid2.Text = "";
			Image2.ImageUrl = "#";
			tbidcard2.Text = "";
			rblist2.SelectedIndex = 0;

			labcardtype1.Text = "";
			labid1.Text = "";
			Image1.ImageUrl = "#";
			tbidcard1.Text = "";
			rblist1.SelectedIndex = 0;
		}

		private void PickData(int flag)
		{/* ҳ��ϳ� begin
			try
			{
				//Random ram = new Random();
				int max = 4;
				string fuin = Session["uid"].ToString().Trim();

				Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

				Finance_Header fh = new Finance_Header();
				fh.UserIP = Request.UserHostAddress;
				fh.UserName = Session["uid"].ToString();

				fh.OperID = Int32.Parse(Session["OperID"].ToString());
				fh.SzKey = Session["SzKey"].ToString();

				qs.Finance_HeaderValue = fh;

				DataSet ds = qs.GetCFTUserPickList(fuin,max,flag);
				if(ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count == 0)
				{
					WebUtils.ShowMessage(this.Page,"û���쵽���ߵ�,�����޵����߶�ȡ�д�");
					return;
				}

				
				string url = System.Configuration.ConfigurationManager.AppSettings["AppealUrlPath"].Trim();

				if(!url.EndsWith("/"))
					url += "/";

				if(ds.Tables[0].Rows.Count >= 4)
				{
					DataRow dr = ds.Tables[0].Rows[3];
					labcardtype4.Text = GetCreType(dr["cre_type"].ToString());
					labid4.Text = dr["FID"].ToString();

					//���image�ڱ���
					string imagestr = dr["cre_image"].ToString().Trim();
					Image4.ImageUrl =  url + imagestr;
				}

				if(ds.Tables[0].Rows.Count >= 3)
				{
					DataRow dr = ds.Tables[0].Rows[2];
					labcardtype3.Text = GetCreType(dr["cre_type"].ToString());
					labid3.Text = dr["FID"].ToString();

					//���image�ڱ���
					string imagestr = dr["cre_image"].ToString().Trim();
					Image3.ImageUrl =  url + imagestr;
				}

				if(ds.Tables[0].Rows.Count >= 2)
				{
					DataRow dr = ds.Tables[0].Rows[1];
					labcardtype2.Text = GetCreType(dr["cre_type"].ToString());
					labid2.Text = dr["FID"].ToString();

					//���image�ڱ���
					string imagestr = dr["cre_image"].ToString().Trim();
					Image2.ImageUrl =  url + imagestr;
				}

				if(ds.Tables[0].Rows.Count >= 1)
				{
					DataRow dr = ds.Tables[0].Rows[0];
					labcardtype1.Text = GetCreType(dr["cre_type"].ToString());
					labid1.Text = dr["FID"].ToString();

					//���image�ڱ���
					string imagestr = dr["cre_image"].ToString().Trim();
					Image1.ImageUrl =  url + imagestr;
				}

				
			}
			catch(Exception err)
			{
				WebUtils.ShowMessage(this.Page,"�쵥ʱ����,�����Ի�֪ͨ����Ա:" + PublicRes.GetErrorMsg(err.Message));
			} ҳ��ϳ� end */
		}

		private void WriteImgFile(string imagestr,string filename)
		{
			byte[] imagebyte = Convert.FromBase64String(imagestr);
			string filepath = Server.MapPath(Request.ApplicationPath) + "\\BaseAccount\\" + filename;

			if(File.Exists(filepath))
			{
				File.Delete(filepath);
			}

			FileStream fs = new FileStream(filepath,FileMode.Create);

			try
			{
				fs.Write(imagebyte,0,imagebyte.Length);
			}
			finally
			{
				fs.Close();
			}
		}

		private string GetCreType(string creid)
		{
	
			if(creid == null || creid.Trim() == "")
				return "δָ������";

			int icreid = 0;
			try
			{
				icreid = Int32.Parse(creid);
			}
			catch
			{
				return "����ȷ����" + creid;
			}

			if(icreid >=1 && icreid <= 11)
			{
				if(icreid == 1)
				{
					return "���֤";
				}
				else if(icreid == 2)
				{
					return "����";
				}
				else if(icreid == 3)
				{
					return "����֤";
				}
				else if(icreid == 4)
				{
					return "ʿ��֤";
				}
				else if(icreid == 5)
				{
					return "����֤";
				}
				else if(icreid == 6)
				{
					return "��ʱ���֤";
				}
				else if(icreid == 7)
				{
					return "���ڲ�";
				}
				else if(icreid == 8)
				{
					return "����֤";
				}
				else if(icreid == 9)
				{
					return "̨��֤";
				}
				else if(icreid == 10)
				{
					return "Ӫҵִ��";
				}
				else if(icreid == 11)
				{
					return "����֤��";
				}
				else
				{
					return "����ȷ����" + creid;
				}
			}
			else
			{
				return "����ȷ����" + creid;
			}
		}

		protected void btnCommit_Click(object sender, System.EventArgs e)
		{/* ҳ��ϳ� begin
			//���������ύ.
			//����֤�����Ƿ���ȷ.
			if(labid1.Text != "" && rblist1.SelectedIndex < 0 && tbidcard1.Text.Trim().Length != 5)
			{
				WebUtils.ShowMessage(this.Page,"������֤���������λ");
				return;
			}

			if(labid2.Text != "" && rblist2.SelectedIndex < 0 && tbidcard2.Text.Trim().Length != 5)
			{
				WebUtils.ShowMessage(this.Page,"������֤���������λ");
				return;
			}

			if(labid3.Text != "" && rblist3.SelectedIndex < 0 && tbidcard3.Text.Trim().Length != 5)
			{
				WebUtils.ShowMessage(this.Page,"������֤���������λ");
				return;
			}

			if(labid4.Text != "" && rblist4.SelectedIndex < 0 && tbidcard4.Text.Trim().Length != 5)
			{
				WebUtils.ShowMessage(this.Page,"������֤���������λ");
				return;
			}

			//��֤���ύ��̨.
			try
			{
				string fuin = Session["uid"].ToString().Trim();

				Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

				Finance_Header fh = new Finance_Header();
				fh.UserIP = Request.UserHostAddress;
				fh.UserName = fuin;

				fh.OperID = Int32.Parse(Session["OperID"].ToString());
				fh.SzKey = Session["SzKey"].ToString();

				qs.Finance_HeaderValue = fh;

				//��ϲ�����
				int imaxlen = 0;
				if(labid4.Text != "")
					imaxlen = 4;
				else if(labid3.Text != "")
					imaxlen = 3;
				else if(labid2.Text != "")
					imaxlen = 2;
				else if(labid1.Text != "")
					imaxlen = 1;

				if(imaxlen == 0)
				{
					WebUtils.ShowMessage(this.Page,"û�п����ύ�����߽��");
					return;
				}

				string[] result = new string[imaxlen];
				if(imaxlen >= 1)
				{
					string strresult1 = labid1.Text.Trim() + ";" + rblist1.SelectedIndex + ";" 
						+ tbidcard1.Text.Trim().Replace("'","\\'").Replace(";","");
					result[0] = strresult1;
				}

				if(imaxlen >= 2)
				{
					string strresult2 = labid2.Text.Trim() + ";" + rblist2.SelectedIndex + ";" 
						+ tbidcard2.Text.Trim().Replace("'","\\'").Replace(";","");
					result[1] = strresult2;
				}

				if(imaxlen >= 3)
				{
					string strresult3 = labid3.Text.Trim() + ";" + rblist3.SelectedIndex + ";" 
						+ tbidcard3.Text.Trim().Replace("'","\\'").Replace(";","");
					result[2] = strresult3;
				}

				if(imaxlen >= 4)
				{
					string strresult4 = labid4.Text.Trim() + ";" + rblist4.SelectedIndex + ";" 
						+ tbidcard4.Text.Trim().Replace("'","\\'").Replace(";","");
					result[3] = strresult4;
				}

				string msg = "";
				if(qs.GetCFTUserCommit(result,out msg))
				{
					if(msg != "")
						throw new Exception(msg);
					InitPage();
				}
				else
				{
					InitPage();
					WebUtils.ShowMessage(this.Page,msg);
				}

				//�ٴ��쵥��
				//PickData();
				if(type == "new")
				{
					PickData(1);
				}
			}
			catch(Exception err)
			{
				string msg = PublicRes.GetErrorMsg(err.Message);
				WebUtils.ShowMessage(this.Page,msg);
			}			 ҳ��ϳ� end */
		}

		
		protected void btnSum_Click(object sender, System.EventArgs e)
		{/* ҳ��ϳ� begin
			//ͳ�Ƹ�����Ϣ
			try
			{
				string fuin = Session["uid"].ToString().Trim();

				Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

				Finance_Header fh = new Finance_Header();
				fh.UserIP = Request.UserHostAddress;
				fh.UserName = fuin;

				fh.OperID = Int32.Parse(Session["OperID"].ToString());
				fh.SzKey = Session["SzKey"].ToString();

				qs.Finance_HeaderValue = fh;

				DateTime begindate = DateTime.Parse(DateTime.Parse(tbSumDate.Text.Trim()).ToString("yyyy-MM-dd 00:00:00"));
				DateTime enddate = DateTime.Parse(DateTime.Parse(tbSumDate.Text.Trim()).ToString("yyyy-MM-dd 23:59:59"));

				string msg = "";
				if(qs.GetAppealUserSumInfo(fuin,begindate,enddate,out msg))
				{
					labSumInfo.Text = msg;
				}
				else
				{
					labSumInfo.Text = msg;
				}
			}
			catch(Exception err)
			{
				string msg = PublicRes.GetErrorMsg(err.Message);
				WebUtils.ShowMessage(this.Page,msg);
			}			 ҳ��ϳ� end */
		}

	}
}
