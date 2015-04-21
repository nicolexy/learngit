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
	/// CFTUserCheck 的摘要说明。
	/// </summary>
	public partial class CFTUserCheck : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面
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
                    //20131107 lxl 分库分表后增加两个参数
                    db = Request.QueryString["db"].ToString();
                    tb = Request.QueryString["tb"].ToString();
					ViewState["fid"] = fid;
					ViewState["flist_id"] = flist_id;
                    ViewState["db"] = db;
                    ViewState["tb"] = tb;
				}
				catch
				{
					WebUtils.ShowMessage(this.Page,"参数有误！");
					return;
				}

				try
				{
                    if (fid != null && fid != "")
                    {
                        if (db == null || db == "" || tb == null || tb == "")
                            BindInfoFID(int.Parse(fid));
                        else//三种类型的分库表
                            BindInfoFIDDBTB(fid,db,tb);
                    }
                    else if (flist_id != null && flist_id != "")
                    {
                        BindInfoFListID(int.Parse(flist_id));
                    }
                    else
                    {
                        WebUtils.ShowMessage(this.Page, "参数有误！");
                        return;
                    }
				}
				catch(LogicException err)
				{
					WebUtils.ShowMessage(this.Page,err.Message);
				}
				catch(SoapException eSoap) //捕获soap类异常
				{
					string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
					WebUtils.ShowMessage(this.Page,"调用服务出错：" + errStr);
				}
				catch(Exception eSys)
				{
					WebUtils.ShowMessage(this.Page,"读取数据失败！" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
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

		private string GetCreType(string creid)
		{
	
			if(creid == null || creid.Trim() == "")
				return "未指定类型";

			int icreid = 0;
			try
			{
				icreid = Int32.Parse(creid);
			}
			catch
			{
				return "不正确类型" + creid;
			}

			if(icreid >=1 && icreid <= 11)
			{
				if(icreid == 1)
				{
					return "身份证";
				}
				else if(icreid == 2)
				{
					return "护照";
				}
				else if(icreid == 3)
				{
					return "军官证";
				}
				else if(icreid == 4)
				{
					return "士兵证";
				}
				else if(icreid == 5)
				{
					return "回乡证";
				}
				else if(icreid == 6)
				{
					return "临时身份证";
				}
				else if(icreid == 7)
				{
					return "户口簿";
				}
				else if(icreid == 8)
				{
					return "警官证";
				}
				else if(icreid == 9)
				{
					return "台胞证";
				}
				else if(icreid == 10)
				{
					return "营业执照";
				}
				else if(icreid == 11)
				{
					return "其它证件";
				}
				else
				{
					return "不正确类型" + creid;
				}
			}
			else
			{
				return "不正确类型" + creid;
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

					btOK.Attributes["onClick"]= "return confirm('确定要执行“通过申诉”操作吗？');";
					btSetRealName.Attributes["onClick"] = "return confirm('确定通过申诉并置实名吗？')";
					btCancel.Attributes["onClick"]= "return confirm('确定要执行“拒绝申诉”操作吗？');";
					btnDel.Attributes["onClick"]= "return confirm('确定要执行“删除申诉”操作吗？');";
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

					btOK.Attributes["onClick"]= "return confirm('确定要执行“二次审核通过”操作吗？');";
					btSetRealName.Attributes["onClick"]= "return confirm('确定要执行“二次审核通过置实名”操作吗？');";
				}

				//读取内容，读出QQ号后，就要读取后台信息。
//				labFuin.Text = dr["Fuin"].ToString();

				labFTypeName.Text = dr["FTypeName"].ToString();
				cre_id.Text = dr["cre_id"].ToString();
				new_cre_id.Text = dr["new_cre_id"].ToString();

				cre_type.Text = GetCreType(dr["cre_type"].ToString());

				email.Text = dr["Femail"].ToString();

				labFstatename.Text = dr["FStateName"].ToString();

				if(dr["clear_pps"].ToString() == "1" && dr["FType"].ToString() == "1")
				{
					clear_pps.Text = "清除";
				}
				else if (dr["FType"].ToString() == "1" && dr["clear_pps"].ToString() != "1")
				{
					clear_pps.Text = "不清除";
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
           //     lbauthenState.Text = dr["authenState"].ToString();//增加实名认证字段

				lblivrresult.Text = dr["FIVRResult"].ToString();

				string imagestr = dr["cre_image"].ToString().Trim();
				string url = System.Configuration.ConfigurationManager.AppSettings["AppealUrlPath"].Trim();

				if(!url.EndsWith("/"))
					url += "/";

				// 目前财付盾解绑需要两张图片
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

					//更换关联手机时再加入一个图片
					string imagestr2 = dr["cre_image2"].ToString().Trim();
					Image2.ImageUrl =  url + imagestr2;
				}


				try
				{
					DataSet dsuser =  qs.GetAppealUserInfo(dr["Fuin"].ToString());
					if(dsuser == null || dsuser.Tables.Count == 0 || dsuser.Tables[0].Rows.Count != 1)
					{
						labFQQid.Text = "读取数据有误" ;
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
					labFQQid.Text = "读取数据有误：" + PublicRes.GetErrorMsg(err.Message);
					btOK.Visible = false;
					btSetRealName.Visible = false;
				}

                qs.Finance_HeaderValue = classLibrary.setConfig.setFH(this);
                //2.查询实名认证
                DataSet dsA = qs.GetUserAuthenState(dr["Fuin"].ToString(),"",0);
                if (dsA == null || dsA.Tables.Count < 1 || dsA.Tables[0].Rows.Count != 1)
                {
                    lbauthenState.Text = "否";//是否实名认证
                }
                else
                {
                    DataRow row = dsA.Tables[0].Rows[0];
                    if (row["queryType"].ToString() == "2")
                    {
                        lbauthenState.Text = "是";
                    }
                    else
                    {
                        lbauthenState.Text = "否";
                    }
                }

			}
			else
			{
				throw new Exception("没有找到记录！");
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

					btOK.Attributes["onClick"]= "return confirm('确定要执行“通过申诉”操作吗？');";
					btSetRealName.Attributes["onClick"] = "return confirm('确定通过申诉并置实名吗？')";
					btCancel.Attributes["onClick"]= "return confirm('确定要执行“拒绝申诉”操作吗？');";
					btnDel.Attributes["onClick"]= "return confirm('确定要执行“删除申诉”操作吗？');";
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

					btOK.Attributes["onClick"]= "return confirm('确定要执行“二次审核通过”操作吗？');";
					btSetRealName.Attributes["onClick"]= "return confirm('确定要执行“二次审核通过置实名”操作吗？');";
				}

				//读取内容，读出QQ号后，就要读取后台信息。
//				labFuin.Text = dr["Fuin"].ToString();

				labFTypeName.Text = dr["FTypeName"].ToString();
				cre_id.Text = dr["cre_id"].ToString();
				new_cre_id.Text = dr["new_cre_id"].ToString();

				cre_type.Text = GetCreType(dr["cre_type"].ToString());

				email.Text = dr["Femail"].ToString();

				labFstatename.Text = dr["FStateName"].ToString();

				if(dr["clear_pps"].ToString() == "1" && dr["FType"].ToString() == "1")
				{
					clear_pps.Text = "清除";
				}
				else if (dr["FType"].ToString() == "1" && dr["clear_pps"].ToString() != "1")
				{
					clear_pps.Text = "不清除";
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
           //     lbauthenState.Text = dr["authenState"].ToString();//增加实名认证字段

				lblivrresult.Text = dr["FIVRResult"].ToString();

                lbImageSpecial.Text = "资金来源";

				string imagestr = dr["cre_image"].ToString().Trim();
				string url = System.Configuration.ConfigurationManager.AppSettings["AppealUrlPath"].Trim();

				if(!url.EndsWith("/"))
					url += "/";

                string urlCGI = System.Configuration.ConfigurationManager.AppSettings["GetAppealImageCgi"].Trim();//拉图片cgi
                //20131111 lxl 加三种分库表类型可能两张图片
				// 目前财付盾解绑需要两张图片
                if (dr["FType"].ToString() == "0" || dr["FType"].ToString() == "9" || dr["FType"].ToString() == "10" || dr["FType"].ToString() == "1" || dr["FType"].ToString() == "5" || dr["FType"].ToString() == "6")
                {
                    if (imagestr.IndexOf("|") > 0)
                    {
                        string[] imgUrls = imagestr.Split('|');
                        //Image1.ImageUrl = url + imgUrls[0];
                        //Image2.ImageUrl = url + imgUrls[1];
                        //新库表的身份证图片通过cgi来获取
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

                    //更换关联手机时再加入一个图片
                    string imagestr2 = dr["cre_image2"].ToString().Trim();  //是否是5类型？
                  //  Image2.ImageUrl = url + imagestr2;
                    this.Image2.ImageUrl = urlCGI + imagestr2;
                }


				try
				{
					DataSet dsuser =  qs.GetAppealUserInfo(dr["Fuin"].ToString());
					if(dsuser == null || dsuser.Tables.Count == 0 || dsuser.Tables[0].Rows.Count != 1)
					{
						labFQQid.Text = "读取数据有误" ;
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
					labFQQid.Text = "读取数据有误：" + PublicRes.GetErrorMsg(err.Message);
					btOK.Visible = false;
					btSetRealName.Visible = false;
				}

                qs.Finance_HeaderValue = classLibrary.setConfig.setFH(this);
                //2.查询实名认证
                DataSet dsA = qs.GetUserAuthenState(dr["Fuin"].ToString(),"",0);
                if (dsA == null || dsA.Tables.Count < 1 || dsA.Tables[0].Rows.Count != 1)
                {
                    lbauthenState.Text = "否";//是否实名认证
                }
                else
                {
                    DataRow row = dsA.Tables[0].Rows[0];
                    if (row["queryType"].ToString() == "2")
                    {
                        lbauthenState.Text = "是";
                    }
                    else
                    {
                        lbauthenState.Text = "否";
                    }
                }

			}
			else
			{
				throw new Exception("没有找到记录！");
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

					btOK.Attributes["onClick"]= "return confirm('确定要执行“通过申诉”操作吗？');";
					btSetRealName.Attributes["onClick"]= "return confirm('确定通过申诉并置实名吗？');";
					btCancel.Attributes["onClick"]= "return confirm('确定要执行“拒绝申诉”操作吗？');";
				}
				// 2012/4/17 添加允许2次审核实名认证的申诉
				else if(dr["Fpickstate"].ToString() == "3" )
				{
					btOK.Visible = true;
					btSetRealName.Visible = true;
					btCancel.Visible = false;
					btnDel.Visible = false;

					btOK.Attributes["onClick"]= "return confirm('确定要二次执行“通过申诉”操作吗？');";
					btSetRealName.Attributes["onClick"]= "return confirm('确定要执行“二次审核通过置实名”操作吗？');";
				}

				//读取内容，读出QQ号后，就要读取后台信息。
//				labFuin.Text = dr["Fqqid"].ToString();

				labFTypeName.Text = "实名认证";
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
//					clear_pps.Text = "清除";
//				}
//				else if (dr["FType"].ToString() == "1" && dr["clear_pps"].ToString() != "1")
//				{
//					clear_pps.Text = "不清除";
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
                //实名认证图片由原来的配置UserClassUrlPath改为GetAppealImageCgi，跟申诉1，5，6类型一致，gregyao提供改的方法
                string url = System.Configuration.ConfigurationManager.AppSettings["GetAppealImageCgi"].Trim();

				if(imagestr.IndexOf("|")>0)
				{
					string[] strtmps = imagestr.Split('|');
					Image1.ImageUrl =  url + strtmps[0];
					Image2.ImageUrl =  url + strtmps[1];
				}
				else
				Image1.ImageUrl =  url + imagestr;

//				//更换关联手机时再加入一个图片
//				string imagestr2 = dr["Fdes_path"].ToString().Trim();
//				Image2.ImageUrl =  url + imagestr2;

				string Msg = "";

				try
				{
					DataSet dsuser =  qs.GetUserClassInfoFlag(dr["Fqqid"].ToString(),out Msg);
					if(dsuser == null || dsuser.Tables.Count == 0 || dsuser.Tables[0].Rows.Count != 1)
					{
						labFQQid.Text = "读取数据有误,ICE接口错误!" + Msg ;
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
					labFQQid.Text = "读取数据有误：" + PublicRes.GetErrorMsg(err.Message) + Msg;
					btOK.Visible = false;
					btSetRealName.Visible = false;
				}
			}
			else
			{
				throw new Exception("没有找到记录！");
			}
		}


		protected void btOK_Click(object sender, System.EventArgs e)
		{
			//通过请求.
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
                            WebUtils.ShowMessage(this.Page, "操作失败:" + PublicRes.GetErrorMsg(msg));
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
                            WebUtils.ShowMessage(this.Page, "操作失败:" + PublicRes.GetErrorMsg(msg));
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
						WebUtils.ShowMessage(this.Page,"操作失败:" +  PublicRes.GetErrorMsg(msg));
					}
				}
				else
				{
					WebUtils.ShowMessage(this.Page,"参数有误！");
					return;
				}
				
			}
			catch(Exception err)
			{
				WebUtils.ShowMessage(this.Page,"通过失败:" + PublicRes.GetErrorMsg(err.Message + msg));
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
                            WebUtils.ShowMessage(this.Page, "操作失败:" + PublicRes.GetErrorMsg(msg));
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
                            WebUtils.ShowMessage(this.Page, "操作失败:" + PublicRes.GetErrorMsg(msg));
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
						WebUtils.ShowMessage(this.Page,"操作失败:" +  PublicRes.GetErrorMsg(msg));
					}
				}
				else
				{
					WebUtils.ShowMessage(this.Page,"参数有误！");
					return;
				}
				
			}
			catch(Exception err)
			{
				WebUtils.ShowMessage(this.Page,"通过失败:" + PublicRes.GetErrorMsg(err.Message + msg));
			}
		}


		protected void btCancel_Click(object sender, System.EventArgs e)
		{
			//拒绝请求.
			string msg = "";
			string tips_3 = "特殊申诉找回地址";
			//string tips_3 = @"请您重新提交申述表时，上传账户资金来源截图。请参考“<a href='http://help.tenpay.com/cgi-bin/helpcenter/help_center.cgi?id=2232&type=0'>特殊申诉找回</a>”指引";
			try
			{
				string reason = "",OtherReason = "";

				if(this.cb_0.Checked && this.tbFCheckInfo.Text.Trim() == "")
				{
					throw new Exception("请输入其它原因！");
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

				if(classLibrary.getData.IsTestMode)//测试加了！
				{
					this.ShowMsg(reason + OtherReason);
					
					return;
				}

				if(reason.Trim() == "" && OtherReason.Trim() == "")
					throw new Exception("请选择拒绝原因！");

//				if(this.RejectReason.SelectedIndex == -1)
//				{
//					throw new Exception("请选择拒绝原因！");
//				}
//				if(this.RejectReason.Items[5].Selected && this.tbFCheckInfo.Text.Trim() == "")
//				{
//					throw new Exception("请输入其它原因！");
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
                            WebUtils.ShowMessage(this.Page, "操作失败:" + PublicRes.GetErrorMsg(msg));
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
                            WebUtils.ShowMessage(this.Page, "操作失败:" + PublicRes.GetErrorMsg(msg));
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
						WebUtils.ShowMessage(this.Page,"操作失败:" +  PublicRes.GetErrorMsg(msg));
						BindInfoFListID(int.Parse(flist_id));
					}
				}
				else
				{
					WebUtils.ShowMessage(this.Page,"参数有误！");
					return;
				}
			}
			catch(Exception err)
			{
				WebUtils.ShowMessage(this.Page,"拒绝失败:" + PublicRes.GetErrorMsg(err.Message+msg));
			}
			finally
			{
				System.GC.Collect();
			}
		}


		protected void btnDel_Click(object sender, System.EventArgs e)
		{
			//删除审批,最简单,直接置状态就行.
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
                            WebUtils.ShowMessage(this.Page, "操作失败:" + PublicRes.GetErrorMsg(msg));
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
                            WebUtils.ShowMessage(this.Page, "操作失败:" + PublicRes.GetErrorMsg(msg));
                            BindInfoFIDDBTB(fid, db, tb);
                        }
                    }
				}
				else if(flist_id != null && flist_id != "")
				{
					WebUtils.ShowMessage(this.Page,"不允许删除！");
					return;
				}
				else
				{
					WebUtils.ShowMessage(this.Page,"参数有误！");
					return;
				}
				
			}
			catch(Exception err)
			{
				WebUtils.ShowMessage(this.Page,"拒绝失败:" + PublicRes.GetErrorMsg(err.Message+msg));
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
