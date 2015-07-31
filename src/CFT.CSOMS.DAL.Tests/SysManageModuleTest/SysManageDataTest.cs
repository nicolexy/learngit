﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CFT.CSOMS.DAL.Infrastructure;
using System.Collections;
using System.Data;
using CFT.CSOMS.DAL.SysManageModule;

namespace CFT.CSOMS.DAL.Tests
{
     [TestClass()]
    public class SysManageDataTest
    {
         [TestMethod]
         public void TestUpdateBankBulletin()
         {
             SysManageData sysManageData = new SysManageData();

             string businesstype = "1";
             int op_support_flag = 1;
             int banktype = 4501;
             string closetype = "2";
             string title = "为了测试自己新增公告";
             string maintext = "银行系统维护中，预计startime恢复。因银行系统维护，此期间操作的资金将延迟到XX月XX日返回结果。";
             string popuptext = "因银行系统维护，此期间操作的资金将延迟到XX月XX日返回结果。";
             string createuser = "echo";
             string updateuser = "echo";

             // 为空字符串模拟老接口
             string op_flag = "1";
             string bull_type = "1";
             string bulletin_id = System.DateTime.Now.ToString("yyyyMMddHHmmss") + PublicRes.NewStaticNoManage();
             string starttime = "2015-07-28 00:00:00";
             string endtime = "2015-07-30 23:59:59";
             bool index = sysManageData.UpdateBankBulletin(businesstype, op_support_flag,
                 banktype, bulletin_id, closetype, title, maintext, popuptext, createuser,
                 updateuser, op_flag, bull_type, starttime, endtime);
         }

    }
}