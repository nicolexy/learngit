using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using CFT.CSOMS.BLL.UserAppealModule;

namespace TENCENT.OSS.CFT.KF.KF_Web.Control
{
    public partial class UserAppealCheckPwdControl : System.Web.UI.UserControl
    {
        protected System.Web.UI.WebControls.Label lblTurnBig;
        protected System.Web.UI.WebControls.Label lblTurnShort;
        protected System.Web.UI.WebControls.Label lblTurnBack;

        public DataRow dr;
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public void BindData()
        {
            string ftype = dr["FType"].ToString();
            if (ftype == "1" || ftype == "5" || ftype == "6" || ftype == "99")
            {
                lbldb.Text = dr["DBName"].ToString();
                lbltb.Text = dr["tableName"].ToString();
            }
            else
            {
                lbldb.Text = "";
                lbltb.Text = "";
            }

            //lxl 20131111 为了后面传参的必要
            lblftype.Text = ftype;
            lbldb.Visible = false;
            lbltb.Visible = false;
            lblftype.Visible = false;

            //读取内容，读出QQ号后，就要读取后台信息。
            lblfid.Text = dr["Fid"].ToString();

            //labFuin.Text = dr["Fuin"].ToString();  //不要了,重复展示

            labFTypeName.Text = dr["FTypeName"].ToString();
            cre_id.Text = dr["cre_id"].ToString();
            new_cre_id.Text = dr["new_cre_id"].ToString();

            cre_type.Text = GetCreType(dr["cre_type"].ToString());

            email.Text = dr["Femail"].ToString();

            labFstatename.Text = dr["FStateName"].ToString();

            if (dr["clear_pps"].ToString() == "1" && dr["FType"].ToString() == "1")
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
            //tbFCheckInfo.Text = PublicRes.GetString(dr["FCheckInfo"]);

            if (dr["FType"].ToString() == "3")
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
            
            try
            {
                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                DataSet dsuser = new UserAppealService().GetAppealUserInfo(dr["Fuin"].ToString());
                if (dsuser == null || dsuser.Tables.Count == 0 || dsuser.Tables[0].Rows.Count != 1)
                {
                    labFQQid.Text = "读取数据有误";
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

                    if (dr["FType"].ToString() == "3")
                    {
                        labFtruename.Text = PublicRes.GetString(druser["Fcompany_name"]);
                    }


                    //2.查询实名认证
                    bool stateMsg = false;
                    DataSet authenState = new UserAppealService().GetUserAuthenState(druser["Fqqid"].ToString(), "", 0, out stateMsg);
                    if (stateMsg)
                    {
                        lbauthenState.Text = "是";
                    }
                    else
                    {
                        lbauthenState.Text = "否";
                    }
                }
            }
            catch (Exception err)
            {
                labFQQid.Text = "读取数据有误：" + PublicRes.GetErrorMsg(err.Message);
            }
        }

        private string GetCreType(string creid)
        {
            if (creid == null || creid.Trim() == "")
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

            if (icreid >= 1 && icreid <= 11)
            {
                if (icreid == 1)
                {
                    return "身份证";
                }
                else if (icreid == 2)
                {
                    return "护照";
                }
                else if (icreid == 3)
                {
                    return "军官证";
                }
                else if (icreid == 4)
                {
                    return "士兵证";
                }
                else if (icreid == 5)
                {
                    return "回乡证";
                }
                else if (icreid == 6)
                {
                    return "临时身份证";
                }
                else if (icreid == 7)
                {
                    return "户口簿";
                }
                else if (icreid == 8)
                {
                    return "警官证";
                }
                else if (icreid == 9)
                {
                    return "台胞证";
                }
                else if (icreid == 10)
                {
                    return "营业执照";
                }
                else if (icreid == 11)
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

        public void Clean()
        {
            lblfid.Text = "";
            this.rbtnOK.Checked = false;
            this.rbtnReject.Checked = false;
            this.rbtnDelete.Checked = false;
            this.rbtnSub.Checked = false;
           
        }

        public DataRow _dr
        {
            get
            {
                return dr;
            }
            set
            {
                dr = value;
            }
        }

        public string SubmitType
        {           
            get
            {
                if (this.rbtnOK.Checked)
                    return "1";
                else if (this.rbtnReject.Checked)
                    return "2";
                else if (this.rbtnDelete.Checked)
                    return "3";
                else if (this.rbtnSub.Checked)
                    return "4";

                return "";
            }
        }
        public string fid
        {
            get
            {
                return lblfid.Text;
            }
        }

        public string db
        {
            get
            {
                return lbldb.Text;
            }
        }

        public string tb
        {
            get
            {
                return lbltb.Text;
            }
        }

        public string ftype
        {
            get
            {
                return lblftype.Text;
            }
        }

        // 获取拒绝原因
        public bool GetRejectReason(out string reason, out string otherReason)
        {
            reason = ""; otherReason = "";           
            otherReason = classLibrary.setConfig.replaceMStr(tbFCheckInfo.Text.Trim());           
            if (otherReason.Trim() == "")
                throw new Exception("请选择拒绝原因！");

            return true;
        }


        private void ShowMsg(string msg)
        {
            Response.Write("<script language=javascript>alert('" + msg + "')</script>");
        }

        public string Comment
        {
            get
            {
                return tbComment.Text.Trim();
            }
        }
    }
}