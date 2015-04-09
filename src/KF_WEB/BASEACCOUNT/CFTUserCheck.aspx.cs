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
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using TENCENT.OSS.CFT.KF.Common;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using System.Web.Services.Protocols;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using System.Net;
using System.IO;
using System.Text;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
	/// <summary>
	/// CFTUserCheck ��ժҪ˵����
	/// </summary>
	public partial class CFTUserCheck : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
			try
			{
				string szkey = Session["SzKey"].ToString();
				//int operid = Int32.Parse(Session["OperID"].ToString());

				//if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"InfoCenter")) Response.Redirect("../login.aspx?wh=1");

				if(!classLibrary.ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}

			if(!IsPostBack)
			{
				string fid,flist_id;
                string db, tb;
				try
				{
					fid = Request.QueryString["fid"].ToString();
					flist_id = Request.QueryString["flist_id"].ToString();
                    //20131107 lxl �ֿ�ֱ��������������
                    db = Request.QueryString["db"].ToString();
                    tb = Request.QueryString["tb"].ToString();
					ViewState["fid"] = fid;
					ViewState["flist_id"] = flist_id;
                    ViewState["db"] = db;
                    ViewState["tb"] = tb;
				}
				catch
				{
					WebUtils.ShowMessage(this.Page,"��������");
					return;
				}

				try
				{
                    if (fid != null && fid != "")
                    {
                        if (db == null || db == "" || tb == null || tb == "")
                            BindInfoFID(int.Parse(fid));
                        else//�������͵ķֿ��
                            BindInfoFIDDBTB(fid,db,tb);
                    }
                    else if (flist_id != null && flist_id != "")
                    {
                        BindInfoFListID(int.Parse(flist_id));
                    }
                    else
                    {
                        WebUtils.ShowMessage(this.Page, "��������");
                        return;
                    }
				}
				catch(LogicException err)
				{
					WebUtils.ShowMessage(this.Page,err.Message);
				}
				catch(SoapException eSoap) //����soap���쳣
				{
					string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
					WebUtils.ShowMessage(this.Page,"���÷������" + errStr);
				}
				catch(Exception eSys)
				{
					WebUtils.ShowMessage(this.Page,"��ȡ����ʧ�ܣ�" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
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


		private void BindInfoFID(int fid)
		{
			lblfid.Text = fid.ToString();

			Query_Service.Query_Service qs = new Query_Service.Query_Service();
            qs.Finance_HeaderValue = classLibrary.setConfig.setFH(this);
			DataSet ds =  qs.GetCFTUserAppealDetail(fid);

			if(ds != null && ds.Tables.Count >0 && ds.Tables[0].Rows.Count > 0 )
			{
				DataRow dr = ds.Tables[0].Rows[0];
				
				if(dr["FState"].ToString() == "0" || dr["FState"].ToString() == "3"
					|| dr["FState"].ToString() == "4" || dr["FState"].ToString() == "5"
					|| dr["FState"].ToString() == "6" || dr["FState"].ToString() == "8")
				{
					btOK.Visible = true;
					btSetRealName.Visible = true;
					btCancel.Visible = true;
					btnDel.Visible = true;

					btOK.Attributes["onClick"]= "return confirm('ȷ��Ҫִ�С�ͨ�����ߡ�������');";
					btSetRealName.Attributes["onClick"] = "return confirm('ȷ��ͨ�����߲���ʵ����')";
					btCancel.Attributes["onClick"]= "return confirm('ȷ��Ҫִ�С��ܾ����ߡ�������');";
					btnDel.Attributes["onClick"]= "return confirm('ȷ��Ҫִ�С�ɾ�����ߡ�������');";
				}
				else if(dr["FState"].ToString() == "2" || dr["FState"].ToString() == "9")
				{
					btOK.Visible = true;
					btSetRealName.Visible = true;
					if(dr["FState"].ToString() == "9")
					{
						this.btCancel.Visible = false;
						this.btnDel.Visible = false;
					}

					btOK.Attributes["onClick"]= "return confirm('ȷ��Ҫִ�С��������ͨ����������');";
					btSetRealName.Attributes["onClick"]= "return confirm('ȷ��Ҫִ�С��������ͨ����ʵ����������');";
				}

				//��ȡ���ݣ�����QQ�ź󣬾�Ҫ��ȡ��̨��Ϣ��
//				labFuin.Text = dr["Fuin"].ToString();

				labFTypeName.Text = dr["FTypeName"].ToString();
				cre_id.Text = dr["cre_id"].ToString();
				new_cre_id.Text = dr["new_cre_id"].ToString();

				cre_type.Text = GetCreType(dr["cre_type"].ToString());

				email.Text = dr["Femail"].ToString();

				labFstatename.Text = dr["FStateName"].ToString();

				if(dr["clear_pps"].ToString() == "1" && dr["FType"].ToString() == "1")
				{
					clear_pps.Text = "���";
				}
				else if (dr["FType"].ToString() == "1" && dr["clear_pps"].ToString() != "1")
				{
					clear_pps.Text = "�����";
				}
				else
				{
					clear_pps.Text = "";
				}

				tbReason.Text = dr["reason"].ToString();
				tbComment.Text = dr["Fcomment"].ToString();

				old_name.Text = dr["old_name"].ToString();
				new_name.Text = dr["new_name"].ToString();
				tbFCheckInfo.Text = PublicRes.GetString(dr["FCheckInfo"]);

				if(dr["FType"].ToString() == "3")
				{
					old_name.Text = dr["old_company"].ToString();
					new_name.Text = dr["new_company"].ToString();
				}

				labIsAnswer.Text = dr["labIsAnswer"].ToString();
				lblBindMobileUser.Text = dr["mobile_no"].ToString();
				lblBindMailUser.Text = dr["Femail"].ToString();
				lblstandard_score.Text = dr["standard_score"].ToString();
				lblscore.Text = dr["score"].ToString();
				lbldetail_score.Text = dr["detail_score"].ToString();
				lblrisk_result.Text = dr["risk_result"].ToString();
           //     lbauthenState.Text = dr["authenState"].ToString();//����ʵ����֤�ֶ�

				lblivrresult.Text = dr["FIVRResult"].ToString();

				string imagestr = dr["cre_image"].ToString().Trim();
				string url = System.Configuration.ConfigurationManager.AppSettings["AppealUrlPath"].Trim();

				if(!url.EndsWith("/"))
					url += "/";

				// Ŀǰ�Ƹ��ܽ����Ҫ����ͼƬ
                if (dr["FType"].ToString() == "0" || dr["FType"].ToString() == "9" || dr["FType"].ToString() == "10")
				{
					if(imagestr.IndexOf("|")>0)
					{
						string[] imgUrls = imagestr.Split('|');
						Image1.ImageUrl = url + imgUrls[0];
						Image2.ImageUrl = url + imgUrls[1];
					}
					else
					{
						Image1.ImageUrl =  url + imagestr;
					}
				}
				else
				{
					Image1.ImageUrl =  url + imagestr;

					//���������ֻ�ʱ�ټ���һ��ͼƬ
					string imagestr2 = dr["cre_image2"].ToString().Trim();
					Image2.ImageUrl =  url + imagestr2;
				}


				try
				{
					DataSet dsuser =  qs.GetAppealUserInfo(dr["Fuin"].ToString());
					if(dsuser == null || dsuser.Tables.Count == 0 || dsuser.Tables[0].Rows.Count != 1)
					{
						labFQQid.Text = "��ȡ��������" ;
						btOK.Visible = false;
						btSetRealName.Visible = false;
					}
					else
					{
						DataRow druser = dsuser.Tables[0].Rows[0];

						labFQQid.Text = druser["Fqqid"].ToString();
						labFbalance.Text = classLibrary.setConfig.FenToYuan(druser["FBalance"].ToString());
						labFCon.Text = classLibrary.setConfig.FenToYuan(druser["Fcon"].ToString());

						labFcre_type.Text = GetCreType(druser["Fcre_type"].ToString());

						labFcreid.Text = PublicRes.GetString(druser["Fcreid"]);
						labFEmail.Text = PublicRes.GetString(druser["FEmail"]);
						labFtruename.Text = PublicRes.GetString(druser["Ftruename"]);

						labFBankAcc.Text = PublicRes.GetString(druser["Fbankid"]);

						if(dr["FType"].ToString() == "3")
						{
							labFtruename.Text = PublicRes.GetString(druser["Fcompany_name"]);
						}
					}
				}
				catch(Exception err)
				{
					labFQQid.Text = "��ȡ��������" + PublicRes.GetErrorMsg(err.Message);
					btOK.Visible = false;
					btSetRealName.Visible = false;
				}

                qs.Finance_HeaderValue = classLibrary.setConfig.setFH(this);
                //2.��ѯʵ����֤
                DataSet dsA = qs.GetUserAuthenState(dr["Fuin"].ToString(),"",0);
                if (dsA == null || dsA.Tables.Count < 1 || dsA.Tables[0].Rows.Count != 1)
                {
                    lbauthenState.Text = "��";//�Ƿ�ʵ����֤
                }
                else
                {
                    DataRow row = dsA.Tables[0].Rows[0];
                    if (row["queryType"].ToString() == "2")
                    {
                        lbauthenState.Text = "��";
                    }
                    else
                    {
                        lbauthenState.Text = "��";
                    }
                }

			}
			else
			{
				throw new Exception("û���ҵ���¼��");
			}
		}

        private void BindInfoFIDDBTB(string fid,string db,string tb)
		{
			lblfid.Text = fid.ToString();

			Query_Service.Query_Service qs = new Query_Service.Query_Service();
            qs.Finance_HeaderValue = classLibrary.setConfig.setFH(this);
            DataSet ds = qs.GetCFTUserAppealDetailByDBTB(fid,db,tb);

			if(ds != null && ds.Tables.Count >0 && ds.Tables[0].Rows.Count > 0 )
			{
				DataRow dr = ds.Tables[0].Rows[0];
				
				if(dr["FState"].ToString() == "0" || dr["FState"].ToString() == "3"
					|| dr["FState"].ToString() == "4" || dr["FState"].ToString() == "5"
					|| dr["FState"].ToString() == "6" || dr["FState"].ToString() == "8")
				{
					btOK.Visible = true;
					btSetRealName.Visible = true;
					btCancel.Visible = true;
					btnDel.Visible = true;

					btOK.Attributes["onClick"]= "return confirm('ȷ��Ҫִ�С�ͨ�����ߡ�������');";
					btSetRealName.Attributes["onClick"] = "return confirm('ȷ��ͨ�����߲���ʵ����')";
					btCancel.Attributes["onClick"]= "return confirm('ȷ��Ҫִ�С��ܾ����ߡ�������');";
					btnDel.Attributes["onClick"]= "return confirm('ȷ��Ҫִ�С�ɾ�����ߡ�������');";
				}
				else if(dr["FState"].ToString() == "2" || dr["FState"].ToString() == "9")
				{
					btOK.Visible = true;
					btSetRealName.Visible = true;
					if(dr["FState"].ToString() == "9")
					{
						this.btCancel.Visible = false;
						this.btnDel.Visible = false;
					}

					btOK.Attributes["onClick"]= "return confirm('ȷ��Ҫִ�С��������ͨ����������');";
					btSetRealName.Attributes["onClick"]= "return confirm('ȷ��Ҫִ�С��������ͨ����ʵ����������');";
				}

				//��ȡ���ݣ�����QQ�ź󣬾�Ҫ��ȡ��̨��Ϣ��
//				labFuin.Text = dr["Fuin"].ToString();

				labFTypeName.Text = dr["FTypeName"].ToString();
				cre_id.Text = dr["cre_id"].ToString();
				new_cre_id.Text = dr["new_cre_id"].ToString();

				cre_type.Text = GetCreType(dr["cre_type"].ToString());

				email.Text = dr["Femail"].ToString();

				labFstatename.Text = dr["FStateName"].ToString();

				if(dr["clear_pps"].ToString() == "1" && dr["FType"].ToString() == "1")
				{
					clear_pps.Text = "���";
				}
				else if (dr["FType"].ToString() == "1" && dr["clear_pps"].ToString() != "1")
				{
					clear_pps.Text = "�����";
				}
				else
				{
					clear_pps.Text = "";
				}

				tbReason.Text = dr["reason"].ToString();
				tbComment.Text = dr["Fcomment"].ToString();

				old_name.Text = dr["old_name"].ToString();
				new_name.Text = dr["new_name"].ToString();
				tbFCheckInfo.Text = PublicRes.GetString(dr["FCheckInfo"]);

				if(dr["FType"].ToString() == "3")
				{
					old_name.Text = dr["old_company"].ToString();
					new_name.Text = dr["new_company"].ToString();
				}

				labIsAnswer.Text = dr["labIsAnswer"].ToString();
				lblBindMobileUser.Text = dr["mobile_no"].ToString();
				lblBindMailUser.Text = dr["Femail"].ToString();
				lblstandard_score.Text = dr["standard_score"].ToString();
				lblscore.Text = dr["score"].ToString();
				lbldetail_score.Text = dr["detail_score"].ToString();
				lblrisk_result.Text = dr["risk_result"].ToString();
           //     lbauthenState.Text = dr["authenState"].ToString();//����ʵ����֤�ֶ�

				lblivrresult.Text = dr["FIVRResult"].ToString();

                lbImageSpecial.Text = "�ʽ���Դ";

				string imagestr = dr["cre_image"].ToString().Trim();
				string url = System.Configuration.ConfigurationManager.AppSettings["AppealUrlPath"].Trim();

				if(!url.EndsWith("/"))
					url += "/";

                string urlCGI = System.Configuration.ConfigurationManager.AppSettings["GetAppealImageCgi"].Trim();//��ͼƬcgi
                //20131111 lxl �����ַֿ�����Ϳ�������ͼƬ
				// Ŀǰ�Ƹ��ܽ����Ҫ����ͼƬ
                if (dr["FType"].ToString() == "0" || dr["FType"].ToString() == "9" || dr["FType"].ToString() == "10" || dr["FType"].ToString() == "1" || dr["FType"].ToString() == "5" || dr["FType"].ToString() == "6")
                {
                    if (imagestr.IndexOf("|") > 0)
                    {
                        string[] imgUrls = imagestr.Split('|');
                        //Image1.ImageUrl = url + imgUrls[0];
                        //Image2.ImageUrl = url + imgUrls[1];
                        //�¿������֤ͼƬͨ��cgi����ȡ
                        this.Image1.ImageUrl = urlCGI + imgUrls[0];
                        this.Image2.ImageUrl = urlCGI + imgUrls[1];
                      
                    }
                    else
                    {
                      //  Image1.ImageUrl = url + imagestr;
                        this.Image1.ImageUrl = urlCGI + imagestr;
                    }
                }
                else
                {
                    //Image1.ImageUrl = url + imagestr;
                    this.Image1.ImageUrl = urlCGI + imagestr;

                    //���������ֻ�ʱ�ټ���һ��ͼƬ
                    string imagestr2 = dr["cre_image2"].ToString().Trim();  //�Ƿ���5���ͣ�
                  //  Image2.ImageUrl = url + imagestr2;
                    this.Image2.ImageUrl = urlCGI + imagestr2;
                }


				try
				{
					DataSet dsuser =  qs.GetAppealUserInfo(dr["Fuin"].ToString());
					if(dsuser == null || dsuser.Tables.Count == 0 || dsuser.Tables[0].Rows.Count != 1)
					{
						labFQQid.Text = "��ȡ��������" ;
						btOK.Visible = false;
						btSetRealName.Visible = false;
					}
					else
					{
						DataRow druser = dsuser.Tables[0].Rows[0];

						labFQQid.Text = druser["Fqqid"].ToString();
						labFbalance.Text = classLibrary.setConfig.FenToYuan(druser["FBalance"].ToString());
						labFCon.Text = classLibrary.setConfig.FenToYuan(druser["Fcon"].ToString());

						labFcre_type.Text = GetCreType(druser["Fcre_type"].ToString());

						labFcreid.Text = PublicRes.GetString(druser["Fcreid"]);
						labFEmail.Text = PublicRes.GetString(druser["FEmail"]);
						labFtruename.Text = PublicRes.GetString(druser["Ftruename"]);

						labFBankAcc.Text = PublicRes.GetString(druser["Fbankid"]);

						if(dr["FType"].ToString() == "3")
						{
							labFtruename.Text = PublicRes.GetString(druser["Fcompany_name"]);
						}
					}
				}
				catch(Exception err)
				{
					labFQQid.Text = "��ȡ��������" + PublicRes.GetErrorMsg(err.Message);
					btOK.Visible = false;
					btSetRealName.Visible = false;
				}

                qs.Finance_HeaderValue = classLibrary.setConfig.setFH(this);
                //2.��ѯʵ����֤
                DataSet dsA = qs.GetUserAuthenState(dr["Fuin"].ToString(),"",0);
                if (dsA == null || dsA.Tables.Count < 1 || dsA.Tables[0].Rows.Count != 1)
                {
                    lbauthenState.Text = "��";//�Ƿ�ʵ����֤
                }
                else
                {
                    DataRow row = dsA.Tables[0].Rows[0];
                    if (row["queryType"].ToString() == "2")
                    {
                        lbauthenState.Text = "��";
                    }
                    else
                    {
                        lbauthenState.Text = "��";
                    }
                }

			}
			else
			{
				throw new Exception("û���ҵ���¼��");
			}
		}


		private void BindInfoFListID(int flist_id)
		{
			lblfid.Text = flist_id.ToString();

			Query_Service.Query_Service qs = new Query_Service.Query_Service();
			DataSet ds =  qs.GetUserClassDetail(flist_id);

			if(ds != null && ds.Tables.Count >0 && ds.Tables[0].Rows.Count > 0 )
			{
				DataRow dr = ds.Tables[0].Rows[0];
				
				if(dr["Fpickstate"].ToString() == "0" || dr["Fpickstate"].ToString() == "1")
				{
					btOK.Visible = true;
					btSetRealName.Visible = true;
					btCancel.Visible = true;
					btnDel.Visible = false;

					btOK.Attributes["onClick"]= "return confirm('ȷ��Ҫִ�С�ͨ�����ߡ�������');";
					btSetRealName.Attributes["onClick"]= "return confirm('ȷ��ͨ�����߲���ʵ����');";
					btCancel.Attributes["onClick"]= "return confirm('ȷ��Ҫִ�С��ܾ����ߡ�������');";
				}
				// 2012/4/17 �������2�����ʵ����֤������
				else if(dr["Fpickstate"].ToString() == "3" )
				{
					btOK.Visible = true;
					btSetRealName.Visible = true;
					btCancel.Visible = false;
					btnDel.Visible = false;

					btOK.Attributes["onClick"]= "return confirm('ȷ��Ҫ����ִ�С�ͨ�����ߡ�������');";
					btSetRealName.Attributes["onClick"]= "return confirm('ȷ��Ҫִ�С��������ͨ����ʵ����������');";
				}

				//��ȡ���ݣ�����QQ�ź󣬾�Ҫ��ȡ��̨��Ϣ��
//				labFuin.Text = dr["Fqqid"].ToString();

				labFTypeName.Text = "ʵ����֤";
				tbComment.Visible = false;
//				cre_id.Text = dr["cre_id"].ToString();

//				cre_type.Text = GetCreType(dr["cre_type"].ToString());
//
//				email.Text = dr["Femail"].ToString();
//
//				labFstatename.Text = dr["FStateName"].ToString();
//
//				if(dr["clear_pps"].ToString() == "1" && dr["FType"].ToString() == "1")
//				{
//					clear_pps.Text = "���";
//				}
//				else if (dr["FType"].ToString() == "1" && dr["clear_pps"].ToString() != "1")
//				{
//					clear_pps.Text = "�����";
//				}
//				else
//				{
//					clear_pps.Text = "";
//				}

//				tbReason.Text = dr["reason"].ToString();
//				tbComment.Text = dr["Fcomment"].ToString();

//				old_name.Text = dr["old_name"].ToString();
//				new_name.Text = dr["new_name"].ToString();
				tbFCheckInfo.Text = PublicRes.GetString(dr["Fmemo"]);

//				if(dr["FType"].ToString() == "3")
//				{
//					old_name.Text = dr["old_company"].ToString();
//					new_name.Text = dr["new_company"].ToString();
//				}
			
				string imagestr = dr["Fpath"].ToString().Trim();
                //ʵ����֤ͼƬ��ԭ��������UserClassUrlPath��ΪGetAppealImageCgi��������1��5��6����һ�£�gregyao�ṩ�ĵķ���
                string url = System.Configuration.ConfigurationManager.AppSettings["GetAppealImageCgi"].Trim();

				if(imagestr.IndexOf("|")>0)
				{
					string[] strtmps = imagestr.Split('|');
					Image1.ImageUrl =  url + strtmps[0];
					Image2.ImageUrl =  url + strtmps[1];
				}
				else
				Image1.ImageUrl =  url + imagestr;

//				//���������ֻ�ʱ�ټ���һ��ͼƬ
//				string imagestr2 = dr["Fdes_path"].ToString().Trim();
//				Image2.ImageUrl =  url + imagestr2;

				string Msg = "";

				try
				{
					DataSet dsuser =  qs.GetUserClassInfoFlag(dr["Fqqid"].ToString(),out Msg);
					if(dsuser == null || dsuser.Tables.Count == 0 || dsuser.Tables[0].Rows.Count != 1)
					{
						labFQQid.Text = "��ȡ��������,ICE�ӿڴ���!" + Msg ;
						btOK.Visible = false;
						btSetRealName.Visible = false;
					}
					else
					{
						DataRow druser = dsuser.Tables[0].Rows[0];

						labFQQid.Text = druser["Fqqid"].ToString();

						labFcre_type.Text = GetCreType(druser["Fcre_type"].ToString());

						labFcreid.Text = PublicRes.GetString(druser["Fcreid"]);
//						labFEmail.Text = PublicRes.GetString(druser["FEmail"]);
						labFtruename.Text = PublicRes.GetString(druser["Ftruename"]);

//						labFBankAcc.Text = PublicRes.GetString(druser["Fbankid"]);

//						if(dr["FType"].ToString() == "3")
//						{
//							labFtruename.Text = PublicRes.GetString(druser["Fcompany_name"]);
//						}
					}
				}
				catch(Exception err)
				{
					labFQQid.Text = "��ȡ��������" + PublicRes.GetErrorMsg(err.Message) + Msg;
					btOK.Visible = false;
					btSetRealName.Visible = false;
				}
			}
			else
			{
				throw new Exception("û���ҵ���¼��");
			}
		}


		protected void btOK_Click(object sender, System.EventArgs e)
		{
			//ͨ������.
			string msg = "";

			try
			{
				string fid = ViewState["fid"].ToString();
				string flist_id = ViewState["flist_id"].ToString();
                string db = ViewState["db"].ToString();
                string tb = ViewState["tb"].ToString();

				Query_Service.Query_Service qs = new Query_Service.Query_Service();

				Finance_Header fh = setConfig.setFH(this);
				//			fh.UserIP = Request.UserHostAddress;
				//			fh.UserName = Session["uid"].ToString();
				//
				//			fh.OperID = Int32.Parse(Session["OperID"].ToString());
				//			fh.SzKey = Session["SzKey"].ToString();
				//
				qs.Finance_HeaderValue = fh;
				//			qs.Finance_HeaderValue = setConfig.setFH(this);

				string UserIP = Request.UserHostAddress;
				string UserName = Session["uid"].ToString();

				if(fid != null && fid != "")
				{
                    if (db == null || db == "" || tb == null || tb == "")
                    {
                        if (qs.CFTConfirmAppeal(int.Parse(fid), tbComment.Text.Trim(), UserName, UserIP, out msg))
                        {
                            btCancel.Visible = false;
                            btOK.Visible = false;
                            btSetRealName.Visible = false;
                            btnDel.Visible = false;
                        }
                        else
                        {
                            WebUtils.ShowMessage(this.Page, "����ʧ��:" + PublicRes.GetErrorMsg(msg));
                        }
                    }
                    else
                    {
                        if (qs.CFTConfirmAppealDBTB(fid,db,tb, tbComment.Text.Trim(), UserName, UserIP, out msg))
                        {
                            btCancel.Visible = false;
                            btOK.Visible = false;
                            btSetRealName.Visible = false;
                            btnDel.Visible = false;
                        }
                        else
                        {
                            WebUtils.ShowMessage(this.Page, "����ʧ��:" + PublicRes.GetErrorMsg(msg));
                        }

                    }
				}
				else if(flist_id != null && flist_id != "")
				{
					if(qs.UserClassConfirm(int.Parse(flist_id),UserName,out msg))
					{
						btCancel.Visible = false;
						btOK.Visible = false;
						btnDel.Visible = false;
						btSetRealName.Visible = false;
					}
					else
					{
						WebUtils.ShowMessage(this.Page,"����ʧ��:" +  PublicRes.GetErrorMsg(msg));
					}
				}
				else
				{
					WebUtils.ShowMessage(this.Page,"��������");
					return;
				}
				
			}
			catch(Exception err)
			{
				WebUtils.ShowMessage(this.Page,"ͨ��ʧ��:" + PublicRes.GetErrorMsg(err.Message + msg));
			}
			finally
			{
				System.GC.Collect();
			}
		}


		protected void btSetRealName_Click(object sender, System.EventArgs e)
		{
			string msg = "";
			try
			{
				string fid = ViewState["fid"].ToString();
				string flist_id = ViewState["flist_id"].ToString();
                string db = ViewState["db"].ToString();
                string tb = ViewState["tb"].ToString();

				Query_Service.Query_Service qs = new Query_Service.Query_Service();
				Finance_Header fh = setConfig.setFH(this);
				qs.Finance_HeaderValue = fh;
				string UserIP = Request.UserHostAddress;
				string UserName = Session["uid"].ToString();

				if(fid != null && fid != "")
				{
                    if (db == null || db == "" || tb == null || tb == "")
                    {
                        if (qs.CFTConfirmAppeal(int.Parse(fid), tbComment.Text.Trim(), UserName, UserIP, out msg))
                        {
                            btCancel.Visible = false;
                            btOK.Visible = false;
                            btSetRealName.Visible = false;
                            btnDel.Visible = false;
                        }
                        else
                        {
                            WebUtils.ShowMessage(this.Page, "����ʧ��:" + PublicRes.GetErrorMsg(msg));
                        }
                    }
                    else
                    {
                        if (qs.CFTConfirmAppealDBTB(fid, db, tb, tbComment.Text.Trim(), UserName, UserIP, out msg))
                        {
                            btCancel.Visible = false;
                            btOK.Visible = false;
                            btSetRealName.Visible = false;
                            btnDel.Visible = false;
                        }
                        else
                        {
                            WebUtils.ShowMessage(this.Page, "����ʧ��:" + PublicRes.GetErrorMsg(msg));
                        }

                    }
				}
				else if(flist_id != null && flist_id != "")
				{
					if(qs.UserClassConfirm(int.Parse(flist_id),UserName,out msg))
					{
						System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
						string account = labFQQid.Text.Trim();
						string internalID = qs.QQ2Uid(account);
						string ID = new_cre_id.Text.Trim();
						string IDType = cre_type.Text.Trim();
						string realName = new_name.Text.Trim();
						string md5Key = "022dc90369a73ebf963b94617fd2b2f7";
						string sign = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(account + internalID + ID + IDType + md5Key,"md5");
						WebClient client = new WebClient();
						//10.137.151.211  10.137.151.210
						string url = "http://www.tenpay.com/app/v1.0/verify_cre_name_info.cgi?uin={0}&uid={1}&cre_id={2}&cre_type={3}&truename={4}&sign={5}&input_charset=GBK";
						Stream data = client.OpenRead(string.Format(url,account,internalID,ID,IDType,realName,sign));
						StreamReader reader = new StreamReader(data);
						string result = reader.ReadToEnd();
						qs.UpdateInfoFromPolice(flist_id, result);

						btCancel.Visible = false;
						btOK.Visible = false;
						btnDel.Visible = false;
						btSetRealName.Visible = false;
					}
					else
					{
						WebUtils.ShowMessage(this.Page,"����ʧ��:" +  PublicRes.GetErrorMsg(msg));
					}
				}
				else
				{
					WebUtils.ShowMessage(this.Page,"��������");
					return;
				}
				
			}
			catch(Exception err)
			{
				WebUtils.ShowMessage(this.Page,"ͨ��ʧ��:" + PublicRes.GetErrorMsg(err.Message + msg));
			}
		}


		protected void btCancel_Click(object sender, System.EventArgs e)
		{
			//�ܾ�����.
			string msg = "";
			string tips_3 = "���������һص�ַ";
			//string tips_3 = @"���������ύ������ʱ���ϴ��˻��ʽ���Դ��ͼ����ο���<a href='http://help.tenpay.com/cgi-bin/helpcenter/help_center.cgi?id=2232&type=0'>���������һ�</a>��ָ��";
			try
			{
				string reason = "",OtherReason = "";

				if(this.cb_0.Checked && this.tbFCheckInfo.Text.Trim() == "")
				{
					throw new Exception("����������ԭ��");
				}
				if(this.cb_1.Checked)
				{
					reason += this.cb_1.Text + "&";
				}
				if(this.cb_5.Checked)
				{
					reason += this.cb_5.Text + "&";
				}
				if(this.cb_2.Checked)
				{
					reason += this.cb_2.Text+ "&";
					for(int i=0;i<this.cbxl_2.Items.Count;i++)
					{
						if(this.cbxl_2.Items[i].Selected)
							reason += this.cbxl_2.Items[i].Value + "&";
					}
				}
				if(this.cb_3.Checked)
				{
					reason += this.cb_3.Text + "&";
					if(this.cbx_detail_cbx3.Checked)
						reason += tips_3 + "&";
				}
				if(this.cb_4.Checked)
				{
					reason += this.cb_4.Text + "&";
				}

				if(this.cb_0.Checked)
				{
					OtherReason = classLibrary.setConfig.replaceMStr(tbFCheckInfo.Text.Trim());
				}

				if(classLibrary.getData.IsTestMode)//���Լ��ˣ�
				{
					this.ShowMsg(reason + OtherReason);
					
					return;
				}

				if(reason.Trim() == "" && OtherReason.Trim() == "")
					throw new Exception("��ѡ��ܾ�ԭ��");

//				if(this.RejectReason.SelectedIndex == -1)
//				{
//					throw new Exception("��ѡ��ܾ�ԭ��");
//				}
//				if(this.RejectReason.Items[5].Selected && this.tbFCheckInfo.Text.Trim() == "")
//				{
//					throw new Exception("����������ԭ��");
//				}
//				string reason = "",OtherReason = "";
//				for(int i=0; i<this.RejectReason.Items.Count; i++)
//				{
//					if(i == 5)
//						OtherReason = classLibrary.setConfig.replaceMStr(tbFCheckInfo.Text.Trim());
//					else if(this.RejectReason.Items[i].Selected)
//					{
//						reason += this.RejectReason.Items[i].Text + "&";
//					}
//				}

				string fid = ViewState["fid"].ToString();
				string flist_id = ViewState["flist_id"].ToString();
                string db = ViewState["db"].ToString();
                string tb = ViewState["tb"].ToString();

				Query_Service.Query_Service qs = new Query_Service.Query_Service();

				Finance_Header fh = setConfig.setFH(this);
				//			fh.UserIP = Request.UserHostAddress;
				//			fh.UserName = Session["uid"].ToString();
				//
				//			fh.OperID = Int32.Parse(Session["OperID"].ToString());
				//			fh.SzKey = Session["SzKey"].ToString();
				//
				qs.Finance_HeaderValue = fh;
				//			qs.Finance_HeaderValue = setConfig.setFH(this);

				string UserIP = Request.UserHostAddress;
				string UserName = Session["uid"].ToString();

				if(fid != null && fid != "")
				{
                    if (db == null || db == "" || tb == null || tb == "")
                    {
                        if (qs.CFTCancelAppeal(int.Parse(fid), reason, OtherReason, this.tbComment.Text.Trim(), UserName, UserIP, out msg))
                        {
                            btCancel.Visible = false;
                            btOK.Visible = false;
                            btSetRealName.Visible = false;
                            btnDel.Visible = false;
                        }
                        else
                        {
                            WebUtils.ShowMessage(this.Page, "����ʧ��:" + PublicRes.GetErrorMsg(msg));
                            BindInfoFID(int.Parse(fid));
                        }
                    }
                    else
                    {
                        if (qs.CFTCancelAppealDBTB(fid, db, tb, reason, OtherReason, this.tbComment.Text.Trim(), UserName, UserIP, out msg))
                        {
                            btCancel.Visible = false;
                            btOK.Visible = false;
                            btSetRealName.Visible = false;
                            btnDel.Visible = false;
                        }
                        else
                        {
                            WebUtils.ShowMessage(this.Page, "����ʧ��:" + PublicRes.GetErrorMsg(msg));
                            BindInfoFIDDBTB(fid, db, tb);
                        }
                    }
				}
				else if(flist_id != null && flist_id != "")
				{
					if(qs.UserClassCancel(int.Parse(flist_id),reason,OtherReason,UserName,out msg))
					{
						btCancel.Visible = false;
						btOK.Visible = false;
						btSetRealName.Visible = false;
						btnDel.Visible = false;
					}
					else
					{
						WebUtils.ShowMessage(this.Page,"����ʧ��:" +  PublicRes.GetErrorMsg(msg));
						BindInfoFListID(int.Parse(flist_id));
					}
				}
				else
				{
					WebUtils.ShowMessage(this.Page,"��������");
					return;
				}
			}
			catch(Exception err)
			{
				WebUtils.ShowMessage(this.Page,"�ܾ�ʧ��:" + PublicRes.GetErrorMsg(err.Message+msg));
			}
			finally
			{
				System.GC.Collect();
			}
		}


		protected void btnDel_Click(object sender, System.EventArgs e)
		{
			//ɾ������,���,ֱ����״̬����.
			string msg = "";

			try
			{

				string fid = ViewState["fid"].ToString();
				string flist_id = ViewState["flist_id"].ToString();
                string db = ViewState["db"].ToString();
                string tb = ViewState["tb"].ToString();

				Query_Service.Query_Service qs = new Query_Service.Query_Service();

				Finance_Header fh = setConfig.setFH(this);
				//			fh.UserIP = Request.UserHostAddress;
				//			fh.UserName = Session["uid"].ToString();
				//
				//			fh.OperID = Int32.Parse(Session["OperID"].ToString());
				//			fh.SzKey = Session["SzKey"].ToString();
				//
				qs.Finance_HeaderValue = fh;
				//			qs.Finance_HeaderValue = setConfig.setFH(this);

				string UserIP = Request.UserHostAddress;
				string UserName = Session["uid"].ToString();

				if(fid != null && fid != "")
				{
                    if (db == null || db == "" || tb == null || tb == "")
                    {
                        if (qs.CFTDelAppeal(int.Parse(fid), tbComment.Text.Trim(), UserName, UserIP, out msg))
                        {
                            btCancel.Visible = false;
                            btOK.Visible = false;
                            btSetRealName.Visible = false;
                            btnDel.Visible = false;
                        }
                        else
                        {
                            WebUtils.ShowMessage(this.Page, "����ʧ��:" + PublicRes.GetErrorMsg(msg));
                            BindInfoFID(int.Parse(fid));
                        }
                    }
                    else
                    {
                        if (qs.CFTDelAppealDBTB(fid, db,tb,tbComment.Text.Trim(), UserName, UserIP, out msg))
                        {
                            btCancel.Visible = false;
                            btOK.Visible = false;
                            btSetRealName.Visible = false;
                            btnDel.Visible = false;
                        }
                        else
                        {
                            WebUtils.ShowMessage(this.Page, "����ʧ��:" + PublicRes.GetErrorMsg(msg));
                            BindInfoFIDDBTB(fid, db, tb);
                        }
                    }
				}
				else if(flist_id != null && flist_id != "")
				{
					WebUtils.ShowMessage(this.Page,"������ɾ����");
					return;
				}
				else
				{
					WebUtils.ShowMessage(this.Page,"��������");
					return;
				}
				
			}
			catch(Exception err)
			{
				WebUtils.ShowMessage(this.Page,"�ܾ�ʧ��:" + PublicRes.GetErrorMsg(err.Message+msg));
			}
			finally
			{
				System.GC.Collect();
			}
		}


		private void ShowMsg(string msg)
		{
			Response.Write("<script language=javascript>alert('" + msg + "')</script>");
		}
	}
}
