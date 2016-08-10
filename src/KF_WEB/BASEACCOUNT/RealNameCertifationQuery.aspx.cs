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
using System.Web.Services;
using System.Web.Services.Protocols;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;
using System.Linq;
using System.Configuration;
using CFT.CSOMS.BLL.PublicService;
using CFT.CSOMS.BLL.RealNameModule;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
    public partial class RealNameCertifationQuery : TENCENT.OSS.CFT.KF.KF_Web.PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Label_uid.Text = Session["uid"].ToString();
          
            if (!IsPostBack)
            {
                if (Request.Params["action"] != null && Request.Params["action"].ToString() != "")
                {
                    string action = Request.Params["action"].ToString();
                    switch (action)
                    {
                        case "Query":
                            {
                                if (Request.Params["method"] != null && Request.Params["method"].ToString() != "")
                                {
                                    string method = Request.Params["method"].ToString();
                                    int pageIndex = string.IsNullOrEmpty(Request.Params["page"]) ? 1 : int.Parse(Request.Params["page"]);
                                    int pageSize = string.IsNullOrEmpty(Request.Params["pagesize"]) ? 0 : int.Parse(Request.Params["pagesize"]);
                                    switch (method)
                                    {
                                        case "GetInfoByIdentityCard":
                                            {
                                                Response.ContentType = "application/json";
                                                string identityId = Request.Params["cre_id"].ToString();
                                                string ret = GetInfoByIdentityCard(identityId, pageIndex, pageSize);
                                                Response.Write(ret);
                                                Response.End();
                                            }
                                            break;
                                        case "GetInfoByUserType":
                                            {
                                                Response.ContentType = "application/json";
                                                string user = Request.Params["user"];
                                                string usertype = Request.Params["usertype"];
                                                string ret = GetInfoByUserType(user, usertype, pageIndex, pageSize);
                                                Response.Write(ret);
                                                Response.End();
                                            }
                                            break;
                                    }
                                }
                            }
                            break;
                        case "Edit":
                            {
                                if (Request.Params["method"] != null && Request.Params["method"].ToString() != "")
                                {
                                    string method = Request.Params["method"].ToString();
                                    switch (method)
                                    {
                                        case "GetQuotaDetail":
                                            {
                                                Response.ContentType = "text/html";
                                                int uid_type = string.IsNullOrEmpty(Request.Params["uid_type"]) ? 0 : int.Parse(Request.Params["uid_type"]);
                                                Int64 uid = string.IsNullOrEmpty(Request.Params["uid"]) ? 0 : Int64.Parse(Request.Params["uid"]);
                                                int cre_type = string.IsNullOrEmpty(Request.Params["cre_type"]) ? 1 : int.Parse(Request.Params["cre_type"]);
                                                string cre_id = string.IsNullOrEmpty(Request.Params["cre_id"]) ? "" : Request.Params["cre_id"];
                                                Int64 authen_channel_state = string.IsNullOrEmpty(Request.Params["channel_state"]) ? 0 : Int64.Parse(Request.Params["channel_state"]);
                                                int have_cre_photocopy = GetHasCrePhotocopyByState(authen_channel_state);
                                                string ret = GetQuotaDetail(uid_type, uid, cre_type, cre_id, have_cre_photocopy);
                                                Response.Write(ret);
                                                Response.End();
                                            }
                                            break;
                                        case "SettingAuMaintainWhite":
                                            {
                                                Response.ContentType = "application/json";
                                                Int64 uid = string.IsNullOrEmpty(Request.Params["uid"]) ? 0 : Int64.Parse(Request.Params["uid"]);
                                                string uin = string.IsNullOrEmpty(Request.Params["uin"]) ? "" : Request.Params["uin"];
                                                int op_type = int.Parse(Request.Params["op_type"]);
                                                int valid_days = string.IsNullOrEmpty(Request.Params["valid_days"]) ? 0 : int.Parse(Request.Params["valid_days"]);
                                                string ret = SettingAuMaintainWhite(uid, uin, op_type, valid_days);
                                                Response.Write(ret);
                                                Response.End();
                                            }
                                            break;
                                    }
                                }
                            }
                            break;
                    }

                }
            }

        }



        //通过身份证查找信息
        public string GetInfoByIdentityCard(string identityId, int pageIndex, int pageSize)
        {
            string ret = string.Empty;
            if (!string.IsNullOrEmpty(identityId))
            {
                DataTable dt = new RealNameCertificateService().GetInfoByIdentityCard(identityId, Session["uid"].ToString());
                DataTable final = new RealNameCertificateService().GetPagedTable(dt, pageIndex, pageSize);
                int count = (dt != null && dt.Rows.Count > 0) ? ((dt.Rows.Count % pageSize == 0) ? dt.Rows.Count / pageSize : dt.Rows.Count / pageSize + 1) : 0;
                ret = GetResultJsonStr(count, final, pageIndex);
            }
            return ret;
        }
        //通过账户查找信息
        public string GetInfoByUserType(string user, string usertype, int pageIndex, int pageSize)
        {
            string ret = string.Empty;
            DataTable dt = new RealNameCertificateService().GetInfoByUid(user, usertype, Session["uid"].ToString());
            DataTable final = new RealNameCertificateService().GetPagedTable(dt, pageIndex, pageSize);
            int count = (dt != null && dt.Rows.Count > 0) ? ((dt.Rows.Count % pageSize == 0) ? dt.Rows.Count / pageSize : dt.Rows.Count / pageSize + 1) : 0;
            ret = GetResultJsonStr(count, final, pageIndex);
            return ret;
        }
        public string GetResultJsonStr(int rowcount, DataTable dt, int pageIndex)
        {           
            if (rowcount == 0 || dt == null)
            {
                return "{\"pages\":0,\"content\":\"<tr><td colspan='16'>没有符合条件的数据!</td></tr>\"}";
            }
            bool isRight_SensitiveRole = TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("SensitiveRole", this);
            bool isRight = TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("RealNameCertification", this);
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.AppendFormat("\"pages\":{0},\"content\":\"", rowcount);
            int tempRow = 0;
            StringBuilder tempCreQuota = new StringBuilder();
            foreach (DataRow row in dt.Rows)
            {
                tempRow++;
                string uid_type = string.IsNullOrEmpty(row["uid_type"].ToString()) ? "" : row["uid_type"].ToString();
                string uid = string.IsNullOrEmpty(row["uid"].ToString()) ? "" : row["uid"].ToString();
                string uin = string.IsNullOrEmpty(row["uin"].ToString()) ? "" : row["uin"].ToString();
                string cre_type = string.IsNullOrEmpty(row["cre_type"].ToString()) ? "" : row["cre_type"].ToString();
                string cre_id = string.IsNullOrEmpty(row["cre_id"].ToString()) ? "" : row["cre_id"].ToString();
                string channelstate = string.IsNullOrEmpty(row["authen_channel_state"].ToString()) ? "" : row["authen_channel_state"].ToString();
                string btn = string.Format("<button onclick= 'quotaDetail(this,{0},{1},{2},&quot;{3}&quot;,{4});' oper_state='0' type='button'>明细</button>", uid_type, uid, cre_type, cre_id, channelstate);
                string setbtn = string.Format("<button onclick='settingWhite({0},&quot;{1}&quot;,{2});' type='button'>添加</button>", uid, uin, pageIndex);
                string cancelbtn = string.Format("<button onclick='cancelWhite({0},&quot;{1}&quot;,{2});' type='button'>取消</button>", uid, uin, pageIndex);
                string finalstr = isRight && uid_type != "0" ? ((uid_type == "99") ? cancelbtn : setbtn) : "";
                if (tempRow == 1)
                {
                    tempCreQuota.Append("<tr><td colspan='16' class='tbtr'>证件号对应的限额</td></tr>");
                    int uid_type1 = string.IsNullOrEmpty(uid_type) ? 0 : int.Parse(uid_type);
                    Int64 uid1 = string.IsNullOrEmpty(uid) ? 0 : Int64.Parse(uid);
                    int cre_type1 = string.IsNullOrEmpty(cre_type) ? 1 : int.Parse(cre_type);                   
                    Int64 authen_channel_state = string.IsNullOrEmpty(channelstate) ? 0 : Int64.Parse(channelstate);
                    int have_cre_photocopy = GetHasCrePhotocopyByState(authen_channel_state);
                    DataTable dt1 = new RealNameCertificateService().GetQuotaDetail(uid_type1, uid1, cre_type1, cre_id, have_cre_photocopy);
                    if (dt1 != null && dt1.Rows.Count==1)
                    {
                        tempCreQuota.AppendFormat("<tr><td>月进出金额</td><td>{0}</td><td>年支出金额</td><td>{1}</td><td>微信账户体系年支出金额</td><td>{2}</td><td>qq账户体系年支出金额</td><td>{3}</td><td>日支付金额</td><td>{4}</td><td>微信账户体系日支出金额</td><td>{5}</td><td>qq账户体系日支出金额</td><td colspan='3'>{6}</td></tr>", dt1.Rows[0]["cre_month_outin_amount"].ToString(), dt1.Rows[0]["cre_year_out_amount"].ToString(), dt1.Rows[0]["cre_wx_year_out_amount"].ToString(),dt1.Rows[0]["cre_sqpc_year_out_amount"].ToString(),dt1.Rows[0]["cre_day_out_amount"].ToString(),dt1.Rows[0]["cre_wx_day_out_amount"].ToString(),dt1.Rows[0]["cre_sqpc_day_out_amount"].ToString());
                    }
                }
                if (row["bind_bank_info"] != null && row["bind_bank_info"].ToString() != "")
                {
                    DataTable temp_dt = CommQuery.ParseRelayStringToDataTableNew("result=0&" + row["bind_bank_info"].ToString(), "bind_bank_num", "row");
                    int cols = (temp_dt != null && temp_dt.Rows.Count > 0) ? temp_dt.Rows.Count : 1;
                    sb.Append("<tr>");
                    sb.AppendFormat("<td rowspan='{0}'>{1}</td>", cols, string.IsNullOrEmpty(row["uin"].ToString()) ? "" : row["uin"].ToString());
                    sb.AppendFormat("<td rowspan='{0}'>{1}</td>", cols, string.IsNullOrEmpty(row["user_true_name"].ToString()) ? "" : classLibrary.setConfig.ConvertName(row["user_true_name"].ToString(), isRight_SensitiveRole));
                    sb.AppendFormat("<td rowspan='{0}'>{1}</td>", cols, string.IsNullOrEmpty(row["cre_type_txt"].ToString()) ? "" : row["cre_type_txt"].ToString());
                    if (!string.IsNullOrEmpty(row["cre_type_txt"].ToString()) && row["cre_type_txt"].ToString().Contains("身份证"))
                    {
                        sb.AppendFormat("<td rowspan='{0}'>{1}</td>", cols, string.IsNullOrEmpty(row["cre_id"].ToString()) ? "" : classLibrary.setConfig.IDCardNoSubstring(row["cre_id"].ToString(), isRight_SensitiveRole));
                    }
                    else
                    {
                        sb.AppendFormat("<td rowspan='{0}'>{1}</td>", cols, string.IsNullOrEmpty(row["cre_id"].ToString()) ? "" : row["cre_id"].ToString());
                    }
                    sb.AppendFormat("<td rowspan='{0}'>{1}</td>", cols, string.IsNullOrEmpty(row["gov_auth_info_dt"].ToString()) ? "" : row["gov_auth_info_dt"].ToString());
                    sb.AppendFormat("<td rowspan='{0}'>{1}</td>", cols, string.IsNullOrEmpty(row["ocr_authen_info_dt"].ToString()) ? "" : row["ocr_authen_info_dt"].ToString());
                    sb.AppendFormat("<td rowspan='{0}'>{1}</td>", cols, string.IsNullOrEmpty(row["mobile_authen_info_dt"].ToString()) ? "" : row["mobile_authen_info_dt"].ToString());
                    sb.AppendFormat("<td rowspan='{0}'>{1}</td>", cols, string.IsNullOrEmpty(row["edu_authen_info_dt"].ToString()) ? "" : row["edu_authen_info_dt"].ToString());
                    sb.AppendFormat("<td rowspan='{0}'>{1}</td>", cols, string.IsNullOrEmpty(row["authen_account_type"].ToString()) ? "" : row["authen_account_type"].ToString());
                    sb.AppendFormat("<td rowspan='{0}'>{1}</td>", cols, string.IsNullOrEmpty(row["authen_channel_info"].ToString()) ? "" : row["authen_channel_info"].ToString());
                    if (temp_dt != null && temp_dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < temp_dt.Rows.Count; i++)
                        {
                            if (i > 0) sb.Append("<tr>");
                            sb.AppendFormat("<td>{0}</td>", string.IsNullOrEmpty(temp_dt.Rows[i]["card_tail"].ToString()) ? "" : temp_dt.Rows[i]["card_tail"].ToString());
                            sb.AppendFormat("<td>{0}</td>", string.IsNullOrEmpty(temp_dt.Rows[i]["bank_name"].ToString()) ? "" : temp_dt.Rows[i]["bank_name"].ToString());
                            sb.AppendFormat("<td>{0}</td>", string.IsNullOrEmpty(temp_dt.Rows[i]["mobile"].ToString())?"": classLibrary.setConfig.ConvertTelephoneNumber(temp_dt.Rows[i]["mobile"].ToString(), isRight_SensitiveRole));
                            sb.AppendFormat("<td>{0}</td>", string.IsNullOrEmpty(temp_dt.Rows[i]["authen_time"].ToString()) ? "" : temp_dt.Rows[i]["authen_time"].ToString());
                            if (i > 0) sb.Append("</tr>");
                            if (i == 0)
                            {
                                sb.AppendFormat("<td rowspan='{0}'>{1}</td>", cols, btn);
                                sb.AppendFormat("<td rowspan='{0}'>{1}</td>", cols, finalstr);
                                sb.Append("</tr>");
                            }
                        }
                    }
                    else
                    {
                        sb.Append("<td></td>");
                        sb.Append("<td></td>");
                        sb.Append("<td></td>");
                        sb.Append("<td></td>");
                        sb.AppendFormat("<td rowspan='{0}'>{1}</td>", cols, btn);
                        sb.AppendFormat("<td rowspan='{0}'>{1}</td>", cols, finalstr);
                        sb.Append("</tr>");
                    }
                }
                else
                {
                    sb.Append("<tr>");
                    sb.AppendFormat("<td>{0}</td>", string.IsNullOrEmpty(row["uin"].ToString()) ? "" : row["uin"].ToString());
                    sb.AppendFormat("<td>{0}</td>", string.IsNullOrEmpty(row["user_true_name"].ToString()) ? "" : classLibrary.setConfig.ConvertName(row["user_true_name"].ToString(), isRight_SensitiveRole));
                    sb.AppendFormat("<td>{0}</td>", string.IsNullOrEmpty(row["cre_type_txt"].ToString()) ? "" : row["cre_type_txt"].ToString());
                    sb.AppendFormat("<td>{0}</td>", string.IsNullOrEmpty(row["cre_id"].ToString()) ? "" : classLibrary.setConfig.IDCardNoSubstring(row["cre_id"].ToString(), isRight_SensitiveRole));
                    sb.AppendFormat("<td>{0}</td>", string.IsNullOrEmpty(row["gov_auth_info_dt"].ToString()) ? "" : row["gov_auth_info_dt"].ToString());
                    sb.AppendFormat("<td>{0}</td>", string.IsNullOrEmpty(row["ocr_authen_info_dt"].ToString()) ? "" : row["ocr_authen_info_dt"].ToString());
                    sb.AppendFormat("<td>{0}</td>", string.IsNullOrEmpty(row["mobile_authen_info_dt"].ToString()) ? "" : row["mobile_authen_info_dt"].ToString());
                    sb.AppendFormat("<td>{0}</td>", string.IsNullOrEmpty(row["edu_authen_info_dt"].ToString()) ? "" : row["edu_authen_info_dt"].ToString());
                    sb.AppendFormat("<td>{0}</td>", string.IsNullOrEmpty(row["authen_account_type"].ToString()) ? "" : row["authen_account_type"].ToString());
                    sb.AppendFormat("<td>{0}</td>", string.IsNullOrEmpty(row["authen_channel_info"].ToString()) ? "" : row["authen_channel_info"].ToString());
                    sb.Append("<td></td>");
                    sb.Append("<td></td>");
                    sb.Append("<td></td>");
                    sb.Append("<td></td>");
                    sb.AppendFormat("<td>{0}</td>", btn);
                    sb.AppendFormat("<td>{0}</td>", finalstr);
                    sb.Append("</tr>");
                }
            }
            if (!string.IsNullOrEmpty(tempCreQuota.ToString()))
            {
                sb.Append(tempCreQuota.ToString());
            }
            sb.Append("\"}");
            return sb.ToString();
        }

        public string GetQuotaDetail(int uid_type, Int64 uid, int cre_type, string cre_id, int have_cre_photocopy)
        {
            DataTable dt = new RealNameCertificateService().GetQuotaDetail(uid_type, uid, cre_type, cre_id, have_cre_photocopy);
            StringBuilder sb = new StringBuilder();
            sb.Append("<table cellspacing='1' cellpadding='0' align='center' bgcolor='#666666' border='0' width='95%'>");
            sb.Append("<thead class='th'><tr><th>账户终身支出金额</th><th>账户月进出金额</th><th>账户年支出金额</th><th>账户终身剩余额度</th><th>账户剩余月进出额度</th><th>账户剩余年支付额度</th></tr></thead>");
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    sb.Append("<tr>");
                    sb.AppendFormat("<td>{0}</td>", (row["total_out_amount"] != null && row["total_out_amount"].ToString() != "") ? row["total_out_amount"].ToString() : "");
                    sb.AppendFormat("<td>{0}</td>", (row["month_outin_amount"] != null && row["month_outin_amount"].ToString() != "") ? row["month_outin_amount"].ToString() : "");
                    sb.AppendFormat("<td>{0}</td>", (row["year_out_amount"] != null && row["year_out_amount"].ToString() != "") ? row["year_out_amount"].ToString() : "");
                    sb.AppendFormat("<td>{0}</td>", (row["rest_total_out_amount"] != null && row["rest_total_out_amount"].ToString() != "") ? row["rest_total_out_amount"].ToString() : "");
                    sb.AppendFormat("<td>{0}</td>", (row["rest_month_outin_amount"] != null && row["rest_month_outin_amount"].ToString() != "") ? row["rest_month_outin_amount"].ToString() : "");
                    sb.AppendFormat("<td>{0}</td>", (row["rest_year_out_amount"] != null && row["rest_year_out_amount"].ToString() != "") ? row["rest_year_out_amount"].ToString() : "");
                    sb.Append("</tr>");
                }
            }
            else
            {
                sb.Append("<tr><td colspan='6'>没有符合条件的数据！</td></tr>");
            }
            sb.Append("</table>");
            return sb.ToString();
        }

        public string SettingAuMaintainWhite(Int64 uid, string uin, int op_type, int valid_days)
        {
            StringBuilder sb = new StringBuilder();
            string sign = "";
            string outMsg = string.Empty;
            if (op_type == 1)
            {
                if (valid_days != 0)
                {
                    string tmp = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|", uin, uid, Session["uid"].ToString(), "kf.cf.com", op_type, valid_days);
                    sign = new RealNameCertificateService().FormatStrEnscript(tmp, "AuMaintainWhiteListCKey");
                }
                else
                {
                    return "{\"ret\":\"白名单有效期至少为一天！\"}";
                }
            }
            else
            {
                string tmp = string.Format("{0}|{1}|{2}|{3}|{4}||", uin, uid, Session["uid"].ToString(), "kf.cf.com", op_type);
                sign = new RealNameCertificateService().FormatStrEnscript(tmp, "AuMaintainWhiteListCKey");
            }
            bool state = new RealNameCertificateService().AuMaintainWhiteListC("kf.cf.com", Session["uid"].ToString(), uin, uid, op_type, valid_days, sign, out outMsg);
            sb.Append("{\"ret\":\"" + (state ? "操作成功" : "操作失败") + "\"}");
            return sb.ToString();
        }

        public int GetHasCrePhotocopyByState(Int64 authen_channel_state)
        {
            return 0 == (authen_channel_state & (0x1 << 1)) ? 0 : 1;
        }

    }
}