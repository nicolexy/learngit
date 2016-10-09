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
using CFT.CSOMS.BLL.UserAppealModule;
using CFT.CSOMS.BLL.CFTAccountModule;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
    public partial class CFTUserCheckPwd : PageBase
    {
        public string uid;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                uid = Session["uid"] as string;
                string szkey = Session["SzKey"].ToString();
                if (!classLibrary.ClassLib.ValidateRight("InfoCenter", this)) Response.Redirect("../login.aspx?wh=1");
            }
            catch
            {
                Response.Redirect("../login.aspx?wh=1");
            }
            if (!IsPostBack)
            {
                string fid, flist_id;
                string db, tb;
                try
                {
                    fid = Request.QueryString["fid"].ToString();
                    flist_id = Request.QueryString["flist_id"].ToString();
                    //20131107 lxl 分库分表后增加两个参数
                    db = Request.QueryString["db"].ToString();
                    tb = Request.QueryString["tb"].ToString();
                    // 访问这个页面的来源 可空
                    ViewState["from"] = Request.QueryString["from"] ?? "";
                    ViewState["fid"] = fid;
                    ViewState["flist_id"] = flist_id;
                    ViewState["db"] = db;
                    ViewState["tb"] = tb;
                }
                catch
                {
                    WebUtils.ShowMessage(this.Page, "参数有误！");
                    return;
                }

                try
                {
                    if (fid != null && fid != "")
                    {
                        if (db == null || db == "" || tb == null || tb == "")
                            BindInfoFID(int.Parse(fid));
                        else//三种类型的分库表
                            BindInfoFIDDBTB(fid, db, tb);
                    }
                    else
                    {
                        WebUtils.ShowMessage(this.Page, "参数有误！");
                        return;
                    }
                }
                catch (LogicException err)
                {
                    WebUtils.ShowMessage(this.Page, err.Message);
                }
                catch (SoapException eSoap) //捕获soap类异常
                {
                    string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                    WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr + ", stacktrace" + eSoap.StackTrace);
                }
                catch (Exception eSys)
                {
                    WebUtils.ShowMessage(this.Page, "读取数据失败！" + PublicRes.GetErrorMsg(eSys.Message.ToString()) + ", stacktrace" + eSys.StackTrace);
                }
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

        private void BindInfoFID(int fid)
        {
            lblfid.Text = fid.ToString();

            Query_Service.Query_Service qs = new Query_Service.Query_Service();
            qs.Finance_HeaderValue = classLibrary.setConfig.setFH(this);
            DataSet ds = qs.GetCFTUserAppealDetail(fid);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                if (dr["FState"].ToString() == "0" || dr["FState"].ToString() == "3"
                    || dr["FState"].ToString() == "4" || dr["FState"].ToString() == "5"
                    || dr["FState"].ToString() == "6" || dr["FState"].ToString() == "8")
                {
                    btOK.Visible = true;
                    btSetRealName.Visible = true;
                    btCancel.Visible = true;
                    btnDel.Visible = true;

                    btOK.Attributes["onClick"] = "return confirm('确定要执行“通过申诉”操作吗？');";
                    btSetRealName.Attributes["onClick"] = "return confirm('确定通过申诉并置实名吗？')";
                    btCancel.Attributes["onClick"] = "return confirm('确定要执行“拒绝申诉”操作吗？');";
                    btnDel.Attributes["onClick"] = "return confirm('确定要执行“删除申诉”操作吗？');";
                }
                else if (dr["FState"].ToString() == "2" || dr["FState"].ToString() == "9")
                {
                    btOK.Visible = true;
                    btSetRealName.Visible = true;
                    if (dr["FState"].ToString() == "9")
                    {
                        this.btCancel.Visible = false;
                        this.btnDel.Visible = false;
                    }

                    btOK.Attributes["onClick"] = "return confirm('确定要执行“二次审核通过”操作吗？');";
                    btSetRealName.Attributes["onClick"] = "return confirm('确定要执行“二次审核通过置实名”操作吗？');";
                }

                //读取内容，读出QQ号后，就要读取后台信息。
                //				labFuin.Text = dr["Fuin"].ToString();

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
                tbFCheckInfo.Text = PublicRes.GetString(dr["FCheckInfo"]);

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
                //     lbauthenState.Text = dr["authenState"].ToString();//增加实名认证字段

                lblivrresult.Text = dr["FIVRResult"].ToString();
                try
                {
                    DataSet dsuser = new UserAppealService().GetAppealUserInfo(dr["Fuin"].ToString());
                    if (dsuser == null || dsuser.Tables.Count == 0 || dsuser.Tables[0].Rows.Count != 1)
                    {
                        labFQQid.Text = "读取数据有误";
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
                        var creid = PublicRes.GetString(druser["Fcreid"]);
                        labFcreid.Text = ViewState["from"].ToString() == "UserClassQuery" ? setConfig.ConvertID(creid, creid.Length - 6, 3) : creid;
                        labFEmail.Text = PublicRes.GetString(druser["FEmail"]);
                        labFtruename.Text = PublicRes.GetString(druser["Ftruename"]);

                        labFBankAcc.Text = PublicRes.GetString(druser["Fbankid"]);

                        if (dr["FType"].ToString() == "3")
                        {
                            labFtruename.Text = PublicRes.GetString(druser["Fcompany_name"]);
                        }
                    }
                }
                catch (Exception err)
                {
                    labFQQid.Text = "读取数据有误：" + PublicRes.GetErrorMsg(err.Message) + ", stacktrace" + err.StackTrace;
                    btOK.Visible = false;
                    btSetRealName.Visible = false;
                }


                //2.查询实名认证
                bool stateMsg = false;
                DataSet authenState = new UserAppealService().GetUserAuthenState(dr["Fuin"].ToString(), "", 0, out stateMsg);
                if (stateMsg)
                {
                    lbauthenState.Text = "是";
                }
                else
                {
                    lbauthenState.Text = "否";
                }

            }
            else
            {
                throw new Exception("没有找到记录！");
            }
        }

        private void BindInfoFIDDBTB(string fid, string db, string tb)
        {
            lblfid.Text = fid.ToString();

            Query_Service.Query_Service qs = new Query_Service.Query_Service();
            qs.Finance_HeaderValue = classLibrary.setConfig.setFH(this);
            DataSet ds = qs.GetCFTUserAppealDetailByDBTB(fid, db, tb);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                if (dr["FState"].ToString() == "0" || dr["FState"].ToString() == "3"
                    || dr["FState"].ToString() == "4" || dr["FState"].ToString() == "5"
                    || dr["FState"].ToString() == "6" || dr["FState"].ToString() == "8")
                {
                    btOK.Visible = true;
                    btSetRealName.Visible = true;
                    btCancel.Visible = true;
                    btnDel.Visible = true;

                    btOK.Attributes["onClick"] = "return confirm('确定要执行“通过申诉”操作吗？');";
                    btSetRealName.Attributes["onClick"] = "return confirm('确定通过申诉并置实名吗？')";
                    btCancel.Attributes["onClick"] = "return confirm('确定要执行“拒绝申诉”操作吗？');";
                    btnDel.Attributes["onClick"] = "return confirm('确定要执行“删除申诉”操作吗？');";
                }
                else if (dr["FState"].ToString() == "2" || dr["FState"].ToString() == "9")
                {
                    btOK.Visible = true;
                    btSetRealName.Visible = true;
                    if (dr["FState"].ToString() == "9")
                    {
                        this.btCancel.Visible = false;
                        this.btnDel.Visible = false;
                    }

                    btOK.Attributes["onClick"] = "return confirm('确定要执行“二次审核通过”操作吗？');";
                    btSetRealName.Attributes["onClick"] = "return confirm('确定要执行“二次审核通过置实名”操作吗？');";
                }

                //读取内容，读出QQ号后，就要读取后台信息。
                //				labFuin.Text = dr["Fuin"].ToString();

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
                tbFCheckInfo.Text = PublicRes.GetString(dr["FCheckInfo"]);

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
                //     lbauthenState.Text = dr["authenState"].ToString();//增加实名认证字段

                lblivrresult.Text = dr["FIVRResult"].ToString();

                try
                {
                    DataSet dsuser = new UserAppealService().GetAppealUserInfo(dr["Fuin"].ToString());
                    if (dsuser == null || dsuser.Tables.Count == 0 || dsuser.Tables[0].Rows.Count != 1)
                    {
                        labFQQid.Text = "读取数据有误";
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

                        var creid = PublicRes.GetString(druser["Fcreid"]);
                        labFcreid.Text = ViewState["from"].ToString() == "UserClassQuery" ? setConfig.ConvertID(creid, creid.Length - 6, 3) : creid;
                        labFEmail.Text = PublicRes.GetString(druser["FEmail"]);
                        labFtruename.Text = PublicRes.GetString(druser["Ftruename"]);

                        labFBankAcc.Text = PublicRes.GetString(druser["Fbankid"]);

                        if (dr["FType"].ToString() == "3")
                        {
                            labFtruename.Text = PublicRes.GetString(druser["Fcompany_name"]);
                        }
                    }
                }
                catch (Exception err)
                {
                    LogError("BaseAccount.CFTUserCheckPwd", "private void BindInfoFIDDBTB(string fid,string db,string tb)，读取数据有误：", err);
                    labFQQid.Text = "读取数据有误：" + PublicRes.GetErrorMsg(err.Message) + ", stacktrace" + err.StackTrace;
                    btOK.Visible = false;
                    btSetRealName.Visible = false;
                }

                qs.Finance_HeaderValue = classLibrary.setConfig.setFH(this);


                //2.查询实名认证
                bool stateMsg = false;
                DataSet authenState = new UserAppealService().GetUserAuthenState(dr["Fuin"].ToString(), "", 0, out stateMsg);
                if (stateMsg)
                {
                    lbauthenState.Text = "是";
                }
                else
                {
                    lbauthenState.Text = "否";
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
            DataSet ds = qs.GetUserClassDetail(flist_id);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                if (dr["Fpickstate"].ToString() == "0" || dr["Fpickstate"].ToString() == "1")
                {
                    btOK.Visible = true;
                    btSetRealName.Visible = true;
                    btCancel.Visible = true;
                    btnDel.Visible = false;

                    btOK.Attributes["onClick"] = "return confirm('确定要执行“通过申诉”操作吗？');";
                    btSetRealName.Attributes["onClick"] = "return confirm('确定通过申诉并置实名吗？');";
                    btCancel.Attributes["onClick"] = "return confirm('确定要执行“拒绝申诉”操作吗？');";
                }
                // 2012/4/17 添加允许2次审核实名认证的申诉
                else if (dr["Fpickstate"].ToString() == "3")
                {
                    btOK.Visible = true;
                    btSetRealName.Visible = true;
                    btCancel.Visible = false;
                    btnDel.Visible = false;

                    btOK.Attributes["onClick"] = "return confirm('确定要二次执行“通过申诉”操作吗？');";
                    btSetRealName.Attributes["onClick"] = "return confirm('确定要执行“二次审核通过置实名”操作吗？');";
                }

                //读取内容，读出QQ号后，就要读取后台信息。
                //				labFuin.Text = dr["Fqqid"].ToString();

                labFTypeName.Text = "实名认证";
                tbComment.Visible = false;
                tbFCheckInfo.Text = PublicRes.GetString(dr["Fmemo"]);
                string Msg = "";

                try
                {
                    DataSet dsuser = qs.GetUserClassInfoFlag(dr["Fqqid"].ToString(), out Msg);
                    if (dsuser == null || dsuser.Tables.Count == 0 || dsuser.Tables[0].Rows.Count != 1)
                    {
                        labFQQid.Text = "读取数据有误,ICE接口错误!" + Msg;
                        btOK.Visible = false;
                        btSetRealName.Visible = false;
                    }
                    else
                    {
                        DataRow druser = dsuser.Tables[0].Rows[0];

                        labFQQid.Text = druser["Fqqid"].ToString();
                        labFcre_type.Text = GetCreType(druser["Fcre_type"].ToString());

                        var creid = PublicRes.GetString(druser["Fcreid"]);
                        labFcreid.Text = ViewState["from"].ToString() == "UserClassQuery" ? setConfig.ConvertID(creid, creid.Length - 6, 3) : creid;
                        labFtruename.Text = PublicRes.GetString(druser["Ftruename"]);
                    }
                }
                catch (Exception err)
                {
                    LogError("BaseAccount.CFTUserCheckPwd", "private void BindInfoFListID(int flist_id)，读取数据有误：", err);
                    labFQQid.Text = "读取数据有误：" + PublicRes.GetErrorMsg(err.Message) + Msg + ", stacktrace" + err.StackTrace;
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
                qs.Finance_HeaderValue = fh;
                string UserIP = Request.UserHostAddress;
                string UserName = Session["uid"].ToString();

                if (fid != null && fid != "")
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
                else
                {
                    WebUtils.ShowMessage(this.Page, "参数有误！");
                    return;
                }

            }
            catch (Exception err)
            {
                LogError("BaseAccount.CFTUserCheckPwd", "protected void btOK_Click(object sender, System.EventArgs e)，通过失败：", err);
                WebUtils.ShowMessage(this.Page, "通过失败:" + PublicRes.GetErrorMsg(err.Message + msg) + ", stacktrace" + err.StackTrace);
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

                if (fid != null && fid != "")
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
                else
                {
                    WebUtils.ShowMessage(this.Page, "参数有误！");
                    return;
                }

            }
            catch (Exception err)
            {
                LogError("BaseAccount.CFTUserCheckPwd", "protected void btSetRealName_Click(object sender, System.EventArgs e)，通过失败：", err);
                WebUtils.ShowMessage(this.Page, "通过失败:" + PublicRes.GetErrorMsg(err.Message + msg) + ", stacktrace" + err.StackTrace);
            }
        }

        protected void btCancel_Click(object sender, System.EventArgs e)
        {
            //拒绝请求.
            string msg = "";
            try
            {
                string reason = "", OtherReason = "";           

                OtherReason = classLibrary.setConfig.replaceMStr(tbFCheckInfo.Text.Trim());               

                if (classLibrary.getData.IsTestMode)//测试加了！
                {
                    this.ShowMsg(reason + OtherReason);

                    return;
                }

                if (reason.Trim() == "" && OtherReason.Trim() == "")
                    throw new Exception("请填写拒绝原因！");

                string fid = ViewState["fid"].ToString();
                string flist_id = ViewState["flist_id"].ToString();
                string db = ViewState["db"].ToString();
                string tb = ViewState["tb"].ToString();

                Query_Service.Query_Service qs = new Query_Service.Query_Service();

                Finance_Header fh = setConfig.setFH(this);
                qs.Finance_HeaderValue = fh;
                string UserIP = Request.UserHostAddress;
                string UserName = Session["uid"].ToString();

                if (fid != null && fid != "")
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
                else
                {
                    WebUtils.ShowMessage(this.Page, "参数有误！");
                    return;
                }
            }
            catch (Exception err)
            {
                LogError("BaseAccount.CFTUserCheckPwd", "protected void btCancel_Click(object sender, System.EventArgs e)，通过失败：", err);
                WebUtils.ShowMessage(this.Page, "拒绝失败:" + PublicRes.GetErrorMsg(err.Message + msg) + ", stacktrace" + err.StackTrace);
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
                qs.Finance_HeaderValue = fh;
                string UserIP = Request.UserHostAddress;
                string UserName = Session["uid"].ToString();

                if (fid != null && fid != "")
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
                        if (qs.CFTDelAppealDBTB(fid, db, tb, tbComment.Text.Trim(), UserName, UserIP, out msg))
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
                else
                {
                    WebUtils.ShowMessage(this.Page, "参数有误！");
                    return;
                }

            }
            catch (Exception err)
            {
                LogError("BaseAccount.CFTUserCheckPwd", "protected void btnDel_Click(object sender, System.EventArgs e)，拒绝失败：", err);
                WebUtils.ShowMessage(this.Page, "拒绝失败:" + PublicRes.GetErrorMsg(err.Message + msg) + ", stacktrace" + err.StackTrace);
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