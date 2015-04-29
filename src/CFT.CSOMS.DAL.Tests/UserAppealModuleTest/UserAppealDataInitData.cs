using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CFT.CSOMS.DAL.Infrastructure;
using System.Collections;
using System.Data;
using CFT.CSOMS.DAL.UserAppealModule;

namespace CFT.CSOMS.DAL.Tests
{
    public class UserAppealDataInitData
    {
         //测试之前初始化数据
         public void SetUp()
         {
             ArrayList sqlList = new ArrayList();
            //为防止初始化数据失败，先清除数据
             sqlList.Add("delete from db_appeal_2015.t_tenpay_appeal_trans_04 where Fid='20150417001';");
             sqlList.Add("delete from db_appeal_2015.t_tenpay_appeal_trans_04 where Fid='20150417002';");
             sqlList.Add("delete from db_appeal_2015.t_tenpay_appeal_trans_04 where Fid='20150417003';");
             sqlList.Add("delete from db_appeal_2015.t_tenpay_appeal_trans_04 where Fid='20150417004';");
             sqlList.Add("delete from db_appeal_2015.t_tenpay_appeal_trans_04 where FSubmitTime between '2015-04-15 00:00:00' and '2015-04-15 23:59:59';");
             //普通、微信解冻未处理状态批量结单查询数据
             sqlList.Add("INSERT INTO db_appeal_2015.t_tenpay_appeal_trans_04 (Fid,FType,Fuin,FState,FSubmitTime)VALUES ('20150417001', '8', '12345671', '0','2015-04-15 15:26:22');");
             sqlList.Add("INSERT INTO db_appeal_2015.t_tenpay_appeal_trans_04 (Fid,FType,Fuin,FState,FSubmitTime)VALUES ('20150417002', '8', '12345672', '0','2015-04-15 15:26:22');");
             sqlList.Add("INSERT INTO db_appeal_2015.t_tenpay_appeal_trans_04 (Fid,FType,Fuin,FState,FSubmitTime)VALUES ('20150417003', '19', '12345673', '0','2015-04-15 15:26:22');");
             sqlList.Add("INSERT INTO db_appeal_2015.t_tenpay_appeal_trans_04 (Fid,FType,Fuin,FState,FSubmitTime)VALUES ('20150417004', '19', '12345674', '0','2015-04-15 15:26:22');");
             //特殊找密待补充资料时间超过60天
             sqlList.Add("delete from db_appeal_2015.t_tenpay_appeal_trans_02;");
             sqlList.Add("INSERT INTO db_appeal_2015.t_tenpay_appeal_trans_02 (Fid,FType,Fuin,FState,FSubmitTime)VALUES ('201502170011', '11', '12345671', '11','2015-02-15 15:26:22');");
             sqlList.Add("INSERT INTO db_appeal_2015.t_tenpay_appeal_trans_02 (Fid,FType,Fuin,FState,FSubmitTime)VALUES ('201502170012', '11', '12345672', '11','2015-02-18 15:26:22');");

            new Tool().InsertORDeleteData(sqlList, "fkdj");

             //冻结日志初始化数据
             sqlList.Clear();
             sqlList.Add("delete from c2c_fmdb.t_freeze_list where Fid='100427';");
             sqlList.Add("delete from c2c_fmdb.t_freeze_list where Fid='100428';");
             sqlList.Add("INSERT INTO c2c_fmdb.t_freeze_list(Fid,FFreezeID,FHandleFinish)VALUES ('100427', '12345672', '1');");
             sqlList.Add("INSERT INTO c2c_fmdb.t_freeze_list(Fid,FFreezeID,FHandleFinish)VALUES ('100428', '12345673', '1');");
             new Tool().InsertORDeleteData(sqlList, "DataSource_ht");
         }

        //测试跑完后清除数据
         public void TearDown()
         {
             ArrayList sqlList = new ArrayList();
             //普通、微信解冻未处理状态批量结单查询数据清除
             sqlList.Add("delete from db_appeal_2015.t_tenpay_appeal_trans_04 where Fid='20150417001';");

             new Tool().InsertORDeleteData(sqlList, "fkdj");
         }
    }
}