using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace commLib
{
    /// <summary>
    /// 通用方法
    /// </summary>
    public static class CommonMethod
    {
        #region 获取连续10年的List集合
        /// <summary>
        /// 获取连续10年的List集合
        /// </summary>
        /// <returns></returns>
        public static IList<int> GetYearList()
        {
            int currentYear = DateTime.Now.Year;
            IList<int> yearList = new List<int>();
            for (int i = currentYear + 1; i > currentYear - 10; i--)
            {
                yearList.Add(i);
            }
            return yearList;
        }
        ///// <summary>
        ///// 获取分页数据
        ///// </summary>
        ///// <param name="ddl">DropDownList</param>
        //public static void GetPageList(DropDownList ddl)
        //{
        //    IList<int> pageList = new List<int>();
        //    for (int i = 5; i <= 100; i += 5)
        //    {
        //        pageList.Add(i);
        //    }
        //    ListItem list = new ListItem("--请选择--", "-1");
        //    ddl.Items.Add(list);
        //    for (int i = 0; i < pageList.Count; i++)
        //    {
        //        if (!string.IsNullOrEmpty(pageList[i].ToString()))
        //        {
        //            ListItem items = new ListItem(pageList[i].ToString(), pageList[i].ToString());
        //            ddl.Items.Add(items);
        //        }
        //    }
        //}
        #endregion

        #region  将DataTable转成Json格式的数据
        /// <summary>
        /// 将DataTable转成Json格式的数据
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DataTableToJson(DataTable dt)
        {
            if (dt.Rows.Count == 0)
            {
                return "";
            }

            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("{");
            jsonBuilder.Append("\"");
            jsonBuilder.Append("TotalRecords");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(":");

            jsonBuilder.Append(dt.Rows.Count);
            jsonBuilder.Append(",");
            jsonBuilder.Append("\"");
            jsonBuilder.Append("rows");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(":");
            jsonBuilder.Append("[");//转换成多个model的形式
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                jsonBuilder.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    jsonBuilder.Append("\"");
                    jsonBuilder.Append(dt.Columns[j].ColumnName);
                    jsonBuilder.Append("\":\"");
                    jsonBuilder.Append(dt.Rows[i][j].ToString());
                    jsonBuilder.Append("\",");
                }
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                jsonBuilder.Append("},");
            }
            jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            jsonBuilder.Append("]");
            jsonBuilder.Append("}");
            return jsonBuilder.ToString();
        }

        public static string DataTableToJson(DataTable dt, int total)
        {
            if (dt.Rows.Count == 0)
            {
                return "";
            }

            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("{");
            jsonBuilder.Append("\"");
            jsonBuilder.Append("TotalRecords");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(":");

            jsonBuilder.Append(total);
            jsonBuilder.Append(",");
            jsonBuilder.Append("\"");
            jsonBuilder.Append("rows");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(":");
            jsonBuilder.Append("[");//转换成多个model的形式
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                jsonBuilder.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    jsonBuilder.Append("\"");
                    jsonBuilder.Append(dt.Columns[j].ColumnName);
                    jsonBuilder.Append("\":\"");
                    jsonBuilder.Append(dt.Rows[i][j].ToString());
                    jsonBuilder.Append("\",");
                }
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                jsonBuilder.Append("},");
            }
            jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            jsonBuilder.Append("]");
            jsonBuilder.Append("}");
            return jsonBuilder.ToString();
        }

        public static string DataTableToJson2(string jsonName, DataTable dt)
        {
            StringBuilder Json = new StringBuilder();
            Json.Append("{\"" + jsonName + "\":[");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Json.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        Json.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":\"" + dt.Rows[i][j].ToString() + "\"");
                        if (j < dt.Columns.Count - 1)
                        {
                            Json.Append(",");
                        }
                    }
                    Json.Append("}");
                    if (i < dt.Rows.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
            }
            Json.Append("]}");
            return Json.ToString();
        }

        #endregion

        #region GridView的分页控件绑定数据
        ///// <summary>
        /////GridView的分页控件(AspNetPager)绑定数据
        ///// </summary>
        ///// <param name="pagerName">分页控件名称</param>
        ///// <param name="dt">数据集合</param>
        //public static PagedDataSource BindPagingControls(AspNetPager pagerName, DataTable dt)
        //{
        //    pagerName.RecordCount = dt.Rows.Count;
        //    PagedDataSource pds = new PagedDataSource();
        //    pds.DataSource = dt.DefaultView;
        //    pds.AllowPaging = true;
        //    pds.PageSize = pagerName.PageSize;
        //    pds.CurrentPageIndex = pagerName.CurrentPageIndex - 1;
        //    return pds;
        //}

        //public static PagedDataSource BindPagingControls(AspNetPager pagerName, List<object> list)
        //{
        //    pagerName.RecordCount = list.Count;
        //    PagedDataSource pds = new PagedDataSource();
        //    pds.DataSource = list;
        //    pds.AllowPaging = true;
        //    pds.PageSize = pagerName.PageSize;
        //    pds.CurrentPageIndex = pagerName.CurrentPageIndex - 1;
        //    return pds;
        //}
        ///// <summary>
        ///// GridView的分页控件(AspNetPager)绑定数据
        ///// </summary>
        ///// <param name="pagerName">分页控件名称</param>
        ///// <param name="dt">数据集合</param>
        ///// <param name="gv">绑定的GridView</param>
        //public static void BindPagingControls(AspNetPager pagerName, DataTable dt, GridView gv)
        //{
        //    pagerName.RecordCount = dt.Rows.Count;
        //    PagedDataSource pds = new PagedDataSource();
        //    pds.DataSource = dt.DefaultView;
        //    pds.AllowPaging = true;
        //    pds.PageSize = pagerName.PageSize;
        //    pds.CurrentPageIndex = pagerName.CurrentPageIndex - 1;
        //    gv.DataSource = pds;
        //    gv.DataBind();
        //}

        #endregion

        #region 生成动态列的GridView
        ///// <summary>
        ///// 绑定生成GridView
        ///// </summary>
        ///// <param name="gdv">要绑定的GridView</param>
        ///// <param name="dtblDataSource">GridView的数据源</param>
        ///// <param name="strDataKey">GridView的DataKeyNames</param>
        //public static void GridViewBind(GridView gdv, DataTable dtblDataSource, string strDataKey)
        //{
        //    gdv.Columns.Clear();

        //    gdv.AutoGenerateColumns = false;
        //    gdv.DataSource = dtblDataSource;
        //    //gdv.DataKeyNames = new string[] { strDataKey };

        //    for (int i = 0; i < dtblDataSource.Columns.Count; i++)   //绑定普通数据列
        //    {
        //        BoundField bfColumn = new BoundField();
        //        bfColumn.DataField = dtblDataSource.Columns[i].ColumnName;
        //        bfColumn.HeaderText = dtblDataSource.Columns[i].Caption;
        //        gdv.Columns.Add(bfColumn);
        //    }

        //    //gdv.Columns[1].Visible = false;

        //    //CommandField cfModify = new CommandField();  //绑定命令列
        //    //cfModify.ButtonType = ButtonType.Button;
        //    //cfModify.SelectText = "修改";
        //    //cfModify.ShowSelectButton = true;
        //    //gdv.Columns.Add(cfModify);

        //    gdv.DataBind();
        //}
        #endregion

        #region 将GridView中的数据导出到Excel中
        ///// <summary>
        ///// 将GrivdView数据导出到Excel中
        ///// </summary>
        ///// <param name="gv">要导出的GridView名称</param>
        ///// <param name="title">Excel名称</param>
        //public static void GridViewToExcel(GridView gv, string title)
        //{
        //    try
        //    {
        //        StringBuilder sb = new StringBuilder();
        //        StringWriter sw = new StringWriter(sb);
        //        HtmlTextWriter htw = new HtmlTextWriter(sw);
        //        Page page = new Page();
        //        HtmlForm form = new HtmlForm();
        //        gv.EnableViewState = false;
        //        gv.AllowPaging = false;
        //        page.EnableEventValidation = false;
        //        page.DesignerInitialize();
        //        page.Controls.Add(form);
        //        form.Controls.Add(gv);
        //        page.RenderControl(htw);
        //        HttpContext.Current.Response.Clear();
        //        HttpContext.Current.Response.Buffer = true;
        //        HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
        //        HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(title, System.Text.Encoding.UTF8) + ".xls");
        //        HttpContext.Current.Response.Charset = "UTF-8";
        //        HttpContext.Current.Response.ContentEncoding = Encoding.Default;
        //        HttpContext.Current.Response.Output.Write(sb.ToString());
        //        HttpContext.Current.Response.Flush();
        //        HttpContext.Current.Response.End();
        //        HttpContext.Current.ApplicationInstance.CompleteRequest();
        //    }
        //    catch (Exception e)
        //    {
        //        throw new Exception("Error错误!!! Source:" + e.Source.ToString() + " Message:" + e.Message.ToString());
        //    }

        //}
        #endregion

        #region 将数据绑定到DropDownList
        ///// <summary>
        ///// 将连续10年绑定到DropDownList
        ///// </summary>
        ///// <param name="ddl">要绑定的控件名称</param>
        //public static void BindlevelYear(DropDownList ddl)
        //{
        //    IList<int> years = CommonMethod.GetYearList();
        //    ListItem list = new ListItem("--请选择--", "-1");
        //    ddl.Items.Add(list);
        //    for (int i = 0; i < years.Count; i++)
        //    {
        //        if (!string.IsNullOrEmpty(years[i].ToString()))
        //        {
        //            ListItem items = new ListItem(years[i].ToString(), years[i].ToString());
        //            ddl.Items.Add(items);
        //        }
        //    }

        //}

        ///// <summary>
        ///// 将部门名称绑定到DropDownList
        ///// </summary>
        ///// <param name="ddl">要绑定的控件名称</param>
        //public static void BindDepartMent(DropDownList ddl)
        //{
        //    MODEL.OAEntities OAModel = new MODEL.OAEntities();
        //    List<MODEL.T_Hr_Dept> departmentList = OAModel.T_Hr_Dept.Where(d => d.state == 0).ToList<MODEL.T_Hr_Dept>();
        //    ListItem list = new ListItem("--请选择--", "-1");
        //    ddl.Items.Add(list);
        //    foreach (MODEL.T_Hr_Dept department in departmentList)
        //    {
        //        if (department != null)
        //        {
        //            ListItem items = new ListItem(department.name, department.id.ToString());
        //            ddl.Items.Add(items);
        //        }
        //    }
        //}
        #endregion

        #region 获取当前日期
        public static string GetCurrentDate()
        {
            string dt = DateTime.Now.ToString("yyyy-MM-dd");
            return dt;
        }
        public static string GetCurrentMonthsFirstDay()
        {
            return new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyy-MM-dd");

        }
        public static string GetCurrentMonthsLastDay()
        {
            return new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
        }
        #endregion

        #region
        /// <summary>
        /// 自定义字符窜截取
        /// </summary>
        /// <param name="sString"></param>
        /// <param name="nLeng"></param>
        /// <returns></returns>
        public static string SubStr(string sString, int nLeng)
        {
            if (sString.Length <= nLeng)
            {
                return sString;
            }
            string sNewStr = sString.Substring(0, nLeng);
            sNewStr = sNewStr + "...";
            return sNewStr;
        }
        #endregion

        #region 空表格
        public static DataTable GetEmptyDataTable(string columns)
        {
            DataTable dt = new DataTable();
            string[] columnList = columns.TrimEnd(',').Split(',');
            foreach (var column in columnList)
            {
                dt.Columns.Add(column);
            }
            DataRow dr = dt.NewRow();
            dt.Rows.Add(dr);
            return dt;
        }

        #endregion

        #region 正则表达式
        /// <summary>
        /// 判断输入的值是否符合正则表达式
        /// </summary>
        /// <param name="inputValue">输入值</param>
        /// <param name="selectedVlaue">选择的正则表达式。1：非负数判断；2：日期格式判断</param>
        /// <param name="messages">输出参数</param>
        /// <returns></returns>
        public static bool IsConformRegularExpressions(object inputValue, int selectedVlaue, out string messages)
        {
            bool result = false;
            messages = string.Empty;
            Regex re;
            Match mt;
            switch (selectedVlaue)
            {
                case 1:
                    re = new Regex("^\\d+(\\.\\d+)?$");             //非负浮点数
                    mt = re.Match(inputValue.ToString());
                    if (mt.Success)
                    {
                        result = true;
                    }
                    else
                    {
                        messages = "应为非负的数字！";
                    }
                    break;
                case 2:
                    re = new Regex(@"^(?:(?!0000)[0-9]{4}-(?:(?:0[1-9]|1[0-2])-(?:0[1-9]|1[0-9]|2[0-8])|(?:0[13-9]|1[0-2])-(?:29|30)|(?:0[13578]|1[02])-31)|(?:[0-9]{2}(?:0[48]|[2468][048]|[13579][26])|(?:0[48]|[2468][048]|[13579][26])00)-02-29)$");
                    mt = re.Match(inputValue.ToString());
                    if (mt.Success)
                    {
                        result = true;
                    }
                    else
                    {
                        messages = "请输入正确的日期格式！";
                    }
                    break;
            }
            return result;
        }
        #endregion

        #region

        ///// <summary>
        ///// 打开一个.net窗口口，并且这个.net窗口位于最前面
        ///// </summary>
        ///// <param name="page">提用的页面</param>
        ///// <param name="URL">要打开的URL</param>
        ///// <param name="PageName">要打开页面的名称</param>
        ///// <param name="Win_Width">窗口宽度</param>
        ///// <param name="Win_Hight">窗口高度</param>
        ///// <param name="Left">窗口左侧位置</param>
        ///// <param name="Top">窗口右侧位置</param>
        ///// <param name="CenterFlag">是否右中 yes/no</param>
        ///// <param name="Status">是否显示状态栏 yes/no</param>
        ///// <param name="ParentFlag">true:不关闭弹出窗口，将不能操作父窗口 false 可以操作父窗口</param>
        //public static void OpenNewWinodw(Page page, string URL, string PageName, string Win_Width, string Win_Hight, string Left, string Top, string CenterFlag, string Status, bool ParentFlag)
        //{

        //    string scriptstr = "";
        //    if (ParentFlag)
        //    {
        //        scriptstr = "<script language=javascript>showModalDialog('" + URL + "','" + PageName + "','dialogWidth:" + Win_Width + "px;"
        //            + "dialogHeight:" + Win_Hight + "px;dialogLeft:" + Left + "px;dialogTop:" + Top + "px;center:" + CenterFlag.ToString() + ";help:no;resizeable:yes;status:" + Status + "')</script>";
        //    }
        //    else
        //    {
        //        scriptstr = "<script language=javascript>showModelessDialog('" + URL + "','" + PageName + "','dialogWidth:" + Win_Width + "px;"
        //            + "dialogHeight:" + Win_Hight + "px;dialogLeft:" + Left + "px;dialogTop:" + Top + "px;center:" + CenterFlag.ToString() + ";help:no;resizeable:yes;status:" + Status + "')</script>";
        //    }
        //    page.Response.Write(scriptstr);
        //}
        ///// <summary>
        ///// 打开一个窗口，并且这个窗口位于最前面，不关闭，将不能操作父窗口
        ///// </summary>
        ///// <param name="page">提用的页面</param>
        ///// <param name="URL">要打开的URL</param>
        ///// <param name="PageName">要打开页面的名称</param>
        ///// <param name="Win_Width">窗口宽度</param>
        ///// <param name="Win_Hight">窗口高度</param>
        ///// <param name="Left">窗口左侧位置</param>
        ///// <param name="Top">窗口右侧位置</param>
        ///// <param name="CenterFlag">是否右中 yes/no</param>
        ///// <param name="ParentFlag">true:不关闭弹出窗口，将不能操作父窗口 false 可以操作父窗口</param>
        //public static void OpenNewWinodw(Page page, string URL, string PageName, string Win_Width, string Win_Hight, string Left, string Top, string CenterFlag, bool ParentFlag)
        //{
        //    string scriptstr = "";
        //    if (ParentFlag)
        //    {
        //        scriptstr = "<script language=javascript>showModalDialog('" + URL + "','" + PageName + "','dialogWidth:" + Win_Width + "px;"
        //            + "dialogHeight:" + Win_Hight + "px;dialogLeft:" + Left + "px;dialogTop:" + Top + "px;center:" + CenterFlag + ";help:no;resizeable:yes;status:no')</script>";
        //    }
        //    else
        //    {
        //        scriptstr = "<script language=javascript>showModelessDialog('" + URL + "','" + PageName + "','dialogWidth:" + Win_Width + "px;"
        //            + "dialogHeight:" + Win_Hight + "px;dialogLeft:" + Left + "px;dialogTop:" + Top + "px;center:" + CenterFlag + ";help:no;resizeable:yes;status:no')</script>";
        //    }
        //    page.Response.Write(scriptstr);
        //}

        ///// <summary>
        ///// 打开一个窗口，并且这个窗口位于最前面，不关闭，将不能操作父窗口
        ///// </summary>
        ///// <param name="page">提用的页面</param>
        ///// <param name="URL">要打开的URL</param>
        ///// <param name="PageName">要打开页面的名称</param>
        ///// <param name="Win_Width">窗口宽度</param>
        ///// <param name="Win_Hight">窗口高度</param>
        ///// <param name="Left">窗口左侧位置</param>
        ///// <param name="Top">窗口右侧位置</param>
        ///// <param name="ParentFlag">true:不关闭弹出窗口，将不能操作父窗口 false 可以操作父窗口</param>
        //public static void OpenNewWinodw(Page page, string URL, string PageName, string Win_Width, string Win_Hight, string Left, string Top, bool ParentFlag)
        //{
        //    string scriptstr = "";
        //    if (ParentFlag)
        //    {
        //        scriptstr = "<script language=javascript>showModalDialog('" + URL + "','" + PageName + "','dialogWidth:" + Win_Width + "px;"
        //            + "dialogHeight:" + Win_Hight + "px;dialogLeft:" + Left + "px;dialogTop:" + Top + "px;center:no;help:no;resizeable:yes;status:no')</script>";
        //    }
        //    else
        //    {
        //        scriptstr = "<script language=javascript>showModelessDialog('" + URL + "','" + PageName + "','dialogWidth:" + Win_Width + "px;"
        //            + "dialogHeight:" + Win_Hight + "px;dialogLeft:" + Left + "px;dialogTop:" + Top + "px;center:no;help:no;resizeable:yes;status:no')</script>";

        //    }
        //    page.Response.Write(scriptstr);
        //}

        ///// <summary>
        ///// 打开一个窗口，并且这个窗口位于最前面，不关闭，将不能操作父窗口
        ///// </summary>
        ///// <param name="page">提用的页面</param>
        ///// <param name="URL">要打开的URL</param>
        ///// <param name="PageName">要打开页面的名称</param>
        ///// <param name="Win_Width">窗口宽度</param>
        ///// <param name="Win_Hight">窗口高度</param>
        ///// <param name="ParentFlag">true:不关闭弹出窗口，将不能操作父窗口 false 可以操作父窗口</param>
        //public static void OpenNewWinodw(Page page, string URL, string PageName, string Win_Width, string Win_Hight, bool ParentFlag)
        //{
        //    string scriptstr = "";
        //    if (ParentFlag)
        //    {
        //        scriptstr = "<script language=javascript>showModalDialog('" + URL + "','" + PageName + "','dialogWidth:" + Win_Width + "px;"
        //            + "dialogHeight:" + Win_Hight + "px;dialogLeft:0px;dialogTop:0px;center:no;help:no;resizeable:yes;status:no')</script>";
        //    }
        //    else
        //    {
        //        scriptstr = "<script language=javascript>showModelessDialog('" + URL + "','" + PageName + "','dialogWidth:" + Win_Width + "px;"
        //            + "dialogHeight:" + Win_Hight + "px;dialogLeft:0px;dialogTop:0px;center:no;help:no;resizeable:yes;status:no')</script>";
        //    }
        //    page.Response.Write(scriptstr);
        //    // page.Response.Redirect(scriptstr);
        //}
        ///// <summary>
        ///// 打开一个窗口，并且这个窗口位于最前面，不关闭，将不能操作父窗口
        ///// </summary>
        ///// <param name="page">提用的页面</param>
        ///// <param name="URL">要打开的URL</param>
        ///// <param name="Win_Width">窗口宽度</param>
        ///// <param name="Win_Hight">窗口高度</param>
        ///// <param name="ParentFlag">true:不关闭弹出窗口，将不能操作父窗口 false 可以操作父窗口</param>
        //public static void OpenNewWinodw(Page page, string URL, string Win_Width, string Win_Hight, bool ParentFlag)
        //{
        //    string scriptstr = "";
        //    if (ParentFlag)
        //    {
        //        scriptstr = "<script language=javascript>showModalDialog('" + URL + "','','dialogWidth:" + Win_Width + "px;"
        //            + "dialogHeight:" + Win_Hight + "px;dialogLeft:0px;dialogTop:0px;center:no;help:no;resizeable:yes;status:no')</script>";
        //    }
        //    else
        //    {
        //        scriptstr = "<script language=javascript>showModelessDialog('" + URL + "','','dialogWidth:" + Win_Width + "px;"
        //            + "dialogHeight:" + Win_Hight + "px;dialogLeft:0px;dialogTop:0px;center:no;help:no;resizeable:yes;status:no')</script>";
        //    }
        //    page.Response.Write(scriptstr);
        //}
        #endregion

        #region 表格排序
        /// <summary>
        /// DataTable排序
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="sortCode"></param>
        /// <returns></returns>
        public static DataTable SortForDataTabel(DataTable dt, string sortCode)
        {
            DataTable newTable = dt.Copy();
            DataView dv = dt.DefaultView;
            dv.Sort = sortCode;
            newTable = dv.ToTable();
            return newTable;
        }
        #endregion

        #region 去除DataTable中的空行数据
        /// <summary>
        /// 去除DataTable中的空行数据
        /// </summary>
        /// <param name="dt">需去除空行的DataTable</param>
        public static void RemoveEmpty(DataTable dt)
        {
            List<DataRow> removelist = new List<DataRow>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                bool rowdataisnull = true;
                for (int j = 0; j < dt.Columns.Count; j++)
                {

                    if (!string.IsNullOrEmpty(dt.Rows[i][j].ToString().Trim()))
                    {

                        rowdataisnull = false;
                    }

                }
                if (rowdataisnull)
                {
                    removelist.Add(dt.Rows[i]);
                }

            }
            for (int i = 0; i < removelist.Count; i++)
            {
                dt.Rows.Remove(removelist[i]);
            }
        }
        #endregion


        #region 字符串处理
        /// <summary>
        /// 规律字符串转为list格式
        /// </summary>
        /// <typeparam name="T">转换目标类型</typeparam>
        /// <param name="inputString">输入的字符串</param>
        /// <param name="splitWord">字符串分割符号</param>
        /// <returns></returns>
        public static List<T> StringToList<T>(string inputString, char splitWord)
        {
            List<T> list = new List<T>();
            try
            {
                if (!string.IsNullOrEmpty(inputString))
                {
                    string[] inputArray = inputString.Split(splitWord);
                    for (int i = 0; i < inputArray.Length; i++)
                    {
                        object obj = inputArray[i].ToString();
                        list.Add((T)obj);
                    }
                }
            }
            catch (Exception ex)
            {
                list = null;
            }
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <param name="split"></param>
        /// <param name="convertHandler"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this string str, char split, Converter<string, T> convertHandler)
        {
            if (string.IsNullOrEmpty(str))
            {
                return new List<T>();
            }
            else
            {
                string[] arr = str.Split(split);
                T[] Tarr = Array.ConvertAll(arr, convertHandler);
                return new List<T>(Tarr);
            }
        }

        /// <summary>
        /// list转换为string
        /// </summary>
        /// <typeparam name="T">list类型</typeparam>
        /// <param name="inputList">输入的list</param>
        /// <param name="beforeWord">字符串前面的符号</param>
        /// <param name="behindWord">字符串后面的符号</param>
        /// <param name="spilt">返回的字符串分割符号</param>
        /// <returns></returns>
        public static string stringToSqlSearchCondition(string inputString, char inputSplit, char beforeWord, char behindWord, char spilt)
        {
            string returnResult = string.Empty;
            StringBuilder sb = new StringBuilder();
            try
            {
                if (!string.IsNullOrEmpty(inputString) && inputString.Length > 0)
                {
                    string[] inputArray = inputString.Split(inputSplit);
                    for (int i = 0; i < inputArray.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(inputArray[i]) && inputArray[i].ToString().Length > 0)
                        {
                            sb.Append(beforeWord).Append(inputArray[i].ToString()).Append(behindWord).Append(spilt);
                        }

                    }
                    returnResult = sb.ToString().TrimEnd(spilt);
                }
            }
            catch (Exception ex)
            {
                returnResult = string.Empty;
            }
            return returnResult;
        }

        public static string stringToSqlSearchCondition2(string inputString, char inputSplit, string beforeWord, string behindWord, char spilt)
        {
            string returnResult = string.Empty;
            StringBuilder sb = new StringBuilder();
            try
            {
                if (!string.IsNullOrEmpty(inputString) && inputString.Length > 0)
                {
                    string[] inputArray = inputString.Split(inputSplit);
                    for (int i = 0; i < inputArray.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(inputArray[i]) && inputArray[i].ToString().Length > 0)
                        {
                            sb.Append(beforeWord).Append(inputArray[i].ToString()).Append(behindWord).Append(spilt);
                        }

                    }
                    returnResult = sb.ToString().TrimEnd(spilt);
                }
            }
            catch (Exception ex)
            {
                returnResult = string.Empty;
            }
            return returnResult;
        }
        #endregion

        #region list处理
        /// <summary>
        /// list转换为string 
        /// </summary>
        /// <typeparam name="T">list类型</typeparam>
        /// <param name="inputList">输入的list</param>
        /// <param name="spilt">返回的字符串分割符号</param>
        /// <returns></returns>
        public static string listToString<T>(List<T> inputList, char spilt)
        {
            string returnResult = string.Empty;
            StringBuilder sb = new StringBuilder();
            try
            {
                if (inputList != null)
                {

                    for (int i = 0; i < inputList.Count; i++)
                    {
                        sb.Append(inputList[i].ToString()).Append(spilt);
                    }
                    returnResult = sb.ToString().TrimEnd(spilt);
                }
            }
            catch (Exception ex)
            {
                returnResult = string.Empty;
            }
            return returnResult;
        }

        /// <summary>
        /// list转换为string
        /// </summary>
        /// <typeparam name="T">list类型</typeparam>
        /// <param name="inputList">输入的list</param>
        /// <param name="beforeWord">字符串前面的符号</param>
        /// <param name="behindWord">字符串后面的符号</param>
        /// <param name="spilt">返回的字符串分割符号</param>
        /// <returns></returns>
        public static string listToString<T>(List<T> inputList, char beforeWord, char behindWord, char spilt)
        {
            string returnResult = string.Empty;
            StringBuilder sb = new StringBuilder();
            try
            {
                if (inputList != null)
                {
                    for (int i = 0; i < inputList.Count; i++)
                    {
                        sb.Append(beforeWord).Append(inputList[i].ToString()).Append(behindWord).Append(spilt);
                    }
                    returnResult = sb.ToString().TrimEnd(spilt);
                }
            }
            catch (Exception ex)
            {
                returnResult = string.Empty;
            }
            return returnResult;
        }
        #endregion

    }
}
