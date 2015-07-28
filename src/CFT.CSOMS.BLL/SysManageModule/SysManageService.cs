using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using System.Data;

namespace CFT.CSOMS.BLL.SysManageModule
{
    using CFT.CSOMS.DAL.SysManageModule;
    using System.Collections;
    using CFT.CSOMS.DAL.Infrastructure;
    using CFT.CSOMS.BLL.PublicService;
    public class SysManageService
    {
        public static readonly Dictionary<string, string> BankInterfaceName = new Dictionary<string, string>();
        public static readonly Dictionary<string, string> BullType = new Dictionary<string, string>();
        public static readonly Dictionary<string, string> BullState = new Dictionary<string, string>();

        static SysManageService()
        {
            BankInterfaceName.Add("1", "提现银行接口");
            BankInterfaceName.Add("2", "向银行卡付款接口");
            BankInterfaceName.Add("3", "还房贷银行接口");
            BankInterfaceName.Add("4", "信用卡还款银行接口");
            BankInterfaceName.Add("5", "代扣银行接口");
            BankInterfaceName.Add("6", "银行收款接口");
            BankInterfaceName.Add("8", "退款接口");

            BullType.Add("1", "例行维护");
            BullType.Add("2", "自动维护");

            BullState.Add("1", "正常");
            BullState.Add("2", "作废");
        }

        //查询公告联系人所有分组
        public DataSet QueryAllContactsGroup(int limit, int offset)
        {
            return new SysManageData().QueryAllContactsGroup(limit, offset);
        }

        //添加公告联系人分组
        public void AddContactsGroup(string groupName, string createuser)
        {
            new SysManageData().AddContactsGroup(groupName, createuser);
        }

        //删除公告联系人分组
        public void DelContactsGroup(string id, string updateuser)
        {
            new SysManageData().DelContactsGroup(id, updateuser);
        }

        //查询某组公告联系人
        public DataSet QueryOneGroupContacts(string groupId, int limit, int offset)
        {
            return new SysManageData().QueryOneGroupContacts(groupId, limit, offset);
        }

        //添加公告联系人
        public void AddContacts(string groupId, string name, string email, string createuser)
        {
            new SysManageData().AddContacts(groupId, name, email, createuser);
        }

        //删除公告联系人
        public void DelContacts(string id, string updateuser)
        {
            new SysManageData().DelContacts(id, updateuser);
        }

        public DataSet QueryBankBulletin(string businesstype, int op_support_flag, int banktype, string bulletin_id,
            string bull_state, string bull_type, string starttime, string endtime, string current_datetime, int limit, int offset, out int total_num)
        {
           DataSet ds=new SysManageData().QueryBankBulletin(businesstype, op_support_flag, banktype, bulletin_id, bull_state,  bull_type,  starttime,  endtime, current_datetime, limit,  offset,out total_num);
           if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
           {
               ds.Tables[0].Columns.Add("banktype_str", typeof(System.String));
               ds.Tables[0].Columns.Add("sysid", typeof(String));
               ds.Tables[0].Columns.Add("closetype_str", typeof(String));
               ds.Tables[0].Columns.Add("op_support_flag_str", typeof(String));
               ds.Tables[0].Columns.Add("bull_type_str", typeof(String));
               ds.Tables[0].Columns.Add("bull_state_str", typeof(String));
               Hashtable ht1 = new Hashtable();
               ht1.Add("1", "硬关闭");
               ht1.Add("2", "软关闭");
               ht1.Add("3", "测试关闭");
               Hashtable ht2 = new Hashtable();
               ht2.Add("1", "全部关闭");
               ht2.Add("2", "快捷签约");
               ht2.Add("3", "快捷支付");
               ht2.Add("4", "退款");

               COMMLIB.CommUtil.DbtypeToPageContent(ds.Tables[0], "closetype", "closetype_str", ht1);
               if (businesstype == "6")
                   COMMLIB.CommUtil.DbtypeToPageContent(ds.Tables[0], "op_support_flag", "op_support_flag_str", ht2);
               COMMLIB.CommUtil.DbtypeToPageContent(ds.Tables[0], "bull_type", "bull_type_str", BullType);
               COMMLIB.CommUtil.DbtypeToPageContent(ds.Tables[0], "bull_state", "bull_state_str", BullState);
               foreach (DataRow dr in ds.Tables[0].Rows)
               {
                   dr.BeginEdit();
                   dr["sysid"] = businesstype;
                   dr["banktype_str"] = TransferMeaning.Transfer.convertbankType(dr["banktype"].ToString());
                   dr.EndEdit();
               }
           }
        return ds;
        }
       
        /// <summary>
        /// 将接口返回公告信息转换成公告类
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public commData.T_BANKBULLETIN_INFO TurnBankBulletinClass(DataSet ds)
        {
            try
            {
                DataTable dt = new DataTable();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    dt = ds.Tables[0];

                    commData.T_BANKBULLETIN_INFO um = new commData.T_BANKBULLETIN_INFO();
                    um.LoadFromDB(dt.Rows[0]);
                    return um;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw new Exception("公告信息转换成公告类处理失败！" + ex.Message);
            }
        }

        /// <summary>
        /// 查询银行接口信息ByObjid
        /// </summary>
        /// <param name="objid"></param>
        /// <param name="checkType"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public commData.T_BANKBULLETIN_INFO QueryBankBulletinByObjid(string objid, string checkType)
        {

            try
            {
                DataTable dt =new PublicService().GetCheckInfo(objid, checkType);
                if (dt == null)
                {
                    return null;
                }
                commData.T_BANKBULLETIN_INFO um = new commData.T_BANKBULLETIN_INFO();
                um.LoadFromParamDB(dt);
                return um;
            }
            catch (Exception ex)
            {
               throw new Exception(ex.Message);
            }

        }

    }
}
