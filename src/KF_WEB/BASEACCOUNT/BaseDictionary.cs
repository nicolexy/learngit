using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
    public static class BaseDictionary
    {
        public static Dictionary<string, string> ModType
        {
            get
            {
                if (_modType.Count == 0)
                {
                   
                    _modType.Add("0", "修改银行信息(开户名不变)");
                    _modType.Add("1", "修改银行信息(开户名变更)");
                    _modType.Add("2", "修改结算规则");
                    _modType.Add("3", "修改合同信息");
                    _modType.Add("4", "修改商户名称或域名或邮箱信息");
                    _modType.Add("6", "修改商户权限位");
                    _modType.Add("7", "企业大额收款");
                    _modType.Add("8", "商户注销");
                    _modType.Add("9", "商户分帐规则");
                    _modType.Add("10", "T+0结算规则");
                    _modType.Add("11", "提现规则");
                    _modType.Add("12", "商户冻结");
                    _modType.Add("13", "商户恢复");
                    _modType.Add("14", "商户大额支付");
                    _modType.Add("15", "共享登录");
                    _modType.Add("18", "商户自助修改域名");
                    _modType.Add("19", "委托代扣");
                    _modType.Add("20", "T+1结算周期申请");
                    _modType.Add("21", "中小商户提交资质实名认证申请");
                    _modType.Add("22", "商户代扣申请");
                    _modType.Add("23", "修改商户属组");
                    _modType.Add("25", "商户开通中介功能");
                    _modType.Add("26", "快捷支付");
                    _modType.Add("27", "C账号证书个数修改");
                    _modType.Add("28", "预付金委托扣款");
                    _modType.Add("29", "收付易申请");
                    _modType.Add("30", "帐号对应表管理");
                    _modType.Add("31", "BD商户转自助商户");
                    _modType.Add("40", "商户加黑申请");
                    _modType.Add("32", "商户代付申请");
                    _modType.Add("47", "营转增申请");
                    _modType.Add("48", "修改商户资质信息");
                    _modType.Add("53", "商户联系信息修改申请");
                    _modType.Add("56", "Pos商户MCC审批");
                }
                return _modType;
            }
        }

        static Dictionary<string, string> _modType =new Dictionary<string, string>();

       
    }
}