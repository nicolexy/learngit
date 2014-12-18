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
using System.Web.Mail;
using System.IO;

namespace TENCENT.OSS.CFT.KF.KF_Web.SysManage
{
	/// <summary>
	/// SysBulletinManage_Detail ��ժҪ˵����
	/// </summary>
    public partial class BankInterfaceManage_Detail : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Label lbTitle;
		protected System.Web.UI.WebControls.DropDownList ddlSysList;
		protected System.Web.UI.WebControls.DataGrid DataGrid1;
		protected System.Web.UI.WebControls.Button btnNew;
		protected System.Web.UI.WebControls.Button btnIssue;
        private string sysid, bulletinId, uctype;
        private static int nTime=0;//���ʱ���
        private int max = 5;//�����������
        private static bool queryed = false;//�Ƿ��Ѳ�ѯ�����ݿ��ж�������¼
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				//Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();
				//int operid = Int32.Parse(Session["OperID"].ToString());

				//if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"InfoCenter")) Response.Redirect("../login.aspx?wh=1");

				if(!classLibrary.ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}
            if (!IsPostBack)
            {
                iStartTime.Attributes.Add("onclick", "openBankModeBegin()");
                iEndTime.Attributes.Add("onclick", "openBankModeEnd()");
                iStartTime1.Attributes.Add("onclick", "openBankModeBegin1()");
                iEndTime1.Attributes.Add("onclick", "openBankModeEnd1()");
                iStartTime2.Attributes.Add("onclick", "openBankModeBegin2()");
                iEndTime2.Attributes.Add("onclick", "openBankModeEnd2()");
                iStartTime3.Attributes.Add("onclick", "openBankModeBegin3()");
                iEndTime3.Attributes.Add("onclick", "openBankModeEnd3()");
                iStartTime4.Attributes.Add("onclick", "openBankModeBegin4()");
                iEndTime4.Attributes.Add("onclick", "openBankModeEnd4()");
                this.btInterfaceAdd.Attributes.Add("onclick", "return confirm(\"��ȷ��Ҫ������������������\");");
                this.btInterfaceUpdate.Attributes.Add("onclick", "return confirm(\"��ȷ��Ҫ�����޸Ĳ���������\");");

                //��ʼ��static
                nTime=0;
                queryed = false;

                sysid = Request.QueryString["sysid"].Trim();
                ViewState["sysid"] = sysid;

                if (Request.QueryString["sysid"] != null && Request.QueryString["sysid"].Trim() != "")
                {
                    #region
                    tbStartTime.Enabled = true;
                    iStartTime.Visible = true;
                    tbEndTime.Enabled = true;
                    iEndTime.Visible = true;

                 
                    Table4.Visible = true;

                    //DropDownListShow();//����radio��ʾʱ��

                  //  setConfig.GetAllBankList(ddlQueryBankTypeInterface);

                    //���������б�
                    setConfig.GetAllBankListFromDic(ddlQueryBankTypeInterface);
                    ddlQueryBankTypeInterface.Items.Insert(0, new ListItem("��������", ""));


                    this.tbStartTime.Text = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                    this.tbEndTime.Text = DateTime.Now.ToString("yyyy-MM-dd 23:59:59");
                    if (Request.QueryString["opertype"] != null && Request.QueryString["opertype"].Trim() != "")
                    {
                        string opertype = Request.QueryString["opertype"].Trim();
                        if (opertype == "2")//�༭���ѯ
                        {
                            this.btInterfaceAdd.Visible = false;
                            this.btInterfaceUpdate.Visible = true;
                            this.btInterfaceBack.Visible = true;
                            hlInterfaceBack.Visible = false;
                        }
                        else if (opertype == "1")//����
                        {
                            this.btInterfaceAdd.Visible = true;
                            this.btInterfaceUpdate.Visible = false;
                            this.btInterfaceBack.Visible = true;
                            hlInterfaceBack.Visible = false;
                            this.InterfaceOpen.Visible = false;

                        }
                        else//����ϵͳ�����鿴
                        {
                            this.btInterfaceAdd.Visible = false;
                            this.btInterfaceUpdate.Visible = false;
                            this.btInterfaceBack.Visible = false;
                            hlInterfaceBack.Visible = true;
                            if (Request.QueryString["objid"] != null && Request.QueryString["objid"].Trim() != "")
                            {
                                string objid = Request.QueryString["objid"].Trim();
                                ViewState["objid"] = objid;
                                BindBankInterface("", objid);
                            }

                        }
                    }

                    #endregion
                }
                else
                {
                    //�����ˣ����ܲ�������ʱ��
                    Response.Redirect("../login.aspx?wh=1");
                }

                if (Request.QueryString["bulletinId"] != null && Request.QueryString["bulletinId"].Trim() != "")//�༭�鿴
                {
                    bulletinId = Request.QueryString["bulletinId"].Trim();
                    ViewState["bulletinId"] = bulletinId;
                }
                else//����
                {
                    bulletinId = "";
                    ViewState["bulletinId"] = bulletinId;
                }

                if (bulletinId == "")
                {

                    labTitle.Text = "���нӿ�����";


                    if (ViewState["objid"] != null)
                    {
                        labTitle.Text = "�鿴���нӿ�";
                    }
                }
                else
                {
                    labTitle.Text = "���нӿ��޸�";
                    BindBankInterface(bulletinId, "");
                }
            }
            else
            {
                bulletinId = ViewState["bulletinId"].ToString();
                sysid = ViewState["sysid"].ToString();
            }

