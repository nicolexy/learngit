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
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web;
using System.Web.Services.Protocols;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using System.Xml;
using CFT.CSOMS.BLL.ForeignCardModule;
using TENCENT.OSS.CFT.KF.KF_Web.InternetBank;
using CFT.CSOMS.BLL.TradeModule;

namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
    /// <summary>
    /// QueryYTTrade ��ժҪ˵����
    /// </summary>
    public partial class BeforeCancelTradeQuery : TENCENT.OSS.CFT.KF.KF_Web.PageBase
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                Label1.Text = Session["uid"].ToString();
                string szkey = Session["SzKey"].ToString();

                if (!IsPostBack)
                {
                    if (!classLibrary.ClassLib.ValidateRight("TradeManagement", this)) Response.Redirect("../login.aspx?wh=1");
                }
            }
            catch
            {
                Response.Redirect("../login.aspx?wh=1");
            }
        }

        public void btnQuery_Click(object sender, System.EventArgs e)
        {
            try
            {
                string uid = txtuid.Text.Trim();
                string start_time_str = txt_start_time.Value.Trim();
                string end_time_str = txt_end_time.Value.Trim();
                DateTime start_time, end_time;
                DataSet ds = null;
                if (uid.Length < 1)
                {
                    throw new Exception("�ڲ�ID�� ������Ϊ��");
                }

                if (start_time_str.Length > 0 && end_time_str.Length > 0)
                {
                    if (DateTime.TryParse(start_time_str, out start_time) && DateTime.TryParse(end_time_str, out end_time))
                    {
                        var day = (end_time - start_time).Days;
                        if (day < 1)
                        {
                            throw new Exception("����ʱ��Ӧ�� ���� ��ʼʱ��");
                        }
                        if (start_time.Year != end_time.Year || day > 30)
                        {
                            throw new Exception("����ʱ�䲻�������,���ҿ����30����");
                        }
                        ds = new TradeService().BeforeCancelTradeHistoryQuery(uid, start_time, end_time);
                    }
                    else
                    {
                        throw new Exception("��������ȷ��ʱ��");
                    }
                }
                else
                {
                    ds = new TradeService().BeforeCancelTradeQuery(uid);
                }

                if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                {
                    WebUtils.ShowMessage(this.Page, "û���ҵ���¼");
                    dg_info.DataSource = null;
                }
                else
                {
                    var dt = ds.Tables[0];

                    dt.Columns.Add("Fpaynum_str", typeof(string));
                    classLibrary.setConfig.FenToYuan_Table(dt, "Fpaynum", "Fpaynum_str");
                    dg_info.DataSource = dt;
                }
                dg_info.DataBind();

            }
            catch (SoapException eSoap) //����soap���쳣
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "���÷������" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }
        }
    }
}