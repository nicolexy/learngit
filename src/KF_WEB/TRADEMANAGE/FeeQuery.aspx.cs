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
using System.Web.Services.Protocols;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web;
using TENCENT.OSS.CFT.KF.DataAccess;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using System.Configuration;
using CFT.CSOMS.BLL.WechatPay;
using CFT.CSOMS.BLL.LifeFeePaymentModule;
using CFT.CSOMS.COMMLIB;

namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// FeeQuery ��ժҪ˵����
	/// </summary>
	public partial class FeeQuery : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();
				int operid = Int32.Parse(Session["OperID"].ToString());

				//if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"InfoCenter")) Response.Redirect("../login.aspx?wh=1");
                if (!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("TradeManagement", this)) Response.Redirect("../login.aspx?wh=1");
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
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

		protected void btnQuery_Click(object sender, System.EventArgs e)
		{
			labErrMsg.Text = "";
			
			if(this.tblistID.Text=="")
			{
				WebUtils.ShowMessage(this.Page,"�������뽻�׵���!");
				return ;
			}
			try
			{
				BindData(1);
			}
			catch(Exception ex)
			{
				string msg=ex.Message.Replace("\'","\\\'");
				WebUtils.ShowMessage(this.Page,msg);
			}
		}

		private void BindData(int index)
		{
            try
            {
                string ftype = "9";// ddlFtype.SelectedValue;
                string state = "9";// ddlState.SelectedValue;
                string paystate = "9";// ddlPaytypeState.SelectedValue;

                string listID = tblistID.Text.Trim();
                ViewState["ListID"] = listID;
                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

                DataSet ds = qs.QueryFeeDataList(ftype, state, paystate, listID, 0, 1000);

                if (ds != null && ds.Tables.Count > 0)
                {
                    LifeFeePaymentService liftFee = new LifeFeePaymentService();
                    ds.Tables[0].Columns.Add("FrefundNumName", typeof(String));
                    classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "FrefundNum", "FrefundNumName");

                    ds.Tables[0].Columns.Add("FnumName", typeof(String));
                    classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Fnum", "FnumName");

                    ds.Tables[0].Columns.Add("FPaynumName", typeof(String));
                    classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "FPaynum", "FPaynumName");

                    ds.Tables[0].Columns.Add("FstateName", typeof(String));
                    ds.Tables[0].Columns.Add("FPaytypeName", typeof(String));
                    ds.Tables[0].Columns.Add("FcreateTypeName", typeof(String));
                    ds.Tables[0].Columns.Add("FtypeName", typeof(String));

                    ds.Tables[0].Columns.Add("account", typeof(String));//echo 20141022
                    ds.Tables[0].Columns.Add("StatusStr", typeof(String));//���׵�״̬
                    ds.Tables[0].Columns.Add("UserId", typeof(String));//�ɷ��˺�
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        dr.BeginEdit();

                        string tmp = dr["Fstate"].ToString();
                        if (tmp == "0")
                            tmp = "��ʼ״̬";
                        else if (tmp == "1")
                            tmp = "�ɷѳɹ�";
                        else if (tmp == "2")
                            tmp = "�˿�ɹ�";
                        else
                            tmp = "�˵�״̬δ��";
                        dr["FstateName"] = tmp;

                        tmp = dr["FPaytype"].ToString();
                        if (tmp == "0")
                            tmp = "�˿�";
                        else if (tmp == "1")
                            tmp = "���";
                        dr["FPaytypeName"] = tmp;

                        tmp = dr["FcreateType"].ToString();
                        if (tmp == "1")
                            tmp = "���Զ����ļ�";
                        else if (tmp == "2")
                            tmp = "��ʱ����";
                        dr["FcreateTypeName"] = tmp;

                        tmp = dr["Ftype"].ToString();
                        if (tmp == "water")
                            tmp = "ˮ��";
                        else if (tmp == "elec")
                            tmp = "���";
                        else if (tmp == "gas")
                            tmp = "����";
                        else if (tmp == "tel")
                            tmp = "�绰��";
                        else if (tmp == "outtime")
                            tmp = "��ʱ�˿�";
                        dr["FtypeName"] = tmp;

                        //���ݽ��׵��Ų�ѯ����˻�����
                        string account = GetBuyAccount();
                       //������
                       // account = "100000000";
                        if (!string.IsNullOrEmpty(account.Trim()) && account.Trim() != "δ֪")
                        {
                            dr["account"] = account;
                            DataSet dsLifeFee = liftFee.QueryChargeBill(account, dr["flistid"].ToString());
                            if (dsLifeFee != null && dsLifeFee.Tables.Count > 0 && dsLifeFee.Tables[0].Rows.Count > 0)
                            {
                                dsLifeFee.Tables[0].Columns.Add("FStatusStr", typeof(String));
                                Hashtable ht1 = new Hashtable();
                                ht1.Add("0", "��ѯǷ�ѳɹ�");
                                ht1.Add("1", "���ɽ��׵��ɹ�");
                                ht1.Add("2", "֧���ɹ�");
                                ht1.Add("3", "������(֪ͨ�̻�����ǰ����)");
                                ht1.Add("4", "���ʳɹ�");
                                ht1.Add("5", "����ʧ��");
                                ht1.Add("6", "�˿�ɹ�");
                                ht1.Add("7", "���׹ر�");
                                CommUtil.DbtypeToPageContent(dsLifeFee.Tables[0], "FStatus", "FStatusStr", ht1);
                                dr["StatusStr"] = dsLifeFee.Tables[0].Rows[0]["FStatusStr"].ToString();
                                dr["UserId"] = dsLifeFee.Tables[0].Rows[0]["FUserId"].ToString();
                            }
                        }
                        else
                        {
                            dr["account"] = account;
                            dr["StatusStr"] = "";
                            dr["UserId"] = "";
                        }


                        dr.EndEdit();
                    }

                    DataGrid1.DataSource = ds.Tables[0].DefaultView;
                    DataGrid1.DataBind();

                }
                else
                {
                    WebUtils.ShowMessage(this.Page, "û���ҵ���¼");
                }
            }
            catch (Exception eSys)
            {
                string err = PublicRes.GetErrorMsg(eSys.Message.ToString());
                WebUtils.ShowMessage(this.Page, "��ѯ�쳣��" +err);
            }
		}


        private string GetBuyAccount()//�ο�TradeLogQuery�� ����˻�����
        {
            string acc = "";
            try
            {
                //�󶨽���������Ϣ
                Query_Service.Query_Service myService = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

                myService.Finance_HeaderValue = classLibrary.setConfig.setFH(this);

                string selectStrSession = ViewState["ListID"].ToString();

                DateTime beginTime = DateTime.Parse(ConfigurationManager.AppSettings["sBeginTime"].ToString());

                DateTime endTime = DateTime.Parse(ConfigurationManager.AppSettings["sEndTime"].ToString());

                int istr = 1;

                int imax = 2;

                DataSet ds = new DataSet();
                // iType = 4 ���ݽ��׵��Ų�ѯ
                ds = myService.GetPayList(selectStrSession, 4, beginTime, endTime, istr, imax);

                if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                {
                    return acc = "δ֪";
                }

                //DataTable wx_dt = null;
                //try
                //{
                //    //���Ľӿڻ�δ���ߣ����¿ͷ����й���ʹ�ò��ˣ���ʱ��������
                //    wx_dt = new WechatPayService().QueryWxTrans(ds.Tables[0].Rows[0]["Flistid"].ToString()); //��ѯ΢��ת��ҵ��
                //}
                //catch
                //{
                //}

                //if (wx_dt != null && wx_dt.Rows.Count > 0)
                //{
                //    //ͨ�����ҽ��׵����鸶�
                //    acc = PublicRes.objectToString(wx_dt, "pay_openid");
                //}
                //else
                //{
                    acc = ds.Tables[0].Rows[0]["Fbuyid"].ToString();
                //}
                return acc;
            }
            catch { return acc; }
        }


	}
}