            if (ViewState["sysid"].ToString() != "6")
            {
                this.TR1.Visible = false;
            }
		}


        protected void BindBankInterface(string bulletinId, string objid)
        {
            try
            {
                Query_Service.Query_Service qs = new Query_Service.Query_Service();
                string msg = "";
                Query_Service.T_BANKBULLETIN_INFO bankbulletin = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.T_BANKBULLETIN_INFO();
                DataSet ds = new DataSet();
                if (bulletinId != null && bulletinId != "")
                {
                    ds = qs.QueryBankBulletin(int.Parse(ViewState["sysid"].ToString()), 0, 0, bulletinId,5,0);//ͨ���ӿڷ��ؼ�¼
                    bankbulletin = qs.TurnBankBulletinClass(ds);//����¼ת���ɹ�����
                }
                if (objid != null && objid != "")
                {
                    bankbulletin = qs.QueryBankBulletinByObjid(objid, "BankBulletinForType", out msg);
                }
                if (bankbulletin == null)
                {
                    WebUtils.ShowMessage(this.Page, "��ѯ����");
                    return;
                }

                this.ddlQueryBankTypeInterface.SelectedValue = bankbulletin.banktype.Trim();
                this.tbInterfaceMainText.Text = bankbulletin.maintext.Trim().Replace("<br/>", "\r\n");
                this.tbStartTime.Text = bankbulletin.startime.Trim();
                this.tbEndTime.Text = bankbulletin.endtime.Trim();
           //     this.tbalwtime.Text = bankbulletin.Falwtime.Trim();//��Чʱ���
                this.tbcreateuserInterface.Text = bankbulletin.createuser.Trim();
                this.InterfaceOpen.Visible = true;
                this.tbbusinetype.Text = bankbulletin.businesstype.Trim();
                this.TextTCMainText.Text = bankbulletin.popuptext.Trim();
                this.tbbulletin_id.Text = bankbulletin.bulletin_id.Trim();
                if (bankbulletin.closetype.Trim() == "1")
                {
                  //  this.cbForbid.Checked = true;
                    this.ForbidRadio.SelectedValue = "1";
                    tcTextId.Visible = false;
                }
                else if (bankbulletin.closetype.Trim() == "2")
                {
                    this.ForbidRadio.SelectedValue = "2";
                    tcTextId.Visible = true;
                }
                else if (bankbulletin.closetype.Trim() == "3")
                {
                    this.ForbidRadio.SelectedValue = "3";
                    tcTextId.Visible = false;
                }
                else
                {
                    throw new Exception("�ر�����" + bankbulletin.closetype.Trim() + "����֪��");
                }
                this.closedRadio.SelectedValue = bankbulletin.op_support_flag.Trim();//������
                this.tbTitle.Text = bankbulletin.title;//����
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
            this.InterfaceOpen.CheckedChanged += new System.EventHandler(this.InterfaceOpen_CheckedChanged);
          //  this.cbForbid.CheckedChanged += new System.EventHandler(this.cbForbid_CheckedChanged);
		}
		#endregion

        protected void btInterfaceAdd_Click(object sender, System.EventArgs e)
        {
            try
            {
                string sysid = ViewState["sysid"].ToString();
                if (this.tbInterfaceMainText.Text.Trim().Length > 50)
                {
                    WebUtils.ShowMessage(this.Page, "���ĳ���50�ַ�,���������룡");
                    return;
                }
                if (this.TextTCMainText.Text.Trim().Length > 80)
                {
                    WebUtils.ShowMessage(this.Page, "�������ĳ���80�ַ�,���������룡");
                    return;
                }

                Query_Service.T_BANKBULLETIN_INFO bankbulletin = new Query_Service.T_BANKBULLETIN_INFO();
                bankbulletin.IsNew = true;
                bankbulletin.IsOPen = false;
                bankbulletin.banktype = this.ddlQueryBankTypeInterface.SelectedValue.Trim();
                bankbulletin.businesstype = sysid;
                if (sysid == "6")//֧���������ͣ�1.ȫ���ر�  2.ǩԼ  3.֧��  4.�˿�
                {
                    bankbulletin.op_support_flag = this.closedRadio.SelectedValue;
                }
                else
                    bankbulletin.op_support_flag = "1";

                //�Ƿ�Ӱ��ӿ�
                //if (this.cbForbid.Checked == true)
                //    bankbulletin.closetype = "1";
                //else
                //    bankbulletin.closetype = "2";

                if (this.ForbidRadio.SelectedValue == "1")
                    bankbulletin.closetype = "1";
                else if (this.ForbidRadio.SelectedValue == "2")
                    bankbulletin.closetype = "2";
                else if (this.ForbidRadio.SelectedValue == "3")
                    bankbulletin.closetype = "3";

                bankbulletin.title = tbTitle.Text.Trim() ;
                bankbulletin.maintext = this.tbInterfaceMainText.Text.Replace("\r\n", "<br/>").Replace("\r", "<br/>").Replace("\n", "<br/>");
                bankbulletin.popuptext = TextTCMainText.Text.Trim();
                bankbulletin.createuser = Session["uid"].ToString();
                bankbulletin.createtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                bankbulletin.updateuser = Session["uid"].ToString();
                bankbulletin.updatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                if (bankbulletin.closetype == "2")//��رգ������е��㣬���⼰���������һ��������ֻȷ�����㼴��
                {
                    if (bankbulletin.popuptext == "" || bankbulletin.popuptext == null)
                    {
                        WebUtils.ShowMessage(this.Page, "��رձ������뵯�����ģ�");
                        return;
                    }
                }
                else
                {
                    bankbulletin.popuptext = "";//Ӳ�رտɲ����뵯��
                }

                //������������
                string bankName = "";
                try
                {
                    bankName = bankbulletin.maintext.Substring(0, bankbulletin.maintext.IndexOf("����"));//ȡ��������
                }
                catch
                {
                    WebUtils.ShowMessage(this.Page, "�밴ģ����д���棡");
                    return;
                }

                string[] startTime = new string[nTime + 1];
                string[] endTime = new string[nTime + 1];

                //��ȡʱ������
                for (int i = 0; i <= nTime; i++)//nTime���4
                {
                    #region
                    try
                    {
                        if (i == 0)
                        {
                            startTime[i] = DateTime.Parse(this.tbStartTime.Text.Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                            endTime[i] = DateTime.Parse(this.tbEndTime.Text.Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        if (i == 1)
                        {
                            startTime[i] = DateTime.Parse(this.tbStartTime1.Text.Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                            endTime[i] = DateTime.Parse(this.tbEndTime1.Text.Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        if (i == 2)
                        {
                            startTime[i] = DateTime.Parse(this.tbStartTime2.Text.Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                            endTime[i] = DateTime.Parse(this.tbEndTime2.Text.Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        if (i == 3)
                        {
                            startTime[i] = DateTime.Parse(this.tbStartTime3.Text.Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                            endTime[i] = DateTime.Parse(this.tbEndTime3.Text.Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        if (i == 4)
                        {
                            startTime[i] = DateTime.Parse(this.tbStartTime4.Text.Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                            endTime[i] = DateTime.Parse(this.tbEndTime4.Text.Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                    }
                    catch
                    {
                        WebUtils.ShowMessage(this.Page, "��������ȷ������");
                        return;
                    }
                    #endregion

                }

                string msg = "";
                if (IsRepeatedSelf(startTime, endTime, out msg))
                { //�ж�����ʱ��������ظ�
                    WebUtils.ShowMessage(this.Page, msg);
                    return;
                }

                //if (IsRepeated("", startTime, endTime, out msg))//������ʱ�����ݿⲻ��������bulletin_idһ���ļ�¼
                //{ //�ж����ݿ��Ƿ�����ظ�ʱ��εĹ���
                //    WebUtils.ShowMessage(this.Page, msg);
                //    return;
                //}

                string objid = "";
                //����
                #region
                for (int i = 0; i <= nTime; i++)//nTime���4
                {
                    try
                    {
                        objid = System.DateTime.Now.ToString("yyyyMMddHHmmss") + PublicRes.StaticNoManage(); ;//ÿ��������
                        bankbulletin.returnUrl = "http://kf.cf.com/SysManage/BankInterfaceManage_Detail.aspx?Fbanktype=" + this.ddlQueryBankTypeInterface.SelectedValue.Trim() + "&sysid=" + sysid + "&objid=" + objid + "&opertype=0";
                        bankbulletin.bulletin_id = System.DateTime.Now.ToString("yyyyMMddHHmmss") + PublicRes.NewStaticNoManage();//ÿ�����й���ID
                        bankbulletin.startime = startTime[i];
                        bankbulletin.endtime = endTime[i];

                        if (i != 0)
                        {
                            bankbulletin.maintext = bankName + "����ϵͳά���У�Ԥ��" + DateTime.Parse(bankbulletin.endtime).ToString("MM��dd��HH:mm") + "�ָ���";
                            if (bankbulletin.closetype == "2")
                            {
                                bankbulletin.popuptext = DateTime.Parse(bankbulletin.startime).ToString("MM��dd��HH:mm") + "��" + DateTime.Parse(bankbulletin.endtime).ToString("MM��dd��HH:mm") +
                                    "��" + bankName + "����ϵͳά�������ڼ�����ĸ���ӳٵ�" + DateTime.Parse(bankbulletin.endtime).AddDays(1).ToString("MM��dd��") + "����";
                            }
                            else
                            {
                                bankbulletin.popuptext = "";
                            }
                        }


                        string[,] param = GetBulletin(bankbulletin);//��õ�������

                        Check_WebService.Param[] pa = ToParamArray(param);

                        string memo = "�������нӿ�ά����Ϣ,��������:" + this.ddlQueryBankTypeInterface.SelectedItem.Text.Trim(); ;
                        PublicRes.CreateCheckService(this).StartCheck(objid,
                            "BankBulletinForType", memo, "0", pa);
                        PublicRes.writeSysLog(Session["uid"].ToString(), Request.UserHostAddress, "BankBulletinForType", "�������нӿ�ά����������", 1, this.ddlQueryBankTypeInterface.SelectedValue.Trim(), "");
                    }
                    catch
                    {
                        WebUtils.ShowMessage(this.Page, "��ά��ʱ���" + bankbulletin.startime + "--" + bankbulletin.endtime + "ʧ�ܣ����ѯ���ټ����������޸ģ�");
                        return;
                    }
                }
                #endregion

                nTime = 0;
                queryed = false;//��Ҫ���²�ѯ���ݿ�ȷ�����ڼ�������

                TimesHide();

                WebUtils.ShowMessage(this.Page, "ȫ���������нӿ�ά���������ύ����ȴ�������");
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

        protected void btInterfaceUpdate_Click(object sender, System.EventArgs e)
        {
            try
            {
                string sysid = ViewState["sysid"].ToString();
                if (this.tbInterfaceMainText.Text.Trim().Length > 50)
                {
                    WebUtils.ShowMessage(this.Page, "���ĳ���50�ַ�,���������룡");
                    return;
                }
                if (this.TextTCMainText.Text.Trim().Length > 80)
                {
                    WebUtils.ShowMessage(this.Page, "�������ĳ���80�ַ�,���������룡");
                    return;
                }

                Query_Service.T_BANKBULLETIN_INFO bankbulletin = new Query_Service.T_BANKBULLETIN_INFO();
                bankbulletin.bulletin_id = this.tbbulletin_id.Text.Trim();
                bankbulletin.IsNew = false;
                bankbulletin.IsOPen = this.InterfaceOpen.Checked;
                bankbulletin.banktype = this.ddlQueryBankTypeInterface.SelectedValue.Trim();
                bankbulletin.businesstype = sysid;
                if (sysid == "6")//֧���������ͣ�1.ȫ���ر�  2.ǩԼ  3.֧��  4.�˿�
                {
                    bankbulletin.op_support_flag = this.closedRadio.SelectedValue;
                }
                else
                    bankbulletin.op_support_flag = "1";
                //�Ƿ�Ӱ��ӿ�
                //if (this.cbForbid.Checked == true)
                //    bankbulletin.closetype = "1";
                //else
                //    bankbulletin.closetype = "2";
                if (this.ForbidRadio.SelectedValue == "1")
                    bankbulletin.closetype = "1";
                else if (this.ForbidRadio.SelectedValue == "2")
                    bankbulletin.closetype = "2";
                else if (this.ForbidRadio.SelectedValue == "3")
                    bankbulletin.closetype = "3";

                bankbulletin.title = tbTitle.Text.Trim();
                bankbulletin.maintext = this.tbInterfaceMainText.Text.Replace("\r\n", "<br/>").Replace("\r", "<br/>").Replace("\n", "<br/>");
                bankbulletin.popuptext = TextTCMainText.Text.Trim();
                bankbulletin.createuser = Session["uid"].ToString();
                bankbulletin.createtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                bankbulletin.updateuser = Session["uid"].ToString();
                bankbulletin.updatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                if (bankbulletin.closetype == "2")//��رգ������е��㣬���⼰���������һ��������ֻȷ�����㼴��
                {
                    if (bankbulletin.popuptext == "" || bankbulletin.popuptext == null)
                    {
                        WebUtils.ShowMessage(this.Page, "��رձ������뵯�����ģ�");
                        return;
                    }
                }

                //����
                string bankName = "";
                try
                {
                    bankName = bankbulletin.maintext.Substring(0, bankbulletin.maintext.IndexOf("����"));
                }
                catch
                {
                    WebUtils.ShowMessage(this.Page, "�밴ģ����д���棡");
                    return;
                }

                if (this.InterfaceOpen.Checked)//������������ֻ���޸ı�������
                {
                    nTime = 0;

                }

                string[] startTime = new string[nTime + 1];
                string[] endTime = new string[nTime + 1];

                for (int i = 0; i <= nTime; i++)//nTime���4
                {
                    #region
                    try
                    {
                        if (i == 0)
                        {
                            startTime[i] = DateTime.Parse(this.tbStartTime.Text.Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                            endTime[i] = DateTime.Parse(this.tbEndTime.Text.Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        if (i == 1)
                        {
                            startTime[i] = DateTime.Parse(this.tbStartTime1.Text.Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                            endTime[i] = DateTime.Parse(this.tbEndTime1.Text.Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        if (i == 2)
                        {
                            startTime[i] = DateTime.Parse(this.tbStartTime2.Text.Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                            endTime[i] = DateTime.Parse(this.tbEndTime2.Text.Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        if (i == 3)
                        {
                            startTime[i] = DateTime.Parse(this.tbStartTime3.Text.Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                            endTime[i] = DateTime.Parse(this.tbEndTime3.Text.Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        if (i == 4)
                        {
                            startTime[i] = DateTime.Parse(this.tbStartTime4.Text.Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                            endTime[i] = DateTime.Parse(this.tbEndTime4.Text.Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                    }
                    catch
                    {
                        WebUtils.ShowMessage(this.Page, "��������ȷ������");
                        return;
                    }
                    #endregion
                }

                string msg = "";
                if (IsRepeatedSelf(startTime, endTime, out msg))
                { //�ж�����ʱ��������ظ�
                    WebUtils.ShowMessage(this.Page, msg);
                    return;
                }

                //if (IsRepeated(bankbulletin.bulletin_id, startTime, endTime, out msg))//������ʱ�����ݿⲻ��������bulletin_idһ���ļ�¼
                //{ //�ж����ݿ��Ƿ�����ظ�ʱ��εĹ���
                //    WebUtils.ShowMessage(this.Page, msg);
                //    return;
                //}
                string objid = "";
                //����
                #region
                for (int i = 0; i <= nTime; i++)//nTime���4
                {
                    try
                    {
                        bankbulletin.startime = startTime[i];
                        bankbulletin.endtime = endTime[i];
                        if (this.InterfaceOpen.Checked)//���������������ʱ��Ϊ��ǰʱ��
                            bankbulletin.endtime = DateTime.Now.AddMinutes(5).ToString("yyyy-MM-dd HH:mm:ss");

                        if (i != 0)//i=0Ϊ�޸ģ�����Ϊ����
                        {
                            bankbulletin.IsNew = true;
                            bankbulletin.bulletin_id = System.DateTime.Now.ToString("yyyyMMddHHmmss") + PublicRes.NewStaticNoManage();
                            bankbulletin.maintext = bankName + "����ϵͳά���У�Ԥ��" + DateTime.Parse(bankbulletin.endtime).ToString("MM��dd��HH:mm") + "�ָ���";
                            if (bankbulletin.closetype == "2")
                            {
                                bankbulletin.popuptext = DateTime.Parse(bankbulletin.startime).ToString("MM��dd��HH:mm") + "��" + DateTime.Parse(bankbulletin.endtime).ToString("MM��dd��HH:mm") +
                                    "��" + bankName + "����ϵͳά�������ڼ�����ĸ���ӳٵ�" + DateTime.Parse(bankbulletin.endtime).AddDays(1).ToString("MM��dd��") + "����";
                            }
                            else
                            {
                                bankbulletin.popuptext = "";
                            }
                            bankbulletin.createtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        objid = System.DateTime.Now.ToString("yyyyMMddHHmmss") + PublicRes.StaticNoManage();//ÿ��������
                        bankbulletin.returnUrl = "http://kf.cf.com/SysManage/BankInterfaceManage_Detail.aspx?Fbanktype=" + this.ddlQueryBankTypeInterface.SelectedValue.Trim() + "&sysid=" + sysid + "&objid=" + objid + "&opertype=0";
                       

                        string[,] param = GetBulletin(bankbulletin);//��õ�������

                        Check_WebService.Param[] pa = ToParamArray(param);

                        string memo = "�������нӿ�ά����Ϣ,��������:" + this.ddlQueryBankTypeInterface.SelectedItem.Text.Trim(); ;

                        PublicRes.CreateCheckService(this).StartCheck(objid,
                            "BankBulletinForType", memo, "0", pa);
                        PublicRes.writeSysLog(Session["uid"].ToString(), Request.UserHostAddress, "BankBulletinForType", "�������нӿ�ά����������", 1, this.ddlQueryBankTypeInterface.SelectedValue.Trim(), "");
                    }
                    catch(Exception err)
                    {
                        WebUtils.ShowMessage(this.Page, "��ά��ʱ���" + bankbulletin.startime + "--" + bankbulletin.endtime + "ʧ�ܣ����ѯ���ټ����������޸ģ�����" + err.Message);
                        return;
                    }
                }
                #endregion
                nTime = 0;
                queryed = false;//��Ҫ���²�ѯ���ݿ�ȷ�����ڼ�������
                TimesHide();
                WebUtils.ShowMessage(this.Page, "ȫ���޸Ļ��������нӿ�ά���������ύ����ȴ�������");

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

        //�ж����ݿ��Ƿ�����ظ�ʱ��εĹ���
        protected bool IsRepeated(string bulletin_id,string[] startTime, string[] endTime, out string msg)
        {
            msg = "";
            bool index = false;
            try
            {
                int len = startTime.Length;
                if (startTime.Length != endTime.Length)
                {
                    msg = "�ж����ݿ��Ƿ�����ظ�ʱ��εĹ������";
                }

                QueryBankBulletinOfBusiness(int.Parse(ViewState["sysid"].ToString()), int.Parse(this.closedRadio.SelectedValue), int.Parse(this.ddlQueryBankTypeInterface.SelectedValue.Trim()));

                DataTable g_dt = (DataTable)ViewState["g_dt"];

                if (g_dt != null)
                {
                   
                    //���ж�
                    for (int i = 0; i <= nTime; i++)//nTime���4
                    {
                        DateTime start = DateTime.Parse(startTime[i]);//�������濪ʼʱ��
                        DateTime end = DateTime.Parse(endTime[i]);//�����������ʱ��
                       
                        foreach (DataRow dr in g_dt.Rows)
                        {
                            string dbBulletin_id = dr["bulletin_id"].ToString();
                            if (bulletin_id == dbBulletin_id)//���뱻�޸ĵ�������¼���Ƚ�
                            {
                                continue;
                            }
                            DateTime dbStart = DateTime.Parse(dr["startime"].ToString());
                            DateTime dbEnd = DateTime.Parse(dr["endtime"].ToString());
                            if (end <= dbStart || dbEnd <= start)
                            {
                            }
                            else
                            {
                                msg = "�����ݿ⹫��ʱ���" + dr["startime"].ToString() + "--" + dr["endtime"].ToString() + "�ظ���";
                                index = true;
                                return index;
                            }
                        }
                    }
                }
                return index;
            }
            catch (Exception e)
            {
                msg = "�ж����ݿ��Ƿ�����ظ�ʱ��εĹ������";
                throw new Exception(msg + e.Message);
            }
        }

        //�ж�����ʱ��������ظ�
        protected bool IsRepeatedSelf(string[] startTime, string[] endTime, out string msg)
        {
            bool index = false;
            msg = "";
            try
            {
                int len = startTime.Length;
                if (startTime.Length != endTime.Length)
                {
                    msg = "�ж�����ʱ��������ظ�����";
                }
                for (int i = 0; i < len; i++)
                {
                    DateTime start = DateTime.Parse(startTime[i]);
                    DateTime end = DateTime.Parse(endTime[i]);
                    for (int j = 0; j < len; j++)
                    {
                        DateTime start2 = DateTime.Parse(startTime[j]);
                        DateTime end2 = DateTime.Parse(endTime[j]);
                        if (i == j)
                            continue;
                        else
                        {
                            if (end <= start2 || end2 <= start)
                            {
                            }
                            else
                            {
                                msg = "����ʱ���" + start.ToString("yyyy-MM-dd HH:mm:ss") + "--" + end.ToString("yyyy-MM-dd HH:mm:ss") + "��"
                                    + start2.ToString("yyyy-MM-dd HH:mm:ss") + "--" + end2.ToString("yyyy-MM-dd HH:mm:ss") + "�ظ���";
                                index = true;
                                return index;
                            }
                        }
                    }
                }
                return index;
            }
            catch (Exception e)
            {
                msg = "�ж�����ʱ��������Ƿ��ظ�����";
                throw new Exception(msg+e.Message);
            }
        }
        
        //���������й����������ӵ�ʱ���
        private void TimesHide()
        {
            this.StartTime1.Visible = false;
            this.EndTime1.Visible = false;
            this.StartTime2.Visible = false;
            this.EndTime2.Visible = false;
            this.StartTime3.Visible = false;
            this.EndTime3.Visible = false;
            this.StartTime4.Visible = false;
            this.EndTime4.Visible = false;
        }

        protected void btInterfaceBack_Click(object sender, System.EventArgs e)
        {
            Response.Redirect("BankInterfaceManage.aspx?sysid=" + sysid);
        }

        protected void ddlQueryBankTypeInterface_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            this.lbbanktypeInterface.Text = this.ddlQueryBankTypeInterface.SelectedValue.Trim();
        }

        private void InterfaceOpen_CheckedChanged(object sender, System.EventArgs e)
		{
            if (InterfaceOpen.Checked)
			{
                tbStartTime.Enabled = false;
                iStartTime.Visible = false;
                tbEndTime.Enabled = false;
                iEndTime.Visible = false;
                this.TRTimeSet.Visible = false;
                this.StartTime1.Visible = false;
                this.EndTime1.Visible = false;
                this.StartTime2.Visible = false;
                this.EndTime2.Visible = false;
                this.StartTime3.Visible = false;
                this.EndTime3.Visible = false; 
                this.StartTime4.Visible = false;
                this.EndTime4.Visible = false;
                nTime = 0;
			}
			else
			{
                tbStartTime.Enabled = true;
                iStartTime.Visible = true;
                tbEndTime.Enabled = true;
                iEndTime.Visible = true;
                this.TRTimeSet.Visible = true;
			}
		}

        ////�������Ŀ���
        //private void cbForbid_CheckedChanged(object sender, System.EventArgs e)
        //{
        //    if (cbForbid.Checked)
        //    {
        //      //  TextTCMainText.Text = "";//������������ʱ���ύ�����������Ϣ
        //        tcTextId.Visible = false;
        //    }
        //    else
        //        tcTextId.Visible = true;
        //}

        protected void closedRadio_SelectedIndexChanged(object sender, System.EventArgs e)
        {
           
        }
        protected void ForbidRadio_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (this.ForbidRadio.SelectedValue == "2")
            {
                tcTextId.Visible = true;
            }
            else
            {
                //  TextTCMainText.Text = "";//������������ʱ���ύ�����������Ϣ
                tcTextId.Visible = false;
            }
        }

        protected void btAddTime_Click(object sender, System.EventArgs e)
        {
            int dbN = 0;
            if (!queryed)//����û��ѯ�����ݿ��¼�����Ȳ�ѯ
            {
                QueryBankBulletinOfBusiness(int.Parse(ViewState["sysid"].ToString()), int.Parse(this.closedRadio.SelectedValue), int.Parse(this.ddlQueryBankTypeInterface.SelectedValue.Trim()));
            }
           
            DataTable g_dt = (DataTable)ViewState["g_dt"];
            if (g_dt == null)//��¼��Ϊ0
                dbN = 0;
            else
                dbN = g_dt.Rows.Count;

            if (max - dbN - nTime > 0)
            {
                nTime++;//ÿ��һ�ΰ�ťʱ��μ�һ��i
                if (nTime == 1)
                {
                    StartTime1.Visible = true;
                    EndTime1.Visible = true;
                    this.tbStartTime1.Text = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                    this.tbEndTime1.Text = DateTime.Now.ToString("yyyy-MM-dd 23:59:59");
                }
                if (nTime == 2)
                {
                    StartTime2.Visible = true;
                    EndTime2.Visible = true;
                    this.tbStartTime2.Text = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                    this.tbEndTime2.Text = DateTime.Now.ToString("yyyy-MM-dd 23:59:59");
                }
                if (nTime == 3)
                {
                    StartTime3.Visible = true;
                    EndTime3.Visible = true;
                    this.tbStartTime3.Text = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                    this.tbEndTime3.Text = DateTime.Now.ToString("yyyy-MM-dd 23:59:59");
                }
                if (nTime == 4)
                {
                    StartTime4.Visible = true;
                    EndTime4.Visible = true;
                    this.tbStartTime4.Text = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                    this.tbEndTime4.Text = DateTime.Now.ToString("yyyy-MM-dd 23:59:59");
                }
            }
            else
            {
                WebUtils.ShowMessage(this.Page, "��������������");
                return;
            }

        }

        //����
        protected string[,] GetBulletin(Query_Service.T_BANKBULLETIN_INFO bankbulletin)
        {
            string[,] param = new string[,] { { "IsNew", Convert.ToString(bankbulletin.IsNew) }, 
            { "IsOPen", Convert.ToString(bankbulletin.IsOPen) },
            { "bulletin_id", Convert.ToString(bankbulletin.bulletin_id) },
            { "banktype", Convert.ToString(bankbulletin.banktype) },
            { "businesstype", Convert.ToString(bankbulletin.businesstype) },
            { "op_support_flag", Convert.ToString(bankbulletin.op_support_flag) },
            { "closetype", Convert.ToString(bankbulletin.closetype) },
            { "title", Convert.ToString(bankbulletin.title) },
            { "maintext", Convert.ToString(bankbulletin.maintext) },
            { "popuptext", Convert.ToString(bankbulletin.popuptext) },
            { "startime", Convert.ToString(bankbulletin.startime) },
            { "endtime", Convert.ToString(bankbulletin.endtime) },
            { "createuser", Convert.ToString(bankbulletin.createuser) },
            { "createtime", Convert.ToString(bankbulletin.createtime) },
            { "updateuser", Convert.ToString(bankbulletin.updateuser) },
            //{ "Falwtime", Convert.ToString(bankbulletin.Falwtime) },
            { "returnUrl", Convert.ToString(bankbulletin.returnUrl) }};
            return param;
        }

        //��ȡĳ���е�ҵ�������ͣ����ƹ���������ʱ����ظ�
        protected void QueryBankBulletinOfBusiness(int businesstype, int op_flag, int banktype)
        {
            Query_Service.Query_Service qs = new Query_Service.Query_Service();
            Query_Service.T_BANKBULLETIN_INFO bankbulletin = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.T_BANKBULLETIN_INFO();
            DataSet ds = qs.QueryBankBulletin(businesstype, op_flag, banktype, "",8,0);//ͨ���ӿڷ������������ͼ�¼��һ�㲻����5��
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ViewState["g_dt"] = ds.Tables[0];
                queryed = true;
            }
        }

        //protected void InterfaceLong_SelectedIndexChanged(object sender, System.EventArgs e)
        //{
        //    if (!InterfaceOpen.Checked)
        //        DropDownListShow();
        //}

        //private void DropDownListShow()
        //{
        //    if (this.InterfaceLong.SelectedValue == "1")
        //    {
        //        this.alwtime.Visible = true;
        //        this.StartTime.Visible = false;
        //        this.EndTime.Visible = false;
        //    }
        //    else
        //    {
        //        this.alwtime.Visible = false;
        //        this.StartTime.Visible = true;
        //        this.EndTime.Visible = true;
        //    }
        //}

        private static Check_WebService.Param[] ToParamArray(string[,] param)
        {
            Check_WebService.Param[] pa = new Check_WebService.Param[param.GetLength(0)];
            for (int i = 0; i < pa.Length; i++)
            {
                pa[i] = new Check_WebService.Param();
                pa[i].ParamName = param[i, 0];
                pa[i].ParamValue = Convert.ToString(param[i, 1]);
                pa[i].ParamFlag = "";
            }
            return pa;
        }
	}
}