using CFT.CSOMS.DAL.CRTModule;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CFT.CSOMS.BLL.CRTModule
{
    public class CRTService
    {
        /// <summary>
        /// 查询个人证书信息列表
        /// </summary>
        /// <param name="qqid"></param>
        /// <returns></returns>
        public DataSet GetUserCrtList(string qqid)
        {
            DataSet ds = new CRTData().GetUserCrtList(qqid);

            if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count <= 0)
            {
                return null;
            }

            ds.Tables[0].Columns.Add("FstateName", typeof(string));
            ds.Tables[0].Columns.Add("FlstateName", typeof(string));
            ds.Tables[0].Columns.Add("Fmodify_time2", typeof(string));

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int Fstate = Int32.Parse(dr["Fstate"].ToString());
                int Flstate = Int32.Parse(dr["Flstate"].ToString());

                if (Fstate == 1)
                {
                    dr["FstateName"] = "初始状态";
                }
                else if (Fstate == 2)
                {
                    dr["FstateName"] = "有效(已签署)";
                }
                else if (Fstate == 3)
                {
                    dr["FstateName"] = "挂起(暂时冻结)";
                }
                else if (Fstate == 4)
                {
                    dr["FstateName"] = "注销(不再可用)";
                }
                else
                {
                    dr["FstateName"] = "未定义" + Fstate;
                }

                if (Flstate == 1)
                {
                    dr["FlstateName"] = "正常";
                }
                else if (Flstate == 2)
                {
                    dr["FlstateName"] = "冻结";
                }
                else
                {
                    dr["FlstateName"] = "未定义" + Flstate;
                }

                if (dr["Fstate"].ToString() == "4")
                {
                    dr["Fmodify_time2"] = dr["Fmodify_time"].ToString();
                }
            }

            return ds;
        }

        /// <summary>
        /// 查询关闭证书服务信息
        /// </summary>
        /// <param name="qqid"></param>
        /// <returns></returns>
        public DataSet GetDeleteQueryInfo(string qqid)
        {
            return new CRTData().GetDeleteQueryInfo(qqid);
        }

        /// <summary>
        /// 删除个人证书
        /// </summary>
        /// <param name="qqid"></param>
        public void DeleteUserCrt(string qqid)
        {
            new CRTData().DeleteUserCrt(qqid);
        }

        /// <summary>
        /// 关闭证书服务
        /// </summary>
        /// <param name="qqid"></param>
        /// <param name="ip"></param>
        public void DeleteCrtService(string qqid, string ip)
        {
            new CRTData().DeleteCrtService(qqid, ip);
        }
    }
}
