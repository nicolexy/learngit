using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace commLib
{
    /// <summary>
    /// JSON帮助类
    /// </summary>
    public static class JsonHelper
    {
        /// <summary>
        /// 生成表单编辑赋值 JSON格式
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="displayCount"></param>
        /// <returns></returns>
        public static string CreateJsonOne(DataTable dt, bool displayCount)
        {
            StringBuilder JsonString = new StringBuilder();
            //Exception Handling        
            if (dt != null && dt.Rows.Count > 0)
            {

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    JsonString.Append("{ ");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (j < dt.Columns.Count - 1)
                        {
                            //JsonString.Append("ipt_" + dt.Columns[j].ColumnName.ToString()  + ":" + "\"" + dt.Rows[i][j].ToString() + "\",");
                            JsonString.Append(dt.Columns[j].ColumnName.ToString() + ":" + "\"" + dt.Rows[i][j].ToString() + "\",");
                        }
                        else if (j == dt.Columns.Count - 1)
                        {
                            //JsonString.Append("ipt_" + dt.Columns[j].ColumnName.ToString()  + ":" + "\"" + dt.Rows[i][j].ToString() + "\"");
                            JsonString.Append(dt.Columns[j].ColumnName.ToString() + ":" + "\"" + dt.Rows[i][j].ToString() + "\"");
                        }
                    }

                    if (i == dt.Rows.Count - 1)
                    {
                        JsonString.Append("} ");
                    }
                    else
                    {
                        JsonString.Append("}, ");
                    }
                }

                return JsonString.ToString();
            }
            else
            {
                return null;
            }

        }


        /// <summary>
        /// 将DataTable中的数据转换成JSON格式
        /// </summary>
        /// <param name="dt">数据源DataTable</param>
        /// <param name="displayCount">是否输出数据总条数</param>
        /// <returns></returns>
        public static string CreateJsonParameters(DataTable dt, bool displayCount)
        {
            return CreateJsonParameters(dt, displayCount, dt.Rows.Count);
        }
        /// <summary>
        /// 将DataTable中的数据转换成JSON格式
        /// </summary>
        /// <param name="dt">数据源DataTable</param>
        /// <returns></returns>
        public static string CreateJsonParameters(DataTable dt)
        {
            return CreateJsonParameters(dt, true);
        }
        /// <summary>
        /// 将DataTable中的数据转换成JSON格式
        /// </summary>
        /// <param name="dt">数据源DataTable</param>
        /// <param name="displayCount">是否输出数据总条数</param>
        /// <param name="totalcount">JSON中显示的数据总条数</param>
        /// <returns></returns>
        public static string CreateJsonParameters(DataTable dt, bool displayCount, int totalcount)
        {
            StringBuilder JsonString = new StringBuilder();
            //Exception Handling        

            if (dt != null)
            {
                JsonString.Append("{ ");
                JsonString.Append("\"rows\":[ ");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    JsonString.Append("{ ");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (j < dt.Columns.Count - 1)
                        {
                            //if (dt.Rows[i][j] == DBNull.Value) continue;
                            if (dt.Columns[j].DataType == typeof(bool))
                            {
                                JsonString.Append("\"JSON_" + dt.Columns[j].ColumnName + "\":" +
                                                  dt.Rows[i][j].ToString() + ",");
                            }
                            else if (dt.Columns[j].DataType == typeof(string))
                            {
                                JsonString.Append("\"JSON_" + dt.Columns[j].ColumnName + "\":" + "\"" +
                                                  dt.Rows[i][j].ToString().Replace("\"", "\\\"") + "\",");
                            }
                            else
                            {
                                JsonString.Append("\"JSON_" + dt.Columns[j].ColumnName + "\":" + "\"" + dt.Rows[i][j] + "\",");
                            }
                        }
                        else if (j == dt.Columns.Count - 1)
                        {
                            //if (dt.Rows[i][j] == DBNull.Value) continue;
                            if (dt.Columns[j].DataType == typeof(bool))
                            {
                                JsonString.Append("\"JSON_" + dt.Columns[j].ColumnName + "\":" +
                                                  dt.Rows[i][j].ToString());
                            }
                            else if (dt.Columns[j].DataType == typeof(string))
                            {
                                JsonString.Append("\"JSON_" + dt.Columns[j].ColumnName + "\":" + "\"" +
                                                  dt.Rows[i][j].ToString().Replace("\"", "\\\"") + "\"");
                            }
                            else
                            {
                                JsonString.Append("\"JSON_" + dt.Columns[j].ColumnName + "\":" + "\"" + dt.Rows[i][j] + "\"");
                            }
                        }
                    }
                    /*end Of String*/
                    if (i == dt.Rows.Count - 1)
                    {
                        JsonString.Append("} ");
                    }
                    else
                    {
                        JsonString.Append("}, ");
                    }
                }
                JsonString.Append("]");

                if (displayCount)
                {
                    JsonString.Append(",");

                    JsonString.Append("\"total\":");
                    JsonString.Append(totalcount);
                }

                JsonString.Append("}");
                return JsonString.ToString().Replace("\n", "");
            }
            else
            {
                return null;
            }
        }

        #region object 2 json

        private static void WriteDataRow(StringBuilder sb, DataRow row)
        {
            sb.Append("{");
            foreach (DataColumn column in row.Table.Columns)
            {
                sb.AppendFormat("\"{0}\":", column.ColumnName);
                WriteValue(sb, row[column]);
                sb.Append(",");
            }
            // Remove the trailing comma.
            if (row.Table.Columns.Count > 0)
            {
                --sb.Length;
            }
            sb.Append("}");
        }

        private static void WriteDataSet(StringBuilder sb, DataSet ds)
        {
            sb.Append("{\"Tables\":{");
            foreach (DataTable table in ds.Tables)
            {
                sb.AppendFormat("\"{0}\":", table.TableName);
                WriteDataTable(sb, table);
                sb.Append(",");
            }
            // Remove the trailing comma.
            if (ds.Tables.Count > 0)
            {
                --sb.Length;
            }
            sb.Append("}}");
        }

        private static void WriteDataTable(StringBuilder sb, DataTable table)
        {
            sb.Append("{\"Rows\":[");
            foreach (DataRow row in table.Rows)
            {
                WriteDataRow(sb, row);
                sb.Append(",");
            }
            // Remove the trailing comma.
            if (table.Rows.Count > 0)
            {
                --sb.Length;
            }
            sb.Append("]}");
        }

        private static void WriteEnumerable(StringBuilder sb, IEnumerable e)
        {
            bool hasItems = false;
            sb.Append("[");
            foreach (object val in e)
            {
                WriteValue(sb, val);
                sb.Append(",");
                hasItems = true;
            }
            // Remove the trailing comma.
            if (hasItems)
            {
                --sb.Length;
            }
            sb.Append("]");
        }

        private static void WriteHashtable(StringBuilder sb, IDictionary e)
        {
            bool hasItems = false;
            sb.Append("{");
            foreach (string key in e.Keys)
            {
                sb.AppendFormat("\"{0}\":", key);
                WriteValue(sb, e[key]);
                sb.Append(",");
                hasItems = true;
            }
            // Remove the trailing comma.
            if (hasItems)
            {
                --sb.Length;
            }
            sb.Append("}");
        }

        private static void WriteObject(StringBuilder sb, object o)
        {
            MemberInfo[] members = o.GetType().GetMembers(BindingFlags.Instance | BindingFlags.Public);
            sb.Append("{");
            bool hasMembers = false;
            foreach (MemberInfo member in members)
            {
                bool hasValue = false;
                object val = null;
                if ((member.MemberType & MemberTypes.Field) == MemberTypes.Field)
                {
                    FieldInfo field = (FieldInfo)member;
                    val = field.GetValue(o);
                    hasValue = true;
                }
                else if ((member.MemberType & MemberTypes.Property) == MemberTypes.Property)
                {
                    PropertyInfo property = (PropertyInfo)member;
                    if (property.CanRead && property.GetIndexParameters().Length == 0)
                    {
                        val = property.GetValue(o, null);
                        hasValue = true;
                    }
                }
                if (hasValue)
                {
                    sb.Append("\"");
                    sb.Append(member.Name);
                    sb.Append("\":");
                    WriteValue(sb, val);
                    sb.Append(",");
                    hasMembers = true;
                }
            }
            if (hasMembers)
            {
                --sb.Length;
            }
            sb.Append("}");
        }

        private static void WriteString(StringBuilder sb, IEnumerable s)
        {
            sb.Append("\"");
            foreach (char c in s)
            {
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\"");
                        break;
                    case '\\':
                        sb.Append("\\\\");
                        break;
                    case '\b':
                        sb.Append("\\b");
                        break;
                    case '\f':
                        sb.Append("\\f");
                        break;
                    case '\n':
                        sb.Append("\\n");
                        break;
                    case '\r':
                        sb.Append("\\r");
                        break;
                    case '\t':
                        sb.Append("\\t");
                        break;
                    default:
                        int i = c;
                        if (i < 32 || i > 127)
                        {
                            sb.AppendFormat("\\u{0:X04}", i);
                        }
                        else
                        {
                            sb.Append(c);
                        }
                        break;
                }
            }
            sb.Append("\"");
        }

        public static void WriteValue(StringBuilder sb, object val)
        {
            if (val == null || val == DBNull.Value)
            {
                sb.Append("null");
            }
            else if (val is string || val is Guid)
            {
                WriteString(sb, val.ToString());
            }
            else if (val is bool)
            {
                sb.Append(val.ToString());
            }
            else if (val is double ||
                     val is float ||
                     val is long ||
                     val is int ||
                     val is short ||
                     val is byte ||
                     val is decimal)
            {
                sb.AppendFormat(CultureInfo.InvariantCulture.NumberFormat, "{0}", val);
            }
            else if (val.GetType().IsEnum)
            {
                sb.Append((int)val);
            }
            else if (val is DateTime)
            {
                sb.Append("new Date(\"");
                sb.Append(((DateTime)val).ToString("MMMM, d yyyy HH:mm:ss",
                                                    new CultureInfo("en-US", false).DateTimeFormat));
                sb.Append("\")");
            }
            else if (val is DataSet)
            {
                WriteDataSet(sb, val as DataSet);
            }
            else if (val is DataTable)
            {
                WriteDataTable(sb, val as DataTable);
            }
            else if (val is DataRow)
            {
                WriteDataRow(sb, val as DataRow);
            }
            else if (val is Hashtable)
            {
                WriteHashtable(sb, val as Hashtable);
            }
            else if (val is IEnumerable)
            {
                WriteEnumerable(sb, val as IEnumerable);
            }
            else
            {
                WriteObject(sb, val);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string Convert2Json(object o)
        {
            StringBuilder sb = new StringBuilder();
            WriteValue(sb, o);
            return sb.ToString();
        }

        #endregion

        public static string ReturnMenuJson(DataTable dt)
        {
            string menuJson = string.Empty;
            if (dt != null && dt.Rows.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("{menus:[");
                DataRow[] rows = dt.Select("ParentId=0");
                foreach (DataRow row in rows)
                {
                    int menuid = int.Parse(row["id"].ToString());
                    sb.Append("");
                }

                sb.Append("]}");
            }
            return menuJson;
        }
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
            jsonBuilder.Append("total");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(":");
            jsonBuilder.Append(dt.Rows.Count);
            jsonBuilder.Append(",");
            jsonBuilder.Append("\"");
            jsonBuilder.Append("rows");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(":");
            jsonBuilder.Append("[");//转换成多个model的形式

            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    jsonBuilder.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        jsonBuilder.Append("\"");
                        jsonBuilder.Append(dt.Columns[j].ColumnName.ToLower());
                        jsonBuilder.Append("\":\"");
                        jsonBuilder.Append(dt.Rows[i][j].ToString());
                        jsonBuilder.Append("\",");
                    }
                    jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                    jsonBuilder.Append("},");
                }
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            }
            jsonBuilder.Append("]");
            jsonBuilder.Append("}");
            return jsonBuilder.ToString();
        }

        public static string DataTableToJson(DataTable dt, int total)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("{");
            jsonBuilder.Append("\"");
            jsonBuilder.Append("total");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(":");
            if (dt == null || dt.Rows.Count < 1)
            {
                jsonBuilder.Append(0);
            }
            else
            {
                jsonBuilder.Append(total);
            }
            jsonBuilder.Append(",");
            jsonBuilder.Append("\"");
            jsonBuilder.Append("rows");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(":");
            jsonBuilder.Append("[");//转换成多个model的形式
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    jsonBuilder.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        jsonBuilder.Append("\"");
                        jsonBuilder.Append(dt.Columns[j].ColumnName.ToLower());
                        jsonBuilder.Append("\":\"");
                        jsonBuilder.Append(dt.Rows[i][j].ToString());
                        jsonBuilder.Append("\",");
                    }
                    jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                    jsonBuilder.Append("},");
                }
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            }
            else
            {
                jsonBuilder.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    jsonBuilder.Append("\"");
                    jsonBuilder.Append(dt.Columns[j].ColumnName.ToLower());
                    jsonBuilder.Append("\":\"");
                    jsonBuilder.Append("");
                    jsonBuilder.Append("\",");
                }
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                jsonBuilder.Append("}");
            }

            jsonBuilder.Append("]");
            jsonBuilder.Append("}");
            return jsonBuilder.ToString();
        }

        public static string DataTableToJson(DataTable dt, int total, bool isShowTotal = true)
        {
            StringBuilder builder = new StringBuilder();
            if (dt == null || dt.Rows.Count == 0)
            {
                builder.Append("{ ");
                builder.Append("\"rows\":[ ");
                builder.Append("]");
                if (isShowTotal)
                {
                    builder.Append(",");
                    builder.Append("\"total\":");
                    builder.Append(total);
                }
                builder.Append("}");
                return builder.ToString();
            }
            builder.Append("{ ");
            builder.Append("\"rows\":[ ");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                builder.Append("{ ");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (j < (dt.Columns.Count - 1))
                    {
                        builder.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":\"" + JsonCharFilter(dt.Rows[i][j].ToString()) + "\",");
                    }
                    else if (j == (dt.Columns.Count - 1))
                    {
                        builder.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":\"" + JsonCharFilter(dt.Rows[i][j].ToString()) + "\"");
                    }
                }
                if (i == (dt.Rows.Count - 1))
                {
                    builder.Append("} ");
                }
                else
                {
                    builder.Append("}, ");
                }
            }
            builder.Append("]");
            if (isShowTotal)
            {
                builder.Append(",");
                builder.Append("\"total\":");
                builder.Append(total);
            }
            builder.Append("}");
            return builder.ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="father"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public static string DataTableToJson2(DataTable dt, string jsonName, int fatherid, StringBuilder jsonBuilder)
        {
            if (dt == null || dt.Rows.Count < 1)
            {
                StringBuilder result = new StringBuilder();
                result.Append("{");
                result.Append("\"");
                result.Append("" + jsonName + "");
                result.Append("\"");
                result.Append(":");
                result.Append("[]");
                result.Append("}");
                jsonBuilder = result;
            }
            else
            {
                //StringBuilder jsonBuilder = new StringBuilder();
                if (jsonBuilder.ToString().Length < 1 || jsonBuilder == null)
                {
                    jsonBuilder.Append("{");
                }
                //else
                //{
                //    jsonBuilder.Append(",");
                //}
                jsonBuilder.Append("\"");
                jsonBuilder.Append("" + jsonName + "");
                jsonBuilder.Append("\"");
                jsonBuilder.Append(":");
                jsonBuilder.Append("[");
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow[] rows = dt.Select("FatherId=" + fatherid + "");
                    if (rows != null && rows.Length > 0)
                    {
                        int rowIndex = 0;
                        foreach (DataRow row in rows)
                        {
                            rowIndex++;
                            jsonBuilder.Append("{");
                            for (int i = 0; i < dt.Columns.Count; i++)
                            {
                                jsonBuilder.Append("\"").Append("" + dt.Columns[i].ColumnName.ToLower() + "").Append("\"").Append(":");
                                jsonBuilder.Append("\"").Append("" + row[i].ToString() + "").Append("\"").Append(",");
                            }
                            int id = int.Parse(row["id"].ToString());
                            DataRow[] childrendRow = dt.Select("FatherId=" + id + "");
                            if (childrendRow != null && childrendRow.Length > 0)
                            {
                                //jsonBuilder.Append(",");
                                DataTableToJson2(dt, jsonName, id, jsonBuilder);
                            }
                            else
                            {
                                jsonBuilder.Remove(jsonBuilder.ToString().Length - 1, 1);
                                jsonBuilder.Append("},");
                                if (rowIndex == rows.Length)
                                {
                                    jsonBuilder.Remove(jsonBuilder.ToString().Length - 1, 1);
                                    //jsonBuilder.Append("},");
                                }

                            }
                        }
                        jsonBuilder.Append("]},");
                        //if (rowIndex == dt.Rows.Count)
                        //{
                        //    jsonBuilder.Remove(jsonBuilder.ToString().Length - 1, 1);
                        //    //jsonBuilder.Append("},");
                        //}
                    }
                    else
                    {
                        jsonBuilder.Append("]}");
                    }
                }
                else
                {
                    jsonBuilder.Append("]}");
                }
            }
            //jsonBuilder.Append("]");
            //jsonBuilder.Append("}");
            //jsonBuilder.Remove(jsonBuilder.ToString().Length - 1, 1);
            return jsonBuilder.ToString();
        }





        /// <summary>
        /// 将DataTable转成Json格式的数据
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DataTableToEasyUIComboboxJson(DataTable dt)
        {
            if (dt.Rows.Count == 0)
            {
                return "";
            }

            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("[");//转换成多个model的形式
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    jsonBuilder.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        jsonBuilder.Append("\"");
                        jsonBuilder.Append(dt.Columns[j].ColumnName.ToLower());
                        jsonBuilder.Append("\":\"");
                        jsonBuilder.Append(dt.Rows[i][j].ToString());
                        jsonBuilder.Append("\",");
                    }
                    jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                    jsonBuilder.Append("},");
                }
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            }
            jsonBuilder.Append("]");
            return jsonBuilder.ToString();
        }



        public static string JsonCharFilter(string sourceStr)
        {
            sourceStr = sourceStr.Replace(@"\", @"\\");
            sourceStr = sourceStr.Replace("\"", "\\\"");
            sourceStr = sourceStr.Replace("\b", @"\b");
            sourceStr = sourceStr.Replace("\t", @"\t");
            sourceStr = sourceStr.Replace("\n", @"\n");
            sourceStr = sourceStr.Replace("\f", @"\f");
            sourceStr = sourceStr.Replace("\r", @"\r");
            sourceStr = sourceStr.Replace("\r\n", "");
            sourceStr = sourceStr.Replace("（", "(");
            sourceStr = sourceStr.Replace("）", ")");
            return sourceStr;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="father"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        //public static string DataTableToJsonTree(DataTable dt ,string filterWord,int fatherid, StringBuilder jsonBuilder)
        //{
        //    //if (dt == null || dt.Rows.Count < 1)
        //    //{
        //    //    StringBuilder result = new StringBuilder();
        //    //    result.Append("{");
        //    //    result.Append("\"");
        //    //    result.Append("" + jsonName + "");
        //    //    result.Append("\"");
        //    //    result.Append(":");
        //    //    result.Append("[]");
        //    //    result.Append("}");
        //    //    sb = result;
        //    //}
        //    //else
        //    //{
        //    //    if (sb.ToString().Length < 1 || sb == null)
        //    //    {
        //    //        sb.Append("{");
        //    //    }
        //    //    sb.Append("\"");
        //    //    sb.Append("" + jsonName + "");
        //    //    sb.Append("\"");
        //    //    sb.Append(":");
        //    //    sb.Append("[");
        //    //    DataRow[] rows = dt.Select("FatherId=" + fatherid + "");
        //    //    if (rows != null && rows.Length > 0)
        //    //    {
        //    //        int rowIndex = 0;
        //    //        for (int i = 0; i < rows.Length; i++)
        //    //        {
        //    //            rowIndex++;
        //    //            index++;
        //    //            int id = int.Parse(rows[i]["id"].ToString());
        //    //            string name = rows[i]["Name"].ToString();

        //    //            int fatherId = int.Parse(rows[i]["FatherId"].ToString());
        //    //            string icon = rows[i]["Icon"].ToString();
        //    //            string url = rows[i]["Url"].ToString();
        //    //            string remark = rows[i]["id"].ToString();
        //    //            string state = "closed";
        //    //            sb.Append("{");
        //    //            sb.Append("\"").Append("id").Append("\"").Append(":").Append("\"").Append("" + id + "").Append("\"").Append(",");
        //    //            sb.Append("\"").Append("name").Append("\"").Append(":").Append("\"").Append("" + name + "").Append("\"").Append(",");
        //    //            sb.Append("\"").Append("icon").Append("\"").Append(":").Append("\"").Append("" + icon + "").Append("\"").Append(",");
        //    //            sb.Append("\"").Append("url").Append("\"").Append(":").Append("\"").Append("" + url + "").Append("\"").Append(",");
        //    //            sb.Append("\"").Append("remark").Append("\"").Append(":").Append("\"").Append("" + remark + "").Append("\"").Append(",");
        //    //            sb.Append("\"").Append("fatherId").Append("\"").Append(":").Append("\"").Append("" + fatherId + "").Append("\"").Append(",");
        //    //            sb.Append("\"").Append("state").Append("\"").Append(":").Append("\"").Append("" + state + "").Append("\"").Append(",");
        //    //            DataRow[] childrenRows = dt.Select("FatherId=" + id + "");
        //    //            if (childrenRows != null && childrenRows.Length > 0)
        //    //            {
        //    //                DataTableToJsonTree(dt, jsonName, id, sb);
        //    //            }
        //    //            else
        //    //            {

        //    //                sb.Remove(sb.ToString().Length - 1, 1);
        //    //                sb.Append("},");
        //    //                if (rowIndex == rows.Length)
        //    //                {
        //    //                    sb.Remove(sb.ToString().Length - 1, 1);
        //    //                }
        //    //            }
        //    //        }
        //    //    }
        //    //    sb.Append("]},");
        //    //    if (index == dt.Rows.Count)
        //    //    {
        //    //        sb.Remove(sb.ToString().Length - 1, 1);
        //    //    }
        //    //}
        //    return "";// sb.ToString();
        //}

    }
}
