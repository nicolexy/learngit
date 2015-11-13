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
using System.Configuration;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using CFT.CSOMS.BLL.TradeModule;
using System.Collections.Generic;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
    /// <summary>
    /// bankrolllog ��ժҪ˵����
    /// </summary>
    public partial class bankrolllog : System.Web.UI.Page
    {
        protected System.Data.DataSet DS_Bankroll;
        protected System.Data.DataTable dataTable1;
        protected System.Data.DataColumn dataColumn1;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    //�󶨵�һҳ����
                    BindData(1);
                }
                catch (Exception err)
                {
                    WebUtils.ShowMessage(this.Page, "������ʱ�������²�ѯ��" + PublicRes.GetErrorMsg(err.Message));
                }
            }
        }

        private void BindData(int pageIndex)
        {
            DateTime beginTime, endTime;

            if (Request.QueryString["BeginDate"] != null)
            {
                beginTime = DateTime.Parse(Request.QueryString["BeginDate"].ToString());
                endTime = DateTime.Parse(Request.QueryString["EndDate"].ToString());
            }
            else
            {
                beginTime = DateTime.Now.AddDays(-PublicRes.PersonInfoDayCount);
                endTime = DateTime.Now.AddDays(1);
            }

            int pageSize = Int32.Parse(ConfigurationManager.AppSettings["pageSize"].ToString());  //ͨ��webconfig����ҳ��С
            int istr = 1 + pageSize * (pageIndex - 1);  //��ʼΪ1����ʵ������0ʼ��
            int imax = pageSize;                       //ÿҳ��ʾ10����¼

            #region type==QQID
            if (Request.QueryString["type"].ToString() == "QQID")
            {

                string selectStr = Request.QueryString["qqid"] != null ? Request.QueryString["qqid"].ToString() : Session["QQID"].ToString();
                string fuid = Session["fuid"] as String;

                int fcurtype = 1;
                if (Request.QueryString["currtype"] != null)
                {
                    fcurtype = int.Parse(Request.QueryString["currtype"].Trim()); //���Ա�֤���ѯ
                }

                //���������ʻ�

                if (Request.QueryString["Fcurtype"] != null && Request.QueryString["Fcurtype"].ToString() != "")
                {
                    Query_Service.Query_Service myService = new Query_Service.Query_Service();
                    this.DS_Bankroll = myService.GetChildrenBankRollList(selectStr, beginTime, endTime, Request.QueryString["Fcurtype"].ToString(), istr, imax);
                }
                else
                {
                    //this.DS_Bankroll = classLibrary.setConfig.returnDataSet(selectStr,beginTime,endTime,0,"Bankroll",istr,imax,Session["uid"].ToString(),Request.UserHostAddress);
                    //KF��Ա��ӳ�кܶ���Ϊ0�Ŀ����ݣ����������

                    string ref_param = ViewState["ref_param"] == null ? "" : ViewState["ref_param"].ToString();

                    if (!string.IsNullOrEmpty(fuid))
                        this.DS_Bankroll = new TradeService().GetBankRollList("", fuid, beginTime, endTime,"", istr, imax, ref  ref_param);  //ע���˻�ͨ��qqid�޷��鵽fuid,ֱ��ͨ��fuid��ѯ��
                    else
                        this.DS_Bankroll = new TradeService().GetBankRollList(selectStr, "", beginTime, endTime,"", istr, imax, ref  ref_param);

                    ViewState["ref_param"] = ref_param;


                    if (DS_Bankroll != null && DS_Bankroll.Tables.Count != 0 && DS_Bankroll.Tables[0].Rows.Count != 0)
                    {
                        for (int i = 0; i < DS_Bankroll.Tables[0].Rows.Count; i++)
                        {
                            if (DS_Bankroll.Tables[0].Rows[i]["Fpaynum"].ToString().Trim() == "0")
                            {
                                DS_Bankroll.Tables[0].Rows[i].Delete();
                                i--;
                            }
                        }
                    }
                }

                int total;

                if (DS_Bankroll != null && DS_Bankroll.Tables.Count != 0 && DS_Bankroll.Tables[0].Rows.Count != 0)
                    total = 1000;//Int32.Parse(DS_Bankroll.Tables[0].Rows[0]["total"].ToString());
                else
                    total = 0;
                pager.RecordCount = total;
                pager.PageSize = pageSize;
                pager.CustomInfoText = "��¼������<font color=\"blue\"><b>" + pager.RecordCount.ToString() + "</b></font>";
                pager.CustomInfoText += " ��ҳ����<font color=\"blue\"><b>" + pager.PageCount.ToString() + "</b></font>";
                pager.CustomInfoText += " ��ǰҳ��<font color=\"red\"><b>" + pager.CurrentPageIndex.ToString() + "</b></font>";
            } 
            #endregion

            #region type==ListID
            else if (Request.QueryString["type"].ToString() == "ListID")
            {
                if (Session["uid"] == null)
                {
                    Response.Redirect("../login.aspx?wh=1"); //���µ�½
                }

                Query_Service.Query_Service myService = new Query_Service.Query_Service();
                myService.Finance_HeaderValue = classLibrary.setConfig.setFH(this);
                imax = 30; //����ǲ�ѯ���׵�ʱ��һ������ʾ���еļ�¼
                string selectStr = Session["ListID"].ToString();
                this.DS_Bankroll = myService.GetBankRollList_withID(beginTime, endTime, selectStr, istr, imax);
            } 
            #endregion

            #region �������
            if (DS_Bankroll != null && DS_Bankroll.Tables.Count != 0)
            {
                DS_Bankroll.Tables[0].Columns.Add("FpaynumStr", typeof(string));
                DS_Bankroll.Tables[0].Columns.Add("FbalanceStr", typeof(string));
                string[] CoverPickFuid = System.Configuration.ConfigurationManager.AppSettings["CoverPickFuid"].ToString().Split('|');

                foreach (DataRow dr in DS_Bankroll.Tables[0].Rows)
                {
                    try
                    {
                        string Fpaynum = classLibrary.setConfig.FenToYuan(dr["Fpaynum"].ToString());
                        string Fbalance = classLibrary.setConfig.FenToYuan(dr["Fbalance"].ToString());
                        dr["FpaynumStr"] = Fpaynum;
                        dr["FbalanceStr"] = Fbalance;

                        var fmemo = dr["Fmemo"] as string;  //����Bug����
                        var list= new List<string> { "1000039701", "1000030901", "1000030601", "1000040101", " 1000035801", "1000035001", "1000040901", "1000037301" }; //�⼸���̻���ע����utf-8 ���͵��ַ���
                        if (!string.IsNullOrEmpty(fmemo) && list.Contains((string)dr["Ffromid"]))
                        {
                            var buff = System.Text.Encoding.GetEncoding("gb2312").GetBytes(fmemo);
                            dr["Fmemo"] = System.Text.Encoding.UTF8.GetString(buff);
                        }

                        for (int i = 0; i < CoverPickFuid.Length; i++)
                        {
                            if (CoverPickFuid[i].ToString() == dr["Fuid"].ToString())
                            {
                                try
                                {                                  
                                    int PointIndex = Fpaynum.IndexOf(".");
                                    dr["FpaynumStr"] = "******" + Fpaynum.Substring(PointIndex - 1, Fpaynum.Length - PointIndex + 1);
                                    PointIndex = Fbalance.IndexOf(".");
                                    dr["FbalanceStr"] = "******" + Fbalance.Substring(PointIndex - 1, Fbalance.Length - PointIndex + 1);
                                }
                                catch
                                {
                                    dr["FpaynumStr"] = "******";
                                    dr["FbalanceStr"] = "******";
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("���Fpaynum��" + dr["Fnum"].ToString() + "���Fbalance��" + dr["Fbalance"].ToString() + "ת��������" + ex.Message);
                    }
                }
            } 
            #endregion
            Page.DataBind();
        }

        public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            pager.CurrentPageIndex = e.NewPageIndex;
            BindData(pager.CurrentPageIndex);
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
            this.DS_Bankroll = new System.Data.DataSet();
            this.dataTable1 = new System.Data.DataTable();
            this.dataColumn1 = new System.Data.DataColumn();
            ((System.ComponentModel.ISupportInitialize)(this.DS_Bankroll)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable1)).BeginInit();
            // 
            // DS_Bankroll
            // 
            this.DS_Bankroll.DataSetName = "NewDataSet";
            this.DS_Bankroll.Locale = new System.Globalization.CultureInfo("zh-CN");
            this.DS_Bankroll.Tables.AddRange(new System.Data.DataTable[] { this.dataTable1 });
            // 
            // dataTable1
            // 
            this.dataTable1.Columns.AddRange(new System.Data.DataColumn[] { this.dataColumn1 });
            this.dataTable1.TableName = "Table1";
            // 
            // dataColumn1
            // 
            this.dataColumn1.ColumnName = "FBKid";
            ((System.ComponentModel.ISupportInitialize)(this.DS_Bankroll)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable1)).EndInit();
        }

        #endregion
    }
}

