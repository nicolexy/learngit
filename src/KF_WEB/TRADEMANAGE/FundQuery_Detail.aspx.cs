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
using Tencent.DotNet.OSS.Web.UI;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using System.Web.Services.Protocols;
using TENCENT.OSS.CFT.KF.Common;


namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// FundQuery_Detail ��ժҪ˵����
	/// </summary>
	public partial class FundQuery_Detail : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
			try
			{
				//string sr = Session["key"].ToString();
				labUid.Text = Session["uid"].ToString();
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}

			if(!IsPostBack)
			{
				string tdeid = Request.QueryString["tdeid"];
				string type =Request.QueryString["type"];
				


				if(tdeid == null || tdeid.Trim() == "")
				{
					WebUtils.ShowMessage(this.Page,"��������");
                    return;
				}

				try
				{
					if(type!=null && type!="")
					{
						if(type.Trim()=="subacc")//���ʻ���ֵ��ѯ�����
						{
							string fcurtype=Request.QueryString["curtype"];
							if(fcurtype==null || fcurtype=="")
							{
								WebUtils.ShowMessage(this.Page,"�������󣡱���Ϊ��");
								return;
							}
							BindInfo(tdeid,"","","",fcurtype,"","","","");
						}


					}
					else
					{
				
						string listid=Request.QueryString["listid"];
						string begintime = Request.QueryString["begintime"];
						string endtime = Request.QueryString["endtime"];
						string fpay_front_time=Request.QueryString["fpay_front_time"];
						string fbank_type=Request.QueryString["Fbank_type"];
						string fbank_list=Request.QueryString["Fbank_list"];
                        string fhis_flag = Request.QueryString["FHistoryFlag"];
                        BindInfo(tdeid, listid, begintime, endtime, "", fpay_front_time, fbank_type, fbank_list, fhis_flag);
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
					WebUtils.ShowMessage(this.Page,"��ȡ����ʧ�ܣ�" + eSys.Message.ToString());
				}
			}
		}


		private void BindInfo(string tdeid, string listid,string begintime, string endtime,string fcurtype,string fpay_front_time,string fbank_type,string fbank_list, string his_flag)
		{

			DataSet ds = null;
			string mesgg="";

            DateTime u_beginTime = new DateTime(1940, 1, 1);
            DateTime u_endTime = new DateTime(1940, 1, 1);

			Query_Service.Query_Service qs = new Query_Service.Query_Service();
			if(fpay_front_time!=null && fpay_front_time!="" && qs.IsNewOrderCZData(DateTime.Parse(fpay_front_time)))//������µĳ�ֵ��ֱ�Ӳ�ѯ�³�ֵ���� rowenawu 20120301
			{
				//string strSql = "listid=" + listid + "&bank_list="+fbank_list+"&bank_type="+fbank_type;
                u_beginTime = DateTime.Parse(begintime + " 00:00:00");
                u_endTime = DateTime.Parse(endtime + " 23:59:59");

                ds = qs.GetFundListDetail_New(listid, fbank_list, fbank_type, u_beginTime, u_endTime, out mesgg);
			}
			else
			{
				if(fcurtype.Trim()=="" ||fcurtype.Trim()==null)
				{
					bool isHistory = true;
					if(begintime == null || endtime == null)
					{				
						isHistory=false;
					}
					else
					{
						try
						{
							u_beginTime = DateTime.Parse(begintime + " 00:00:00");
							u_endTime = DateTime.Parse(endtime + " 23:59:59");
						}
						catch
						{
							u_beginTime  = new DateTime(1940,1,1);
							u_endTime  = new DateTime(1940,1,1);
						}

					}

					//furion ������ʷ��¼��ѯ 20060522
					ds =  qs.GetFundListDetail(tdeid,listid,u_beginTime,u_endTime,false,false);
					if(ds==null||ds.Tables.Count==0)
					{
						ds =  qs.GetFundListDetail(tdeid,listid,u_beginTime,u_endTime,false,true);
					}

				}
				else
				{
					ds =  qs.GetFundListDetail_Subacc(tdeid,fcurtype,out mesgg);

				}
			}
            //yinhuang 2014/3/9
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) 
            {
                //if (!string.IsNullOrEmpty(his_flag) && his_flag == "2")
                //{
                    ds = qs.GetFundHistoryDetail(fbank_list, fpay_front_time, listid, fbank_type);
                //}
            }
            
			if(ds != null && ds.Tables.Count >0 && ds.Tables[0].Rows.Count > 0 )
			{
				DataRow dr = ds.Tables[0].Rows[0];
				labFListID.Text = PublicRes.GetString(dr["FListID"]);

				long itmp = long.Parse(PublicRes.GetString(dr["FNum"]));

				//furion 20051012 ��Ԫת��ר����
				//float ltmp = (float)itmp/100;
				double ltmp = MoneyTransfer.FenToYuan(itmp);

				labFNum.Text = ltmp.ToString();

				string tmp = PublicRes.GetInt(dr["FState"]);
				labFState.Text = classLibrary.setConfig.returnDicStr("TCLIST_STATE",tmp);

				tmp = PublicRes.GetInt(dr["Fsign"]);
				labFSign.Text = classLibrary.setConfig.returnDicStr("TCLIST_SIGN",tmp);
				

				labFBank_List.Text = PublicRes.GetString(dr["FBank_List"]);
				labFBank_Acc.Text = PublicRes.GetString(dr["FBank_Acc"]);
				labFBank_Type.Text = PublicRes.GetString(setConfig.returnDicStr("BANK_TYPE",PublicRes.GetInt(dr["FBank_Type"])));

				labFaid.Text = PublicRes.GetString(dr["faid"]);
				labFaname.Text = PublicRes.GetString(dr["faname"]);
				labFpay_front_time.Text = PublicRes.GetDateTime(dr["Fpay_front_time"]);
				labFbank_time.Text = PublicRes.GetDateTime(dr["FBank_Time"]);
				labFmodify_time.Text = PublicRes.GetDateTime(dr["FModify_time"]);
			}
			else
			{
				WebUtils.ShowMessage(this.Page,"û���ҵ���¼��"+mesgg);
			
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
	}
}
