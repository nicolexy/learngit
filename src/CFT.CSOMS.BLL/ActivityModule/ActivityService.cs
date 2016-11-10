using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using CFT.CSOMS.DAL.ActivityModule;
using CFT.CSOMS.DAL.FundModule;
using System.Collections;

namespace CFT.CSOMS.BLL.ActivityModule
{
    public class ActivityService
    {
       
       

        /// <summary>
        /// 心意卡发送详情查询。yinhuang 20140616
        /// </summary>
        /// <param name="sendId">发送单号</param>
        public DataSet QueryXYKSendDetail(string sendId)
        {
            if (string.IsNullOrEmpty(sendId))
            {
                throw new ArgumentNullException("sendId");
            }

            DataSet ds = new ActivityData().QueryXYKSendDetail(sendId);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ds.Tables[0].Columns.Add("statusStr", typeof(String));//使用状态

                Hashtable ht1 = new Hashtable();
                ht1.Add("1", "初始化");
                ht1.Add("2", "领取中");
                ht1.Add("3", "领取成功");
                ht1.Add("4", "转赠成功");
                ht1.Add("5", "已使用");

                COMMLIB.CommUtil.DbtypeToPageContent(ds.Tables[0], "status", "statusStr", ht1);
            }
            return ds;
        }

        /// <summary>
        /// 添加新活动
        /// </summary>
        /// <param name="actNo"></param>
        /// <param name="actName"></param>
        /// <param name="opeUin"></param>
        public void AddLctActivity(string actNo, string actName, string opeUin)
        {
            if (string.IsNullOrEmpty(actNo))
            {
                throw new ArgumentNullException("actNo");
            }
            new ActivityData().AddLctActivity(actNo, actName, opeUin);
        }

        public DataTable QueryActivityList(string actNo, string actName, int offset, int limit)
        {
            return new ActivityData().QueryActivityList(actNo, actName, offset, limit);
        }

        public void DelLctActivity(int fid)
        {
            new ActivityData().DelLctActivity(fid);
        }

        //通过FUin找到用户参加活动的渠道号
        public string GetChannelIDByFUid(string FUid)
        {
            return new ActivityData().GetChannelIDByFUId(FUid);
        }

        public DataSet QueryHandQActivity(string strListID, string strBegingTime, string strEndTime, int nStart, int nCount)
        {
            return new ActivityData().QueryHandQActivity(strListID, strBegingTime, strEndTime, nStart,nCount);
        }

        /// <summary>
        /// 获取活动号对应的活动名称
        /// 主要用于理财通活动查询功能使用
        /// </summary>
        /// <param name="act_no"></param>
        /// <returns></returns>
        public string GetActNameByActNo(string act_no)
        {
            DataTable dt = new ActivityService().QueryActivityList(act_no, "", 0, 1);
            if (dt != null && dt.Rows.Count > 0)
            {
                return dt.Rows[0]["Fact_name"].ToString().Trim();
            }
            else
                return "未知" + act_no;
        }
    }
}
