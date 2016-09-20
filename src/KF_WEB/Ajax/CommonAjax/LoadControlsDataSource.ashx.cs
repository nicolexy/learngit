using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using commLib;

namespace TENCENT.OSS.CFT.KF.KF_Web.Ajax.CommonAjax
{
    /// <summary>
    /// Summary description for LoadControlsDataSource
    /// </summary>
    public class LoadControlsDataSource : IHttpHandler,IRequiresSessionState 
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            
            string getAction = context.Request.QueryString["getAction"] != null || string.IsNullOrEmpty(context.Request.QueryString["getAction"].ToString()) ? context.Request.QueryString["getAction"].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(getAction))
            {
                Action(getAction, context);

            }
        }

        public void Action(string actionName, HttpContext context)
        {
            if (actionName.Equals("LoadCommonCombobox"))
            {                
                LoadCommonCombobox(context);
            }
        
        }

        public void LoadCommonCombobox(HttpContext context)
        {
            bool isShowSelect = context.Request.QueryString["isShowSelect"] != null || string.IsNullOrEmpty(context.Request.QueryString["isShowSelect"].ToString()) ? bool.Parse(context.Request.QueryString["isShowSelect"].ToString()) : false;
            string showSelectText = context.Request.QueryString["showSelectText"] != null || string.IsNullOrEmpty(context.Request.QueryString["showSelectText"].ToString()) ? context.Request.QueryString["showSelectText"].ToString() : string.Empty;
            string type = context.Request.QueryString["type"] != null || string.IsNullOrEmpty(context.Request.QueryString["type"].ToString()) ? context.Request.QueryString["type"].ToString() : string.Empty;
            if (type.Equals("IDCardManualReview_LoadReveiwStatus"))
            {
                //身份证审核加载审核状态
                LoadIDCardManualReviewStatus(context, isShowSelect, showSelectText);
            }
            else if (type.Equals("IDCardManualReview_LoadReveiwResult"))
            {
                //身份证审核加载审核状态
                LoadIDCardManualReviewResult(context, isShowSelect, showSelectText);
            }
            else if (type.Equals("IDCardManualReview_LoadFmemo"))
            {
                //身份证审核失败原因
                LoadFmemo(context, isShowSelect, showSelectText);
            }
            
        }
        /// <summary>
        /// 身份证审核加载审核状态
        /// </summary>
        public void LoadIDCardManualReviewStatus(HttpContext context, bool isShowSelect,string showSelectText)
        {
            string result = string.Empty;
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("id");
                dt.Columns.Add("name");
                dt.Columns.Add("orderStr");
                DataRow dr1 = dt.NewRow();
                dr1["id"] = "1";
                dr1["name"] = "未领单";
                dr1["orderStr"] = "1";

                DataRow dr2 = dt.NewRow();
                dr2["id"] = "2";
                dr2["name"] = "已领单";
                dr2["orderStr"] = "2";

                DataRow dr3 = dt.NewRow();
                dr3["id"] = "3";
                dr3["name"] = "推送到实名系统失败";
                dr3["orderStr"] = "3";

                DataRow dr4 = dt.NewRow();
                dr4["id"] = "4";
                dr4["name"] = "推送成功";
                dr4["orderStr"] = "4";

                dt.Rows.Add(dr1);
                dt.Rows.Add(dr2);
                dt.Rows.Add(dr3);
                dt.Rows.Add(dr4);
                
                if (isShowSelect)
                {
                    DataRow row = dt.NewRow();
                    row["id"] = 0;
                    row["name"] = showSelectText;
                    row["orderStr"] = "0";
                    dt.Rows.Add(row);
                   
                }
                DataTable dt2 = dt.Copy();
                DataView defaultView = dt.DefaultView;
                defaultView.Sort = "orderStr";
                defaultView.ToTable().AcceptChanges();
                dt2 = defaultView.ToTable();
                result = CreateComboboxJson(dt2, "id", "name");
            }
            catch (Exception ex)
            {
                result = "[]";
            }
            
            context.Response.Write(result);
            context.Response.End();
        }

        /// <summary>
        /// 身份证审核加载审核状态
        /// </summary>
        public void LoadIDCardManualReviewResult(HttpContext context, bool isShowSelect, string showSelectText)
        {
            string result = string.Empty;
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("id");
                dt.Columns.Add("name");
                dt.Columns.Add("orderStr");

                DataRow dr1 = dt.NewRow();
                dr1["id"] = "0";
                dr1["name"] = "未处理";
                dr1["orderStr"] = "1";

                DataRow dr2 = dt.NewRow();
                dr2["id"] = "1";
                dr2["name"] = "通过";
                dr2["orderStr"] = "2";

                DataRow dr3 = dt.NewRow();
                dr3["id"] = "2";
                dr3["name"] = "驳回";
                dr3["orderStr"] = "3";

                dt.Rows.Add(dr1);
                dt.Rows.Add(dr2);
                dt.Rows.Add(dr3);

                if (isShowSelect)
                {
                    DataRow row = dt.NewRow();
                    row["id"] = -1;
                    row["name"] = showSelectText;
                    row["orderStr"] = 0;
                    dt.Rows.Add(row);
                    

                }
                DataTable dt2 = dt.Copy();
                DataView defaultView = dt.DefaultView;
                defaultView.Sort = "orderStr";
                defaultView.ToTable().AcceptChanges();
                dt2 = defaultView.ToTable();
                result = CreateComboboxJson(dt2, "id", "name");
            }
            catch (Exception ex)
            {
                result = "[]";
            }

            context.Response.Write(result);
            context.Response.End();
        }

        
        /// <summary>
        /// 身份证审核失败原因
        /// </summary>
        public void LoadFmemo(HttpContext context, bool isShowSelect, string showSelectText)
        {
            string result = string.Empty;
            try
            {


                DataTable dt = new DataTable();
                dt.Columns.Add("id");
                dt.Columns.Add("name");
                #region
                //1 = "未提供照片";
                //2 = "上传非身份证照片";
                //3 = "身份证件不清晰不完整";
                //4 = "身份证证件号不一致";
                //5 = "其他原因";
                //6 = "身份证姓名和提供姓名不符";
                //7 = "身份证签发机关和地址不一致";
                //8 = "两张均为正面或反面";
                //9 = "身份证证件虚假";
                //10 = "身份证已超过有效期";

                DataRow dr1 = dt.NewRow();
                dr1["id"] = "1";
                dr1["name"] = "未提供照片";//未显示图片
                dt.Rows.Add(dr1);

                DataRow dr2 = dt.NewRow();
                dr2["id"] = "2";
                dr2["name"] = "上传非身份证照片";//未提供身份证扫描件
                dt.Rows.Add(dr2);

                DataRow dr3 = dt.NewRow();
                dr3["id"] = "3";
                dr3["name"] = "身份证件不清晰不完整";//上传的扫描件不够完整、清晰、有效
                dt.Rows.Add(dr3);

                DataRow dr4 = dt.NewRow();
                dr4["id"] = "4";
                dr4["name"] = "身份证证件号不一致";//证件号码与原注册证件号码不符
                dt.Rows.Add(dr4);

                DataRow dr5 = dt.NewRow();
                dr5["id"] = "5";
                dr5["name"] = "其他原因"; 
                dt.Rows.Add(dr5);

                DataRow dr6 = dt.NewRow();
                dr6["id"] = "6";
                dr6["name"] = "身份证姓名和提供姓名不符";
                dt.Rows.Add(dr6);

                DataRow dr7 = dt.NewRow();
                dr7["id"] = "7";
                dr7["name"] = "身份证签发机关和地址不一致";
                dt.Rows.Add(dr7);

                DataRow dr8 = dt.NewRow();
                dr8["id"] = "8";
                dr8["name"] = "两张均为正面或反面";
                dt.Rows.Add(dr8);

                DataRow dr9 = dt.NewRow();
                dr9["id"] = "9";
                dr9["name"] = "身份证证件虚假";
                dt.Rows.Add(dr9);

                DataRow dr10 = dt.NewRow();
                dr10["id"] = "10";
                dr10["name"] = "身份证已超过有效期";
                dt.Rows.Add(dr10);
                #endregion

                if (isShowSelect)
                {
                    DataRow row = dt.NewRow();
                    row["id"] = 0;
                    row["name"] = showSelectText;
                    dt.Rows.Add(row);

                }
                DataTable dt2 = dt.Copy();
                DataView defaultView = dt.DefaultView;
                defaultView.Sort = "id";
                defaultView.ToTable().AcceptChanges();
                dt2 = defaultView.ToTable();
                result = CreateComboboxJson(dt2, "id", "name");
            }
            catch (Exception ex)
            {
                result = "[]";
            }

            context.Response.Write(result);
            context.Response.End();
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        public string CreateComboboxJson(DataTable dt,string keyName,string valueName)
        {
            StringBuilder builder = new StringBuilder();
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                builder.Append("[ ");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i < (dt.Rows.Count - 1))
                    {
                        builder.Append("{ ");
                        builder.Append("\"id\": ");
                        builder.Append(dt.Rows[i][keyName] + ",");
                        builder.Append("\"name\": ");
                        builder.Append("\"" + JsonHelper.JsonCharFilter(dt.Rows[i][valueName].ToString()) + "\"");
                        builder.Append("},");
                    }
                    if (i == (dt.Rows.Count - 1))
                    {
                        builder.Append("{ ");
                        builder.Append("\"id\": ");
                        builder.Append(dt.Rows[i][keyName] + ",");
                        builder.Append("\"name\": ");
                        builder.Append("\"" + JsonHelper.JsonCharFilter(dt.Rows[i][valueName].ToString()) + "\"");
                        builder.Append("}");
                    }
                }
                builder.Append("]");
                return builder.ToString();
            }
            builder.Append("[]");
            return builder.ToString();
        }
    }
}