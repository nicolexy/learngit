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
using System.Web.Services.Protocols;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using TENCENT.OSS.CFT.KF.Common;

namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// MediOperatorManage_Detail ��ժҪ˵����
	/// </summary>
	public partial class MediOperatorManage_Detail : System.Web.UI.Page
	{
		protected System.Web.UI.HtmlControls.HtmlTable Table2;
   
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    string spid = Request.QueryString["SPID"];
                    labSP.Text = spid;

                    string username = Request.QueryString["UserName"];
                    labUserName.Text = username;

                    string qq = Request.QueryString["QQ"];
                    labQQ.Text = qq;

                   // int iFSign = GetRole(spid, qq);
                    GetRole(spid);

                }
                catch (Exception err)
                {
                    WebUtils.ShowMessage(this.Page, "�����������" + err.Message);
                }
            }
        }

        private void GetRole(string spid)
        {
            DataSet ds = QuerySPRole(spid);
           
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int n = 1;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                  
                    ListItem li = new ListItem();
                    string GroupName = ds.Tables[0].Rows[i]["GroupName"].ToString();
                    li.Text = ds.Tables[0].Rows[i]["Value"].ToString() + " " + ds.Tables[0].Rows[i]["Text"].ToString();
                    if (ds.Tables[0].Rows[i]["Selected"].ToString().ToLower() == "false" || ds.Tables[0].Rows[i]["Selected"].ToString().ToLower() == "true")
                        li.Selected = bool.Parse(ds.Tables[0].Rows[i]["Selected"].ToString());
                    else
                        li.Selected = false;

                    if (GroupName == "��������")
                        this.cblJCH.Items.Add(li);
                    else if (GroupName == "���֧��")
                        this.cblKJZF.Items.Add(li);
                    else if (GroupName == "�⿨֧��")
                        this.cblWKZF.Items.Add(li);
                    else if (GroupName == "�˿�")
                        this.cblTK.Items.Add(li);
                    else if (GroupName == "��ֵ")
                        this.cblCHZH.Items.Add(li);
                    else if (GroupName == "ת��")
                        this.cblZHZH.Items.Add(li);
                    else if (GroupName == "����")
                        this.cblQT.Items.Add(li);
                    else if (GroupName == "����")
                        this.cblDK.Items.Add(li);
                    else if (GroupName == "����")
                        this.cblFZH.Items.Add(li);
                    else if (GroupName == "�н齻��")
                        this.cblZHJJY.Items.Add(li);
                    else if (GroupName == "����")
                        this.cblDF.Items.Add(li);
                    else if (GroupName == "Ԥ����")
                        this.cblYFK.Items.Add(li);
                    else if (GroupName == "�羳")
                        this.cblKJ.Items.Add(li);
                    else if (GroupName == "POS")
                        this.cblPOS.Items.Add(li);
                    else if (GroupName == "΢�տ�")
                        this.cblmicro.Items.Add(li);
                    else if (GroupName == "δ����")
                        this.cblWQY.Items.Add(li);

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


        public DataSet QuerySPRole(string SPID)
        {
            try
            {
                BatchPay_Service.BatchPay_Service bs = new BatchPay_Service.BatchPay_Service();
                string mediOperatorName = bs.GetZWDicValueByKey("MediOperatorName_new");
                if (mediOperatorName == null || mediOperatorName == "")
                {
                    throw new Exception("δ��ѯ��MediOperatorName_new��Ӧ������ֵ");
                }

                string[] opertorNames = mediOperatorName.Split('#');
                //int left=20;
                DataTable dt = new DataTable();

                dt.Columns.Add("GroupName", typeof(System.String));
                dt.Columns.Add("Value", typeof(System.String));
                dt.Columns.Add("Text", typeof(System.String));
                dt.Columns.Add("Selected", typeof(System.Boolean));

                foreach (string oneValue in opertorNames)
                {
                    if (oneValue != null && oneValue != "")
                    {
                        string[] groupInfos = oneValue.Split(':');
                        if (groupInfos.Length == 2)
                        {
                            string groupName = groupInfos[0].Trim();
                            string trueGroupName = groupName.Substring(0, groupName.IndexOf('='));
                            string groupDetail = groupInfos[1].Trim();
                            string[] operDetails = groupDetail.Split('|');
                            foreach (string oneOper in operDetails)
                            {
                                if (oneOper != null && oneOper != "")
                                {
                                    string[] keyAndValue = oneOper.Split('=');
                                    if (keyAndValue.Length == 2)
                                    {
                                        string operkey = keyAndValue[0].Trim();
                                        string operValue = keyAndValue[1].Trim();

                                        DataRow dr = dt.NewRow();
                                        dr["GroupName"] = trueGroupName;
                                        dr["Value"] = operkey;
                                        dr["Text"] = operValue;
                                        dt.Rows.Add(dr);
                                    }
                                }
                            }
                        }
                    }
                }
                Query_Service.Query_Service qs = new Query_Service.Query_Service();

                //��ȡȨ��.
                int iFSign = 0;
                int iFSign2 = 0;
                int iFSign3 = 0;
                int iFSign5 = 0;
                try
                {
                    iFSign = qs.GetMediOperatorRoleNew(SPID, SPID, 1);
                    iFSign2 = qs.GetMediOperatorRoleNew(SPID, SPID, 2);
                    iFSign3 = qs.GetMediOperatorRoleNew(SPID, SPID, 3);
                    iFSign5 = qs.GetMediOperatorRoleNew(SPID, SPID, 5);
                }
                catch
                {
                    iFSign = 0;
                    iFSign2 = 0;
                    iFSign3 = 0;
                    iFSign5 = 0;
                }
                SetRoleSelect(ref  dt, iFSign, 1, 31);
                SetRoleSelect(ref  dt, iFSign2, 32, 62);
                SetRoleSelect(ref  dt, iFSign3, 63, 93);
                SetRoleSelect(ref  dt, iFSign5, 125, 155);

                return new DataSet() { Tables = { dt } };
            }
            catch (Exception ex)
            {
                throw new Exception("��ѯȨ��λ�쳣��"+ex.Message);
            }
        }

        private void SetRoleSelect(ref DataTable dt, int iFsignNew, int iBegin, int iEnd)
        {
            string str = Convert.ToString((long)iFsignNew, 2);
            str = str.PadLeft((iEnd - iBegin + 1), '0');

            string strR = "";
            for (int i = str.Length - 1; i >= 0; i--)
            {
                strR += str[i];
            }

            for (int i = 0; i < strR.Length; i++)
            {
                foreach (DataRow li in dt.Rows)
                {
                    if (Convert.ToInt32(li["Value"]) == iBegin + i)
                    {
                        if (strR[i].ToString() == "1")
                            li["Selected"] = true;
                        else
                            li["Selected"] = false;
                    }
                }

            }
        }
	}
}
